#region Usings
using AutoMapper;
using gHRM.Core.Filters.Payroll;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service;
using gHRM.Service.Payroll;
using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.Infrastructure.Date;
using gHRM.Web.Infrastucture.Framework;
using gHRM.Web.ViewModels.Payroll;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
#endregion

namespace gHRM.Web.Controllers.Payroll
{
    public class PRDepositController : BaseController
    {
        #region Priavate Variables

        private readonly IPRDepositService prDepositService;
        private readonly IComponentPayrollService componentPayrollService;
        private readonly IEmployeeStatusService employeeStatusService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSalaryDepositService employeeSalaryDepositService;
        private readonly IPRComponentGroupService prComponentGroupService;
        private readonly IPRComponentService prComponentService;
        private readonly IEmployeeMonthlySalaryService employeeMonthlySalaryService;
        private readonly IEmployeeSalaryDeductionService employeeSalaryDeductionService;
        private readonly IEmployeeStatusHistoryService employeeStatusHistoryService;
        private readonly IEmployeeSalaryIncentiveService employeeSalaryIncentiveService;
        private readonly ISalaryDateConfigService salaryDateConfigService;
        private readonly ICompanyWisePayrollConfigService companyWisePayrollConfigService;


        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;

        #endregion

        #region Ctor
        public PRDepositController(IPRDepositService prDepositService
            , IComponentPayrollService componentPayrollService
            , IEmployeeStatusService employeeStatusService
            , IEmployeeSPService employeeSPService
            , IEmployeeService employeeService
            , IEmployeeSalaryDepositService employeeSalaryDepositService
            , IPRComponentGroupService prComponentGroupService
            , IPRComponentService prComponentService
            , IEmployeeMonthlySalaryService employeeMonthlySalaryService
            , IEmployeeSalaryDeductionService employeeSalaryDeductionService
            , IEmployeeStatusHistoryService employeeStatusHistoryService
            , IEmployeeSalaryIncentiveService employeeSalaryIncentiveService
            , ISalaryDateConfigService salaryDateConfigService
            , ICompanyWisePayrollConfigService companyWisePayrollConfigService

       )
        {
            this.prDepositService = prDepositService;
            this.componentPayrollService = componentPayrollService;
            this.employeeStatusService = employeeStatusService;
            this.employeeSPService = employeeSPService;
            this.employeeService = employeeService;
            this.employeeSalaryDepositService = employeeSalaryDepositService;
            this.prComponentGroupService = prComponentGroupService;
            this.prComponentService = prComponentService;
            this.employeeMonthlySalaryService = employeeMonthlySalaryService;
            this.employeeSalaryDeductionService = employeeSalaryDeductionService;
            this.employeeStatusHistoryService = employeeStatusHistoryService;
            this.employeeSalaryIncentiveService = employeeSalaryIncentiveService;
            this.salaryDateConfigService = salaryDateConfigService;
            this.companyWisePayrollConfigService = companyWisePayrollConfigService;

            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }
        #endregion

        #region Index

        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Add PRDeposit Info

        public ActionResult Create()
        {
            var entity = new PRDepositViewModel();
            MapDropDown(entity);
            return View(entity);
        }

        [HttpPost]
        public JsonResult SavePRDeposit(PRDepositViewModel model)
        {
            var result = 0;
            var message = "";
            bool isOperationSuccess = true;

            if (!ModelState.IsValid)
            {
                message = "Warning, You must fill all the required fields.";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }

            var filter = new PRDepositSearchFilter
            {
                ComponentName = model.ComponentName,
                ComponentCategory = ComponentDepositCategoryConstants.Deposit,
                EmployeeStatusId = model.EmployeeStatusId,
                EmployeeTypeId = model.EmployeeType,
                OfficeLocationId = model.OfficeLocationId
            };

            var singlePRDeposit = prDepositService.GetSingleComponentByFilter(filter);

            if (singlePRDeposit != null && singlePRDeposit.Id > 0)
                return Json(new { result = result, message = "Duplicate Employee Type and Status found, Save denied" }, JsonRequestBehavior.AllowGet);

            var componentPayroll = componentPayrollService.GetByComponentName(model.ComponentName);

            if (componentPayroll == null)
                return Json(new { result = result, message = "Payroll component not found, Save denied" }, JsonRequestBehavior.AllowGet);

            model.ComponentPayrollId = componentPayroll.Id;

            var newPRDeposit = Mapper.Map<PRDepositViewModel, PRDeposit>(model);

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var prComponentList = new List<PRComponent>();

                    //populate prcomponent
                    PRComponent newPRComponent = PopulatePRComponent(model);

                    //let's create pr component for [PRComponent]
                    var resultPRComponent = prComponentService.Create(newPRComponent);

                    newPRDeposit.PRComponentId = resultPRComponent.PRComponentID;
                    newPRDeposit.ComponentCategory = ComponentDepositCategoryConstants.Deposit;
                    newPRDeposit.IsActive = true;
                    newPRDeposit.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    newPRDeposit.CreateDate = DateTime.UtcNow;

                    //let's create pr deposit [PRDeposit]
                    prDepositService.Create(newPRDeposit);

                    result = 1;
                    message = "Saved successfully";
                }
                catch (Exception ex)
                {
                    message = ex.InnerException.ToString();
                    isOperationSuccess = false;
                }

