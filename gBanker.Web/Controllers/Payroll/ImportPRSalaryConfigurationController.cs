
#region Usings
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service;
using gHRM.Service.Basic;
using gHRM.Service.Payroll;
using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.Payroll;
using gHRM.Core.Utilities.Constants;
using System.Web;
using System.IO;
using System.Data.OleDb;
using gHRM.Core.Utilities;

#endregion

namespace gHRM.Web.Controllers.Payroll
{
    public class ImportPRSalaryConfigurationController : BaseController
    {
        #region Private Variables

        private readonly IEmployeeSPService employeeSPService;
        private readonly IPRSalaryConfigurationService prSalaryConfigurationService;
        private readonly IEmployeeService employeeService;
        private readonly IView_EmployeeSalaryConfigurationService viewSalaryConfigurationService;
        private readonly IEmployeeGradeListService employeeGradeListService;
        private readonly IEmployeeMonthlySalaryApprovedService employeeMonthlySalaryApprovedService;
        private readonly IEmployeeMonthlySalaryService employeeMonthlySalaryService;
        private readonly IEmployeeSalaryConfigurationHistoryService employeeSalaryConfigurationHistoryService;
        private readonly IBankNameService bankNameService;
        private readonly IPRComponentService prComponentService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IEmployeeDesignationService employeeDesignationService;
        private readonly IEmployeePromotionService employeePromotionService;

        private readonly IOfficeService officeService;
        private readonly ICompanyService companyService;
        private CommonStaticDropDown commonStaticDropDown;
        private CommonDynamicDropDown CommonDynamicDropDown;

        #endregion

        #region Ctor

