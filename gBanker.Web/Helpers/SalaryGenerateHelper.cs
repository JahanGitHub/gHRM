using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using gHRM.Web.ViewModels;
using Newtonsoft.Json;
using BasicDataAccess;
using System.Data;
using System.Web.Mvc;
using gHRM.Core.Utilities.Constants;
using System.Globalization;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service.StoreProcedure;
using gHRM.Service.Payroll;
using gHRM.Service.payroll;
using gHRM.Service;
using gHRM.Service.PF;
using gHRM.Service.Loan;
using gHRM.Web.ViewModels.Payroll;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.DBDetailModels.Promotions;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Core.Filters.Payroll;
using System.Data.Entity.Validation;
using System.Transactions;
using System.Data.Entity;
using System.Configuration;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc.Async;
using System.Web.Mvc.Filters;
using System.Web.Profile;
using System.Web.Routing;

namespace gHRM.Web.Helpers
{
    public class SalaryGenerateHelper
    {

        #region Private Variables

        private readonly IEmployeeSPService employeeSPService;
        private readonly IEmployeeMonthlySalaryService employeeMonthlySalaryService;
        private readonly IPRSalaryRegisterService prSalaryRegisterService;
        private readonly IEmployeeMonthlySalaryExceptionService employeeMonthlySalaryExceptionService;
        private readonly IPRDepositService prDepositService;
        private readonly IEmployeeSalaryDepositService employeeSalaryDepositService;
        private readonly IEmployeeStatusHistoryService employeeStatusHistoryService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IPRComponentService prComponentService;
        private readonly IEmployeeMonthlySalaryApprovedService employeeMonthlySalaryApprovedService;
        private readonly IOfficeService officeService;
        private readonly IEmployeePromotionService employeePromotionService;
        private readonly ISalaryDateConfigService salaryDateConfigService;
        private readonly ICompanyWisePayrollConfigService companyWisePayrollConfigService;
        private readonly IEmployeeService employeeService;
        private readonly ITempPFCollectionService pfCollectionService;
        private readonly ILoanDisbursementService loanDisbursementService;
        private readonly ILoanPurposeService loanPurposeService;
        private readonly ILoanRegisterService loanRegisterService;
        public SalaryGenerateHelper(
              IEmployeeSPService employeeSPService
            , IEmployeeMonthlySalaryService employeeMonthlySalaryService
            , IPRComponentService prComponentService
            , IEmployeeMonthlySalaryApprovedService employeeMonthlySalaryApprovedService
            , IPRSalaryRegisterService prSalaryRegisterService
            , IEmployeeMonthlySalaryExceptionService employeeMonthlySalaryException
            , IEmployeeSalaryDepositService employeeSalaryDepositService
            , IPRDepositService prDepositService
            , IEmployeeStatusHistoryService employeeStatusHistoryService
            , IOfficeTypeService officeTypeService
            , IOfficeService officeService
            , IEmployeePromotionService employeePromotionService
            , ICompanyWisePayrollConfigService companyWisePayrollConfigService
            , ISalaryDateConfigService salaryDateConfigService
            , IEmployeeService employeeService
            , ITempPFCollectionService pfCollectionService
            , ILoanDisbursementService loanDisbursementService
            , ILoanPurposeService loanPurposeService
            , ILoanRegisterService loanRegisterService
        )
        {
            this.employeeSPService = employeeSPService;
            this.employeeMonthlySalaryService = employeeMonthlySalaryService;
            this.prComponentService = prComponentService;
            this.employeeMonthlySalaryApprovedService = employeeMonthlySalaryApprovedService;
            this.prSalaryRegisterService = prSalaryRegisterService;
            this.employeeMonthlySalaryExceptionService = employeeMonthlySalaryException;
            this.prDepositService = prDepositService;
            this.employeeSalaryDepositService = employeeSalaryDepositService;
            this.employeeStatusHistoryService = employeeStatusHistoryService;
            this.officeTypeService = officeTypeService;
            this.officeService = officeService;
            this.employeePromotionService = employeePromotionService;
            this.salaryDateConfigService = salaryDateConfigService;
            this.companyWisePayrollConfigService = companyWisePayrollConfigService;
            this.employeeService = employeeService;
            this.pfCollectionService = pfCollectionService;
            this.loanDisbursementService = loanDisbursementService;
            this.loanPurposeService = loanPurposeService;
            this.loanRegisterService = loanRegisterService;
        }

        #endregion

 
        public string MonthlySalaryProcess(string empType, string month, string salaryYear, int OfficeTypeId)
        {
            bool isOperationSuccess = true;

            string result = "";

            if (OfficeTypeId <= 0)
                return "Office Type Required";

            StringBuilder andCondition = new StringBuilder();
            andCondition.Append(" AND ems.SalaryYear=" + salaryYear);
            andCondition.Append(" AND ems.SalaryMonth=" + month);
            andCondition.Append(" AND ems.OfficeTypeId=" + OfficeTypeId);

            //get existing monthly salary EmployeeMonthlySalary
            var existingMonthlySalary = GetExistingMonthlySalary(andCondition);//Method 01

            var firstDate = new DateTime(Convert.ToInt32(salaryYear), Convert.ToInt32(month), 1);
            DateTime firstOfNextMonth = new DateTime(Convert.ToInt32(salaryYear), Convert.ToInt32(month), 1).AddMonths(1);
            var lastDate = firstOfNextMonth.AddDays(-1);

            //check validation before salary generation
            result = ValidateBeforeSalaryGeneration(existingMonthlySalary, firstDate, lastDate);//Method 02

            if (result != "OK")
                return result;

            //get current monthly salary application employees [PRSalaryConfiguration]
            var salaryconfigurations = GetCurrentMonthSalaryApplicableEmployee(firstDate, lastDate, OfficeTypeId)/*.Where(x => x.EmployeeCode == "289")*/.ToList();//Method 03

            var promotionlstObj = new gHRMDBContext().EmployeePromotion.Where(x => x.IsActive && !x.IsReviewed && (
                                                     DbFunctions.TruncateTime(x.PromotionDate) >= DbFunctions.TruncateTime(firstDate)
                                                     && DbFunctions.TruncateTime(x.PromotionDate) <= DbFunctions.TruncateTime(lastDate)
                                                 )).Select(x => new EmployeePromotionModel { EmployeeId = x.EmployeeId, PromotionDate = x.PromotionDate }).ToList();

            if (!salaryconfigurations.Any())
                return "Salary configuration not found. Please Configure salary first!";

            //get salary date configuration
            var salaryDateConfiguration = salaryDateConfigService.GetCurrentSalaryDateConfig();

            if (salaryDateConfiguration == null)
                return "Currently active salary Date configuration not found. Please Configure salary date first!";

            //var lastDayofSalary = new DateTime(Convert.ToInt32(salaryYear), Convert.ToInt32(month), 1).AddMonths(1).AddDays(-1).Day;
            int lastDayofSalary = DateTime.DaysInMonth(int.Parse(salaryYear), int.Parse(month));
            if (salaryDateConfiguration.DayOfMonthlySalary > lastDayofSalary)
                return "Salary date invalid. Please Configure valid salary date first!";


            var monInText = MonthConstants.GetText(month);
            int day = 0;

            if (salaryDateConfiguration.DayOfMonthlySalary > lastDayofSalary)
                day = lastDayofSalary;
            else
                day = salaryDateConfiguration.DayOfMonthlySalary;

            string salaryDate = $"{day}-{monInText.Substring(0, 3)}-{salaryYear}";
            DateTime dt_sd = DateTime.Now;
            DateTime.TryParse(salaryDate, out dt_sd);
            //var salarydateLst = new gHRMDBContext().EmployeeMonthlySalary.Where(x => x.IsActive && !x.IsApproved && x.SalaryDate < dt_sd).Select(x => x.SalaryDate).Distinct().OrderBy(x => x).ToList().Select(x=>x.ToString("dd-MMM-yyyy"));

            //if (salarydateLst.Any())
            //    return Json("Non approved salary found, Please check non approved salary. Salary Month are: " + string.Join(", ", salarydateLst) + "", JsonRequestBehavior.AllowGet);

            bool hasPF = false, usedLoanModule = false;
            if (ConfigurationManager.AppSettings["HasPF"] != null)
                hasPF = bool.Parse(ConfigurationManager.AppSettings["HasPF"].ToString());
            if (ConfigurationManager.AppSettings["UsedLoanModule"] != null)
                usedLoanModule = bool.Parse(ConfigurationManager.AppSettings["UsedLoanModule"].ToString());


            List<EmployeeMonthlySalary> loanForSalaryList = new List<EmployeeMonthlySalary>();
            List<EmployeeMonthlySalary> reglarDeductionList = new List<EmployeeMonthlySalary>();
            List<EmployeeMonthlySalary> regularIncList = new List<EmployeeMonthlySalary>();
            List<EmployeeMonthlySalary> employeeMonthSalaryLst = new List<EmployeeMonthlySalary>();
            List<EmployeeMonthlySalaryException> employeeMonthExceptionList = new List<EmployeeMonthlySalaryException>();
            List<EmployeeMonthlySalary> List1 = new List<EmployeeMonthlySalary>();
            List<TempPFCollection> List2 = new List<TempPFCollection>();
            var tup = Tuple.Create(List1,List2);
            var tup2 = Tuple.Create(employeeMonthSalaryLst, employeeMonthExceptionList);
            List<TempPFCollection> lst = new List<TempPFCollection>();

            try
            {
                var components = prComponentService.GetMany(p => p.IsActive == true).ToList();

                var loanEmployees = new List<LoanInstallmentDetail>();

                #region Manual Loan Part
                if (!usedLoanModule)
                {
                    loanEmployees = GetExistingMonthlyLoanDeduction(firstDate, lastDate, OfficeTypeId);
                }

                #endregion Manual Loan Part
                //get employee salary deduction [EmployeeSalaryDeduction]
                var deductedSalarys = GetEmployeesDeductedSalaryWithoutOtherImpactInSalary(firstDate, lastDate, OfficeTypeId); //Method 11                                                                      

                //get employee salary incentive [EmployeeSalaryIncentive]
                var approvedincentives = GetExistingMonthlyIncentivesWithoutOtherImpactInSalary(firstDate, lastDate, OfficeTypeId); //Method 13


                if (existingMonthlySalary.Any())
                {
                    //let's insert monthly salary history [EmployeeMonthlySalaryHistory] and remove from employee monthly salary [EmployeeMonthlySalary]
                    InsertMonthlySalaryHistory(Convert.ToInt32(salaryYear), Convert.ToInt32(month), OfficeTypeId); //Method 07
                }             

                #region Manual Loan Part
                if (!usedLoanModule)
                {
                    //let's insert loans in monthly salary [EmployeeMonthlySalary]
                    loanForSalaryList = InsertLoansInMonthlySalary(loanEmployees, month, salaryYear, salaryDate, components, salaryconfigurations); //Method 10
                }

                #endregion Insert
                //let's insert into employee monthly salary if any for employee salary deduction [EmployeeMonthlySalary]                                                                                                         
                reglarDeductionList =  InsertRegularSalaryDeduction(deductedSalarys, month, salaryYear, salaryDate, components, salaryconfigurations); //Method 16    
                                                                                                                            //let's insert regular incentives [EmployeeMonthlySalary]
                regularIncList = InsertRegularIncentives(approvedincentives, month, salaryYear, salaryDate, components, salaryconfigurations); //Method 15

                //let's insert new monthly salary into [prl.EmployeeMonthlySalaryException and prl.EmployeeMonthlySalary]
                    tup2 = InsertNewMonthlySalary(salaryconfigurations, components, OfficeTypeId, firstDate, lastDate, month, salaryYear, salaryDate, promotionlstObj); //Method 17

                //if (hasPF)
                //{
                //    // PF Insert into Temporary table gcpf.TempPFCollection
                //        lst = InsertPFTemporary(int.Parse(month), int.Parse(salaryYear), DateTime.Parse(salaryDate), employeeMonthSalaryLst, components);
                //    if (usedLoanModule)
                //    {
                //        // Loan
                //            tup =
                //InsertLoanTemporary(int.Parse(month), int.Parse(salaryYear), DateTime.Parse(salaryDate), lst, employeeMonthSalaryLst, components);
                            
                //    }
                      
                //}



                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(2, 0, 0)))
                {
                    if (loanForSalaryList.Any())
                        employeeMonthlySalaryService.AddEmployeeMonthlySalaryList(loanForSalaryList);

                    if (reglarDeductionList.Any())
                        employeeMonthlySalaryService.AddEmployeeMonthlySalaryList(reglarDeductionList);

                    if (regularIncList.Any())
                        employeeMonthlySalaryService.AddEmployeeMonthlySalaryList(regularIncList);


                    List<EmployeeMonthlySalary> item3 = tup2.Item1;
                    List<EmployeeMonthlySalaryException> item4 = tup2.Item2;                    

                    if (item3.Any())
                        employeeMonthlySalaryService.AddEmployeeMonthlySalaryList(item3);

                    if(item4.Any())
                        employeeMonthlySalaryExceptionService.AddEmplyoeeSalaryExceptionList(item4);

                    if (lst.Any())
                        pfCollectionService.AddBulk(lst);

                    List<EmployeeMonthlySalary> item1 = tup.Item1;
                    List<TempPFCollection> item2 = tup.Item2;

                    if (item1.Any())                   
                        employeeMonthlySalaryService.AddEmployeeMonthlySalaryList(item1);                  

                    //if (item2.Any())
                    //    pfCollectionService.AddBulk(item2);

                    if (existingMonthlySalary.Any())               
                        InActiveExceptionalSalaryDetail(firstDate, lastDate, OfficeTypeId); 

                    if (isOperationSuccess)
                        scope.Complete();

                    scope.Dispose();
                }
                result = "Process Successfull";
            }
            catch (DbEntityValidationException ex)
            {
                isOperationSuccess = false;
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                result = "There was an error while processing monthly salary!";
            }

