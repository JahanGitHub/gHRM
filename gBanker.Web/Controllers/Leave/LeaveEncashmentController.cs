using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using System.Transactions;
using System.Collections.Generic;

using AutoMapper;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Web.ViewModels;
using gHRM.Web.Helpers;
using gHRM.Service.StoreProcedure;
using gHRM.Web.DropDownService;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gHRM.Service.Payroll;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Reports;
using gHRM.Web.Reports.Leave;
using gHRM.Core.Utilities.Constants;
using gHRM.Core.Utilities;
using Newtonsoft.Json;
using gHRM.Data.DBDetailModels;

namespace gHRM.Web.Controllers
{
    public class LeaveEncashmentController : BaseController
    {
        #region Variable
        private readonly ILeaveSellService leaveSellService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly ILeaveHistoryService leaveHistoryService;
        private readonly ILeaveELOpeningService leaveELOpeningService;
        private readonly ILeaveTypeService leaveTypeService;
        private readonly IELEncashmentConfigurationService eLEncashmentConfigurationService;
        private readonly IELEncashmentAuthorityService elEncashmentAuthorityService;

        private readonly IPRComponentService pRComponentService;
        private readonly IPRSalaryConfigurationService pRSalaryConfiguration;
        private readonly IEmployeeSalaryIncentiveService employeeSalaryIncentiveService;
        private readonly IEmployeeMonthlySalaryService employeeMonthlySalaryService;

        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;

        public LeaveEncashmentController(
              ILeaveSellService leaveSellService
            , IEmployeeService employeeService
            , ILeaveHistoryService leaveHistoryService
            , ILeaveELOpeningService leaveELOpeningService
            , IEmployeeSPService employeeSPService
            , ILeaveTypeService leaveTypeService
            , IELEncashmentConfigurationService eLEncashmentConfigurationService
            , IELEncashmentAuthorityService elEncashmentAuthorityService

            , IPRComponentService pRComponentService
            , IPRSalaryConfigurationService pRSalaryConfiguration
            , IEmployeeSalaryIncentiveService employeeSalaryIncentiveService
            , IEmployeeMonthlySalaryService employeeMonthlySalaryService
            )
        {
            this.leaveSellService = leaveSellService;
            this.employeeService = employeeService;
            this.leaveHistoryService = leaveHistoryService;
            this.leaveELOpeningService = leaveELOpeningService;
            this.employeeSPService = employeeSPService;
            this.leaveTypeService = leaveTypeService;
            this.eLEncashmentConfigurationService = eLEncashmentConfigurationService;

            this.elEncashmentAuthorityService = elEncashmentAuthorityService;
            this.employeeMonthlySalaryService = employeeMonthlySalaryService;
            this.pRComponentService = pRComponentService;
            this.pRSalaryConfiguration = pRSalaryConfiguration;
            this.employeeSalaryIncentiveService = employeeSalaryIncentiveService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion

        #region Events

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var model = new LeaveSellViewModel();
            model.EmployeeCode = SessionHelper.LoggedInEmployee.EmployeeCode;
            var empId = LoggedInEmployeeId;
            var ifExists = elEncashmentAuthorityService.GetMany(a =>
                                                a.IsActive == true &&
                                                a.EmployeeId == empId)
                                            .ToList();
            model.IsAuthorized = false;

            if (ifExists.Any())
                model.IsAuthorized = true;

            return View(model);
        }

        public ActionResult EncashWithSalary()
        {
            var entity = new LeaveSellViewModel();
            mapDropDownList(entity);
            return View(entity);
        }

        public ActionResult BulkEncashment()
        {
            if (!elEncashmentAuthorityService.IsEmployeeAuthorizedForEncashment(LoggedInEmployeeId ?? 0))
            {
                ViewBag.ErrMessage = "Sorry! You are not Authorized For Bulk Encashment";
                return View("~/Views/Shared/ShowError.cshtml");
            }
            List<BulkLeaveEncashmentModel> DataList = leaveSellService.GetBulkEncashmentData();

            if (1 == DataList.Count() && 0 == DataList.First().Id)
            {
                ViewBag.ErrMessage = DataList.First().Name;
                return View("~/Views/Shared/ShowError.cshtml");
            }
            ViewBag.BulkEncashmentData = JsonConvert.SerializeObject(DataList);
            return View();
        }
        #endregion

        #region Manual Leave Sell 

        public ActionResult ManualLeaveSell()
        {
            var model = new LeaveSellAdviseViewModel();

            return View(model);
        }