        public ImportPRSalaryConfigurationController(
            IView_EmployeeSalaryConfigurationService viewSalaryConfigurationService,
            IEmployeeSPService employeeSPService, IPRSalaryConfigurationService prSalaryConfigurationService,
            IEmployeeService employeeService, IEmployeeGradeListService employeeGradeListService,
            IEmployeeMonthlySalaryApprovedService employeeMonthlySalaryApprovedService,
            IEmployeeSalaryConfigurationHistoryService employeeSalaryConfigurationHistoryService,
            IEmployeeMonthlySalaryService employeeMonthlySalaryService,
            IBankNameService bankNameService,
            IPRComponentService prComponentService,
            IEmployeeDepartmentService employeeDepartmentService,
            IEmployeeDesignationService employeeDesignationService,
            IEmployeePromotionService employeePromotionService,
            IOfficeService officeService,
            ICompanyService companyService)
        {
            this.employeeSPService = employeeSPService;
            this.prSalaryConfigurationService = prSalaryConfigurationService;
            this.employeeService = employeeService;
            this.viewSalaryConfigurationService = viewSalaryConfigurationService;
            this.employeeGradeListService = employeeGradeListService;
            this.employeeMonthlySalaryApprovedService = employeeMonthlySalaryApprovedService;
            this.employeeSalaryConfigurationHistoryService = employeeSalaryConfigurationHistoryService;
            this.employeeMonthlySalaryService = employeeMonthlySalaryService;
            this.bankNameService = bankNameService;
            this.prComponentService = prComponentService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.employeeDesignationService = employeeDesignationService;
            this.officeService = officeService;
            this.employeePromotionService = employeePromotionService;
            this.companyService = companyService;

            commonStaticDropDown = new CommonStaticDropDown();
            CommonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion

        #region Import Configuration
        public ActionResult Import()
        {
            return View();
        }
        public ActionResult Import2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportConfiguration()
        {
            try
            {
                string validationMessage;
                var importSalaryConfigurationErrorList = "";

                if (!ModelState.IsValid)
                    return Json(new { type = "warning", errorLisings = false, message = "Error on file, Please try again" },
                               JsonRequestBehavior.AllowGet);

                if (Request.Files.Count <= 0)
                    return Json(new { type = "warning", errorLisings = false, message = "File not found. Please try again." },
                             JsonRequestBehavior.AllowGet);

                var file = Request.Files[0];

                // Generate dataset
                var ds = GetMemberDatasetFromFile(file, out validationMessage);

                if (ds == null)
                    return Json(new { type = "warning", errorLisings = false, message = validationMessage },
                              JsonRequestBehavior.AllowGet);

                if (!string.IsNullOrWhiteSpace(validationMessage))
                    return Json(new { type = "warning", errorLisings = false, message = validationMessage },
                              JsonRequestBehavior.AllowGet);

                var salaryConfigExcelViewModelList = new List<PRSalaryConfigExcelViewModel>();
                long createdBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);

                // Generate member list
                validationMessage = GenerateEmployeeSalaryConfigurationList(salaryConfigExcelViewModelList, createdBy, ds);

                if (salaryConfigExcelViewModelList.Count == 0 &&
                    !string.IsNullOrWhiteSpace(validationMessage))
                {
                    importSalaryConfigurationErrorList = GetImportSalaryConfigurationErrorList(validationMessage);

                    return Json(new
                    {
                        type = "warning",
                        errorLisings = true,
                        importContactErrorList = importSalaryConfigurationErrorList,
                        message = "Error occurred. Please seee details in validation message section."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (salaryConfigExcelViewModelList.Count == 0)
                    return Json(new { type = "warning", errorLisings = false, message = "No configuration were found to import." },
                              JsonRequestBehavior.AllowGet);

                //let's create incentive for both incentive and deduction
                var response = ConfigureEmployeeSalary(salaryConfigExcelViewModelList);
                if (!response.IsSuccess)
                    return Json(new { type = "warning", message = response.Message },
                             JsonRequestBehavior.AllowGet);

                return Json(new { type = "success", message = "Import Existing Salary configuration successfull!." },
                              JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { type = "warning", message = "There was an error while adding Import Existing Salary Allowance and Deduction. Please try with valid excel data!" },
                            JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult ImportConfiguration2()
        {
            try
            {
                string validationMessage;
                var importSalaryConfigurationErrorList = "";

                if (!ModelState.IsValid)
                    return Json(new { type = "warning", errorLisings = false, message = "Error on file, Please try again" },
                               JsonRequestBehavior.AllowGet);

                if (Request.Files.Count <= 0)
                    return Json(new { type = "warning", errorLisings = false, message = "File not found. Please try again." },
                             JsonRequestBehavior.AllowGet);

                var file = Request.Files[0];

                // Generate dataset
                var ds = GetMemberDatasetFromFile2(file, out validationMessage);

                if (ds == null)
                    return Json(new { type = "warning", errorLisings = false, message = validationMessage },
                              JsonRequestBehavior.AllowGet);

                if (!string.IsNullOrWhiteSpace(validationMessage))
                    return Json(new { type = "warning", errorLisings = false, message = validationMessage },
                              JsonRequestBehavior.AllowGet);

                var salaryConfigExcelViewModelList = new List<PRSalaryConfigExcelViewModel>();
                long createdBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);

                // Generate member list
                validationMessage = GenerateEmployeeSalaryConfigurationList2(salaryConfigExcelViewModelList, createdBy, ds);

   
                importSalaryConfigurationErrorList = GetImportSalaryConfigurationErrorList2(validationMessage);


                return Json(new { type = "success", message = "Import Existing Salary configuration successfull!." },
                              JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { type = "warning", message = "There was an error while adding Import Existing Salary Allowance and Deduction. Please try with valid excel data!" },
                            JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Private Methods

        private void InsertNewSalaryConfiguration(
            List<PRSalaryConfigurationViewModel> salaryConfigurationList,
            int officeId, long employeeId,
            DateTime effectiveStartDate,
            DateTime effectiveEndDate)
        {
            var crObj = new List<PRSalaryConfiguration>();
            foreach (var item in salaryConfigurationList)
            {
                var entity = new PRSalaryConfiguration();
                entity.OfficeID = officeId;
                entity.EmployeeID = employeeId;
                entity.PRComponentID = item.PRComponentID;
                entity.ComponentAmount = item.ComponentAmount;
                entity.EffectiveStartDate = effectiveStartDate;
                entity.EffectiveEndDate = effectiveEndDate;
                entity.IsActive = true;
                entity.ComponentCategory = item.ComponentCategory;
                entity.TransactionType = item.TransactionType;
                entity.CreateUser = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                entity.CreateDate = DateTime.UtcNow;
                entity.UpdateDate = DateTime.UtcNow;
                entity.UpdateUser = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                prSalaryConfigurationService.Create(entity);
            }
        }

        private double CalculateRatioforComponent(double ratio, double amount)
        {
            return amount != 0 ? (ratio * amount) / 100 : 0;
        }

        private List<PRSalaryScaleViewModel> DistributeEmployeeSalaryInComponents(int empSalaryTypeId,
            double basicSalary, double gross, int EmployeeStatusId, int OfficeLocationId, int pfType)
        {
            var param2 = new
            {
                EmployeeTypeId = empSalaryTypeId,
                EmployeeStatusId = EmployeeStatusId,
                OfficeLocationId = OfficeLocationId,
                PFTypeId = pfType
            };

            var empTypeWiseCompConfig = employeeSPService.GetDataWithParameter(param2, "prl.SP_Get_EmployeeTypeWiseComponentConfiguration");
            empTypeWiseCompConfig.Tables[0].Columns.Add(new DataColumn("CalculatedAmount", typeof(System.Double)));

            for (int i = 0; i <= empTypeWiseCompConfig.Tables[0].Rows.Count - 1; i++)
            {
                var componentName = empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentName"].ToString();
                var componentType = empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentType"].ToString();
                var ratioPercent = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentAmount"].ToString());
                var ratioBasedOn = empTypeWiseCompConfig.Tables[0].Rows[i]["RatioBasedOn"].ToString();

                var salaryRoundType = empTypeWiseCompConfig.Tables[0].Rows[i]["SalaryRoundType"].ToString();

                if (componentType == SalaryCalculationTypeConstants.Ratio
                    && ratioBasedOn == RatioBasedOnConstants.Gross)
                {
                    var ratio = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), gross);
                    var maxLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MaximumLimit"].ToString());
                    var minLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MinimumLimit"].ToString());
                    if (ratio < minLimit && minLimit != 0)
                    {
                        ratio = minLimit;
                    }
                    if (ratio > maxLimit && maxLimit != 0)
                    {
                        ratio = maxLimit;
                    }

                    if (salaryRoundType == "RoundUp")
                    {
                        ratio = Math.Round(ratio);
                    }
                    if (salaryRoundType == "RoundDown")
                    {
                        ratio = Math.Ceiling(ratio);
                    }

                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratio;
                }
                else if (componentType == SalaryCalculationTypeConstants.Ratio
                        && ratioBasedOn == RatioBasedOnConstants.Basic)
                {
                    var ratio = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), basicSalary);
                    var maxLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MaximumLimit"].ToString());
                    var minLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MinimumLimit"].ToString());
                    if (ratio < minLimit && minLimit != 0)
                    {
                        ratio = minLimit;
                    }
                    if (ratio > maxLimit && maxLimit != 0)
                    {
                        ratio = maxLimit;
                    }

                    if (salaryRoundType == "RoundUp")
                    {
                        ratio = Math.Round(ratio);
                    }
                    if (salaryRoundType == "RoundDown")
                    {
                        ratio = Math.Ceiling(ratio);
                    }
                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratio;
                }
                else if (componentType == SalaryCalculationTypeConstants.Fixed)
                {
                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratioPercent;//for fixed ratioPercentage is the fixed amount
                }
            }

            List<PRSalaryScaleViewModel> dataList = new List<PRSalaryScaleViewModel>();

            dataList = empTypeWiseCompConfig.Tables[0].AsEnumerable()
            .Select(row => new PRSalaryScaleViewModel
            {
                PRComponentId = row.Field<int>("PRComponentId"),
                EmployeeTypeName = row.Field<string>("EmployeeTypeName"),
                ComponentGroupName = row.Field<string>("ComponentGroupName"),
                ComponentName = row.Field<string>("ComponentName"),
                ComponentType = row.Field<string>("ComponentType"),
                ComponentAmount = row.Field<decimal>("ComponentAmount"),
                RatioBasedOn = row.Field<string>("RatioBasedOn"),
                EmployeeTypeId = row.Field<int>("EmployeeTypeId"),
                CalculatedAmount = row.Field<double>("CalculatedAmount"),
                ComponentCategory = row.Field<string>("ComponentCategory"),
                TransactionType = row.Field<string>("TransactionType"),
                EmployeeStatusId = row.Field<int?>("EmployeeStatusId"),
                TransactionTypeView = row.Field<string>("TransactionTypeView")
            }).ToList();

            return dataList;
        }

        private void UpdateEmployee(
               long employeeId,
               int newDesignationId,
               int? employeeTypeId,
               bool PFApplicable,
               int pfTypeId,

               double grossSalary,
               int gradeId,
               int step,
               double totalEarnings,

               bool isOverTime,
               string maxOvertimePerDay,
               string maxOvertimePerMonth,
               string loginTime,
               string logoutTime,
               string lastLoginTime,

               string bankAccount,
               string bankName,
               string bankBranchName,

               DateTime effectiveStartDate,
               DateTime effectiveEndDate
          )
        {
            var model = employeeService.GetByEmpId(employeeId);
            model.GrossSalary = Convert.ToDecimal(grossSalary);
            model.TotalEarnings = Convert.ToDecimal(totalEarnings);
            model.EmployeeTypeId = employeeTypeId;

            if (PFApplicable == true)
            {
                model.IsPFApplicable = true;
                model.IsPFClossed = false;
            }
            else
            {
                model.IsPFApplicable = false;
                model.IsPFClossed = true;
            }

            model.BankAccountNo = bankAccount;
            model.GradeId = gradeId;
            model.Step = step;

            var currentDateTime = DateTime.Now;

            var currentDate = $"{currentDateTime.Year}-{currentDateTime.Month}-{currentDateTime.Day}";

            if (string.IsNullOrWhiteSpace(loginTime) ||
                string.IsNullOrWhiteSpace(logoutTime) ||
                string.IsNullOrWhiteSpace(lastLoginTime))
            {
                loginTime = $"{currentDate} 10:00:00.000";
                logoutTime = $"{currentDate} 18:00:00.000";
                lastLoginTime = $"{currentDate} 10:00:00.000";
            }

            model.LoginTime = Convert.ToDateTime(loginTime);
            model.LogoutTime = Convert.ToDateTime(logoutTime);
            model.LastLoginTime = Convert.ToDateTime(lastLoginTime);
            model.EffectiveStartDate = effectiveStartDate;
            model.EffectiveEndDate = effectiveEndDate;
            model.IsOverTime = isOverTime;
            model.MaxOvertimePerDay = maxOvertimePerDay == string.Empty ? 0 : Convert.ToDecimal(maxOvertimePerDay);
            model.MaxOvertimePerMonth = maxOvertimePerMonth == string.Empty ? 0 : Convert.ToDecimal(maxOvertimePerMonth);
            //model.OvertimeHour = overtimeHour == string.Empty ? 0 : Convert.ToDecimal(overtimeHour);
            model.UpdateUser = SessionHelper.LoggedInEmployeeID;
            model.UpdateDate = DateTime.Now;
            //model.IncrementMonth = incrementMonth;
            //model.OvertimeRate = overTimeRate == string.Empty ? 0 : Convert.ToDecimal(overTimeRate);
            model.BankName = bankName == null ? "" : bankName;
            model.BankBranchName = bankBranchName == null ? "" : bankBranchName;
            model.PFTypeId = pfTypeId;
            if (newDesignationId > 0)
            {
                model.DesignationId = newDesignationId;
            }
            //  model.IncrementYearFrom = incrementYearFrom;
            employeeService.Update(model);
        }

        private BaseResponse ConfigureEmployeeSalary(List<PRSalaryConfigExcelViewModel> salaryConfigExcelListing)
        {
            bool isOperationSuccess = true;
            var response = new BaseResponse();
            string result = string.Empty;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(2, 0, 0)))
            {
                try
                {
                    foreach (var item in salaryConfigExcelListing)
                    {
                        item.EmployeeCode = CommonHelper.GetFormattedEmployeeCodeWithFourDigit(item.EmployeeCode);

                        var employee = employeeService.GetEmployeeByEmployeeCode(item.EmployeeCode);

                        if (employee == null)
                        {
                            result = $@"Employee not found with code: {item.EmployeeCode}";
                            response.IsSuccess = false;
                            response.Message = result;
                            isOperationSuccess = false;
                            break;
                        }

                        var officeDetail = officeService.GetById((int)employee.OfficeId);
                        if (officeDetail == null || !(officeDetail.OfficeLocationId > 0))
                        {
                            result = $@"Employee Office not found with employee code: {item.EmployeeCode}";
                            response.IsSuccess = false;
                            response.Message = result;
                            isOperationSuccess = false;
                            break;
                        }

                        int employeeId = Convert.ToInt32(employee.EmployeeId);

                        int empSalaryTypeId = Convert.ToInt32(SalaryStructureTypeConstants.Unstructured);
                        int EmployeeStatusId = employee.EmployeeStatusId;
                        double grossSalary = item.GrossSalary;
                        string salaryGenerationType = EmploymentTypeConstants.NonPayScale;
                        int OfficeLocationId = Convert.ToInt32(officeDetail.OfficeLocationId);
                        string pfTypeId = ProvidentFundTypeConstants.GPF;

                        //get employee salary configurations
                        var salaryScaleListing = GenerateEmployeeSalary(empSalaryTypeId, EmployeeStatusId, grossSalary,
                                                      salaryGenerationType, OfficeLocationId, pfTypeId);

                        if (!salaryScaleListing.Any())
                        {
                            result = $@"Employee Salary Scale not found with code: {item.EmployeeCode}";
                            response.IsSuccess = false;
                            response.Message = result;
                            isOperationSuccess = false;
                            break;
                        }

                        //prepare PR Salary Configuration listing
                        var salaryConfigurationList = new List<PRSalaryConfigurationViewModel>();
                        foreach (var salaryScale in salaryScaleListing)
                        {
                            var newSalaryConfiguration = new PRSalaryConfigurationViewModel
                            {
                                PRComponentID = salaryScale.PRComponentId,
                                ComponentAmount = Convert.ToDecimal(salaryScale.CalculatedAmount),
                                ComponentCategory = salaryScale.ComponentCategory,
                                TransactionType = salaryScale.TransactionType
                            };

                            salaryConfigurationList.Add(newSalaryConfiguration);
                        }

                        if (!salaryConfigurationList.Any())
                        {
                            result = $@"Employee Salary Configuration not found with code: {item.EmployeeCode}";
                            response.IsSuccess = false;
                            response.Message = result;
                            isOperationSuccess = false;
                            break;
                        }

                        var model = PopulateSalaryConfigurationImport(item, employee, officeDetail, employeeId,
                                                            grossSalary, pfTypeId, salaryConfigurationList);

                        //let's create employee salary configuration
                        response = CreateEmployeeSalaryConfiguration(model);

                        if (!response.IsSuccess)
                        {
                            isOperationSuccess = false;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.Message = "There was an error while adding. Please try again!";
                    isOperationSuccess = false;
                }

                if (isOperationSuccess)
                    scope.Complete();

                scope.Dispose();
            }

            return response;
        }

        private PRSalaryConfigurationImportViewModel PopulateSalaryConfigurationImport(
            PRSalaryConfigExcelViewModel item, Data.CodeFirstMigration.Employee employee,
            Data.CodeFirstMigration.Office officeDetail,
            int employeeId, double grossSalary, string pfTypeId,
            List<PRSalaryConfigurationViewModel> salaryConfigurationList)
        {
            var company = companyService.GetById((int)employee.CompanyId);
            if (company.CompanyCode.Trim().ToLower() == GHRMPlusCompanyConstants.GrameenTelecomTrust.Trim().ToLower())
                employee.EmployeeTypeId = Convert.ToInt32(EmployeeTypeConfigConstants.NonPayScale);//Under Pay Scale

            var model = new PRSalaryConfigurationImportViewModel
            {
                SalaryConfigurationList = salaryConfigurationList,

                OfficeId = officeDetail.OfficeId,
                EmployeeId = employeeId,
                NewDesignationId = 0,
                PromotionId = 0,
                PromotionTypeId = 0,
                EmployeeTypeId = employee.EmployeeTypeId,
                PFTypeId = pfTypeId,
                GrossSalary = grossSalary,
                GradeId = null,
                Step = null,
                IsOverTime = item.IsOverTime,
                LoginTime = employee.LoginTime.ToString(),
                LogoutTime = employee.LogoutTime.ToString(),
                LastLoginTime = employee.LastLoginTime.ToString(),
                BankAccount = item.BankAccountNo,
                BankName = item.BankName,
                BankBranchName = item.BankBranchName,
                PromotionDate = "",
                NextReviewDate = "",
                EffectiveStartDate = item.EffectiveStartDate,
                EffectiveEndDate = item.EffectiveEndDate
            };

            if (item.IsOverTime)
            {
                model.MaxOvertimePerDay = item.MaxOvertimePerDay.ToString();
                model.MaxOvertimePerMonth = item.MaxOvertimePerMonth.ToString();
            }

            return model;
        }

        private List<PRSalaryScaleViewModel> GenerateEmployeeSalary(
           int empSalaryTypeId, int EmployeeStatusId, double grossSalary,
           string salaryGenerationType, int OfficeLocationId, string pfTypeId)
        {

            if (salaryGenerationType == EmploymentTypeConstants.PayScale)
                empSalaryTypeId = 1;
            if (salaryGenerationType == EmploymentTypeConstants.NonPayScale)
                empSalaryTypeId = 2;

            List<PRSalaryScaleViewModel> prSalaryList = new List<PRSalaryScaleViewModel>();
            double basicSalary = 0;
            try
            {
                var param2 = new
                {
                    EmployeeTypeId = Convert.ToInt32(empSalaryTypeId),
                    EmployeeStatusId = EmployeeStatusId,
                    OfficeLocationId = OfficeLocationId,
                    PFTypeId = Convert.ToInt32(pfTypeId)
                };

                //get payroll components from [prl].[PRComponent]
                var empTypeWiseCompConfig = employeeSPService.GetDataWithParameter(param2, "prl.SP_Get_EmployeeTypeWiseComponentConfiguration");

                for (int i = 0; i <= empTypeWiseCompConfig.Tables[0].Rows.Count - 1; i++)
                {
                    if (empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim() == "Basic Salary")
                    {
                        basicSalary = CalculateRatioforComponent(Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentAmount"].ToString()), grossSalary);
                        break;
                    }
                    //break;
                }

                if (basicSalary > 0)
                {
                    prSalaryList = DistributeEmployeeSalaryInComponents(empSalaryTypeId, basicSalary, grossSalary,
                        EmployeeStatusId, OfficeLocationId, Convert.ToInt32(pfTypeId));
                }
            }
            catch (Exception ex)
            {
                prSalaryList = new List<PRSalaryScaleViewModel>();
            }

            return prSalaryList;
        }

        private BaseResponse CreateEmployeeSalaryConfiguration(PRSalaryConfigurationImportViewModel model)
        {
            double totalEarnings = 0;
            bool pfApplicable = false;
            var response = new BaseResponse();

            var effectiveStartDateDt = Convert.ToDateTime(model.EffectiveStartDate);
            var effectiveEndDateDate = Convert.ToDateTime(model.EffectiveEndDate);

            var effectiveEndDateDt = new DateTime(effectiveEndDateDate.Year, effectiveEndDateDate.Month, 1).AddMonths(1).AddDays(-1);

            var firstDayOfEndMonth = new DateTime(effectiveEndDateDt.Year, effectiveEndDateDt.Month, 1);
            var lastDayOfEndMonth = new DateTime(effectiveEndDateDt.Year, effectiveEndDateDt.Month, 1).AddMonths(1).AddDays(-1);

            if (lastDayOfEndMonth > effectiveEndDateDt)
            {
                response.Message = "End Date needs to be last date of month, Configuration Denied";
                return response;
            }

            var salaryYear = effectiveStartDateDt.Year;
            int salaryMonth = effectiveStartDateDt.Month;

            //check monthly salary approved (Table: [prl].[EmployeeMonthlySalaryApproved]) for this month and year and of this employee. 
            //if not found then process will be continued
            var paramSalaryCheck = new { SalaryYear = salaryYear, SalaryMonth = salaryMonth, EmployeeId = model.EmployeeId };
            var listSalaryApproved = employeeSPService.GetDataWithParameter(paramSalaryCheck, "prl.SP_Check_MonthlySalaryApproved");

            var checkSalaryConfiguration = listSalaryApproved.Tables[0].AsEnumerable().Select(row => new EmployeeMonthlySalaryApproved()
            {
                EmployeeId = row.Field<long>("EmployeeId"),
                PRComponentId = row.Field<int>("PRComponentId"),
            }).ToList();

            if (checkSalaryConfiguration.Any())
            {
                response.Message = "Already salary Approved for this configuration, Configuration Denied";
                return response;
            }

            //check salary configuration (table: [prl].[EmployeeMonthlySalary]) for this month and year and IsSendForApproval=1 or IsRejected=1 or IsApproved=1 of this employee.
            //if not found then process will be continued
            var listSalaryBeforeApproval = employeeSPService.GetDataWithParameter(paramSalaryCheck, "prl.SP_Check_MonthlySalaryBeforeApproval");
            var checkSalaryGenerated = listSalaryBeforeApproval.Tables[0].AsEnumerable().Select(row => new EmployeeMonthlySalary()
            {
                EmployeeId = row.Field<long>("EmployeeId"),
                PRComponentId = row.Field<int>("PRComponentId"),
            }).ToList();

            if (checkSalaryGenerated.Any())
            {
                response.Message = "Already salary Generated for this configuration, Configuration Denied";
                return response;
            }

            try
            {
                //if salary configuration found for this employee ([prl].[PRSalaryConfiguration]) 
                //then update as isactive=0
                var existingEmpSalary = prSalaryConfigurationService.ExisstPRSalaryConfigurationByEmployeeId(model.EmployeeId);

                if (existingEmpSalary)
                {
                    var paramS = new { EffectiveStartDate = effectiveStartDateDt.AddDays(-1), EmployeeId = model.EmployeeId };
                    employeeSPService.GetDataWithParameter(paramS, "prl.SP_DeleteSalaryConfigurationForSameEffectiveStartDate");
                }

                //get total earnings from salary detail listings
                totalEarnings = Convert.ToDouble(model.SalaryConfigurationList.Sum(p => p.ComponentAmount));

                //let's add into [prl].[PRSalaryConfiguration]
                InsertNewSalaryConfiguration(model.SalaryConfigurationList, model.OfficeId, model.EmployeeId, effectiveStartDateDt, effectiveEndDateDt);

                //get possible provident fund related components
                var prComponents = prComponentService.GetMany(p => p.IsProvidentFundComponent == true).ToList();

                //check provident fund is applicable or not 
                foreach (var item in model.SalaryConfigurationList)
                {
                    if (prComponents.Where(p => p.PRComponentID == item.PRComponentID).Any())
                    {
                        pfApplicable = true;
                        break;
                    }
                }

                int salarygradeId = 0;
                int salarystep = 0;

                if (model.EmployeeTypeId == Convert.ToInt32(EmployeeTypeConfigConstants.PayScale))//Under Pay Scale
                {
                    salarygradeId = model.GradeId == "" ? 0 : Convert.ToInt32(model.GradeId);
                    salarystep = model.Step == "" ? 0 : Convert.ToInt32(model.Step);
                }

                //let's update employee related information in employee table
                UpdateEmployee(model.EmployeeId, model.NewDesignationId, model.EmployeeTypeId, pfApplicable, Convert.ToInt32(model.PFTypeId),
                    model.GrossSalary, Convert.ToInt32(salarygradeId), Convert.ToInt32(salarystep), totalEarnings,
                    model.IsOverTime, model.MaxOvertimePerDay, model.MaxOvertimePerMonth, model.LoginTime, model.LogoutTime, model.LastLoginTime,
                    model.BankAccount, model.BankName, model.BankBranchName, effectiveStartDateDt, effectiveEndDateDt);

                response.Message = "Ok";
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.InnerException.Message.ToString();
                response.IsSuccess = false;
                return response;
            }
        }

        private string GetImportSalaryConfigurationErrorList(string validationMessage)
        {
            var validationErrorList = validationMessage.Split(new string[] { "Error:" }, StringSplitOptions.None);
            var htmlContent = $@" 
                   
                    <div class='row'>
                        <div class='col-md-12'>
                            <div class='panel panel-primary'>
                                <div class='panel-body'>
                                    <div class='lead'>Import Validation Message Summary <small>Partially Imported. Please see details below...</small> </div>
                                    <hr />
                                    <ul class='list-group'>";
            int index = 1;
            foreach (var error in validationErrorList)
            {
                if (!string.IsNullOrWhiteSpace(error))
                {
                    htmlContent += $@" <li class='list-group-item'>{index}. {error}</li>";
                    index++;
                }
            }

            htmlContent += $@"</ul>
                                </div>
                            </div>
                        </div>
                    </div>
                    ";

            return htmlContent;
        }


        private string GetImportSalaryConfigurationErrorList2(string validationMessage)
        {
            var result = employeeSPService.GetDataWithoutParameter("SP_SET_CALCULATION_FROM_EXCEL_SALARY_CONFIGURATION");

            return "";
        }


        private string GenerateEmployeeSalaryConfigurationList(
            ICollection<PRSalaryConfigExcelViewModel> salaryConfigExcelViewModelList,
            long createdBy, DataSet ds)
        {
            var validationMessage = "";

            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows == null)
                return "There is an issue reading data from this file. Please try again.";

            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var j = 0;
                var errorMessage = "";
                var newSalaryConfigExcelViewModel = new PRSalaryConfigExcelViewModel();

                //employee code
                var employeeCode = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(employeeCode))
                    newSalaryConfigExcelViewModel.EmployeeCode = employeeCode;
                else
                    errorMessage += " Error: Employee Code not found in the file. " +
                                         "Row is " + (1 + i) + " and column is " + j;

                //gross salary
                var grossSalary = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(grossSalary))
                    newSalaryConfigExcelViewModel.GrossSalary = Convert.ToDouble(grossSalary);
                else
                    errorMessage += " Error: Gross Salary not found in the file. " +
                                         "Row is " + (1 + i) + " and column is " + j;

                //is overtime
                var isOvertime = ds.Tables[0].Rows[i][j++].ToString();
                var overtimeAllow = false;

                if (!string.IsNullOrWhiteSpace(isOvertime))
                {
                    newSalaryConfigExcelViewModel.IsOverTime = isOvertime.ToLower() == "yes" ? true : false;
                    overtimeAllow = newSalaryConfigExcelViewModel.IsOverTime;
                }
                else
                {
                    errorMessage += " Error: Is Overtime not found in the file. " +
                                         "Row is " + (1 + i) + " and column is " + j;
                }

                //max overtime per day
                var maxOvertimePerDay = ds.Tables[0].Rows[i][j++].ToString();

                if (overtimeAllow && !string.IsNullOrWhiteSpace(maxOvertimePerDay))
                    newSalaryConfigExcelViewModel.MaxOvertimePerDay = Convert.ToInt32(maxOvertimePerDay);

                //max overtime per month
                var maxOvertimePerMonth = ds.Tables[0].Rows[i][j++].ToString();

                if (overtimeAllow && !string.IsNullOrWhiteSpace(maxOvertimePerMonth))
                    newSalaryConfigExcelViewModel.MaxOvertimePerMonth = Convert.ToInt32(maxOvertimePerMonth);

                // bank name
                var bankName = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(bankName))
                    newSalaryConfigExcelViewModel.BankName = bankName;

                // bank branch name
                var bankBranchName = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(bankBranchName))
                    newSalaryConfigExcelViewModel.BankBranchName = bankBranchName;

                // bank account no
                var bankAccountNo = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(bankAccountNo))
                    newSalaryConfigExcelViewModel.BankAccountNo = bankAccountNo;

