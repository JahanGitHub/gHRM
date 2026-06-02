using System.Data;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using gHRM.Web.ViewModels;
using gHRM.Web.ViewModels.Basic;
using gHRM.Core.Filters.Employee;
using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using System.Threading.Tasks;

namespace gHRM.Web.Controllers
{
    public class EmployeeProfileReportController : BaseController
    {
        private readonly IEmployeeSPService employeeSpService;
        private readonly IOfficeService officeService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IEmployeeStatusService employeeStatusService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IEmployeeDesignationService employeeDesignationService;
        private readonly IEmployementTypeService employementTypeService;
        private readonly IEmployeeDepartmentSectionService employeeDepartmentSectionService;
        private readonly IEmployeeService employeeService;
        private readonly IAspNetRoleService aspNetRoleService;
        private readonly IAspNetUserService aspNetUserService;
        
        public EmployeeProfileReportController(
            IEmployeeSPService employeeSpService,
            IOfficeService officeService,
            IOfficeTypeService officeTypeService,
            IEmployeeStatusService employeeStatusService,
            IEmployeeDepartmentService employeeDepartmentService,
            IEmployeeDesignationService employeeDesignationService,
            IEmployementTypeService employementTypeService,
            IEmployeeDepartmentSectionService employeeDepartmentSectionService,
            IEmployeeService employeeService, 
            IAspNetRoleService aspNetRoleService, 
            IAspNetUserService aspNetUserService)
        {
            this.employeeSpService = employeeSpService;
            this.officeService = officeService;
            this.officeTypeService = officeTypeService;
            this.employeeStatusService = employeeStatusService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.employeeDesignationService = employeeDesignationService;
            this.employementTypeService = employementTypeService;
            this.employeeDepartmentSectionService = employeeDepartmentSectionService;
            this.employeeService = employeeService;
            this.aspNetRoleService = aspNetRoleService;
            this.aspNetUserService = aspNetUserService;
        }

        #region Events
        public ActionResult Index()
        {
            var model = new EmployeeProfileReportViewModel();
            MapDropdownForReport(model);
            model.EmployeeId = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
            model.EmployeeCode = employeeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployeeID)).EmployeeCode;
           
            var superAdminRoleId = Convert.ToInt32(aspNetRoleService.Get(x => x.IsActive == true && x.Name == "Super Admin").Id);
            var loginRoleId = aspNetUserService.Get(r => r.EmployeeId == model.EmployeeId).RoleId;