                if (isOperationSuccess)
                    scope.Complete();

                scope.Dispose();
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Salary Deposit & Refund

        public ActionResult SalaryDepositAndRefund()
        {
            var model = new PRDepositViewModel();
            MapDropDownForSalaryDepositAndRefund(model);

            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            model.SalaryYear = currentYear;
            model.SalaryMonth = currentMonth;

            return View(model);
        }

        #endregion

        #region Get PRDeposit Listings

        public JsonResult GetPRDepositInfo([DataSourceRequest]DataSourceRequest request, string FilterColumn, string FilterValue)
        {
            var depositInfo = employeeSPService.GetDataWithoutParameter("[prl].[PRDeposit_GetPRDepositInfo]");
            var viewDepositInfo = depositInfo.Tables[0].AsEnumerable().Select(p => new PRDepositViewModel()
            {
                Id = p.Field<int>("Id"),
                PRComponentId = p.Field<int>("PRComponentId"),
                ComponentName = p.Field<string>("ComponentName"),
                EmployeeType = p.Field<int>("EmployeeType"),
                EmployeeTypeName = p.Field<string>("EmployeeTypeName"),
                EmployeeStatusId = p.Field<int>("EmployeeStatusId"),
                EmployeeStatusName = p.Field<string>("StatusName"),
                IsDepositRequired = p.Field<int>("IsDepositRequired"),
                DepositeType = p.Field<string>("DepositeType"),
                OfficeLocationName = p.Field<string>("OfficeLocationName"),
                ReturnDepositeOnEmployeeStatusId = p.Field<int>("ReturnDepositeOnEmployeeStatusId"),
                ReturnDepositeOnEmployeeStatus = p.Field<string>("ReturnDepositeOnEmployeeStatusName"),
                TransactionType = p.Field<string>("TransactionType"),
                ComponentGroup = p.Field<string>("ComponentGroup"),
                NoOfSalaryDays = p.Field<int?>("NoOfSalaryDays"),
                EffectiveDate = p.Field<string>("EffectiveDate")
            }).ToList();
            DataSourceResult result = viewDepositInfo.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete PRDeposit Info

        [HttpPost]
        public JsonResult DeletePRDepositInfo(PRDepositViewModel obj)
        {
            var result = 0;
            var message = "";

            // check any on employee monthly salary [EmployeeMonthlySalary]
            var checkMonthlySalaryTableForPRDeposit =
                employeeMonthlySalaryService.CheckEmployeeMonthlySalaryByComponent(obj.PRComponentId);

            // check any on employee salary deduction [EmployeeSalaryDeduction]
            var checkSalaryDeductionTableForPRDeposit =
                                employeeSalaryDeductionService.CheckEmployeeSalaryDeductionByComponentId(obj.PRComponentId);

            // check any on employee salary deduction [EmployeeSalaryIncentive]
            var checkSalaryIncentiveTableForPRDeposit =
                                employeeSalaryIncentiveService.CheckEmployeeSalaryIncentiveByComponentId(obj.PRComponentId);

            if (checkMonthlySalaryTableForPRDeposit || checkSalaryDeductionTableForPRDeposit || checkSalaryIncentiveTableForPRDeposit)
                return Json(new { result = result, message = "This Component used on other tables, delete denied" }, JsonRequestBehavior.AllowGet);

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var model = prDepositService.GetById(obj.Id);
                    model.IsActive = false;
                    model.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;

                    //let's update [prl.PRDeposit]
                    prDepositService.Update(model);

                    var removePRComponent = prComponentService.GetById(model.PRComponentId);
                    removePRComponent.IsActive = false;
                    removePRComponent.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    removePRComponent.UpdateDate = DateTime.UtcNow;

                    //let's update [prl.PRComponent]
                    prComponentService.Update(removePRComponent);

                    scope.Complete();
                    result = 1;
                    message = "Success, Deleted Successfully!";
                }
                catch (Exception ex)
                {
                    message = ex.InnerException.ToString();
                }

                scope.Dispose();
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get Required Deposit Info

        public JsonResult GetRequiredDepositInfo([DataSourceRequest]DataSourceRequest request, string FilterColumn, string FilterValue
            , int salaryYear, int salaryMonth, DateTime fromDate, DateTime toDate
            )
        {
            var startDate = Convert.ToDateTime(fromDate);
            var endDate = Convert.ToDateTime(toDate);

            var param = new { startDate = startDate, endDate = endDate };

            //get employee with first joining date within this month [Employee]
            var employeeInfo = employeeSPService.GetDataWithParameter(param, "prl.SP_GetEmployeeInfoForSalaryDeposit");

            var viewEmpInfo = employeeInfo.Tables[0].AsEnumerable().Select(p => new PRDepositViewModel()
            {
                EmployeeId = p.Field<long>("EmployeeId"),
                EmployeeCode = p.Field<string>("EmployeeCode"),
                EmployeeName = p.Field<string>("EmployeeName"),
                EmployeeType = p.Field<int>("EmployeeTypeId"),
                OfficeLocationId = p.Field<int>("OfficeLocationId"),
                EmployeeTypeName = p.Field<string>("EmployeeTypeName"),
                EmployeeStatusId = p.Field<int>("EmployeeStatusId"),
                EmployeeStatusName = p.Field<string>("StatusName"),
                GrossSalary = p.Field<decimal>("GrossSalary")
            }).ToList();

            var tempEmployeeListForDeposit = new List<TempDepositRequiredEmployee>();

            //get all possible payroll deposits from [prl.PRDeposit]
            var prDepositInfo = prDepositService.GetPRDepositsByDataRange(startDate, endDate);

            if (!prDepositInfo.Any())
                return Json(new { Result = "OK", Records = 0, TotalRecordCount = tempEmployeeListForDeposit.LongCount(), JsonRequestBehavior.AllowGet });

            var salaryDays = prDepositInfo.FirstOrDefault(f => f.ComponentName == PayrollDepositTypeConstants.SalaryDeposit).NoOfSalaryDays;

            var companyWisePayrollConfig = companyWisePayrollConfigService.GetByCompanyCode(SessionHelper.CompanyCode);

            if (companyWisePayrollConfig == null)
                return Json(new { Result = "OK", Records = 0, TotalRecordCount = tempEmployeeListForDeposit.LongCount(), JsonRequestBehavior.AllowGet });

            int daysInMonth = companyWisePayrollConfig.PayrollType == PayrollTypeConstants.CalendarDay
                                            ? DateTime.DaysInMonth(salaryYear, salaryMonth) : companyWisePayrollConfig.NoOfSalaryDays;

            foreach (var item in viewEmpInfo)
            {
                var empStatus = item.EmployeeStatusId;
                var empType = item.EmployeeType;
                var grossSalary = item.GrossSalary;
                var empId = item.EmployeeId;
                decimal calculatedDepositAmount = 0;

                if (!prDepositInfo.Where(p => p.EmployeeStatusId == empStatus && p.EmployeeType == empType && p.OfficeLocationId == item.OfficeLocationId).Any())
                    continue;

                calculatedDepositAmount = (grossSalary / daysInMonth) * salaryDays;
                var entity = GenerateEntity(prDepositInfo, empType, item, empStatus, grossSalary, calculatedDepositAmount);
                tempEmployeeListForDeposit.Add(entity);
            }

            DataSourceResult result = tempEmployeeListForDeposit.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Approve Deposit

        [HttpPost]
        public JsonResult ApproveDepositInfo(PRDepositViewModel obj)
        {
            var result = 0;
            var message = "";
            var prComponentId = 0;
            var prDepositComponentId = 0;
            var flag = 0;

            var startDate = new DateTime(obj.SalaryYear, obj.SalaryMonth, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var salaryMonth = obj.SalaryMonth;
            var salaryYear = obj.SalaryYear;

            var componentSearchFilter = new PRComponentSearchFilter
            {
                OfficeLocationId = obj.OfficeLocationId,
                EmployeeTypeId = obj.EmployeeType,
                EmployeeStatusId = obj.EmployeeStatusId,
                ComponentName = obj.ComponentName,
                ComponentCategory = ComponentDepositCategoryConstants.Deposit
            };

            //get component from [prl.PRComponent]
            var prComponent = prComponentService.GetSingleComponentByFilter(componentSearchFilter);

            if (prComponent == null)
                return Json(new { result = 0, message = "No Component Configuration Found, Approval Denied" }, JsonRequestBehavior.AllowGet);

            if (prComponent != null)
                prComponentId = prComponent.PRComponentID;

            var prDeposit = prDepositService.GetActivePRComponentById(prComponent.PRComponentID);

            if (prDeposit == null)
                return Json(new { result = 0, message = "No Deposit Configuration Found, Approval Denied" }, JsonRequestBehavior.AllowGet);

            if (prDeposit != null)
                prDepositComponentId = prDeposit.PRComponentId;

            //get employee monthly salary from [prl.EmployeeMonthlySalary]
            var checkMonthlySalaryTable = employeeMonthlySalaryService
                                            .GetActiveEmployeeMonthlySalary(salaryYear, salaryMonth, (int)obj.EmployeeId);

            if (!checkMonthlySalaryTable.Any(p => p.IsActive == true))
                return Json(new { result = 0, message = "Monthly Salary not found. Please generate salary first!" }, JsonRequestBehavior.AllowGet);

            if (checkMonthlySalaryTable.Any(p => p.IsApproved == true))
                return Json(new { result = 0, message = "Salary already approved, insertion denied" }, JsonRequestBehavior.AllowGet);
            
            if (checkMonthlySalaryTable.Any(p => p.IsSendForApproval == true))
                return Json(new { result = 0, message = "Salary already send for approval, insertion denied" }, JsonRequestBehavior.AllowGet);

            if (checkMonthlySalaryTable.Any(p => p.PRComponentId == prComponentId))
                return Json(new { result = 0, message = "Salary already exist for this this month, insertion denied" }, JsonRequestBehavior.AllowGet);

            var employeeSalaryDeductionSearchFilter = new EmployeeSalaryDeductionSearchFilter
            {
                PrComponentId = prComponentId,
                StartDate = startDate,
                EndDate = endDate,
                EmployeeId = Convert.ToInt32(obj.EmployeeId)
            };

            //check employee salary deduction in [prl.EmployeeSalaryDeduction]          
            var checkDuplicateDepositDeduction = employeeSalaryDeductionService.GetEmployeeSalaryDeductionsByFilter(employeeSalaryDeductionSearchFilter);

            if (checkDuplicateDepositDeduction.Any())
                return Json(new { result = 0, message = "Employee deduction already exit for this salary month and year" }, JsonRequestBehavior.AllowGet);

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    //populate salary deposit for table [prl.EmployeeSalaryDeposit]
                    var model = PopulateSalaryDeposit(prComponentId, prDepositComponentId, obj, startDate, endDate);

                    //let's insert employee salary deposit  into [prl.EmployeeSalaryDeposit]
                    employeeSalaryDepositService.Create(model);

                    //populate employee salary deduction for [prl.EmployeeSalaryDeduction]
                    var entity = GenerateEmployeeSalaryDeductionEntity(obj, prComponentId, startDate, endDate);

                    //let's insert employee salary deduction into [prl.EmployeeSalaryDeduction]
                    employeeSalaryDeductionService.Create(entity);

                    var officeId = employeeService.GetByEmpId(obj.EmployeeId).OfficeId;

                    //Pupulate Employee Monthly Salary for [prl.EmployeeMonthlySalary]
                    var newEmployeeMonthlySalary = PupulateEmployeeMonthlySalary(obj, prComponentId, endDate, salaryMonth, salaryYear, officeId);

                    //let's insert into [prl.EmployeeMonthlySalary]
                    employeeMonthlySalaryService.Create(newEmployeeMonthlySalary);

                    flag = 1;
                }
                catch (Exception ex)
                {
                    flag = 0;
                    message = ex.InnerException.ToString();
                }

                if (flag == 1)
                {
                    scope.Complete();
                    result = 1;
                    message = "Approved successfully";
                }
                scope.Dispose();
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Required Refund Info

        public JsonResult GetRequiredRefundInfo([DataSourceRequest]DataSourceRequest request, string FilterColumn, string FilterValue
            , int salaryYear, int salaryMonth, DateTime fromDate, DateTime toDate)
        {
            var tempRefundRequiredEmployee = new List<TempRefundRequiredEmployee>();

            var startDate = Convert.ToDateTime(fromDate);
            var endDate = Convert.ToDateTime(toDate);

            var param = new { startDate = startDate, endDate = endDate };

            //get employee listing from [prl.EmployeeSalaryDeposit]
            var employeeInfo = employeeSPService.GetDataWithParameter(param, "[prl].[RefundSalary_GetEmployeeInfoForSalaryRefund]");

            var viewEmpInfo = employeeInfo.Tables[0].AsEnumerable().Select(p => new PRDepositViewModel()
            {
                EmployeeId = p.Field<long>("EmployeeId"),
                EmployeeCode = p.Field<string>("EmployeeCode"),
                EmployeeName = p.Field<string>("EmployeeName"),
                EmployeeType = p.Field<int>("EmployeeTypeId"),
                OfficeLocationId = p.Field<int>("OfficeLocationId"),
                EmployeeTypeName = p.Field<string>("EmployeeTypeName"),
                EmployeeStatusId = p.Field<int>("EmployeeStatusId"),
                EmployeeStatusName = p.Field<string>("StatusName"),
                GrossSalary = p.Field<decimal>("GrossSalary"),
                RefundDays = p.Field<int>("RefundDays"),
                RefundAmount = p.Field<decimal>("RefundAmount")
            }).ToList();

            DataSourceResult result = viewEmpInfo.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRequiredRefundInfo_NotUsed(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var result = 0;
            var message = "";
            var tempRefundRequiredEmployee = new List<TempRefundRequiredEmployee>();

            DateTime day = DateTime.Now;
            var startDate = new DateTime(day.Year, day.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var employeeStatusHistoryList =
                      employeeStatusHistoryService.GetAll()
                          .Where(p => p.IsActive == true && p.StartDate >= startDate)
                          .ToList();

            var prComponents =
               prComponentService.GetAll()
                   .Where(p => p.IsActive == true && (p.ComponentName == "Salary Deposit Refund" || p.ComponentName == "Salary Deposit"))
                   .ToList();

            var isEmployeeInSalaryDeposit =
               employeeSalaryDepositService.GetAll()
                   .Where(p => p.IsActive == true && p.DepositDone == true && p.IsRefundRequired == false && p.EffectiveStartDate >= startDate && p.EffectiveEndDate <= endDate)
                   .ToList();

            var prDeposits =
                prDepositService.GetAll()
                    .Where(p => p.IsActive == true && p.EffectiveStartDate >= startDate)
                    .ToList();

            var employeeTypeStatus = employeeSPService.GetDataWithoutParameter("SP_GetEmployeeStatusName");
            var viewEmployeeTypeStatus =
                employeeTypeStatus.Tables[0].AsEnumerable().Select(p => new TempRefundRequiredEmployee()
                {
                    StatusName = p.Field<string>("StatusName"),
                    StatusValue = p.Field<string>("StatusValue")
                }).ToList();

            var employeeTypeName = employeeSPService.GetDataWithoutParameter("SP_GetEmployeeTypeName");
            var viewEmployeeType =
                employeeTypeName.Tables[0].AsEnumerable().Select(p => new TempRefundRequiredEmployee()
                {
                    EmployeeType = p.Field<int>("EmployeeTypeId"),
                    EmployeeTypeName = p.Field<string>("EmployeeTypeName")
                }).ToList();

            foreach (var item in employeeStatusHistoryList)
            {
                var checkDepositStatus =
                    isEmployeeInSalaryDeposit.Where(p => p.EmployeeId == item.EmployeeId && p.IsActive == true && p.DepositDone == true)
                        .FirstOrDefault();
                if (checkDepositStatus != null)
                {
                    var depositComponentId = checkDepositStatus.DepositComponentId;

                    var checkDepositCondition =
                        prDeposits.Where(p => p.PRComponentId == depositComponentId).FirstOrDefault();

                    if (checkDepositCondition != null)
                    {
                        var employeeStatus = checkDepositCondition.ReturnDepositeOnEmployeeStatusId;
                        var employeeType = Convert.ToInt32(checkDepositCondition.EmployeeType);

                        var checkPRComponent =
                            prComponents.Where(
                                p =>
                                    p.EmployeeTypeId == employeeType && p.EmployeeStatusId == employeeStatus &&
                                    p.ComponentName == "Salary Deposit Refund").FirstOrDefault();

                        if (checkPRComponent != null)
                        {
                            var refundAmount = checkDepositStatus.DepositAmount;
                            var refundDays = checkDepositStatus.NoOfSalaryDays;
                            var grossSalary =
                                employeeService.GetAll()
                                    .Where(p => p.EmployeeId == item.EmployeeId)
                                    .FirstOrDefault()
                                    .GrossSalary;

                            var entity = new TempRefundRequiredEmployee();
                            entity.PRComponentId = checkPRComponent.PRComponentID;
                            entity.EmployeeType = checkPRComponent.EmployeeTypeId;
                            var empTypeId = checkPRComponent.EmployeeTypeId;
                            entity.EmployeeTypeName =
                                viewEmployeeType.Where(p => p.EmployeeType == empTypeId).FirstOrDefault().EmployeeTypeName;

                            entity.EmployeeStatusId = checkPRComponent.EmployeeStatusId.Value;
                            var empStatusValue = checkPRComponent.EmployeeStatusId;
                            entity.EmployeeStatusName =
                                viewEmployeeTypeStatus.Where(p => p.StatusId == empStatusValue)
                                    .FirstOrDefault()
                                    .StatusName;

                            entity.RefundAmount = refundAmount;
                            entity.RefundDays = refundDays;
                            entity.GrossSalary = grossSalary;
                            entity.EmployeeId = checkDepositStatus.EmployeeId;
                            entity.EmployeeCode = checkDepositStatus.EmployeeCode;
                            entity.EmployeeName = checkDepositStatus.EmployeeName;
                            tempRefundRequiredEmployee.Add(entity);
                        }
                    }
                }
            }

            var currentPageRecords = tempRefundRequiredEmployee.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = tempRefundRequiredEmployee.LongCount(), JsonRequestBehavior.AllowGet });
        }
        #endregion

        #region Approve Refund Info

        [HttpPost]
        public JsonResult ApproveRefundInfo(PRDepositViewModel obj)
        {
            var result = 0;
            var message = "";
            var prComponentId = 0;
            var flag = 0;

            DateTime now = DateTime.UtcNow;

            var startDate = new DateTime(obj.SalaryYear, obj.SalaryMonth, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var salaryMonth = obj.SalaryMonth;
            var salaryYear = obj.SalaryYear;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var componentSearchFilter = new PRComponentSearchFilter
                    {
                        OfficeLocationId = obj.OfficeLocationId,
                        EmployeeTypeId = obj.EmployeeType,
                        EmployeeStatusId = obj.EmployeeStatusId,
                        ComponentName = PayrollDepositTypeConstants.SalaryDepositRefund,
                        ComponentCategory = ComponentDepositCategoryConstants.Deposit
                    };

                    //get component from [prl.PRComponent]
                    var prComponent = prComponentService.GetSingleComponentByFilter(componentSearchFilter);

                    if (prComponent == null)
                        return Json(new { result = 0, message = "No Component Configuration Found, Approval Denied" }, JsonRequestBehavior.AllowGet);

                    var filter = new PRDepositSearchFilter
                    {
                        ComponentName = PayrollDepositTypeConstants.SalaryDepositRefund,
                        ComponentCategory = ComponentDepositCategoryConstants.Deposit,
                        EmployeeStatusId = obj.EmployeeStatusId,
                        EmployeeTypeId = obj.EmployeeType,
                        OfficeLocationId = obj.OfficeLocationId
                    };

                    var prDeposit = prDepositService.GetSingleComponentByFilter(filter);

                    if (prDeposit == null)
                        return Json(new { result = 0, message = "No Deposit Configuration Found, Approval Denied" }, JsonRequestBehavior.AllowGet);

                    //get employee monthly salary from [prl.EmployeeMonthlySalary]
                    var checkMonthlySalaryTable = employeeMonthlySalaryService
                                                    .GetActiveEmployeeMonthlySalary(salaryYear, salaryMonth, (int)obj.EmployeeId);

                    if (!checkMonthlySalaryTable.Any(p => p.IsActive == true))
                        return Json(new { result = 0, message = "Monthly Salary not found. Please generate salary first!" }, JsonRequestBehavior.AllowGet);

                    if (checkMonthlySalaryTable.Any(p => p.IsApproved == true))
                        return Json(new { result = 0, message = "Salary already approved, insertion denied" }, JsonRequestBehavior.AllowGet);

                    if (checkMonthlySalaryTable.Any(p => p.IsSendForApproval == true))
                        return Json(new { result = 0, message = "Salary already send for approval, insertion denied" }, JsonRequestBehavior.AllowGet);

                    if (checkMonthlySalaryTable.Any(p => p.PRComponentId == prComponentId))
                        return Json(new { result = 0, message = "Salary already exist for this this month, insertion denied" }, JsonRequestBehavior.AllowGet);

                    if (prComponent != null)
                        prComponentId = prComponent.PRComponentID;

                    var checkDuplicateRefundSalary = employeeSalaryIncentiveService.GetIncentiveByComponentAndEmployeeId(prComponentId, (int)obj.EmployeeId);

                    if (checkDuplicateRefundSalary != null)
                        return Json(new { result = 0, message = "Incentive already exist for this month, Approve denied" }, JsonRequestBehavior.AllowGet);

                    var updateSalaryDeposit = employeeSalaryDepositService
                                                            .GetDepositedSalaryDepositByEmployeeId((int)obj.EmployeeId);

                    if (updateSalaryDeposit == null)
                        return Json(new { result = 0, message = "Employee salary deposit not found, Approve denied" }, JsonRequestBehavior.AllowGet);

                    var officeId = 0;
                    var officeInfo = employeeService.GetByEmpId(obj.EmployeeId);

                    if (officeInfo == null)
                        return Json(new { result = 0, message = "Employee Office not found not found, Approve denied" }, JsonRequestBehavior.AllowGet);

                    officeId = (int)officeInfo.OfficeId;

                    var entity = updateSalaryDeposit;

                    entity.PRComponentRefundId = prComponentId;
                    entity.IsRefundRequired = true;
                    entity.RefundDone = true;
                    entity.RefundStartDate = startDate;
                    entity.RefundEndDate = endDate;
                    entity.UpdateBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateDate = DateTime.UtcNow;

                    //let's update for [prl.EmployeeSalaryDeposit] 
                    employeeSalaryDepositService.Update(entity);
                    flag = 1;

                    var model = new EmployeeSalaryIncentive();
                    model.EmployeeId = obj.EmployeeId;
                    model.PRComponentId = prComponentId;
                    model.ProductId = 0;
                    model.SerialId = 0;
                    model.PRComponentAmount = obj.RefundAmount;
                    model.PRComponentHour = 0;
                    model.IsActive = true;
                    model.IsApproved = true;
                    model.StartDate = startDate;
                    model.EndDate = endDate;
                    model.CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;

                    //let's insert into [prl.EmployeeSalaryIncentive]
                    employeeSalaryIncentiveService.Create(model);
                    flag = 1;

                    var data = new EmployeeMonthlySalary();
                    data.PRComponentId = prComponentId;
                    data.EmployeeId = obj.EmployeeId;
                    //data.TransactionType = transactionType;
                    data.PRComponentAmount = obj.RefundAmount;
                    data.SalaryMonth = salaryMonth;
                    data.SalaryYear = salaryYear;
                    data.SalaryDate = endDate;
                    data.ComponentCategory = prComponent.ComponentCategory; //"Salary";
                    data.TransactionType = prComponent.TransactionType;
                    data.OfficeId = officeId;
                    data.IsApproved = false;
                    data.IsActive = true;

                    data.CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                    data.CreateDate = DateTime.UtcNow;
                    data.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                    data.UpdateDate = DateTime.UtcNow;

                    //let's insert into [prl.EmployeeMonthlySalary]
                    employeeMonthlySalaryService.Create(data);
                    flag = 1;
                }
                catch (Exception ex)
                {
                    flag = 0;
                    message = ex.InnerException.ToString();
                }

                if (flag == 1)
                {
                    scope.Complete();
                    result = 1;
                    message = "Approved successfully";
                }

                scope.Dispose();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get Deposits 

        public JsonResult GetDepositedInfo([DataSourceRequest]DataSourceRequest request, string FilterColumn, string FilterValue
            , DateTime fromDate, DateTime toDate)
        {            
            var startDate = Convert.ToDateTime(fromDate);
            var endDate = Convert.ToDateTime(toDate);
            
            var deposiedDoneList =
                employeeSalaryDepositService.GetAll()
                    .Where(
                        p =>
                            p.IsActive == true && p.DepositDone == true && p.EffectiveStartDate >= startDate &&
                            p.EffectiveEndDate <= endDate)
                    .ToList();


            DataSourceResult result = deposiedDoneList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepositedInfo_NotUsed(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            DateTime now = DateTime.UtcNow;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var deposiedDoneList =
                employeeSalaryDepositService.GetAll()
                    .Where(
                        p =>
                            p.IsActive == true && p.DepositDone == true && p.EffectiveStartDate >= startDate &&
                            p.EffectiveEndDate <= endDate)
                    .ToList();
            var currentPageRecords = deposiedDoneList.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = deposiedDoneList.LongCount(), JsonRequestBehavior.AllowGet });
        }
        
        #endregion

        #region Get Refunds 

        public JsonResult GetRefundInfo([DataSourceRequest]DataSourceRequest request, string FilterColumn, string FilterValue
            , DateTime fromDate, DateTime toDate)
        {
            var startDate = Convert.ToDateTime(fromDate);
            var endDate = Convert.ToDateTime(toDate);

            var refundDoneList =
                employeeSalaryDepositService.GetAll()
                    .Where(
                        p =>
                            p.IsActive == true && p.RefundDone == true && p.RefundStartDate >= startDate &&
                            p.RefundEndDate <= endDate)
                    .ToList();

            DataSourceResult result = refundDoneList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }       

        public JsonResult GetRefundInfo_NotUsed(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            DateTime now = DateTime.UtcNow;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var refundDoneList =
                employeeSalaryDepositService.GetAll()
                    .Where(
                        p =>
                            p.IsActive == true && p.RefundDone == true && p.EffectiveStartDate >= startDate &&
                            p.EffectiveEndDate <= endDate)
                    .ToList();
            var currentPageRecords = refundDoneList.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = refundDoneList.LongCount(), JsonRequestBehavior.AllowGet });
        }

        #endregion

        #region Private Methods


        private EmployeeMonthlySalary PupulateEmployeeMonthlySalary(PRDepositViewModel obj, int prComponentId, DateTime endDate, int salaryMonth, int salaryYear, int? officeId)
        {
            var newEmployeeMonthlySalary = new EmployeeMonthlySalary
            {
                PRComponentId = prComponentId,
                EmployeeId = obj.EmployeeId,
                TransactionType = obj.TransactionType,
                PRComponentAmount = obj.DepositeAmount,
                SalaryMonth = salaryMonth,
                SalaryYear = salaryYear,
                SalaryDate = endDate,
                ComponentCategory = "Deduction",
                IsApproved = false,
                IsActive = true,
                OfficeId = officeId,
                CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID),
                CreateDate = DateTime.UtcNow,
                UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID),
                UpdateDate = DateTime.UtcNow
            };

