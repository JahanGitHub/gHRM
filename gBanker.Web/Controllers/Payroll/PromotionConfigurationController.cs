
#region Usings
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Mvc;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service;
using gHRM.Service.Basic;
using gHRM.Service.Payroll;
using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using gHRM.Web.ViewModels.Payroll;
using gHRM.Data.CodeFirstMigration.EmployeePromotion;
using gHRM.Core.Utilities.Constants;

#endregion

namespace gHRM.Web.Controllers.Payroll
{
    public class PromotionConfigurationController : BaseController
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
        private readonly ICompanyWisePayrollConfigService companyWisePayrollConfigService;

        private readonly IOfficeService officeService;
        private CommonStaticDropDown commonStaticDropDown;
        private CommonDynamicDropDown CommonDynamicDropDown;

        public PromotionConfigurationController(
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
            ICompanyWisePayrollConfigService companyWisePayrollConfigService,
            IOfficeService officeService)
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
            this.companyWisePayrollConfigService = companyWisePayrollConfigService;

            commonStaticDropDown = new CommonStaticDropDown();
            CommonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion

        #region Configure
        public ActionResult Configure(string employeeCode, int promotionId = 0)
        {
            if (string.IsNullOrWhiteSpace(employeeCode))
                return Redirect("/EmployeePromotion/Index");

            var model = new PRSalaryConfigurationViewModel();
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;
            MapDropDown(model);
            model.EmployeeCode = employeeCode;
            model.PromotionId = promotionId;
            return View(model);
        }

        #endregion

        public ActionResult EmployeeSalaryConfigurationAfterAssesment(long? eid, int? pid)
        {
            if (eid.HasValue && pid.HasValue)
            {
                var model = new PRSalaryConfigurationViewModel();
                IEnumerable<SelectListItem> items = new SelectList(" ");
                ViewData["ComponentList"] = items;
                MapDropDown(model);
                var emp = employeeService.GetByEmpId(eid.Value);
                model.EmployeeCode = emp.EmployeeCode;
                model.PromotionId = pid.Value;
                return View(model);
            }
            else
                return RedirectToAction("", "");
        }
        #region Employee Salary Grade
        public ActionResult EmployeeSalaryGrade()
        {
            var model = new EmployeeGradeListViewModel { };

            return View(model);
        }

        #endregion

        #region HttpRequests