        [HttpPost]
        public JsonResult ManualLeaveSell(LeaveSellAdviseViewModel model)
        {
            int result = 0;
            string message = "";
                       
            if (!ModelState.IsValid)
            {
                message = "Failed to apply for leave encashment";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    if (!IsValidManualLeaveSell(model, out message)) return Json(new { result = 0, message = message, leaveSellId = 0 }, JsonRequestBehavior.AllowGet);
                    //Populate Leave Sell
                    LeaveSell leaveSell = PopulateLeaveSell(model);

                    //let's create leave sell [leave.LeaveSell]
                    var newLeaveSell = leaveSellService.Create(leaveSell);

                    if (newLeaveSell == null)
                    {
                        result = 0;
                        message = "Error on leave encashment saving!";
                        return Json(new { result = result, message = message, leaveSellId = newLeaveSell.LeaveSellId }, JsonRequestBehavior.AllowGet);
                    }

                    string emmployeeCode = model.EmployeeCode;
                    int leaveSellId = leaveSell.LeaveSellId;
                    int status = LeaveSellAdviseStatusConstants.Sold;

                    //let's update leave sell advise status
                    leaveSellService.UpdateLeaveSellAdviseStatus(emmployeeCode, leaveSellId, status);
                    transactionScope.Complete();
                    transactionScope.Dispose();

                    result = 1;
                    message = "Successfully applied for leave encashment";
                    return Json(new { result = result, message = message, leaveSellId = newLeaveSell.LeaveSellId }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    result = 0;
                    message = "Failed to apply for leave encashment";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult ManualLeaveSellForInactive()
        {
            var model = new LeaveSellAdviseViewModel();

            return View(model);
        }

        [HttpPost]
        public JsonResult ManualLeaveSellForInactive(LeaveSellAdviseViewModel model)
        {
            int result = 0;
            string message = "";

            if (!ModelState.IsValid)
            {
                message = "Failed to apply for leave encashment";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    if (!IsValidManualLeaveSellForInactive(model, out message)) return Json(new { result = 0, message = message, leaveSellId = 0 }, JsonRequestBehavior.AllowGet);
                    //Populate Leave Sell
                    LeaveSell leaveSell = PopulateLeaveSell(model);
                    leaveSell.IsManualLeaveSellForInactive = true;
                    leaveSell.RequestDate = leaveSell.SaleDate ?? DateTime.Now;
                    leaveSell.ApprovedDate = leaveSell.RequestDate;
                    leaveSell.PaymentDate = leaveSell.RequestDate;
                    leaveSell.IsApproved = true;
                    leaveSell.IsAmountPaid = true;

                    //let's create leave sell [leave.LeaveSell]
                    var newLeaveSell = leaveSellService.Create(leaveSell);

                    if (newLeaveSell == null)
                    {
                        result = 0;
                        message = "Error on leave encashment saving!";
                        return Json(new { result = result, message = message, leaveSellId = newLeaveSell.LeaveSellId }, JsonRequestBehavior.AllowGet);
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();

                    result = 1;
                    message = "Leave encashment successfull";
                    return Json(new { result = result, message = message, leaveSellId = newLeaveSell.LeaveSellId }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    result = 0;
                    message = "Leave encashment failed";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion

        #region HttpRequests

        public JsonResult GetLeaveSellList([DataSourceRequest] DataSourceRequest request, string ApprovalStatus)
        {
            try
            {
                List<LeaveSellViewModel> List_LeaveSellViewModel = new List<LeaveSellViewModel>();
                var empId = LoggedInEmployeeId;
                var ifExists = elEncashmentAuthorityService.GetMany(a => a.IsActive == true && a.EmployeeId == empId).ToList();
                if (ifExists.Any())
                {
                    StringBuilder AndCondition = new StringBuilder();

                    if (ApprovalStatus == "A") // Approved
                    {
                        AndCondition.Append(" WHERE LS.IsActive = 1");
                        AndCondition.Append(" AND LS.IsApproved = 1");
                    }
                    else if (ApprovalStatus == "N") // Not Approve
                    {
                        AndCondition.Append(" WHERE LS.IsActive = 1");
                        AndCondition.Append(" AND LS.IsApproved = 0");
                    }
                    else if (ApprovalStatus == "R") // reject
                    {
                        AndCondition.Append(" WHERE LS.IsActive = 0");
                        AndCondition.Append(" AND LS.IsApproved = 0");
                    }

                    else if (ApprovalStatus == "E") // Approved but Not paid
                    {
                        AndCondition.Append(" WHERE LS.IsActive = 1");
                        AndCondition.Append(" AND LS.IsApproved = 1");
                        AndCondition.Append(" AND LS.IsAmountPaid= 0");
                    }

                    var OfficeTypeId = Convert.ToInt32(LoggedInOfficeType);
                    var OfficeId = Convert.ToInt32(LoggedInOfficeID);
                    var param = new { OfficeId = OfficeId, AndCondition = AndCondition.ToString() };
                    var promotionDetail = employeeSPService.GetDataWithParameter(param, "leave.SP_GetLeaveSellList");

                    List_LeaveSellViewModel = promotionDetail.Tables[0].AsEnumerable()
                          .Select(row => new LeaveSellViewModel
                          {
                              Rowsl = row.Field<string>("Rowsl"),
                              LeaveSellId = row.Field<int>("LeaveSellId"),
                              EmployeeId = row.Field<long>("EmployeeId"),
                              EmployeeCode = row.Field<string>("EmployeeCode"),
                              EmployeeName = row.Field<string>("EmployeeName"),
                              DesignationName = row.Field<string>("DesignationName"),
                              OfficeName = row.Field<string>("OfficeName"),
                              ScaleDate = row.Field<string>("ScaleDate"),
                              SaleDateMsg = row.Field<string>("SaleDateMsg"),
                              BalanceEl = row.Field<decimal?>("BalanceEl"),
                              TotalDays = row.Field<int>("TotalDays"),
                              EncashedAmount = row.Field<decimal>("EncashedAmount"),
                              IsApproved = row.Field<bool>("IsApproved"),
                              IsApprovedMsg = row.Field<string>("Approved"),
                              IsActive = row.Field<bool?>("IsActive"),
                              ActiveStatus = row.Field<string>("ActiveStatus"),
                              IsAmountPaid = row.Field<bool>("IsAmountPaid")
                          }).ToList();
                }

                DataSourceResult result = List_LeaveSellViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetLeaveSellListByEmployee([DataSourceRequest] DataSourceRequest request, int EmployeeId)
        {
            try
            {
                List<LeaveSellViewModel> List_LeaveSellViewModel = new List<LeaveSellViewModel>();
                var empId = LoggedInEmployeeId;
                var ifExists = elEncashmentAuthorityService.GetAll().Where(a => a.IsActive == true && a.EmployeeId == empId).ToList();
                if (ifExists.Any())
                {
                    string empInfo = string.Format("AND LS.EmployeeId = " + EmployeeId);

                    StringBuilder AndCondition = new StringBuilder();
                    AndCondition.Append(" WHERE LS.IsActive = 1");
                    AndCondition.Append(empInfo);

                    var OfficeTypeId = Convert.ToInt32(LoggedInOfficeType);
                    var OfficeId = Convert.ToInt32(LoggedInOfficeID);
                    var param = new { OfficeId = OfficeId, AndCondition = AndCondition.ToString() };
                    var promotionDetail = employeeSPService.GetDataWithParameter(param, "leave.SP_GetLeaveSellList");

                    List_LeaveSellViewModel = promotionDetail.Tables[0].AsEnumerable()
                          .Select(row => new LeaveSellViewModel
                          {
                              Rowsl = row.Field<string>("Rowsl"),
                              LeaveSellId = row.Field<int>("LeaveSellId"),
                              EmployeeId = row.Field<long>("EmployeeId"),
                              EmployeeCode = row.Field<string>("EmployeeCode"),
                              EmployeeName = row.Field<string>("EmployeeName"),
                              DesignationName = row.Field<string>("DesignationName"),
                              OfficeName = row.Field<string>("OfficeName"),
                              ScaleDate = row.Field<string>("ScaleDate"),
                              //SaleDate = row.Field<DateTime>("SaleDate"),
                              SaleDateMsg = row.Field<string>("SaleDateMsg"),
                              BalanceEl = row.Field<int?>("BalanceEl"),
                              //RequestDate = row.Field<DateTime>("RequestDate"),
                              TotalDays = row.Field<int>("TotalDays"),
                              EncashedAmount = row.Field<decimal>("EncashedAmount"),
                              IsApproved = row.Field<bool>("IsApproved"),
                              IsApprovedMsg = row.Field<string>("Approved"),
                              //ApprovedDate = row.Field<DateTime?>("ApprovedDate"),
                              IsAmountPaid = row.Field<bool>("IsAmountPaid"),
                              IsPaidWithSalary = row.Field<bool?>("IsPaidWithSalary"),
                              //PaymentDate = row.Field<DateTime?>("PaymentDate"),
                              IsActive = row.Field<bool>("IsActive"),
                              ActiveStatus = row.Field<string>("ActiveStatus"),
                              Remark = row.Field<string>("Remark"),
                          }).ToList();
                }

                DataSourceResult result = List_LeaveSellViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult leaveSellReject(string LeaveSellId)
        {
            int result = 0;
            if (LeaveSellId != "")
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        var entity = leaveSellService.GetById(Convert.ToInt32(LeaveSellId));

                        entity.IsActive = false;
                        entity.IsApproved = false;
                        entity.UpdateDate = DateTime.Now;
                        entity.UpdateUser = SessionHelper.LoggedInEmployeeID;
                        leaveSellService.Update(entity);
                        leaveSellService.IfManualLeaveSellAllowManualAgain(Convert.ToInt32(LeaveSellId));
                        transactionScope.Complete();
                        transactionScope.Dispose();
                        result = 1;
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Dispose();
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult leaveSellApprove(string LeaveSellId)
        {
            int result = 0;
            string message = String.Empty;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if (LeaveSellId != "")
                    {
                        var prComponent = pRComponentService.GetMany(c => c.IsActive == true && c.ComponentName == "Leave Encashment").FirstOrDefault();//Leave Encashment
                        if (prComponent != null)
                        {
                            var entity = leaveSellService.GetById(Convert.ToInt32(LeaveSellId));
                            //var dt = Convert.ToDateTime(entity.SaleDate);
                            //var firstDayOfMonth = new DateTime(dt.Year, dt.Month, 1);
                            //var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                            var empStatusId = employeeService.GetById(Convert.ToInt32(entity.EmployeeId)).EmployeeStatusId;
                            var prcompId = pRComponentService.GetMany(p => p.IsActive == true && p.ComponentName == "Basic Salary" && p.EmployeeStatusId == empStatusId).FirstOrDefault().PRComponentID;
                            var comp = pRSalaryConfiguration.GetMany(x => x.IsActive == true && x.PRComponentID == prcompId && x.EmployeeID == entity.EmployeeId).FirstOrDefault();
                            if (comp != null)
                            {
                                //entity.EncashedAmount = comp.ComponentAmount;
                                //entity.SaleDate = DateTime.Now;
                                entity.IsApproved = true;
                               //entity.ApprovedDate = DateTime.Now;
                                entity.UpdateUser = SessionHelper.LoggedInEmployeeID;
                                leaveSellService.Update(entity);
                                message = "Approved Successfully";
                                result = 1;
                            }
                            else
                            {
                                result = 0;
                                message = "Component Not Configured";
                                scope.Dispose();
                            }
                        }
                        else
                        {
                            result = 0;
                            message = "Component Not Configured";
                            scope.Dispose();
                        }
                        scope.Complete();
                    }
                    else
                    {
                        result = 0;
                        message = "Invalid Component Id";
                        scope.Dispose();
                    }
                }
                catch (Exception e)
                {
                    result = 0;
                    message = "Component Not Configured";
                    scope.Dispose();
                }
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLeaveSellInfo(string employee_Code)
        {
            string message = String.Empty;
            int result = 0;
            LeaveSellViewModel model = new LeaveSellViewModel();
            try
            {
                var param = new { EmployeeCode = employee_Code };
                //get employee from [dbo.Employee]
                var list = employeeSPService.GetDataWithParameter(param, "cmm.SP_GetEmployeeInfo_ByEmployeeCode");

                var employee = list.Tables[0].AsEnumerable().Select(row => new EmployeeViewModel()
                {
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    OffcDesignName = row.Field<string>("OffcDesignName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    FirstJoiningDate = row.Field<DateTime>("FirstJoiningDate"),
                    OfficeId = row.Field<int>("OfficeId"),
                    DesignationName = row.Field<string>("DesignationName"),
                    OfficeName = row.Field<string>("OfficeName"),
                    ConfirmationDate = row.Field<DateTime?>("ConfirmationDate")

                }).ToList();

                // Validation 1
                if (employee.Count == 0)
                {
                    message = "Employee not found or not eligible for Earn Leave";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                long employeeId = employee[0].EmployeeId;
                var EmpoyeeName = employee[0].EmployeeName;

                // Validation 2
                var elConfiguration = leaveTypeService.GetMany(l =>
                                                l.IsActive == true &&
                                                l.LeaveCategory == LeaveCategoryConstants.Annual_EL)
                                                .FirstOrDefault();
                if (elConfiguration == null)
                {
                    message = "No Leave Configuration found";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                // Validation 3
                var LeaveAdjust = leaveHistoryService.GetNotAdjustLeave(employee_Code, employeeId);
                if (LeaveAdjust != null)
                {
                    message = "Adjustment not complete, Earn Leave Encashment denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                // Validation 4
                var eligibleDate = elConfiguration.EligibleFrom == "C" ? employee[0].ConfirmationDate : employee[0].FirstJoiningDate;
                if (eligibleDate == null)
                {
                    message = "No EligibleDate found";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                //Need to change after this exception order for GC
                eligibleDate = employee[0].FirstJoiningDate;

                // Validation 5
                var notApprovedEncashmentList = leaveSellService.GetMany(x => x.EmployeeId == employeeId && x.IsApproved == false && x.IsActive == true);
                if (notApprovedEncashmentList.Any())
                {
                    message = "Previous Application not Approved yet. New Application denied.";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                model.EmployeeId = employeeId;
                model.EmployeeName = EmpoyeeName;
                model.OfficeName = employee[0].OfficeName;
                model.DesignationName = employee[0].DesignationName;
                model.DepartmentName = employee[0].DepartmentName;

                var empLeaveOpen = leaveELOpeningService.GetByEmployeeId(employee[0].EmployeeId);
                var empLeaveSellDt = leaveSellService.GetLastSellDateByEmpId(employee[0].EmployeeId);

                DateTime? LastSellDt = null;

                if (empLeaveSellDt != null)
                {
                    LastSellDt = empLeaveSellDt;
                }
                else if (empLeaveOpen != null)
                {
                    if (empLeaveOpen.LastSaleDate != null)
                    {
                        LastSellDt = empLeaveOpen.LastSaleDate;
                    }
                }

                TimeSpan difference;

                var earnLeaveTaken = leaveHistoryService.GetEarnLeaveTakenByEmpId(Convert.ToInt64(employeeId));
                string ENTRY_STAGE = GetSetting("EL_ENCASHMENT_ENTRY_STAGE");
                if ("" == ENTRY_STAGE) ENTRY_STAGE = "Other";
                var elEncashmentConfig = eLEncashmentConfigurationService.GetMany(x => x.IsActive == true && x.EncashmentStage == ENTRY_STAGE).FirstOrDefault();

                if (elEncashmentConfig == null)
                {
                    message = "No Encashment Config found";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                /*
                if (LastSellDt != null) // Employee has sold EL
                {
                    model.LastSellDateMsg = Convert.ToDateTime(LastSellDt).ToString("dd-MMM-yyyy");
                    model.NextEligibleDateMsg = Convert.ToDateTime(LastSellDt).AddYears(elEncashmentConfig.EligibilityDuration).AddDays(1).ToString("dd-MMM-yyyy");
                }
                else
                {
                    */
                model.LastSellDateMsg = "";
                model.NextEligibleDateMsg = Convert.ToDateTime(eligibleDate).AddYears(elEncashmentConfig.EligibilityDuration).AddDays(1).ToString("dd-MMM-yyyy");
                /*    
                }
                */


                if (empLeaveOpen != null) // Fetch data from Openning Table
                {
                    difference = DateTime.Parse(DateTime.Now.ToString()).Date - DateTime.Parse(empLeaveOpen.LeaveEndDate.ToString()).Date;
                    model.TotalEarnLeave = empLeaveOpen.BalanceFull + Convert.ToInt32(Math.Round((((Convert.ToDecimal(difference.TotalDays) + 1)) / Convert.ToInt32(elConfiguration.DaysPerEL))));
                    model.TotalLeaveSold = leaveSellService.GetTotSellEmpId(Convert.ToInt64(employeeId));
                    model.TotalEarnLeaveTaken = earnLeaveTaken;
                }
                else // No data in opening table
                {
                    difference = DateTime.Parse(DateTime.Now.ToString()).Date - DateTime.Parse(eligibleDate.ToString()).Date;
                    model.TotalEarnLeave = Convert.ToInt32(Math.Round(((Convert.ToDecimal(difference.TotalDays) + 1)) / Convert.ToInt32(elConfiguration.DaysPerEL)));// EarnLeaveTaken
                    model.TotalLeaveSold = leaveSellService.GetTotSellEmpId(Convert.ToInt64(employeeId));
                    model.TotalEarnLeaveTaken = earnLeaveTaken;
                }

                model.ConfirmDateMsg = Convert.ToDateTime(eligibleDate).ToString("dd-MMM-yyyy");
                model.EligibleDateMsg = Convert.ToDateTime(eligibleDate).AddYears(elEncashmentConfig.EligibilityDuration).AddDays(1).ToString("dd-MMM-yyyy");
                model.AvailableLeave = model.TotalEarnLeave - ((model.TotalLeaveSold + model.TotalEarnLeaveTaken));

                model.EncashmentEligibleQuantity = elEncashmentConfig.EncashmentEligibleQuantity; //TODO: should come from config
                model.EncashmentEligibleYears = elEncashmentConfig.EligibilityDuration; //TODO: should come from config
                model.EncashmentQuantity = GetEncashmentQuantity(elEncashmentConfig, model.AvailableLeave ?? 0, ENTRY_STAGE);

                var dateDiffFromEligibleDate = DateTime.Parse(DateTime.Now.ToString()).Date - DateTime.Parse(eligibleDate.ToString()).Date;

                double totalYears = (dateDiffFromEligibleDate.TotalDays / 365);

                if (totalYears < model.EncashmentEligibleYears || ("General" != ENTRY_STAGE && model.AvailableLeave < model.EncashmentEligibleQuantity))
                {
                    message = $"Total Years {(int)totalYears} and Availablbe Balance {model.AvailableLeave}. Earn Leave Encashment denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                /*
                if (model.AvailableLeave >= elEncashmentConfig.MinimumBalance)
                {
                */
                if (DateTime.Parse(model.EligibleDateMsg).Date <= DateTime.Now.Date) // after 6 years
                {
                    if (model.LastSellDateMsg == string.Empty)
                    {
                        model.LeaveSellMessage = "You can encash your earn leaves from " + DateTime.Parse(model.EligibleDateMsg).Date.ToString("dd-MMM-yyyy");
                        model.LeaveSellSave = "1";
                    }
                    else
                    {
                        if (DateTime.Parse(model.LastSellDateMsg).Date.AddYears(elEncashmentConfig.EligibilityDuration) <= DateTime.Now.Date) // after 3 years
                        {
                            model.LeaveSellMessage = "You can encash your earn leaves from " + DateTime.Parse(model.LastSellDateMsg).Date.AddYears(elEncashmentConfig.EligibilityDuration).ToString("dd-MMM-yyyy");
                            model.LeaveSellSave = "1";
                        }
                        else
                        {
                            model.LeaveSellMessage = "You cannot encash your earn leaves before " + DateTime.Parse(model.LastSellDateMsg).Date.AddYears(elEncashmentConfig.EligibilityDuration).ToString("dd-MMM-yyyy");
                            model.LeaveSellSave = "0";
                        }
                    }
                }
                else
                {
                    model.LeaveSellMessage = "You cannot encash your earn leaves before " + DateTime.Parse(model.EligibleDateMsg).Date.ToString("dd-MMM-yyyy");
                    model.LeaveSellSave = "0";
                }

                /*
            }
            else
            {
                model.LeaveSellMessage = "You cannot encash your earn leaves, because total EL is less than " + elEncashmentConfig.MinimumBalance;
                model.LeaveSellSave = "0";
            }
                */
            }
            catch (Exception e)
            {
                throw;
            }

            return Json(new { result = 1, model = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(LeaveSellViewModel model)
        {
            var entity = Mapper.Map<LeaveSellViewModel, LeaveSell>(model);
            int result = 0;
            string message = "";

            //validation 01
            if (!ModelState.IsValid)
            {
                message = "Failed to apply for leave encashment";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                //validation 02
                if (model.LeaveSellSave != "1")
                {
                    message = "Failed to apply for leave encashment";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                //validation 03
                var employeeDetail = employeeService.GetById(Convert.ToInt32(model.EmployeeId));
                if (employeeDetail == null)
                {
                    message = "Employee Detail not found";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                var component = pRComponentService.GetMany(p =>
                                p.IsActive == true &&
                                p.EmployeeTypeId == employeeDetail.EmployeeTypeId &&
                                p.EmployeeStatusId == employeeDetail.EmployeeStatusId
                                && p.ComponentName == "Basic Salary").FirstOrDefault();

                //validation 04
                if (component == null)
                {
                    message = "Component not configured";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                var componentId = component.PRComponentID;

                var salaryConfigure = pRSalaryConfiguration.GetMany(p =>
                                                p.IsActive == true &&
                                                p.EmployeeID == model.EmployeeId &&
                                                p.PRComponentID == componentId).FirstOrDefault();

                //validation 04
                if (salaryConfigure == null)
                {
                    message = "Salary Not configured";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                entity.EncashedAmount = Funct.GetEncashedAmount(salaryConfigure.ComponentAmount, entity.TotalDays);
                entity.IsAmountPaid = false;
                entity.IsActive = true;
                entity.AnulipiTxt = model.AnulipiTxt;
                entity.OrderCreateOfficeId = SessionHelper.LoginUserOfficeID;
                entity.CreateDate = DateTime.UtcNow;
                entity.UpdateDate = DateTime.UtcNow;
                entity.CreateUser = LoggedInEmployeeId ?? 0;
                entity.UpdateUser = LoggedInEmployeeId ?? 0;

                //let's create leave sell [leave.LeaveSell]
                var newLeaveSell = leaveSellService.Create(entity);

                if (newLeaveSell == null)
                {
                    result = 0;
                    message = "Error on leave encashment saving!";
                    return Json(new { result = result, message = message, leaveSellId = newLeaveSell.LeaveSellId }, JsonRequestBehavior.AllowGet);
                }

                result = 1;
                message = "Successfully applied for leave encashment";
                return Json(new { result = result, message = message, leaveSellId = newLeaveSell.LeaveSellId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result = 0;
                message = "Failed to apply for leave encashment";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveEncashment(LeaveSellViewModel Encashment)
        {
            int result = 0;
            string message = string.Empty;

            try
            {
                if (Encashment == null || String.IsNullOrEmpty(Encashment.Payment))
                {
                    message = "Please select all required field";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }

                if (Convert.ToInt32(Encashment.Payment) == 2) // Without Salary
                {

                    if (Encashment == null || String.IsNullOrEmpty(Encashment.PaymentDate.ToString()))
                    {
                        message = "Please select all required field";
                        return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                    }

                    var model = leaveSellService.GetById(Encashment.LeaveSellId);
                    model.IsAmountPaid = true;
                    model.IsPaidWithSalary = false;
                    model.Remark = Encashment.Remark;
                    model.PaymentDate = Encashment.PaymentDate;
                    model.UpdateUser = LoggedInEmployeeId;
                    model.UpdateDate = DateTime.UtcNow;
                    leaveSellService.Update(model);

                    result = 1;
                    message = "Enacshemetn Save Successfully";
                }

                if (Convert.ToInt32(Encashment.Payment) == 1)// With Salary
                {

                    if (Encashment == null || Encashment.Year == 0 || Encashment.Month == 0)
                    {
                        message = "Please select all required field";
                        return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                    }

                    var month = Encashment.Month;
                    var year = Encashment.Year;

                    var isSalaryAlreadyApproved = employeeMonthlySalaryService.GetMany(x => x.IsActive == true && x.SalaryMonth == month && x.SalaryYear == year && x.IsApproved == true);

                    if (isSalaryAlreadyApproved.Count() > 0)
                    {
                        message = "Salary of selected month already approved, Save denied";
                        return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                    }

                    var employeeCode = Encashment.EmployeeCode.Trim();
                    var employeeDetail = employeeService.GetByCode(employeeCode);
                    var empTypeId = employeeDetail.EmployeeTypeId;
                    var empStatusId = employeeDetail.EmployeeStatusId;

                    var firstDate = new DateTime(Encashment.Year, Encashment.Month, 1);
                    var lastDate = firstDate.AddMonths(1).AddDays(-1);

                    var prComponent = pRComponentService.Get(x => x.IsActive == true && x.ComponentName == "Leave Encashment" && x.EmployeeStatusId == empStatusId);
                    if (prComponent == null)
                    {
                        message = "Leave Encashment component not found";
                        return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                    }

                    using (TransactionScope scope = new TransactionScope())
                    {
                        var model = leaveSellService.GetById(Encashment.LeaveSellId);
                        model.IsAmountPaid = true;
                        model.IsPaidWithSalary = true;
                        model.Remark = "With Salary";
                        model.PaymentDate = DateTime.UtcNow;
                        model.UpdateUser = LoggedInEmployeeId;
                        model.UpdateDate = DateTime.UtcNow;
                        leaveSellService.Update(model);

                        var salaryIncentive = new EmployeeSalaryIncentive();
                        salaryIncentive.EmployeeId = employeeDetail.EmployeeId;
                        salaryIncentive.PRComponentId = prComponent.PRComponentID;
                        salaryIncentive.ProductId = 0;
                        salaryIncentive.SerialId = 0;
                        salaryIncentive.PRComponentAmount = Convert.ToDecimal(model.EncashedAmount);
                        salaryIncentive.PRComponentHour = 0;
                        salaryIncentive.IsActive = true;
                        salaryIncentive.IsApproved = true;
                        salaryIncentive.StartDate = firstDate;
                        salaryIncentive.EndDate = lastDate;
                        salaryIncentive.CreateDate = DateTime.UtcNow;
                        salaryIncentive.CreatedBy = Convert.ToInt64(LoggedInEmployeeId);
                        salaryIncentive.UpdateDate = DateTime.UtcNow;
                        salaryIncentive.UpdatedBy = Convert.ToInt64(LoggedInEmployeeId);
                        employeeSalaryIncentiveService.Create(salaryIncentive);

                        result = 1;
                        message = "Enacshemetn Save Successfully";
                        scope.Complete();
                    }

                }
            }
            catch (Exception e)
            {
                result = 0;
                message = "Enacshemtn Save Failed";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ELEncashmentApplication(int LeaveSellId, long EmployeeId)
        {
            try
            {
                var param = new { LeaveSellId = LeaveSellId };
                var EncashmentApplication = employeeSPService.GetDataWithParameter(param, "leave.SP_rpt_LeaveEncashmentApplication");
                var param1 = new { EmployeeId = EmployeeId };
                var EncashmentApplicationHistory = employeeSPService.GetDataWithParameter(param1, "leave.SP_rpt_LeaveEncashmentApplicationHistory");
                var reportParam = new Dictionary<string, object>();
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("LeaveEncashmentApplicationHistory", EncashmentApplicationHistory.Tables[0]);

                ReportHelper.PrintWithSubReport("Leave/rpt_ELEncashmentApplication.rpt", EncashmentApplication.Tables[0], reportParam, subReportDB, new rpt_ELEncashmentApplication());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult MakeBulkEncashment(List<long> ExcludedEmployeeIdList)
        {
            if (null == ExcludedEmployeeIdList) ExcludedEmployeeIdList = new List<long>();
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    if (!elEncashmentAuthorityService.IsEmployeeAuthorizedForEncashment(LoggedInEmployeeId ?? 0))
                    {
                        transactionScope.Dispose();
                        return Json(new { success = false, message = "Sorry! You are not Authorized For Bulk Encashment" });
                    }
                    var prComponent = pRComponentService.GetMany(c => c.IsActive == true && c.ComponentName == "Leave Encashment").FirstOrDefault();//Leave Encashment
                    if (prComponent != null)
                    {
                        leaveSellService.BulkEncash(ExcludedEmployeeIdList, LoggedInEmployeeId ?? 0, LoginUserOfficeID ?? 0);
                        leaveSellService.Save();
                        transactionScope.Complete();
                        transactionScope.Dispose();
                    }
                    else
                    {
                        transactionScope.Dispose();
                        return Json(new { success = false, message = "Leave Encashment Component Not Configured" });
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = true });
        }
        #endregion

        #region Ajax Calls

        public JsonResult GetLeaveSellAdviseInfo(string employee_Code)
        {
            try
            {
                var employeeWithELAdvise = leaveSellService.GetEmployeeWithELAdvise(employee_Code);

                if(employeeWithELAdvise==null || employeeWithELAdvise.EmployeeId<=0)
                    return Json(new { result = 0, message = "Employee or leave sell advise not found." }, JsonRequestBehavior.AllowGet);

                return Json(new { result = 1, model = employeeWithELAdvise }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = 0, message = "Employee or leave sell advise not found." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetManualLeaveSellForInactiveInfo(string employee_Code)
        {
            try
            {
                var employeeWithELAdvise = leaveSellService.GetManualLeaveSellForInactiveInfo(employee_Code);

                if (employeeWithELAdvise == null || employeeWithELAdvise.EmployeeId <= 0)
                    return Json(new { result = 0, message = "No inactive Employee was found with code " + employee_Code }, JsonRequestBehavior.AllowGet);

                if (leaveSellService.WasManualLeaveSellForInactiveDoneForEmployee(employeeWithELAdvise.EmployeeId))
                {
                    return Json(new { result = 0, message = "Encashment is not allowed, since Manual Encashed for inactive previously" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = 1, model = employeeWithELAdvise }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = 0, message = "No inactive Employee was found with code " + employee_Code }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Private Methods

        private LeaveSell PopulateLeaveSell(LeaveSellAdviseViewModel model)
        {
            var leaveSell = new LeaveSell
            {
                LeaveSellNo = model.LeaveSellNo,
                EmployeeId = model.EmployeeId,
                RequestDate = Convert.ToDateTime(model.RequestDate),
                SaleDate = Convert.ToDateTime(model.SaleDate),
                TotalDays = Convert.ToInt32(model.TotalDays),
                EncashedAmount = Convert.ToDecimal(model.EncashedAmount),
                IsAuthorized = true,
                IsApproved = false,
                AnulipiTxt = model.Remarks,
                OrderCreateOfficeId = LoggedInOfficeID,
                IsAmountPaid = false,
                IsPaidWithSalary = false,
                PaymentDate = DateTime.Now,
                Remark = model.Remarks,
                CreateDate = DateTime.Now,
                IsActive = true,
                UpdateDate = DateTime.UtcNow,
                CreateUser = SessionHelper.LoginUserOfficeID,
                UpdateUser = SessionHelper.LoginUserOfficeID,
            };
            return leaveSell;
        }

        private void mapDropDownList(LeaveSellViewModel entity)
        {

            var PleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };

            var paymentTypes = new List<SelectListItem>();
            paymentTypes.Add(PleaseSelect);
            paymentTypes.Add(new SelectListItem() { Text = "With salary", Value = "1" });
            paymentTypes.Add(new SelectListItem() { Text = "Without Salary", Value = "2" });
            entity.PaymentTypes = paymentTypes;

            var yearList = new List<SelectListItem>();
            yearList.Add(PleaseSelect);
            for (int i = DateTime.Now.Year; i >= (DateTime.Now.Year) - 1; i--)
            {
                yearList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            entity.YearList = yearList;

            entity.MonthList = commonStaticDropDown.GetMonthListList();
        }

        private int GetEncashmentQuantity(ELEncashmentConfiguration Config, int AvailableLeave, string ENTRY_STAGE)
        {
            if ("General" == ENTRY_STAGE)
            {
                if (AvailableLeave >= Config.MinimumBalance)
                {
                    return Config.EncashmentEligibleQuantity;
                }
                else if (Config.Formula == EncashmentFormulaConstants.HalfIfLessThanMinimum)
                {
                    return 0 == AvailableLeave ? 0 : AvailableLeave / 2;
                }
                return 0;
            }
            return Config.MinimumBalance;
        }

        private bool IsValidManualLeaveSell(LeaveSellAdviseViewModel model, out string Message)
        {
            Message = "";
            string MANUAL_LEAVE_ENCASHMENT_BLOCK_IF_ENCASHED_DAYS = GetSetting("MANUAL_LEAVE_ENCASHMENT_BLOCK_IF_ENCASHED_DAYS");
            if ("" != MANUAL_LEAVE_ENCASHMENT_BLOCK_IF_ENCASHED_DAYS)
            {
                int ENCASHED_DAYS = 0, TotalDays = 0;
                int.TryParse(MANUAL_LEAVE_ENCASHMENT_BLOCK_IF_ENCASHED_DAYS, out ENCASHED_DAYS);
                int.TryParse(model.TotalDays, out TotalDays);

                if (leaveSellService.HasEmployeeEverEncashedDays(model.EmployeeId, ENCASHED_DAYS) && TotalDays >= ENCASHED_DAYS)
                {
                    Message = "Encashment of " + TotalDays + " days is not allowed, since Encashed " + ENCASHED_DAYS + " days previously";
                    return false;
                }
            }
            if (leaveSellService.HasEmployeeDoneManualLeaveSell(model.EmployeeCode))
            {
                Message = "Encashment is not allowed, since Manual Encashed previously";
                return true;
            }
            return true;
        }

        private bool IsValidManualLeaveSellForInactive(LeaveSellAdviseViewModel model, out string Message)
        {
            Message = "";
            if (leaveSellService.WasManualLeaveSellForInactiveDoneForEmployee(model.EmployeeId))
            {
                Message = "Encashment is not allowed, since Manual Encashed for inactive previously";
                return false;
            }
            if (Convert.ToInt32(model.TotalDays) <= 0)
            {
                Message = "Total Days is required";
                return false;
            }
            if (!(new int[] { 60, 120 }).Contains(Convert.ToInt32(model.TotalDays)))
            {
                Message = "Total Days must be 60 or 120 days";
                return false;
            }
            if (Convert.ToInt32(model.EncashedAmount.Replace(",", "")) <= 0)
            {
                Message = "Encashed Amount is required";
                return false;
            }
            return true;
        }
        #endregion
    }
}