                // effective start date
                var effectiveStartDate = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(effectiveStartDate))
                    newSalaryConfigExcelViewModel.EffectiveStartDate = effectiveStartDate;

                // effective end date
                var effectiveEndDate = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(effectiveEndDate))
                    newSalaryConfigExcelViewModel.EffectiveEndDate = effectiveEndDate;

                if (string.IsNullOrEmpty(errorMessage))
                    salaryConfigExcelViewModelList.Add(newSalaryConfigExcelViewModel);
                else
                    validationMessage += errorMessage;
            }

            return validationMessage;
        }

        private string GenerateEmployeeSalaryConfigurationList2(
     ICollection<PRSalaryConfigExcelViewModel> salaryConfigExcelViewModelList,
     long createdBy, DataSet ds)
        {
            var validationMessage = "";

            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows == null)
                return "There is an issue reading data from this file. Please try again.";

            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var j = 0;
                var errorMessage = "";
               
                //employee code
                var EmployeeCode = ds.Tables[0].Rows[i][j++].ToString();


                //gross salary
                var ProvidentFundType = ds.Tables[0].Rows[i][j++].ToString();

                //is overtime
                var SalaryType = ds.Tables[0].Rows[i][j++].ToString();
                
                //max overtime per day
                var GenerationType = ds.Tables[0].Rows[i][j++].ToString();

                //max overtime per month
                var GradeList = ds.Tables[0].Rows[i][j++].ToString();


                // bank name
                var Step = ds.Tables[0].Rows[i][j++].ToString();

                // effective start date
                var effectiveStartDate = ds.Tables[0].Rows[i][j++].ToString();

                // effective end date
                var effectiveEndDate = ds.Tables[0].Rows[i][j++].ToString();



                var param = new
                {
                    EmployeeCode = EmployeeCode,
                    ProvidentFundType = ProvidentFundType,
                    SalaryType = SalaryType,
                    GenerationType = GenerationType,
                    GradeList = GradeList,
                    Step = Step,
                    EffectiveStartDate = effectiveStartDate,
                    EffectiveEndDate = effectiveEndDate,
                    
                    CreateBy = SessionHelper.LoggedInEmployeeID,
                };

                employeeSPService.GetDataWithParameter(param, "SP_INSERT_FROM_EXCEL_FOR_SALARY_CONFIGURATION");


            }

            return validationMessage;
        }

        private DataSet GetMemberDatasetFromFile(HttpPostedFileBase file, out string validationMessage)
        {
            var ds = new DataSet();

            validationMessage = "";

            if (file != null && file.ContentLength > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    var ticks = DateTime.Now.Ticks;

                    var serverMappedPath = Server.MapPath("~/WebShared/Uploads/SalaryConfigurationImport/");
                    var fileLocation = $"{serverMappedPath}{ticks}/{file.FileName}";
                    var directory = $"{serverMappedPath}{ticks}";

                    try
                    {
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }

                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        file.SaveAs(fileLocation);
                    }
                    catch
                    {
                        validationMessage = "Error on processing file, Please try again";
                        return null;
                    }

                    var excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
                        + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

                    //Create Connection to Excel work book and add oledb namespace
                    var excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();

                    var dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    if (dt == null)
                    {
                        validationMessage = "Error on processing file, Please try again";
                        return null;
                    }

                    var excelSheets = new string[dt.Rows.Count];
                    var t = 0;

                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    var excelConnection1 = new OleDbConnection(excelConnectionString);


                    var query = string.Format("Select * from [{0}]", excelSheets[0]);

                    using (var dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }

                    excelConnection.Close();
                }
                else
                {
                    validationMessage = "Error! Please import an correct file. You can download the sample file & try again.";
                    return null;
                }
            }
            else
            {
                validationMessage = "Error on file. Please try again.";
                return null;
            }

            return ds;
        }

        private DataSet GetMemberDatasetFromFile2(HttpPostedFileBase file, out string validationMessage)
        {
            var ds = new DataSet();

            validationMessage = "";

            if (file != null && file.ContentLength > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    var ticks = DateTime.Now.Ticks;

                    var serverMappedPath = Server.MapPath("~/WebShared/Uploads/SalaryConfigurationImport/");
                    var fileLocation = $"{serverMappedPath}{ticks}/{file.FileName}";
                    var directory = $"{serverMappedPath}{ticks}";

                    try
                    {
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }

                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        file.SaveAs(fileLocation);
                    }
                    catch
                    {
                        validationMessage = "Error on processing file, Please try again";
                        return null;
                    }

                    var excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
                        + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

                    //Create Connection to Excel work book and add oledb namespace
                    var excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();

                    var dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    if (dt == null)
                    {
                        validationMessage = "Error on processing file, Please try again";
                        return null;
                    }

                    var excelSheets = new string[dt.Rows.Count];
                    var t = 0;

                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    var excelConnection1 = new OleDbConnection(excelConnectionString);


                    var query = string.Format("Select * from [{0}]", excelSheets[0]);

                    using (var dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }

                    excelConnection.Close();
                }
                else
                {
                    validationMessage = "Error! Please import an correct file. You can download the sample file & try again.";
                    return null;
                }
            }
            else
            {
                validationMessage = "Error on file. Please try again.";
                return null;
            }

            return ds;
        }



        #endregion
    }
}