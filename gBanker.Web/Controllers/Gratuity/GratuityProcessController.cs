using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using gHRM.Service;
using gHRM.Web.CommonDropdown;
using gHRM.Web.ViewModels;
using gHRM.Web.Helpers;
using System.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gHRM.Data.CodeFirstMigration;

namespace gHRM.Web.Controllers
{
    public class GratuityProcessController : BaseController
    {
        private readonly IGratuityConfigService _GratuityConfigService;
        //private readonly IEmployeeStatusService _EmployeeStatusService;
        private readonly IOfficeTypeService _OfficeTypeService;
        private readonly IOfficeService _OfficeService;
        //public CommonDynamicDropDown _CommonDynamicDropDown;
        public CommonStaticDropDown _CommonStaticDropDown;
        private string Message;

        public GratuityProcessController(
            IGratuityConfigService _GratuityConfigService,
            //IEmployeeStatusService _EmployeeStatusService,
            IOfficeTypeService _OfficeTypeService,
            IOfficeService _OfficeService
            )
        {
            this._GratuityConfigService = _GratuityConfigService;
            //this._EmployeeStatusService = _EmployeeStatusService;
            this._OfficeTypeService = _OfficeTypeService;
            this._OfficeService = _OfficeService;
            //_CommonDynamicDropDown = new CommonDynamicDropDown();
            _CommonStaticDropDown = new CommonStaticDropDown();
        }

        public ActionResult Index()
        {
            GratuityProcessViewModel model = new GratuityProcessViewModel();
            return LoadIndexPage(model);
        }

        public ActionResult Index2()
        {
            GratuityProcessViewModel model = new GratuityProcessViewModel();
            return LoadIndexPage2(model);
        }

        private ActionResult LoadIndexPage(GratuityProcessViewModel model)
        {
            MapDropDown(model);
            DateTime? GeneratedLastDate = _GratuityConfigService.GratuityGeneratedLastDate();
            model.EmployeeName = LoggedInEmployee.EmployeeName;
            ViewData["GeneratedLastDate"] = null == GeneratedLastDate ? "" : GeneratedLastDate.Value.ToString("dd-MMM-yyyy");
            ViewBag.LoggedInOfficeTypeId = SessionHelper.LoggedInOfficeTypeId;
            return View("Index", model);
        }

