#region Usings

using AutoMapper;
using gHRM.Core.Utilities;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.DBDetailModels.Offices;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

#endregion

namespace gHRM.Web.Controllers
{
    public class GratuityConfigController : BaseController
    {
        private readonly IGratuityConfigService _GratuityConfigService;
        private readonly IEmployeeStatusService _EmployeeStatusService;
        private readonly IOfficeTypeService _OfficeTypeService;
        private readonly IOfficeService _OfficeService;
        public CommonDynamicDropDown _CommonDynamicDropDown;
        public CommonStaticDropDown _CommonStaticDropDown;
        private string Message;
        private readonly IEmployeeSPService _EmployeeSPService;

        public GratuityConfigController(IGratuityConfigService _GratuityConfigService,
            IEmployeeStatusService _EmployeeStatusService,
            IOfficeTypeService _OfficeTypeService,
            IOfficeService _OfficeService,
            IEmployeeSPService _EmployeeSPService
            )
        {
            this._GratuityConfigService = _GratuityConfigService;
            this._EmployeeStatusService = _EmployeeStatusService;
            this._OfficeTypeService = _OfficeTypeService;
            this._OfficeService = _OfficeService;
            this._EmployeeSPService = _EmployeeSPService;
            _CommonDynamicDropDown = new CommonDynamicDropDown();
            _CommonStaticDropDown = new CommonStaticDropDown();
        }

        public ActionResult Index()
        {
            var EmployeeStatusList = _CommonDynamicDropDown.ddlEmployeeStatusList(true);
            EmployeeStatusList.RemoveAll(x => x.Value == "");
            ViewBag.EmployeeStatusList = EmployeeStatusList;
            return View();
        }

        public ActionResult Create()
        {
            var EmployeeStatusList = _CommonDynamicDropDown.ddlEmployeeStatusList(true);
            EmployeeStatusList.RemoveAll(x => x.Value == "");
            ViewBag.EmployeeStatusList = EmployeeStatusList;
            return View();
        }

        public ActionResult Process()
        {
            GratuityProcessViewModel model = new GratuityProcessViewModel();
            MapProcessDropDown(model);
            model.EmployeeName = LoggedInEmployee.EmployeeName;
            ViewBag.LoggedInOfficeTypeId = SessionHelper.LoggedInOfficeTypeId;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public JsonResult Save()
        {
            try
            {
                Message = "";
                GratuityGlobalConfig Config = new JavaScriptSerializer().Deserialize<GratuityGlobalConfig>(Request.Form["Data"].ToString());
                var param = new
                {
                    ServiceAgeFrom =  Config.ServiceAgeFrom, 
                    ServiceAgeTo = Config.ServiceAgeTo,
                    EmployeeStatusId = Config.EmployeeStatusId
                };

                var validity = _EmployeeSPService.GetDataWithParameter(param, "gr.ConfigValidation");

                if (validity.Tables[0].Rows.Count == 0)
                {
                    if (!_GratuityConfigService.AddGConfig(Config, LoggedInEmployeeId ?? 0, out Message)) return GetErrorMessageResult(Message);
                    return GetSuccessMessageResult();
                }
                else
                {
                    // Handle validation failure
                    return GetErrorMessageResult("Age From and Age To Already Exists in Configuration ");
                }


            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        public JsonResult LoadGratuityConfigList([DataSourceRequest] DataSourceRequest request, int EmployeeStatusId)
        {
            try
            {
                using (var DB = new gHRMDBContext())
                {
                    var _List = (from GC in DB.GratuityGlobalConfigs
                                          join ES in DB.EmployeeStatus on GC.EmployeeStatusId equals ES.StatusId
                                          where GC.IsActive && (0 == EmployeeStatusId || GC.EmployeeStatusId == EmployeeStatusId)
                                          orderby ES.StatusName
                                          select new
                                          {
                                              Id = GC.GratuityGlobalConfigId,
                                              StatusName = ES.StatusName,
                                              GC.ServiceAgeFrom,
                                              GC.ServiceAgeTo,
                                              GC.GratuityTimes,
                                              GC.EffectiveStartDate,
                                              GC.EffectiveEndDate,
                                              GC.EligibleFrom
                                          }).OrderBy(x => x.StatusName).ThenBy(x => x.ServiceAgeFrom)
                                          .ThenByDescending(x => x.EffectiveStartDate).ToList();
                    var DataList = _List.Select(x => new
                    {
                        Id = x.Id,
                        StatusName = x.StatusName,
                        x.ServiceAgeFrom,
                        x.ServiceAgeTo,
                        x.GratuityTimes,
                        EffectiveStartDate = x.EffectiveStartDate.ToString("dd-MMM-yyyy"),
                        EffectiveEndDate = null == x.EffectiveEndDate ? "" : x.EffectiveEndDate.Value.ToString("dd-MMM-yyyy"),
                        x.EligibleFrom
                    });
                    DataSourceResult result = DataList.ToDataSourceResult(request);
                    return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public JsonResult Delete()
        {
            try
            {
                Message = "";
                int Id = Convert.ToInt32(Request.Form["Id"]);
                if (!_GratuityConfigService.DeleteConfig(Id, out Message)) return GetErrorMessageResult(Message);
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        private bool FoundAgeConflict(int AgeFrom, int AgeTo, int OtherAgeFrom, int OtherAgeTo)
        {
            return AgeFrom <= OtherAgeTo && AgeTo >= OtherAgeFrom;
        }

        public void MapProcessDropDown(GratuityProcessViewModel model)
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

            ViewData["Months"] = _CommonStaticDropDown.GetMonthListList();
            ViewData["Years"] = _CommonStaticDropDown.YearList(5, 0);
        }
    }
}