            if (superAdminRoleId == loginRoleId)
            {
                ViewData["UserRole"] = "Super Admin";
            }
            string UserRoleName = aspNetRoleService.GetNameById(SessionHelper.LoggedInRoleId.ToString());
            string EMPLOYEE_EDIT_PAGE_EMPLOYEE_CODE_MODIFY_ALLOW_FOR_USER_ROLE = AppSetting.Get(AppSetting.EMPLOYEE_PERSONAL_REPORT_PAGE_EMPLOYEE_CODE_MODIFY_ALLOW_FOR_USER_ROLE, HttpContext);
            ViewBag.IsEmployeeCodeEditAllowed = !string.IsNullOrEmpty(UserRoleName) && UserRoleName == EMPLOYEE_EDIT_PAGE_EMPLOYEE_CODE_MODIFY_ALLOW_FOR_USER_ROLE;
            return View(model);
        }


        public ActionResult Index_Addin()
        {
            var model = new EmployeeProfileReportViewModel();
            MapDropdownForReport(model);
            model.EmployeeId = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
            model.EmployeeCode = employeeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployeeID)).EmployeeCode;

            var superAdminRoleId = Convert.ToInt32(aspNetRoleService.Get(x => x.IsActive == true && x.Name == "Super Admin").Id);
            var loginRoleId = aspNetUserService.Get(r => r.EmployeeId == model.EmployeeId).RoleId;

            if (superAdminRoleId == loginRoleId)
            {
                ViewData["UserRole"] = "Super Admin";
            }
            string UserRoleName = aspNetRoleService.GetNameById(SessionHelper.LoggedInRoleId.ToString());
            string EMPLOYEE_EDIT_PAGE_EMPLOYEE_CODE_MODIFY_ALLOW_FOR_USER_ROLE = AppSetting.Get(AppSetting.EMPLOYEE_PERSONAL_REPORT_PAGE_EMPLOYEE_CODE_MODIFY_ALLOW_FOR_USER_ROLE, HttpContext);
            ViewBag.IsEmployeeCodeEditAllowed = !string.IsNullOrEmpty(UserRoleName) && UserRoleName == EMPLOYEE_EDIT_PAGE_EMPLOYEE_CODE_MODIFY_ALLOW_FOR_USER_ROLE;
            OfficeWiseDropDownList();
            return View(model);
        }

        private void OfficeWiseDropDownList()
        {
            IEnumerable<SelectListItem> items = new SelectList("");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["OrganizerList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            var officelevel = officeTypeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            //ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            ViewData["OfficeLevel"] = offcdetail.OfficeLevel.ToString() ?? "0";  //SessionHelper.LoginUserOfficeLevel;
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeId;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeId;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeId;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeId;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeId;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeId;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeId;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeId;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeId;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeId;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeId;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeId;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeId;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeId;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeId;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeId;
            }
        }


        #region Ajax Calls For Office DropDown

        public JsonResult SelectOffice(int officeId)
        {
            var officeFullName = "";
            var OrgName = "";
            if (officeId > 0)
            {
                SessionHelper.LoginUserOfficeID = officeId;
                var office = officeService.GetById(SessionHelper.LoginUserOfficeID.Value);
                var entity = AutoMapper.Mapper.Map<Office, OfficeViewModel>(office);
                SessionHelper.LoggedInOfficeDetail = entity;
                try
                {
                    officeFullName = office.OfficeCode + ", " + office.OfficeName;
                    OrgName = SessionHelper.OrganizationName + "-" + officeFullName;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            var resultObj = new { OfficeName = officeFullName };
            return Json(resultObj, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetOfficeWiseAssetUserList(int officeID)
        {
            var empList = await employeeService.GetFixedAssetEmployeeByOffice(officeID);
            var viewEmpList = empList.Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.EmployeeCode, p.EmployeeName),
                Value = p.EmployeeId.ToString()
            });
            var employeeList = new List<SelectListItem>();
            employeeList.Add(new SelectListItem { Text = "--This Office Employee", Value = "0", Selected = true });
            employeeList.AddRange(viewEmpList);
            return Json(employeeList, JsonRequestBehavior.AllowGet);
        }
     

        public JsonResult GetHOList()
        {
            var First_Level = officeService.GetByOfficeOrgID(Convert.ToInt32(SessionHelper.LoginUserOfficeID), Convert.ToInt32(CompanyID));
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 1 && c.FirstLevel == First_Level.FirstLevel);
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetZoneList(string HO_val)
        {
            var ofcId = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 2 && c.FirstLevel == ofcId && c.CompanyId == Convert.ToInt32(CompanyID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAreaList(string HO_val, string zone_val)
        {
            var ho_code = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var zone_code = officeService.GetById(Convert.ToInt32(zone_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 3 && c.FirstLevel == ho_code && c.SecondLevel == zone_code && c.CompanyId == Convert.ToInt32(CompanyID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOfficeList(string HO_val, string zone_val, string area_val)
        {
            var ho_code = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var zone_code = officeService.GetById(Convert.ToInt32(zone_val)).OfficeCode;
            var area_Code = "";
            var office_items = new List<SelectListItem>();
            if (area_val == "0" || area_val == null)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                return Json(office_items, JsonRequestBehavior.AllowGet);
            }
            if (area_val != "0" || area_val != null)
            {
                area_Code = officeService.GetById(Convert.ToInt32(area_val == null ? "0" : area_val)).OfficeCode;
                var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 4 && c.FirstLevel == ho_code && c.SecondLevel == zone_code && c.ThirdLevel == area_Code && c.CompanyId == Convert.ToInt32(CompanyID));
                var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.OfficeId.ToString(),
                    Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
                });

                if (viewOffice.ToList().Count > 0)
                {
                    office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                }
                office_items.AddRange(viewOffice);
            }
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }

        #endregion

        //// report 1
        //public ActionResult OfficialInfoReport(string EmployeeCode)
        //{
        //    try
        //    {
        //        var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p=>p.EmployeeId).FirstOrDefault();
        //        var param = new { EmployeeId = EmployeeId };
        //        var sparam = new { EmployeeId = EmployeeId };
        //        var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
        //        var subReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeOfficialInfo");
        //        var subReportDb = new Dictionary<string, DataTable>();
        //        subReportDb.Add("EmployeeOfficialInfo", subReport.Tables[0]);
        //        var reportParam = new Dictionary<string, object>();
        //        ReportHelper.PrintWithSubReport("rpt_OfficialInfoReport.rpt", MainReport.Tables[0], reportParam, subReportDb);
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //// report 1.1
        //public ActionResult single_OfficialInfoReport(string EmployeeCode)
        //{
        //    try
        //    {
        //        var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
        //        var param = new { EmployeeId = EmployeeId };
        //        var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_GetEmployeeOfficialInfo");
        //        var reportParam = new Dictionary<string, object>();
        //        ReportHelper.PrintReport("single_OfficialInfoReport.rpt", MainReport.Tables[0], reportParam);
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        // report 1
        public ActionResult OfficialInfoReport(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeOfficialInfo");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_OfficialInfoReport", single_OfficialInfoReport.Tables[0]);
                 var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_OfficialInfoReport.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // report 2
        public ActionResult EmployeeSupervisorInfo(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeSupervisor");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_EmployeeSupervisorInfo", single_OfficialInfoReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_EmployeeSupervisorInfo.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // report 3
        public ActionResult EmployeeEducationInfoReport(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeEducationInfo");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_EmployeeEducationInfoReport", single_OfficialInfoReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_EmployeeEducationInfoReport.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GuarantorMoneyReportMousumi(string EmployeeCode)
        {
            try
            {

                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = "1,2,3,4,5,6,7,8,9,10,11" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = "0" });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "ToDate", Value = "0" });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = EmployeeCode.ToString() });

                PrintSSRSReport("/gHRMPlus_Reports/GurantorMoneyReport", paramValues.ToArray());


                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // employee education report for mousumi 
        public ActionResult EmployeeEducationInfoReportMousumi(string EmployeeCode)
        {
            try
            {
                //var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                //var param = new { EmployeeId = EmployeeId };
                //var sparam = new { EmployeeId = EmployeeId };
                //var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                //var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeEducationInfo");
                //var subReportDb = new Dictionary<string, DataTable>();
                //subReportDb.Add("single_EmployeeEducationInfoReport", single_OfficialInfoReport.Tables[0]);
                //var reportParam = new Dictionary<string, object>();
                //ReportHelper.PrintWithSubReport("Employee/Sub_EmployeeEducationInfoReportMousumi.rpt", MainReport.Tables[0], reportParam, subReportDb);



                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = "1,2,3,4,5,6,7,8,9,10,11" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = "0" });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "ToDate", Value = "0" });

                 paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = EmployeeCode.ToString() });

                PrintSSRSReport("/gHRMPlus_Reports/EmployeeEducationReportByCode", paramValues.ToArray());


                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // report 4
        public ActionResult EmployeeBasicInfoReport(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeBasicInfo");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_EmployeeBasicInfoReport", single_OfficialInfoReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_EmployeeBasicInfoReport.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // report 5
        public ActionResult EmployeeAddressInfo(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeAddressInfo");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_EmployeeAddressInfo", single_OfficialInfoReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_EmployeeAddressInfo.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // report 6
        public ActionResult EmployeeFamilyInfo(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeFamilyInfo");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_EmployeeFamilyInfo", single_OfficialInfoReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_EmployeeFamilyInfo.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // report 7
        public ActionResult EmployeeOtherQualification(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeOtherQualification");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_EmployeeOtherQualification", single_OfficialInfoReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_EmployeeOtherQualification.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // report 8
        public ActionResult EmergencyContactInfo(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeEmergencyContact");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_EmergencyContactInfo", single_OfficialInfoReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_EmergencyContactInfo.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // report 9
        public ActionResult EmployeeMedicalInfo(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeMedicalInfo");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_EmployeeMedicalInfo", single_OfficialInfoReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_EmployeeMedicalInfo.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // report 10
        public ActionResult EmployeeReferenceGuarantorInfo(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetGuarantorInformation");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_EmployeeReferenceGuarantorInfo", single_OfficialInfoReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_EmployeeReferenceGuarantorInfo.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // report 11
        public ActionResult EmployeeCertificateInfo(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeCertificateInfo");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_EmployeeCertificateInfo", single_OfficialInfoReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_EmployeeCertificateInfo.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // report 12
        public ActionResult EmployeeTrainingInfo(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeTrainingInfo");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_EmployeeTrainingInfo", single_OfficialInfoReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_EmployeeTrainingInfo.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // report 13
        public ActionResult EmployeeOfficeVisitInfo(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeOfficeVisitInfo");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_EmployeeOfficeVisitInfo", single_OfficialInfoReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_EmployeeOfficeVisitInfo.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // report 14
        public ActionResult RelationshipWithCurrentOrganizationEmployee(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeCurrOrgRelationship");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_RelationshipWithCurrentOrganizationEmployee", single_OfficialInfoReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_RelationshipWithCurrentOrganizationEmployee.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // report 15
        public ActionResult RelationshipWithInterOrganizationEmployee(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetRelationWithInterOrgRelation");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_RelationshipWithInterOrganizationEmployee", single_OfficialInfoReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_RelationshipWithInterOrganizationEmployee.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // report 16
        public ActionResult WorkExperienceWithInterOrganization(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetWorkExperienceWithInterOrganization");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_WorkExperienceWithInterOrganization", single_OfficialInfoReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_WorkExperienceWithInterOrganization.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // report 17
        public ActionResult PreviousWorkExperience(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetPreviousWorkExperience");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_PreviousWorkExperience", single_OfficialInfoReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_PreviousWorkExperience.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // report 18
        public ActionResult Publication(string EmployeeCode)
        {
            try
            {
                var EmployeeId = employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeBasicInfo_Header");
                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetPublicationInfo");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_Publication", single_OfficialInfoReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/Sub_Publication.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // report 22     

        public ActionResult EmployeeReferenceGuarantorInfoForMousumi(
                        string EmployeeCode, string OfficeTypeId, string OfficeId, string DesignationId, string ResponsibilityId, string DeptId, string SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = (string.IsNullOrEmpty(EmployeeCode) ? "0" : EmployeeCode) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (string.IsNullOrEmpty(OfficeTypeId) ? "0" : OfficeTypeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (string.IsNullOrEmpty(OfficeId) ? "0" : OfficeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = (string.IsNullOrEmpty(DesignationId) ? "0" : DesignationId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = "1, 2, 3, 4, 5, 6, 7, 8, 9, 10" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = (string.IsNullOrEmpty(DeptId) ? "0" : DeptId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = (string.IsNullOrEmpty(SectionId) ? "0" : SectionId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = (string.IsNullOrEmpty(ResponsibilityId) ? "0" : ResponsibilityId) });

                PrintSSRSReport("/gHRMPlus_Reports/EmployeeGurantorForMousumi", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }



        public ActionResult FullProfileReport(string EmployeeCode)
        {
            try
            {
                int EmployeeId = 0;
                var employeeInfo = employeeService.GetEmployeeByEmployeeCode(EmployeeCode);
                if (employeeInfo != null)
                    EmployeeId = Convert.ToInt32(employeeInfo.EmployeeId);

                   // employeeService.GetAll().Where(p => p.IsActive == true && p.EmployeeCode == EmployeeCode).Select(p => p.EmployeeId).FirstOrDefault();
                var param = new { EmployeeId = EmployeeId };
                var sparam = new { EmployeeId = EmployeeId };
                var filter = new EmployeeSearchFilter { EmployeeId= EmployeeId };
                var employeeProfileHeader = employeeSpService.GetEmployeeProfileHeaderByFilter(filter);
                var mainReport = employeeProfileHeader.ToDataTable();

                var single_OfficialInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeOfficialInfo");
                var single_EmployeeSupervisorInfo = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeSupervisor");
                var single_EmployeeEducationInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeEducationInfo");
                var single_EmployeeBasicInfoReport = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeBasicInfo");
                var single_EmployeeAddressInfo = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeAddressInfo");
                var single_EmployeeFamilyInfo = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeFamilyInfo");
                var single_EmployeeOtherQualification = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeOtherQualification");
                var single_EmergencyContactInfo = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeEmergencyContact");
                var single_EmployeeMedicalInfo = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeMedicalInfo");
                var single_EmployeeReferenceGuarantorInfo = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetGuarantorInformation");
                var single_EmployeeCertificateInfo = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeCertificateInfo");
                var single_EmployeeTrainingInfo = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeTrainingInfo");
                var single_EmployeeOfficeVisitInfo = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeOfficeVisitInfo");
                var single_RelationshipWithCurrentOrganizationEmployee = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetEmployeeCurrOrgRelationship");
                var single_RelationshipWithInterOrganizationEmployee = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetRelationWithInterOrgRelation");
                var single_WorkExperienceWithInterOrganization = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetWorkExperienceWithInterOrganization");
                var single_PreviousWorkExperience = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetPreviousWorkExperience");
                var single_Publication = employeeSpService.GetDataWithParameter(sparam, "emp.SP_RPT_PROFILE_GetPublicationInfo");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("single_OfficialInfoReport", single_OfficialInfoReport.Tables[0]);
                subReportDb.Add("single_EmployeeSupervisorInfo", single_EmployeeSupervisorInfo.Tables[0]);
                subReportDb.Add("single_EmployeeEducationInfoReport", single_EmployeeEducationInfoReport.Tables[0]);
                subReportDb.Add("single_EmployeeBasicInfoReport", single_EmployeeBasicInfoReport.Tables[0]);
                subReportDb.Add("single_EmployeeAddressInfo", single_EmployeeAddressInfo.Tables[0]);
                subReportDb.Add("single_EmployeeFamilyInfo", single_EmployeeFamilyInfo.Tables[0]);
                subReportDb.Add("single_EmployeeOtherQualification", single_EmployeeOtherQualification.Tables[0]);
                subReportDb.Add("single_EmergencyContactInfo", single_EmergencyContactInfo.Tables[0]);
                subReportDb.Add("single_EmployeeMedicalInfo", single_EmployeeMedicalInfo.Tables[0]);
                subReportDb.Add("single_EmployeeReferenceGuarantorInfo", single_EmployeeReferenceGuarantorInfo.Tables[0]);
                subReportDb.Add("single_EmployeeCertificateInfo", single_EmployeeCertificateInfo.Tables[0]);
                subReportDb.Add("single_EmployeeTrainingInfo", single_EmployeeTrainingInfo.Tables[0]);
                subReportDb.Add("single_EmployeeOfficeVisitInfo", single_EmployeeOfficeVisitInfo.Tables[0]);
                subReportDb.Add("single_RelationshipWithCurrentOrganizationEmployee", single_RelationshipWithCurrentOrganizationEmployee.Tables[0]);
                subReportDb.Add("single_RelationshipWithInterOrganizationEmployee", single_RelationshipWithInterOrganizationEmployee.Tables[0]);
                subReportDb.Add("single_WorkExperienceWithInterOrganization", single_WorkExperienceWithInterOrganization.Tables[0]);
                subReportDb.Add("single_PreviousWorkExperience", single_PreviousWorkExperience.Tables[0]);
                subReportDb.Add("single_Publication", single_Publication.Tables[0]);
                var reportParam = new Dictionary<string, object>();               

                ReportHelper.PrintWithSubReport("Employee/FullProfileReport.rpt", mainReport, reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        

        #endregion

        public JsonResult GetDepartmentWiseSection(int deptId)
        {
            var sectionList = employeeDepartmentSectionService.GetAll().Where(p => p.IsActive == true && p.DepartmentId == deptId).ToList();
            var viewSectionList = sectionList.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.SectionName,
                Value = p.SectionId.ToString()
            }).ToList();
            var secList = new List<SelectListItem>();
            secList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            secList.AddRange(viewSectionList);
            return Json(secList, JsonRequestBehavior.AllowGet);
        }



        public void MapDropdownForReport(EmployeeProfileReportViewModel model)
        {
            var employeeProfilelist = new List<SelectListItem>();
            employeeProfilelist.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Employee Official Info", Value = "1" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Set Employee Supervisor", Value = "2" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Employee Educational Info", Value = "3" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Employee Educational Info Mousumi", Value = "33" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Employee Basic Info", Value = "4" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Employee Address Info", Value = "5" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Employee Family Info", Value = "6" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Employee Other Experience", Value = "7" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Employee Emergency Contact Info", Value = "8" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Employee Medical Info", Value = "9" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Employee Reference/Guarantor Info", Value = "10" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Guarantor Money Report Mousumi ", Value = "44" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Employee Certificates Info", Value = "11" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Employee Training Info", Value = "12" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Employee Office Visit", Value = "13" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Relation With Current Organization Employee", Value = "14" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Relation With Inter Organization Employee", Value = "15" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Work Experience With Inter Organizations", Value = "16" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Previous Work Experience", Value = "17" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Publication", Value = "18" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Full Profile Report", Value = "21" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Employee Reference/Guarantor Info for Mousumi", Value = "22" });
            model.EmployeeProfileList = employeeProfilelist;            
        }

        public void MapDropdownForReportSignature(EmployeeProfileReportViewModel model)
        {
            var officeType = officeTypeService.GetMany(w => w.IsActive == true); ;
            var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeTypeId.ToString(),
                Text = string.Format("{0}", x.OfficeTypeName)
            });
            var officeType_items = new List<SelectListItem>();
            officeType_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            officeType_items.AddRange(viewofficeType);
            model.OfficeTypeList = officeType_items;

            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //ofc_items.AddRange(viewOfcList);
            model.OfficeList = ofc_items;

            var ZoneList = officeService.GetMany(x => x.OfficeTypeId == 4 && x.IsActive == true);
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
        }

        public JsonResult GetEmpInfoByCode(string employee_code)
        {
            var result = 0;
            try
            {
                var List_EmployeeViewModel = new List<EmployeeTransferViewModel>();
                var oldOfficeData = new List<EmployeeTransferViewModel>();
                var param = new { EmployeeCode = employee_code };
                var empList = employeeSpService.GetDataWithParameter(param, "cmm.SP_GetEmployeeInfo_ByEmployeeCode");

                if(empList.Tables[0].Rows.Count == 0)
                {
                    result = 0;
                    return Json(new { result = result, data = List_EmployeeViewModel.ToList() }, JsonRequestBehavior.AllowGet);
                }

                List_EmployeeViewModel = empList.Tables[0].AsEnumerable()
                    .Select(row => new EmployeeTransferViewModel
                    {
                        EmployeeId = row.Field<long>("EmployeeId"),
                        EmployeeName = row.Field<string>("EmployeeName"),
                        CurrentOfficeType = row.Field<string>("OfficeTypeName"),
                        EmployeeCurrentOfficeId = row.Field<int>("OfficeId"),
                        EmployeeCurrentOfficeName = row.Field<string>("OfficeName"),
                        EmployeeCurrentDepartmentName = row.Field<string>("DepartmentName"),
                        EmployeeCurrentDesignation = row.Field<string>("Responsibility"),
                    }).ToList();

                result = 1;
                return Json(new { result = result, data = List_EmployeeViewModel.ToList() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetEmpInfoByWithoutCode()
        {
            var result = 0;
            try
            {
                var List_EmployeeViewModel = new List<EmployeeTransferViewModel>();
                var oldOfficeData = new List<EmployeeTransferViewModel>();
               // var param = new { EmployeeCode = employee_code };
                var empList = employeeSpService.GetDataWithoutParameter("cmm.SP_GetEmployeeInfo_ByEmployeeCode");

                List_EmployeeViewModel = empList.Tables[0].AsEnumerable()
                    .Select(row => new EmployeeTransferViewModel
                    {
                        EmployeeId = row.Field<long>("EmployeeId"),
                        EmployeeName = row.Field<string>("EmployeeName"),
                        CurrentOfficeType = row.Field<string>("OfficeTypeName"),
                        EmployeeCurrentOfficeId = row.Field<int>("OfficeId"),
                        EmployeeCurrentOfficeName = row.Field<string>("OfficeName"),
                        EmployeeCurrentDepartmentName = row.Field<string>("DepartmentName"),
                        EmployeeCurrentDesignation = row.Field<string>("Responsibility"),
                    }).ToList();

                result = 1;
                return Json(new { result = result, data = List_EmployeeViewModel.ToList() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDepositMoney(string employee_code)
        {
            var result = 0;
            try
            {
                var List_EmployeeViewModel = new List<EmployeeGuarantorMoneyViewModel>();
                var oldOfficeData = new List<EmployeeGuarantorMoneyViewModel>();
                var param = new { EmployeeCode = employee_code };
                var empList = employeeSpService.GetDataWithParameter(param, "cmm.SP_GetGurantorMoney_ByEmployeeCode");

                List_EmployeeViewModel = empList.Tables[0].AsEnumerable()
                    .Select(row => new EmployeeGuarantorMoneyViewModel
                    {
                        EmployeeId = row.Field<long>("EmployeeId"),
                        EmployeeName = row.Field<string>("EmployeeName"),
                        CurrentOfficeType = row.Field<string>("OfficeTypeName"),
                        EmployeeCurrentOfficeId = row.Field<int>("OfficeId"),
                        EmployeeCurrentOfficeName = row.Field<string>("OfficeName"),
                        EmployeeCurrentDepartmentName = row.Field<string>("DepartmentName"),
                        EmployeeCurrentDesignation = row.Field<string>("Responsibility"),
                        deposit = row.Field<double>("GuaranteeMoney"),
                        balance = row.Field<decimal>("balance"),
                    }).ToList();

                result = 1;
                return Json(new { result = result, data = List_EmployeeViewModel.ToList() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            }
        }
        }


    }