        private ActionResult LoadIndexPage2(GratuityProcessViewModel model)
        {
            MapDropDown(model);
            DateTime? GeneratedLastDate = _GratuityConfigService.GratuityGeneratedLastDate();
            model.EmployeeName = LoggedInEmployee.EmployeeName;
            ViewData["GeneratedLastDate"] = null == GeneratedLastDate ? "" : GeneratedLastDate.Value.ToString("dd-MMM-yyyy");
            ViewBag.LoggedInOfficeTypeId = SessionHelper.LoggedInOfficeTypeId;
            return View("Index2", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Index(GratuityProcessViewModel model)
        {
            string Message = "";
            long CreateUser = null == LoggedInEmployeeId ? 0 : LoggedInEmployeeId.Value;
            ViewBag.IsSuccess = _GratuityConfigService.GenerateGratuity(model.OfficeId, model.FromYear, model.FromMonth, CreateUser, Convert.ToDateTime(model.ProcessDate), out Message);
            ViewBag.Message = ViewBag.IsSuccess ? "Gratuity generated successfully !" : Message;
            return LoadIndexPage(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Index2(GratuityProcessViewModel model)
        {
            string Message = "";
            long CreateUser = null == LoggedInEmployeeId ? 0 : LoggedInEmployeeId.Value;
            ViewBag.IsSuccess = _GratuityConfigService.GenerateGratuity2(model.OfficeTypeId, model.OfficeId, model.FromYear, model.FromMonth, CreateUser, Convert.ToDateTime(model.ProcessDate), out Message);
            ViewBag.Message = ViewBag.IsSuccess ? "Gratuity generated successfully !" : Message;
            return LoadIndexPage2(model);
        }


        public ActionResult Approve()
        {
            GratuityApproveViewModel model = new GratuityApproveViewModel();
            model.MonthList = _CommonStaticDropDown.GetMonthListList();
            model.YearList = _CommonStaticDropDown.YearList(5, 0);
            return View(model);
        }

        [HttpPost]
        public ActionResult GratuitySummaryPreviewBeforeSendForApproval([DataSourceRequest] DataSourceRequest request, long OfficeId, int Year, int Month)
        {
            try
            {
                using (var DB = new gHRMDBContext())
                {
                    var GratuityData = (from G in DB.EmployeeGratuities
                                        join E in DB.Employees on G.EmployeeId equals E.EmployeeId
                                        where E.OfficeId == OfficeId
                                        && (G.SalaryDate.Year > Year || (G.SalaryDate.Year == Year && G.SalaryDate.Month >= Month))
                                        && G.IsActive && !G.IsSendForApproval && !G.IsApproved && !G.IsRejected
                                        select new
                                        {
                                            Name = E.EmployeeName,
                                            Code = E.EmployeeCode,
                                            SalaryDate = G.SalaryDate,
                                            BasicSalary = G.BasicSalary,
                                            SerMonth = G.SerMonth,
                                            CurGratuity = G.CurGratuity,
                                            CumGratuity = G.CumGratuity,
                                            GratuityTimes = G.GratuityTimes,
                                            EligibleFrom = G.EligibleFrom,
                                            JoinOrConfirmationDate = G.EligibleFrom == "J" ? E.FirstJoiningDate : E.ConfirmationDate
                                        }).ToList();
                    var EmpMonthlyGratuitys = GratuityData.Select(x => new {
                        Name = x.Name,
                        Code = x.Code,
                        SalaryDate = x.SalaryDate.ToString("dd-MMM-yyyy"),
                        BasicSalary = x.BasicSalary,
                        SerMonth = x.SerMonth,
                        CurGratuity = x.CurGratuity,
                        CumGratuity = x.CumGratuity,
                        GratuityTimes = x.GratuityTimes,
                        EligibleFrom = x.EligibleFrom,
                        JoinOrConfirmationDate = null == x.JoinOrConfirmationDate ? "" : x.JoinOrConfirmationDate.Value.ToString("dd-MMM-yyyy")
                    }).ToList();
                    DataSourceResult result = EmpMonthlyGratuitys.ToDataSourceResult(request);
                    return Json(new { data = result.Data, total = result.Total });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult GratuitySummaryPreviewBeforeApproval([DataSourceRequest] DataSourceRequest request, int Year, int Month)
        {
            try
            {
                using (var DB = new gHRMDBContext())
                {
                    var GratuityData = (from G in DB.EmployeeGratuities
                                        join E in DB.Employees on G.EmployeeId equals E.EmployeeId
                                        where (G.SalaryDate.Year > Year || (G.SalaryDate.Year == Year && G.SalaryDate.Month >= Month))
                                        && G.IsActive && G.IsSendForApproval && !G.IsApproved && !G.IsRejected
                                        select new
                                        {
                                            Name = E.EmployeeName,
                                            Code = E.EmployeeCode,
                                            SalaryDate = G.SalaryDate,
                                            BasicSalary = G.BasicSalary,
                                            SerMonth = G.SerMonth,
                                            CurGratuity = G.CurGratuity,
                                            CumGratuity = G.CumGratuity,
                                            GratuityTimes = G.GratuityTimes,
                                            EligibleFrom = G.EligibleFrom,
                                            JoinOrConfirmationDate = G.EligibleFrom == "J" ? E.FirstJoiningDate : E.ConfirmationDate
                                        }).ToList();
                    var EmpMonthlyGratuitys = GratuityData.Select(x => new {
                        Name = x.Name,
                        Code = x.Code,
                        SalaryDate = x.SalaryDate.ToString("dd-MMM-yyyy"),
                        BasicSalary = x.BasicSalary,
                        SerMonth = x.SerMonth,
                        CurGratuity = x.CurGratuity,
                        CumGratuity = x.CumGratuity,
                        GratuityTimes = x.GratuityTimes,
                        EligibleFrom = x.EligibleFrom,
                        JoinOrConfirmationDate = null == x.JoinOrConfirmationDate ? "" : x.JoinOrConfirmationDate.Value.ToString("dd-MMM-yyyy")
                    }).ToList();
                    DataSourceResult result = EmpMonthlyGratuitys.ToDataSourceResult(request);
                    return Json(new { data = result.Data, total = result.Total });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpPost]
        public ActionResult GratuitySummaryPreviewBeforeApproval2([DataSourceRequest] DataSourceRequest request, int Year, int Month, int OfficeId, int OfficeTypeId )
        {
            try
            {
                using (var DB = new gHRMDBContext())
                {
                    var GratuityData = (from G in DB.EmployeeGratuities
                                        join E in DB.Employees on G.EmployeeId equals E.EmployeeId
                                        join O in DB.Offices on E.OfficeId equals O.OfficeId
                                        join OT in DB.OfficeTypes on O.OfficeTypeId equals OT.OfficeTypeId
                                        where (G.SalaryDate.Year == Year && OT.OfficeTypeId == OfficeTypeId)
                                        && G.IsActive && !G.IsSendForApproval && !G.IsApproved && !G.IsRejected
                                        select new
                                        {
                                            Name = E.EmployeeName,
                                            Code = E.EmployeeCode,
                                            SalaryDate = G.SalaryDate,
                                            BasicSalary = G.BasicSalary,
                                            SerMonth = G.SerMonth,
                                            CurGratuity = G.CurGratuity,
                                            CumGratuity = G.CumGratuity,
                                            GratuityTimes = G.GratuityTimes,
                                            EligibleFrom = G.EligibleFrom,
                                            JoinOrConfirmationDate = G.EligibleFrom == "J" ? E.FirstJoiningDate : E.ConfirmationDate
                                        }).ToList();
                    var EmpMonthlyGratuitys = GratuityData.Select(x => new {
                        Name = x.Name,
                        Code = x.Code,
                        SalaryDate = x.SalaryDate.ToString("dd-MMM-yyyy"),
                        BasicSalary = x.BasicSalary,
                        SerMonth = x.SerMonth,
                        CurGratuity = x.CurGratuity,
                        CumGratuity = x.CumGratuity,
                        GratuityTimes = x.GratuityTimes,
                        EligibleFrom = x.EligibleFrom,
                        JoinOrConfirmationDate = null == x.JoinOrConfirmationDate ? "" : x.JoinOrConfirmationDate.Value.ToString("dd-MMM-yyyy")
                    }).ToList();
                    DataSourceResult result = EmpMonthlyGratuitys.ToDataSourceResult(request);
                    return Json(new { data = result.Data, total = result.Total });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpPost]
        public ActionResult SendGeneratedGratuityForApproval(long OfficeId, int Year, int Month)
        {
            try
            {
                string Message = "";
                if (!_GratuityConfigService.SendGeneratedGratuityForApproval(OfficeId, Year, Month, out Message))
                {
                    return Json(new BaseResponseModel { message = Message });
                }
                return Json(new BaseResponseModel { success = true });
            }
            catch (Exception ex)
            {
                return Json(new BaseResponseModel { message = ex.Message });
            }
        }


        [HttpPost]
        public ActionResult SendGeneratedGratuityForApproval2( long OfficeId, int Year, int Month ,long OfficeTypeId )
        {
            try
            {
                string Message = "";
                if (!_GratuityConfigService.SendGeneratedGratuityForApproval2(OfficeId, OfficeTypeId, Year, Month, out Message))
                {
                    return Json(new BaseResponseModel { message = Message });
                }
                return Json(new BaseResponseModel { success = true });
            }
            catch (Exception ex)
            {
                return Json(new BaseResponseModel { message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult ApproveGratuitySendForApproval(int Year, int Month, DateTime ApproveDate)
        {
            try
            {
                string Message = "";
                if (!_GratuityConfigService.ApproveGratuitySendForApproval(Year, Month, ApproveDate, LoggedInEmployeeId ?? 0, out Message))
                {
                    return Json(new BaseResponseModel { message = Message });
                }
                return Json(new BaseResponseModel { success = true });
            }
            catch (Exception ex)
            {
                return Json(new BaseResponseModel { message = ex.Message });
            }
        }



        [HttpPost]
        public ActionResult RejectGratuitySendForApproval(int Year, int Month)
        {
            try
            {
                string Message = "";
                if (!_GratuityConfigService.RejectGratuitySendForApproval(Year, Month, out Message))
                {
                    return Json(new BaseResponseModel { message = Message });
                }
                return Json(new BaseResponseModel { success = true });
            }
            catch (Exception ex)
            {
                return Json(new BaseResponseModel { message = ex.Message });
            }
        }

        private void MapDropDown(GratuityProcessViewModel model)
        {
            var officeType = _OfficeTypeService.GetAll().Where(w => w.IsActive == true);
            var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeTypeId.ToString(),
                Text = string.Format("{0}", x.OfficeTypeName)
            });
            var officeType_items = new List<SelectListItem>();
            officeType_items.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            officeType_items.AddRange(viewofficeType);
            model.OfficeTypeList = officeType_items;

            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            model.OfficeList = ofc_items;

            var ZoneList = _OfficeService.GetAll().Where(x => x.OfficeTypeId == 4 && x.IsActive == true);
            var viewZoneList = ZoneList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = x.OfficeName.ToString()
            });
            var zone_items = new List<SelectListItem>();
            zone_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            zone_items.AddRange(viewZoneList);
            model.ZoneList = zone_items;

            var area_items = new List<SelectListItem>();
            area_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            model.AreaList = area_items;

            var unit_items = new List<SelectListItem>();
            unit_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            model.UnitList = unit_items;

            model.MonthList = _CommonStaticDropDown.GetMonthListList();
            model.YearList = _CommonStaticDropDown.YearList(5, 0);
        }
    }
}