            return result;
        }



        #region Salary Generate

        //Method 01
        private List<SalaryGenerationLog> GetExistingMonthlySalary(StringBuilder andCondition)
        {
            var param = new { @AndCondition = andCondition.ToString() };
            var list = employeeSPService.GetDataWithParameter(param, "prl.SP_GET_CurrentMonthSalary");

            var salaryList = list.Tables[0].AsEnumerable().Select(row => new SalaryGenerationLog()
            {
                SalaryYear = row.Field<int>("SalaryYear"),
                SalaryMonth = row.Field<int>("SalaryMonth"),
                IsActive = row.Field<bool>("IsActive"),
                IsApproved = row.Field<bool>("IsApproved"),
                IsSendForApproval = row.Field<bool>("IsSendForApproval"),
                IsRejected = row.Field<bool>("IsRejected")
            }).ToList();
            return salaryList;
        }

        //Method 02
        public string ValidateBeforeSalaryGeneration(List<SalaryGenerationLog> existingMonthlySalary, DateTime firstDate, DateTime lastDate)
        {
            var result = "OK";
            if (existingMonthlySalary.Any(p => p.IsApproved == true))
            {
                result = "Salary of this month for this office type is already approved";
                return result;
            }

            if (existingMonthlySalary.Any(p => p.IsSendForApproval == true))
            {
                result = "Salary of this month for this office type is already send for approval";
                return result;
            }

            //if (!CheckDepositDone())   //Method 02-01
            //{
            //    result = "Employee Salary Deposit Required, Monthly Salary Generation Denied";
            //    return result;
            //}

            //if (!checkRefundDone(firstDate, lastDate))   //Method 02-02
            //{
            //    result = "Employee Salary Refund Required, Monthly Salary Generation Denied";
            //    return result;
            //}

            return result;
        }


        //Method 02-01
        public bool CheckDepositDone()
        {
            bool paryrollCondition = true;
            DateTime day = DateTime.Now;
            var startDate = new DateTime(day.Year, day.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var employeeInfo = employeeSPService.GetDataWithoutParameter("prl.SP_GetEmployeeInfoForSalaryDeposit");
            var viewEmpInfo = employeeInfo.Tables[0].AsEnumerable().Select(p => new PRDepositViewModel()
            {
                EmployeeId = p.Field<long>("EmployeeId"),
                EmployeeCode = p.Field<string>("EmployeeCode"),
                EmployeeName = p.Field<string>("EmployeeName"),
                EmployeeType = p.Field<int>("EmployeeTypeId"),
                //IsSalaryApplicable = p.Field<bool?>("IsSalaryApplicable"),
                EmployeeTypeName = p.Field<string>("EmployeeTypeName"),
                //EmployeeStatusId = p.Field<int?>("EmployeeStatusId"),
                EmployeeStatusName = p.Field<string>("StatusName"),
                GrossSalary = p.Field<decimal>("GrossSalary")
            }).ToList();


            var prDepositType = prDepositService.GetAll().ToList();
            var checkEmployeeSalaryDepositandRefund = employeeSalaryDepositService.GetAll()
               .Where(p => p.IsActive == true && p.EffectiveStartDate == startDate && p.EffectiveEndDate == endDate).ToList();

            foreach (var item in viewEmpInfo)
            {
                var empStatus = item.EmployeeStatusId;
                var empType = item.EmployeeType;

                if (prDepositType.Where(p => p.EmployeeStatusId == empStatus && p.EmployeeType == empType && p.DepositeType != "NR").Any())
                {
                    var checkDepositRequired = checkEmployeeSalaryDepositandRefund.Where(p => p.EmployeeId == item.EmployeeId && p.IsActive == true).FirstOrDefault();
                    if (checkDepositRequired != null)
                    {
                        if (checkDepositRequired.IsDepositRequired == false)
                        {
                            return paryrollCondition = false;
                        }
                    }
                    else
                    {
                        return paryrollCondition = false;
                    }
                }
            }
            return paryrollCondition;
        }

        //Method 02-02
        public bool checkRefundDone(DateTime startDate, DateTime endDate)
        {
            var employeeStatusHistoryList =
                      employeeStatusHistoryService.GetAll()
                          .Where(p => p.IsActive == true && p.StartDate >= startDate)
                          .ToList();


            var isEmployeeInSalaryDeposit =
              employeeSalaryDepositService.GetAll()
                  .Where(p => p.IsActive == true && p.DepositDone == true && p.IsRefundRequired == false && p.EffectiveStartDate >= startDate && p.EffectiveEndDate <= endDate)
                  .ToList();


            foreach (var item in employeeStatusHistoryList)
            {
                var checkDepositStatus =
                    isEmployeeInSalaryDeposit.Where(p => p.EmployeeId == item.EmployeeId && p.IsActive == true && p.DepositDone == true)
                        .FirstOrDefault();
                if (checkDepositStatus != null)
                {
                    if (checkDepositStatus.IsRefundRequired == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //Method 03
        public List<PRSalaryConfigurationViewModel> GetCurrentMonthSalaryApplicableEmployee(DateTime firstDate, DateTime lastDate, int OfficeTypeId)
        {
            var param = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeId = OfficeTypeId };
            var salaryApplicableDS = employeeSPService.GetDataWithParameter(param, "prl.SP_GET_PREmployeeSalaryCurrentConfigurationAllEmployee");
            var salaryconfigurations = salaryApplicableDS.Tables[0].AsEnumerable().Select(row => new PRSalaryConfigurationViewModel()
            {
                PRSalaryConfigurationID = row.Field<long>("PRSalaryConfigurationID"),
                EmployeeID = row.Field<long>("EmployeeID"),
                PRComponentID = row.Field<int>("PRComponentID"),
                ComponentAmount = row.Field<decimal>("ComponentAmount"),
                EffectiveStartDate = row.Field<DateTime>("EffectiveStartDate"),
                EffectiveEndDate = row.Field<DateTime>("EffectiveEndDate"),
                IsActive = row.Field<bool>("IsActive"),
                ComponentCategory = row.Field<string>("ComponentCategory"),
                TransactionType = row.Field<string>("TransactionType"),
                OfficeID = row.Field<int>("OfficeID"),
                StatusName = row.Field<string>("StatusName"),
                PRWorkAreaID = row.Field<int>("PRWorkAreaID"),
                OfficeTypeId = row.Field<int>("OfficeTypeId"),
                EmployeeName = row.Field<string>("EmployeeName"),
                DepartmentId = row.Field<int>("DepartmentId"),
                DesignationId = row.Field<int>("DesignationId"),
                EmployeeRank = row.Field<string>("EmployeeRank"),
                EmployeeCode = row.Field<string>("EmployeeCode"),
                EmployeeStatusId = row.Field<int>("EmployeeStatusId"),
                BankCode = row.Field<string>("BankCode"),
                FirstJoiningDate = row.Field<DateTime>("FirstJoiningDate"),
                TotalEarnings = row.Field<decimal>("TotalEarnings"),
                GrossSalary = row.Field<decimal>("GrossSalary"),
                BasicSalary = row.Field<decimal>("BasicSalary"),
                EmployeeTypeId = row.Field<int>("EmployeeTypeId"),
                CompanyId = row.Field<int>("CompanyId"),
                GradeId = row.Field<int>("GradeId"),
                Step = row.Field<int>("Step"),
                OfficeLocationId = row.Field<int>("OfficeLocationId"),
                PFTypeId = row.Field<int>("PFTypeId"),
                SalaryRoundType = row.Field<string>("SalaryRoundType")
            }).ToList();

            return salaryconfigurations;
        }

        //Method 04
        public List<PRSalaryConfiguration> GetLastInActiveSalary(int OfficeTypeId, DateTime firstDate, DateTime lastDate)
        {
            var param2 = new { OfficeTypeID = OfficeTypeId, StartDate = firstDate, EndDate = lastDate };
            var lastConfiguredSalary = employeeSPService.GetDataWithParameter(param2, "prl.GETPRSalaryConfigurationInActive");
            var lastInActiveSalaryConfigurations = lastConfiguredSalary.Tables[0].AsEnumerable()
                 .Select(row => new PRSalaryConfiguration()
                 {
                     EmployeeID = row.Field<long>("EmployeeId"),
                     PRComponentID = row.Field<int>("PRComponentID"),
                     ComponentAmount = row.Field<decimal>("ComponentAmount"),
                     EffectiveStartDate = row.Field<DateTime>("EffectiveStartDate"),
                     ComponentCategory = row.Field<string>("ComponentCategory")
                 }).ToList();
            return lastInActiveSalaryConfigurations;
        }

        //Method 05
        public List<EmployeeMonthlySalaryApproved> GetLastApprovedSalary()
        {
            var lastApproved = employeeSPService.GetDataWithoutParameter("[prl].[EmployeeMonthlySalaryApproved_GetLastApprovedSalary]");
            var finalMonthlySalaryApproved = lastApproved.Tables[0].AsEnumerable()
                 .Select(row => new EmployeeMonthlySalaryApproved()
                 {
                     EmployeeId = row.Field<long>("EmployeeId"),
                     PRComponentId = row.Field<int>("PRComponentID"),
                     PRComponentAmount = row.Field<decimal>("PRComponentAmount"),
                     SalaryDate = row.Field<DateTime>("SalaryDate")
                 }).ToList();
            return finalMonthlySalaryApproved;
        }

        //Method 06
        public List<Employee> GetEmployeeDeductionRequiredByJoinigDate(int officeTypeId, DateTime lastApprovedSalaryDate, DateTime endDate)
        {
            var param = new { OfficeTypeID = officeTypeId, LastApprovedSalaryDate = lastApprovedSalaryDate, EndDate = endDate };
            var partialSalaryDS = employeeSPService.GetDataWithParameter(param, "prl.SP_GetEmployeeDeductionRequiredByJoinigDate");
            var employeeParitalSalary = partialSalaryDS.Tables[0].AsEnumerable()
                 .Select(row => new Employee()
                 {
                     EmployeeName = row.Field<string>("EmployeeName"),
                     EmployeeId = row.Field<long>("EmployeeId"),
                     OfficeId = row.Field<int>("OfficeId"),
                     DesignationId = row.Field<int>("DesignationId"),
                     EmployeeCode = row.Field<string>("EmployeeCode"),
                     EmployeeStatusId = row.Field<int>("EmployeeStatusId"),
                     FirstJoiningDate = row.Field<DateTime>("FirstJoiningDate"),
                     TotalEarnings = row.Field<decimal>("TotalEarnings"),
                     GrossSalary = row.Field<decimal>("GrossSalary"),
                     BasicSalary = row.Field<decimal>("BasicSalary"),
                     EmployeeTypeId = row.Field<int>("EmployeeTypeId"),
                     GradeId = row.Field<int>("GradeId"),
                     Step = row.Field<int>("Step"),
                 }).ToList();
            return employeeParitalSalary;
        }

        //Method 07
        private void InsertMonthlySalaryHistory(int salaryYear, int salaryMonth, int officeTypeId)
        {
            var param = new { SalaryYear = salaryYear, SalaryMonth = salaryMonth, UserAction = "Salary Regenerate", OfficeTypeId = officeTypeId };
            var val = employeeSPService.GetDataWithParameter(param, "prl.SP_InsertMonthlySalaryHistory");
        }

        //Method 08
        private void InActiveExceptionalSalaryDetail(DateTime firstDate, DateTime lastDate, int officeTypeID)
        {
            var param = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeID = officeTypeID };
            var list = employeeSPService.GetDataWithParameter(param, "prl.SP_InActive_ExceptionalSalary");
        }

        //Method 09
        private List<LoanInstallmentDetail> GetExistingMonthlyLoanDeduction(DateTime firstDate, DateTime lastDate, int officeTypeID)
        {
            var param = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeID = officeTypeID };
            var list = employeeSPService.GetDataWithParameter(param, "prl.SP_GET_PFLoanForAllEmployee");

            var loanDetailList = list.Tables[0].AsEnumerable().Select(row => new LoanInstallmentDetail()
            {
                InstallmentAmount = row.Field<decimal>("InstallmentAmount"),
                IsActive = row.Field<bool>("IsActive"),
                EmployeeId = row.Field<long>("EmployeeId"),
                PRComponentId = row.Field<int>("PRComponentId")
            }).ToList();
            return loanDetailList;
        }

        //Method 10
        private List<EmployeeMonthlySalary> InsertLoansInMonthlySalary(List<LoanInstallmentDetail> loanEmployees, string month, string salaryYear, string salaryDate, List<PRComponent> components,
            List<PRSalaryConfigurationViewModel> salaryconfigurations)
        {
            var loanForSalary = new List<EmployeeMonthlySalary>();
            foreach (var item in loanEmployees)
            {
                var empStChk = salaryconfigurations.Where(p => p.EmployeeID == item.EmployeeId).FirstOrDefault();
                if (empStChk != null)
                {
                    var component = components.Where(p => p.PRComponentID == item.PRComponentId).FirstOrDefault();
                    if (component != null)
                    {
                        var componentCategory = component.ComponentCategory;
                        var transactionType = component.TransactionType;
                        var entity = new EmployeeMonthlySalary();
                        entity.SalaryMonth = Convert.ToInt32(month);
                        entity.SalaryYear = Convert.ToInt32(salaryYear);
                        entity.SalaryDate = Convert.ToDateTime(salaryDate);
                        entity.EmployeeId = item.EmployeeId;
                        entity.PRComponentId = item.PRComponentId;
                        entity.PRComponentAmount = item.InstallmentAmount;
                        entity.IsActive = true;
                        entity.IsApproved = false;
                        entity.ComponentCategory = componentCategory;
                        entity.TransactionType = transactionType;
                        entity.CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                        entity.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                        entity.CreateDate = DateTime.Today;
                        entity.UpdateDate = DateTime.Today;
                        entity.OfficeId = empStChk.OfficeID;
                        loanForSalary.Add(entity);
                    }
                }
            }          
            
            return loanForSalary;          

        }

        //Method 11
        private List<EmployeeSalaryDeduction> GetEmployeesDeductedSalaryWithoutOtherImpactInSalary(DateTime firstDate, DateTime lastDate, int officeTypeId)
        {
            var param = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeID = officeTypeId };
            var list = employeeSPService.GetDataWithParameter(param, "prl.SP_GET_SalaryDeductionListAllEmployee");

            var deductionList = list.Tables[0].AsEnumerable().Select(row => new EmployeeSalaryDeduction()
            {
                Id = row.Field<int>("Id"),
                EmployeeId = row.Field<long>("EmployeeId"),
                ComponentId = row.Field<int>("ComponentId"),
                ProductId = row.Field<int>("ProductId"),
                SerialId = row.Field<int>("SerialId"),
                DeductedAmount = row.Field<decimal>("DeductedAmount"),
                IsActive = row.Field<bool>("IsActive"),
                IsApproved = row.Field<bool>("IsApproved"),
                StartDate = row.Field<DateTime>("StartDate"),
                EndDate = row.Field<DateTime>("EndDate")

            }).ToList();

            return deductionList;
        }

        //Method 12
        private List<EmployeeSalaryDeduction> GetEmployeesDeductedSalaryWithNegativeImpactInSalary(DateTime firstDate, DateTime lastDate, int officeTypeId)
        {
            var param = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeID = officeTypeId };
            var list = employeeSPService.GetDataWithParameter(param, "prl.SP_GET_SalaryDeductionListAllEmployeeWithNegativeSalaryImpact");

            var deductionList = list.Tables[0].AsEnumerable().Select(row => new EmployeeSalaryDeduction()
            {
                Id = row.Field<int>("Id"),
                EmployeeId = row.Field<long>("EmployeeId"),
                ComponentId = row.Field<int>("ComponentId"),
                ProductId = row.Field<int>("ProductId"),
                SerialId = row.Field<int>("SerialId"),
                DeductedAmount = row.Field<decimal>("DeductedAmount"),
                IsActive = row.Field<bool>("IsActive"),
                IsApproved = row.Field<bool>("IsApproved"),
                StartDate = row.Field<DateTime>("StartDate"),
                EndDate = row.Field<DateTime>("EndDate")
            }).ToList();

            return deductionList;
        }

        //Method 13
        private List<EmployeeSalaryIncentive> GetExistingMonthlyIncentivesWithoutOtherImpactInSalary(DateTime firstDate, DateTime lastDate, int officeTypeId)
        {
            var param = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeID = officeTypeId };
            var list = employeeSPService.GetDataWithParameter(param, "prl.SP_GET_SalaryIncentiveListAllEmployee");

            var incentiveList = list.Tables[0].AsEnumerable().Select(row => new EmployeeSalaryIncentive()
            {
                SalaryIncentiveId = row.Field<int>("SalaryIncentiveId"),
                EmployeeId = row.Field<long>("EmployeeId"),
                PRComponentId = row.Field<int>("PRComponentId"),
                ProductId = row.Field<int>("ProductId"),
                SerialId = row.Field<int>("SerialId"),
                PRComponentAmount = row.Field<decimal>("PRComponentAmount"),
                IsActive = row.Field<bool>("IsActive"),
                IsApproved = row.Field<bool>("IsApproved"),
                StartDate = row.Field<DateTime>("StartDate"),
                EndDate = row.Field<DateTime>("EndDate")

            }).ToList();
            return incentiveList;
        }

        // Method 14
        private List<EmployeeSalaryIncentive> GetExistingMonthlyIncentivesWithPositiveImpactInSalary(DateTime firstDate, DateTime lastDate, int officeTypeId)
        {
            var param = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeID = officeTypeId };
            var list = employeeSPService.GetDataWithParameter(param, "prl.SP_GET_SalaryIncentiveListAllEmployeeWithPositiveImpactInSalary");

            var incentiveList = list.Tables[0].AsEnumerable().Select(row => new EmployeeSalaryIncentive()
            {
                SalaryIncentiveId = row.Field<int>("SalaryIncentiveId"),
                EmployeeId = row.Field<long>("EmployeeId"),
                PRComponentId = row.Field<int>("PRComponentId"),
                ProductId = row.Field<int>("ProductId"),
                SerialId = row.Field<int>("SerialId"),
                PRComponentAmount = row.Field<decimal>("PRComponentAmount"),
                IsActive = row.Field<bool>("IsActive"),
                IsApproved = row.Field<bool>("IsApproved"),
                StartDate = row.Field<DateTime>("StartDate"),
                EndDate = row.Field<DateTime>("EndDate"),
                SalaryRoundType = row.Field<string>("SalaryRoundType")
            }).ToList();
            return incentiveList;
        }

        // Method 15
        private List<EmployeeMonthlySalary> InsertRegularIncentives(List<EmployeeSalaryIncentive> salaryIncentives, string month, string salaryYear, string salaryDate, List<PRComponent> components, List<PRSalaryConfigurationViewModel> salaryconfigurations)
        {
            List<EmployeeMonthlySalary> objEmployeeMonthlySalaries = new List<EmployeeMonthlySalary>();
            foreach (var item in salaryIncentives)
            {
                var employeeStatusCheck = salaryconfigurations.Where(p => p.EmployeeID == item.EmployeeId).FirstOrDefault();
                var component = components.Where(p => p.PRComponentID == item.PRComponentId).FirstOrDefault();

                if (employeeStatusCheck != null && component != null)
                {
                    var componentCategory = component.ComponentCategory;
                    var transactionType = component.TransactionType;
                    var entity = new EmployeeMonthlySalary();
                    entity.SalaryMonth = Convert.ToInt32(month);
                    entity.SalaryYear = Convert.ToInt32(salaryYear);
                    entity.SalaryDate = Convert.ToDateTime(salaryDate);
                    entity.EmployeeId = item.EmployeeId;
                    entity.PRComponentId = item.PRComponentId;
                    entity.PRComponentAmount = item.PRComponentAmount;
                    entity.TransactionType = transactionType;
                    entity.ComponentCategory = componentCategory;
                    entity.IsActive = true;
                    entity.IsApproved = false;
                    entity.CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                    entity.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.Today;
                    entity.UpdateDate = DateTime.Today;
                    entity.OfficeId = employeeStatusCheck.OfficeID;

                    objEmployeeMonthlySalaries.Add(entity);
                }
            }

            return objEmployeeMonthlySalaries;
        }

        // Method 16
        private List<EmployeeMonthlySalary> InsertRegularSalaryDeduction(List<EmployeeSalaryDeduction> salaryDiductions, string month, string salaryYear, string salaryDate, List<PRComponent> components, List<PRSalaryConfigurationViewModel> salaryconfigurations)
        {
            var id = 0;
            var objEmployeeMonthlySalaries = new List<EmployeeMonthlySalary>();
            foreach (var item in salaryDiductions)
            {
                id = item.Id;
                var employeeStatusCheck = salaryconfigurations.Where(p => p.EmployeeID == item.EmployeeId).FirstOrDefault();
                var component = components.Where(p => p.PRComponentID == item.ComponentId).FirstOrDefault();

                if (employeeStatusCheck != null && component != null)
                {
                    var componentCategory = component.ComponentCategory;
                    var transactionType = component.TransactionType;

                    var entity = new EmployeeMonthlySalary();
                    entity.SalaryMonth = Convert.ToInt32(month);
                    entity.SalaryYear = Convert.ToInt32(salaryYear);
                    entity.SalaryDate = Convert.ToDateTime(salaryDate);
                    entity.EmployeeId = item.EmployeeId;
                    entity.PRComponentId = item.ComponentId;
                    entity.PRComponentAmount = (item.DeductedAmount);
                    entity.ComponentCategory = componentCategory;
                    entity.TransactionType = transactionType;
                    entity.IsActive = true;
                    entity.IsApproved = false;
                    entity.OfficeId = employeeStatusCheck.OfficeID;
                    entity.CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                    entity.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.Now;
                    entity.UpdateDate = DateTime.Now;

                    objEmployeeMonthlySalaries.Add(entity);
                }
            }

            return objEmployeeMonthlySalaries;
        }

        // Method 17   middel confimation problem 
        private Tuple<List<EmployeeMonthlySalary>, List<EmployeeMonthlySalaryException>> InsertNewMonthlySalary(List<PRSalaryConfigurationViewModel> salaryconfigurations
          , List<PRComponent> components
          , int OfficeTypeId
          , DateTime firstDate
          , DateTime lastDate
          , string month
          , string salaryYear
          , string salaryDate
           , List<EmployeePromotionModel> promotionlst
          )
        {
            //get last inactive salary configuration from [PRSalaryConfiguration]
            var lastInActivatedSalary = GetLastInActiveSalary(OfficeTypeId, firstDate, lastDate);//Method 04

            //get employee information with [PRSalaryConfiguration]
            var partialDeductionSalary = GetEmployeeDeductionRequiredByJoinigDate(OfficeTypeId, firstDate, lastDate);//Method 06

            //get employee salary deduction [EmployeeSalaryDeduction] with 'Negative' impact on salary
            var deductionNegativeImpacts = GetEmployeesDeductedSalaryWithNegativeImpactInSalary(firstDate, lastDate, OfficeTypeId); //Method 12

            //get employee salary incentive [EmployeeSalaryIncentive] with 'Positive' impact on salary
            var incentivePositiveImpacts = GetExistingMonthlyIncentivesWithPositiveImpactInSalary(firstDate, lastDate, OfficeTypeId); //Method 14

            int daysInMonth = 0;
            var companyWisePayrollConfig = companyWisePayrollConfigService.GetByCompanyCode(SessionHelper.CompanyCode);

            if (companyWisePayrollConfig.PayrollType == PayrollTypeConstants.FixedDays)
                daysInMonth = companyWisePayrollConfig.NoOfSalaryDays;

            else //calendar Day
                //get days in month
                daysInMonth = DateTime.DaysInMonth(Convert.ToInt32(salaryYear), Convert.ToInt32(month));

            //get positive and negative impact component ids [PRComponent]
            IEnumerable<int> positiveImpactIds = PositiveImpactsComponent(components); // Method 17-1
            IEnumerable<int> negativeImpactIds = NegativeImpactsComponent(components); // Method 17-2

            List<EmployeeMonthlySalary> objEmployeeMonthlySalaries = new List<EmployeeMonthlySalary>();
            List<EmployeeMonthlySalaryException> empSalaryExceptions = new List<EmployeeMonthlySalaryException>();
            //get distinct employee salary configurations
            var distinctEmployeeinSalaryConfiguration = salaryconfigurations.GroupBy(g => g.EmployeeID)
                                                          .Select(s => s.First())
                                                          .ToList().OrderBy(p => p.EmployeeID);

            foreach (var item in distinctEmployeeinSalaryConfiguration)
            {
                double grossOrBasicSalary = 0;

                //get first employee salary configuration for this employee [PRSalaryConfiguration]
                var empSalInformation = salaryconfigurations.Where(p => p.EmployeeID == item.EmployeeID && p.IsActive == true ).FirstOrDefault();

                if (empSalInformation == null)
                    continue;

                //get employee salary configuration list for this employee [PRSalaryConfiguration]
                var empRegularSalaryConfigurations = salaryconfigurations.Where(p => p.EmployeeID == item.EmployeeID && p.IsActive == true).ToList();

                var tempSalaryConfiguration = new List<PRSalaryConfigurationViewModel>();
                var tempSalaryList = new List<PRSalaryScaleViewModel>();
                var exceptionEmployee = new EmployeeMonthlySalaryException();

                //get employee related information from salary configuration
                var employeeCode = empSalInformation.EmployeeCode;
                var employeeTypeId = Convert.ToInt32(empSalInformation.EmployeeTypeId);

                var currentGrossSalary = Convert.ToDouble(empSalInformation.GrossSalary);
                var currentBasicSalary = Convert.ToDouble(empSalInformation.BasicSalary);

                //get gross or basic depending on company wise Payroll Configuration Type
                grossOrBasicSalary = currentGrossSalary;
                if (companyWisePayrollConfig.PayrollConfigurationType == PayrollConfigurationTypeConstants.Basic)
                    grossOrBasicSalary = currentBasicSalary;

                var employeeStatusId = Convert.ToInt32(empSalInformation.EmployeeStatusId);
                var employeeOfficeLocationid = Convert.ToInt32(empSalInformation.OfficeLocationId);
                var PFTypeId = Convert.ToInt32(empSalInformation.PFTypeId);
                var officeId = Convert.ToInt32(empSalInformation.OfficeID);

                var positives = incentivePositiveImpacts.Where(p => positiveImpactIds.Any(a => a == p.PRComponentId) && p.EmployeeId == item.EmployeeID).ToList();
                var negatives = deductionNegativeImpacts.Where(p => negativeImpactIds.Any(a => a == p.ComponentId) && p.EmployeeId == item.EmployeeID).ToList();

                // double positiveImpactAmount = Convert.ToDouble(positives.Sum(p => p.PRComponentAmount));

                double positiveImpactAmount = 0;
                if (SessionHelper.CompanyInfo.CompanyShortName == "GTT")
                {
                    var db = new gHRMDBContext();
                    var arrearNotDistributedIds = db.PRComponents
                                                    .Where(z => z.ComponentName == "Arrear not distributed")
                                                    .Select(k => k.PRComponentID)
                                                    .ToList();

                    positiveImpactAmount = Convert.ToDouble(positives
                        .Where(p => !arrearNotDistributedIds.Contains(p.PRComponentId)) // Filter out excluded components
                        .Sum(p => p.PRComponentAmount)); // Sum the amounts
                }
                else
                {
                    positiveImpactAmount = Convert.ToDouble(positives.Sum(p => p.PRComponentAmount));
                }


                double negativeImpactAmount = Convert.ToDouble(negatives.Sum(p => p.DeductedAmount));

                string incentiveItems = string.Empty;
                string deductionItems = string.Empty;
                string exceptionRemarks = string.Empty;

                var tmpComponentPos = new List<TempComponent>();
                var tmpComponentNeg = new List<TempComponent>();

                //populate temp positive impacts
                tmpComponentPos = PopulateTempPositiveImpacts(components, positives);

                //populate temp negative impacts
                tmpComponentNeg = PopulateTempNegativeImpacts(components, negatives);

                incentiveItems = ConcatItems(tmpComponentPos);   // Method 17-3
                deductionItems = ConcatItems(tmpComponentNeg);   // Method 17-3

                double grossOrBasicChangesAmount = (grossOrBasicSalary + positiveImpactAmount) - negativeImpactAmount;

                //Re-generate temporary salary for the month

                if (grossOrBasicSalary != grossOrBasicChangesAmount)
                {
                    if (SessionHelper.CompanyInfo.CompanyShortName == "GTT")
                    {
                        tempSalaryList = ReGenerateTemporarySalaryForTheMonth(employeeTypeId, employeeStatusId, grossOrBasicChangesAmount,
                                                        grossOrBasicSalary, employeeOfficeLocationid, PFTypeId, officeId, item.EmployeeID, month, salaryYear);  // Method 17-4

                        tempSalaryConfiguration = ReGenerateEmployeeSalaryForPositiveOrNegativeImpact(tempSalaryList, firstDate, lastDate,
                                                        empSalInformation);  // Method 17-5
                    }
                    else
                    {
                        tempSalaryList = ReGenerateTemporarySalaryForTheMonth(employeeTypeId, employeeStatusId, grossOrBasicChangesAmount,
                                                        grossOrBasicSalary, employeeOfficeLocationid, PFTypeId, officeId, item.EmployeeID, month, salaryYear);  // Method 17-4

                        tempSalaryConfiguration = ReGenerateEmployeeSalaryForPositiveOrNegativeImpact(tempSalaryList, firstDate, lastDate,
                                                        empSalInformation);  // Method 17-5
                    }
                }


                //partial deduction Salary means: employee information with [PRSalaryConfiguration]
                var checkPartialGeneration = partialDeductionSalary.FirstOrDefault(p => p.EmployeeId == item.EmployeeID);



                // Tazdik

                //var db = new gHRMDBContext();
                // var emp = db.Employees.FirstOrDefault(e => e.EmployeeId == item.EmployeeID);
                // if (emp != null)
                // {
                //     DateTime? joiningDate = emp.AgreementFromDate;
                //     DateTime? confirmationDate = emp.ConfirmationDate;

                //     // Default = current configured salary
                //     decimal monthlySalary = Convert.ToDecimal(grossOrBasicSalary);
                //     decimal dbasic = 0;

                //     // Case: New join this month
                //     if (joiningDate != null && joiningDate.Value >= firstDate && joiningDate.Value <= lastDate)
                //     {
                //         int totalDays = daysInMonth;
                //         int postJoinDays = (lastDate - joiningDate.Value).Days + 1;
                //         decimal perDaySalary = monthlySalary / totalDays;

                //         // If confirmation also happens in same month
                //         if (confirmationDate != null && confirmationDate.Value >= joiningDate.Value && confirmationDate.Value <= lastDate)
                //         {
                //             int preConfirmDays = (confirmationDate.Value - joiningDate.Value).Days;
                //             int postConfirmDays = totalDays - (joiningDate.Value.Day + preConfirmDays - 1);

                //             decimal preConfirmSalary = perDaySalary * preConfirmDays;
                //             decimal postConfirmSalary = (perDaySalary * postConfirmDays) - ((perDaySalary * postConfirmDays) * 0.10m);

                //             dbasic = preConfirmSalary + postConfirmSalary;
                //         }
                //         else
                //         {
                //             // Just joined, not confirmed yet
                //             dbasic = perDaySalary * postJoinDays;
                //         }
                //     }
                //     // Case: Already working, confirmed in this month
                //     else if (confirmationDate != null && confirmationDate >= firstDate && confirmationDate <= lastDate)
                //     {
                //         int totalDays = daysInMonth;
                //         int preConfirmDays = (confirmationDate.Value - firstDate).Days;
                //         int postConfirmDays = totalDays - preConfirmDays;

                //         decimal perDaySalary = monthlySalary / totalDays;

                //         decimal preConfirmSalary = perDaySalary * preConfirmDays;
                //         decimal postConfirmSalary = (perDaySalary * postConfirmDays) - ((perDaySalary * postConfirmDays) * 0.10m);

                //         dbasic = preConfirmSalary + postConfirmSalary;
                //     }
                //     // Case: Promotion in this month
                //     var promotion = promotionlst.FirstOrDefault(p => p.EmployeeId == item.EmployeeID &&
                //                                                      p.PromotionDate >= firstDate &&
                //                                                      p.PromotionDate <= lastDate);
                //     if (promotion != null)
                //     {
                //         int totalDays = daysInMonth;
                //         int prePromotionDays = (promotion.PromotionDate - firstDate).Days;
                //         int postPromotionDays = totalDays - prePromotionDays;

                //         decimal oldSalary = monthlySalary;
                //         decimal newSalary = Convert.ToDecimal(promotion.NewSalary);

                //         decimal prePromotionSalary = (oldSalary / totalDays) * prePromotionDays;
                //         decimal postPromotionSalary = (newSalary / totalDays) * postPromotionDays;

                //         // If confirmation already done, PF applies after promotion
                //         if (confirmationDate != null && confirmationDate <= promotion.PromotionDate)
                //         {
                //             postPromotionSalary -= postPromotionSalary * 0.10m;
                //         }

                //         dbasic = prePromotionSalary + postPromotionSalary;
                //     }

                //     // Assign to employee salary object
                //     empSalInformation.DBasic = Convert.ToDouble(dbasic);
                // }

                // Tazdik

                #region Generate Salary when salary is configured after the first date of month


                // Generate Salary when salary is configured after the first date of month
                if (checkPartialGeneration != null)
                {
                    var dateDifference = 0;
                    var firstDayOfSalary = checkPartialGeneration.FirstJoiningDate.Day;

                    if (lastDate.Day <= 30)
                    {
                        if(lastDate.Day == 29)
                            dateDifference = (29 - firstDayOfSalary) + 1;
                        else if(lastDate.Day == 28)
                            dateDifference = (28 - firstDayOfSalary) + 1;
                        else
                            dateDifference = (30 - firstDayOfSalary) + 1;
                    }                        
                    else if (lastDate.Day > 30)
                        dateDifference = (31 - firstDayOfSalary) + 1;

                    if (grossOrBasicSalary == grossOrBasicChangesAmount)
                    {
                        //Generate Partial employee monthly salary for newly joined employee
                        var salaryObject = GeneratePartialSalaryForNewJoinedEmployee(dateDifference, daysInMonth, empRegularSalaryConfigurations, month, salaryYear, salaryDate, components);    // Method 17-7
                        if (salaryObject.Count > 0)
                            objEmployeeMonthlySalaries.AddRange(salaryObject);
                    }
                    else
                    {
                        //Generate Partial employee monthly salary for newly joined employee
                        var salaryObject = GeneratePartialSalaryForNewJoinedEmployee(dateDifference, daysInMonth, tempSalaryConfiguration, month, salaryYear, salaryDate, components);      // Method 17-7
                        if (salaryObject.Count > 0)
                            objEmployeeMonthlySalaries.AddRange(salaryObject);
                    }

                    //ChangesInGrossAmountFortheMonth
                    var grossForTheMonth = ChangesInGrossAmountFortheMonth(dateDifference, daysInMonth, grossOrBasicChangesAmount); // Method 17-6

                    //generate remarks
                    exceptionRemarks = GenerateExceptionalEmployeeSalaryCondition(dateDifference, positiveImpactAmount, negativeImpactAmount, grossForTheMonth, "Partial", incentiveItems, deductionItems);   // Method 17-8

                    //let's populate temporary employee monthly salary exception listing [EmployeeMonthlySalaryException]
                    exceptionEmployee = GenerateExceptionList(item.EmployeeID, employeeCode, firstDate, lastDate, exceptionRemarks);   // Method 17-9
                    empSalaryExceptions.Add(exceptionEmployee);
                }

                #endregion

                #region Generate Salary when salary is configured before the first date of month

                if (checkPartialGeneration == null)
                {
                    //from [EmployeePromotion]
                    //var employeePromotion = employeePromotionService.GetEmployeePromotionByDateRange(item.EmployeeID, firstDate, lastDate);
                    if (promotionlst != null)
                    {
                        var employeePromotion = promotionlst.Where(x => x.EmployeeId == item.EmployeeID);
                        DateTime effectiveStartDate = empRegularSalaryConfigurations.Max(p => p.EffectiveStartDate);
                        if (employeePromotion.Any())
                        {
                            //
                            DateTime salaryDt = Convert.ToDateTime(salaryDate);
                            int salMonth = (salaryDt.Month == 1 ? 12 : salaryDt.Month - 1);
                            int salYear = (salaryDt.Month == 1 ? salaryDt.Year - 1 : salaryDt.Year);
                            var lastSalary = new gHRMDBContext().EmployeeMonthlySalaryApproved.Where(x => x.IsActive && x.EmployeeId == item.EmployeeID && x.IsApproved && x.SalaryMonth == salMonth && x.SalaryYear == salYear).ToList();
                            // Generate Salary when increment or decrement found not from the first date of month
                            if (lastSalary.Any())
                            {
                                var effectiveDateDifferencewithFirstDate = Convert.ToInt32((effectiveStartDate - firstDate).TotalDays);
                                if (grossOrBasicSalary == grossOrBasicChangesAmount)
                                {
                                    //var salaryObject = GeneratePartialSalaryEmployeeIncrement(effectiveDateDifferencewithFirstDate, daysInMonth, empRegularSalaryConfigurations, month, salaryYear, salaryDate, components, checkLastConfiguredSalary, item.EmployeeID);  // Method 17-12
                                    var salaryObject = GeneratePartialSalaryEmployeeIncrement_New(effectiveDateDifferencewithFirstDate, daysInMonth, empRegularSalaryConfigurations, month, salaryYear, salaryDate, components, lastSalary, item.EmployeeID);  // Method 17-12
                                    if (salaryObject.Count > 0)
                                        objEmployeeMonthlySalaries.AddRange(salaryObject);

                                    var grossForTheMonth = ChangesInGrossAmountFortheMonth(daysInMonth, daysInMonth, grossOrBasicChangesAmount);  // Method 17-6
                                    exceptionRemarks = GenerateExceptionalEmployeeSalaryConditionIncrement(daysInMonth, positiveImpactAmount, negativeImpactAmount, grossForTheMonth, "Increment inside month"); // Method 17-11
                                    exceptionEmployee = GenerateExceptionList(item.EmployeeID, employeeCode, firstDate, lastDate, exceptionRemarks);  // Method 17-9
                                    empSalaryExceptions.Add(exceptionEmployee);
                                }
                                else
                                {
                                    var checkLastConfiguredSalary = lastInActivatedSalary.Where(p => p.EmployeeID == item.EmployeeID).ToList();
                                    var salaryObject = GeneratePartialSalaryEmployeeIncrement(effectiveDateDifferencewithFirstDate, daysInMonth, tempSalaryConfiguration, month, salaryYear, salaryDate, components, checkLastConfiguredSalary, item.EmployeeID); // Method 17-12
                                    tempSalaryConfiguration = new List<PRSalaryConfigurationViewModel>();

                                    if (salaryObject.Count > 0)
                                        objEmployeeMonthlySalaries.AddRange(salaryObject);


                                    var grossForTheMonth = ChangesInGrossAmountFortheMonth(daysInMonth, daysInMonth, grossOrBasicChangesAmount);  // Method 17-6
                                    exceptionRemarks = GenerateExceptionalEmployeeSalaryCondition(daysInMonth, positiveImpactAmount, negativeImpactAmount, grossForTheMonth, "Increment inside month", incentiveItems, deductionItems);
                                    exceptionEmployee = GenerateExceptionList(item.EmployeeID, employeeCode, firstDate, lastDate, exceptionRemarks);  // Method 17-9
                                    empSalaryExceptions.Add(exceptionEmployee);
                                }
                            }
                        }
                        else
                        {
                            if (grossOrBasicChangesAmount == grossOrBasicSalary)
                            {
                                var salaryObject = PopulateRegularSalaryForEmployee(empRegularSalaryConfigurations, month, salaryYear, salaryDate
                                    , PFTypeId, employeeTypeId, employeeStatusId, employeeOfficeLocationid, item.EmployeeID); // Method 17-10

                                if (salaryObject.Count > 0)
                                    objEmployeeMonthlySalaries.AddRange(salaryObject);
                            }
                            else
                            {
                                var salaryObject = GenerateRegularSalaryForEmployee(tempSalaryConfiguration, month, salaryYear, salaryDate); // Method 17-10
                                tempSalaryConfiguration = new List<PRSalaryConfigurationViewModel>();

                                if (salaryObject.Count > 0)
                                    objEmployeeMonthlySalaries.AddRange(salaryObject);

                                //ChangesInGrossAmountFortheMonth
                                var grossForTheMonth = ChangesInGrossAmountFortheMonth(daysInMonth, daysInMonth, grossOrBasicChangesAmount);  // Method 17-6

                                //generate remarks
                                exceptionRemarks = GenerateExceptionalEmployeeSalaryCondition(daysInMonth, positiveImpactAmount, negativeImpactAmount, grossForTheMonth, "Positive Or Negative Impact", incentiveItems, deductionItems); // Method 17-8

                                //let's populate temporary employee monthly salary exception listing [EmployeeMonthlySalaryException]
                                exceptionEmployee = GenerateExceptionList(item.EmployeeID, employeeCode, firstDate, lastDate, exceptionRemarks);  // Method 17-9
                                empSalaryExceptions.Add(exceptionEmployee);
                            }
                        }
                        //employeePromotionService.GetEmployeePromotionByDateRange(item.EmployeeID, firstDate, lastDate);



                        // Generate Salary when salary is configured before the first date of month
                        //    if (employeePromotion == null)
                        //    {
                        //        if (grossOrBasicChangesAmount == grossOrBasicSalary)
                        //        {
                        //            var salaryObject = PopulateRegularSalaryForEmployee(empRegularSalaryConfigurations, month, salaryYear, salaryDate
                        //                , PFTypeId, employeeTypeId, employeeStatusId, employeeOfficeLocationid, item.EmployeeID); // Method 17-10

                        //            if (salaryObject.Count > 0)
                        //                objEmployeeMonthlySalaries.AddRange(salaryObject);
                        //        }
                        //        else
                        //        {
                        //            var salaryObject = GenerateRegularSalaryForEmployee(tempSalaryConfiguration, month, salaryYear, salaryDate); // Method 17-10
                        //            tempSalaryConfiguration = new List<PRSalaryConfigurationViewModel>();

                        //            if (salaryObject.Count > 0)
                        //                objEmployeeMonthlySalaries.AddRange(salaryObject);

                        //            //ChangesInGrossAmountFortheMonth
                        //            var grossForTheMonth = ChangesInGrossAmountFortheMonth(daysInMonth, daysInMonth, grossOrBasicChangesAmount);  // Method 17-6

                        //            //generate remarks
                        //            exceptionRemarks = GenerateExceptionalEmployeeSalaryCondition(daysInMonth, positiveImpactAmount, negativeImpactAmount, grossForTheMonth, "Positive Or Negative Impact", incentiveItems, deductionItems); // Method 17-8

                        //            //let's populate temporary employee monthly salary exception listing [EmployeeMonthlySalaryException]
                        //            exceptionEmployee = GenerateExceptionList(item.EmployeeID, employeeCode, firstDate, lastDate, exceptionRemarks);  // Method 17-9
                        //            empSalaryExceptions.Add(exceptionEmployee);
                        //        }
                        //    }
                        //    // Generate Salary when salary is configured after the first date of month that means increment or decrement
                        //    else
                        //    {
                        //        var checkLastConfiguredSalary = lastInActivatedSalary.Where(p => p.EmployeeID == item.EmployeeID).ToList();

                        //        // Generate Salary when increment or decrement found not from the first date of month
                        //        if (checkLastConfiguredSalary.Any())
                        //        {
                        //            var effectiveDateDifferencewithFirstDate = Convert.ToInt32((effectiveStartDate - firstDate).TotalDays);
                        //            if (grossOrBasicSalary == grossOrBasicChangesAmount)
                        //            {
                        //                var salaryObject = GeneratePartialSalaryEmployeeIncrement(effectiveDateDifferencewithFirstDate, daysInMonth, empRegularSalaryConfigurations, month, salaryYear, salaryDate, components, checkLastConfiguredSalary, item.EmployeeID);  // Method 17-12
                        //                if (salaryObject.Count > 0)
                        //                    objEmployeeMonthlySalaries.AddRange(salaryObject);

                        //                var grossForTheMonth = ChangesInGrossAmountFortheMonth(daysInMonth, daysInMonth, grossOrBasicChangesAmount);  // Method 17-6
                        //                exceptionRemarks = GenerateExceptionalEmployeeSalaryConditionIncrement(daysInMonth, positiveImpactAmount, negativeImpactAmount, grossForTheMonth, "Increment inside month"); // Method 17-11
                        //                exceptionEmployee = GenerateExceptionList(item.EmployeeID, employeeCode, firstDate, lastDate, exceptionRemarks);  // Method 17-9
                        //                empSalaryExceptions.Add(exceptionEmployee);
                        //            }
                        //            else
                        //            {
                        //                var salaryObject = GeneratePartialSalaryEmployeeIncrement(effectiveDateDifferencewithFirstDate, daysInMonth, tempSalaryConfiguration, month, salaryYear, salaryDate, components, checkLastConfiguredSalary, item.EmployeeID); // Method 17-12
                        //                tempSalaryConfiguration = new List<PRSalaryConfigurationViewModel>();

                        //                if (salaryObject.Count > 0)
                        //                    objEmployeeMonthlySalaries.AddRange(salaryObject);


                        //                var grossForTheMonth = ChangesInGrossAmountFortheMonth(daysInMonth, daysInMonth, grossOrBasicChangesAmount);  // Method 17-6
                        //                exceptionRemarks = GenerateExceptionalEmployeeSalaryCondition(daysInMonth, positiveImpactAmount, negativeImpactAmount, grossForTheMonth, "Increment inside month", incentiveItems, deductionItems);
                        //                exceptionEmployee = GenerateExceptionList(item.EmployeeID, employeeCode, firstDate, lastDate, exceptionRemarks);  // Method 17-9
                        //                empSalaryExceptions.Add(exceptionEmployee);
                        //            }
                        //        }
                    }
                }

                #endregion
            }

           // employeeMonthlySalaryExceptionService.AddEmplyoeeSalaryExceptionList(empSalaryExceptions);
           // employeeMonthlySalaryService.AddEmployeeMonthlySalaryList(objEmployeeMonthlySalaries);

            return Tuple.Create(objEmployeeMonthlySalaries, empSalaryExceptions);

           // return objEmployeeMonthlySalaries;
        }
        #region PF & Loan
        private List<TempPFCollection> InsertPFTemporary(int salarymonth, int salaryYear, DateTime salaryDate
            , List<EmployeeMonthlySalary> employeeMonthSalaryLst, List<PRComponent> components)
        {
            var objLst = (from ms in employeeMonthSalaryLst
                          join c in components on ms.PRComponentId equals c.PRComponentID
                          where c.IsActive == true && ms.IsActive == true && ms.IsApproved == false && ms.IsRejected == false && ms.IsSendForApproval == false
                          && ms.SalaryMonth == salarymonth && ms.SalaryYear == salaryYear
                          && c.TransactionType == "Dr" && (c.IsProvidentFundComponent == true || c.ComponentCategory == "Loan")
                          select new
                          {
                              EmployeeId = ms.EmployeeId,
                              OfficeId = ms.OfficeId,
                              ComponentName = c.ComponentName,
                              PRComponentAmount = ms.PRComponentAmount
                          }).ToList();



            List<TempPFCollection> modellst = new List<TempPFCollection>();
            if (objLst.Any())
            {
                var emp = objLst.Select(x => new { EmployeeId = x.EmployeeId, OfficeId = x.OfficeId }).OrderBy(x => x.EmployeeId).Distinct().ToList();



                foreach (var e in emp)
                {
                    if (objLst.Where(x => x.EmployeeId == e.EmployeeId && (x.ComponentName.Contains("Employee") || x.ComponentName.Contains("Self"))).Any())
                    {

                        TempPFCollection model = new TempPFCollection()
                        {
                            EmployeeId = e.EmployeeId,
                            OfficeID = (e.OfficeId ?? 0),
                            EmployeeContribution = objLst.FirstOrDefault(x => x.EmployeeId == e.EmployeeId && (x.ComponentName.Contains("Employee") || x.ComponentName.Contains("Self")))?.PRComponentAmount,
                            OfficeContribution = objLst.FirstOrDefault(x => x.EmployeeId == e.EmployeeId && x.ComponentName.Contains("Office")).PRComponentAmount,
                            PFDistributionMonth = salarymonth,
                            PFDistributionYear = salaryYear,
                            PFDistributionDate = salaryDate
                        };
                        //--------------------------- Loan Collection
                        modellst.Add(model);
                    }
                }

            }
            return modellst;
        }

        private Tuple<List<EmployeeMonthlySalary>,List<TempPFCollection>> InsertLoanTemporary22(int salarymonth, int salaryYear, DateTime salaryDate
            , List<TempPFCollection> pf_modelLst, List<EmployeeMonthlySalary> employeeMonthSalaryLst, List<PRComponent> components)
        {
            //  employeeMonthSalaryLst = employeeMonthSalaryLst.Where(x => x.EmployeeId == 29).ToList();
            List<EmployeeMonthlySalary> objEmpMonthlySalary = new List<EmployeeMonthlySalary>();
            var purposeLst = loanPurposeService.GetMany(x => x.IsActive);
            var componentLst = components.Where(x => x.ComponentCategory == "Loan");
            var loanCalculation = new gHRMDBContext().prlLoanCalculation.Where(x => (x.IsActive ?? false));

            if (purposeLst.Any() && componentLst.Any())
            {
                
                var disburseLst = loanDisbursementService.GetMany(x => !(x.IsDeleted ?? false) && !x.IsClose).OrderBy(x => x.EmployeeId);
                var empLst = (from e in employeeMonthSalaryLst
                              join d in disburseLst on e.EmployeeId equals d.EmployeeId
                              select e.EmployeeId).Distinct().OrderBy(x => x).ToList();
                var employeeLst = employeeService.GetMany(x => empLst.Contains(x.EmployeeId));
                var loanidLst = disburseLst.Select(x => x.LoanId).Distinct();
                var collectionLastDateLstFromLoanRegister = new gHRMDBContext().LoanRegister.Where(x => loanidLst.Contains(x.LoanId) && !(x.IsDeleted ?? false))
                    .GroupBy(g => g.LoanId)
                    .Select(s => new
                    {
                        LoanId = s.Key,
                        TransactionDate = s.Max(x => x.TransactionDate),
                        PaidAmount = s.Sum(x => x.LoanAmount),
                        PaidInterestAmount = s.Sum(x => x.InterestAmount),
                        InterestCharge = s.Sum(x => x.InterestCharge)
                    });

                foreach (var emp in employeeLst)
                {
                    foreach (var d in disburseLst.Where(x => x.EmployeeId == emp.EmployeeId))
                    {
                        //var purpose = purposeLst.First(x => x);
                        var loancomponent = (from c in componentLst
                                             join l in loanCalculation on c.LoanCalculationId equals l.LoanCalculationId
                                             join p in purposeLst on c.ComponentName equals p.PurposeName
                                             where p.PurposeId == d.PurposeId //&& c.EmployeeStatusId == emp.EmployeeStatusId
                                             && c.EmployeeTypeId == emp.EmployeeTypeId
                                             select new
                                             {
                                                 LoanCalculationName = l.LoanCalculationName,
                                                 ComponentName = c.ComponentName,
                                                 TransactionType = c.TransactionType,
                                                 PRComponentID = c.PRComponentID,
                                                 PurposeName = p.PurposeName,
                                                 LoanType = p.LoanType,
                                                 ComponentCategory = c.ComponentCategory
                                             });
                        if (loancomponent.Any())
                        {

                            decimal previousPrincipal = 0, presentPrincipal = 0, preInterestAmt = 0, presentInterestAmt = 0;
                            decimal preCharge = 0, pressentCharge = 0;

                            var com = loancomponent.First();

                            #region Collection Method wise Calculation
                            if (com.LoanCalculationName == "Amortization")
                            {
                                //var monthlyInterest = Math.Round((((d.DisburseAmount * d.IntersetRate) / 100) / d.NoOfInstallment), 2);
                                //var monthlyInstallment = monthlyPrincipal + monthlyInterest;
                            }
                            else if (com.LoanCalculationName == "Classic") { }
                            else if (com.LoanCalculationName == "Decline")
                            {
                                DateTime lastCollectionDate = salaryDate;

                                if (collectionLastDateLstFromLoanRegister.Any())
                                {
                                    if (collectionLastDateLstFromLoanRegister.Where(x => x.LoanId == d.LoanId).Any())
                                    {
                                        lastCollectionDate = collectionLastDateLstFromLoanRegister.First(x => x.LoanId == d.LoanId).TransactionDate;
                                        previousPrincipal = collectionLastDateLstFromLoanRegister.First(x => x.LoanId == d.LoanId).PaidAmount;
                                        preInterestAmt = collectionLastDateLstFromLoanRegister.First(x => x.LoanId == d.LoanId).PaidInterestAmount;
                                        preCharge = collectionLastDateLstFromLoanRegister.First(x => x.LoanId == d.LoanId).InterestCharge ?? 0;
                                    }
                                    else lastCollectionDate = d.DisburseDate;
                                }
                                else lastCollectionDate = d.DisburseDate;

                                var previousInstallment = previousPrincipal + preInterestAmt;

                                var totalDays = (int)(salaryDate - lastCollectionDate).TotalDays;
                                var monthlyPrincipal = Math.Round(Convert.ToDecimal(d.DisburseAmount / d.NoOfInstallment));
                                //var monthlyInterest = Math.Round((((d.DisburseAmount * d.IntersetRate) / 100) / d.NoOfInstallment),2);
                                //var monthlyInstallment = monthlyPrincipal + monthlyInterest;

                                presentPrincipal = (int)(((d.DisburseAmount - previousPrincipal) >= monthlyPrincipal) ? monthlyPrincipal : (d.DisburseAmount - previousPrincipal));

                                pressentCharge = Math.Round((presentPrincipal == 0 ? 0 : ((d.DisburseAmount - previousPrincipal) * d.IntersetRate * totalDays) / 36500), 2);

                                presentInterestAmt = presentPrincipal == monthlyPrincipal ? 0
                                    : ((preCharge - preInterestAmt) >= monthlyPrincipal ? monthlyPrincipal : (preCharge - preInterestAmt));
                            }
                            else if (com.LoanCalculationName == "Flat")
                            {
                                //var monthlyInterest = Math.Round((((d.DisburseAmount * d.IntersetRate) / 100) / d.NoOfInstallment),2);
                                //var monthlyInstallment = monthlyPrincipal + monthlyInterest;
                            }
                            #endregion Collection Method wise Calculation

                            #region Tempory Table
                            if (pf_modelLst.Where(x => x.EmployeeId == d.EmployeeId).Any())
                            {
                                foreach (var pf in pf_modelLst.Where(x => x.EmployeeId == d.EmployeeId))
                                {
                                    if (d.LoanType == "PF")
                                    {
                                        pf.PFLoanID = d.LoanId;
                                        pf.PFLoanPrincipalColl = presentPrincipal;
                                        pf.PFLoanInterestCharge = pressentCharge;
                                        pf.PFLoanInterestColl = preInterestAmt;
                                    }
                                    if (d.LoanType == "Cl")
                                    {
                                        pf.CLLoanID = d.LoanId;
                                        pf.CLLoanPrincipalColl = presentPrincipal;
                                        pf.CLLoanInterestCharge = pressentCharge;
                                        pf.CLLoanInterestColl = preInterestAmt;
                                    }
                                }
                            }
                            else
                            {
                                TempPFCollection nonPF = new TempPFCollection()
                                {
                                    CLLoanCollection = presentInterestAmt + presentPrincipal,
                                    CLLoanID = d.LoanId,
                                    CLLoanPrincipalColl = presentPrincipal,
                                    CLLoanInterestColl = presentInterestAmt,
                                    CLLoanInterestCharge = pressentCharge,
                                    EmployeeId = d.EmployeeId,
                                    PFDistributionDate = salaryDate,
                                    PFDistributionMonth = salarymonth,
                                    PFDistributionYear = salaryYear
                                };
                                pf_modelLst.Add(nonPF);
                            }
                            #endregion Tempory Table

                            #region Salary Impact
                            EmployeeMonthlySalary loan = new EmployeeMonthlySalary()
                            {
                                SalaryMonth = salarymonth,
                                SalaryYear = salaryYear,
                                SalaryDate = salaryDate,
                                EmployeeId = d.EmployeeId,
                                PRComponentId = com.PRComponentID,
                                PRComponentAmount = presentPrincipal + presentInterestAmt,
                                IsActive = true,
                                IsApproved = false,
                                ComponentCategory = com.ComponentCategory,
                                TransactionType = com.TransactionType,
                                CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID),
                                UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID),
                                CreateDate = DateTime.Today,
                                UpdateDate = DateTime.Today,
                                OfficeId = emp.OfficeId
                            };
                            objEmpMonthlySalary.Add(loan);
                            #endregion Salary Impact
                        }
                    }
                }
                
            }
            return Tuple.Create(objEmpMonthlySalary, pf_modelLst);
        }
        #endregion PF & Loan
        private List<TempComponent> PopulateTempNegativeImpacts(List<PRComponent> components, List<EmployeeSalaryDeduction> negatives)
        {
            List<TempComponent> tmpComponentNeg = new List<TempComponent>();
            foreach (var pos in negatives.ToList())
            {
                var entity = new TempComponent();
                entity.PRComponentID = Convert.ToInt32(pos.ComponentId);
                if (components.Where(p => p.PRComponentID == entity.PRComponentID).FirstOrDefault() != null)
                {
                    entity.ComponentName = components.Where(p => p.PRComponentID == entity.PRComponentID).FirstOrDefault().ComponentName;
                }
                tmpComponentNeg.Add(entity);
            }

            return tmpComponentNeg;
        }

        private List<TempComponent> PopulateTempPositiveImpacts(List<PRComponent> components, List<EmployeeSalaryIncentive> positives)
        {
            List<TempComponent> tmpComponentPos = new List<TempComponent>();
            foreach (var pos in positives.ToList())
            {
                var entity = new TempComponent();
                entity.PRComponentID = Convert.ToInt32(pos.PRComponentId);

                if (components.Any(p => p.PRComponentID == entity.PRComponentID))
                    entity.ComponentName = components.FirstOrDefault(p => p.PRComponentID == entity.PRComponentID).ComponentName;

                tmpComponentPos.Add(entity);
            }

            return tmpComponentPos;
        }

        // Method 17-1
        private IEnumerable<int> PositiveImpactsComponent(List<PRComponent> components)
        {
            IEnumerable<int> positiveImpacts = null;
            var checkPositiveImpacts = components.Where(p => p.SalaryChangesByComponent == "Positive" && p.SalaryEffect == true).ToList();
            if (checkPositiveImpacts != null)
            {
                positiveImpacts = checkPositiveImpacts.Select(p => p.PRComponentID);
            }
            return positiveImpacts;
        }


        // Method 17-2
        private IEnumerable<int> NegativeImpactsComponent(List<PRComponent> components)
        {
            IEnumerable<int> negativeImpacts = null;
            var checkNegativeImpacts = components.Where(p => p.SalaryChangesByComponent == "Negative" && p.SalaryEffect == true).ToList();
            if (checkNegativeImpacts != null)
            {
                negativeImpacts = checkNegativeImpacts.Select(p => p.PRComponentID);
            }
            return negativeImpacts;
        }

        // Method 17-3
        private string ConcatItems(List<TempComponent> componentItems)
        {
            return string.Join(", ", from item in componentItems select item.ComponentName);
        }

        // Method 17-4
        public List<PRSalaryScaleViewModel> ReGenerateTemporarySalaryForTheMonth(int empSalaryTypeId, int EmployeeStatusId,
            double grossOrBasicChangesAmount, double currentGrossOrBasicSalary, int employeeOfficeLocationid, int PFTypeId, int officeId, long employeeId, string month, string salaryYear)
        {
            var payrollSalaryScale = new List<PRSalaryScaleViewModel>();
            double basicSalary = 0;
            double currentBasicSalary = 0;
            try
            {
                var param2 = new
                {
                    EmployeeTypeId = Convert.ToInt32(empSalaryTypeId),
                    EmployeeStatusId = EmployeeStatusId,
                    OfficeLocationId = employeeOfficeLocationid,
                    PFTypeId = Convert.ToInt32(PFTypeId)
                };

                // get type wise component configuration [prl.PRComponent] AND (ComponentCategory='Salary')
                var empTypeWiseCompConfig = employeeSPService.GetDataWithParameter(param2, "prl.SP_Get_EmployeeTypeWiseComponentConfiguration");
                for (int i = 0; i <= empTypeWiseCompConfig.Tables[0].Rows.Count - 1; i++)
                {
                    //calculate change basic salary and current basic salary by component ratio amount
                    if (empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim() != "Basic Salary") //ComponentName
                        continue;

                    var ratioBaseOn = empTypeWiseCompConfig.Tables[0].Rows[i][6].ToString().Trim();
                    var payrollConfigurationType = SessionHelper.PayrollConfigurationType;
                    var ratio = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentAmount"].ToString());

                    if (payrollConfigurationType == PayrollConfigurationTypeConstants.Basic)
                    {
                        if (ratioBaseOn != SalaryRatioConstants.Basic)
                            continue;

                        basicSalary = CalculateBasicRatioOrFixedforComponent(ratio, grossOrBasicChangesAmount);
                        currentBasicSalary = CalculateBasicRatioOrFixedforComponent(ratio, currentGrossOrBasicSalary);
                        break;
                    }
                    else
                    {
                        if (ratioBaseOn != SalaryRatioConstants.Gross)
                            continue;

                        basicSalary = CalculateRatioforComponent(ratio, grossOrBasicChangesAmount);
                        currentBasicSalary = CalculateRatioforComponent(ratio, currentGrossOrBasicSalary);
                        break;
                    }

                }

                //if change basic salary found then re-distribute employee salary
                if (basicSalary > 0)
                {
                    payrollSalaryScale = ReDistributeEmployeeSalaryInComponentFortheMonth(empSalaryTypeId, basicSalary,
                        grossOrBasicChangesAmount, EmployeeStatusId, currentGrossOrBasicSalary, currentBasicSalary,
                        employeeOfficeLocationid, PFTypeId, officeId, employeeId, month, salaryYear);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return payrollSalaryScale;
        }

        // Method 17-5
        private List<PRSalaryConfigurationViewModel> ReGenerateEmployeeSalaryForPositiveOrNegativeImpact(
            List<PRSalaryScaleViewModel> tempSalaryList, DateTime firstDate, DateTime lastDate,
            PRSalaryConfigurationViewModel empsalary)
        {
            var tempSalaryConfiguration = new List<PRSalaryConfigurationViewModel>();
            foreach (var salary in tempSalaryList)
            {
                var entity = new PRSalaryConfigurationViewModel();
                entity.EmployeeID = empsalary.EmployeeID;
                entity.PRComponentID = salary.PRComponentId;
                entity.ComponentAmount = Convert.ToDecimal(salary.CalculatedAmount);
                entity.EffectiveStartDate = firstDate;
                entity.EffectiveEndDate = lastDate;
                entity.IsActive = true;
                entity.InActiveDate = null;
                entity.ComponentCategory = salary.ComponentCategory;
                entity.TransactionType = salary.TransactionType;
                entity.CreateUser = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                entity.UpdateUser = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                entity.CreateDate = DateTime.UtcNow;
                entity.UpdateDate = DateTime.UtcNow;
                entity.OfficeID = salary.OfficeId;

                tempSalaryConfiguration.Add(entity);
            }
            return tempSalaryConfiguration;
        }


        // Method 17-6
        private double ChangesInGrossAmountFortheMonth(int dateDifference, int daysInMonth, double grossOrBasicSalary)
        {
            return ((grossOrBasicSalary / daysInMonth) * dateDifference);
        }


        // Method 17-7
        private List<EmployeeMonthlySalary> GeneratePartialSalaryForNewJoinedEmployee(int dateDifference, int daysInMonth, List<PRSalaryConfigurationViewModel> salaryconfigurations, string month, string salaryYear, string salaryDate, List<PRComponent> components)
        {
            var lstMonthlySalary = new List<EmployeeMonthlySalary>();
            foreach (var item in salaryconfigurations)
            {
                decimal componentAmount = 0;
                var entity = new EmployeeMonthlySalary();
                entity.SalaryMonth = Convert.ToInt32(month);
                entity.SalaryYear = Convert.ToInt32(salaryYear);
                entity.SalaryDate = Convert.ToDateTime(salaryDate);
                entity.EmployeeId = item.EmployeeID;
                entity.PRSalaryConfigurationId = item.PRSalaryConfigurationID;
                entity.PRComponentId = item.PRComponentID;

                var componentRevenueStamp = components.FirstOrDefault(p => p.PRComponentID == item.PRComponentID);

                if (componentRevenueStamp != null && componentRevenueStamp.ComponentName.Trim() == "Revenue Stamp")
                    componentAmount = item.ComponentAmount;
                else
                    componentAmount = ((item.ComponentAmount / daysInMonth) * dateDifference);

                entity.PRComponentAmount = (decimal)GetRatioDependingOnSalaryRoundType(item.SalaryRoundType, (double)componentAmount);

                entity.ComponentCategory = item.ComponentCategory;
                entity.TransactionType = item.TransactionType;
                entity.IsActive = true;
                entity.IsSendForApproval = false;
                entity.IsApproved = false;
                entity.CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                entity.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                entity.CreateDate = DateTime.Today;
                entity.UpdateDate = DateTime.Today;
                entity.OfficeId = item.OfficeID;
                lstMonthlySalary.Add(entity);
            }

            return lstMonthlySalary;
        }

        // Method 17-8
        private string GenerateExceptionalEmployeeSalaryCondition(int daysInMonth, double incentiveAmount, double deductionAmount, double grossForTheMonth, string salaryType, string incentiveItems, string deductionItems)
        {
            var basicOrGross = SessionHelper.PayrollConfigurationType == PayrollConfigurationTypeConstants.Basic ? "Basic" : "Gross";
            var details = salaryType + " Salary, Days: " + daysInMonth + ", " + incentiveItems + " : " + incentiveAmount + " , " + deductionItems + " : " + deductionAmount + $" {basicOrGross} for the Month: " + Math.Round(grossForTheMonth, 2);
            return details;
        }

        // Method 17-9
        private EmployeeMonthlySalaryException GenerateExceptionList(long employeeId, string employeeCode, DateTime fristDate, DateTime lastDate, string exceptionRemarks)
        {
            var exceptionEmployee = new EmployeeMonthlySalaryException();
            exceptionEmployee.EmployeeId = employeeId;
            exceptionEmployee.EmployeeCode = employeeCode;
            exceptionEmployee.EffectiveDateFrom = fristDate;
            exceptionEmployee.EffectiveDateTo = lastDate;
            exceptionEmployee.Remarks = exceptionRemarks;
            exceptionEmployee.IsActive = true;
            exceptionEmployee.IsRejected = false;
            exceptionEmployee.IsApproved = false;
            exceptionEmployee.CreateDate = DateTime.UtcNow;
            exceptionEmployee.UpdateDate = DateTime.UtcNow;
            return exceptionEmployee;
        }

        // Method 17-10
        private List<EmployeeMonthlySalary> PopulateRegularSalaryForEmployee(
            List<PRSalaryConfigurationViewModel> empRegularSalaryConfigurations, string month, string salaryYear, string salaryDate
            , int pfTypeId, int employeeTypeId, int employeeStatusId, int employeeOfficeLocationid, long employeeId
            )
        {
            var lstMonthlySalary = new List<EmployeeMonthlySalary>();

            DateTime? partialPFInMonthlySalary = null;
            //get pf components
            List<string> pfComponents = GetPFComponents();

            //Get partial pf in monthly salary
            if (pfTypeId > 0)
                partialPFInMonthlySalary = GetPartialPFDateInMonthlySalary(employeeTypeId, employeeStatusId, employeeOfficeLocationid, pfTypeId, employeeId, month, salaryYear, partialPFInMonthlySalary, pfComponents);

            foreach (var item in empRegularSalaryConfigurations)
            {
                decimal prComponentAmount = item.ComponentAmount;

                //for partial pf
                if (partialPFInMonthlySalary != null && pfComponents.Any(a => a == item.ComponentName))
                {
                    double pfFinalAmount = GetPartialPFAmount(month, salaryYear, partialPFInMonthlySalary, (double)item.ComponentAmount);
                    prComponentAmount = (decimal)pfFinalAmount;
                }

                prComponentAmount = (decimal)GetRatioDependingOnSalaryRoundType(item.SalaryRoundType, (double)prComponentAmount);

                var entity = new EmployeeMonthlySalary();
                entity.SalaryMonth = Convert.ToInt32(month);
                entity.SalaryYear = Convert.ToInt32(salaryYear);
                entity.SalaryDate = Convert.ToDateTime(salaryDate);
                entity.EmployeeId = item.EmployeeID;
                entity.PRSalaryConfigurationId = item.PRSalaryConfigurationID;
                entity.PRComponentId = item.PRComponentID;
                entity.PRComponentAmount = prComponentAmount;
                entity.ComponentCategory = item.ComponentCategory;
                entity.TransactionType = item.TransactionType;
                entity.IsActive = true;
                entity.IsSendForApproval = false;
                entity.IsApproved = false;
                entity.CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                entity.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                entity.CreateDate = DateTime.Today;
                entity.UpdateDate = DateTime.Today;
                entity.OfficeId = item.OfficeID;
                lstMonthlySalary.Add(entity);
            }

            return lstMonthlySalary;
        }

        private List<EmployeeMonthlySalary> GenerateRegularSalaryForEmployee(List<PRSalaryConfigurationViewModel> salaryconfigurations, string month, string salaryYear, string salaryDate)
        {
            var lstMonthlySalary = new List<EmployeeMonthlySalary>();
            foreach (var item in salaryconfigurations)
            {
                item.ComponentAmount = (decimal)GetRatioDependingOnSalaryRoundType(item.SalaryRoundType, (double)(item.ComponentAmount));

                var entity = new EmployeeMonthlySalary();
                entity.SalaryMonth = Convert.ToInt32(month);
                entity.SalaryYear = Convert.ToInt32(salaryYear);
                entity.SalaryDate = Convert.ToDateTime(salaryDate);
                entity.EmployeeId = item.EmployeeID;
                entity.PRSalaryConfigurationId = item.PRSalaryConfigurationID;
                entity.PRComponentId = item.PRComponentID;
                entity.PRComponentAmount = item.ComponentAmount;
                entity.ComponentCategory = item.ComponentCategory;
                entity.TransactionType = item.TransactionType;
                entity.IsActive = true;
                entity.IsSendForApproval = false;
                entity.IsApproved = false;
                entity.CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                entity.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                entity.CreateDate = DateTime.Today;
                entity.UpdateDate = DateTime.Today;
                entity.OfficeId = item.OfficeID;
                lstMonthlySalary.Add(entity);
            }

            return lstMonthlySalary;
        }


        // Method 17-11
        private string GenerateExceptionalEmployeeSalaryConditionIncrement(int dateDifference, double arrearAmount, double leaveWithoutPaymentAmount, double grossForTheMonth, string salaryType)
        {
            return salaryType + " Salary, Days: " + dateDifference + " Arrear: " + arrearAmount + " LWP: " + leaveWithoutPaymentAmount;
        }

        // Method 17-12
        private List<EmployeeMonthlySalary> GeneratePartialSalaryEmployeeIncrement(
            int dateDifferencewithEffectiveDate,
            int daysInMonth,
            List<PRSalaryConfigurationViewModel> salaryconfigurations,
            string month, string salaryYear, string salaryDate,
            List<PRComponent> components,
            List<PRSalaryConfiguration> lastConfiguredSalary,
            long employeeId)
        {
            var tmpSalaryCalculation = new List<TempComponentForIncrement>();
            var lstMonthlySalary = new List<EmployeeMonthlySalary>();
            var restOftheDayInMonth = daysInMonth - dateDifferencewithEffectiveDate;
            var previousConfiguredSalary = lastConfiguredSalary.Where(p => p.EmployeeID == employeeId).ToList();

            foreach (var item in salaryconfigurations)
            {
                decimal componentAmount = 0;
                var component = components.Where(p => p.PRComponentID == item.PRComponentID).FirstOrDefault();
                if (component != null)
                {
                    var componentType = component.ComponentCategory.Trim();
                    if (componentType == "Salary")
                    {
                        var tmpEntity = new TempComponentForIncrement();
                        var componentName = component.ComponentName.Trim();
                        if (componentName == "Revenue Stamp")
                        {
                            componentAmount = item.ComponentAmount;
                        }
                        else
                        {
                            componentAmount = ((item.ComponentAmount / daysInMonth) * restOftheDayInMonth);
                        }

                        componentAmount = (decimal)GetRatioDependingOnSalaryRoundType(item.SalaryRoundType, (double)componentAmount);

                        tmpEntity.ComponentId = item.PRComponentID;
                        tmpEntity.ComponentAmount = componentAmount;
                        tmpEntity.EmployeeId = item.EmployeeID;
                        tmpEntity.ComponentName = componentName;
                        tmpEntity.ComponentCategory = item.ComponentCategory;
                        tmpEntity.PRSalaryConfigurationId = item.PRSalaryConfigurationID;
                        tmpEntity.TransactionType = item.TransactionType;
                        tmpEntity.OfficeId = item.OfficeID;
                        tmpSalaryCalculation.Add(tmpEntity);
                    }
                }
            }

            foreach (var item in tmpSalaryCalculation)
            {
                var entity = new EmployeeMonthlySalary();
                decimal previousComponentAmount = 0;
                decimal componentCalculation = 0;
                decimal calculateAmount = 0;

                var componentAmount = previousConfiguredSalary.Where(p => p.PRComponentID == item.ComponentId).FirstOrDefault();
                if (componentAmount != null)
                {
                    calculateAmount = componentAmount.ComponentAmount;
                }
                if (item.ComponentName == "Revenue Stamp")
                {
                    item.ComponentAmount = item.ComponentAmount;
                }
                else
                {
                    componentCalculation = ((calculateAmount / daysInMonth) * dateDifferencewithEffectiveDate);
                    previousComponentAmount = item.ComponentAmount;
                    item.ComponentAmount = previousComponentAmount + componentCalculation;
                }
                entity.SalaryMonth = Convert.ToInt32(month);
                entity.SalaryYear = Convert.ToInt32(salaryYear);
                entity.SalaryDate = Convert.ToDateTime(salaryDate);
                entity.EmployeeId = item.EmployeeId;
                entity.PRSalaryConfigurationId = item.PRSalaryConfigurationId;
                entity.PRComponentId = item.ComponentId;
                entity.PRComponentAmount = item.ComponentAmount;
                entity.ComponentCategory = item.ComponentCategory;
                entity.TransactionType = item.TransactionType;
                entity.IsActive = true;
                entity.IsSendForApproval = false;
                entity.IsApproved = false;
                entity.OfficeId = item.OfficeId;
                entity.CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                entity.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                entity.CreateDate = DateTime.Today;
                entity.UpdateDate = DateTime.Today;
                lstMonthlySalary.Add(entity);
            }

            return lstMonthlySalary;
        }
        private List<EmployeeMonthlySalary> GeneratePartialSalaryEmployeeIncrement_New(
    int dateDifferencewithEffectiveDate,
    int daysInMonth,
    List<PRSalaryConfigurationViewModel> salaryconfigurations,
    string month, string salaryYear, string salaryDate,
    List<PRComponent> components,
    List<EmployeeMonthlySalaryApproved> lastSalary,
    long employeeId)
        {
            var tmpSalaryCalculation = new List<TempComponentForIncrement>();
            var lstMonthlySalary = new List<EmployeeMonthlySalary>();
            var restOftheDayInMonth = daysInMonth - dateDifferencewithEffectiveDate;
            var previousConfiguredSalary = lastSalary.Where(p => p.EmployeeId == employeeId).ToList();
            var lastSalarycomponent = new List<PRComponent>();
            if (lastSalary.Any())
            {
                var cids = lastSalary.Select(s => s.PRComponentId);
                lastSalarycomponent = components.Where(p => cids.Contains(p.PRComponentID)).ToList();
            }


            foreach (var item in salaryconfigurations)
            {
                decimal componentAmount = 0;
                var component = components.Where(p => p.PRComponentID == item.PRComponentID).FirstOrDefault();
                if (component != null)
                {
                    var componentType = component.ComponentCategory.Trim();
                    if (componentType == "Salary")
                    {
                        var tmpEntity = new TempComponentForIncrement();
                        var componentName = component.ComponentName.Trim();
                        if (componentName == "Revenue Stamp")
                            componentAmount = item.ComponentAmount;
                        else
                            componentAmount = ((item.ComponentAmount / daysInMonth) * restOftheDayInMonth);

                        componentAmount = (decimal)GetRatioDependingOnSalaryRoundType(item.SalaryRoundType, (double)componentAmount);

                        tmpEntity.ComponentId = item.PRComponentID;
                        tmpEntity.ComponentAmount = componentAmount;
                        tmpEntity.EmployeeId = item.EmployeeID;
                        tmpEntity.ComponentName = componentName;
                        tmpEntity.ComponentCategory = item.ComponentCategory;
                        tmpEntity.PRSalaryConfigurationId = item.PRSalaryConfigurationID;
                        tmpEntity.TransactionType = item.TransactionType;
                        tmpEntity.OfficeId = item.OfficeID;
                        tmpSalaryCalculation.Add(tmpEntity);
                    }
                }
            }

            foreach (var item in tmpSalaryCalculation)
            {
                var entity = new EmployeeMonthlySalary();
                decimal previousComponentAmount = 0;
                decimal componentCalculation = 0;
                decimal calculateAmount = 0;
                int PRComponentID = 0;
                if (lastSalarycomponent.Any())
                {
                    var comname = lastSalarycomponent.FirstOrDefault(x => x.ComponentName.Trim().ToLower() == item.ComponentName.Trim().ToLower());
                    if (comname != null)
                        PRComponentID = comname.PRComponentID;
                }
                var componentAmount = previousConfiguredSalary.Where(p => p.PRComponentId == PRComponentID/*item.ComponentId*/).FirstOrDefault();
                if (componentAmount != null)
                    calculateAmount = componentAmount.PRComponentAmount;
                if (item.ComponentName == "Revenue Stamp")
                    item.ComponentAmount = item.ComponentAmount;

                else
                {
                    componentCalculation = ((calculateAmount / daysInMonth) * dateDifferencewithEffectiveDate);
                    previousComponentAmount = item.ComponentAmount;
                    item.ComponentAmount = previousComponentAmount + componentCalculation;
                }
                entity.SalaryMonth = Convert.ToInt32(month);
                entity.SalaryYear = Convert.ToInt32(salaryYear);
                entity.SalaryDate = Convert.ToDateTime(salaryDate);
                entity.EmployeeId = item.EmployeeId;
                entity.PRSalaryConfigurationId = item.PRSalaryConfigurationId;
                entity.PRComponentId = item.ComponentId;
                entity.PRComponentAmount = item.ComponentAmount;
                entity.ComponentCategory = item.ComponentCategory;
                entity.TransactionType = item.TransactionType;
                entity.IsActive = true;
                entity.IsSendForApproval = false;
                entity.IsApproved = false;
                entity.OfficeId = item.OfficeId;
                entity.CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                entity.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                entity.CreateDate = DateTime.Today;
                entity.UpdateDate = DateTime.Today;
                lstMonthlySalary.Add(entity);
            }

            return lstMonthlySalary;
        }

        // Method 17-4-1
        private List<PRSalaryScaleViewModel> ReDistributeEmployeeSalaryInComponentFortheMonth(int empSalaryTypeId,
            double basicSalary, double grossOrBasicChangesAmount, int EmployeeStatusId, double currentGrossOrBasicSalary,
            double currentBasicSalary, int officeLocationId, int PFTypeId, int officeId, long employeeId, string month, string salaryYear)
        {

            var param2 = new
            {
                EmployeeTypeId = empSalaryTypeId,
                EmployeeStatusId = EmployeeStatusId,
                OfficeLocationId = officeLocationId,
                PFTypeId = Convert.ToInt32(PFTypeId)
            };
            var empTypeWiseCompConfig = employeeSPService.GetDataWithParameter(param2, "prl.SP_Get_EmployeeTypeWiseComponentConfiguration");
            empTypeWiseCompConfig.Tables[0].Columns.Add(new DataColumn("CalculatedAmount", typeof(System.Double)));

            List<PRSalaryScaleViewModel> dataList = new List<PRSalaryScaleViewModel>();
            DateTime? partialPFDateInMonthlySalary = null;

            //get pf components
            List<string> components = GetPFComponents();

            //Get partial pf date in monthly salary
            if (PFTypeId > 0)
                partialPFDateInMonthlySalary = GetPartialPFDateInMonthlySalary(empSalaryTypeId, EmployeeStatusId, officeLocationId, PFTypeId, employeeId, month, salaryYear, partialPFDateInMonthlySalary, components);

            for (int i = 0; i <= empTypeWiseCompConfig.Tables[0].Rows.Count - 1; i++)
            {
                var componentName = empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentName"].ToString();
                var componentType = empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentType"].ToString();

                var payrollConfigurationType = SessionHelper.PayrollConfigurationType;

                if (payrollConfigurationType == PayrollConfigurationTypeConstants.Basic
                    && componentName == "Basic Salary")
                {
                    if (componentType != SalaryCalculationTypeConstants.Fixed)
                        continue;
                }

                var ratioPercent = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentAmount"].ToString());
                var ratioBasedOn = empTypeWiseCompConfig.Tables[0].Rows[i]["RatioBasedOn"].ToString();
                var isSalaryImpactProhibited = Convert.ToBoolean(empTypeWiseCompConfig.Tables[0].Rows[i]["IsSalaryImpactProhibited"]);
                var componentSalaryRoundType = empTypeWiseCompConfig.Tables[0].Rows[i]["SalaryRoundType"].ToString();

                double ratio = 0;

                if (componentType == SalaryCalculationTypeConstants.Ratio && ratioBasedOn == SalaryRatioConstants.Gross)
                {
                    if (isSalaryImpactProhibited != true)
                        ratio = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), grossOrBasicChangesAmount);

                    if (isSalaryImpactProhibited == true)
                        ratio = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), currentGrossOrBasicSalary);

                    //get ratio depending on salary round type
                    ratio = GetRatioDependingOnSalaryRoundType(componentSalaryRoundType, ratio);

                    var maxLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MaximumLimit"].ToString());
                    var minLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MinimumLimit"].ToString());

                    if (minLimit != 0 && ratio < minLimit)
                        ratio = minLimit;
                    if (maxLimit != 0 && ratio > maxLimit)
                        ratio = maxLimit;

                    if (partialPFDateInMonthlySalary != null && components.Any(a => a == componentName))
                    {
                        //for partial pf
                        double pfFinalAmount = GetPartialPFAmount(month, salaryYear, partialPFDateInMonthlySalary, ratio);
                        empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = pfFinalAmount;
                    }
                    else
                    {
                        empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratio;
                    }
                }
                else if (componentType == SalaryCalculationTypeConstants.Ratio && ratioBasedOn == SalaryRatioConstants.Basic)
                {
                    if (isSalaryImpactProhibited != true)
                        ratio = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), basicSalary);

                    if (isSalaryImpactProhibited == true)
                        ratio = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), currentBasicSalary);

                    //get ratio depending on salary round type
                    ratio = GetRatioDependingOnSalaryRoundType(componentSalaryRoundType, ratio);

                    var maxLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MaximumLimit"].ToString());
                    var minLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MinimumLimit"].ToString());

                    if (ratio < minLimit && minLimit != 0)
                        ratio = minLimit;

                    if (ratio > maxLimit && maxLimit != 0)
                        ratio = maxLimit;

                    if (partialPFDateInMonthlySalary != null && components.Any(a => a == componentName))
                    {
                        //for partial pf
                        double pfFinalAmount = GetPartialPFAmount(month, salaryYear, partialPFDateInMonthlySalary, ratio);
                        empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = pfFinalAmount;
                    }
                    else
                    {
                        empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratio;
                    }
                }
                else if (componentType == SalaryCalculationTypeConstants.Fixed
                        && ratioBasedOn == RatioBasedOnConstants.NotRequired)
                {
                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = basicSalary;
                }
                else if (componentType == SalaryCalculationTypeConstants.Fixed)
                {
                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratioPercent;//for fixed ratioPercentage is the fixed amount
                }

                dataList.Add(new PRSalaryScaleViewModel
                {
                    PRComponentId = empTypeWiseCompConfig.Tables[0].Rows[i].Field<int>("PRComponentId"),
                    EmployeeTypeName = empTypeWiseCompConfig.Tables[0].Rows[i].Field<string>("EmployeeTypeName"),
                    ComponentGroupName = empTypeWiseCompConfig.Tables[0].Rows[i].Field<string>("ComponentGroupName"),
                    ComponentName = empTypeWiseCompConfig.Tables[0].Rows[i].Field<string>("ComponentName"),
                    ComponentType = empTypeWiseCompConfig.Tables[0].Rows[i].Field<string>("ComponentType"),
                    ComponentAmount = empTypeWiseCompConfig.Tables[0].Rows[i].Field<decimal>("ComponentAmount"),
                    RatioBasedOn = empTypeWiseCompConfig.Tables[0].Rows[i].Field<string>("RatioBasedOn"),
                    EmployeeTypeId = empTypeWiseCompConfig.Tables[0].Rows[i].Field<int>("EmployeeTypeId"),
                    CalculatedAmount = empTypeWiseCompConfig.Tables[0].Rows[i].Field<double>("CalculatedAmount"),
                    ComponentCategory = empTypeWiseCompConfig.Tables[0].Rows[i].Field<string>("ComponentCategory"),
                    TransactionType = empTypeWiseCompConfig.Tables[0].Rows[i].Field<string>("TransactionType"),
                    EmployeeStatusId = empTypeWiseCompConfig.Tables[0].Rows[i].Field<int?>("EmployeeStatusId"),
                    TransactionTypeView = empTypeWiseCompConfig.Tables[0].Rows[i].Field<string>("TransactionTypeView"),
                    OfficeId = officeId
                });
            }

            return dataList;
        }

        private DateTime? GetPartialPFDateInMonthlySalary(int empSalaryTypeId, int EmployeeStatusId, int officeLocationId, int PFTypeId, long employeeId, string month, string salaryYear, DateTime? partialPFDateInMonthlySalary, List<string> components)
        {
            try
            {
                //check pf for previous monlthly salary
                var pfMonthlySalaryFilter = new EmployeeMonthlySalarySearchFilter
                {
                    EmployeeId = (int)employeeId,
                    EmployeeTypeId = empSalaryTypeId,
                    EmployeeStatusId = EmployeeStatusId,
                    OfficeLocationId = officeLocationId,
                    PFTypeId = Convert.ToInt32(PFTypeId),
                    Components = components
                };

                //check pf in previous monthly salary [prl.EmployeeMonthlySalay]
                var checkPFInMonthlySalary = employeeMonthlySalaryService.CheckMonthlySalaryByEmployeeAndComponents(pfMonthlySalaryFilter);

                if (checkPFInMonthlySalary)
                    return partialPFDateInMonthlySalary;

                bool withResignEmployee = false;
                var employee = employeeService.GetEmployeeById(employeeId, withResignEmployee);
                if (employee == null)
                    return partialPFDateInMonthlySalary;

                DateTime employeeJoiningDate = employee.FirstJoiningDate;

                //check joining date for 1st day of month or not 
                if ((employeeJoiningDate.Year.ToString() == salaryYear && employeeJoiningDate.Month.ToString() == month) &&
                    (employeeJoiningDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) != new DateTime(employeeJoiningDate.Year, employeeJoiningDate.Month, 1).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)))
                {
                    var daysInSalaryMonth = DateTime.DaysInMonth(Convert.ToInt32(salaryYear), Convert.ToInt32(month));
                    if (daysInSalaryMonth >= employeeJoiningDate.Day)
                        partialPFDateInMonthlySalary = new DateTime(Convert.ToInt32(salaryYear), Convert.ToInt32(month), employeeJoiningDate.Day);
                }

                return partialPFDateInMonthlySalary;
            }
            catch (Exception ex)
            {
                return partialPFDateInMonthlySalary;
            }
        }

        private double GetPartialPFAmount(string month, string salaryYear, DateTime? partialPFInMonthlySalary, double ratio)
        {
            var lastDateOfSalaryMonth = new DateTime(Convert.ToInt32(salaryYear), Convert.ToInt32(month), 1).AddMonths(1).AddDays(-1);
            double totalDayOfSalaryMonth = DateTime.DaysInMonth(Convert.ToInt32(salaryYear), Convert.ToInt32(month));
            double totalPFDays = (lastDateOfSalaryMonth - (DateTime)partialPFInMonthlySalary).TotalDays + 1;
            var pfFinalAmount = (ratio * totalPFDays) / totalDayOfSalaryMonth;
            return pfFinalAmount;
        }

        private List<string> GetPFComponents()
        {
            var components = new List<string>();
            components.Add(ComponentPayrollConstants.Salary_PFOfficeContribution);
            components.Add(ComponentPayrollConstants.Salary_PFEmployeeDeduction);
            components.Add(ComponentPayrollConstants.Salary_PFOfficeDeduction);
            return components;
        }

        private double GetRatioDependingOnSalaryRoundType(string componentSalaryRoundType,
            double ratio)
        {

            if (componentSalaryRoundType != SalaryRoundTypeConstants.NotApplicable)
            {
                if (componentSalaryRoundType == SalaryRoundTypeConstants.RoundUp)
                    return ratio = Math.Ceiling(ratio);

                if (componentSalaryRoundType == SalaryRoundTypeConstants.RoundDown)
                    return ratio = Math.Floor(ratio);

                if (componentSalaryRoundType == SalaryRoundTypeConstants.RoundNormal)
                    return ratio = Math.Round(ratio, 0);

                if (componentSalaryRoundType == "Round")
                    return ratio = Math.Round(ratio, 0);
            }

            return ratio;
        }

        // Method 17-4-1-1 and   // Method 17-4

        private double CalculateRatioforComponent(double ratio, double amount)
        {
            return amount != 0 ? (ratio * amount) / 100 : 0;
        }

        private double CalculateBasicRatioOrFixedforComponent(double ratio, double amount)
        {
            var payrollConfigurationType = SessionHelper.PayrollConfigurationType;
            if (payrollConfigurationType == PayrollConfigurationTypeConstants.Basic)
                return amount;

            return amount != 0 ? (ratio * amount) / 100 : 0;
        }

        #endregion
    }
}