        public JsonResult GetExistingSalaryConfigurationList(long employeeId)
        {
            try
            {
                var dataList = viewSalaryConfigurationService.GetEmployeeSalaryConfigurationList(employeeId);
                return Json(new { Result = "OK", dataList, Message = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Result = "ERROR", Message = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetExistingSalaryConfigurationListbyEmployeeCode(string employeeCode)
        {
            try
            {
                var withResignEmployee = false;
                //get employee information
                var employeeInfo = employeeService.GetByCode(employeeCode.Trim(), withResignEmployee);

                if (employeeInfo == null)
                    return Json(new { Result = "ERROR", Message = "Employee not exist. Please try again!" }, JsonRequestBehavior.AllowGet);

                var officeInfo = officeService.Get(b => b.OfficeId == employeeInfo.OfficeId);
                var joiningDate = Convert.ToDateTime(employeeInfo.FirstJoiningDate).ToString("dd-MMM-yyyy");
                var confirmationDate = employeeInfo.ConfirmationDate != null
                    ? Convert.ToDateTime(employeeInfo.ConfirmationDate).ToString("dd-MMM-yyyy")
                    : "N/A";

                var departmentName = employeeDepartmentService.GetById(Convert.ToInt32(employeeInfo.DepartmentId)).DepartmentName;
                var designationName = employeeDesignationService.GetById(Convert.ToInt32(employeeInfo.DesignationId)).DesignationName;

                //get employee promotion from [promo].[EmployeePromotion]
                var promotionInfo = employeePromotionService.GetLastPromotionInfo(employeeInfo.EmployeeId);

                var promotionDate = string.Empty;
                var nextReviewDate = string.Empty;
                int promotionTypeId = 0;

                if (promotionInfo != null)
                {
                    promotionDate = Convert.ToDateTime(promotionInfo.PromotionDate).ToString("dd-MMM-yyyy");
                    nextReviewDate = Convert.ToDateTime(promotionInfo.NextReviewDate).ToString("dd-MMM-yyyy");
                    promotionTypeId = promotionInfo.PromotionTypeId;
                }

                var dataList = new List<View_EmployeeSalaryConfiguration>();
                //get existing salary configuration data [prl.View_EmployeeSalaryConfiguration]
                dataList = viewSalaryConfigurationService.GetEmployeeSalaryConfigurationListbyCode(employeeCode);

                //if not exist salary configuration data then generate fly data listing
                if (dataList.Count <= 0)
                {
                    //generate fly data listing for salary configuration
                    dataList = GenerateDataList(employeeCode);
                }

                var employeeStatusId = employeeInfo.EmployeeStatusId;
                //var employeeStatusId = employeeInfo.promo;

                var designationId = employeeInfo.DesignationId;
                var bankAccountNo = employeeInfo.BankAccountNo;
                var bankName = employeeInfo.BankName;
                var bankBranchName = employeeInfo.BankBranchName;
                var officeLocationId = officeInfo.OfficeLocationId;
                var officeId = officeInfo.OfficeId;
                var pfTypeId = employeeInfo.PFTypeId;
                var gradeId = employeeInfo.GradeId;

                return Json(new
                {
                    Result = "OK",
                    dataList,
                    Message = "OK",
                    JoiningDate = joiningDate,
                    ConfirmationDate = confirmationDate,
                    DepartmentName = departmentName,
                    DesignationName = designationName,
                    OfficeId = officeId,
                    OfficeLocationId = officeLocationId,
                    PromotionDate = promotionDate,
                    NextReviewDate = nextReviewDate,
                    PromotionTypeId = promotionTypeId,
                    EmployeeStausId = employeeStatusId,
                    DesignationId = designationId,
                    BankAccountNo = bankAccountNo,
                    BankName = bankName,
                    BankBranchName = bankBranchName,
                    PFTypeId = pfTypeId,
                    GradeId = gradeId,
                    IsOvertimeException = employeeInfo.IsOvertimeException,
                    PayrollConfigurationType = SessionHelper.PayrollConfigurationType

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetExistingSalaryConfigurationListbyEmployeeCodeNew(string employeeCode, int? pid)
        {
            try
            {
                var withResignEmployee = false;
                //get employee information
                var employeeInfo = employeeService.GetByCode(employeeCode.Trim(), withResignEmployee);

                if (employeeInfo == null)
                    return Json(new { Result = "ERROR", Message = "Employee not exist. Please try again!" }, JsonRequestBehavior.AllowGet);

                var officeInfo = officeService.Get(b => b.OfficeId == employeeInfo.OfficeId);
                var joiningDate = Convert.ToDateTime(employeeInfo.FirstJoiningDate).ToString("dd-MMM-yyyy");
                var confirmationDate = employeeInfo.ConfirmationDate != null
                    ? Convert.ToDateTime(employeeInfo.ConfirmationDate).ToString("dd-MMM-yyyy")
                    : "N/A";

                var departmentName = employeeDepartmentService.GetById(Convert.ToInt32(employeeInfo.DepartmentId)).DepartmentName;
                var designationName = employeeDesignationService.GetById(Convert.ToInt32(employeeInfo.DesignationId)).DesignationName;

                //get employee promotion from [promo].[EmployeePromotion]
                string lastPromotionDate = "", lastNextReviewDate = "";
                var pro_obj = employeePromotionService.GetById(pid ?? 0);

                var pro_lst = employeePromotionService.GetMany(x => x.EmployeeId == employeeInfo.EmployeeId && x.PromotionId != pro_obj.PromotionId).ToList();
                if (pro_lst.Any())
                {
                    lastPromotionDate = pro_lst.Max(x => x.PromotionDate ?? employeeInfo.FirstJoiningDate).ToString("dd-MMM-yyyy");
                    lastNextReviewDate = pro_lst.Max(x => x.NextReviewDate ?? pro_obj.PromotionDate ?? employeeInfo.FirstJoiningDate).ToString("dd-MMM-yyyy");
                }

                else
                {
                    lastPromotionDate = employeeInfo.FirstJoiningDate.ToString("dd-MMM-yyyy");
                    lastNextReviewDate = (pro_obj.PromotionDate ?? employeeInfo.FirstJoiningDate).ToString("dd-MMM-yyyy");
                }


                //var promotionInfo = employeePromotionService.GetLastPromotionInfo(employeeInfo.EmployeeId);

                var promotionDate = string.Empty;
                var nextReviewDate = string.Empty;
                int promotionTypeId = 0;


                promotionDate = Convert.ToDateTime(pro_obj.PromotionDate).ToString("dd-MMM-yyyy");
                nextReviewDate = Convert.ToDateTime(pro_obj.NextReviewDate ?? (pro_obj.PromotionDate ?? employeeInfo.FirstJoiningDate).AddYears(1)).ToString("dd-MMM-yyyy");
                promotionTypeId = pro_obj.PromotionTypeId;

                var dataList = new List<View_EmployeeSalaryConfiguration>();
                //get existing salary configuration data [prl.View_EmployeeSalaryConfiguration]
                dataList = viewSalaryConfigurationService.GetEmployeeSalaryConfigurationListbyCode(employeeCode);

                //if not exist salary configuration data then generate fly data listing
                if (dataList.Count <= 0)
                    //generate fly data listing for salary configuration
                    dataList = GenerateDataList(employeeCode);

                var employeeStatusId = employeeInfo.EmployeeStatusId;
                //var employeeStatusId = employeeInfo.promo;

                var designationId = employeeInfo.DesignationId;
                var bankAccountNo = employeeInfo.BankAccountNo;
                var bankName = employeeInfo.BankName;
                var bankBranchName = employeeInfo.BankBranchName;
                var officeLocationId = officeInfo.OfficeLocationId;
                var officeId = officeInfo.OfficeId;
                var pfTypeId = employeeInfo.PFTypeId;
                var gradeId = employeeInfo.GradeId;

                return Json(new
                {
                    Result = "OK",
                    dataList,
                    Message = "OK",
                    JoiningDate = joiningDate,
                    ConfirmationDate = confirmationDate,
                    DepartmentName = departmentName,
                    DesignationName = designationName,
                    OfficeId = officeId,
                    OfficeLocationId = officeLocationId,
                    PreviousPromotionDate = lastPromotionDate,
                    PreviousNextReviewDate = lastNextReviewDate,
                    PromotionDate = promotionDate,
                    NextReviewDate = nextReviewDate,
                    PromotionTypeId = promotionTypeId,
                    EmployeeStausId = employeeStatusId,
                    DesignationId = designationId,
                    BankAccountNo = bankAccountNo,
                    BankName = bankName,
                    BankBranchName = bankBranchName,
                    PFTypeId = pfTypeId,
                    GradeId = gradeId,
                    IsOvertimeException = employeeInfo.IsOvertimeException,
                    PayrollConfigurationType = SessionHelper.PayrollConfigurationType

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GenerateSalaryForEmployeeInPayScale(string empSalaryTypeId, string grade, string scale, int EmployeeStatusId, int OfficeLocationId, string providentFundTypeId)
        {
            List<PRSalaryScaleViewModel> dataTable = new List<PRSalaryScaleViewModel>();
            double basicSalary = 0;
            try
            {
                var param = new
                {
                    GradeId = Convert.ToInt32(grade)
                };

                //get grade by grade id from [EmployeeGradeList]
                var salaryGrade = employeeSPService.GetDataWithParameter(param, "prl.SP_Get_EmployeeGradeByGradeId");

                double initialAmt = Convert.ToDouble(salaryGrade.Tables[0].Rows[0][3].ToString());
                double amtPerIncrement = Convert.ToDouble(salaryGrade.Tables[0].Rows[0][4].ToString());
                double scaleInStep = Convert.ToDouble(scale);

                //calculate Gross about
                var grossSalary = CalcualteGross(initialAmt, amtPerIncrement, scaleInStep);

                var param2 = new
                {
                    EmployeeTypeId = Convert.ToInt32(empSalaryTypeId),
                    EmployeeStatusId = EmployeeStatusId,
                    OfficeLocationId = OfficeLocationId,
                    PFTypeId = Convert.ToInt32(providentFundTypeId)
                };

                //get payroll components from [prl].[PRComponent]
                var empTypeWiseCompConfig = employeeSPService.GetDataWithParameter(param2, "prl.SP_Get_EmployeeTypeWiseComponentConfiguration");

                for (int i = 0; i <= empTypeWiseCompConfig.Tables[0].Rows.Count - 1; i++)
                {
                    if (empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim() == "Basic Salary")
                    {
                        var ratioBaseOn = empTypeWiseCompConfig.Tables[0].Rows[i][6].ToString().Trim();
                        var payrollConfigurationType = SessionHelper.PayrollConfigurationType;

                        if (payrollConfigurationType == PayrollConfigurationTypeConstants.Basic)
                        {
                            if (ratioBaseOn != SalaryRatioConstants.NotRequired)
                                continue;

                            var ratio = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentAmount"].ToString());
                            basicSalary = CalculateBasicRatioOrFixedforComponent(ratio, grossSalary);
                            break;
                        }
                        else
                        {
                            if (ratioBaseOn != SalaryRatioConstants.Gross)
                                continue;

                            var ratio = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentAmount"].ToString());
                            basicSalary = CalculateRatioforComponent(ratio, grossSalary);
                            break;
                        }
                    }

                    //break;
                }

                if (basicSalary > 0)
                {
                    dataTable = EmployeeInPayScale(grade, scale, empSalaryTypeId, basicSalary, grossSalary, EmployeeStatusId, OfficeLocationId, Convert.ToInt32(providentFundTypeId));
                }
                return Json(new { Result = "OK", dataTable, grossSalary, Message = "OK" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var result = 0;
                return Json(new { Result = result, Message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GenerateSalaryForEmployeeNotInPayScale(string empSalaryTypeId, double grossSalary, int EmployeeStatusId, int OfficeLocationId)
        {
            List<PRSalaryScaleViewModel> dataTable = new List<PRSalaryScaleViewModel>();
            double basicSalary = 0;
            try
            {
                var param2 = new { EmployeeTypeId = Convert.ToInt32(empSalaryTypeId), EmployeeStatusId = EmployeeStatusId, OfficeLocationId = OfficeLocationId };
                var empTypeWiseCompConfig = employeeSPService.GetDataWithParameter(param2, "prl.SP_Get_EmployeeTypeWiseComponentConfiguration");
                for (int i = 0; i <= empTypeWiseCompConfig.Tables[0].Rows.Count - 1; i++)
                {
                    if (empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim() == "Basic Salary")
                    {
                        if (empTypeWiseCompConfig.Tables[0].Rows[i][4].ToString().Trim() == "R")
                        {
                            if (empTypeWiseCompConfig.Tables[0].Rows[i][6].ToString().Trim() == "G")
                            {
                                basicSalary = CalculateRatioforComponent(Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentAmount"].ToString()), grossSalary);
                                break;
                            }
                        }
                        if (empTypeWiseCompConfig.Tables[0].Rows[i][4].ToString().Trim() == "F")
                        {
                            basicSalary = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentAmount"].ToString());
                            break;
                        }
                    }
                    //break;
                }

                if (basicSalary > 0)
                {
                    dataTable = EmployeeNotInPayScale(empSalaryTypeId, basicSalary, grossSalary, EmployeeStatusId, OfficeLocationId);
                }
                return Json(new { Result = "OK", dataTable, grossSalary, Message = "OK" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                //return Json(ex.InnerException.Message.Split('!'), JsonRequestBehavior.AllowGet);
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult SalaryConfigurationSave(
            List<PRSalaryConfigurationViewModel> SalaryConfigurationList,
            int officeId,
            long employeeId,
            int newDesignationId,
            int promotionId,
            int promotionTypeId,
            int employeeTypeId,
            string pfTypeId,
            double grossSalary,
            string gradeId,
            string step,
            bool isOverTime,
            bool isOvertimeException,
            string maxOvertimePerDay,
            string maxOvertimePerMonth,
            string loginTime,
            string logoutTime,
            string lastLoginTime,
            string bankAccount,
            string bankName,
            string bankBranchName,

            string promotionDate,
            string nextReviewDate,

            string effectiveStartDate,
            string effectiveEndDate
           )
        {
            var result = "";
            bool isOperationSuccess = true;
            double totalEarnings = 0;
            bool pfApplicable = false;


            if (!(employeeId > 0))
                return Json(result = "Employee and Employee Office Detail Missing", JsonRequestBehavior.AllowGet);

            var promotionDateInDateFormat = Convert.ToDateTime(promotionDate);
            var nextReviewDateInDateFormat = Convert.ToDateTime(nextReviewDate);

            if (promotionDateInDateFormat > nextReviewDateInDateFormat)
                return Json(result = "NEXT REVIEW DATE Must Greater than PROMOTION DATE, Configuration Denied", JsonRequestBehavior.AllowGet);

            var promotionValidation = employeePromotionService.ValidatePromotion(employeeId, promotionId, promotionDateInDateFormat);

            if (!promotionValidation.IsSuccess)
                return Json(result = promotionValidation.Message, JsonRequestBehavior.AllowGet);

            //get office from employee table
            officeId = Convert.ToInt32(employeeService.GetById(Convert.ToInt32(employeeId)).OfficeId);

            var effectiveStartDateDt = Convert.ToDateTime(effectiveStartDate);
            var effectiveEndDateDt = Convert.ToDateTime(effectiveEndDate);
            var firstDayOfEndMonth = new DateTime(effectiveEndDateDt.Year, effectiveEndDateDt.Month, 1);
            var lastDayOfEndMonth = new DateTime(effectiveEndDateDt.Year, effectiveEndDateDt.Month, 1).AddMonths(1).AddDays(-1);

            if (lastDayOfEndMonth > effectiveEndDateDt)
                return Json(result = "End Date needs to be last date of month, Configuration Denied", JsonRequestBehavior.AllowGet);

            var salaryYear = effectiveStartDateDt.Year;
            int salaryMonth = effectiveStartDateDt.Month;

            //check monthly salary approved (Table: [prl].[EmployeeMonthlySalaryApproved]) for this month and year and of this employee. 
            //if not found then process will be continued
            var paramSalaryCheck = new { SalaryYear = salaryYear, SalaryMonth = salaryMonth, EmployeeId = employeeId };
            var listSalaryApproved = employeeSPService.GetDataWithParameter(paramSalaryCheck, "prl.SP_Check_MonthlySalaryApproved");

            var checkSalaryConfiguration = listSalaryApproved.Tables[0].AsEnumerable().Select(row => new EmployeeMonthlySalaryApproved()
            {
                EmployeeId = row.Field<long>("EmployeeId"),
                PRComponentId = row.Field<int>("PRComponentId"),
            }).ToList();

            if (checkSalaryConfiguration.Any())
                return Json(result = "Already salary Approved for this configuration, Configuration Denied", JsonRequestBehavior.AllowGet);

            //check salary configuration (table: [prl].[EmployeeMonthlySalary]) for this month and year and IsSendForApproval=1 or IsRejected=1 or IsApproved=1 of this employee.
            //if not found then process will be continued
            var listSalaryBeforeApproval = employeeSPService.GetDataWithParameter(paramSalaryCheck, "prl.SP_Check_MonthlySalaryBeforeApproval");
            var checkSalaryGenerated = listSalaryBeforeApproval.Tables[0].AsEnumerable().Select(row => new EmployeeMonthlySalary()
            {
                EmployeeId = row.Field<long>("EmployeeId"),
                PRComponentId = row.Field<int>("PRComponentId"),
            }).ToList();

            if (checkSalaryGenerated.Any())
                return Json(result = "Already salary Generated for this configuration, Configuration Denied", JsonRequestBehavior.AllowGet);

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    //if salary configuration found for this employee ([prl].[PRSalaryConfiguration]) 
                    //then update as isactive=0
                    var existingEmpSalary = prSalaryConfigurationService.ExisstPRSalaryConfigurationByEmployeeId(employeeId);

                    if (existingEmpSalary)
                    {
                        var paramS = new { EffectiveStartDate = effectiveStartDateDt.AddDays(-1), EmployeeId = employeeId };
                        employeeSPService.GetDataWithParameter(paramS, "prl.SP_DeleteSalaryConfigurationForSameEffectiveStartDate");
                    }

                    if (SalaryConfigurationList != null && SalaryConfigurationList.Any())
                    {
                        //get total earnings from salary detail listings
                        totalEarnings = Convert.ToDouble(SalaryConfigurationList.Sum(p => p.ComponentAmount));

                        //let's insert into [prl].[PRSalaryConfiguration]
                        InsertNewSalaryConfiguration(SalaryConfigurationList, officeId, employeeId, effectiveStartDateDt, effectiveEndDateDt);

                        //get possible provident fund related components
                        var prComponents = prComponentService.GetMany(p => p.IsProvidentFundComponent == true).ToList();

                        //check provident fund is applicable or not 
                        foreach (var item in SalaryConfigurationList)
                        {
                            if (prComponents.Where(p => p.PRComponentID == item.PRComponentID).Any())
                            {
                                pfApplicable = true;
                                break;
                            }
                        }
                    }

                    int salarygradeId = 0;
                    int salarystep = 0;

                    if (employeeTypeId == Convert.ToInt32(EmployeeTypeConfigConstants.PayScale))//Under Pay Scale
                    {
                        salarygradeId = gradeId == "" ? 0 : Convert.ToInt32(gradeId);
                        salarystep = step == "" ? 0 : Convert.ToInt32(step);
                    }

                    //let's update employee related information in employee table
                    UpdateEmployee(employeeId, newDesignationId, employeeTypeId, pfApplicable, Convert.ToInt32(pfTypeId),
                        grossSalary, Convert.ToInt32(salarygradeId), Convert.ToInt32(salarystep), totalEarnings,
                        isOverTime, isOvertimeException, maxOvertimePerDay, maxOvertimePerMonth, loginTime, logoutTime, lastLoginTime,
                        bankAccount, bankName, bankBranchName, effectiveStartDateDt, effectiveEndDateDt);

                    //let's update promotion info in promo.EmployeePromotion table if found newDesignationId  
                    //update this promotion as reviewed
                    UpdatePromotion(promotionId);

                    //let's add new employee promotion into promo.EmployeePromotion
                    //SavePromotion(employeeId, newDesignationId, promotionTypeId, promotionDate, nextReviewDate);
                    result = "OK";
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;
                    result = ex.InnerException.Message.ToString();
                }

                if (isOperationSuccess)
                    scope.Complete();

                scope.Dispose();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetHouseRent(string WorkArea, string BasicSalary)
        {
            List<PRSalaryConfigurationViewModel> List_ViewModel = new List<PRSalaryConfigurationViewModel>();
            var param = new { WorkArea = WorkArea, BasicSalary = BasicSalary };
            var empList = employeeSPService.GetDataWithParameter(param, "prl.SP_PR_Get_HouseRent");
            List_ViewModel = empList.Tables[0].AsEnumerable()
               .Select(row => new PRSalaryConfigurationViewModel
               {
                   ComponentAmount = row.Field<decimal>("HouseRent")

               }).ToList();
            if (List_ViewModel.Count() == 0)
            {
                Response.StatusCode = 403;
            }
            return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBasicSalary(string EmpId)
        {
            List<PRSalaryConfigurationViewModel> List_ViewModel = new List<PRSalaryConfigurationViewModel>();
            var param = new { EmpId = EmpId };
            var empList = employeeSPService.GetDataWithParameter(param, "SP_PR_Get_BasicSalary");
            List_ViewModel = empList.Tables[0].AsEnumerable()
               .Select(row => new PRSalaryConfigurationViewModel
               {
                   ComponentAmount = row.Field<decimal>("BasicAmount")

               }).ToList();
            if (List_ViewModel.Count() == 0)
            {
                Response.StatusCode = 403;
            }
            return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeBasicSalaryInfo(string EmpCode)
        {
            List<PRSalaryConfigurationViewModel> List_ViewModel = new List<PRSalaryConfigurationViewModel>();
            var param = new { EmpCode = EmpCode };
            var empList = employeeSPService.GetDataWithParameter(param, "prl.SP_PR_Get_EmpBasicSalaryInfo");
            List_ViewModel = empList.Tables[0].AsEnumerable()
               .Select(row => new PRSalaryConfigurationViewModel
               {
                   EmployeeID = row.Field<long>("EmployeeId"),
                   OfficeID = row.Field<int>("OfficeId"),
                   EmployeeName = row.Field<string>("EmployeeName"),
                   EmployeeTypeId = row.Field<int>("EmployeeTypeId"),
                   EmployeeStatusName = row.Field<string>("EmployeeStatusName"),
                   EmployeeStatusId = row.Field<int?>("EmployeeStatusId")
               }).ToList();
            return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListByEmployee(string TranTypeId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (TranTypeId != null)
                {
                    if (TranTypeId != "")
                        sb.Append(" AND PRSC.EmployeeID =" + TranTypeId);
                }
                List<PRSalaryConfigurationViewModel> List_ViewModel = new List<PRSalaryConfigurationViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = employeeSPService.GetDataWithParameter(param, "prl.SP_PR_Get_SalaryConfig_List");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new PRSalaryConfigurationViewModel
                {
                    PRSalaryConfigurationID = row.Field<long>("PRSalaryConfigurationID"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    PRComponentID = row.Field<int>("PRComponentID"),
                    EmployeeID = row.Field<long>("EmployeeID"),
                    ComponentName = row.Field<string>("ComponentName"),
                    ComponentAmount = row.Field<decimal>("ComponentAmount"),
                    EffectiveStartDateInstring = row.Field<string>("EffectiveStartDate"),
                    EffectiveEndDateInString = row.Field<string>("EffectiveEndDate"),
                    OfficeID = row.Field<int>("OfficeID")
                }).ToList();

                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GenerateEmployeeSalary(int empSalaryTypeId, int EmployeeStatusId, double grossSalary,
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
                        var componentType = empTypeWiseCompConfig.Tables[0].Rows[i][4].ToString().Trim();
                        var payrollConfigurationType = SessionHelper.PayrollConfigurationType;

                        if (payrollConfigurationType == PayrollConfigurationTypeConstants.Basic)
                        {
                            if (componentType != SalaryCalculationTypeConstants.Fixed)
                                continue;

                            var componentAmount = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentAmount"].ToString());
                            basicSalary = CalculateBasicRatioOrFixedforComponent(componentAmount, grossSalary);
                            break;
                        }
                        else
                        {
                            var componentAmount = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentAmount"].ToString());
                            basicSalary = CalculateRatioforComponent(componentAmount, grossSalary);
                            break;
                        }
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
                var result = 0;
            }

            return Json(prSalaryList, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetEmployeeDataOnly(string EmpId)
        //{
        //    List<PRSalaryConfigurationViewModel> List_ViewModel = new List<PRSalaryConfigurationViewModel>();
        //    var param = new { EmpId = EmpId };
        //    var empList = employeeSPService.GetDataWithParameter(param, "prl.SP_Payroll_Get_EmpData");
        //    List_ViewModel = empList.Tables[0].AsEnumerable()
        //       .Select(row => new PRSalaryConfigurationViewModel
        //       {
        //           EmployeeID = row.Field<long>("EmployeeId"),
        //           OfficeID = row.Field<int>("OfficeId"),
        //           EmployeeName = row.Field<string>("EmployeeName"),
        //           ComponentAmount = row.Field<decimal>("ComponentAmount"),
        //           PRWorkAreaID = row.Field<int>("PRWorkAreaID"),
        //           EmployeeStatusId = row.Field<int?>("EmployeeStatusId"),
        //           EmployeeStatusName = row.Field<string>("EmployeeStatusName")
        //       }).ToList();
        //    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetEmployeeData(string EmpId)
        //{
        //    List<PRSalaryConfigurationViewModel> List_ViewModel = new List<PRSalaryConfigurationViewModel>();
        //    var param = new { EmpId = EmpId };
        //    var empList = employeeSPService.GetDataWithParameter(param, "prl.SP_Payroll_ShortEmployeeInfo");
        //    List_ViewModel = empList.Tables[0].AsEnumerable()
        //       .Select(row => new PRSalaryConfigurationViewModel
        //       {
        //           EmployeeID = row.Field<long>("EmployeeId"),
        //           OfficeID = row.Field<int>("OfficeId"),
        //           EmployeeName = row.Field<string>("EmployeeName"),
        //           EmployeeStatusId = row.Field<int?>("EmployeeStatusId")

        //       }).ToList();
        //    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult AutoCompleteOrganization(string term)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    List<PRSalaryConfigurationViewModel> List_ViewModel = new List<PRSalaryConfigurationViewModel>();
        //    var param = new { AndCondition = sb.ToString() };
        //    var List = employeeSPService.GetDataWithParameter(param, "prl.SP_PR_Get_Component_List");

        //    List_ViewModel = List.Tables[0].AsEnumerable()
        //    .Select(row => new PRSalaryConfigurationViewModel
        //    {
        //        PRComponentID = row.Field<int>("PRComponentID"),
        //        ComponentName = row.Field<string>("ComponentName")

        //    }).ToList();

        //    var result = (from r in List_ViewModel
        //                  where r.ComponentName.ToLower().Contains(term.ToLower())
        //                  select new { r.ComponentName, r.PRComponentID }).Distinct();

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult Create(int OfficeID, long EmployeeID, int PRComponentID, decimal ComponentAmount, string EffectiveStartDate, string EffectiveEndDate)
        {
            string result = "OK";
            try
            {
                Int64 CreateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                DateTime CreateDate = DateTime.Now;
                //[SP_CreatePRSalaryConfig](@EmployeeID bigint,@PRComponentID int,@ComponentAmount numeric,@EffectiveStartDate datetime,@EffectiveEndDate datetime,@CreateUser bigint,@CreateDate datetime)
                var param = new { OfficeID = OfficeID, EmployeeID = EmployeeID, PRComponentID = PRComponentID, ComponentAmount = ComponentAmount, EffectiveStartDate = EffectiveStartDate, EffectiveEndDate = EffectiveEndDate, CreateUser = CreateUser, CreateDate = CreateDate };
                var val = employeeSPService.GetDataWithParameter(param, "prl.SP_PR_CreateSalaryConfig");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update(int PRSalaryConfigurationID, int OfficeID, long EmployeeID, int PRComponentID, decimal ComponentAmount, string EffectiveStartDate, string EffectiveEndDate)
        {
            string result = "OK";
            try
            {
                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                DateTime UpdateDate = DateTime.Now;
                //[SP_UpdatePRSalaryConfiguration](@PRSalaryConfigurationID bigint ,@EmployeeID bigint, @PRComponentID int,@ComponentAmount numeric, @EffectiveStartDate datetime, @EffectiveEndDate datetime,@UpdateUser bigint,@UpdateDate datetime)
                var param = new { PRSalaryConfigurationID = PRSalaryConfigurationID, OfficeID = OfficeID, EmployeeID = EmployeeID, PRComponentID = PRComponentID, ComponentAmount = ComponentAmount, EffectiveStartDate = EffectiveStartDate, EffectiveEndDate = EffectiveEndDate, UpdateUser = UpdateUser, UpdateDate = UpdateDate };
                var val = employeeSPService.GetDataWithParameter(param, "prl.SP_PR_UpdateSalaryConfiguration");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(string PRSalaryConfigurationID)
        {
            string result = "OK";
            try
            {
                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                DateTime UpdateDate = DateTime.Now;

                var param = new { PRSalaryConfigurationID = PRSalaryConfigurationID, UpdateUser = UpdateUser, UpdateDate = UpdateDate };
                var val = employeeSPService.GetDataWithParameter(param, "prl.SP_PR_DeleteSalaryConfiguration");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetList(string TranTypeId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string WorkAreaIds = Convert.ToString(TranTypeId);

                if (TranTypeId != null)
                    sb.Append(" AND PRSC.PRSalaryConfigurationID =" + WorkAreaIds);

                List<PRSalaryConfigurationViewModel> List_ViewModel = new List<PRSalaryConfigurationViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = employeeSPService.GetDataWithParameter(param, "prl.SP_PR_Get_SalaryConfig_List");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new PRSalaryConfigurationViewModel
                {
                    PRSalaryConfigurationID = row.Field<long>("PRSalaryConfigurationID"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    PRComponentID = row.Field<int>("PRComponentID"),
                    EmployeeID = row.Field<long>("EmployeeID"),
                    ComponentName = row.Field<string>("ComponentName"),
                    ComponentAmount = row.Field<decimal>("ComponentAmount"),
                    EffectiveStartDateInstring = row.Field<string>("EffectiveStartDate"),
                    EffectiveEndDateInString = row.Field<string>("EffectiveEndDate"),
                    OfficeID = row.Field<int>("OfficeID")
                }).ToList();

                if (TranTypeId != null)
                {
                    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                }

                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        #endregion

        #region SalaryGrade

        public JsonResult SaveEmployeeSalaryGrade(EmployeeGradeList obj)
        {
            var result = 0;
            var message = "";
            var MaxGradeNo = 0;
            var NewGradeNo = 0;

            try
            {
                var checkDuplicate = employeeGradeListService.GetMany(p => p.IsActive == true && p.GradeName == obj.GradeName).ToList();

                if (checkDuplicate.Any())
                {
                    message = "Grade Name already exists";
                    return Json(new { result = 0, message = message }, JsonRequestBehavior.AllowGet);
                }

                var checkMaxGradeId = employeeGradeListService.GetMany(p => p.IsActive == true);
                if (checkMaxGradeId.Any())
                {
                    MaxGradeNo = checkMaxGradeId.Max(p => p.GradeId);
                    NewGradeNo = MaxGradeNo + 1;
                }
                else
                {
                    NewGradeNo = 1;
                }

                if (obj.EffectiveDateFrom > obj.EffectiveDateTo)
                {
                    message = "Effective From Date must less than Effective To Date";
                    return Json(new { result = 0, message = message }, JsonRequestBehavior.AllowGet);
                }

                var model = new EmployeeGradeList();
                model.GradeId = NewGradeNo;
                model.GradeName = obj.GradeName;
                model.GradeDescription = obj.GradeDescription;
                model.InitialAmount = obj.InitialAmount;
                model.AmountPerIncrement = obj.AmountPerIncrement;
                model.EffectiveDateFrom = obj.EffectiveDateFrom;
                model.EffectiveDateTo = obj.EffectiveDateTo;
                model.RatioOn = obj.RatioOn;
                model.Percentage = GradeRatioOnConstants.Percentage == obj.RatioOn ? obj.Percentage : 0;

                model.IsActive = true;
                model.CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;

                //let's create EmployeeGradeList
                employeeGradeListService.Create(model);

                message = "Saved successfully";
                return Json(new { result = 1, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeGradeList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                var gradeList = employeeGradeListService.GetMany(p => p.IsActive == true).ToList();
                var view_GradeList = gradeList.AsEnumerable().Select(p => new EmployeeGradeListViewModel()
                {
                    Id = p.Id,
                    GradeId = p.GradeId,
                    GradeName = p.GradeName,
                    GradeDescription = p.GradeDescription,
                    InitialAmount = p.InitialAmount,
                    AmountPerIncrement = p.AmountPerIncrement,
                    RatioOn = p.RatioOn,
                    Percentage = p.Percentage,
                    EffectiveDateFrom = Convert.ToDateTime(p.EffectiveDateFrom).ToString("dd-MMM-yyyy"),
                    EffectiveDateTo = Convert.ToDateTime(p.EffectiveDateTo).ToString("dd-MMM-yyyy")
                }).ToList();

                var currentPageRecords = view_GradeList.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = view_GradeList.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult UpdateEmployeeSalaryGrade(EmployeeGradeList obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeGradeListService.GetById(obj.Id);
                model.GradeName = obj.GradeName;
                model.GradeDescription = obj.GradeDescription;
                model.InitialAmount = obj.InitialAmount;
                model.AmountPerIncrement = obj.AmountPerIncrement;
                model.RatioOn = obj.RatioOn;
                model.Percentage = GradeRatioOnConstants.Percentage == obj.RatioOn ? obj.Percentage : 0;

                if (obj.EffectiveDateFrom < obj.EffectiveDateTo)
                {
                    model.EffectiveDateFrom = obj.EffectiveDateFrom;
                    model.EffectiveDateTo = obj.EffectiveDateTo;
                    model.IsActive = true;
                    model.CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.UtcNow;
                    employeeGradeListService.Update(model);
                    result = 1;
                    message = "Updated successfully";
                }
                else
                {
                    message = "Effective Date From must greater than Effective Date To";
                }

            }
            catch (Exception)
            {
                result = 0;
                message = "Update denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSalaryGrade(int Id)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = employeeGradeListService.GetById(Id);
                model.IsActive = false;
                model.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
                model.UpdateDate = DateTime.UtcNow;
                employeeGradeListService.Update(model);
                result = 1;
                message = "Deleted successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Methods

        public void MapDropDown(PRSalaryConfigurationViewModel model)
        {
            var pleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            model.DesignationList = CommonDynamicDropDown.GetAllPayrollDesignationList();
            model.EmployeeSalaryType = commonStaticDropDown.SalaryStructuredTypeList();
            model.SalaryGenerationTypeList = commonStaticDropDown.SalaryGenerationTypeList();
            model.GradeList = CommonDynamicDropDown.GetEmployeeGradeList();
            model.SalaryScaleList = commonStaticDropDown.NumberSerialDropDown(0, 60);
            model.OverTimeList = commonStaticDropDown.YesNoDropDown_bool();
            model.PFTypeList = CommonDynamicDropDown.ProvidentFundType();
            model.MonthList = commonStaticDropDown.MonthList();
            model.BankList = CommonDynamicDropDown.PayrollBankNameWithCode();
            model.PromotionTypeList = CommonDynamicDropDown.PromotionTypeList();

            var yearList = new List<SelectListItem>();
            yearList.Add(pleaseSelect);

            for (int i = 0; i < 2; i++)
                yearList.Add(new SelectListItem() { Text = (Convert.ToInt32(DateTime.Now.Year) + i).ToString(), Value = (Convert.ToInt32(DateTime.Now.Year) + i).ToString() });

            model.IncrementYearFromList = yearList;

            var employeeStatusList = CommonDynamicDropDown.ddlEmployeeStatusList();
            employeeStatusList.RemoveAll(x => x.Value == "");
            model.EmployeeStatusList = employeeStatusList;
        }

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

        private void UpdateEmployee(
                long employeeId,
                int newDesignationId,
                int employeeTypeId,
                bool PFApplicable,
                int pfTypeId,

                double grossSalary,
                int gradeId,
                int step,
                double totalEarnings,

                bool isOverTime,
                bool isOvertimeException,
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

            if (SessionHelper.PayrollConfigurationType == PayrollConfigurationTypeConstants.Basic)
            {
                model.BasicSalary = Convert.ToDecimal(grossSalary);
                model.GrossSalary = 0;
            }
            else
            {
                model.BasicSalary = 0;
                model.GrossSalary = Convert.ToDecimal(grossSalary);
            }

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
            model.LoginTime = Convert.ToDateTime(loginTime);
            model.LogoutTime = Convert.ToDateTime(logoutTime);
            model.LastLoginTime = Convert.ToDateTime(lastLoginTime);
            model.EffectiveStartDate = effectiveStartDate;
            model.EffectiveEndDate = effectiveEndDate;
            model.IsOverTime = isOverTime;
            model.IsOvertimeException = isOvertimeException;
            model.MaxOvertimePerDay = maxOvertimePerDay == string.Empty ? 0 : Convert.ToDecimal(maxOvertimePerDay);
            model.MaxOvertimePerMonth = maxOvertimePerMonth == string.Empty ? 0 : Convert.ToDecimal(maxOvertimePerMonth);
            model.UpdateUser = SessionHelper.LoggedInEmployeeID;
            model.UpdateDate = DateTime.Now;
            model.BankName = bankName == null ? "" : bankName;
            model.BankBranchName = bankBranchName == null ? "" : bankBranchName;
            model.PFTypeId = pfTypeId;

            if (newDesignationId > 0)
                model.DesignationId = newDesignationId;

            employeeService.Update(model);
        }


        public void UpdatePromotion(int promotionId)
        {
            var promotion = employeePromotionService.GetById(promotionId);
            if (promotion != null)
            {
                promotion.IsReviewed = true;
                promotion.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                promotion.UpdateDate = DateTime.UtcNow;
                employeePromotionService.Update(promotion);

            }
        }

        public void SavePromotion(long EmployeeId, int OfficeDesignationId, int promotionTypeId, string PromotionDate, string NextReviewDate)
        {
            var employeePromotion = new EmployeePromotion
            {
                EmployeeId = EmployeeId,
                DesignationId = OfficeDesignationId,
                PromotionDate = Convert.ToDateTime(PromotionDate),
                NextReviewDate = Convert.ToDateTime(NextReviewDate),
                PromotionTypeId = promotionTypeId,
                IsReviewed = false,
                IsActive = true,
                CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID),
                UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID),
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

           var promotionInfo =  employeePromotionService.Create(employeePromotion);

            var param = new
            {
                @promotionId = promotionInfo.PromotionId,
                @EmployeeId = EmployeeId 
            };
            var List = employeeSPService.GetDataWithParameter(param, "promo.SP_CreateSalaryConfiguration");





        }

        private double CalcualteGross(double initialAmt, double amtPerIncrement, double scale)
        {
            double grossSalary = 0;
            if (scale == 0)
            {
                grossSalary = initialAmt;
            }
            else
            {
                grossSalary = initialAmt + (amtPerIncrement * scale);
            }
            return grossSalary;
        }

        private double CalculateBasicRatioOrFixedforComponent(double ratio, double amount)
        {
            var payrollConfigurationType = SessionHelper.PayrollConfigurationType;
            if (payrollConfigurationType == PayrollConfigurationTypeConstants.Basic)
                return amount;

            return amount != 0 ? (ratio * amount) / 100 : 0;
        }

        private double CalculateRatioforComponent(double ratio, double amount)
        {
            return amount != 0 ? (ratio * amount) / 100 : 0;
        }

        //private void GenerateEmployeeSalaryMasterHistory(long employeeId, double grossSalary, int employeeTypeId, string bankAccount, bool isOverTime, string effectiveStartDate, string effectiveEndDate, double overTimeHour, double totalEarnings, double OvertimeRate, int pfTypeId)
        //{
        //    var entityList = employeeSalaryConfigurationHistoryService.GetMany(p => p.EmployeeId == employeeId && p.IsActive == true).ToList();
        //    foreach (var item in entityList)
        //    {
        //        item.IsActive = false;
        //        employeeSalaryConfigurationHistoryService.Update(item);
        //    }

        //    var entity = new EmployeeSalaryConfigurationHistory();
        //    entity.EmployeeId = employeeId;
        //    entity.GrossSalary = Convert.ToDouble(grossSalary);
        //    entity.TotalSalary = Convert.ToDouble(totalEarnings);
        //    entity.PFTypeId = pfTypeId;
        //    //entity.EffectiveDateFrom = effectiveStartDate == "" ? "": Convert.ToDateTime(effectiveStartDate);
        //    entity.EffectiveDateFrom = Convert.ToDateTime(effectiveStartDate);
        //    entity.EffectiveDateTo = Convert.ToDateTime(effectiveEndDate);
        //    entity.IsOvertime = isOverTime;
        //    entity.OvertimeHour = Convert.ToDecimal(overTimeHour);
        //    entity.OvertimeRate = Convert.ToDecimal(OvertimeRate);
        //    entity.IsActive = true;
        //    entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
        //    entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
        //    entity.CreateDate = DateTime.UtcNow;
        //    entity.UpdateDate = DateTime.UtcNow;
        //    employeeSalaryConfigurationHistoryService.Create(entity);
        //}


        private List<PRSalaryScaleViewModel> EmployeeInPayScale(string grade, string scale, string empSalaryTypeId, double basicSalary, double gross, int EmployeeStatusId, int OfficeLocationId, int pfTypeId)
        {
            var param2 = new { EmployeeTypeId = Convert.ToInt32(empSalaryTypeId), EmployeeStatusId = EmployeeStatusId, OfficeLocationId = OfficeLocationId, PFTypeId = pfTypeId };
            var empTypeWiseCompConfig = employeeSPService.GetDataWithParameter(param2, "prl.SP_Get_EmployeeTypeWiseComponentConfiguration");
            empTypeWiseCompConfig.Tables[0].Columns.Add(new DataColumn("CalculatedAmount", typeof(System.Double)));

            List<PRSalaryScaleViewModel> dataList = new List<PRSalaryScaleViewModel>();

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
                var salaryRoundType = empTypeWiseCompConfig.Tables[0].Rows[i]["SalaryRoundType"].ToString();

                if (componentType == SalaryCalculationTypeConstants.Ratio
                    && ratioBasedOn == RatioBasedOnConstants.Gross)
                {
                    var ratio = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), gross);
                    if (salaryRoundType == "RoundUp")
                    {
                        ratio = Math.Round(ratio);
                    }
                    if (salaryRoundType == "RoundDown")
                    {
                        ratio = Math.Ceiling(ratio);
                    }
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

                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratio;
                }
                else if (componentType == SalaryCalculationTypeConstants.Ratio
                   && ratioBasedOn == RatioBasedOnConstants.Basic)
                {
                    var ratio = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), basicSalary);

                    if (salaryRoundType == "RoundUp")
                    {
                        ratio = Math.Round(ratio);
                    }
                    if (salaryRoundType == "RoundDown")
                    {
                        ratio = Math.Ceiling(ratio);
                    }

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

                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratio;
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
                    TransactionTypeView = empTypeWiseCompConfig.Tables[0].Rows[i].Field<string>("TransactionTypeView")
                });
            }

            return dataList;
        }

        private List<View_EmployeeSalaryConfiguration> GenerateDataList(string employeeCode)
        {
            var empList = new List<View_EmployeeSalaryConfiguration>();

            var param = new { EmployeeCode = Convert.ToString(employeeCode) };
            var employeeData = employeeSPService.GetDataWithParameter(param, "prl.SP_GetPayroll_EmployeebyEmployeeCode");

            var empdataList = employeeData.Tables[0].AsEnumerable()
            .Select(row => new EmployeeViewModel
            {
                OfficeId = row.Field<int>("OfficeId"),
                EmployeeId = row.Field<long>("EmployeeId"),
                EmployeeCode = row.Field<string>("EmployeeCode"),
                EmployeeName = row.Field<string>("EmployeeName"),
                EmployeeNameBng = row.Field<string>("EmployeeNameBng"),
                EmployeeTypeId = row.Field<int?>("EmployeeTypeId"),
                EmployeeStatusId = Convert.ToInt32(row.Field<int?>("EmployeeStatusId")),
                EmployeeStatusName = row.Field<string>("EmployeeStatusName"),
                EmployeeStatusValue = row.Field<string>("EmployeeStatusValue"),
                IsSalaryApplicable = row.Field<bool?>("IsSalaryApplicable"),
                DepartmentName = row.Field<string>("DepartmentName"),
                DesignationName = row.Field<string>("OffcDesignName"),
                FirstJoiningDateMsg = row.Field<string>("FirstJoiningDate"),
                ConfirmationDateMsg = row.Field<string>("ConfirmationDate"),
                BankAccountNo = row.Field<string>("BankAccountNo"),
                OfficeLocationId = row.Field<int>("OfficeLocationId"),
                PFTypeId = row.Field<int>("PFTypeId")
            }).ToList();

            foreach (var item in empdataList)
            {
                var data = new View_EmployeeSalaryConfiguration();
                data.OfficeID = Convert.ToInt32(item.OfficeId);
                data.EmployeeID = item.EmployeeId;
                data.PRComponentId = 0;
                data.EmployeeTypeName = "";
                data.ComponentGroupName = "";
                data.ComponentName = "";
                data.IsActive = true;
                data.CalculatedAmount = 0;
                data.ComponentType = "";
                data.RatioBasedOn = "";
                data.EmployeeTypeId = item.EmployeeTypeId == null ? 0 : item.EmployeeTypeId.Value;
                data.EffectiveStartDate = DateTime.Now.ToString("dd-MMM-yyyy");//DateTime.Today.ToString();
                var dateAdv = DateTime.Now.AddYears(3);
                data.EffectiveEndDate = dateAdv.ToString("dd-MMM-yyyy");
                data.GrossSalary = 0;
                data.BasicSalary = 0;
                data.BankAccountNo = "";
                data.Step = 0;
                data.GradeId = 0;
                data.LogInTime = "10:00:00";
                data.LogOutTime = "18:00:00";
                data.LastLoginTime = "10:00:00";
                data.IsOverTime = false;
                //data.OvertimeHour = 0;
                //data.IncrementMonth = 0;
                data.EmployeeCode = item.EmployeeCode;
                data.EmployeeName = item.EmployeeName;
                data.EmployeeNameBng = item.EmployeeNameBng;
                data.EmployeeStatusId = item.EmployeeStatusId;
                data.EmployeeStatusName = item.EmployeeStatusName;
                data.OfficeLocationId = item.OfficeLocationId;
                // data.EmployeeStatusName = ReturnEmployeeStatusReverse(item.EmployeeStatus.Trim());
                //data.DepartmentName = item.DepartmentName;
                //data.DesignationName = item.DesignationName;

                empList.Add(data);
            }
            return empList;
        }


        private List<PRSalaryScaleViewModel> EmployeeNotInPayScale(string empSalaryTypeId, double basicSalary, double grossSalary, int EmployeeStatusId, int OfficeLocationId)
        {
            var param2 = new { EmployeeTypeId = Convert.ToInt32(empSalaryTypeId), EmployeeStatus = EmployeeStatusId, OfficeLocationId = OfficeLocationId };
            var empTypeWiseCompConfig = employeeSPService.GetDataWithParameter(param2, "prl.SP_Get_EmployeeTypeWiseComponentConfiguration");
            empTypeWiseCompConfig.Tables[0].Columns.Add(new DataColumn("CalculatedAmount", typeof(System.Double)));

            for (int i = 0; i <= empTypeWiseCompConfig.Tables[0].Rows.Count - 1; i++)
            {
                var componentName = empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentName"].ToString();
                var componentType = empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentType"].ToString();
                var ratioPercent = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentAmount"].ToString());
                var ratioBasedOn = empTypeWiseCompConfig.Tables[0].Rows[i]["RatioBasedOn"].ToString();

                if (componentType == "R" && ratioBasedOn == "G")
                {
                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), grossSalary);

                }
                else if (componentType == "R" && ratioBasedOn == "B")
                {
                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), basicSalary);
                }
                else if (componentType == "F")
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
                TransactionTypeView = row.Field<string>("TransactionTypeView"),
            }).ToList();

            return dataList;
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

            List<PRSalaryScaleViewModel> dataList = new List<PRSalaryScaleViewModel>();

            for (int i = 0; i <= empTypeWiseCompConfig.Tables[0].Rows.Count - 1; i++)
            {
                var componentType = empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentType"].ToString();
                var componentName = empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentName"].ToString();

                var payrollConfigurationType = SessionHelper.PayrollConfigurationType;

                if (payrollConfigurationType == PayrollConfigurationTypeConstants.Basic
                    && componentName == "Basic Salary")
                {
                    if (componentType != SalaryCalculationTypeConstants.Fixed)
                        continue;
                }

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
                    TransactionTypeView = empTypeWiseCompConfig.Tables[0].Rows[i].Field<string>("TransactionTypeView")
                });
            }

            return dataList;
        }

        #endregion

        #region GeneratePayrollFromOldSalaryTable


        //public ActionResult SalaryDeduction()
        //{
        //    int employeeId = 771;
        //    int componentId = 58;
        //    int deductionDays = 6;
        //    int salaryMonth = 2;
        //    int salaryYear = 2017;

        //    var emplyoee = employeeService.GetByEmpId(employeeId);
        //    //employeeService.GetAll().Where(p => p.EmployeeId == employeeId).FirstOrDefault();
        //    if (emplyoee != null)
        //    {
        //        var grossSalary = emplyoee.GrossSalary;
        //        var noOfDaysinMonth = DateTime.DaysInMonth(salaryYear, salaryMonth);
        //        var calculatedAmount = (grossSalary / noOfDaysinMonth) * deductionDays;

        //        var entity = new EmployeeSalaryDeduction();
        //        entity.EmployeeId = employeeId;
        //        entity.ComponentId = componentId;
        //        entity.DeductedAmount = Convert.ToDecimal(calculatedAmount);
        //        entity.DeductionDays = deductionDays;
        //        // entity.SalaryMonth = salaryMonth;
        //        // entity.SalaryYear = salaryYear;
        //        entity.IsActive = true;
        //        entity.CreateDate = DateTime.UtcNow;
        //        entity.UpdateDate = DateTime.UtcNow;
        //        entity.CreatedBy = Convert.ToInt32((SessionHelper.LoggedInEmployeeID));
        //        entity.UpdatedBy = Convert.ToInt32((SessionHelper.LoggedInEmployeeID));
        //    }
        //    return View();
        //}

        //private void MakeIsActiveFalse(List<PRSalaryConfiguration> empSalary)
        //{
        //    foreach (var item in empSalary)
        //    {
        //        //var entity = new PRSalaryConfiguration();
        //        item.IsActive = false;
        //        item.UpdateUser = SessionHelper.LoggedInEmployeeID;
        //        item.UpdateDate = DateTime.Now;
        //        prSalaryConfigurationService.Update(item);
        //    }
        //}



        //private List<PRSalaryScaleViewModel> GetConfigurationList(long employeeId)
        //{
        //    var param = new { EmployeeID = employeeId };
        //    var empTypeWiseCompConfig = employeeSPService.GetDataWithParameter(param, "prl.SP_Get_GetExistingSalaryConfigurationByEmployeeId");
        //    List<PRSalaryScaleViewModel> dataList = new List<PRSalaryScaleViewModel>();

        //    dataList = empTypeWiseCompConfig.Tables[0].AsEnumerable()
        //    .Select(row => new PRSalaryScaleViewModel
        //    {
        //        PRComponentId = row.Field<int>("PRComponentId"),
        //        EmployeeTypeName = row.Field<string>("EmployeeTypeName"),
        //        ComponentGroupName = row.Field<string>("ComponentGroupName"),
        //        ComponentName = row.Field<string>("ComponentName"),
        //        ComponentType = row.Field<string>("ComponentType"),
        //        ComponentAmount = row.Field<decimal>("ComponentAmount"),
        //        RatioBasedOn = row.Field<string>("RatioBasedOn"),
        //        EmployeeTypeId = row.Field<int>("EmployeeTypeId"),
        //        //CalculatedAmount = row.Field<double>("ComponentAmount"),//component amount is the component amt here
        //        EffectiveStartDateMsg = row.Field<string>("EffectiveStartDate"),
        //        EffectiveEndDateMsg = row.Field<string>("EffectiveEndDate")
        //    }).ToList();
        //    return dataList;
        //}


        //private List<SalaryMonthly> GetOldPayrollData()
        //{
        //    var lstOldPayrollData = new List<SalaryMonthly>();
        //    var connetionString = "Data Source=192.192.190.29;Initial Catalog=paperless;User ID=sa;Password=Software@2012";
        //    using (SqlConnection con = new SqlConnection(connetionString))
        //    {
        //        var sqlCommadString = @"select EmpID, Gross,BankAC, Status  from [dbo].[SalaryMonthly] order by EmpID";

        //        con.Open();

        //        using (SqlCommand cmd = new SqlCommand(sqlCommadString, con))
        //        {
        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var entity = new SalaryMonthly();
        //                        entity.EmpID = Convert.ToString(reader["EmpID"]);
        //                        entity.Gross = Convert.ToDouble(reader["Gross"]);
        //                        entity.BankAC = Convert.ToString(reader["BankAC"]);
        //                        entity.Status = Convert.ToString(reader["Status"]);
        //                        lstOldPayrollData.Add(entity);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return lstOldPayrollData;
        //}


        //public ActionResult GenerateSalaryConfigurationFromOldGCDataBasePayroll()
        //{
        //    GenerateSalaryConfigurationFromOldGCDataBase();
        //    return View();
        //}


        //public List<PRSalaryScaleViewModel> GenerateEmployeeSalaryFromOldPayrollTable(int empSalaryTypeId, int EmployeeStatusId, double grossSalary,int OfficeLocationId)
        //{
        //    List<PRSalaryScaleViewModel> prSalaryList = new List<PRSalaryScaleViewModel>();
        //    double basicSalary = 0;
        //    try
        //    {
        //        var empStatus = EmployeeStatusId;
        //        var param2 = new { EmployeeTypeId = Convert.ToInt32(empSalaryTypeId), EmployeeStatusId = EmployeeStatusId , OfficeLocationId = OfficeLocationId };
        //        var empTypeWiseCompConfig = employeeSPService.GetDataWithParameter(param2, "SP_Get_EmployeeTypeWiseComponentConfiguration");
        //        for (int i = 0; i <= empTypeWiseCompConfig.Tables[0].Rows.Count - 1; i++)
        //        {
        //            if (empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim() == "Basic Salary")
        //            {
        //                basicSalary = CalculateRatioforComponent(Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentAmount"].ToString()), grossSalary);
        //                break;
        //            }
        //        }

        //        if (basicSalary > 0)
        //        {
        //            prSalaryList = DistributeEmployeeSalaryInComponentsOldDataPayroll(empSalaryTypeId, basicSalary, grossSalary, EmployeeStatusId, OfficeLocationId);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        var result = 0;

        //    }
        //    return prSalaryList;
        //}


        //private void GenerateSalaryConfigurationFromOldGCDataBase()
        //{
        //    var employeeList = employeeService.GetAll().Where(p => p.IsActive == true).ToList();

        //    var lstOldPayrollData = GetOldPayrollData();


        //    foreach (var item in lstOldPayrollData)
        //    {
        //        var checkExistanceEmployee = employeeList.Where(p => p.EmployeeCode == item.EmpID).FirstOrDefault();
        //        if (checkExistanceEmployee != null)
        //        {
        //            var grossSalary = Convert.ToDouble(item.Gross);
        //            var bankAccount = item.BankAC;
        //            var empSalaryTypeId = checkExistanceEmployee.EmployeeTypeId == null ? 1 : Convert.ToInt32(checkExistanceEmployee.EmployeeTypeId);
        //            var employeeStatus = checkExistanceEmployee.EmployeeStatusId;
        //            var employeeId = checkExistanceEmployee.EmployeeId;
        //            var effectiveStartDate = DateTime.Now.AddMonths(-1).ToString();
        //            var effectiveEndDate = DateTime.Now.AddYears(5).ToString();
        //            var officeId = Convert.ToInt32(checkExistanceEmployee.OfficeId == null ? 0 : checkExistanceEmployee.OfficeId);
        //            var OfficeLoactionId = officeService.Get(b => b.OfficeId == officeId).OfficeLocationId;
        //            var gradeId = 0;
        //            var step = 0;
        //            var loginTime = "1900-01-01 10:00:00.000";
        //            var logoutTime = "1900-01-01 18:00:00.000";
        //            var lastLoginTime = "1900-01-01 10:00:00.000";
        //            var isOverTime = false;
        //            var overTimeHour = "0";
        //            var incrementMonth = "";
        //            var OvertimeRate = "0";

        //            var generatedSalaryList = GenerateEmployeeSalaryFromOldPayrollTable(empSalaryTypeId, employeeStatus, grossSalary, OfficeLoactionId.Value);

        //            SalaryConfigurationSaveFromOldData(generatedSalaryList, officeId, employeeId
        //  , grossSalary, effectiveStartDate, effectiveEndDate, empSalaryTypeId, bankAccount, gradeId, step
        //  , loginTime, logoutTime, lastLoginTime, isOverTime, overTimeHour, incrementMonth, OvertimeRate);
        //        }

        //    }
        //}


        //private void SalaryConfigurationSaveFromOldData(List<PRSalaryScaleViewModel> SalaryConfigurationList, int officeId, long employeeId
        //  , double grossSalary, string effectiveStartDate, string effectiveEndDate, int employeeTypeId, string bankAccount, int gradeId, int step
        //  , string loginTime, string logoutTime, string lastLoginTime, bool isOverTime, string overTimeHour, string incrementMonth, string OvertimeRate)
        //{

        //    var effectiveStartDateDt = Convert.ToDateTime(effectiveStartDate);
        //    var effectiveEndDateDt = Convert.ToDateTime(effectiveEndDate);

        //    var firstDayOfEndMonth = new DateTime(effectiveEndDateDt.Year, effectiveEndDateDt.Month, 1);
        //    var lastDayOfEndMonth = new DateTime(effectiveEndDateDt.Year, effectiveEndDateDt.Month, 1).AddMonths(1).AddDays(-1);

        //    var flagOk = 1;

        //    var empSalary = prSalaryConfigurationService.GetByEmployeeId(employeeId);



        //    var salaryYear = effectiveStartDateDt.Year;
        //    int salaryMonth = effectiveStartDateDt.Month;

        //    var checkSalaryConfiguration = employeeMonthlySalaryApprovedService.GetAll().Where(p => p.SalaryYear >= salaryYear && p.SalaryMonth >= salaryMonth && p.EmployeeId == employeeId).ToList();
        //    if (checkSalaryConfiguration.Any())
        //    {
        //        flagOk = 0;
        //    }

        //    var checkSalaryGenerated = employeeMonthlySalaryService.GetAll().Where(p => p.SalaryYear >= salaryYear && p.SalaryMonth >= salaryMonth && p.EmployeeId == employeeId && (p.IsSendForApproval == true || p.IsRejected == true || p.IsApproved == true)).ToList();
        //    if (checkSalaryConfiguration.Any())
        //    {
        //        flagOk = 0;
        //    }

        //    if (flagOk == 1)
        //    {
        //        var param = new { EffectiveStartDate = effectiveStartDateDt, EffectiveEndDate = effectiveEndDateDt, EmployeeId = employeeId };

        //        var list = employeeSPService.GetDataWithParameter(param, "SP_GET_PREmployeeSalaryCurrentConfiguration_ByEmployee");

        //        var checkExistenceSalaryConfiguration = list.Tables[0].AsEnumerable().Select(row => new PRSalaryConfigurationViewModel()
        //        {
        //            PRSalaryConfigurationID = row.Field<long>("PRSalaryConfigurationID"),
        //            OfficeID = row.Field<int>("OfficeID"),
        //            EmployeeID = row.Field<long>("EmployeeID"),
        //            PRComponentID = row.Field<int>("PRComponentID"),
        //            ComponentAmount = row.Field<decimal>("ComponentAmount"),
        //            EffectiveStartDate = row.Field<DateTime>("EffectiveStartDate"),
        //            EffectiveEndDate = row.Field<DateTime>("EffectiveEndDate"),
        //            IsActive = row.Field<bool>("IsActive"),
        //            ComponentCategory = row.Field<string>("ComponentCategory"),
        //            TransactionType = row.Field<string>("TransactionType")

        //        }).ToList();

        //        if (checkExistenceSalaryConfiguration.Any())
        //        {
        //            var paramS = new { EffectiveStartDate = effectiveStartDateDt, EmployeeId = employeeId };
        //            var val = employeeSPService.GetDataWithParameter(paramS, "SP_DeleteSalaryConfigurationForSameEffectiveStartDate");
        //        }

        //        if (OvertimeRate == null || OvertimeRate == "")
        //        {
        //            OvertimeRate = "0";
        //        }
        //        if (overTimeHour == null || overTimeHour == "")
        //        {
        //            overTimeHour = "0";
        //        }

        //        double totalEarnings = Convert.ToDouble(SalaryConfigurationList.Sum(p => p.CalculatedAmount));
        //        InsertNewSalaryConfigurationOldPayrollData(SalaryConfigurationList, officeId, employeeId, effectiveStartDate, effectiveEndDate);
        //        var salarygradeId = gradeId == null ? 0 : Convert.ToInt32(gradeId);
        //        var salarystep = step == null ? 0 : Convert.ToInt32(step);
        //        //UpdateEmployeeFromOldDatabase(employeeId, grossSalary, employeeTypeId, bankAccount, Convert.ToInt32(salarygradeId), Convert.ToInt32(salarystep), loginTime, logoutTime, lastLoginTime, isOverTime, effectiveStartDate, effectiveEndDate, overTimeHour, 0, totalEarnings, OvertimeRate);
        //        //GenerateEmployeeSalaryMasterHistory(Convert.ToInt64(employeeId), Convert.ToDouble(grossSalary), employeeTypeId, bankAccount, isOverTime, effectiveStartDate, effectiveEndDate, Convert.ToDouble(overTimeHour), Convert.ToInt32(incrementMonth), Convert.ToDouble(totalEarnings), Convert.ToDouble(OvertimeRate));
        //    }

        //}


        //private void InsertNewSalaryConfigurationOldPayrollData(List<PRSalaryScaleViewModel> salaryConfigurationList, int officeId, long employeeId, string effectiveStartDate, string effectiveEndDate)
        //{
        //    var crObj = new List<PRSalaryConfiguration>();
        //    foreach (var item in salaryConfigurationList)//prSalaryConfigurationService
        //    {
        //        var entity = new PRSalaryConfiguration();
        //        // entity.OfficeID = officeId;
        //        entity.EmployeeID = employeeId;
        //        entity.PRComponentID = item.PRComponentId;
        //        entity.ComponentAmount = Convert.ToDecimal(item.CalculatedAmount);
        //        entity.EffectiveStartDate = Convert.ToDateTime(effectiveStartDate);
        //        entity.EffectiveEndDate = Convert.ToDateTime(effectiveEndDate);
        //        entity.IsActive = true;
        //        entity.ComponentCategory = item.ComponentCategory;
        //        entity.TransactionType = item.TransactionType;
        //        entity.CreateUser = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
        //        entity.CreateDate = DateTime.UtcNow;
        //        entity.UpdateDate = DateTime.UtcNow;
        //        entity.UpdateUser = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
        //        prSalaryConfigurationService.Create(entity);
        //    }
        //}


        //private void UpdateEmployeeFromOldDatabase(long employeeId, double grossSalary, int employeeTypeId, string bankAccount
        //    , int gradeId, int step, string loginTime, string logoutTime, string lastLoginTime, bool isOverTime, string effectiveStartDate, string effectiveEndDate, string overtimeHour, int incrementMonth, double totalEarnings, string overTimeRate)
        //{
        //    var model = employeeService.GetByEmpId(employeeId);
        //    model.GrossSalary = Convert.ToDecimal(grossSalary);
        //    model.TotalEarnings = Convert.ToDecimal(totalEarnings);
        //    model.EmployeeTypeId = employeeTypeId;
        //    model.BankAccountNo = bankAccount;
        //    model.GradeId = gradeId;
        //    model.Step = step;
        //    //model.LoginTime = Convert.ToDateTime(loginTime);
        //    //model.LogoutTime = Convert.ToDateTime(logoutTime);
        //    //model.LastLoginTime = Convert.ToDateTime(lastLoginTime);
        //    model.EffectiveStartDate = Convert.ToDateTime(effectiveStartDate);
        //    model.EffectiveEndDate = Convert.ToDateTime(effectiveEndDate);
        //    model.IsOverTime = isOverTime;
        //    model.OvertimeHour = Convert.ToDecimal(overtimeHour);
        //    model.UpdateUser = SessionHelper.LoggedInEmployeeID;
        //    model.UpdateDate = DateTime.Now;
        //    model.IncrementMonth = incrementMonth;
        //    model.OvertimeRate = Convert.ToDecimal(overTimeRate);
        //    employeeService.Update(model);
        //}

        //private List<PRSalaryScaleViewModel> DistributeEmployeeSalaryInComponentsOldDataPayroll(int empSalaryTypeId, double basicSalary, double gross, int EmployeeStatusId,int OfficeLocationId)
        //{

        //    var param2 = new { EmployeeTypeId = empSalaryTypeId, EmployeeStatusId = EmployeeStatusId, OfficeLocationId= OfficeLocationId };
        //    var empTypeWiseCompConfig = employeeSPService.GetDataWithParameter(param2, "SP_Get_EmployeeTypeWiseComponentConfiguration");
        //    empTypeWiseCompConfig.Tables[0].Columns.Add(new DataColumn("CalculatedAmount", typeof(System.Double)));
        //    for (int i = 0; i <= empTypeWiseCompConfig.Tables[0].Rows.Count - 1; i++)
        //    {
        //        var componentName = empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentName"].ToString();
        //        var componentType = empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentType"].ToString();
        //        var ratioPercent = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentAmount"].ToString());
        //        var ratioBasedOn = empTypeWiseCompConfig.Tables[0].Rows[i]["RatioBasedOn"].ToString();

        //        if (componentType == "R" && ratioBasedOn == "G")
        //        {
        //            var ratio = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), gross);
        //            var maxLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MaximumLimit"].ToString());
        //            var minLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MinimumLimit"].ToString());
        //            if (ratio < minLimit && minLimit != 0)
        //            {
        //                ratio = minLimit;
        //            }
        //            if (ratio > maxLimit && maxLimit != 0)
        //            {
        //                ratio = maxLimit;
        //            }

        //            empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratio;


        //        }
        //        else if (componentType == "R" && ratioBasedOn == "B")
        //        {
        //            var ratio = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), basicSalary);
        //            var maxLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MaximumLimit"].ToString());
        //            var minLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MinimumLimit"].ToString());
        //            if (ratio < minLimit && minLimit != 0)
        //            {
        //                ratio = minLimit;
        //            }
        //            if (ratio > maxLimit && maxLimit != 0)
        //            {
        //                ratio = maxLimit;
        //            }

        //            empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratio;
        //        }
        //        else if (componentType == "F")
        //        {
        //            empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratioPercent;//for fixed ratioPercentage is the fixed amount
        //        }
        //    }
        //    List<PRSalaryScaleViewModel> dataList = new List<PRSalaryScaleViewModel>();

        //    dataList = empTypeWiseCompConfig.Tables[0].AsEnumerable()
        //    .Select(row => new PRSalaryScaleViewModel
        //    {
        //        PRComponentId = row.Field<int>("PRComponentId"),
        //        EmployeeTypeName = row.Field<string>("EmployeeTypeName"),
        //        ComponentGroupName = row.Field<string>("ComponentGroupName"),
        //        ComponentName = row.Field<string>("ComponentName"),
        //        ComponentType = row.Field<string>("ComponentType"),
        //        ComponentAmount = row.Field<decimal>("ComponentAmount"),
        //        RatioBasedOn = row.Field<string>("RatioBasedOn"),
        //        EmployeeTypeId = row.Field<int>("EmployeeTypeId"),
        //        CalculatedAmount = row.Field<double>("CalculatedAmount"),
        //        ComponentCategory = row.Field<string>("ComponentCategory"),
        //        TransactionType = row.Field<string>("TransactionType"),
        //        EmployeeStatusId = row.Field<int?>("EmployeeStatusId"),
        //        TransactionTypeView = row.Field<string>("TransactionTypeView")
        //    }).ToList();

        //    return dataList;
        //}

        #endregion
    }
}