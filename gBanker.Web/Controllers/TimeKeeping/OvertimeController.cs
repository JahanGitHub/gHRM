using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Web.CommonDropdown;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace gHRM.Web.Controllers
{
    public class OvertimeController : BaseController
    {
        private readonly IManualOvertimeConfigurationService _ManualOvertimeConfigurationService;
        public CommonDynamicDropDown _CommonDynamicDropDown;

        public OvertimeController(IManualOvertimeConfigurationService _ManualOvertimeConfigurationService)
        {
            this._ManualOvertimeConfigurationService = _ManualOvertimeConfigurationService;
            _CommonDynamicDropDown = new CommonDynamicDropDown();
        }

        public ActionResult ManualConfig()
        {
            ViewBag.PayrollDesignationList = _CommonDynamicDropDown.GetAllPayrollDesignationList();
            return View();
        }

        public ActionResult ManualConfigCreate()
        {
            ViewBag.PayrollDesignationList = _CommonDynamicDropDown.GetAllPayrollDesignationList();
            return View();
        }

        public JsonResult LoadManualConfigList([DataSourceRequest] DataSourceRequest request, int EmployeeDesignationId)
        {
            try
            {
                using (var DB = new gHRMDBContext())
                {
                    var DataList = (from MOC in DB.ManualOvertimeConfigurations
                                    join ED in DB.EmployeeDesignations on MOC.EmployeeDesignationId equals ED.DesignationId into c_cd_ED
                                    from ED in c_cd_ED.DefaultIfEmpty()
                                    join E in DB.Employees on MOC.EmployeeId equals E.EmployeeId into c_cd_E
                                    from E in c_cd_E.DefaultIfEmpty()
                                    where MOC.IsActive
                                    && (0 == EmployeeDesignationId || (null != MOC.EmployeeDesignationId && MOC.EmployeeDesignationId == EmployeeDesignationId))
                                    orderby MOC.EffectiveStartDate descending
                                    select new
                                    {
                                        Id = MOC.Id,
                                        Type = null == MOC.EmployeeDesignationId ? "Employee Code" : "Payroll Designation",
                                        Desc = null == MOC.EmployeeDesignationId ? E.EmployeeCode : ED.DesignationName,
                                        MOC.WorkingDayMax,
                                        MOC.HolidayMax,
                                        MOC.MonthlyMax,
                                        ManualOvertimeOnly = MOC.ManualOvertimeOnly ? "Yes" : "No",
                                        MOC.EffectiveStartDate,
                                        MOC.EffectiveEndDate
                                    }).Distinct().ToList();
                    var _List = DataList.Select(x => new {
                        x.Id, x.Type, x.Desc, x.WorkingDayMax, x.HolidayMax, x.MonthlyMax, x.ManualOvertimeOnly,
                        EffectiveStartDate = x.EffectiveStartDate.ToString("dd-MMM-yyyy"),
                        EffectiveEndDate = null == x.EffectiveEndDate ? "" : x.EffectiveEndDate.Value.ToString("dd-MMM-yyyy")
                    }).ToList();
                    DataSourceResult result = _List.ToDataSourceResult(request);
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
        public JsonResult ManualConfigSave()
        {
            try
            {
                string Message = "";
                ManualOvertimeConfiguration OConfig = new JavaScriptSerializer().Deserialize<ManualOvertimeConfiguration>(Request.Form["Data"].ToString());
                if (0 == OConfig.EmployeeDesignationId) OConfig.EmployeeDesignationId = null;
                if (0 == OConfig.EmployeeId) OConfig.EmployeeId = null;
                if (!_ManualOvertimeConfigurationService.IsManualConfigSaveValid(OConfig, out Message)) return GetErrorMessageResult(Message);
                _ManualOvertimeConfigurationService.DisablePreviousConfig(OConfig);
                OConfig.IsActive = true;
                OConfig.CreateDate = DateTime.Now;
                OConfig.CreateUser = LoggedInEmployeeId ?? 0;
                _ManualOvertimeConfigurationService.Create(OConfig);
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public JsonResult ManualConfigDelete()
        {
            try
            {
                long Id = Convert.ToInt32(Request.Form["Data"]);
                _ManualOvertimeConfigurationService.DeleteConfiguration(Id);
                _ManualOvertimeConfigurationService.Save();
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
    }
}