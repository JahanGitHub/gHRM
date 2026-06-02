using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Web.ViewModels;
using gHRM.Web.Helpers;
using gHRM.Service.StoreProcedure;
using gHRM.Web.DropDownService;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gHRM.Web.CommonDropdown;

namespace gHRM.Web.Controllers
{
    public class LeaveApproversAuthorityConfigController : BaseController
    {
        #region Variable
      
        private readonly IEmployeeSPService employeeSPService;
        private readonly ILeaveApproversAuthorityService leaveApproversAuthorityService;
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;

        public LeaveApproversAuthorityConfigController(
                IEmployeeSPService employeeSPService
              , ILeaveApproversAuthorityService leaveApproversAuthorityService
            )
        {
         
            this.employeeSPService = employeeSPService;
            this.leaveApproversAuthorityService = leaveApproversAuthorityService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion


        #region Events

        public ActionResult Index()
        {
            var model = new LeaveAdjustmentAuthorityViewModel();
            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();
            model.OfficeDesignationNameList = commonDynamicDropDown.GetAllOfficeDesignationList();
            model.EmployeeDepartmentList = commonDynamicDropDown.ddlInitial();
            model.EmployeeCodeList = commonDynamicDropDown.ddlInitial();
            return View(model);
        }


        #endregion


        #region HttpRequests


        [HttpPost]
        public ActionResult LeaveApproversAuthorityList([DataSourceRequest]DataSourceRequest request)
        {
            var empOffcDesigList = employeeSPService.GetDataWithoutParameter("leave.SP_GetLeaveApproversAuthority");

            //var approvers=leaveApproversAuthorityService.GetAll().ToList();

            var List_EmployeeViewModel = empOffcDesigList.Tables[0].AsEnumerable()
           .Select(row => new EmployeeViewModel
           {
               SlNo = row.Field<string>("rowSl"),
               Id = row.Field<int>("Id"),
               EmployeeId = row.Field<long>("EmployeeId"),
               EmployeeName = row.Field<string>("EmployeeName"),
               EmployeeCode = row.Field<string>("EmployeeCode"),
           }).ToList();

            DataSourceResult result = List_EmployeeViewModel.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteLeaveApproversAuthority(int Id)
        {
            int result = 0;
            var message = "";
            try
            {
                var model = leaveApproversAuthorityService.GetById(Id);
                model.IsActive = false;
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                leaveApproversAuthorityService.Update(model);
                result = 1;
                message = "Deleted Successfully";

                var param = new { ApproverId = model.EmployeeId };
                var employeeDataSet = employeeSPService.GetDataWithParameter(param, "leave.SP_LeaveApprovers_General_Delete");
            }
            catch (Exception)
            {
                message = "Delete Failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveLeaveApprovarsAuthority(LeaveAdjustmentAuthorityViewModel AdjustmentlAuthority)
        {
            var result = 0;
            var message = string.Empty;

            try
            {
                long empId = AdjustmentlAuthority.EmployeeId;

                var isEmpAlreadyInAuthority = leaveApproversAuthorityService.GetMany(x => x.IsActive == true).FirstOrDefault();
                //var isEmpAlreadyInAuthority = leaveApproversAuthorityService.GetMany(x => x.IsActive == true && x.EmployeeId == empId).FirstOrDefault();
                if (isEmpAlreadyInAuthority == null)
                {
                    var param = new { EmpId = empId };
                    var employeeDataSet = employeeSPService.GetDataWithParameter(param, "emp.SP_Get_Employee_ByEmployeeId");

                    if (employeeDataSet.Tables[0].Rows.Count > 0)
                    {
                        var employee = employeeDataSet.Tables[0].Rows[0];

                        var entity = new LeaveApproversAuthority();
                        entity.EmployeeId = AdjustmentlAuthority.EmployeeId;
                        entity.EmployeeCode = Convert.ToString(employee["EmployeeCode"]);
                        entity.IsActive = true;
                        entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        entity.CreateDate = DateTime.UtcNow;
                        entity.UpdateDate = DateTime.UtcNow;
                        leaveApproversAuthorityService.Create(entity);
                        result = 1;
                        message = "Save Successfull";

                        var approver_param = new { ApproverId = empId };
                        var res = employeeSPService.GetDataWithParameter(approver_param, "leave.SP_LeaveApprovers_General");
                        
                    }
                }
                else
                {
                    result = 0;
                    message = "Leave approvers authority already exists.";
                }
            }
            catch (Exception ex)
            {
                message = "Save Failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
