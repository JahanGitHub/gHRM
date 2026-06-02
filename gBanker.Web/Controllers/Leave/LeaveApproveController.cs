using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Transactions;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gHRM.Web.ViewModels;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Data.CodeFirstMigration;
using gHRM.Web.EmailSenderService;
using System.Text;
using gHRM.Web.Helpers;

namespace gHRM.Web.Controllers
{
    public class LeaveApproveController : BaseController
    {
        #region Variables
        private readonly IEmployeeSPService employeeSPService;
        private readonly ILeaveHistoryService leaveHistoryService;
        private readonly IApprovalNotificationService approvalNotificationService;
        private readonly ILeaveApproversService leaveApproversService;
        private readonly EmailSender2 emailSenderService;


        public LeaveApproveController(
              IEmployeeSPService employeeSPService
            , ILeaveHistoryService leaveHistoryService
            , IApprovalNotificationService approvalNotificationService
            , ILeaveApproversService leaveApproversService
            )
        {
            this.employeeSPService = employeeSPService;
            this.leaveHistoryService = leaveHistoryService;
            this.approvalNotificationService = approvalNotificationService;
            this.leaveApproversService = leaveApproversService;
            emailSenderService = new EmailSender2();
        }

        #endregion


        #region Events

        // GET: LeaveApproved
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Index2()
        {
            return View();
        }


        public ActionResult LeaveUnSent()
        {
            return View();
        }

        public ActionResult LeaveApprovalPending()
        {
            return View();
        }

        #endregion

        #region HttpRequests

        public ActionResult GetLeaveForApproveList([DataSourceRequest]DataSourceRequest request, string Qtype)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (!String.IsNullOrEmpty(Qtype) && Qtype == "N")
                {
                    sb.Append(" AND an.IsChecked = 0 and an.CheckedStatus IS NULL AND  lh.IsActive = 1 AND lh.IsApproved = 0 AND lh.IsAdjustment = 0 ORDER BY lh.LeaveId desc");
                }

                if (!String.IsNullOrEmpty(Qtype) && Qtype == "A")
                { 
                    sb.Append(" AND an.IsChecked = 1 and an.CheckedStatus='A' AND lh.IsActive = 1 AND lh.IsApproved = 1 ORDER BY lh.LeaveId desc");
                }

                if (!String.IsNullOrEmpty(Qtype) && Qtype == "R")
                {
                    sb.Append(" AND an.IsChecked = 1 and an.CheckedStatus='R' AND lh.IsActive = 0 AND lh.IsApproved = 0 AND lh.IsAdjustment = 1 ORDER BY lh.LeaveId desc");
                }

                var param = new { EmployeeId = LoggedInEmployeeId, AndCondition = sb.ToString() };
                var applicationList = employeeSPService.GetDataWithParameter(param, "leave.SP_GetLeaveForNotification");


                var List_ViewModel = applicationList.Tables[0].AsEnumerable()
                .Select(row => new LeaveHistoryViewModel()
                {
                    Rowsl = row.Field<string>("rowSl"),
                    LeaveId = row.Field<long>("LeaveId"),
                    NotificationId = row.Field<long>("NotificationId"),
                    ApprovalMasterId = row.Field<int>("ApprovalMasterId"),
                    ApprovalDetailId = row.Field<int>("ApprovalDetailId"),

                    EmployeeName = row.Field<string>("EmployeeName"),
                    EmployeeId = Convert.ToInt32(row.Field<long>("EmployeeId")),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    OfficeName = row.Field<string>("OfficeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("DesignationName"),
                     SignatureName=row.Field<string>("SignatureName")==null? row.Field<string>("DesignationName") : row.Field<string>("SignatureName"),
                     LeaveReason = row.Field<string>("LeaveReason"),
                     LeaveTypeName = row.Field<string>("LeaveTypeName"),
                     LeaveStartDateMsg = row.Field<string>("LeaveStartDate"),
                      
                    LeaveEndDateMsg = row.Field<string>("LeaveEndDate"),
                      //TotalDays = row.Field<int?>("TotalDays"),
                      TotalDays = row.Field<decimal?>("TotalDays"),
                      MaxLeaveDays=row.Field<int>("Balance"),
                      AddressDuringLeave = row.Field<string>("AddressDuringLeave"),
                     IsApproved = row.Field<bool>("IsApproved"),
                    IsAdjustment = row.Field<bool>("IsAdjustment"),
                    Remarks = row.Field<string>("Remarks"),
                    ReplacementEmployee = row.Field<long>("ReplacementEmployee"),
                    ReplacementEmployeeName = row.Field<string>("ReplacementEmployeeName"),
                    PreviousApprover=row.Field<string>("PreviousApprover")

                }).ToList();

                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
                       }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult GetLeaveForApproveList_NotSent([DataSourceRequest] DataSourceRequest request, string Qtype)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (!String.IsNullOrEmpty(Qtype) && Qtype == "N")
                {
                    sb.Append(" AND an.IsChecked = 0 and an.CheckedStatus IS NULL AND an.ApproverId = 0   AND  lh.IsActive = 1 AND lh.IsApproved = 0 AND lh.IsAdjustment = 0  ORDER BY lh.LeaveId desc");
                }