            return newEmployeeMonthlySalary;
        }

        private PRComponent PopulatePRComponent(PRDepositViewModel model)
        {
            return new PRComponent
            {
                ComponentPayrollId = model.ComponentPayrollId,
                ComponentName = model.ComponentName,
                ComponentType = SalaryCalculationTypeConstants.Fixed,
                ComponentAmount = 0,
                EmployeeTypeId = model.EmployeeType,
                EmployeeStatusId = model.EmployeeStatusId,
                TransactionType = model.TransactionType,
                PRComponentGroupID = model.ComponentGroupId,
                ComponentCategory = ComponentDepositCategoryConstants.Deposit,
                MaximumLimit = 0,
                MinimumLimit = 0,
                EffectiveStartDate = model.EffectiveStartDate.Date,
                EffectiveEndDate = model.EffectiveEndDate.Date,
                OfficeLocationId = model.OfficeLocationId,
                SalaryRoundType = SalaryRoundTypeConstants.NotApplicable,
                SalaryEffect = false,
                SalaryChangesByComponent = "N/A",
                IsSalaryImpactProhibited = false,
                IsProvidentFundComponent = false,
                IsProductDependent = false,
                SalaryAccCode = "0000",
                PFTypeId = 0,
                LoanCalculationId = 0,
                MinDuration = 0,
                MaxDuration = 0,
                IsAdjustable = false,
                RatioBasedOn = RatioBasedOnConstants.NotRequired,
                IsActive = true,
                CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID),
                CreateDate = DateTime.UtcNow,
                UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID),
                UpdateDate = DateTime.UtcNow,
            };
        }

        private EmployeeSalaryDeposit PopulateSalaryDeposit(int prComponentId, int prDepositComponentId, PRDepositViewModel obj, DateTime startDate, DateTime endDate)
        {
            var model = new EmployeeSalaryDeposit();
            model.PRComponentId = prComponentId;
            model.DepositComponentId = prDepositComponentId;
            model.PRComponentRefundId = 0;
            model.EmployeeId = obj.EmployeeId;
            model.EmployeeCode = obj.EmployeeCode;
            model.EmployeeName = obj.EmployeeName;
            model.TransactionType = obj.TransactionType;
            model.ComponentGroup = obj.ComponentGroup;
            model.DepositAmount = obj.DepositeAmount;
            model.DepositOnGrossSalary = obj.GrossSalary;
            model.NoOfSalaryDays = obj.NoOfSalaryDays ?? 0;
            model.DepositDone = true; //TODO: 
            model.RefundDone = false;
            model.EffectiveStartDate = startDate.Date;
            model.EffectiveEndDate = endDate.Date;
            model.IsDepositRequired = true;
            model.IsRefundRequired = false;
            model.IsActive = true;
            model.CreateBy = SessionHelper.LoggedInEmployeeID;
            model.CreateDate = DateTime.UtcNow;
            return model;
        }

        private EmployeeSalaryDeduction GenerateEmployeeSalaryDeductionEntity(PRDepositViewModel obj, int prComponentId, DateTime startDate, DateTime endDate)
        {
            var entity = new EmployeeSalaryDeduction();
            entity.EmployeeId = obj.EmployeeId;
            entity.ComponentId = prComponentId;
            entity.DeductedAmount = obj.DepositeAmount;
            entity.DeductionDays = obj.NoOfSalaryDays ?? 0;
            entity.IsActive = true;
            entity.StartDate = startDate;
            entity.EndDate = endDate;
            entity.ProductId = 0;
            entity.SerialId = 0;
            entity.IsApproved = true;
            entity.CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
            entity.CreateDate = DateTime.UtcNow;
            entity.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
            entity.UpdateDate = DateTime.UtcNow;
            return entity;
        }

        private TempDepositRequiredEmployee GenerateEntity(List<PRDeposit> prDepositType, int empType, PRDepositViewModel item, int empStatus, decimal grossSalary, decimal calculatedDepositAmount)
        {
            var entity = new TempDepositRequiredEmployee();
            entity.PRComponentId = prDepositType.First().PRComponentId;
            entity.EmployeeType = empType;
            entity.EmployeeTypeName = item.EmployeeTypeName;
            entity.EmployeeStatus = empStatus;
            entity.EmployeeStatusName = item.EmployeeStatusName;
            entity.DepositeType = prDepositType.First().DepositeType;
            entity.NoOfSalaryDays = prDepositType.First().NoOfSalaryDays;
            entity.GrossSalary = grossSalary;
            entity.EmployeeId = item.EmployeeId;
            entity.EmployeeCode = item.EmployeeCode;
            entity.DepositeAmount = Math.Round(calculatedDepositAmount, 2);
            entity.EmployeeName = item.EmployeeName;
            entity.TransactionType = prDepositType.First().TransactionType;
            entity.ComponentGroup = prDepositType.First().ComponentGroup;
            entity.ComponentName = prDepositType.First().ComponentName;
            entity.OfficeLocationId = item.OfficeLocationId;

            return entity;
        }

        private void MapDropDown(PRDepositViewModel entity)
        {
            entity.TransactionTypeList = commonStaticDropDown.SalaryAccountTransactionType(); ;

            entity.IsDepositRequiredList = commonStaticDropDown.YesNoDropDown_Int("Y", true, "3");

            var depositeType = new List<SelectListItem>();
            depositeType.Add(new SelectListItem { Text = "1st Month Salary", Value = "Salary" });
            entity.DepositeTypeList = depositeType;

            List<string> ContainList = new List<string>();
            ContainList.Add("Deposit");
            entity.ComponentList = commonDynamicDropDown.PayrollComponentContainByCategory(ContainList);
            entity.EmployeeTypeList = commonDynamicDropDown.ddlEmployeeType();
            entity.EmployeeStatusIdList = commonDynamicDropDown.ddlEmployeeStatusList(true);
            entity.ComponentGroupList = commonDynamicDropDown.PRComponentGroup_Only_SalaryOrDeduction();
            entity.OfficeLocationList = commonDynamicDropDown.OfficeLocationList();
        }

        private void MapDropDownForSalaryDepositAndRefund(PRDepositViewModel model)
        {
            var salDR = new List<SelectListItem>();
            salDR.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            salDR.Add(new SelectListItem() { Text = "Deposit Required", Value = "DR" });
            salDR.Add(new SelectListItem() { Text = "Refund Required", Value = "RR" });
            salDR.Add(new SelectListItem() { Text = "Deposited - Report", Value = "D" });
            salDR.Add(new SelectListItem() { Text = "Refunded - Report", Value = "R" });
            model.SalaryDepositAndRefundTypeList = salDR;

            model.YearList = DateHelper.GetYears(1, 7);
            model.MonthList = DateHelper.GetMonths();
        }

        #endregion
    }
}