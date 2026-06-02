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
using Newtonsoft.Json;

namespace gHRM.Web.Controllers
{
    public class NoticePayProcessController : BaseController
    {
        private readonly INoticePayConfigService _NoticePayConfigService;
        private readonly IOfficeTypeService _OfficeTypeService;
        private readonly IOfficeService _OfficeService;
        public CommonStaticDropDown _CommonStaticDropDown;
        private string Message;

        public NoticePayProcessController(
            INoticePayConfigService _NoticePayConfigService,
            IOfficeTypeService _OfficeTypeService,
            IOfficeService _OfficeService
            )
        {
            this._NoticePayConfigService = _NoticePayConfigService;
            this._OfficeTypeService = _OfficeTypeService;
            this._OfficeService = _OfficeService;
            _CommonStaticDropDown = new CommonStaticDropDown();
        }

        public ActionResult Index()
        {
            NoticePayProcessViewModel model = new NoticePayProcessViewModel();
            return LoadIndexPage(model);
        }

        private ActionResult LoadIndexPage(NoticePayProcessViewModel model)
        {
            var OfficeList = _OfficeService.GetMany(x => x.IsActive == true).OrderBy(x => x.OfficeName)
                .Select(x => new { x.OfficeId, x.OfficeTypeId, x.OfficeName }).ToList();
            ViewBag.OfficeListJson = JsonConvert.SerializeObject(OfficeList);
            MapDropDown(model);
            model.EmployeeName = LoggedInEmployee.EmployeeName;
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Index(NoticePayProcessViewModel model)
        {
            string Message = "";
            long CreateUser = null == LoggedInEmployeeId ? 0 : LoggedInEmployeeId.Value;
            ViewBag.IsSuccess = _NoticePayConfigService.GenerateNoticePay(model.OfficeTypeId, model.OfficeId, model.FromYear, model.FromMonth, CreateUser, Convert.ToDateTime(model.ProcessDate), out Message);
            ViewBag.Message = ViewBag.IsSuccess ? "Notice Pay generated successfully !" : Message;
            return LoadIndexPage(model);
        }

        public ActionResult Approve()
        {
            NoticePayApproveViewModel model = new NoticePayApproveViewModel();
            model.MonthList = _CommonStaticDropDown.GetMonthListList();
            model.YearList = _CommonStaticDropDown.YearList(5, 0);
            return View(model);
        }

        [HttpPost]
        public ActionResult NoticePaySummaryPreviewBeforeSendForApproval([DataSourceRequest] DataSourceRequest request, long OfficeId, int Year, int Month)
        {
            try
            {
                using (var DB = new gHRMDBContext())
                {
                    var GratuityData = (from G in DB.EmployeeNoticePays
                                        join E in DB.Employees on G.EmployeeId equals E.EmployeeId
                                        where E.OfficeId == OfficeId
                                        && G.ResignDate.Year == Year && G.ResignDate.Month == Month
                                        && G.IsActive && !G.IsSendForApproval && !G.IsApproved && !G.IsRejected
                                        select new
                                        {
                                            Name = E.EmployeeName,
                                            Code = E.EmployeeCode,
                                            G.InformDate,
                                            G.ResignDate,
                                            G.NoticePeriod,
                                            G.NoticeGiven,
                                            G.SalaryAmount,
                                            SalaryType = G.IsCalcFromBasic ? "Basic" : "Gross",
                                            G.SalaryPer,
                                            G.Amount
                                        }).ToList();
                    var EmpMonthlyGratuitys = GratuityData.Select(x => new {
                        Name = x.Name,
                        Code = x.Code,
                        InformDate = x.InformDate.ToString("dd-MMM-yyyy"),
                        ResignDate = x.ResignDate.ToString("dd-MMM-yyyy"),
                        x.NoticePeriod,
                        x.NoticeGiven,
                        x.SalaryAmount,
                        x.SalaryType,
                        x.SalaryPer,
                        x.Amount
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
        public ActionResult NoticePaySummaryPreviewBeforeApproval([DataSourceRequest] DataSourceRequest request, int Year, int Month)
        {
            try
            {
                using (var DB = new gHRMDBContext())
                {
                    var GratuityData = (from G in DB.EmployeeNoticePays
                                        join E in DB.Employees on G.EmployeeId equals E.EmployeeId
                                        where G.ResignDate.Year == Year && G.ResignDate.Month == Month
                                        && G.IsActive && G.IsSendForApproval && !G.IsApproved && !G.IsRejected
                                        select new
                                        {
                                            Name = E.EmployeeName,
                                            Code = E.EmployeeCode,
                                            G.InformDate,
                                            G.ResignDate,
                                            G.NoticePeriod,
                                            G.NoticeGiven,
                                            G.SalaryAmount,
                                            SalaryType = G.IsCalcFromBasic ? "Basic" : "Gross",
                                            G.SalaryPer,
                                            G.Amount
                                        }).ToList();
                    var EmpMonthlyGratuitys = GratuityData.Select(x => new {
                        Name = x.Name,
                        Code = x.Code,
                        InformDate = x.InformDate.ToString("dd-MMM-yyyy"),
                        ResignDate = x.ResignDate.ToString("dd-MMM-yyyy"),
                        x.NoticePeriod,
                        x.NoticeGiven,
                        x.SalaryAmount,
                        x.SalaryType,
                        x.SalaryPer,
                        x.Amount
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
        public ActionResult SendGeneratedNoticePayForApproval(long OfficeId, int Year, int Month)
        {
            try
            {
                string Message = "";
                if (!_NoticePayConfigService.SendGeneratedNoticePayForApproval(OfficeId, Year, Month, out Message))
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
        public ActionResult ApproveNoticePaySendForApproval(int Year, int Month, DateTime ApproveDate)
        {
            try
            {
                string Message = "";
                if (!_NoticePayConfigService.ApproveNoticePaySendForApproval(Year, Month, ApproveDate, LoggedInEmployeeId ?? 0, out Message))
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
        public ActionResult RejectNoticePaySendForApproval(int Year, int Month)
        {
            try
            {
                string Message = "";
                if (!_NoticePayConfigService.RejectNoticePaySendForApproval(Year, Month, out Message))
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

        private void MapDropDown(NoticePayProcessViewModel model)
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