                if (!String.IsNullOrEmpty(Qtype) && Qtype == "A")
                {
                    sb.Append("AND an.ApproverId <> 0   AND lh.IsActive = 1    AND lh.IsApproved = 0  ORDER BY lh.LeaveId desc");
                }

                if (!String.IsNullOrEmpty(Qtype) && Qtype == "R")
                {
                    sb.Append(" AND an.IsChecked = 1 and an.CheckedStatus='R' AND lh.IsActive = 0 AND lh.IsApproved = 0 AND lh.IsAdjustment = 1 ORDER BY lh.LeaveId desc");
                }

                var param = new { EmployeeId = LoggedInEmployeeId, AndCondition = sb.ToString() };
                var applicationList = employeeSPService.GetDataWithParameter(param, "leave.SP_GetLeaveForNotification_NotSent");


                var List_ViewModel = applicationList.Tables[0].AsEnumerable()
                .Select(row => new LeaveHistoryViewModel()
                {
                    Rowsl = row.Field<string>("rowSl"),
                    LeaveId = row.Field<long>("LeaveId"),
                    NotificationId = row.Field<long>("NotificationId"),
                    ApprovalMasterId = row.Field<int>("ApprovalMasterId"),
                    ApprovalDetailId = row.Field<int>("ApprovalDetailId"),

                    EmployeeName = row.Field<string>("EmployeeName"),
                    EmployeeId = Convert.ToInt32(row.Field<long>("EmployeeId")),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    OfficeName = row.Field<string>("OfficeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("DesignationName"),
                    SignatureName = row.Field<string>("SignatureName") == null ? row.Field<string>("DesignationName") : row.Field<string>("SignatureName"),
                    LeaveReason = row.Field<string>("LeaveReason"),
                    LeaveTypeName = row.Field<string>("LeaveTypeName"),
                    LeaveStartDateMsg = row.Field<string>("LeaveStartDate"),

                    LeaveEndDateMsg = row.Field<string>("LeaveEndDate"),
                    //TotalDays = row.Field<int?>("TotalDays"),
                    TotalDays = row.Field<decimal?>("TotalDays"),
                    MaxLeaveDays = row.Field<int>("Balance"),
                    AddressDuringLeave = row.Field<string>("AddressDuringLeave"),
                    IsApproved = row.Field<bool>("IsApproved"),
                    IsAdjustment = row.Field<bool>("IsAdjustment"),
                    Remarks = row.Field<string>("Remarks"),
                    ReplacementEmployee = row.Field<long>("ReplacementEmployee"),
                    ReplacementEmployeeName = row.Field<string>("ReplacementEmployeeName"),
                    PreviousApprover = row.Field<string>("PreviousApprover")

                }).ToList();

                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult leaveConfirm(string LeaveId, int NotificationId, int ApprovalMasterId, int ApprovalDetailId, int ApplicationId)
        {
            var result = 0;
            long approverId = 0;
            long employeeId = 0;
            string message = string.Empty;
            string LeaveStartDate = string.Empty;
            string LeaveEndDate = string.Empty;
            bool isNextApproverExist = true;

            if (NotificationId <1 || String.IsNullOrEmpty(LeaveId))
            {
                 message = "LeaveId or NotificationId is null or empty";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var entity = approvalNotificationService.GetById(NotificationId);
                    var applicationInfo = leaveHistoryService.GetById(Convert.ToInt32(entity.ApplicationId));

                    approverId = entity.ApproverId;
                    employeeId = applicationInfo.EmployeeId;

                    LeaveStartDate = applicationInfo.LeaveStartDate.ToString("MM/dd/yyyy");
                    LeaveEndDate = applicationInfo.LeaveEndDate.ToString("MM/dd/yyyy");



                    var param = new { LeaveStartDate = LeaveStartDate , LeaveEndDate = LeaveEndDate  , EmployeeId = employeeId };
                    var approversListDATA = employeeSPService.GetDataWithParameter(param, "leave.ApproversFromToLevel");


                    var approversList = approversListDATA.Tables[0].AsEnumerable()
                    .Select(row => new LeaveApprovers()
                    {
                        ID = row.Field<int>("ID"),
                        EmployeeId = row.Field<long>("EmployeeId"),
                        EmployeeOfficeId = row.Field<int>("EmployeeOfficeId"),
                        EmployeeDepartmentId = row.Field<int>("EmployeeDepartmentId"),
                        EmployeeDesignationId = row.Field<int>("EmployeeDesignationId"),

                        ApprovalLevel = row.Field<int>("ApprovalLevel"),
                        ApproverEmpId = row.Field<long>("ApproverEmpId"),
                        ApproveOfficeId = row.Field<int>("ApproveOfficeId"),
                        ApproveDepartmentId = row.Field<int>("ApproveDepartmentId"),
                        ApproveDesignationId = row.Field<int>("ApproveDesignationId"),
                        ManualUpdated = row.Field<bool>("ManualUpdated"),
                        IsActive = row.Field<bool>("IsActive"),
                        //CreateDate = row.Field<DateTime>("CreateDate"),
                        //UpdateUser = row.Field<long>("UpdateUser"),
                        //UpdateDate = row.Field<DateTime>("UpdateDate"),
                        FromDay = row.Field<int?>("FromDay"),
                        ToDay = row.Field<int?>("ToDay"),                       
                    }).ToList();



                    //final approval level 
                    var finalApprovalLevel = approversList.Max(l => l.ApprovalLevel);

                    var currentLevel = approversList.Where(p => p.ApproverEmpId == approverId).FirstOrDefault().ApprovalLevel;

                    if (finalApprovalLevel == currentLevel)
                    {
                        entity.IsChecked = true;
                        entity.CheckedStatus = "A";
                        entity.CheckedDate = DateTime.UtcNow;
                        entity.UpdateUser = LoggedInEmployeeId;
                        entity.UpdateDate = DateTime.UtcNow;
                        approvalNotificationService.Update(entity);

                        var model = leaveHistoryService.GetById(Convert.ToInt32(LeaveId));
                        model.IsApproved = true;
                        model.ApprovedBy = Convert.ToInt32(LoggedInEmployeeId);
                        model.ApprovedDate = DateTime.Now;
                        leaveHistoryService.Update(model);

                        result = 1;
                        message = "Application Approved Successfully";

                        scope.Complete();
                        isNextApproverExist = false;

                        // Send Mail to the employee

                    }
                    else
                    {
                        var nextApprovalLevel = approversList.Where(x => x.ApproverEmpId > 0 && x.ApprovalLevel > currentLevel).ToList().OrderBy(p => p.ApprovalLevel).ThenBy(p => p.ToDay).FirstOrDefault();

                        if (nextApprovalLevel != null)
                        {
                            entity.IsChecked = true;
                            entity.CheckedStatus = "A";
                            entity.CheckedDate = DateTime.UtcNow;
                            entity.UpdateUser = LoggedInEmployeeId;
                            entity.UpdateDate = DateTime.UtcNow;
                            approvalNotificationService.Update(entity);

                            ApprovalNotification oApprovalNotification = new ApprovalNotification();
                            approverId = nextApprovalLevel.ApproverEmpId;
                            oApprovalNotification.ApplicationId = Convert.ToInt64(LeaveId);
                            oApprovalNotification.ApprovalDetailId = 0;
                            oApprovalNotification.ApprovalMasterId = 0;
                            oApprovalNotification.ApproverId = Convert.ToInt64(approverId);
                            oApprovalNotification.IsActive = true;
                            oApprovalNotification.ModuleName = "LM";
                            oApprovalNotification.CreateDate = DateTime.Now;
                            oApprovalNotification.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                            approvalNotificationService.Create(oApprovalNotification);

                            result = 1;
                            message = "Application Approved Successfully";
                            scope.Complete();

                        }
                    }
                }


                if (SessionHelper.CompanyInfo.CompanyShortName == "GTT")
                {
                    // auto adjsut for gtt 
                    employeeSPService.GetDataWithoutParameter("SP_AUTO_LEAVE_ADJUST_ALL");
                }


            }
            catch (Exception e)
            {
                result = 0;
                message = "Application Approval Failed";
            }

            //try
            //{
                if (approverId > 0 && employeeId > 0 && !String.IsNullOrEmpty(LeaveStartDate) && !String.IsNullOrEmpty(LeaveEndDate) && isNextApproverExist)
                {
                    const string guId = "GID";
                    string destinationUrl = Url.Action("Index", "LeaveApprove", new { guid = guId }, Request.Url.Scheme);
                    int response = emailSenderService.SendNotificatinEmail(approverId, employeeId, LeaveStartDate, LeaveEndDate, destinationUrl, "Application", "", SessionHelper.CompanyCode.ToLower());
                }

                // Send Email to the Employee
                if (employeeId > 0 && !String.IsNullOrEmpty(LeaveStartDate) && !String.IsNullOrEmpty(LeaveEndDate) && !isNextApproverExist)
                {
                    string info = "Approved";
                    int response = emailSenderService.SendNotificatinEmail(approverId, employeeId, LeaveStartDate, LeaveEndDate, "", "Approved", "", SessionHelper.CompanyCode.ToLower());
                }
           //}
            //catch (Exception ex)
            //{

            //    result = 0;
            //    message =  ex.Message;
            //}
            // Send Mail to the Next Approvers
            


            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }



