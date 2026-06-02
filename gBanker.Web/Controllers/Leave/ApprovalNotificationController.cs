using gHRM.Service;
using gHRM.Service.StoreProcedure;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers
{
    public class ApprovalNotificationController : BaseController
    {
        #region Variable
        public readonly  IApprovalNotificationService approvalNotificationService;
        public readonly IEmployeeService employeeService;
        public readonly IApprovalConfigMasterService approvalConfigMasterService;
        public readonly IApprovalConfigDetailService approvalConfigDetailService;

        private readonly IEmployeeSPService employeeSPService;
        public ApprovalNotificationController(IApprovalNotificationService approvalNotificationService, IEmployeeService employeeService, IApprovalConfigMasterService approvalConfigMasterService, IApprovalConfigDetailService approvalConfigDetailService
            , IEmployeeSPService employeeSPService)
        {
            this.approvalNotificationService = approvalNotificationService;
            this.employeeService = employeeService;
            this.approvalConfigDetailService = approvalConfigDetailService;
            this.approvalConfigMasterService = approvalConfigMasterService;

            this.employeeSPService = employeeSPService;
        }
        #endregion

        #region Method
        public JsonResult getTotalLeaveNotification()
        {
            int result = 0; string message = ""; object data = null;
            try
            {
                var empId = Convert.ToInt64(LoggedInEmployeeId);
                var empInfo = employeeService.GetByEmpId(empId);
                var NotificationInfo =  approvalNotificationService.GetAll().Where(b=>b.IsActive==true &&  (b.IsChecked==null || b.IsChecked == false) );
                if(NotificationInfo.Count()>0)
                {
                
                 var leaveParm = new { ModuleName = "LM", ApproveOfficeId = empInfo.OfficeId, ApproveDepartmentId = empInfo.DepartmentId, ApproveDesignationId = empInfo.OfficeDesignationId ,ApprovalEmployeeId=empInfo.EmployeeId};
                 var _TotalNotification = employeeSPService.GetDataWithParameter(leaveParm, "leave.SP_GetTotalLeaveNotification");
                 var NotificationNumber = _TotalNotification.Tables[0].AsEnumerable().Select(b=> new{
                 TotalApplication = b.Field<int>("TotalApplication")
                 }).FirstOrDefault();
                    result =1;
                    data = NotificationNumber.TotalApplication;
                }

                else{
                    result = 1;
                    data =0;

                }
               
            }
            catch (Exception e)
            {
                result = 0;
                message = e.Message;

            }
            return Json(new { result = result, message = message, data = data }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region event


        public ActionResult Index()
        {
            return View();
        }

        #endregion
    }
}