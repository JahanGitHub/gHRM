
#region Usings
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Transactions;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using AutoMapper;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using gHRM.Web.EmailSenderService;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.DBDetailModels;
using System.Text;
using gHRM.Web.CommonDropdown;
using gHRM.Service.Payroll;
using gHRM.Web.Reports.Leave;
using gHRM.Core.Utilities.Constants;
using gHRM.Core.Filters.Leaves;
using System.Web;
using System.IO;
using gHRM.Core.Utilities;
using BasicDataAccess;
using Newtonsoft.Json;
using System.Web.Configuration;
#endregion

namespace gHRM.Web.Controllers
{
    public class LeaveHistoryNewController : BaseController
    {
        #region Variables

        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IOfficeService officeService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly ILeaveHistoryService leaveHistoryService;
        private readonly ILeaveTypeService leaveTypeService;
        private readonly ILeaveSellService leaveSellService;
        private readonly ILeaveELOpeningService leaveELOpeningService;
        private readonly ILeaveMaternityOpeningService leaveMaternityOpeningService;
        private readonly IEmployeeFamilyInfoService employeeFamilyInfoService;
        private readonly IApprovalNotificationService approvalNotificationService;
        private readonly ILeaveAdjustmentAuthorityService leaveAdjustmentAuthorityService;
        private readonly IOfficeDesignationService officeDesignationService;
        private readonly IAttHolidayDeclarationService attHolidayDeclarationService;
        private readonly ILeaveApproversAuthorityService leaveApproversAuthorityService;

        private readonly IPRComponentService prComponentService;
        private readonly IEmployeeMonthlySalaryService employeeMonthlySalaryService;
        private readonly IEmployeeSalaryDeductionService employeeSalaryDeductionService;

        private readonly ILeaveApproversService leaveApproversService;
        private readonly ILeaveHistoryAttachmentService leaveHistoryAttachmentService;
        private readonly EmailSender2 emailSenderService;
        private readonly SmsSender smsSenderService;
        private string AttachmentFolder = "UploadedFiles/Leave/LeaveEntry";

        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;

        public LeaveHistoryNewController(
              IEmployeeService employeeService
            , IOfficeService officeService
            , IEmployeeDepartmentService employeeDepartmentService
            , ILeaveHistoryService leaveHistoryService
            , ILeaveTypeService leaveTypeService
            , ILeaveSellService leaveSellService, IEmployeeSPService employeeSPService
            , ILeaveELOpeningService leaveELOpeningService
            , ILeaveMaternityOpeningService leaveMaternityOpeningService
            , IEmployeeFamilyInfoService employeeFamilyInfoService
            , IApprovalNotificationService approvalNotificationService
            , ILeaveAdjustmentAuthorityService leaveAdjustmentAuthorityService
            , IOfficeDesignationService officeDesignationService
            , IAttHolidayDeclarationService attHolidayDeclarationService
            , IPRComponentService prComponentService
            , IEmployeeMonthlySalaryService employeeMonthlySalaryService
            , IEmployeeSalaryDeductionService employeeSalaryDeductionService
            , ILeaveApproversService leaveApproversService
            , ILeaveApproversAuthorityService leaveApproversAuthorityService
            , ILeaveHistoryAttachmentService leaveHistoryAttachmentService
            )
        {
            this.employeeService = employeeService;
            this.officeService = officeService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.leaveHistoryService = leaveHistoryService;
            this.leaveTypeService = leaveTypeService;
            this.leaveSellService = leaveSellService;
            this.employeeSPService = employeeSPService;
            this.leaveELOpeningService = leaveELOpeningService;
            this.leaveMaternityOpeningService = leaveMaternityOpeningService;
            this.employeeFamilyInfoService = employeeFamilyInfoService;
            this.approvalNotificationService = approvalNotificationService;
            this.leaveAdjustmentAuthorityService = leaveAdjustmentAuthorityService;
            this.officeDesignationService = officeDesignationService;
            this.attHolidayDeclarationService = attHolidayDeclarationService;
            this.prComponentService = prComponentService;
            this.employeeMonthlySalaryService = employeeMonthlySalaryService;
            this.employeeSalaryDeductionService = employeeSalaryDeductionService;
            this.leaveApproversService = leaveApproversService;
            this.leaveApproversAuthorityService = leaveApproversAuthorityService;
            this.leaveHistoryAttachmentService = leaveHistoryAttachmentService;
            emailSenderService = new EmailSender2();
            smsSenderService = new SmsSender();

            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion

        #region Leave Entry
        public ActionResult LeaveEntry()
        {
            var EmployeeID = SessionHelper.LoggedInEmployeeID;
            var empCode = employeeService.GetById(Convert.ToInt32(EmployeeID)).EmployeeCode;
            var model = new LeaveHistoryViewModel();
            model.EmployeeId = Convert.ToInt64(EmployeeID);
            model.EmployeeCode = empCode;
            ViewBag.LEAVE_ENTRY_PAGE_SHOW_LEAVE_DAY_DURATION = AppSetting.GetBool(AppSetting.LEAVE_ENTRY_PAGE_SHOW_LEAVE_DAY_DURATION, HttpContext);

            using (var DB = new gHRMDBContext())
            {
                List<int> AL_LeaveTypeIdList = DB.LeaveTypes.Where(x => x.IsActive && x.LeaveCategory == "AL").Select(x => x.LeaveTypeId).ToList();
                ViewBag.AL_LeaveTypeIdList = JsonConvert.SerializeObject(AL_LeaveTypeIdList);

                List<int> SL_LeaveTypeIdList = DB.LeaveTypes.Where(x => x.IsActive && x.LeaveCategory == "SL").Select(x => x.LeaveTypeId).ToList();
                ViewBag.SL_LeaveTypeIdList = JsonConvert.SerializeObject(SL_LeaveTypeIdList);
            }
            MapDropDownList(model);
            return View(model);
        }


        public ActionResult LeaveEntry2()
        {
            var EmployeeID = SessionHelper.LoggedInEmployeeID;
            var empCode = employeeService.GetById(Convert.ToInt32(EmployeeID)).EmployeeCode;
            var model = new LeaveHistoryViewModel();
            model.EmployeeId = Convert.ToInt64(EmployeeID);
            model.EmployeeCode = empCode;
            ViewBag.LEAVE_ENTRY_PAGE_SHOW_LEAVE_DAY_DURATION = AppSetting.GetBool(AppSetting.LEAVE_ENTRY_PAGE_SHOW_LEAVE_DAY_DURATION, HttpContext);

            using (var DB = new gHRMDBContext())
            {
                List<int> AL_LeaveTypeIdList = DB.LeaveTypes.Where(x => x.IsActive && x.LeaveCategory == "AL").Select(x => x.LeaveTypeId).ToList();
                ViewBag.AL_LeaveTypeIdList = JsonConvert.SerializeObject(AL_LeaveTypeIdList);

                List<int> SL_LeaveTypeIdList = DB.LeaveTypes.Where(x => x.IsActive && x.LeaveCategory == "SL").Select(x => x.LeaveTypeId).ToList();
                ViewBag.SL_LeaveTypeIdList = JsonConvert.SerializeObject(SL_LeaveTypeIdList);
            }
            MapDropDownList(model);
            return View(model);
        }

        public ActionResult LeaveDelete2()
        {
            var EmployeeID = SessionHelper.LoggedInEmployeeID;
            var empCode = employeeService.GetById(Convert.ToInt32(EmployeeID)).EmployeeCode;
            var model = new LeaveHistoryViewModel();
            model.EmployeeId = Convert.ToInt64(EmployeeID);
            model.EmployeeCode = empCode;
            ViewBag.LEAVE_ENTRY_PAGE_SHOW_LEAVE_DAY_DURATION = AppSetting.GetBool(AppSetting.LEAVE_ENTRY_PAGE_SHOW_LEAVE_DAY_DURATION, HttpContext);

            using (var DB = new gHRMDBContext())
            {
                List<int> AL_LeaveTypeIdList = DB.LeaveTypes.Where(x => x.IsActive && x.LeaveCategory == "AL").Select(x => x.LeaveTypeId).ToList();
                ViewBag.AL_LeaveTypeIdList = JsonConvert.SerializeObject(AL_LeaveTypeIdList);

                List<int> SL_LeaveTypeIdList = DB.LeaveTypes.Where(x => x.IsActive && x.LeaveCategory == "SL").Select(x => x.LeaveTypeId).ToList();
                ViewBag.SL_LeaveTypeIdList = JsonConvert.SerializeObject(SL_LeaveTypeIdList);
            }
            MapDropDownList(model);
            return View(model);
        }


        public ActionResult LeaveEntry_Pidim()
        {
            var EmployeeID = SessionHelper.LoggedInEmployeeID;
            var empCode = employeeService.GetById(Convert.ToInt32(EmployeeID)).EmployeeCode;
            var model = new LeaveHistoryViewModel();
            model.EmployeeId = Convert.ToInt64(EmployeeID);
            model.EmployeeCode = empCode;
            ViewBag.LEAVE_ENTRY_PAGE_SHOW_LEAVE_DAY_DURATION = AppSetting.GetBool(AppSetting.LEAVE_ENTRY_PAGE_SHOW_LEAVE_DAY_DURATION, HttpContext);

            using (var DB = new gHRMDBContext())
            {
                List<int> AL_LeaveTypeIdList = DB.LeaveTypes.Where(x => x.IsActive && x.LeaveCategory == "AL").Select(x => x.LeaveTypeId).ToList();
                ViewBag.AL_LeaveTypeIdList = JsonConvert.SerializeObject(AL_LeaveTypeIdList);

                List<int> SL_LeaveTypeIdList = DB.LeaveTypes.Where(x => x.IsActive && x.LeaveCategory == "SL").Select(x => x.LeaveTypeId).ToList();
                ViewBag.SL_LeaveTypeIdList = JsonConvert.SerializeObject(SL_LeaveTypeIdList);
            }
            MapDropDownList(model);
            return View(model);
        }

        [HttpPost]
        public JsonResult LeaveEntry(LeaveHistoryViewModel model, FormCollection form)
        {
            int result = 0;
            long leaveId = 0;
            long employeeId = 0;
            string message = string.Empty;
            string LeaveStartDate = string.Empty;
            string LeaveEndDate = string.Empty;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    List<string> FileNameList;
                    List<string> AttachmentPathList = UploadAttachmentOnLeaveEntry(out FileNameList);
                    if (model.LeaveStartDate != model.LeaveEndDate) model.LeaveDayDuration = "Full";
                    if (model.IsAbsentLeave == 1)
                    {
                        employeeId = Convert.ToInt64(model.EmployeeId);
                        var empInfo = employeeService.GetById(Convert.ToInt32(employeeId));

                        //get leave type
                        var leaveType = leaveTypeService.Get(x =>
                                                     x.IsActive == true
                                                  && x.LeaveTypeId == model.LeaveTypeId
                                                  && (x.LeaveGender == empInfo.Gender.Trim() || x.LeaveGender == LeaveTypeGenderConstants.Both)
                                                  && x.EmployeeStatusId == empInfo.EmployeeStatusId);

                        model.LeaveTypeId = leaveType.LeaveTypeId;

                        model.TotalDays = Convert.ToInt32((Convert.ToDateTime(model.LeaveEndDate) - Convert.ToDateTime(model.LeaveStartDate)).TotalDays) + 1;
                        model.LeaveRequestDate = DateTime.Now;
                        model.LeaveReason = LeaveReasonConstants.Absent;
                    }
                    else
                    {
                        employeeId = Convert.ToInt64(LoggedInEmployeeId);
                    }

                    var approvalLevelCount = CountValidLeaveApproverForEmployee(employeeId);
                    var leavetye = leaveTypeService.GetById(model.LeaveTypeId);

                    if (approvalLevelCount == 0)
                    {
                        result = 0;
                        message = "No Approval Configuration. Please Contact your Admin for Leave Setup.";
                        return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);
                    }


                    // if leave date with in the present of a employee 
                    int isPresentInLeaveDays = CheckEmployeePresentInLeaveDays(employeeId, model.LeaveStartDate, model.LeaveEndDate);

                    if (isPresentInLeaveDays == 1)
                    {
                        result = 0;
                        message = "Sorry Appliead Leave days from " + model.LeaveStartDate?.ToString("dd/MM/yyyy") + " to " + model.LeaveEndDate?.ToString("dd/MM/yyyy") + " are Regular present found , so these days are not applicable for leave";
                        return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);
                    }


