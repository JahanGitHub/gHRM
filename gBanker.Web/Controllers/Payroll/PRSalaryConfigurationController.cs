
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
    public class PRSalaryConfigurationController : BaseController
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
        private readonly IGradeXSalaryStepService gradeXSalaryStepService;
        private readonly IEmployeeAllowenceService employeeAllowenceService;

        private readonly IOfficeService officeService;
        private CommonStaticDropDown commonStaticDropDown;
        private CommonDynamicDropDown CommonDynamicDropDown;

        private readonly IEmployeePromotionService _EmployeePromotionService;
        private readonly IPromotionConfiguredSalaryService _PromotionConfiguredSalaryService;

        public PRSalaryConfigurationController(
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
            IOfficeService officeService,
            IEmployeePromotionService _EmployeePromotionService,
            IPromotionConfiguredSalaryService _PromotionConfiguredSalaryService,
            IGradeXSalaryStepService gradeXSalaryStepService,
            IEmployeeAllowenceService employeeAllowenceService
            )
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

            this._EmployeePromotionService = _EmployeePromotionService;
            this._PromotionConfiguredSalaryService = _PromotionConfiguredSalaryService;
            this.gradeXSalaryStepService = gradeXSalaryStepService;
            this.employeeAllowenceService = employeeAllowenceService;
            commonStaticDropDown = new CommonStaticDropDown();
            CommonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion

        #region ActionResult
        public ActionResult Index()
        {
            var model = new PRSalaryConfigurationViewModel();
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;
            MapDropDown(model);
            return View(model);
        }

        public ActionResult Index2()
        {
            var model = new PRSalaryConfigurationViewModel2();
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;
            MapDropDown2(model);
            return View(model);
        }

        //public ActionResult Index3()
        //{
        //    var model = new PRSalaryConfigurationViewModel();
        //    IEnumerable<SelectListItem> items = new SelectList(" ");
        //    ViewData["ComponentList"] = items;
        //    MapDropDown(model);
        //    return View(model);
        //}

        public ActionResult Index3(string employeeCode, int? promotionId)
        {
            var model = new PRSalaryConfigurationViewModel3();
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;
            MapDropDown3(model);

            if (!string.IsNullOrEmpty(employeeCode))
            {
                // Fetch employee data
                var employeeData = GetEmpInfoByCodeEmp(employeeCode);
                if (employeeData != null)
                {
                    model.EmployeeCode = employeeCode;
                    model.PromotionId = promotionId ?? 0;
                    model.EmployeeID = employeeData.EmployeeId;
                    model.EmployeeName = employeeData.EmployeeName;
                    model.GrossSalary = employeeData.GrossSalary;
                    model.IsEmployeeCodeDisabled = true; // Flag to disable input
                                                         // Populate other properties as needed
                }
            }

            return View(model);
        }


        // Changed return type from JsonResult to dynamic or create a specific return type
        public dynamic GetEmpInfoByCodeEmp(string employee_code)
        {
            var result = 0;
            try
            {
                var param = new { EmployeeCode = employee_code };
                var empList = employeeSPService.GetDataWithParameter(param, "cmm.SP_GetEmployeeInfo_ByEmployeeCode");

                var List_EmployeeViewModel = empList.Tables[0].AsEnumerable()
                    .Select(row => new EmployeePromotionViewModel3
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
                return new { result = result, data = List_EmployeeViewModel };
            }
            catch (Exception ex)
            {
                return new { result = result, data = (List<EmployeePromotionViewModel>)null };
            }
        }
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
                var confirmationDate = Convert.ToDateTime(employeeInfo.ConfirmationDate).ToString("dd-MMM-yyyy");

                var departmentName = employeeDepartmentService.GetById(Convert.ToInt32(employeeInfo.DepartmentId)).DepartmentName;
                var designationName = employeeDesignationService.GetById(Convert.ToInt32(employeeInfo.DesignationId)).DesignationName;

                //get employee promotion from [promo].[EmployeePromotion]
                var promotionInfo = employeePromotionService.GetPromotionInfo(employeeInfo.EmployeeId);

                var promotionDate = string.Empty;
                var nextReviewDate = string.Empty;
                if (promotionInfo != null)
                {
                    promotionDate = Convert.ToDateTime(promotionInfo.PromotionDate).ToString("dd-MMM-yyyy");
                    nextReviewDate = Convert.ToDateTime(promotionInfo.NextReviewDate).ToString("dd-MMM-yyyy");
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


        public JsonResult GetRoutingNobyEmployeeCode(string employeeCode)
        {
            try
            {
                var withResignEmployee = false;

                var param = new { EmployeeCode = employeeCode };
                //get employee information
                var employeeInfo = employeeSPService.GetDataWithParameter(param, "SP_GET_ROUTINGNO");  

                var RoutingNo = employeeInfo.Tables[0].Rows[0]["RoutingNo"].ToString();

                return Json(new
                {
                    Result = RoutingNo
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GenerateSalaryForEmployeeInPayScale_designation(string empSalaryTypeId, string grade, string scale, int EmployeeStatusId, int OfficeLocationId, string providentFundTypeId, int EmployeeID)
        {
            List<PRSalaryScaleViewModel> dataTable = new List<PRSalaryScaleViewModel>();
            double basicSalary = 0;
            try
            {
                ////get grade by grade id from [EmployeeGradeList]
                //var salaryGrade = employeeSPService.GetDataWithParameter(param, "prl.SP_Get_EmployeeGradeByGradeId");
                int gradeid = Convert.ToInt32(grade);
                double scaleInStep = Convert.ToDouble(scale);
                double grossSalary = 0;
                var obj_grade = employeeGradeListService.GetMany(x => x.GradeId == gradeid);
                if (obj_grade.Any())
                {
                    double initialAmt = (double)obj_grade.First().InitialAmount;
                    grossSalary = initialAmt;

                    if (scaleInStep > 0)
                    {
                        var stepList = gradeXSalaryStepService
                            .GetMany(x => x.GradeId == gradeid && x.StepFrom <= scaleInStep)
                            .OrderBy(o => o.StepFrom)
                            .ThenBy(o => o.StepTo)
                            .ToList();

                        double increment = 0;

                        foreach (var step in stepList)
                        {
                            for (int i = step.StepFrom; i <= step.StepTo; i++)
                            {
                                if (step.RatioOn == "Percentage")
                                {
                                    increment += (step.AmountOrPercent / 100.0) * grossSalary;
                                }
                                else if (step.RatioOn == "Fixed")
                                {
                                    increment += step.AmountOrPercent;
                                }

                                if (i == scaleInStep)
                                {
                                    break;
                                }
                            }
                        }

                        grossSalary += increment;
                    }
                }


                //calculate Gross about

                var db = new gHRMDBContext();
                var DesignationId = db.Employees.Where(z => z.EmployeeId == EmployeeID).Select(x => x.DesignationId).FirstOrDefault();

                var param2 = new
                {
                    EmployeeTypeId = Convert.ToInt32(empSalaryTypeId),
                    EmployeeStatusId = EmployeeStatusId,
                    OfficeLocationId = OfficeLocationId,
                    PFTypeId = Convert.ToInt32(providentFundTypeId),
                    DesignationId = Convert.ToInt32(DesignationId)
                };

                //get payroll components from [prl].[PRComponent]
                var empTypeWiseCompConfig = employeeSPService.GetDataWithParameter(param2, "prl.SP_Get_EmployeeTypeWiseComponentConfiguration_designation");

                for (int i = 0; i <= empTypeWiseCompConfig.Tables[0].Rows.Count - 1; i++)
                {
                    var tt = empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim();

                    if (empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim() == "Basic Salary" || empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim() == "Basic")
                    {
                        var ratioBaseOn = empTypeWiseCompConfig.Tables[0].Rows[i][6].ToString().Trim();
                        var payrollConfigurationType = SessionHelper.PayrollConfigurationType;

                        if (payrollConfigurationType == PayrollConfigurationTypeConstants.Basic)
                        {
                            //if (ratioBaseOn != SalaryRatioConstants.NotRequired)
                            //    continue;

                             if (!ratioBaseOn.Equals(SalaryRatioConstants.NotRequired))
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
                    dataTable = EmployeeInPayScale_designation(grade, scale, empSalaryTypeId, basicSalary, grossSalary, EmployeeStatusId, OfficeLocationId, Convert.ToInt32(providentFundTypeId), Convert.ToInt32(DesignationId));
                return Json(new { Result = "OK", dataTable, grossSalary, Message = "OK" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var result = 0;
                return Json(new { Result = result, Message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GenerateSalaryForEmployeeInPayScale_Prottashi(string empSalaryTypeId, string grade, string scale, int EmployeeStatusId, int OfficeLocationId, string providentFundTypeId)
        {
            List<PRSalaryScaleViewModel> dataTable = new List<PRSalaryScaleViewModel>();
            double basicSalary = 0;
            try
            {
                ////get grade by grade id from [EmployeeGradeList]
                //var salaryGrade = employeeSPService.GetDataWithParameter(param, "prl.SP_Get_EmployeeGradeByGradeId");
                int gradeid = Convert.ToInt32(grade);
                double scaleInStep = Convert.ToDouble(scale);
                double grossSalary = 0;
                float initial = 100; 
                var obj_grade = employeeGradeListService.GetMany(x => x.GradeId == gradeid);
                if (obj_grade.Any())
                {
                    double initialAmt = (double)obj_grade.First().InitialAmount;
                    

                    if (scaleInStep > 0)
                    {
                        var stepList = gradeXSalaryStepService
                            .GetMany(x => x.GradeId == gradeid && x.StepFrom <= scaleInStep)
                            .OrderBy(o => o.StepFrom)
                            .ThenBy(o => o.StepTo)
                            .ToList();

                        double increment = 0;

                        int time = 0;

                        foreach (var step in stepList)
                        {
                            if (step.RatioOn == "Percentage")
                            {
                                // Principal amount
                                double principal = initialAmt;

                                // Annual interest rate (e.g., 5% -> 0.05)
                                double rate = 0.05;

                                // Number of times interest is compounded per year
                                int timesCompounded = 1;

                                // Time in years

                                if ((int)scaleInStep == 1)
                                    grossSalary = initialAmt;
                                else
                                {
                                    // Calculate compound interest
                                    time = (int)scaleInStep - 1; 
                                    grossSalary = principal * Math.Pow((1 + rate / timesCompounded), timesCompounded * time);
                                    double compoundInterest = grossSalary - principal;
                                }


                            }      
                           
                        }

                        grossSalary = Math.Round(grossSalary, 0) ;
                    }
                }


                //calculate Gross about

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
                    var tt = empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim();

                    if (empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim() == "Basic Salary" || empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim() == "Basic")
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
                    dataTable = EmployeeInPayScale(grade, scale, empSalaryTypeId, basicSalary, grossSalary, EmployeeStatusId, OfficeLocationId, Convert.ToInt32(providentFundTypeId));
                return Json(new { Result = "OK", dataTable, grossSalary, Message = "OK" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var result = 0;
                return Json(new { Result = result, Message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GenerateSalaryForEmployeeInPayScale(string empSalaryTypeId, string grade, string scale, int EmployeeStatusId, int OfficeLocationId, string providentFundTypeId)
        {
            if(SessionHelper.CompanyInfo.CompanyShortName == "GMPF" || SessionHelper.CompanyInfo.CompanyShortName == "GSSB" || SessionHelper.CompanyInfo.CompanyShortName == "GT" ||
                SessionHelper.CompanyInfo.CompanyShortName == "GTT" || SessionHelper.CompanyInfo.CompanyShortName == "GUP")
            {
                return GenerateSalaryForEmployeeInPayScale2( empSalaryTypeId,  grade,  scale,  EmployeeStatusId,  OfficeLocationId,  providentFundTypeId);
            }
            else
            {
            List<PRSalaryScaleViewModel> dataTable = new List<PRSalaryScaleViewModel>();
            double basicSalary = 0;
            try
            {
                ////get grade by grade id from [EmployeeGradeList]
                //var salaryGrade = employeeSPService.GetDataWithParameter(param, "prl.SP_Get_EmployeeGradeByGradeId");
                int gradeid = Convert.ToInt32(grade);
                double scaleInStep = Convert.ToDouble(scale);
                double grossSalary = 0;
                var obj_grade = employeeGradeListService.GetMany(x => x.GradeId == gradeid);
                if (obj_grade.Any())
                {
                    double initialAmt = (double)obj_grade.First().InitialAmount;
                    grossSalary = initialAmt;

                    if (scaleInStep > 0)
                    {
                        var stepList = gradeXSalaryStepService
                            .GetMany(x => x.GradeId == gradeid && x.StepFrom <= scaleInStep)
                            .OrderBy(o => o.StepFrom)
                            .ThenBy(o => o.StepTo)
                            .ToList();

                        double currentSalary = grossSalary;

                        //if (SessionHelper.CompanyInfo.CompanyShortName == "GUP")
                        //{
                        //    double increment = 0;

                        //    foreach (var step in stepList)
                        //    {
                        //        for (int i = step.StepFrom; i <= step.StepTo; i++) //  step.StepFrom
                        //        {
                        //            if (step.RatioOn == "Percentage")
                        //            {
                        //                increment += (step.AmountOrPercent / 100.0) * grossSalary;
                        //            }
                        //            else if (step.RatioOn == "Fixed")
                        //            {
                        //                //if(scaleInStep == 1)
                        //                //{
                        //                //    increment += step.AmountOrPercent;
                        //                //    break;
                        //                //}
                        //                //else
                        //                //{
                        //                //    increment += step.AmountOrPercent;
                        //                //}
                        //                //
                        //                increment += step.AmountOrPercent;
                        //            }

                        //            if (i == scaleInStep)
                        //            {
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}
                        //else
                        //{
                            //double increment = 0;

                            foreach (var step in stepList)
                            {
                                for (int i = step.StepFrom + 1; i <= step.StepTo; i++) //  step.StepFrom
                                {
                                    //if (step.RatioOn == "Percentage")
                                    //{
                                    //    increment += (step.AmountOrPercent / 100.0) * grossSalary;
                                    //}

                                    if (i > scaleInStep)
                                        break;

                                    if (step.RatioOn == "Percentage")
                                    {
                                        double increment = (step.AmountOrPercent / 100.0) * currentSalary;
                                        currentSalary += increment;
                                    }
                                    else if (step.RatioOn == "Fixed")
                                    {
                                        //if(scaleInStep == 1)
                                        //{
                                        //    increment += step.AmountOrPercent;
                                        //    break;
                                        //}
                                        //else
                                        //{
                                        //    increment += step.AmountOrPercent;
                                        //}
                                        //
                                        currentSalary += step.AmountOrPercent;
                                    }

                                    if (i == scaleInStep)
                                    {
                                        break;
                                    }
                                }
                            }
                        //}

                        grossSalary = currentSalary;
                    }
                }


                //calculate Gross about

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
                    var tt = empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim();

                    if (empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim() == "Basic Salary" || empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim() == "Basic")
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
                    dataTable = EmployeeInPayScale(grade, scale, empSalaryTypeId, basicSalary, grossSalary, EmployeeStatusId, OfficeLocationId, Convert.ToInt32(providentFundTypeId));
                return Json(new { Result = "OK", dataTable, grossSalary, Message = "OK" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var result = 0;
                return Json(new { Result = result, Message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

            }
        }


        public JsonResult GenerateSalaryForEmployeeInPayScale2(string empSalaryTypeId, string grade, string scale, int EmployeeStatusId, int OfficeLocationId, string providentFundTypeId)
        {

                List<PRSalaryScaleViewModel> dataTable = new List<PRSalaryScaleViewModel>();
                double basicSalary = 0;
                try
                {
                    ////get grade by grade id from [EmployeeGradeList]
                    //var salaryGrade = employeeSPService.GetDataWithParameter(param, "prl.SP_Get_EmployeeGradeByGradeId");
                    int gradeid = Convert.ToInt32(grade);
                    double scaleInStep = Convert.ToDouble(scale);
                    double grossSalary = 0;


                //calculate Gross about

                var param = new { grade = gradeid, scale = scaleInStep };

                var result = employeeSPService.GetDataWithParameter(param, "prl.SP_Calculate_GrossSalary");

                grossSalary = Convert.ToDouble( result.Tables[0].Rows[0]["GrossSalary"].ToString() );
                //calculate Gross about

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
                        var tt = empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim();

                        if (empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim() == "Basic Salary" || empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim() == "Basic")
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
                        dataTable = EmployeeInPayScale(grade, scale, empSalaryTypeId, basicSalary, grossSalary, EmployeeStatusId, OfficeLocationId, Convert.ToInt32(providentFundTypeId));
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


        public JsonResult GenerateSalaryForEmployeeInPayScale_Old(string empSalaryTypeId, string grade, string scale, int EmployeeStatusId, int OfficeLocationId, string providentFundTypeId)
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
                    if (empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim() == "Basic Salary" || empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim() == "Basic")
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
            string FractionStep,
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
                        var paramS = new { EffectiveStartDate = effectiveStartDateDt, EmployeeId = employeeId };
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
                        grossSalary, Convert.ToInt32(salarygradeId), Convert.ToInt32(salarystep)
                        , Convert.ToDecimal((string.IsNullOrEmpty(FractionStep) ? "0" : FractionStep))
                        , totalEarnings, isOverTime, isOvertimeException, maxOvertimePerDay, maxOvertimePerMonth, loginTime, logoutTime
                        , lastLoginTime, bankAccount, bankName, bankBranchName, effectiveStartDateDt, effectiveEndDateDt);

                    //let's update promotion info in promo.EmployeePromotion table if found newDesignationId  
                    //TODO: this feature need to review                   
                    //if (newDesignationId > 0
                    //    && !string.IsNullOrWhiteSpace( promotionDate) &&
                    //    !string.IsNullOrWhiteSpace(nextReviewDate) 
                    //    )
                    //{
                    //    //update this promotion as reviewed
                    //    UpdatePromotion(promotionId);

                    //    //let's add new employee promotion into promo.EmployeePromotion
                    //    SavePromotion(employeeId, newDesignationId, promotionDate, nextReviewDate);
                    //}



                    // Call arrear calculation SP with proper error handling
                    try
                    {

                        var employee = employeeService.GetByEmpId(Convert.ToInt64(employeeId));

                        var arrearParams = new
                        {
                            EmployeeId = employeeId,
                            EmpCode = employee.EmployeeCode,
                            EmpName = employee.EmployeeName,
                            Designation = newDesignationId, // You need to implement this
                            PreviousSalary = 0,
                            NewSalary = grossSalary,
                            EffectDate = effectiveStartDateDt,
                            OrderDate = DateTime.Now.Date,
                            Type = newDesignationId > 0 ? "PROMOTION" : "INCREMENT",
                            LastDear = 0, // You may need to calculate this
                            CreatedBy = SessionHelper.LoggedInEmployeeID // Adjust according to your session management
                        };

                        // Check if SP exists before calling
                        if (CheckStoredProcedureExists("prl.USP_SaveIncrementPromotionWithArrear"))
                        {
                            var arrearResult = employeeSPService.GetDataWithParameter(arrearParams, "prl.USP_SaveIncrementPromotionWithArrear");
                            // You can process the result if needed
                        }
                        else
                        {
                            // Log that SP doesn't exist but continue with other operations
                            System.Diagnostics.Debug.WriteLine("SP prl.USP_SaveIncrementPromotionWithArrear does not exist");
                        }
                    }
                    catch (Exception spEx)
                    {
                        // Log SP error but don't break the main transaction
                        System.Diagnostics.Debug.WriteLine($"SP Error: {spEx.Message}");
                        // Continue with other operations
                    }

                    if (SessionHelper.CompanyInfo.CompanyShortName == "GC")
                    {
                        UpdateForServiceBookReport(employeeId, newDesignationId, employeeTypeId, pfApplicable, Convert.ToInt32(pfTypeId),
                    grossSalary, Convert.ToInt32(salarygradeId), Convert.ToInt32(salarystep)
                    , Convert.ToDecimal((string.IsNullOrEmpty(FractionStep) ? "0" : FractionStep))
                    , totalEarnings, isOverTime, isOvertimeException, maxOvertimePerDay, maxOvertimePerMonth, loginTime, logoutTime
                    , lastLoginTime, bankAccount, bankName, bankBranchName, effectiveStartDateDt, effectiveEndDateDt);

                    }



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



        // Helper method to check if stored procedure exists
        private bool CheckStoredProcedureExists(string spName)
        {
            try
            {
                var checkParams = new { ProcedureName = spName };
                var result = employeeSPService.GetDataWithParameter(checkParams, "dbo.USP_CheckStoredProcedureExists");
                return result.Tables[0].Rows.Count > 0;
            }
            catch
            {
                return false;
            }
        }

    

        [HttpPost]
        public JsonResult SalaryConfigurationSave2(
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
       string FractionStep,
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
       string effectiveEndDate,
       string RoutingNo
      )
        {
            var result = "";
            bool isOperationSuccess = true;
            double totalEarnings = 0;
            bool pfApplicable = false;

            if (!(employeeId > 0))
                return Json(result = "Employee and Employee Office Detail Missing", JsonRequestBehavior.AllowGet);

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
                        var paramS = new { EffectiveStartDate = effectiveStartDateDt, EmployeeId = employeeId };
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
                        grossSalary, Convert.ToInt32(salarygradeId), Convert.ToInt32(salarystep)
                        , Convert.ToDecimal((string.IsNullOrEmpty(FractionStep) ? "0" : FractionStep))
                        , totalEarnings, isOverTime, isOvertimeException, maxOvertimePerDay, maxOvertimePerMonth, loginTime, logoutTime
                        , lastLoginTime, bankAccount, bankName, bankBranchName, effectiveStartDateDt, effectiveEndDateDt);

                    //let's update promotion info in promo.EmployeePromotion table if found newDesignationId  
                    //TODO: this feature need to review                   
                    //if (newDesignationId > 0
                    //    && !string.IsNullOrWhiteSpace( promotionDate) &&
                    //    !string.IsNullOrWhiteSpace(nextReviewDate) 
                    //    )
                    //{
                    //    //update this promotion as reviewed
                    //    UpdatePromotion(promotionId);

                    //    //let's add new employee promotion into promo.EmployeePromotion
                    //    SavePromotion(employeeId, newDesignationId, promotionDate, nextReviewDate);
                    //}

                    if (SessionHelper.CompanyInfo.CompanyShortName == "GC")
                    {
                        UpdateForServiceBookReport(employeeId, newDesignationId, employeeTypeId, pfApplicable, Convert.ToInt32(pfTypeId),
                    grossSalary, Convert.ToInt32(salarygradeId), Convert.ToInt32(salarystep)
                    , Convert.ToDecimal((string.IsNullOrEmpty(FractionStep) ? "0" : FractionStep))
                    , totalEarnings, isOverTime, isOvertimeException, maxOvertimePerDay, maxOvertimePerMonth, loginTime, logoutTime
                    , lastLoginTime, bankAccount, bankName, bankBranchName, effectiveStartDateDt, effectiveEndDateDt);

                    }


                    if (SessionHelper.CompanyInfo.CompanyShortName == "Prottyashi")
                    {
                        var param = new { EmployeeId = employeeId, RoutingNo = RoutingNo };
                        var routinginsert = employeeSPService.GetDataWithParameter(param, "SP_UPDATE_ROUTING_NO");
                    }


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

        #region Mahfuz Modify this method
        /*
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

        */
        public JsonResult GenerateEmployeeSalary(int empSalaryTypeId, int EmployeeStatusId, double grossSalary,
            string salaryGenerationType, int OfficeLocationId, string pfTypeId, int? empid)
        {
            List<PRSalaryScaleViewModel> prSalaryList = new List<PRSalaryScaleViewModel>();
            try
            {
                if ((empid ?? 0) > 0)
                {
                    string rawQuery = $@"select sc.PRComponentId	,EmployeeTypeName,ComponentGroupName,ComponentName,ComponentType,sc.ComponentAmount,RatioBasedOn,EmployeeTypeId
                    ,MaximumLimit,MinimumLimit,sc.ComponentCategory,sc.TransactionType,EmployeeStatusId,TransactionTypeView,OfficeLocationId,IsSalaryImpactProhibited,SalaryRoundType,ComponentPayrollId
                    from prl.PRSalaryConfiguration sc INNER JOIN prl.View_EmployeeTypeWiseComponentConfiguration vc on sc.PRComponentID = vc.PRComponentId
                    where sc.IsActive = 1 and EmployeeID = {empid} and cast(GETDATE() as date) between sc.EffectiveStartDate and sc.EffectiveEndDate";
                    prSalaryList = new gHRMDBContext().Database.SqlQuery<PRSalaryScaleViewModel>(rawQuery).ToList();
                    if (prSalaryList.Any())
                        prSalaryList.ForEach(x => x.CalculatedAmount =(double) x.ComponentAmount);
                }

                if (!prSalaryList.Any())
                {
                    if (salaryGenerationType == EmploymentTypeConstants.PayScale)
                        empSalaryTypeId = 1;
                    if (salaryGenerationType == EmploymentTypeConstants.NonPayScale)
                        empSalaryTypeId = 2;


                    double basicSalary = 0;
                    var param2 = new
                    {
                        EmployeeTypeId = Convert.ToInt32(empSalaryTypeId),
                        EmployeeStatusId = EmployeeStatusId,
                        OfficeLocationId = OfficeLocationId,
                        PFTypeId = Convert.ToInt32(pfTypeId)
                    };
                    var empTypeWiseCompConfig = employeeSPService.GetDataWithParameter(param2, "prl.SP_Get_EmployeeTypeWiseComponentConfiguration");
                    for (int i = 0; i <= empTypeWiseCompConfig.Tables[0].Rows.Count - 1; i++)
                    {
                        if (empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim() == "Basic Salary")
                        {
                            var componentType = empTypeWiseCompConfig.Tables[0].Rows[i][4].ToString().Trim();
                            var payrollConfigurationType = SessionHelper.PayrollConfigurationType;

                            if (payrollConfigurationType == PayrollConfigurationTypeConstants.Basic)
                            {
                                if (SessionHelper.CompanyInfo.CompanyShortName == "GUP" || SessionHelper.CompanyInfo.CompanyShortName == "Masuk")
                                {
                                    // for gup
                                }
                                else
                                {
                                    if (componentType != SalaryCalculationTypeConstants.Fixed)
                                        continue;
                                }

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
                    }

                    if (basicSalary > 0)
                        prSalaryList = DistributeEmployeeSalaryInComponents(empSalaryTypeId, basicSalary, grossSalary,
                            EmployeeStatusId, OfficeLocationId, Convert.ToInt32(pfTypeId));
                    else
                    {
                        if(SessionHelper.CompanyInfo.CompanyShortName == "Prottyashi" || SessionHelper.CompanyInfo.CompanyShortName == "GUP")
                        {
                            prSalaryList = DistributeEmployeeSalaryInComponents_prottashi(empSalaryTypeId, basicSalary, grossSalary,
    EmployeeStatusId, OfficeLocationId, Convert.ToInt32(pfTypeId));
                        }
                    }


                }
            }

            catch (Exception ex)
            {
                var result = 0;
            }

            return Json(prSalaryList, JsonRequestBehavior.AllowGet);
        }

        #endregion Mahfuz Modify this method



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
                    message = "Effective Date From must greater than Effective Date To";
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

        #region    Salary Step
        public ActionResult EmployeeSalaryStep()
        {
            PRSalaryConfigurationViewModel model = new PRSalaryConfigurationViewModel();
            model.GradeList = CommonDynamicDropDown.GetEmployeeGradeList();
            return View(model);
        }
        public JsonResult GetEmployeeSalaryStepList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                var lst = gradeXSalaryStepService.GetGradeXSalaryStepList();
                var currentPageRecords = lst.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = lst.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult SaveEmployeeSalaryStep(GradeXSalaryStep obj)
        {
            string msg = ""; int result = 0;
            try
            {
                if (obj.Id > 0)
                {
                    if (!gradeXSalaryStepService.GetMany(x => x.Id != obj.Id && x.GradeId == obj.GradeId &&
                    ((obj.StepFrom >= x.StepFrom && obj.StepFrom <= x.StepTo) || (obj.StepTo >= x.StepFrom && obj.StepTo <= x.StepTo))
                    ).Any())
                    {
                        obj.UpdatedBy = (int)SessionHelper.LoggedInEmployeeID;
                        obj.UpdateDate = DateTime.Now;
                        gradeXSalaryStepService.Update(obj);
                        result = 1;
                        msg = "Successfully Update";
                    }
                    else msg = "duplicate step is found";
                }
                else
                {
                    if (!gradeXSalaryStepService.GetMany(x => x.GradeId == obj.GradeId &&
                    ((obj.StepFrom >= x.StepFrom && obj.StepFrom <= x.StepTo) || (obj.StepTo >= x.StepFrom && obj.StepTo <= x.StepTo))).Any())
                    {
                        obj.CreatedBy = (int)SessionHelper.LoggedInEmployeeID;
                        obj.CreateDate = DateTime.Now;
                        gradeXSalaryStepService.Create(obj);
                        result = 1;
                        msg = "Successfully Save";
                    }
                    else msg = "duplicate step is found";
                }
            }
            catch (Exception EX)
            {
                msg = EX.Message;
            }

            return Json(new { result = result, message = msg });
        }
        public JsonResult DeleteEmployeeSalaryStep(int id)
        {
            var obj = gradeXSalaryStepService.GetById(id);
            obj.UpdatedBy = (int)SessionHelper.LoggedInEmployeeID;
            obj.UpdateDate = DateTime.Now;
            obj.IsActive = false;
            gradeXSalaryStepService.Update(obj);
            return Json(new { result = 1, message = "Data is inactive" });
        }

        [HttpGet]
        public JsonResult GetSalaryStepXGrade(int gradeid, int? step)
        {
            var lst = CommonDynamicDropDown.GetSalaryStepXGrade(gradeid, step);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        #endregion Salary Step

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
            {
                yearList.Add(new SelectListItem() { Text = (Convert.ToInt32(DateTime.Now.Year) + i).ToString(), Value = (Convert.ToInt32(DateTime.Now.Year) + i).ToString() });
            }
            model.IncrementYearFromList = yearList;

            var employeeStatusList = CommonDynamicDropDown.ddlEmployeeStatusList();
            employeeStatusList.RemoveAll(x => x.Value == "");
            model.EmployeeStatusList = employeeStatusList;


        }

        public void MapDropDown3(PRSalaryConfigurationViewModel3 model)
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
            {
                yearList.Add(new SelectListItem() { Text = (Convert.ToInt32(DateTime.Now.Year) + i).ToString(), Value = (Convert.ToInt32(DateTime.Now.Year) + i).ToString() });
            }
            model.IncrementYearFromList = yearList;

            var employeeStatusList = CommonDynamicDropDown.ddlEmployeeStatusList();
            employeeStatusList.RemoveAll(x => x.Value == "");
            model.EmployeeStatusList = employeeStatusList;


        }

        public void MapDropDown2(PRSalaryConfigurationViewModel2 model)
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
            {
                yearList.Add(new SelectListItem() { Text = (Convert.ToInt32(DateTime.Now.Year) + i).ToString(), Value = (Convert.ToInt32(DateTime.Now.Year) + i).ToString() });
            }
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
                //entity.IncrementMonth = incrementMonth;
                //entity.IncrementYear = incrementYear;
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
                decimal FractionStep,
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
                if(SessionHelper.CompanyInfo.CompanyShortName == "Prottyashi" )
                {
                    model.GrossSalary = Convert.ToDecimal(grossSalary);
                }   
                else
                {
                    model.GrossSalary = 0;
                }
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
            model.FractionStep = FractionStep;
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



        private void UpdateForServiceBookReport(
        long employeeId,
        int newDesignationId,
        int employeeTypeId,
        bool PFApplicable,
        int pfTypeId,

        double grossSalary,
        int gradeId,
        int step,
        decimal FractionStep,
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

            var joiningDate = model.FirstJoiningDate.Date;
            var promotionDate = new DateTime(effectiveStartDate.Year, effectiveStartDate.Month, joiningDate.Day);

            gHRMDBContext db = new gHRMDBContext();
            var ifany = db.EmployeePromotion
                .Where(z => z.EmployeeId == employeeId && z.PromotionDate == promotionDate)
                .FirstOrDefault(); // Use FirstOrDefault to check if any record matches

            if (ifany == null)
            {
                EmployeePromotion EPromotion = new EmployeePromotion();
                EPromotion.EmployeeId = employeeId;
                EPromotion.DesignationId = (int)model.DesignationId;
                EPromotion.PromotionTypeId = 1; //  PromotionTypeId;
                EPromotion.PromotionDate = promotionDate;
                EPromotion.NextReviewDate = effectiveEndDate; //  PromotionDate.Value.AddMonths(DurationMonth.Value);
                EPromotion.IsReviewed = true;  //  == IsReviewed;
                EPromotion.IsActive = true;
                EPromotion.CreateUser = SessionHelper.LoggedInEmployeeID;  //_Controller.CreateUserId;
                EPromotion.CreateDate = DateTime.Now;
                _EmployeePromotionService.Create(EPromotion);

                PromotionConfiguredSalary PCSalary = new PromotionConfiguredSalary();
                PCSalary.PromotionId = EPromotion.PromotionId;
                PCSalary.EmployeeId = employeeId;
                PCSalary.GrossSalary = Convert.ToDecimal(model.GrossSalary ?? 0);
                PCSalary.BasicSalary = (PCSalary.GrossSalary.Value * 55) / 100;
                PCSalary.HouseRent = (PCSalary.GrossSalary.Value * 30) / 100;
                PCSalary.Medical = (PCSalary.GrossSalary.Value * 10) / 100;
                PCSalary.Conveyance = (PCSalary.GrossSalary.Value * 5) / 100;
                PCSalary.Others = 0;
                PCSalary.IsActive = true;
                PCSalary.CreateUser = SessionHelper.LoginUserEmployeeId;
                PCSalary.CreateDate = DateTime.Now;
                _PromotionConfiguredSalaryService.Create(PCSalary);
            }

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

        public void SavePromotion(long EmployeeId, int OfficeDesignationId, string PromotionDate, string NextReviewDate)
        {
            var employeePromotion = new EmployeePromotion
            {
                EmployeeId = EmployeeId,
                DesignationId = OfficeDesignationId,
                PromotionDate = Convert.ToDateTime(PromotionDate),
                NextReviewDate = Convert.ToDateTime(NextReviewDate),
                IsReviewed = false,
                IsActive = true,
                CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID),
                UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID),
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            employeePromotionService.Create(employeePromotion);
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

            //if(SessionHelper.CompanyInfo.CompanyShortName == "GUP")
            //    return amount != 0 ? (ratio * amount) / 100 : 0;

            if (payrollConfigurationType == PayrollConfigurationTypeConstants.Basic)
                return amount;

            return amount != 0 ? (ratio * amount) / 100 : 0;
        }

        private double CalculateRatioforComponent(double ratio, double amount)
        {


            if (ratio == 0)
                return amount;

            double amt = 0;
            amt = amount != 0 ? (ratio * amount) / 100 : 0;

            if (SessionHelper.CompanyInfo.CompanyShortName == "GTT")
                amt = Math.Round(amt, MidpointRounding.AwayFromZero);

            return amt;
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



        private List<PRSalaryScaleViewModel> EmployeeInPayScale_designation(string grade, string scale, string empSalaryTypeId, double basicSalary, double gross, int EmployeeStatusId, int OfficeLocationId, int pfTypeId, int DesignationId )
        {
            var param2 = new { EmployeeTypeId = Convert.ToInt32(empSalaryTypeId), EmployeeStatusId = EmployeeStatusId, OfficeLocationId = OfficeLocationId, PFTypeId = pfTypeId , DesignationId = DesignationId };
            var empTypeWiseCompConfig = employeeSPService.GetDataWithParameter(param2, "prl.SP_Get_EmployeeTypeWiseComponentConfiguration_designation");
            empTypeWiseCompConfig.Tables[0].Columns.Add(new DataColumn("CalculatedAmount", typeof(System.Double)));

            List<PRSalaryScaleViewModel> dataList = new List<PRSalaryScaleViewModel>();
            int gradeid = int.Parse(grade);
            var empAllowencelst = employeeAllowenceService.GetMany(x => x.GradeId == gradeid && x.EmployeeStatusId == EmployeeStatusId);

            for (int i = 0; i <= empTypeWiseCompConfig.Tables[0].Rows.Count - 1; i++)
            {
                var componentName = empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentName"].ToString();
                var componentType = empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentType"].ToString();
                var isSalaryImpactProhibited = bool.Parse(empTypeWiseCompConfig.Tables[0].Rows[i]["IsSalaryImpactProhibited"].ToString());
                var maxLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MaximumLimit"].ToString());
                var minLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MinimumLimit"].ToString());

                var payrollConfigurationType = SessionHelper.PayrollConfigurationType;


                if (SessionHelper.CompanyName == "Gono Unnayan Prochesta (GUP)")
                {

                }
                else
                {
                    if (payrollConfigurationType == PayrollConfigurationTypeConstants.Basic
                    && componentName == "Basic Salary")
                    {
                        if (componentType != SalaryCalculationTypeConstants.Fixed)
                            continue;
                    }
                }

                var ratioPercent = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentAmount"].ToString());
                var ratioBasedOn = empTypeWiseCompConfig.Tables[0].Rows[i]["RatioBasedOn"].ToString();
                var salaryRoundType = empTypeWiseCompConfig.Tables[0].Rows[i]["SalaryRoundType"].ToString();

                if (componentType == SalaryCalculationTypeConstants.Ratio
                    && ratioBasedOn == RatioBasedOnConstants.Gross)
                {
                    var ratio = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), gross);
                    if (salaryRoundType == "RoundUp")
                        ratio = Math.Round(ratio);
                    if (salaryRoundType == "RoundDown")
                        ratio = Math.Ceiling(ratio);

                    if (ratio < minLimit && minLimit != 0)
                        ratio = minLimit;
                    if (ratio > maxLimit && maxLimit != 0)
                        ratio = maxLimit;

                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratio;
                }
                else if (componentType == SalaryCalculationTypeConstants.Ratio
                   && ratioBasedOn == RatioBasedOnConstants.Basic)
                {
                    var ratio = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), basicSalary);

                    if (salaryRoundType == "RoundUp")
                        ratio = Math.Round(ratio);
                    if (salaryRoundType == "RoundDown")
                        ratio = Math.Ceiling(ratio);
                    if (ratio < minLimit && minLimit != 0)
                        ratio = minLimit;
                    if (ratio > maxLimit && maxLimit != 0)
                        ratio = maxLimit;

                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratio;
                }
                else if (componentType == SalaryCalculationTypeConstants.Fixed
                        && ratioBasedOn == RatioBasedOnConstants.NotRequired
                        && !isSalaryImpactProhibited)// mahfuz add this validation 
                    if (componentName != "Basic Salary")
                        empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratioPercent;
                    else
                        empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = basicSalary;
                else if (componentType == SalaryCalculationTypeConstants.Fixed)
                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratioPercent;//for fixed ratioPercentage is the fixed                                                                                        //
                else
                {
                    if (SessionHelper.CompanyName == "Gono Unnayan Prochesta (GUP)")
                    {
                        if (componentName == "Basic Salary" && componentType == "R")
                        {
                            empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = basicSalary;
                        }
                    }
                }

                if (empAllowencelst.Any())
                {
                    int ComponentPayrollId = 0;
                    int.TryParse(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentPayrollId"].ToString(), out ComponentPayrollId);
                    var alwLst = empAllowencelst.Where(x => x.ComponentId == ComponentPayrollId);
                    if (alwLst.Any())
                    {
                        double allowenceAmt = 0;
                        if (alwLst.First().RatioOn == "Percentage")
                        {
                            allowenceAmt = Math.Round((basicSalary * (double)(alwLst.First().Allowance / 100)));
                            if (maxLimit > 0)
                                allowenceAmt = allowenceAmt < minLimit ? minLimit : allowenceAmt > maxLimit ? maxLimit : allowenceAmt;
                        }

                        else allowenceAmt = (double)alwLst.First().Allowance;
                        empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = allowenceAmt;
                    }
                }

                if (decimal.Parse(empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"].ToString()) > 0)
                {
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

            }

            return dataList;
        }


        private List<PRSalaryScaleViewModel> EmployeeInPayScale(string grade, string scale, string empSalaryTypeId, double basicSalary, double gross, int EmployeeStatusId, int OfficeLocationId, int pfTypeId)
        {
            var param2 = new { EmployeeTypeId = Convert.ToInt32(empSalaryTypeId), EmployeeStatusId = EmployeeStatusId, OfficeLocationId = OfficeLocationId, PFTypeId = pfTypeId };
            var empTypeWiseCompConfig = employeeSPService.GetDataWithParameter(param2, "prl.SP_Get_EmployeeTypeWiseComponentConfiguration");
            empTypeWiseCompConfig.Tables[0].Columns.Add(new DataColumn("CalculatedAmount", typeof(System.Double)));

            List<PRSalaryScaleViewModel> dataList = new List<PRSalaryScaleViewModel>();
            int gradeid = int.Parse(grade);
            var empAllowencelst = employeeAllowenceService.GetMany(x => x.GradeId == gradeid && x.EmployeeStatusId == EmployeeStatusId);

            for (int i = 0; i <= empTypeWiseCompConfig.Tables[0].Rows.Count - 1; i++)
            {
                var componentName = empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentName"].ToString();
                var componentType = empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentType"].ToString();
                var isSalaryImpactProhibited = bool.Parse(empTypeWiseCompConfig.Tables[0].Rows[i]["IsSalaryImpactProhibited"].ToString());
                var maxLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MaximumLimit"].ToString());
                var minLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MinimumLimit"].ToString());

                var payrollConfigurationType = SessionHelper.PayrollConfigurationType;


                if (SessionHelper.CompanyName == "Gono Unnayan Prochesta (GUP)" || SessionHelper.CompanyName == "PSS" || SessionHelper.CompanyName == "PROTTYASHI" || SessionHelper.CompanyName == "DBS" || SessionHelper.CompanyInfo.CompanyShortName == "ADI")
                {

                }
                else
                {
                    if (payrollConfigurationType == PayrollConfigurationTypeConstants.Basic
                    && componentName == "Basic Salary")
                    {
                        if (componentType != SalaryCalculationTypeConstants.Fixed)
                            continue;
                    }
                }

                var ratioPercent = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentAmount"].ToString());
                var ratioBasedOn = empTypeWiseCompConfig.Tables[0].Rows[i]["RatioBasedOn"].ToString();
                var salaryRoundType = empTypeWiseCompConfig.Tables[0].Rows[i]["SalaryRoundType"].ToString();

                if (componentType == SalaryCalculationTypeConstants.Ratio
                    && ratioBasedOn == RatioBasedOnConstants.Gross)
                {
                    var ratio = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), gross);
                    if (salaryRoundType == "RoundUp")
                        ratio = Math.Round(ratio);
                    if (salaryRoundType == "RoundDown")
                        ratio = Math.Ceiling(ratio);

                    if (ratio < minLimit && minLimit != 0)
                        ratio = minLimit;
                    if (ratio > maxLimit && maxLimit != 0)
                        ratio = maxLimit;

                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratio;
                }
                else if (componentType == SalaryCalculationTypeConstants.Ratio
                   && ratioBasedOn == RatioBasedOnConstants.Basic)
                {
                    var ratio = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), basicSalary);

                    if (salaryRoundType == "RoundUp")
                        ratio = Math.Round(ratio);
                    if (salaryRoundType == "RoundDown")
                        ratio = Math.Ceiling(ratio);
                    if (ratio < minLimit && minLimit != 0)
                        ratio = minLimit;
                    if (ratio > maxLimit && maxLimit != 0)
                        ratio = maxLimit;

                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratio;
                }
                else if (componentType == SalaryCalculationTypeConstants.Fixed
                        && ratioBasedOn == RatioBasedOnConstants.NotRequired
                        && !isSalaryImpactProhibited)// mahfuz add this validation 
                    if (componentName != "Basic Salary")
                        if(SessionHelper.CompanyName == "PROTTYASHI" ||  SessionHelper.CompanyName == "DBS" || SessionHelper.CompanyInfo.CompanyShortName == "ADI")
                            if(componentName == "Basic")
                              empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = basicSalary;  
                            else
                              empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratioPercent;
                        else
                         empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratioPercent;
                    else
                        empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = basicSalary;
                else if (componentType == SalaryCalculationTypeConstants.Fixed)
                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratioPercent;//for fixed ratioPercentage is the fixed                                                                                        //
                else
                {
                    if (SessionHelper.CompanyName == "Gono Unnayan Prochesta (GUP)" || SessionHelper.CompanyName == "PSS" || SessionHelper.CompanyName == "PROTTYASHI" || SessionHelper.CompanyName == "DBS" || SessionHelper.CompanyInfo.CompanyShortName == "ADI" )
                    {
                        if (componentName == "Basic Salary" && componentType == "R")
                        {
                            empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = basicSalary;
                        }
                        if (componentName == "Basic" && componentType == "R")
                        {
                            empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = basicSalary;
                        }

                    }
                }

                if (empAllowencelst.Any())
                {
                    int ComponentPayrollId = 0;
                    int.TryParse(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentPayrollId"].ToString(), out ComponentPayrollId);
                    var alwLst = empAllowencelst.Where(x => x.ComponentId == ComponentPayrollId);
                    if (alwLst.Any())
                    {
                        double allowenceAmt = 0;
                        if (alwLst.First().RatioOn == "Percentage")
                        {
                            allowenceAmt = Math.Round((basicSalary * (double)(alwLst.First().Allowance / 100)));
                            if (maxLimit > 0)
                                allowenceAmt = allowenceAmt < minLimit ? minLimit : allowenceAmt > maxLimit ? maxLimit : allowenceAmt;
                        }

                        else allowenceAmt = (double)alwLst.First().Allowance;
                        empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = allowenceAmt;
                    }
                }

                if (decimal.Parse(empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"].ToString()) > 0)
                {
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

            double rest_amt = 0;
            double lfa_adjut = 0;
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

                    if (SessionHelper.CompanyInfo.CompanyShortName == "GT" || SessionHelper.CompanyInfo.CompanyShortName == "GUP")
                    {
                        // For GUP, skip the continue
                        if (SessionHelper.CompanyInfo.CompanyShortName != "GUP")
                        {
                            if (componentType != SalaryCalculationTypeConstants.Fixed)
                                continue;
                        }
                    }
                    else
                    {

                        if (componentType != SalaryCalculationTypeConstants.Fixed)
                            continue;
                    }
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
                        ratio = Math.Ceiling(ratio);
                    else if (salaryRoundType == "RoundNormal")
                        ratio = Math.Round(ratio);
                    else if (salaryRoundType == "RoundDown")
                        ratio = Math.Floor(ratio);
                    #region Close Mahfuz Format may be wrong
                    //if (salaryRoundType == "RoundUp")
                    //{
                    //    ratio = Math.Round(ratio);
                    //}
                    //if (salaryRoundType == "RoundDown")
                    //{
                    //    ratio = Math.Ceiling(ratio);
                    //}
                    #endregion Close Mahfuz
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
                        ratio = Math.Ceiling(ratio);
                    else if (salaryRoundType == "RoundNormal")
                        ratio = Math.Round(ratio);
                    else if (salaryRoundType == "RoundDown")
                        ratio = Math.Floor(ratio);
                    #region Close Mahfuz Format may be wrong
                    //if (salaryRoundType == "RoundUp")
                    //{
                    //    ratio = Math.Round(ratio);
                    //}
                    //if (salaryRoundType == "RoundDown")
                    //{
                    //    ratio = Math.Ceiling(ratio);
                    //}
                    #endregion Close Mahfuz
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

                double calculatedAmount;

                if (SessionHelper.CompanyInfo.CompanyShortName == "GTT")
                {
                    calculatedAmount = Math.Round(empTypeWiseCompConfig.Tables[0].Rows[i].Field<double>("CalculatedAmount"), MidpointRounding.AwayFromZero);
                }
                else
                {
                    calculatedAmount = empTypeWiseCompConfig.Tables[0].Rows[i].Field<double>("CalculatedAmount");
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
                    CalculatedAmount = calculatedAmount,
                    ComponentCategory = empTypeWiseCompConfig.Tables[0].Rows[i].Field<string>("ComponentCategory"),
                    TransactionType = empTypeWiseCompConfig.Tables[0].Rows[i].Field<string>("TransactionType"),
                    EmployeeStatusId = empTypeWiseCompConfig.Tables[0].Rows[i].Field<int?>("EmployeeStatusId"),
                    TransactionTypeView = empTypeWiseCompConfig.Tables[0].Rows[i].Field<string>("TransactionTypeView")
                });

                if (componentName == "Basic Salary" || componentName == "House Rent" || componentName == "Conveyance" || componentName == "Medical" || componentName == "LFA")
                    rest_amt = rest_amt + calculatedAmount; // Convert.ToDouble( empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"].ToString());
            }

            lfa_adjut = gross - rest_amt;

            if(SessionHelper.CompanyInfo.CompanyShortName == "GTT" )
            {
                var lfa = dataList.FirstOrDefault(z => z.ComponentName == "LFA");
                if (lfa != null)
                {
                    lfa.CalculatedAmount += Convert.ToDouble(lfa_adjut);
                    lfa.CalculatedAmount = Math.Round( lfa.CalculatedAmount, 2);
                    //lfa.SaveChanges();
                }
            }
            
            return dataList;
        }


        private List<PRSalaryScaleViewModel> DistributeEmployeeSalaryInComponents_prottashi(int empSalaryTypeId,
       double basicSalary, double gross, int EmployeeStatusId, int OfficeLocationId, int pfType)
        {
            var param2 = new
            {
                EmployeeTypeId = empSalaryTypeId,
                EmployeeStatusId = EmployeeStatusId,
                OfficeLocationId = OfficeLocationId,
                PFTypeId = pfType
            };

            double rest_amt = 0;
            double lfa_adjut = 0;
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
                        ratio = Math.Ceiling(ratio);
                    else if (salaryRoundType == "RoundNormal")
                        ratio = Math.Round(ratio);
                    else if (salaryRoundType == "RoundDown")
                        ratio = Math.Floor(ratio);
                    #region Close Mahfuz Format may be wrong
                    //if (salaryRoundType == "RoundUp")
                    //{
                    //    ratio = Math.Round(ratio);
                    //}
                    //if (salaryRoundType == "RoundDown")
                    //{
                    //    ratio = Math.Ceiling(ratio);
                    //}
                    #endregion Close Mahfuz
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
                        ratio = Math.Ceiling(ratio);
                    else if (salaryRoundType == "RoundNormal")
                        ratio = Math.Round(ratio);
                    else if (salaryRoundType == "RoundDown")
                        ratio = Math.Floor(ratio);
                    #region Close Mahfuz Format may be wrong
                    //if (salaryRoundType == "RoundUp")
                    //{
                    //    ratio = Math.Round(ratio);
                    //}
                    //if (salaryRoundType == "RoundDown")
                    //{
                    //    ratio = Math.Ceiling(ratio);
                    //}
                    #endregion Close Mahfuz
                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratio;
                }
                else if (componentType == SalaryCalculationTypeConstants.Fixed
                        && ratioBasedOn == RatioBasedOnConstants.NotRequired)
                {
                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = gross;
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

                if (componentName == "Basic Salary" || componentName == "House Rent" || componentName == "Conveyance" || componentName == "Medical" || componentName == "LFA")
                    rest_amt = rest_amt + Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"].ToString());
            }

            lfa_adjut = gross - rest_amt;

            if (SessionHelper.CompanyInfo.CompanyShortName == "GTT")
            {
                var lfa = dataList.FirstOrDefault(z => z.ComponentName == "LFA");
                if (lfa != null)
                {
                    lfa.CalculatedAmount += Convert.ToDouble(lfa_adjut);
                    lfa.CalculatedAmount = Math.Round(lfa.CalculatedAmount, 2);
                    //lfa.SaveChanges();
                }
            }

            return dataList;
        }

        #endregion

        #region GeneratePayrollFromOldSalaryTable


        #endregion
    }
}