        public JsonResult leaveConfirm_all(string LeaveId )
        {
            var result = 0;
            long approverId = 0;
            long employeeId = 0;
            string message = string.Empty;
            string LeaveStartDate = string.Empty;
            string LeaveEndDate = string.Empty;
            bool isNextApproverExist = true;

            try
            {
               
                    var param = new { ApproverLoginId = LoggedInEmployeeId };
                    var approversListDATA = employeeSPService.GetDataWithParameter(param, "leave.ApproveAllPendingLeaves");

                    result = 1;
                    message = "Application Approved Successfully";

                    isNextApproverExist = false;
                   // Send Mail to the employee
                        

            }
            catch (Exception e)
            {
                result = 0;
                message = "Application Approval Failed";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult leaveConfirm_notsent(string LeaveId, int NotificationId, int ApprovalMasterId, int ApprovalDetailId, int ApplicationId)
        {
            var result = 0;
            long approverId = 0;
            long employeeId = 0;
            string message = string.Empty;
            string LeaveStartDate = string.Empty;
            string LeaveEndDate = string.Empty;
            bool isNextApproverExist = true;

            if (NotificationId < 1 || String.IsNullOrEmpty(LeaveId))
            {
                message = "LeaveId or NotificationId is null or empty";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var entity = approvalNotificationService.GetById(NotificationId);
                    var applicationInfo = leaveHistoryService.GetById(Convert.ToInt32(entity.ApplicationId));

                    approverId = entity.ApproverId;
                    employeeId = applicationInfo.EmployeeId;

                    LeaveStartDate = applicationInfo.LeaveStartDate.ToString("dd/MM/yyyy");
                    LeaveEndDate = applicationInfo.LeaveEndDate.ToString("dd/MM/yyyy");

                    var approversList = leaveApproversService.GetMany(x => x.IsActive == true && x.EmployeeId == employeeId).ToList().OrderBy(p => p.ApprovalLevel);

                    var finalApprovalLevel = 0;

                    //final approval level 



                    //if (finalApprovalLevel == currentLevel)
                    //{
                        entity.IsChecked = true;
                        entity.CheckedStatus = "A";
                        entity.CheckedDate = DateTime.UtcNow;
                        entity.UpdateUser = LoggedInEmployeeId;
                        entity.UpdateDate = DateTime.UtcNow;
                        approvalNotificationService.Update(entity);

                        var model = leaveHistoryService.GetById(Convert.ToInt32(LeaveId));
                        model.IsApproved = true;
                        model.ApprovedBy = Convert.ToInt32(LoggedInEmployeeId);
                        model.ApprovedDate = DateTime.Now;
                        leaveHistoryService.Update(model);

                        result = 1;
                        message = "Application Approved Successfully";
                        scope.Complete();
                        isNextApproverExist = false;

                        // Send Mail to the employee
                    //}
                    //else
                    //{
                    //    var nextApprovalLevel = approversList.Where(x => x.ApproverEmpId > 0 && x.ApprovalLevel > currentLevel).ToList().OrderBy(p => p.ApprovalLevel).FirstOrDefault();

                    //    if (nextApprovalLevel != null)
                    //    {
                    //        entity.IsChecked = true;
                    //        entity.CheckedStatus = "A";
                    //        entity.CheckedDate = DateTime.UtcNow;
                    //        entity.UpdateUser = LoggedInEmployeeId;
                    //        entity.UpdateDate = DateTime.UtcNow;
                    //        approvalNotificationService.Update(entity);

                    //        ApprovalNotification oApprovalNotification = new ApprovalNotification();
                    //        approverId = nextApprovalLevel.ApproverEmpId;
                    //        oApprovalNotification.ApplicationId = Convert.ToInt64(LeaveId);
                    //        oApprovalNotification.ApprovalDetailId = 0;
                    //        oApprovalNotification.ApprovalMasterId = 0;
                    //        oApprovalNotification.ApproverId = Convert.ToInt64(approverId);
                    //        oApprovalNotification.IsActive = true;
                    //        oApprovalNotification.ModuleName = "LM";
                    //        oApprovalNotification.CreateDate = DateTime.Now;
                    //        oApprovalNotification.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                    //        approvalNotificationService.Create(oApprovalNotification);

                    //        result = 1;
                    //        message = "Application Approved Successfully";
                    //        scope.Complete();
                    //    }
                    //}
                }

            }
            catch (Exception e)
            {
                result = 0;
                message = "Application Approval Failed";
            }


            // Send Mail to the Next Approvers
            if (approverId > 0 && employeeId > 0 && !String.IsNullOrEmpty(LeaveStartDate) && !String.IsNullOrEmpty(LeaveEndDate) && isNextApproverExist)
            {
                const string guId = "GID";
                string destinationUrl = Url.Action("Index", "LeaveApprove", new { guid = guId }, Request.Url.Scheme);
                int response = emailSenderService.SendNotificatinEmail(approverId, employeeId, LeaveStartDate, LeaveEndDate, destinationUrl, "Application", "", SessionHelper.CompanyCode.ToLower());
            }

            // Send Email to the Employee
            if (employeeId > 0 && !String.IsNullOrEmpty(LeaveStartDate) && !String.IsNullOrEmpty(LeaveEndDate) && !isNextApproverExist)
            {
                string info = "Approved";
                int response = emailSenderService.SendNotificatinEmail(approverId, employeeId, LeaveStartDate, LeaveEndDate, "", "Approved", "", SessionHelper.CompanyCode.ToLower());
            }


            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApproveReject(string LeaveId, int NotificationId, string reason)
        {
            int result = 0;
            long applicationId = 0;
            string message = "";

            if (NotificationId < 1 || String.IsNullOrEmpty(LeaveId))
            {
                message = "Leave is already rejected";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var entityNotification = approvalNotificationService.GetById(NotificationId);
                    applicationId = entityNotification.ApplicationId;

                    entityNotification.IsChecked = true;
                    entityNotification.CheckedStatus = "R";
                    
                    entityNotification.CheckedDate = DateTime.UtcNow;
                    entityNotification.UpdateUser = LoggedInEmployeeId;
                    entityNotification.UpdateDate = DateTime.UtcNow;
                    approvalNotificationService.Update(entityNotification);

                    var entity = leaveHistoryService.GetById(Convert.ToInt32(LeaveId));

                    entity.IsActive = false;
                    entity.IsApproved = false;
                    entity.IsAdjustment = true;
                    entity.Remarks = reason;
                    entity.InActiveDate = DateTime.Now;
                    entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    entity.UpdateDate = DateTime.Now;
                    leaveHistoryService.Update(entity);

                    // Dispatch Leave 
                    var LeaveAdjust = leaveHistoryService.GetAll().Where(x => x.DispatchLeaveId == Convert.ToInt64(LeaveId) && x.IsActive == true);

                    foreach (var r in LeaveAdjust)
                    {
                        r.IsActive = false;
                        r.IsApproved = false;
                        r.IsAdjustment = true;
                        r.InActiveDate = DateTime.Now;
                        r.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                        r.UpdateDate = DateTime.Now;
                        leaveHistoryService.Update(r);
                    }

                    result = 1;
                    message = "Application Rejected Successfully";
                    scope.Complete();
                }

                var applicationInfo = leaveHistoryService.GetById(Convert.ToInt32(applicationId));
                long applicantIdId = applicationInfo.EmployeeId;
                string LeaveStartDate = applicationInfo.LeaveStartDate.ToString("dd/MM/yyyy");
                string LeaveEndDate = applicationInfo.LeaveEndDate.ToString("dd/MM/yyyy");

                // Send Email to the Employee
                if (applicantIdId > 0 && !String.IsNullOrEmpty(LeaveStartDate) && !String.IsNullOrEmpty(LeaveEndDate))
                {                  
                    int response = emailSenderService.SendNotificatinEmail(LoggedInEmployeeId.Value, applicantIdId, LeaveStartDate, LeaveEndDate,"", "Rejected", reason, SessionHelper.CompanyCode.ToLower());
                }
            }
            catch (Exception e)
            {
                result = 0;
                message = "Application Rejection Failed";
            }
                
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLeaveApprovalPendingInfo([DataSourceRequest]DataSourceRequest request)
        {
            var pendingList = employeeSPService.GetDataWithoutParameter("leave.SP_GetPendingLeaveRequest");
            var viewPendingList = pendingList.Tables[0].AsEnumerable().Select(p => new LeaveApprovalPendingViewModel()
            {
                LeaveNo = p.Field<string>("LeaveNo"),
                LeaveTypeName = p.Field<string>("LeaveTypeName"),
                LeaveRequestDate = p.Field<DateTime>("LeaveRequestDate").ToString("dd-MMM-yyyy"),
                LeaveStartDate = p.Field<DateTime>("LeaveStartDate").ToString("dd-MMM-yyyy"),
                LeaveEndDate = p.Field<DateTime>("LeaveEndDate").ToString("dd-MMM-yyyy"),
                EmployeeName = p.Field<string>("EmployeeName"),
                EmployeeCode = p.Field<string>("EmployeeCode"),
                DepartmentName = p.Field<string>("DepartmentName"),
                OffcDesignName = p.Field<string>("OffcDesignName"),
                PendingApproverName = p.Field<string>("PendingApproverName")
            }).ToList();

            DataSourceResult result = viewPendingList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Methods
    
        private int CountValidLeaveApproverForEmployee(long p)
        {
            var apporverList = leaveApproversService.GetMany(x => x.IsActive == true && x.EmployeeId == p).ToList();
            //var levelCount = apporverList.Count();
            long isFinalApproverExist = 0;
            if (apporverList.Count > 0)
            {
                long levelCount = apporverList.Max(x => x.ApprovalLevel);
                isFinalApproverExist = apporverList.Where(x => x.ApprovalLevel == levelCount).First().ApproverEmpId;
            }

            if (isFinalApproverExist > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        #endregion

    }
}