                    if (leavetye.LeaveStatus == LeaveStatusConstants.Laps &&
                        model.LeaveStartDate.Value.Year != model.LeaveEndDate.Value.Year)
                    {
                        result = 0;
                        message = "Casual leave cannot be applied between two years";
                        return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);
                    }

                    long approverId = 0;
                    var newLeaveHistory = Mapper.Map<LeaveHistoryViewModel, LeaveHistory>(model);

                    //check replacement employee is in leave or not 
                    //when applicant not a replacement employee
                    if (model.EmployeeId != model.ReplacementEmployee)
                    {
                        //check replacement employee is in leave or not
                        var isEmployeeInLeave = leaveHistoryService.IsEmployeeInLeave(newLeaveHistory);

                        if (isEmployeeInLeave)
                        {
                            scope.Dispose();
                            message = "Replacement employee is in leave";
                            return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    newLeaveHistory.IsAdjustment = false; //when creating new then set it as false
                    newLeaveHistory.IsActive = true;
                    newLeaveHistory.CreateUser = SessionHelper.LoggedInEmployeeID;
                    newLeaveHistory.CreateDate = DateTime.Now;

                    //let's insert leave history [leave.LeaveHistory]
                    var application = leaveHistoryService.Create(newLeaveHistory);

                    for (int i = 0; i < AttachmentPathList.Count; i++)
                    {
                        LeaveHistoryAttachment LHAttachment = new LeaveHistoryAttachment();
                        LHAttachment.LeaveHistoryId = application.LeaveId;
                        LHAttachment.FileName = FileNameList[i];
                        LHAttachment.FileLocation = AttachmentPathList[i];
                        LHAttachment.IsActive = true;
                        LHAttachment.CreateUser = LoggedInEmployeeId ?? 0;
                        LHAttachment.CreateDate = DateTime.Now;
                        leaveHistoryAttachmentService.Create(LHAttachment);
                    }
                    leaveId = application.LeaveId;

                    //get first approver employee Id
                    var firstLeavelApprover = leaveApproversService.GetMany(x =>
                                                        x.IsActive == true &&
                                                        x.ApproverEmpId > 0 &&
                                                        x.EmployeeId == employeeId)
                                                    .ToList()
                                                    .OrderBy(p => p.ApprovalLevel)
                                                    .FirstOrDefault();

                    if (firstLeavelApprover != null)
                        approverId = firstLeavelApprover.ApproverEmpId;

                    var newApprovalNotification = new ApprovalNotification();
                    newApprovalNotification.ApplicationId = leaveId;
                    newApprovalNotification.ApprovalDetailId = 0;
                    newApprovalNotification.ApprovalMasterId = 0;
                    newApprovalNotification.ApproverId = Convert.ToInt64(approverId);
                    newApprovalNotification.IsActive = true;
                    newApprovalNotification.IsChecked = false;
                    newApprovalNotification.ModuleName = "LM";
                    newApprovalNotification.CreateDate = DateTime.Now;
                    newApprovalNotification.CreateUser = Convert.ToInt64(LoggedInEmployeeId);

                    //let's insert approver notification [leave.ApprovalNotification]
                    approvalNotificationService.Create(newApprovalNotification);

                    // The Complete method commits the transaction. If an exception has been thrown,
                    // Complete is not  called and the transaction is rolled back.

                    // SEND EMAIL NOTIFICATION
                    LeaveStartDate = newLeaveHistory.LeaveStartDate.ToString("dd/MM/yyyy");
                    LeaveEndDate = newLeaveHistory.LeaveEndDate.ToString("dd/MM/yyyy");
                    bool IsEmailSendSuccessFull = false;

                    if (approverId > 0 && employeeId > 0 &&
                        !String.IsNullOrEmpty(LeaveStartDate) &&
                        !String.IsNullOrEmpty(LeaveEndDate))
                    {
                        const string guId = "GID";
                        string destinationUrl = Url.Action("Index", "LeaveApprove", new { guid = guId }, Request.Url.Scheme);

                        // bool sandBoxMood = false;//set values to test leave apply

                        bool devEnv = false;
                        if (WebConfigurationManager.AppSettings["IsDevelopment"] != null)
                            devEnv = bool.Parse(WebConfigurationManager.AppSettings["IsDevelopment"].ToString());

                        if (!devEnv)
                        {
                            //EmailHelper _Helper = new EmailHelper(HttpContext);
                           // emailSenderService._Helper = _Helper;
                        }


                        if (!devEnv)
                        {
                            //let's send email notification to approver
                            IsEmailSendSuccessFull = emailSenderService.SendNotificatinEmail(
                                                    approverId,
                                                    employeeId,
                                                    LeaveStartDate,
                                                    LeaveEndDate,
                                                    destinationUrl,
                                                    EmailNotificationTypeConstants.Application,
                                                    "", SessionHelper.CompanyCode.ToLower()) == 1;


                            //TODO: need to revise
                            //smsSenderService.SendSMS();
                            if (model.ReplacementEmployee > 0)
                            {
                                var replacementEmployee = Convert.ToInt32(model.ReplacementEmployee);

                                //let's send email notification to replacement employee
                                IsEmailSendSuccessFull = emailSenderService.SendNotificatinEmail(
                                                replacementEmployee,
                                                employeeId,
                                                LeaveStartDate,
                                                LeaveEndDate,
                                                "",
                                                EmailNotificationTypeConstants.Replacement,
                                                "", SessionHelper.CompanyCode.ToLower()) == 1;
                            }
                        }
                    }

                    scope.Complete();
                    scope.Dispose();
                    result = 1;
                    message = "Successfully Applied For Leave" + (IsEmailSendSuccessFull ? "" : ". But failed to send email.");
                    return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    result = 0;
                    message = ex.Message+ex.InnerException+ex.Data+ex.Source;
                    return Json(new { Result = result, Message = message.Length == 0 ? "Failed to save data. Please verify all required fields" : message }, JsonRequestBehavior.AllowGet);
                }
            }
        }



        [HttpPost]
        public JsonResult LeaveEntry2(LeaveHistoryViewModel model, FormCollection form)
        {
            int result = 0;
            long leaveId = 0;
            long employeeId = 0;
            string message = string.Empty;
            string LeaveStartDate = string.Empty;
            string LeaveEndDate = string.Empty;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    List<string> FileNameList;
                    List<string> AttachmentPathList = UploadAttachmentOnLeaveEntry(out FileNameList);
                    if (model.LeaveStartDate != model.LeaveEndDate) model.LeaveDayDuration = "Full";
                    if (model.IsAbsentLeave == 1)
                    {
                        employeeId = Convert.ToInt64(model.EmployeeId);
                        var empInfo = employeeService.GetById(Convert.ToInt32(employeeId));

                        //get leave type
                        var leaveType = leaveTypeService.Get(x =>
                                                     x.IsActive == true
                                                  && x.LeaveTypeId == model.LeaveTypeId
                                                  && (x.LeaveGender == empInfo.Gender.Trim() || x.LeaveGender == LeaveTypeGenderConstants.Both)
                                                  && x.EmployeeStatusId == empInfo.EmployeeStatusId);

                        model.LeaveTypeId = leaveType.LeaveTypeId;

                        model.TotalDays = Convert.ToInt32((Convert.ToDateTime(model.LeaveEndDate) - Convert.ToDateTime(model.LeaveStartDate)).TotalDays) + 1;
                        model.LeaveRequestDate = DateTime.Now;
                        model.LeaveReason = LeaveReasonConstants.Absent;
                    }
                    else
                    {
                        employeeId = Convert.ToInt64(LoggedInEmployeeId);
                    }

                    var approvalLevelCount = CountValidLeaveApproverForEmployee(employeeId);
                    var leavetye = leaveTypeService.GetById(model.LeaveTypeId);

                    if (leavetye.LeaveCategory == "SL")
                    {
                        if(AttachmentPathList.Count == 0)
                        {
                            result = 0;
                            message = "Medical certificate is required for sick leave.";
                            return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);
                        }
                    }


                    if (approvalLevelCount == 0)
                    {
                        result = 0;
                        message = "No Approval Configuration. Please Contact your Admin for Leave Setup.";
                        return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);
                    }


                    // if leave date with in the present of a employee 
                    int isPresentInLeaveDays = CheckEmployeePresentInLeaveDays(employeeId, model.LeaveStartDate, model.LeaveEndDate);

                    if (isPresentInLeaveDays == 1)
                    {
                        result = 0;
                        message = "Sorry Appliead Leave days from " + model.LeaveStartDate?.ToString("dd/MM/yyyy") + " to " + model.LeaveEndDate?.ToString("dd/MM/yyyy") + " are Regular present found , so these days are not applicable for leave";
                        return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);
                    }


                    if (leavetye.LeaveStatus == LeaveStatusConstants.Laps &&
                        model.LeaveStartDate.Value.Year != model.LeaveEndDate.Value.Year)
                    {
                        result = 0;
                        message = "Casual leave cannot be applied between two years";
                        return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);
                    }

                    long approverId = 0;
                    var newLeaveHistory = Mapper.Map<LeaveHistoryViewModel, LeaveHistory>(model);

                    //check replacement employee is in leave or not 
                    //when applicant not a replacement employee
                    if (model.EmployeeId != model.ReplacementEmployee)
                    {
                        //check replacement employee is in leave or not
                        var isEmployeeInLeave = leaveHistoryService.IsEmployeeInLeave(newLeaveHistory);

                        if (isEmployeeInLeave)
                        {
                            scope.Dispose();
                            message = "Replacement employee is in leave";
                            return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    newLeaveHistory.IsAdjustment = false; //when creating new then set it as false
                    newLeaveHistory.IsActive = true;
                    newLeaveHistory.CreateUser = SessionHelper.LoggedInEmployeeID;
                    newLeaveHistory.CreateDate = DateTime.Now;

                    //let's insert leave history [leave.LeaveHistory]
                    var application = leaveHistoryService.Create(newLeaveHistory);

                    for (int i = 0; i < AttachmentPathList.Count; i++)
                    {
                        LeaveHistoryAttachment LHAttachment = new LeaveHistoryAttachment();
                        LHAttachment.LeaveHistoryId = application.LeaveId;
                        LHAttachment.FileName = FileNameList[i];
                        LHAttachment.FileLocation = AttachmentPathList[i];
                        LHAttachment.IsActive = true;
                        LHAttachment.CreateUser = LoggedInEmployeeId ?? 0;
                        LHAttachment.CreateDate = DateTime.Now;
                        leaveHistoryAttachmentService.Create(LHAttachment);
                    }
                    leaveId = application.LeaveId;

                    //get first approver employee Id
                    var firstLeavelApprover = leaveApproversService.GetMany(x =>
                                                        x.IsActive == true &&
                                                        x.ApproverEmpId > 0 &&
                                                        x.EmployeeId == employeeId)
                                                    .ToList()
                                                    .OrderBy(p => p.ApprovalLevel)
                                                    .FirstOrDefault();

                    if (firstLeavelApprover != null)
                        approverId = firstLeavelApprover.ApproverEmpId;

                    var newApprovalNotification = new ApprovalNotification();
                    newApprovalNotification.ApplicationId = leaveId;
                    newApprovalNotification.ApprovalDetailId = 0;
                    newApprovalNotification.ApprovalMasterId = 0;
                    newApprovalNotification.ApproverId = Convert.ToInt64(approverId);
                    newApprovalNotification.IsActive = true;
                    newApprovalNotification.IsChecked = false;
                    newApprovalNotification.ModuleName = "LM";
                    newApprovalNotification.CreateDate = DateTime.Now;
                    newApprovalNotification.CreateUser = Convert.ToInt64(LoggedInEmployeeId);

                    //let's insert approver notification [leave.ApprovalNotification]
                    approvalNotificationService.Create(newApprovalNotification);

                    // The Complete method commits the transaction. If an exception has been thrown,
                    // Complete is not  called and the transaction is rolled back.

                    // SEND EMAIL NOTIFICATION
                    LeaveStartDate = newLeaveHistory.LeaveStartDate.ToString("dd/MM/yyyy");
                    LeaveEndDate = newLeaveHistory.LeaveEndDate.ToString("dd/MM/yyyy");
                    bool IsEmailSendSuccessFull = false;

                    if (approverId > 0 && employeeId > 0 &&
                        !String.IsNullOrEmpty(LeaveStartDate) &&
                        !String.IsNullOrEmpty(LeaveEndDate))
                    {
                        const string guId = "GID";
                        string destinationUrl = Url.Action("Index", "LeaveApprove", new { guid = guId }, Request.Url.Scheme);

                        // bool sandBoxMood = false;//set values to test leave apply

                        bool devEnv = false;
                        if (WebConfigurationManager.AppSettings["IsDevelopment"] != null)
                            devEnv = bool.Parse(WebConfigurationManager.AppSettings["IsDevelopment"].ToString());

                        if (!devEnv)
                        {
                            //EmailHelper _Helper = new EmailHelper(HttpContext);
                            //emailSenderService._Helper = _Helper;
                        }


                        if (!devEnv)
                        {
                            //let's send email notification to approver
                            IsEmailSendSuccessFull = emailSenderService.SendNotificatinEmail(
                                                    approverId,
                                                    employeeId,
                                                    LeaveStartDate,
                                                    LeaveEndDate,
                                                    destinationUrl,
                                                    EmailNotificationTypeConstants.Application,
                                                    "", SessionHelper.CompanyCode.ToLower()) == 1;


                            //TODO: need to revise
                            //smsSenderService.SendSMS();
                            if (model.ReplacementEmployee > 0)
                            {
                                var replacementEmployee = Convert.ToInt32(model.ReplacementEmployee);

                                //let's send email notification to replacement employee
                                IsEmailSendSuccessFull = emailSenderService.SendNotificatinEmail(
                                                replacementEmployee,
                                                employeeId,
                                                LeaveStartDate,
                                                LeaveEndDate,
                                                "",
                                                EmailNotificationTypeConstants.Replacement,
                                                "", SessionHelper.CompanyCode.ToLower()) == 1;
                            }
                        }
                    }

                    scope.Complete();
                    scope.Dispose();
                    result = 1;
                    message = "Successfully Applied For Leave" + (IsEmailSendSuccessFull ? "" : ". But failed to send email.");
                    return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    result = 0;
                    message = ex.Message + ex.InnerException + ex.Data + ex.Source;
                    return Json(new { Result = result, Message = message.Length == 0 ? "Failed to save data. Please verify all required fields" : message }, JsonRequestBehavior.AllowGet);
                }
            }
        }


        [HttpPost]
        public JsonResult LeaveEntry_Pidim(LeaveHistoryViewModel model, FormCollection form)
        {
            int result = 0;
            long leaveId = 0;
            long employeeId = 0;
            int companyId = 0;
            string message = string.Empty;
            string LeaveStartDate = string.Empty;
            string LeaveEndDate = string.Empty;
            string AttenDate = string.Empty;
            string Companyname = string.Empty;
            //string  CompanyShortName = string.Empty;

            var companyShortNameList = "";

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    List<string> FileNameList;
                    List<string> AttachmentPathList = UploadAttachmentOnLeaveEntry(out FileNameList);
                    if (model.LeaveStartDate != model.LeaveEndDate) model.LeaveDayDuration = "Full";
                    if (model.IsAbsentLeave == 1)
                    {
                        employeeId = Convert.ToInt64(model.EmployeeId);
                        var empInfo = employeeService.GetById(Convert.ToInt32(employeeId));

                        //get leave type
                        var leaveType = leaveTypeService.Get(x =>
                                                     x.IsActive == true
                                                  && x.LeaveTypeId == model.LeaveTypeId
                                                  && (x.LeaveGender == empInfo.Gender.Trim() || x.LeaveGender == LeaveTypeGenderConstants.Both)
                                                  && x.EmployeeStatusId == empInfo.EmployeeStatusId);

                        model.LeaveTypeId = leaveType.LeaveTypeId;

                        model.TotalDays = Convert.ToInt32((Convert.ToDateTime(model.LeaveEndDate) - Convert.ToDateTime(model.LeaveStartDate)).TotalDays) + 1;
                        model.LeaveRequestDate = DateTime.Now;
                        model.LeaveReason = LeaveReasonConstants.Absent;
                    }
                    else
                    {
                        employeeId = Convert.ToInt64(LoggedInEmployeeId);
                    }

                    var approvalLevelCount = CountValidLeaveApproverForEmployee(employeeId);
                    var leavetye = leaveTypeService.GetById(model.LeaveTypeId);

                    if (approvalLevelCount == 0)
                    {
                        result = 0;
                        message = "No Approval Configuration. Please Contact your Admin for Leave Setup.";
                        return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);
                    }

                    companyId = (int)SessionHelper.CompanyID;

                    Companyname = SessionHelper.CompanyName;

                    // if leave date with in the present of a employee 
                    int isPresentInLeaveDays = CheckEmployeePresentInLeaveDays(employeeId, model.LeaveStartDate, model.LeaveEndDate, Companyname);


                    if (isPresentInLeaveDays == 1)
                    {
                        result = 0;
                        message = "Sorry Appliead Leave days from " + model.LeaveStartDate?.ToString("dd/MM/yyyy") + " to " + model.LeaveEndDate?.ToString("dd/MM/yyyy") + " are Regular present found , so these days are not applicable for leave";
                        return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);
                    }


                    if (leavetye.LeaveStatus == LeaveStatusConstants.Laps &&
                        model.LeaveStartDate.Value.Year != model.LeaveEndDate.Value.Year)
                    {
                        result = 0;
                        message = "Casual leave cannot be applied between two years";
                        return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);
                    }

                    long approverId = 0;
                    var newLeaveHistory = Mapper.Map<LeaveHistoryViewModel, LeaveHistory>(model);

                    //check replacement employee is in leave or not 
                    //when applicant not a replacement employee
                    if (model.EmployeeId != model.ReplacementEmployee)
                    {
                        //check replacement employee is in leave or not
                        var isEmployeeInLeave = leaveHistoryService.IsEmployeeInLeave(newLeaveHistory);

                        if (isEmployeeInLeave)
                        {
                            scope.Dispose();
                            message = "Replacement employee is in leave";
                            return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    newLeaveHistory.IsAdjustment = false; //when creating new then set it as false
                    newLeaveHistory.IsActive = true;
                    newLeaveHistory.CreateUser = SessionHelper.LoggedInEmployeeID;
                    newLeaveHistory.CreateDate = DateTime.Now;

                    //let's insert leave history [leave.LeaveHistory]
                    var application = leaveHistoryService.Create(newLeaveHistory);

                    for (int i = 0; i < AttachmentPathList.Count; i++)
                    {
                        LeaveHistoryAttachment LHAttachment = new LeaveHistoryAttachment();
                        LHAttachment.LeaveHistoryId = application.LeaveId;
                        LHAttachment.FileName = FileNameList[i];
                        LHAttachment.FileLocation = AttachmentPathList[i];
                        LHAttachment.IsActive = true;
                        LHAttachment.CreateUser = LoggedInEmployeeId ?? 0;
                        LHAttachment.CreateDate = DateTime.Now;
                        leaveHistoryAttachmentService.Create(LHAttachment);
                    }
                    leaveId = application.LeaveId;

                    //get first approver employee Id
                    var firstLeavelApprover = leaveApproversService.GetMany(x =>
                                                        x.IsActive == true &&
                                                        x.ApproverEmpId > 0 &&
                                                        x.EmployeeId == employeeId)
                                                    .ToList()
                                                    .OrderBy(p => p.ApprovalLevel)
                                                    .FirstOrDefault();

                    if (firstLeavelApprover != null)
                        approverId = firstLeavelApprover.ApproverEmpId;

                    var newApprovalNotification = new ApprovalNotification();
                    newApprovalNotification.ApplicationId = leaveId;
                    newApprovalNotification.ApprovalDetailId = 0;
                    newApprovalNotification.ApprovalMasterId = 0;
                    newApprovalNotification.ApproverId = Convert.ToInt64(approverId);
                    newApprovalNotification.IsActive = true;
                    newApprovalNotification.IsChecked = false;
                    newApprovalNotification.ModuleName = "LM";
                    newApprovalNotification.CreateDate = DateTime.Now;
                    newApprovalNotification.CreateUser = Convert.ToInt64(LoggedInEmployeeId);

                    //let's insert approver notification [leave.ApprovalNotification]
                    approvalNotificationService.Create(newApprovalNotification);

                    // The Complete method commits the transaction. If an exception has been thrown,
                    // Complete is not  called and the transaction is rolled back.

                    // SEND EMAIL NOTIFICATION
                    LeaveStartDate = newLeaveHistory.LeaveStartDate.ToString("dd/MM/yyyy");
                    LeaveEndDate = newLeaveHistory.LeaveEndDate.ToString("dd/MM/yyyy");
                    bool IsEmailSendSuccessFull = false;

                    if (approverId > 0 && employeeId > 0 &&
                        !String.IsNullOrEmpty(LeaveStartDate) &&
                        !String.IsNullOrEmpty(LeaveEndDate))
                    {
                        const string guId = "GID";
                        string destinationUrl = Url.Action("Index", "LeaveApprove", new { guid = guId }, Request.Url.Scheme);

                        // bool sandBoxMood = false;//set values to test leave apply

                        bool devEnv = false;
                        if (WebConfigurationManager.AppSettings["IsDevelopment"] != null)
                            devEnv = bool.Parse(WebConfigurationManager.AppSettings["IsDevelopment"].ToString());

                        if (!devEnv)
                        {
                            //EmailHelper _Helper = new EmailHelper(HttpContext);
                           // emailSenderService._Helper = _Helper;
                        }

                        if (!devEnv)
                        {
                            //let's send email notification to approver
                            IsEmailSendSuccessFull = emailSenderService.SendNotificatinEmail(
                                                    approverId,
                                                    employeeId,
                                                    LeaveStartDate,
                                                    LeaveEndDate,
                                                    destinationUrl,
                                                    EmailNotificationTypeConstants.Application,
                                                    "", SessionHelper.CompanyCode.ToLower()) == 1;


                            //TODO: need to revise
                            //smsSenderService.SendSMS();
                            if (model.ReplacementEmployee > 0)
                            {
                                var replacementEmployee = Convert.ToInt32(model.ReplacementEmployee);

                                //let's send email notification to replacement employee
                                IsEmailSendSuccessFull = emailSenderService.SendNotificatinEmail(
                                                replacementEmployee,
                                                employeeId,
                                                LeaveStartDate,
                                                LeaveEndDate,
                                                "",
                                                EmailNotificationTypeConstants.Replacement,
                                                "", SessionHelper.CompanyCode.ToLower()) == 1;
                            }
                        }
                    }

                    scope.Complete();
                    scope.Dispose();
                    result = 1;
                    message = "Successfully Applied For Leave" + (IsEmailSendSuccessFull ? "" : ". But failed to send email.");
                    return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    result = 0;
                    message = ex.Message;
                    return Json(new { Result = result, Message = message.Length == 0 ? "Failed to save data. Please verify all required fields" : message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        private int CheckEmployeePresentInLeaveDays(long employeeId, DateTime? leaveStartDate, DateTime? leaveEndDate, string Companyname)
        {

            var company = SessionHelper.CompanyName;
            if (company == "Pidim Foundation")
            {
                int result = 0;
                var param = new { employeeId = employeeId, leaveStartDate = leaveStartDate, leaveEndDate = leaveEndDate, Companyname = Companyname };
                var LeaveInOffice = employeeSPService.GetDataWithParameter(param, "leave.SP_Validate_LeaveDays_In_Regualar_Present");

                if (LeaveInOffice.Tables[0].Rows.Count > 0)
                {
                    result = 0;
                }
                return result;
            }

            else
            {
                int result = 0;
                var param = new { employeeId = employeeId, leaveStartDate = leaveStartDate, leaveEndDate = leaveEndDate };
                var LeaveInOffice = employeeSPService.GetDataWithParameter(param, "leave.SP_Validate_LeaveDays_In_Regualar_Present");
                if (LeaveInOffice.Tables[0].Rows.Count > 0)
                {
                    result = 1;
                }

                return result;
            }
        }



        private int CheckEmployeePresentInLeaveDays(long employeeId, DateTime? leaveStartDate, DateTime? leaveEndDate)
        {
            int result = 0;

            var param = new { employeeId = employeeId, leaveStartDate = leaveStartDate, leaveEndDate = leaveEndDate };
            var LeaveInOffice = employeeSPService.GetDataWithParameter(param, "leave.SP_Validate_LeaveDays_In_Regualar_Present");

            if (LeaveInOffice.Tables[0].Rows.Count > 0)
                result = 1;
            
            return result;
        }

        #endregion

        #region Events

        public ActionResult Index()
        {
            ViewData["OfficeTypeId"] = SessionHelper.LoginUserOfficeType;
            return View();
        }

        public ActionResult LeaveAdjustment(int id)
        {
            var leave = leaveHistoryService.GetById(Convert.ToInt32(id));
            var EmployeeId = leave.EmployeeId;
            var employee = employeeService.GetByEmpId(Convert.ToInt64(EmployeeId));
            var empDept = employeeDepartmentService.GetById(Convert.ToInt32(employee.DepartmentId));

            var entity = Mapper.Map<LeaveHistory, LeaveHistoryViewModel>(leave);
            entity.EmployeeId = Convert.ToInt64(EmployeeId);
            entity.LeaveTypeName = leaveTypeService.GetById(entity.LeaveTypeId).LeaveTypeName;
            entity.EmployeeCode = employee.EmployeeCode;
            entity.EmployeeName = employee.EmployeeName;
            entity.OfficeName = officeService.GetById(Convert.ToInt32(employee.OfficeId)).OfficeName;
            entity.DepartmentName = empDept.DepartmentName;
            entity.JoinDate = leave.JoinDate;
            entity.EmpGender = employee.Gender;
            //entity.EmpStatus = employee.EmployeeStatusId.ToString();
            entity.EmployeeStatusId = employee.EmployeeStatusId;
            entity.JoinDateMsg = leave.JoinDate.HasValue ? String.Format("{0:dd-MMM-yyyy}", leave.JoinDate) : "";
            entity.LeaveStartDateMsg = String.Format("{0:dd-MMM-yyyy}", leave.LeaveStartDate);
            entity.LeaveEndDateMsg = String.Format("{0:dd-MMM-yyyy}", leave.LeaveEndDate);
            entity.LeaveRequestDateMsg = String.Format("{0:dd-MMM-yyyy}", leave.LeaveRequestDate);
            entity.ReplacementEmployee = leave.ReplacementEmployee.HasValue ? leave.ReplacementEmployee : null;
            if (entity.ReplacementEmployee.HasValue)
            {
                entity.ReplacementEmployeeName = employeeService.GetByEmpId((long)entity.ReplacementEmployee).EmployeeName;
            }
            entity.DepartmentId = empDept.DepartmentId;
            var empRank = "";
            if (employee.EmployeeRank == "O")
            {
                empRank = "Officer";
            }
            else
            {
                empRank = "Staff";
            }
            entity.EmployeeRank = empRank;
            entity.DesignationName = officeDesignationService.GetById(Convert.ToInt32(employee.EmployeeRank.Trim())).OffcDesignName;
            entity.EmpGender = employee.Gender;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["LeaveTypeList"] = items;
            ViewData["OfficeTypeId"] = SessionHelper.LoginUserOfficeType;

            return View(entity);
        }

        public ActionResult BackDateLeaveEntry()
        {
            var EmployeeID = SessionHelper.LoggedInEmployeeID;
            var empCode = employeeService.GetById(Convert.ToInt32(EmployeeID)).EmployeeCode;
            var model = new LeaveHistoryViewModel();
            model.EmployeeId = Convert.ToInt64(EmployeeID);
            model.EmployeeCode = empCode;

            MapDropDownList(model);

            return View(model);
        }

        public ActionResult BackDateLeaveEntry2()
        {
            var EmployeeID = SessionHelper.LoggedInEmployeeID;
            var empCode = employeeService.GetById(Convert.ToInt32(EmployeeID)).EmployeeCode;
            var model = new LeaveHistoryViewModel();
            model.EmployeeId = Convert.ToInt64(EmployeeID);
            model.EmployeeCode = empCode;

            MapDropDownList(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult BackDateLeaveEntry(LeaveHistoryViewModel model)
        {
            int result = 0;
            long leaveId = 0;
            string message = string.Empty;
            string LeaveStartDate = string.Empty;
            string LeaveEndDate = string.Empty;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var isduplicate = leaveHistoryService
                        .GetMany(p => p.IsActive == true &&
                        p.IsApproved == true &&
                        p.EmployeeId == model.EmployeeId &&
                        ((p.LeaveStartDate <= model.LeaveStartDate && p.LeaveEndDate >= model.LeaveStartDate) ||
                        (p.LeaveStartDate <= model.LeaveEndDate && p.LeaveEndDate >= model.LeaveEndDate))
                    ).ToList();

                    if (isduplicate.Count > 0)
                    {
                        scope.Dispose();
                        result = 0;
                        message = "Duplicate Leave Found";
                        return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);
                    }

                    var entity = Mapper.Map<LeaveHistoryViewModel, LeaveHistory>(model);

                    entity.IsApproved = true;
                    entity.IsAdjustment = true;
                    entity.AdjustmentDate = DateTime.Now;
                    entity.LeaveReason = "Backlog Entry";
                    entity.IsActive = true;
                    entity.CreateUser = SessionHelper.LoggedInEmployeeID;
                    entity.CreateDate = DateTime.Now;
                    entity.LeaveDayDuration = "Full";
                    //let's add leave history [leave].[LeaveHistory]
                    var application = leaveHistoryService.Create(entity);
                    leaveId = application.LeaveId;

                    scope.Complete();
                    scope.Dispose();
                    result = 1;
                    message = "Leave Saved Successfully";
                    return Json(new { Result = result, Message = message, LeaveId = leaveId }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    result = 0;
                    message = ex.Message;
                    return Json(new { Result = result, Message = message.Length == 0 ? "Failed to save data." : message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        #endregion

        #region Http Requests Common

        public JsonResult getDepartmentWiseEmployee(int? DepartmentId, long? EmployeeId)
        {
            int result = 0;
            string message = "";
            object data = "";

            try
            {
                if (DepartmentId > 0 && EmployeeId > 0)
                {

                    var param = new { DepartmentId = DepartmentId };
                    var empList = employeeSPService.GetDataWithParameter(param, "leave.SP_GetAllEmployeeByDepartment");

                    var replaseEmployeeList = empList.Tables[0].AsEnumerable()
                    .Select(row => new EmployeeViewModel
                    {
                        EmployeeId = row.Field<long>("EmployeeId"),
                        EmployeeName = row.Field<string>("EmployeeName"),
                        EmployeeNameBng = row.Field<string>("EmployeeNameBng"),
                    }).ToList();

                    data = replaseEmployeeList;
                }
                result = 1;
            }
            catch (Exception e)
            {
                result = 0;
                message = e.Message;
            }
            return Json(new { result = result, message = message, data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMaxLeaveDay(string leaveTypeId, string employeeId, string UpTodate)
        {
            var typeId = Convert.ToInt32(leaveTypeId);
            var empId = Convert.ToInt32(employeeId);
            var leaveHistory = new LeaveHistoryViewModel();
            var leaveType = leaveTypeService.GetById(typeId);
            var LeaveAmountHistory = new DataSet();

            var param = new { LeaveTypeId = typeId, EmployeeId = empId };
            if (leaveType.LeaveCategory == LeaveCategoryConstants.Casual)
            {
                var param1 = new { LeaveTypeId = typeId, EmployeeId = empId, Year = UpTodate == "" ? DateTime.Now.Year : Convert.ToDateTime(UpTodate).Year };
                LeaveAmountHistory = employeeSPService.GetDataWithParameter(param1, "leave.SP_GetLeaveBalance_CL");
            }
            else if (leaveType.LeaveCategory == LeaveCategoryConstants.Annual_EL)
            {
                LeaveAmountHistory = employeeSPService.GetDataWithParameter(param, "leave.SP_GetLeaveBalance_AL");
            }
            else if (leaveType.LeaveCategory == LeaveCategoryConstants.Maternity)
            {
                LeaveAmountHistory = employeeSPService.GetDataWithParameter(param, "leave.SP_GetLeaveBalance_ML");
            }
            else if (leaveType.LeaveCategory == LeaveCategoryConstants.Paternity)
            {
                LeaveAmountHistory = employeeSPService.GetDataWithParameter(param, "leave.SP_GetLeaveBalance_PL");
            }
            else if (leaveType.LeaveCategory == LeaveCategoryConstants.Medical)
            {
                var param1 = new { LeaveTypeId = typeId, EmployeeId = empId, Year = UpTodate == "" ? DateTime.Now.Year : Convert.ToDateTime(UpTodate).Year };
                LeaveAmountHistory = employeeSPService.GetDataWithParameter(param1, "leave.SP_GetLeaveBalance_MEL");
            }
            else if (leaveType.LeaveCategory == LeaveCategoryConstants.Annual_EL_Laps)
            {
                var param1 = new { LeaveTypeId = typeId, EmployeeId = empId, Year = UpTodate == "" ? DateTime.Now.Year : Convert.ToDateTime(UpTodate).Year };
                LeaveAmountHistory = employeeSPService.GetDataWithParameter(param1, "leave.SP_GetLeaveBalance_AL_Laps");
            }
            else if (leaveType.LeaveCategory == LeaveCategoryConstants.SickLeave)
            {
                var paramSL = new { LeaveTypeId = typeId, EmployeeId = empId, Year = UpTodate == "" ? DateTime.Now.Year : Convert.ToDateTime(UpTodate).Year };

                LeaveAmountHistory = employeeSPService.GetDataWithParameter(paramSL, "leave.SP_GetLeaveBalance_SL");
            }
            else
            {
                LeaveAmountHistory = employeeSPService.GetDataWithParameter(param, "leave.SP_GetLeaveBalance_OL");
            }

            if (LeaveAmountHistory != null)
            {
                leaveHistory.TotalDays = Convert.ToDecimal(LeaveAmountHistory.Tables[0].Rows[0]["TotalLeave"].ToString());
                leaveHistory.LeaveCount = Convert.ToDouble(LeaveAmountHistory.Tables[0].Rows[0]["LeaveTaken"].ToString());
                leaveHistory.leaveGain = Convert.ToDouble(LeaveAmountHistory.Tables[0].Rows[0]["CurrentBalance"].ToString());
                leaveHistory.MaxAvailDays = Convert.ToInt32(LeaveAmountHistory.Tables[0].Rows[0]["MaxAvailDays"].ToString());
                leaveHistory.LeaveCategory = leaveType.LeaveCategory;
            }
            else
            {
                leaveHistory.TotalDays = 0;
                leaveHistory.LeaveCount = 0;
                leaveHistory.leaveGain = 0;
                leaveHistory.MaxAvailDays = 0;
                leaveHistory.LeaveCategory = "";
            }
            return Json(leaveHistory, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Leave Entry and Backdate LeaveEntry

        public JsonResult GetEmpInfo(string employee_Code)
        {
            try
            {
                var employee = employeeService.GetByCode(employee_Code);
                var approvers = leaveApproversAuthorityService.GetAll().ToList();

                if (!approvers.Any())
                {
                    var param1 = new { desinationID = employee.EmployeeRank, employeeId = employee.EmployeeId };
                    var leaveApproversConfig = employeeSPService.GetDataWithParameter(param1, "leave.SP_LeaveApproversIndividualUpdate");
                }

                List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();
                var param2 = new { EmployeeCode = employee_Code, OfficeId = SessionHelper.LoginUserOfficeID };
                var empOffcDesigList = employeeSPService.GetDataWithParameter(param2, "emp.SP_GetOfficewiseEmployee");

                //List_EmployeeViewModel = new ConvertDataTabletoList().ConvertToList<EmployeeViewModel>(empOffcDesigList.Tables[0]).ToList();

                List_EmployeeViewModel = empOffcDesigList.Tables[0].AsEnumerable()
               .Select(row => new EmployeeViewModel
               {
                   EmployeeId = row.Field<long>("EmployeeId"),
                   EmployeeRank = row.Field<string>("EmployeeRank"),
                   OffcDesignName = row.Field<string>("SignatureName") == null ? row.Field<string>("OffcDesignName") : row.Field<string>("SignatureName"),
                   EmployeeName = row.Field<string>("EmployeeName"),
                   EmployeeCode = row.Field<string>("EmployeeCode"),
                   OfficeId = row.Field<int>("OfficeId"),
                   OfficeName = row.Field<string>("OfficeName"),
                   DesignationName = row.Field<string>("DesignationName"),
                   DepartmentName = row.Field<string>("DepartmentName"),
                   DepartmentId = row.Field<int>("DepartmentId"),
                   EmployeeStatus = row.Field<string>("EmployeeStatus").Trim(),
                   EmployeeStatusId = row.Field<int>("EmployeeStatusId"),
                   Gender = row.Field<string>("Gender"),
                   Adjustment = row.Field<int>("Adjustment"),
                   ValidApproverCount = CountValidLeaveApproverForEmployee(row.Field<long>("EmployeeId"))
               }).ToList();

                return Json(new { result = 1, List_EmployeeViewModel = List_EmployeeViewModel }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


         public JsonResult GetEmpInfo2(string employee_Code)
        {
            try
            {
                var employee = employeeService.GetByCode(employee_Code);
                var approvers = leaveApproversAuthorityService.GetAll().ToList();



                List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();
                var param2 = new { EmployeeCode = employee_Code, OfficeId = SessionHelper.LoginUserOfficeID };
                var empOffcDesigList = employeeSPService.GetDataWithParameter(param2, "emp.SP_GetOfficewiseEmployee");

                //List_EmployeeViewModel = new ConvertDataTabletoList().ConvertToList<EmployeeViewModel>(empOffcDesigList.Tables[0]).ToList();

                List_EmployeeViewModel = empOffcDesigList.Tables[0].AsEnumerable()
               .Select(row => new EmployeeViewModel
               {
                   EmployeeId = row.Field<long>("EmployeeId"),
                   EmployeeRank = row.Field<string>("EmployeeRank"),
                   OffcDesignName = row.Field<string>("SignatureName") == null ? row.Field<string>("OffcDesignName") : row.Field<string>("SignatureName"),
                   EmployeeName = row.Field<string>("EmployeeName"),
                   EmployeeCode = row.Field<string>("EmployeeCode"),
                   OfficeId = row.Field<int>("OfficeId"),
                   OfficeName = row.Field<string>("OfficeName"),
                   DesignationName = row.Field<string>("DesignationName"),
                   DepartmentName = row.Field<string>("DepartmentName"),
                   DepartmentId = row.Field<int>("DepartmentId"),
                   EmployeeStatus = row.Field<string>("EmployeeStatus").Trim(),
                   EmployeeStatusId = row.Field<int>("EmployeeStatusId"),
                   Gender = row.Field<string>("Gender"),
                   Adjustment = row.Field<int>("Adjustment"),
                   ValidApproverCount = CountValidLeaveApproverForEmployee(row.Field<long>("EmployeeId"))
               }).ToList();

                return Json(new { result = 1, List_EmployeeViewModel = List_EmployeeViewModel }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetLeaveType(string gender, int empStatusId)
        {
            if (gender == "M")
            {
                var LeaveTypeList = leaveTypeService.GetAll().Where(l => l.LeaveGender != "F" && l.EmployeeStatusId == empStatusId).OrderBy(o => o.LeaveTypeName);
                var viewLeaveTypeList = LeaveTypeList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.LeaveTypeId.ToString(),
                    Text = x.LeaveTypeName.ToString() + "~" + x.LeaveCategory.ToString()
                });
                var leaveType_items = new List<SelectListItem>();
                leaveType_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                leaveType_items.AddRange(viewLeaveTypeList);
                return Json(new { Data = leaveType_items }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var LeaveTypeList = leaveTypeService.GetAll().Where(l => l.LeaveGender != "M" && l.EmployeeStatusId == empStatusId).OrderBy(o => o.LeaveTypeName);
                var viewLeaveTypeList = LeaveTypeList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.LeaveTypeId.ToString(),
                    Text = x.LeaveTypeName.ToString() + "~" + x.LeaveCategory.ToString()
                });
                var leaveType_items = new List<SelectListItem>();
                leaveType_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                leaveType_items.AddRange(viewLeaveTypeList);
                return Json(new { Data = leaveType_items }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetLeaveHistorytList(int jtStartIndex, int jtPageSize, string jtSorting, string employeeId)
        {
            try
            {
                double TotCount;
                long empId = Convert.ToInt64(string.IsNullOrEmpty(employeeId) ? "0" : employeeId);

                var employeeLeaveHistory = leaveHistoryService.GetLeaveHistoryByEmployee(empId, jtStartIndex, jtSorting, jtPageSize, out TotCount).ToList();

                List<DBLeaveHistoryModel> detailList = new List<DBLeaveHistoryModel>();

                foreach (var app in employeeLeaveHistory)
                {
                    var leaveHistoryView = new DBLeaveHistoryModel() { LeaveId = app.LeaveId, LeaveTypeName = app.LeaveTypeName, TotalDays = app.TotalDays, TotalAvailableDays = app.TotalAvailableDays, LeaveStartDateMsg = Convert.ToDateTime(app.LeaveStartDate).ToString("dd-MMM-yyyy"), LeaveEndDateMsg = Convert.ToDateTime(app.LeaveEndDate).ToString("dd-MMM-yyyy"), LeaveReason = app.LeaveReason, EmployeeId = app.EmployeeId, LeaveDayDuration = app.LeaveDayDuration };
                    detailList.Add(leaveHistoryView);
                }
                var currentPageRecords = detailList.ToList();
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                var innerExp = ex.InnerException;
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetLeaveList(int jtStartIndex, int jtPageSize, string jtSorting, string employeeId)
        {
            try
            {
                long TotCount;
                long empId = Convert.ToInt64(string.IsNullOrEmpty(employeeId) ? "0" : employeeId);

                var EmployeeLeaveDetails = leaveHistoryService.GetLeaveByEmployee(empId, jtStartIndex, jtSorting, jtPageSize, out TotCount);
                var detail = EmployeeLeaveDetails.ToList().Where(x => x.EmployeeId == empId);

                List<DBLeaveModel> detailList = new List<DBLeaveModel>();

                foreach (var app in detail)
                {
                    var leaveView = new DBLeaveModel() { EmployeeId = app.EmployeeId, LeaveId = app.LeaveId, LeaveTypeName = app.LeaveTypeName, TotalDays = app.TotalDays, LeaveStartDateMsg = Convert.ToDateTime(app.LeaveStartDate).ToString("dd-MMM-yyyy"), LeaveEndDateMsg = Convert.ToDateTime(app.LeaveEndDate).ToString("dd-MMM-yyyy"), LeaveReason = app.LeaveReason, AddressDuringLeave = app.AddressDuringLeave, LeaveDayDuration = app.LeaveDayDuration };
                    detailList.Add(leaveView);
                }

                var currentPageRecords = detailList.ToList();

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public JsonResult GetLeaveList2(int jtStartIndex, int jtPageSize, string jtSorting, string employeeId)
        {
            try
            {
                long TotCount;
                long empId = Convert.ToInt64(string.IsNullOrEmpty(employeeId) ? "0" : employeeId);

                var EmployeeLeaveDetails = leaveHistoryService.GetLeaveByEmployee2(empId, jtStartIndex, jtSorting, jtPageSize, out TotCount);
                var detail = EmployeeLeaveDetails.ToList().Where(x => x.EmployeeId == empId);

                List<DBLeaveModel> detailList = new List<DBLeaveModel>();

                foreach (var app in detail)
                {
                    var leaveView = new DBLeaveModel() { EmployeeId = app.EmployeeId, LeaveId = app.LeaveId, LeaveTypeName = app.LeaveTypeName, TotalDays = app.TotalDays, LeaveStartDateMsg = Convert.ToDateTime(app.LeaveStartDate).ToString("dd-MMM-yyyy"), LeaveEndDateMsg = Convert.ToDateTime(app.LeaveEndDate).ToString("dd-MMM-yyyy"), LeaveReason = app.LeaveReason, AddressDuringLeave = app.AddressDuringLeave, LeaveDayDuration = app.LeaveDayDuration };
                    detailList.Add(leaveView);
                }

                var currentPageRecords = detailList.ToList();

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult getValidLeaveDate(string EmpId, string StartDt, string EndDt)
        {
            string Result = "0";
            var EmployeeId = Convert.ToInt64(EmpId);
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            if (StartDt != "")
            {
                startDate = Convert.ToDateTime(StartDt).Date;
            }
            else
            {
                if (EndDt != "")
                {
                    startDate = Convert.ToDateTime(EndDt).Date;
                }
            }
            if (EndDt != "")
            {
                endDate = Convert.ToDateTime(EndDt).Date;
            }
            else
            {
                endDate = startDate;
            }

            var ifDateAcceptable = leaveHistoryService.GetMany(x => x.IsActive == true && x.IsApproved == true && x.EmployeeId == EmployeeId && ((x.LeaveStartDate <= startDate && x.LeaveEndDate >= startDate) || (x.LeaveStartDate <= endDate && x.LeaveEndDate >= endDate) || (startDate <= x.LeaveStartDate && endDate >= x.LeaveEndDate))).ToList();

            if (ifDateAcceptable.Count == 0)
            {
                var filter = new LeaveHistorySearchFilter { EmployeeId = (int)EmployeeId, StartDate = startDate, EndDate = endDate };
                var isReplaceEmployee = leaveHistoryService.GetLeaveHistoriesByFilter(filter);
                //.Get(x => x.IsActive == true && x.ReplacementEmployee == EmployeeId && ((x.LeaveStartDate <= startDate && x.LeaveEndDate >= startDate) || (x.LeaveStartDate <= endDate && x.LeaveEndDate >= endDate) || (startDate <= x.LeaveStartDate && endDate >= x.LeaveEndDate)));
                if (!isReplaceEmployee.Any())
                {
                    Result = "1";//valid 
                }
                else
                {
                    Result = "2";// assigned as replacement employee
                }
            }
            else
            {
                Result = "0";
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getValidLeaveDate2(string EmpId, string StartDt, string EndDt)
        {
            string Result = "0";
            var EmployeeId = Convert.ToInt64(EmpId);
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            if (StartDt != "")
            {
                startDate = Convert.ToDateTime(StartDt).Date;
            }
            else
            {
                if (EndDt != "")
                {
                    startDate = Convert.ToDateTime(EndDt).Date;
                }
            }
            if (EndDt != "")
            {
                if(EndDt == null)
                {
                    endDate = startDate;
                }
                else
                {
                    endDate = Convert.ToDateTime(StartDt).Date.AddDays(3);
                }
                    
            }
            else
            {
                endDate = startDate;
            }

            var ifDateAcceptable = leaveHistoryService.GetMany(x => x.IsActive == true && x.IsApproved == true && x.EmployeeId == EmployeeId && ((x.LeaveStartDate <= startDate && x.LeaveEndDate >= startDate) || (x.LeaveStartDate <= endDate && x.LeaveEndDate >= endDate) || (startDate <= x.LeaveStartDate && endDate >= x.LeaveEndDate))).ToList();

            if (ifDateAcceptable.Count == 0)
            {
                var filter = new LeaveHistorySearchFilter { EmployeeId = (int)EmployeeId, StartDate = startDate, EndDate = endDate };
                var isReplaceEmployee = leaveHistoryService.GetLeaveHistoriesByFilter(filter);
                //.Get(x => x.IsActive == true && x.ReplacementEmployee == EmployeeId && ((x.LeaveStartDate <= startDate && x.LeaveEndDate >= startDate) || (x.LeaveStartDate <= endDate && x.LeaveEndDate >= endDate) || (startDate <= x.LeaveStartDate && endDate >= x.LeaveEndDate)));
                if (!isReplaceEmployee.Any())
                {
                    Result = "1";//valid 
                }
                else
                {
                    Result = "1";// assigned as replacement employee
                }
            }
            else
            {
                Result = "0";
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult leaveDelete(string LeaveId)
        {
            var entity = leaveHistoryService.GetLeaveHistoryById(Convert.ToInt64(LeaveId));
            string Result = "";
            using (TransactionScope Scope = new TransactionScope())
            {
                if (ModelState.IsValid)
                {
                    entity.IsActive = false;
                    entity.InActiveDate = DateTime.Now;
                    entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                    entity.UpdateDate = DateTime.Now;
                    leaveHistoryService.Update(entity);

                    var applicationId = Convert.ToInt64(LeaveId);
                    var notificationList = approvalNotificationService.GetMany(x => x.IsActive == true && x.ApplicationId == applicationId);
                    foreach (var item in notificationList)
                    {
                        item.IsActive = false;
                        item.UpdateDate = DateTime.UtcNow;
                        item.UpdateUser = LoggedInEmployeeId;
                        approvalNotificationService.Update(item);
                    }

                    Result = "OK";
                    Scope.Complete();
                }
                else
                {
                    Result = "";
                    Scope.Dispose();
                }
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Leave Adjust

        public JsonResult GetLeaveAdjustInfo(string dispatchLeaveId)
        {
            try
            {
                List<LeaveHistoryViewModel> List_LeaveHistoryViewModel = new List<LeaveHistoryViewModel>();
                var param = new { DispatchLeaveId = Convert.ToInt64(dispatchLeaveId) };
                var empOffcDesigList = employeeSPService.GetDataWithParameter(param, "leave.SP_GetLeaveAdjustInfo");
                List_LeaveHistoryViewModel = empOffcDesigList.Tables[0].AsEnumerable()
               .Select(row => new LeaveHistoryViewModel
               {
                   LeaveId = row.Field<long>("LeaveId"),
                   LeaveTypeId = row.Field<int>("LeaveTypeId"),
                   LeaveTypeName = row.Field<string>("LeaveTypeName"),
                   LeaveStartDateMsg = row.Field<string>("LeaveStartDate"),
                   LeaveEndDateMsg = row.Field<string>("LeaveEndDate"),
                   TotalDays = row.Field<decimal?>("TotalDays"),
                   LeaveRecommendation = row.Field<string>("LeaveRecommendation"),
                   LeaveNote = row.Field<string>("LeaveNote"),
                   LeaveHeader = row.Field<string>("LeaveHeader"),
                   LeaveFooter = row.Field<string>("LeaveFooter"),
                   chkRecommendation = row.Field<string>("IsRecommendation"),
                   chkEvidence = row.Field<string>("IsEvidence"),
                   leaveDispatchRemarks = row.Field<string>("leaveDispatchRemarks"),
                   Mode = row.Field<string>("Mode"),
                   JoinDateMsg = row.Field<string>("JoinDateMsg"),
                   ReplacementEmployee = row.Field<long?>("ReplacementEmployee"),
                   DepartmentId = (int)employeeService.GetByEmpId(row.Field<long>("EmployeeId")).DepartmentId
               }).ToList();
                return Json(List_LeaveHistoryViewModel, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LeaveJoiningDate(long LeaveId, string JoiningDate)
        {
            int result = 0; string message = "";
            try
            {
                var joinDate = Convert.ToDateTime(JoiningDate);
                var list = leaveHistoryService.GetAll().Where(b => b.LeaveId == LeaveId || b.DispatchLeaveId == LeaveId);
                foreach (var d in list)
                {
                    d.JoinDate = joinDate;
                    d.UpdateDate = DateTime.Now;
                    d.UpdateUser = LoggedInEmployeeId;
                    leaveHistoryService.Update(d);
                }
                result = 1;
                message = "Successfully Updated.";
            }
            catch (Exception e)
            {
                result = 0;
                message = e.Message;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }



        public JsonResult CheckJoiningDate(long LeaveId, string JoiningDate)
        {
            var data = 0;
            var message = "";
            long employeeId = 0;

            var leaveDetail = leaveHistoryService.GetById(Convert.ToInt32(LeaveId));
            if (leaveDetail != null)
            {
                employeeId = leaveDetail.EmployeeId;
            }

            var checkAllUnAdjustedLeave = leaveHistoryService.GetMany(p => p.IsActive == true && p.IsAdjustment == false && (p.LeaveId == LeaveId || p.DispatchLeaveId == LeaveId)).ToList();

            if (checkAllUnAdjustedLeave.Any())
            {
                var leaveEndDate = checkAllUnAdjustedLeave.Max(p => p.LeaveEndDate);

                var joiningDate = Convert.ToDateTime(JoiningDate);

                var dayDifference = (joiningDate - leaveEndDate).TotalDays;

                if (dayDifference > 1)
                {
                    for (int i = 1; i < dayDifference; i++)
                    {
                        var chkDate = leaveEndDate.AddDays(i);
                        var isInHolidayList = attHolidayDeclarationService.GetAll().Where(x => x.IsActive == true && x.HolidayDate == chkDate).ToList();
                        if (isInHolidayList.Count > 0)
                        {
                            data = 1;//in holiday
                        }
                        else
                        {
                            data = 0;//in holiday
                            message = "There is a working day " + Convert.ToDateTime(chkDate).ToString("dd-MMM-yyyy") + " before given joining date. You need to adjust this leave.";
                            break;
                        }
                    }

                    var chkDate22 = joiningDate;
                    //var employeeId = leaveHistoryService.GetById(Convert.ToInt32(LeaveId)).EmployeeId;
                    var officeId22 = employeeService.GetById(Convert.ToInt32(employeeId)).OfficeId;
                    var isInHolidayList22 = attHolidayDeclarationService.GetAll().Where(x => x.IsActive == true && x.HolidayDate == chkDate22 && x.OfficeId == officeId22).ToList();
                    if (isInHolidayList22.Count > 0)
                    {
                        data = 0;//joining date less than end date
                        message = "Joining day can't be a holiday";
                    }
                    else
                    {
                        data = 1;
                    }


                }

                else if (dayDifference < 0)
                {
                    data = 0;//joining date less than end date
                    message = "Joining date can't be less than leave end date";
                }

                else if (dayDifference == 0)
                {
                    data = 0;//joining date less than end date
                    message = "Joining date can't be same with leave end date";
                }

                else if (dayDifference == 1)
                {
                    var chkDate = joiningDate;
                    //var employeeId = leaveHistoryService.GetById(Convert.ToInt32(LeaveId)).EmployeeId;
                    var officeId = employeeService.GetById(Convert.ToInt32(employeeId)).OfficeId;
                    var isInHolidayList = attHolidayDeclarationService.GetAll().Where(x => x.IsActive == true && x.HolidayDate == chkDate && x.OfficeId == officeId).ToList();
                    if (isInHolidayList.Count > 0)
                    {
                        data = 0;//joining date less than end date
                        message = "Joining day can't be a holiday";
                    }
                    else
                    {
                        data = 1;
                    }
                }
            }

            return Json(new { data = data, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckJoiningDate2(long LeaveId, string JoiningDate)
        {
            var data = 0;
            var message = "";
            long employeeId = 0;

            var leaveDetail = leaveHistoryService.GetById(Convert.ToInt32(LeaveId));
            if (leaveDetail != null)
            {
                employeeId = leaveDetail.EmployeeId;
            }

            var checkAllUnAdjustedLeave = leaveHistoryService.GetMany(p => p.IsActive == true && p.IsAdjustment == false && (p.LeaveId == LeaveId || p.DispatchLeaveId == LeaveId)).ToList();

            if (checkAllUnAdjustedLeave.Any())
            {
                var leaveEndDate = checkAllUnAdjustedLeave.Max(p => p.LeaveEndDate);

                var joiningDate = Convert.ToDateTime(JoiningDate);

                var dayDifference = (joiningDate - leaveEndDate).TotalDays;

                if (dayDifference > 1)
                {
                    for (int i = 1; i < dayDifference; i++)
                    {
                        var chkDate = leaveEndDate.AddDays(i);
                        var isInHolidayList = attHolidayDeclarationService.GetAll().Where(x => x.IsActive == true && x.HolidayDate == chkDate).ToList();
                        if (isInHolidayList.Count > 0)
                        {
                            data = 0;//in holiday
                            message = "Joining day can't be a holiday";
                        }
                        else
                        {
                            data = 0;//in holiday
                            message = "There is a working day " + Convert.ToDateTime(chkDate).ToString("dd-MMM-yyyy") + " before given joining date. You need to adjust this leave.";
                            break;
                        }
                    }
                }

                else if (dayDifference < 0)
                {
                    data = 0;//joining date less than end date
                    message = "Joining date can't be less than leave end date";
                }

                else if (dayDifference == 0)
                {
                    data = 0;//joining date less than end date
                    message = "Joining date can't be same with leave end date";
                }

                else if (dayDifference == 1)
                {
                    var chkDate = joiningDate;
                    //var employeeId = leaveHistoryService.GetById(Convert.ToInt32(LeaveId)).EmployeeId;
                    var officeId = employeeService.GetById(Convert.ToInt32(employeeId)).OfficeId;
                    var isInHolidayList = attHolidayDeclarationService.GetAll().Where(x => x.IsActive == true && x.HolidayDate == chkDate && x.OfficeId == officeId).ToList();
                    if (isInHolidayList.Count > 0)
                    {
                        data = 0;//joining date less than end date
                        message = "Joining day can't be a holiday";
                    }
                    else
                    {
                        data = 1;
                    }
                }
            }

            return Json(new { data = data, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult leaveAdjustEdit(string LeaveId, string LeaveTypeId, string LeaveEndDate, string LeaveStartDate, string TotalDays, string LeaveNote, string LeaveHeader, string LeaveFooter, string LeaRequeestdate)
        {
            try
            {
                var param = new
                {
                    LeaveId = Convert.ToInt64(LeaveId),
                    LeaveTypeId = Convert.ToInt32(LeaveTypeId),
                    LeaveEndDate = Convert.ToDateTime(LeaveEndDate),
                    TotalDays = Convert.ToInt32(TotalDays),
                    LeaveNote = LeaveNote,
                    LeaveHeader = LeaveHeader,
                    LeaveFooter = LeaveFooter,
                    LeaveRequestDate = Convert.ToDateTime(LeaRequeestdate)
                };
                var OverdueMls = employeeSPService.GetDataWithParameter(param, "leave.SP_SetLeaveAdjustment");

                var result = 1;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LeaveAdjustFinalSave(string Leave_Id, string JoiningDate)
        {
            int result = 0;
            string message = "";
            long employeeId = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var leaveCategorys = leaveTypeService.GetAll().Where(p => p.IsActive == true).ToList();
                    var leaveHistoryDetail = leaveHistoryService.GetAll().Where(p => p.IsActive == true && p.IsAdjustment == false && (p.LeaveId == Convert.ToInt64(Leave_Id) || p.DispatchLeaveId == Convert.ToInt64(Leave_Id))).ToList();

                    foreach (var item in leaveHistoryDetail)
                    {
                        var leaveCategory = leaveCategorys.Where(p => p.LeaveTypeId == item.LeaveTypeId).FirstOrDefault() == null ? "" : leaveCategorys.Where(p => p.LeaveTypeId == item.LeaveTypeId).FirstOrDefault().LeaveCategory;
                        employeeId = item.EmployeeId;
                        long LeaveId = Convert.ToInt64(item.LeaveId);

                        if (leaveCategory == "LWP")
                        {
                            //result = LWPSaveAndSalaryDeduct(out message, employeeId, LeaveId, JoiningDate);
                            //if (result == 0)
                            //{
                            //    scope.Dispose();
                            //    return Json(new { result = result, employeeId = employeeId, message = message }, JsonRequestBehavior.AllowGet);
                            //}

                            var param = new { LeaHistoryId = LeaveId, JoiningDate = Convert.ToDateTime(JoiningDate).Date };
                            var empOffcDesigList = employeeSPService.GetDataWithParameter(param, "leave.SP_UpdateLeaveHistory");
                            result = 1;
                            message = "Leave adjusted successfully";
                        }
                        else
                        {
                            var param = new { LeaHistoryId = LeaveId, JoiningDate = Convert.ToDateTime(JoiningDate).Date };
                            var empOffcDesigList = employeeSPService.GetDataWithParameter(param, "leave.SP_UpdateLeaveHistory");
                            result = 1;
                            message = "Leave adjusted successfully";
                        }
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    result = 0;
                    message = "Leave adjustment failed";
                    scope.Dispose();
                }
            }
            return Json(new { result = result, employeeId = employeeId, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult leaveAdjustAdd(string leatypeid, string StartDate, string EndDate, string TotalDays, string EmployeeId, string LeaveId, string LeaveNote, string LeaveHeader, string LeaveFooter, string LeaRequeestdate, int ReplacementEmployee)
        {
            try
            {
                long result = 0;
                var message = "";
                bool chkReco = false;
                bool chkEvi = false;
                int LeaveTypeId = Convert.ToInt32(leatypeid);

                var existingLeaveAdjustList = leaveHistoryService.GetAll().Where(x => x.IsActive == true && x.IsAdjustment == false && (x.LeaveId == Convert.ToInt64(LeaveId) || x.DispatchLeaveId == Convert.ToInt64(LeaveId))).ToList();

                if (existingLeaveAdjustList.Count > 0)
                {
                    var totalLeaveDays = 0;
                    foreach (var leave in existingLeaveAdjustList)
                    {
                        totalLeaveDays = totalLeaveDays + Convert.ToInt32(leave.TotalDays);
                    }
                    totalLeaveDays += Convert.ToInt32(TotalDays);
                    var leaveConfiguration = leaveTypeService.GetById(LeaveTypeId);
                    var maxLeaveAvailDays = leaveConfiguration.MaxAvailDays;
                    var leaveType = leaveConfiguration.LeaveTypeName;
                    if (totalLeaveDays > maxLeaveAvailDays)
                    {
                        result = 0;
                        message = "Cant apply " + leaveType + " for more than " + maxLeaveAvailDays + " days at a time.";
                    }
                    else
                    {
                        result = 1;
                    }
                }

                if (result == 1)
                {
                    LeaveHistory LeaveHistory = new LeaveHistory
                    {
                        EmployeeId = Convert.ToInt64(EmployeeId),
                        LeaveTypeId = Convert.ToInt32(leatypeid),
                        LeaveStartDate = Convert.ToDateTime(StartDate),
                        LeaveEndDate = Convert.ToDateTime(EndDate),
                        TotalDays = Convert.ToInt32(TotalDays),
                        IsAdjustment = false,
                        AdjustmentBy = SessionHelper.LoggedInEmployeeID,
                        DispatchLeaveId = Convert.ToInt64(LeaveId),
                        AdjustmentDate = DateTime.Now,
                        IsRecommendation = chkReco,
                        IsEvidence = chkEvi,
                        LeaveNote = LeaveNote,
                        LeaveHeader = LeaveHeader,
                        LeaveFooter = LeaveFooter,
                        IsActive = true,
                        CreateUser = SessionHelper.LoggedInEmployeeID,
                        CreateDate = DateTime.Now,
                        IsApproved = true,
                        LeaveRequestDate = Convert.ToDateTime(LeaRequeestdate),
                        ReplacementEmployee = ReplacementEmployee
                    };

                    leaveHistoryService.Create(LeaveHistory);
                    result = LeaveHistory.LeaveId;
                    result = 1;
                    message = "Leave Adjustment Added Successfully";
                    return Json(new { Result = result, Message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = 0;
                    message = message == "" ? "Leave Adjustment Failed" : message;
                    return Json(new { Result = result, Message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult LeaveAdjustDelete(string LeaveId)
        {
            var result = 0;
            var leave = leaveHistoryService.GetLeaveHistoryById(Convert.ToInt64(LeaveId));
            leave.IsActive = false;
            leave.IsAdjustment = true;
            leaveHistoryService.Update(leave);
            result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion leaveAdjust

        #region Approve

        public JsonResult GetLeaveForAdjustList([DataSourceRequest] DataSourceRequest request, string Qtype)
        {
            try
            {
                List<LeaveHistoryViewModel> List_LeaveHistoryViewModel = new List<LeaveHistoryViewModel>();

                var empId = (long)LoggedInEmployeeId;
                var ifExists = leaveAdjustmentAuthorityService.IsExistLeaveAdjustmentAuthority(empId);
                if (!ifExists)
                    return Json(new { Result = "ERROR", Message = "Leave adjustment authority not found." });

                StringBuilder sb = new StringBuilder();

                if (!String.IsNullOrEmpty(Qtype) && Qtype == LeaveAdjustTypeConstants.NonAdjust)
                {
                    sb.Append("AND L.IsActive = 1 AND L.IsApproved=1 AND L.IsAdjustment=0 ");
                }

                if (!String.IsNullOrEmpty(Qtype) && Qtype == LeaveAdjustTypeConstants.Adjusted)
                {
                    sb.Append("AND L.IsActive = 1 AND L.IsApproved=1 AND L.IsAdjustment=1 ");
                }

                if (!String.IsNullOrEmpty(Qtype) && Qtype == LeaveAdjustTypeConstants.Reject)
                {
                    sb.Append("AND L.IsActive = 0 AND L.IsApproved=0 AND L.IsAdjustment=1");
                }

                var param = new { AndCondition = sb.ToString(), SearchType = Qtype };
                //get leave history listing from [leave.LeaveHistory]
                var leaveAdjustInfo = employeeSPService.GetDataWithParameter(param, "leave.SP_GetLeaveForAdjustListNew");

                List_LeaveHistoryViewModel = leaveAdjustInfo.Tables[0].AsEnumerable()
                      .Select(row => new LeaveHistoryViewModel
                      {
                          Rowsl = row.Field<string>("Rowsl"),
                          LeaveId = row.Field<long>("LeaveId"),
                          LeaveTypeId = row.Field<int>("LeaveTypeId"),
                          EmployeeId = row.Field<long>("EmployeeId"),
                          EmployeeCode = row.Field<string>("EmployeeCode"),
                          EmployeeName = row.Field<string>("EmployeeName"),
                          DesignationName = row.Field<string>("DesignationName"),
                          LeaveReason = row.Field<string>("LeaveReason"),
                          LeaveTypeName = row.Field<string>("LeaveTypeName"),
                          LeaveStartDateMsg = row.Field<string>("LeaveStartDateMsg"),
                          LeaveEndDateMsg = row.Field<string>("LeaveEndDateMsg"),
                          TotalDays = row.Field<int>("TotalDays"),
                          AddressDuringLeave = row.Field<string>("AddressDuringLeave"),
                          comment = row.Field<string>("comment"),
                          DispatchLeaveId = row.Field<long>("DispatchLeaveId")
                      }).ToList();

                DataSourceResult result = List_LeaveHistoryViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult deleteApprovedLeave(int LeaveId)
        {
            var result = 0;
            try
            {
                var entity = leaveHistoryService.GetById(Convert.ToInt32(LeaveId));

                entity.IsActive = false;
                entity.IsApproved = false;
                entity.IsAdjustment = true;
                entity.Remarks = "Leave Deleted After Approval";
                entity.InActiveDate = DateTime.Now;
                entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId);
                entity.UpdateDate = DateTime.Now;

                //let's update leave history as inactive to make delete in [leave.LeaveHistory]
                leaveHistoryService.Update(entity);
                result = 1;
            }
            catch (Exception e)
            {
                result = 0;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Report

        public ActionResult GenerateRequestLeaveReport(string EmployeeId, string leaveId)
        {
            try
            {
                bool LeaveApproved = true;
                var param = new { EmployeeId = Convert.ToInt64(EmployeeId), LeaveId = Convert.ToInt64(leaveId), LeaveApproved = LeaveApproved, IsAdjustment = LeaveApproved };
                var OverdueMls = employeeSPService.GetDataWithParameter(param, "leave.SP_GetLeaveApprovalDetails");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Leave/Rpt_GetLeaveApprovalReport.rpt", OverdueMls.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        // Test 


        public ActionResult GenerateLeaveApplicationReportCal(
          string LeaveId, string OfficeTypeId, string OfficeId, string DesignationId, string ResponsibilityId, string DeptId, string SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "LeaveId", Value = (string.IsNullOrEmpty(LeaveId) ? "0" : LeaveId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (string.IsNullOrEmpty(OfficeTypeId) ? "0" : OfficeTypeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (string.IsNullOrEmpty(OfficeId) ? "0" : OfficeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = (string.IsNullOrEmpty(DesignationId) ? "0" : DesignationId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = "1, 2, 3, 4, 5, 6, 7, 8, 9, 10" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = (string.IsNullOrEmpty(DeptId) ? "0" : DeptId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = (string.IsNullOrEmpty(SectionId) ? "0" : SectionId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = (string.IsNullOrEmpty(ResponsibilityId) ? "0" : ResponsibilityId) });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Date", Value = "2022-04-04" });
                PrintSSRSReport("/gHRMPlus_Reports/Test", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }








        public ActionResult FinalAdjustmentLeaveReport(string LeaveId, string EmployeeId, string adjusted, string QType)
        {
            long employeeId = 0;
            if (EmployeeId == null)
            {
                employeeId = leaveHistoryService.GetById(Convert.ToInt32(LeaveId)).EmployeeId;
            }
            else
            {
                employeeId = Convert.ToInt64(EmployeeId);
            }
            try
            {
                bool isAdjustment = true;
                bool isApproved = true;

                //if (adjusted == "Y")
                //{
                //    isAdjustment = true;
                //}
                //if (adjusted == "N")
                //{
                //    isAdjustment = false;
                //}
                if (QType == "A")
                {
                    isAdjustment = true;
                    isApproved = true;
                }
                if (QType == "R")
                {
                    isAdjustment = true;
                    isApproved = false;
                }


                var param = new { EmployeeId = Convert.ToInt64(employeeId), LeaveId = Convert.ToInt64(LeaveId), LeaveApproved = isApproved, IsAdjustment = isAdjustment };

                var OverdueMls = employeeSPService.GetDataWithParameter(param, "leave.SP_RPT_LeaveAdjustmentReport");
                var reportParam = new Dictionary<string, object>();
                if (QType == "R")
                    ReportHelper.PrintReport("Leave/Rpt_GetLeaveApprovalReportReject.rpt", OverdueMls.Tables[0], reportParam);

                ReportHelper.PrintReport("Leave/Rpt_GetLeaveApprovalReport.rpt", OverdueMls.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GenerateLeaveApplicationReport(int LeaveId)
        {
            try
            {
                var param = new { LeaveId = LeaveId };
                var MainReport = employeeSPService.GetDataWithParameter(param, "leave.SP_RPT_GetLeaveApplicationInfo");

                var param2 = new { DateFrom = DateTime.Now, EmployeeId = LoggedInEmployeeId };
                var subData = employeeSPService.GetDataWithParameter(param2, "leave.SP_RPT_GetLeaveTakenRemainingCount");

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("LeaveTakenRemaining", subData.Tables[0]);

                var reportParam = new Dictionary<string, object>();
                if (SessionHelper.CompanyCode == "GT")
                    ReportHelper.PrintWithSubReport("Leave/rpt_LeaveApplicationInfo_Forgt.rpt", MainReport.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_LeaveApplicationInfo_Forgt());
                else if(SessionHelper.CompanyCode == "GSSB")
                    ReportHelper.PrintWithSubReport("Leave/rpt_LeaveApplicationInfo_Forgt.rpt", MainReport.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_LeaveApplicationInfo_forGSSB());
                else
                    ReportHelper.PrintWithSubReport("Leave/rpt_LeaveApplicationInfo.rpt", MainReport.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_LeaveApplicationInfo());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult GenerateElAvailApproveReport(string leaveId, string LeaveTypeId)
        {
            try
            {
                if (LeaveTypeId == "1")
                {
                    var param = new { LeaveId = leaveId };
                    var OverdueMls = employeeSPService.GetDataWithParameter(param, "leave.SP_RPT_CasualApproved");
                    var reportParam = new Dictionary<string, object>();
                    ReportHelper.PrintReport("rpt_leave_casual_approved.rpt", OverdueMls.Tables[0], reportParam);
                    return Content(string.Empty);
                }
                else
                {
                    var param = new { LeaveId = leaveId };
                    var OverdueMls = employeeSPService.GetDataWithParameter(param, "leave.SP_RPT_ElAvailApproved");
                    var reportParam = new Dictionary<string, object>();
                    ReportHelper.PrintReport("ElApproved.rpt", OverdueMls.Tables[0], reportParam);
                    return Content(string.Empty);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult CalculateLeaveTotalDays(int LeaveTypeId, DateTime LeaveStartDate, DateTime LeaveEndDate, string leaveStatus)
        {
            try
            {
                string leaveStatFrom = leaveStatus??"full";
                decimal ls = (decimal)(leaveStatFrom.ToLower() == "full" ? 0.00 : 0.5);
                using (var db = new gHRMDBContext())
                {
                    var lst = db.Database.SqlQuery<decimal>("leave.SP_CalculateLeaveTotalDays_New " + LeaveTypeId + ",'" + LeaveStartDate + "','" + LeaveEndDate + "'," + ls + "");
                    if (lst.Any())
                    {
                        if (lst.First() > 0 && lst.First() < 1)
                            return Json(new { success = true, data = lst.First() });
                        else return Json(new { success = true, data = Convert.ToInt32(lst.First()) });
                    }
                    else return Json(new { success = false, message = "date check" });
                }
                //using (var DB = new gHRMDataAccess())
                //{
                //    DataSet _List = DB.GetDataOnDateset("leave.SP_CalculateLeaveTotalDays", new {
                //        LeaveTypeId = LeaveTypeId,
                //        StartDate = LeaveStartDate,
                //        EndDate = LeaveEndDate
                //        ,
                //        @leaveStatus
                //    });
                //return Json(new { success = true, data = Convert.ToInt32(_List.Tables[0].Rows[0]["TotalDays"]) });
                //}
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region Payroll Methods

        //private int LWPSaveAndSalaryDeduct(out string message, long employeeId, long LeaveId, string JoiningDate)
        //{
        //    int result = 0;
        //    var employeeEntity = employeeService.GetByEmpId(employeeId);
        //    var employeeGrossSalary = Convert.ToDecimal(employeeEntity.GrossSalary);
        //    if (employeeGrossSalary > 0)
        //    {
        //        decimal salaryDeductAmt = 0;
        //        var lwpConstraint = leaveHistoryService.GetAll().Where(x => x.IsActive == true && x.IsApproved == true && x.IsAdjustment == false && (x.LeaveId == LeaveId || x.DispatchLeaveId == LeaveId)).FirstOrDefault();
        //        var startMonth = lwpConstraint.LeaveStartDate.Month;
        //        var endMonth = lwpConstraint.LeaveEndDate.Month;
        //        var prLWPComponent = prComponentService.GetAll().Where(x => x.IsActive == true && (x.EmployeeTypeId == employeeEntity.EmployeeTypeId || x.EmployeeTypeId == 3) && x.EmployeeStatusId == employeeEntity.EmployeeStatusId && x.ComponentName.Trim() == "Leave Without Payment").FirstOrDefault();

        //        bool isSendForApproval = false;

        //        var employeeMonthlySalaryListStartMonth = employeeMonthlySalaryService.GetAll().Where(x => x.IsActive == true && (x.SalaryMonth == startMonth || x.SalaryMonth == endMonth) && x.SalaryYear == DateTime.Now.Year).ToList();

        //        var firstMonthSalaryDetail = employeeMonthlySalaryListStartMonth.Where(x => x.SalaryMonth == startMonth && x.SalaryYear == DateTime.Now.Year).ToList();

        //        var endMonthSalaryDetail = new List<EmployeeMonthlySalary>();
        //        if (startMonth != endMonth)
        //        {
        //            endMonthSalaryDetail = employeeMonthlySalaryListStartMonth.Where(x => x.SalaryMonth == endMonth && x.SalaryYear == DateTime.Now.Year).ToList();
        //        }

        //        if (lwpConstraint != null)
        //        {
        //            if (prLWPComponent == null)
        //            {
        //                message = "No LWP component configuration Found, LWP Denied";
        //            }

        //            if (firstMonthSalaryDetail.Where(p => p.IsApproved == true).Any() && endMonthSalaryDetail.Where(p => p.IsApproved == true).Any())
        //            {
        //                message = "Salary already approved, LWP Denied";
        //            }

        //            if (startMonth != endMonth)
        //            {
        //                for (var i = startMonth; i <= endMonth; i++)
        //                {
        //                    if (i == startMonth)
        //                    {
        //                        var lastDateOfMonth = ((new DateTime(lwpConstraint.LeaveStartDate.Year, lwpConstraint.LeaveStartDate.Month, 1)).AddMonths(1).AddDays(-1));
        //                        var dayDifference = (lastDateOfMonth - lwpConstraint.LeaveStartDate).TotalDays + 1;
        //                        var deductAmt = ((employeeGrossSalary) / (DateTime.DaysInMonth(lwpConstraint.LeaveStartDate.Year, startMonth))) * Convert.ToDecimal(dayDifference);//deductamt=(grosssalary/TotalDaysinMonth)*DaysinthatMonth
        //                        salaryDeductAmt = salaryDeductAmt + deductAmt;
        //                    }
        //                    else if (i == endMonth)
        //                    {
        //                        var dayDifference = (lwpConstraint.LeaveEndDate - new DateTime(lwpConstraint.LeaveEndDate.Year, lwpConstraint.LeaveEndDate.Month, 1)).TotalDays + 1;
        //                        var deductAmt = ((employeeGrossSalary) / (DateTime.DaysInMonth(lwpConstraint.LeaveEndDate.Year, endMonth))) * Convert.ToDecimal(dayDifference);//deductamt=(grosssalary/TotalDaysinMonth)*DaysinthatMonth
        //                        salaryDeductAmt = salaryDeductAmt + deductAmt;
        //                    }
        //                    else
        //                    {
        //                        var dayDifference = DateTime.DaysInMonth(lwpConstraint.LeaveEndDate.Year, i);
        //                        var deductAmt = ((employeeGrossSalary) / (DateTime.DaysInMonth(lwpConstraint.LeaveEndDate.Year, i))) * Convert.ToDecimal(dayDifference);//deductamt=(grosssalary/TotalDaysinMonth)*DaysinthatMonth
        //                        salaryDeductAmt = salaryDeductAmt + deductAmt;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                var dayDifference = (lwpConstraint.LeaveEndDate - lwpConstraint.LeaveStartDate).TotalDays + 1;
        //                var deductAmt = ((employeeGrossSalary) / (DateTime.DaysInMonth(lwpConstraint.LeaveStartDate.Year, startMonth))) * Convert.ToDecimal(dayDifference);//deductamt=(grosssalary/TotalDaysinMonth)*DaysinthatMonth
        //                salaryDeductAmt = salaryDeductAmt + deductAmt;
        //            }
        //        }

        //        lwpConstraint.IsAdjustment = true;
        //        lwpConstraint.LWPSalaryDeduction = salaryDeductAmt;
        //        lwpConstraint.IsSalaryDeducted = false;
        //        lwpConstraint.JoinDate = Convert.ToDateTime(JoiningDate);
        //        lwpConstraint.AdjustmentBy = LoggedInEmployeeId;
        //        lwpConstraint.AdjustmentDate = DateTime.UtcNow;
        //        leaveHistoryService.Update(lwpConstraint);

        //        var prLWPComponentId = prLWPComponent.PRComponentID;
        //        var firstDayOfMonth = new DateTime(DateTime.Now.Year, startMonth, 1);
        //        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        //        var lwpSalaryDeduction = GenerateDeductionTableObject(employeeEntity, prLWPComponentId, salaryDeductAmt, Convert.ToInt32(lwpConstraint.TotalDays), firstDayOfMonth, lastDayOfMonth);

        //        employeeSalaryDeductionService.Create(lwpSalaryDeduction);

        //        if (firstMonthSalaryDetail.Count > 0 || endMonthSalaryDetail.Count > 0)
        //        {
        //            if (firstMonthSalaryDetail.Where(p => p.IsApproved == true).ToList().Count == 0 && firstMonthSalaryDetail.Count > 0)
        //            {
        //                if (firstMonthSalaryDetail.Where(p => p.EmployeeId == employeeId && p.PRComponentId == prLWPComponentId).ToList().Count == 0)
        //                {
        //                    if (firstMonthSalaryDetail.Where(p => p.IsSendForApproval == true).Any())
        //                    {
        //                        isSendForApproval = true;
        //                    }
        //                    else
        //                    {
        //                        isSendForApproval = false;
        //                    }

        //                    var empMonthlySalary = GenerateEmployeeMonthlySalary(isSendForApproval, startMonth, firstMonthSalaryDetail[0].SalaryDate, employeeEntity, prLWPComponentId, Convert.ToDecimal(salaryDeductAmt), prLWPComponent);
        //                    employeeMonthlySalaryService.Create(empMonthlySalary);
        //                }
        //            }

        //            else if (endMonthSalaryDetail.Count > 0 && endMonthSalaryDetail.Where(p => p.IsApproved == true).ToList().Count == 0)
        //            {
        //                if (endMonthSalaryDetail.Where(p => p.EmployeeId == employeeId && p.PRComponentId == prLWPComponentId).ToList().Count == 0)
        //                {
        //                    if (endMonthSalaryDetail.Where(p => p.IsSendForApproval == true).Any())
        //                    {
        //                        isSendForApproval = true;
        //                    }
        //                    else
        //                    {
        //                        isSendForApproval = false;
        //                    }

        //                    var empMonthlySalary = GenerateEmployeeMonthlySalary(isSendForApproval, startMonth, endMonthSalaryDetail[0].SalaryDate, employeeEntity, prLWPComponentId, Convert.ToDecimal(salaryDeductAmt), prLWPComponent);
        //                    employeeMonthlySalaryService.Create(empMonthlySalary);
        //                }
        //            }
        //        }

        //        result = 1;
        //        message = "Salary Deducted successfully";
        //    }
        //    else
        //    {
        //        message = "No Salary Configuration found for the employee, Leave Adjustment Denied.";
        //    }
        //    return result;
        //}


        //private EmployeeSalaryDeduction GenerateDeductionTableObject(Employee employeeEntity, int prLWPComponentId, decimal salaryDeductAmt, int totalDays, DateTime firstDayOfMonth, DateTime lastDayOfMonth)
        //{
        //    var lwpSalaryDeduction = new EmployeeSalaryDeduction();
        //    lwpSalaryDeduction.EmployeeId = employeeEntity.EmployeeId;
        //    lwpSalaryDeduction.ComponentId = prLWPComponentId;
        //    lwpSalaryDeduction.ProductId = 0;
        //    lwpSalaryDeduction.SerialId = 0;
        //    lwpSalaryDeduction.DeductedAmount = Convert.ToDecimal(salaryDeductAmt);
        //    lwpSalaryDeduction.DeductionDays = Convert.ToInt32(totalDays);
        //    lwpSalaryDeduction.IsActive = true;
        //    lwpSalaryDeduction.IsApproved = true;
        //    lwpSalaryDeduction.StartDate = firstDayOfMonth;
        //    lwpSalaryDeduction.EndDate = lastDayOfMonth;
        //    lwpSalaryDeduction.CreateDate = DateTime.UtcNow;
        //    lwpSalaryDeduction.CreatedBy = Convert.ToInt32(LoggedInEmployeeId);

        //    return lwpSalaryDeduction;
        //}

        //private EmployeeMonthlySalary GenerateEmployeeMonthlySalary(bool isSendForApproval, int startMonth, DateTime SalaryDate, Employee employeeEntity, int prLWPComponentId, decimal salaryDeductAmt, PRComponent prLWPComponent)
        //{
        //    var empMonthlySalary = new EmployeeMonthlySalary();
        //    empMonthlySalary.SalaryMonth = startMonth;
        //    empMonthlySalary.SalaryYear = DateTime.Now.Year;
        //    empMonthlySalary.SalaryDate = SalaryDate;
        //    empMonthlySalary.EmployeeId = employeeEntity.EmployeeId;
        //    empMonthlySalary.PRComponentId = prLWPComponentId;
        //    empMonthlySalary.PRComponentAmount = Convert.ToDecimal(salaryDeductAmt);
        //    empMonthlySalary.ComponentCategory = prLWPComponent.ComponentCategory;

        //    empMonthlySalary.TransactionType = prLWPComponent.TransactionType;
        //    empMonthlySalary.IsActive = true;
        //    empMonthlySalary.IsRejected = false;
        //    empMonthlySalary.IsApproved = false;
        //    empMonthlySalary.OfficeId = employeeEntity.OfficeId;
        //    empMonthlySalary.CreatedBy = Convert.ToInt64(LoggedInEmployeeId);
        //    empMonthlySalary.UpdatedBy = Convert.ToInt64(LoggedInEmployeeId);
        //    empMonthlySalary.IsSendForApproval = isSendForApproval;
        //    empMonthlySalary.CreateDate = DateTime.UtcNow;
        //    empMonthlySalary.UpdateDate = DateTime.UtcNow;
        //    return empMonthlySalary;
        //}

        #endregion

        #region Private Methods

        private int CountValidLeaveApproverForEmployee(long employeeId)
        {
            bool isFinalApproverExist = true;

            //get leave approvers by employee id
            var apporverList = leaveApproversService.GetMany(x =>
                                            x.IsActive == true &&
                                            x.EmployeeId == employeeId)
                                            .ToList();

            if (apporverList.Count > 0)
            {
                long levelCount = apporverList.Max(x => x.ApprovalLevel);
                isFinalApproverExist = apporverList.Any(x => x.ApprovalLevel == levelCount);
            }

            if (isFinalApproverExist)
                return 1;

            return 0;
        }

        private void MapDropDownList(LeaveHistoryViewModel model)
        {
            model.EmployeeList = commonStaticDropDown.ddlInitial();
            model.LeaveTypeList = commonStaticDropDown.ddlInitial();
            model.LeaveDayDurationList = commonStaticDropDown.GetLeaveDayDurationList();
        }

        private List<string> UploadAttachmentOnLeaveEntry(out List<string> FileNameList)
        {
            FileNameList = new List<string>();
            var FileList = Request.Files;
            List<string> AttachmentPathList = new List<string>();
            if (null != FileList && FileList.Count > 0)
            {
                var FolderPath = Server.MapPath("~//" + AttachmentFolder);

                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }
                Random random = new Random();

                for (int i = 0; i < FileList.Count; i++)
                {
                    HttpPostedFileBase FileItem = FileList[i];

                    if (FileItem.ContentLength > 0)
                    {
                        FileNameList.Add(FileItem.FileName);
                        string FileNamePrefix = DateTime.Now.ToString("yyyyMMddHHmmss") + CommonHelper.RandomString(random, 6) + '-';
                        string fullPath = Path.Combine(FolderPath, FileNamePrefix + FileItem.FileName);
                        string UrlPath = "/" + AttachmentFolder + "/" + FileNamePrefix + FileItem.FileName;
                        AttachmentPathList.Add(UrlPath);
                        FileItem.SaveAs(fullPath);
                    }
                }
            }
            return AttachmentPathList;
        }

        [HttpPost]
        public JsonResult GetLeaveAttachmentList(long LeaveHistoryId)
        {
            try
            {
                var AttachmentList = leaveHistoryAttachmentService.GetAttachmentList(LeaveHistoryId);
                return Json(new { success = true, data = AttachmentList });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion
    }
}
