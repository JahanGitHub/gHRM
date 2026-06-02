#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Text;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using gHRM.Service.Payroll;
using gHRM.Service.payroll;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Web.ViewModels.Payroll;
using gHRM.Web.ViewModels.payroll;
using gHRM.Core.Utilities.Constants;
using gHRM.Core.Filters.Payroll;
using System.Configuration;
using gHRM.Service.PF;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Service.Loan;
using System.Data.Entity;
using gHRM.Data.DBDetailModels.Promotions;
using gHRM.Service.Cooperative;
using gHRM.Data.CodeFirstMigration.Cooperative;
using gHRM.Service.Loan.LoanCalculationService;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using gHRM.Service.WelfareFund;
using gHRM.Web.ViewModels;
using gHRM.Web.CommonDropdown;


using OfficeOpenXml;
using System.Web;
using System.IO;
using NPOI.HSSF.UserModel;  // .xls
using NPOI.XSSF.UserModel;  // .xlsx
using NPOI.SS.UserModel;
using System.Data.SqlClient;

#endregion

namespace gHRM.Web.Controllers
{
    public class PRProcessController : BaseController
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
        private readonly ICooperativeLedgerService cooperativeLedgerService;
        private readonly IFundSetupService fundSetupService;
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;


        // Tazdik

        private readonly IEmployeeGradeListService employeeGradeListService;
        private readonly IEmployeeDesignationService employeeDesignationService;
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeDesignationService _employeeDesignationService;
        private readonly IEmployeeMonthlySalaryService _employeeMonthlySalaryService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;


        public PRProcessController(
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
            , ICooperativeLedgerService cooperativeLedgerService
            , IFundSetupService fundSetupService

              //  Tazdik

              , IEmployeeGradeListService employeeGradeListService
              , IEmployeeDesignationService employeeDesignationService
              , IEmployeeDepartmentService employeeDepartmentService
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
            this.cooperativeLedgerService = cooperativeLedgerService;
            this.fundSetupService = fundSetupService;


            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();

            // Tazdik
            this._employeeService = employeeService;
            this._employeeDesignationService = employeeDesignationService;
            this.employeeGradeListService = employeeGradeListService;
            this.employeeGradeListService = employeeGradeListService;
            this._employeeService = employeeService;
            this._employeeDesignationService = employeeDesignationService;
            this.employeeDepartmentService = employeeDepartmentService;
        }

        #endregion

        #region Action Methods


        // Tazdik
        // Task -3
        // SalaryBeforeApproveUpload

        // GET: PRProcess/SalaryBeforeApproveUpload
        [HttpGet]
        public ActionResult SalaryBeforeApproveUpload()
        {
            return View(new List<SalaryBeforeApproveUploadModel>());
        }

        // POST: PRProcess/SalaryBeforeApproveUpload
        [HttpPost]
        public JsonResult SalaryBeforeApproveUpload(List<SalaryBeforeApproveUploadModel> data)
        {
            string message = "";
            try
            {
                if (data == null || data.Count == 0)
                    return Json("No records to save.", JsonRequestBehavior.AllowGet);

                foreach (var item in data)
                {
                    var param = new
                    {
                        Sl = item.Sl,
                        EmployeeCode = item.EmployeeCode
                    };

                    // Call SP for each row
                    employeeSPService.GetDataWithParameter(param, "usp_InsertSalaryBeforeApproveUpload");
                }

                message = $"Saved {data.Count} record(s) successfully.";
            }
            catch (Exception ex)
            {
                message = "Error saving data: " + ex.Message;
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        // POST: Upload and parse file
        [HttpPost]
        public ActionResult UploadAndParseSalaryFile()
        {
            try
            {
                HttpPostedFileBase file = Request.Files["salaryFile"];

                if (file == null || file.ContentLength == 0)
                    return Json(new { success = false, message = "Please select a file." });

                List<SalaryBeforeApproveUploadModel> parsedData = new List<SalaryBeforeApproveUploadModel>();
                string extension = Path.GetExtension(file.FileName).ToLower();

                if (extension == ".csv")
                {
                    parsedData = ParseCsvFile(file);
                }
                else if (extension == ".xlsx")
                {
                    parsedData = ParseXlsxFile(file); // NPOI XSSFWorkbook
                }
                else if (extension == ".xls")
                {
                    parsedData = ParseXlsFile(file);  // NPOI HSSFWorkbook
                }
                else
                {
                    return Json(new { success = false, message = "Unsupported file format." });
                }

                return Json(new { success = true, data = parsedData });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error parsing file: " + ex.Message });
            }
        }

        // CSV parser
        private List<SalaryBeforeApproveUploadModel> ParseCsvFile(HttpPostedFileBase file)
        {
            List<SalaryBeforeApproveUploadModel> data = new List<SalaryBeforeApproveUploadModel>();
            using (var reader = new StreamReader(file.InputStream))
            {
                // Skip first 6 rows (metadata and headers)
                for (int i = 0; i < 6; i++)
                {
                    reader.ReadLine();
                }

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    if (values.Length >= 3)
                    {
                        string slText = values[1].Trim();
                        string empText = values[2].Trim();

                        // Skip sub-total rows and empty rows
                        if (string.IsNullOrEmpty(slText) || string.IsNullOrEmpty(empText) ||
                            empText.Equals("Sub Total", StringComparison.OrdinalIgnoreCase))
                            continue;

                        int slValue;
                        if (int.TryParse(slText, out slValue))
                        {
                            data.Add(new SalaryBeforeApproveUploadModel
                            {
                                Sl = slValue,
                                EmployeeCode = empText
                            });
                        }
                    }
                }
            }
            return data;
        }

        private List<SalaryBeforeApproveUploadModel> ParseXlsFile(HttpPostedFileBase file)
        {
            List<SalaryBeforeApproveUploadModel> data = new List<SalaryBeforeApproveUploadModel>();
            using (var stream = file.InputStream)
            {
                HSSFWorkbook workbook = new HSSFWorkbook(stream);

                // Process all sheets
                for (int sheetIndex = 0; sheetIndex < workbook.NumberOfSheets; sheetIndex++)
                {
                    ISheet sheet = workbook.GetSheetAt(sheetIndex);

                    // Skip the first 6 rows (metadata and headers)
                    for (int rowIndex = 6; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        IRow row = sheet.GetRow(rowIndex);
                        if (row == null) continue;

                        // Column B (SL) is index 1, Column C (Emp. ID) is index 2
                        var slCell = row.GetCell(1);
                        var empCell = row.GetCell(2);

                        if (slCell == null || empCell == null) continue;

                        string slText = slCell.ToString().Trim();
                        string empText = empCell.ToString().Trim();

                        // Skip sub-total rows and empty rows
                        if (string.IsNullOrEmpty(slText) || string.IsNullOrEmpty(empText) ||
                            empText.Equals("Sub Total", StringComparison.OrdinalIgnoreCase))
                            continue;

                        int slValue;
                        if (int.TryParse(slText, out slValue))
                        {
                            data.Add(new SalaryBeforeApproveUploadModel
                            {
                                Sl = slValue,
                                EmployeeCode = empText
                            });
                        }
                    }
                }
            }
            return data;
        }

        private List<SalaryBeforeApproveUploadModel> ParseXlsxFile(HttpPostedFileBase file)
        {
            List<SalaryBeforeApproveUploadModel> data = new List<SalaryBeforeApproveUploadModel>();
            using (var stream = file.InputStream)
            {
                XSSFWorkbook workbook = new XSSFWorkbook(stream);

                // Process all sheets
                for (int sheetIndex = 0; sheetIndex < workbook.NumberOfSheets; sheetIndex++)
                {
                    ISheet sheet = workbook.GetSheetAt(sheetIndex);

                    // Skip the first 6 rows (metadata and headers)
                    for (int rowIndex = 6; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        IRow row = sheet.GetRow(rowIndex);
                        if (row == null) continue;

                        // Column B (SL) is index 1, Column C (Emp. ID) is index 2
                        var slCell = row.GetCell(1);
                        var empCell = row.GetCell(2);

                        if (slCell == null || empCell == null) continue;

                        string slText = slCell.ToString().Trim();
                        string empText = empCell.ToString().Trim();

                        // Skip sub-total rows and empty rows
                        if (string.IsNullOrEmpty(slText) || string.IsNullOrEmpty(empText) ||
                            empText.Equals("Sub Total", StringComparison.OrdinalIgnoreCase))
                            continue;

                        int slValue;
                        if (int.TryParse(slText, out slValue))
                        {
                            data.Add(new SalaryBeforeApproveUploadModel
                            {
                                Sl = slValue,
                                EmployeeCode = empText
                            });
                        }
                    }
                }
            }
            return data;
        }


        // Task - 6
        // Tazdik
        // Promotion

        public ActionResult Promotion()
        {
            var model = new PromotionViewModel();

            ViewData["DesignationList"] = commonDynamicDropDown.GetAllOfficeDesignationList();

            model.NextIncrement = DateTime.Today.AddYears(1);

            return View(model);
        }

        [HttpPost]
        public ActionResult Promotion(PromotionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var context = new gHRMDBContext())
                    {
                        context.Database.ExecuteSqlCommand(
                            "EXEC USP_Insert_Promotion @EmplD, @EmpName, @EmpCode, @Designation, @PreSalary, @Increment, @NewSalary, @EffectDate, @OrderDate, @ArearSalary, @ArearDear, @ArearBonus, @LastSalary, @LastDear",
                            new SqlParameter("@EmplD", model.EmplD ?? (object)DBNull.Value),
                            new SqlParameter("@EmpName", model.EmpName ?? (object)DBNull.Value),
                            new SqlParameter("@EmpCode", model.EmpCode ?? (object)DBNull.Value),
                            new SqlParameter("@Designation", model.Designation ?? (object)DBNull.Value),
                            new SqlParameter("@PreSalary", model.PreSalary),
                            new SqlParameter("@Increment", model.Increment),
                            new SqlParameter("@NewSalary", model.NewSalary),
                            new SqlParameter("@EffectDate", model.EffectDate),
                            new SqlParameter("@OrderDate", model.OrderDate),
                            new SqlParameter("@ArearSalary", (object)model.ArearSalary ?? DBNull.Value),
                            new SqlParameter("@ArearDear", (object)model.ArearDear ?? DBNull.Value),
                            new SqlParameter("@ArearBonus", (object)model.ArearBonus ?? DBNull.Value),
                            new SqlParameter("@LastSalary", model.LastSalary),
                            new SqlParameter("@LastDear", model.LastDear)
                        );
                    }
                    TempData["SuccessMessage"] = "Promotion record inserted successfully!";

                    return RedirectToAction("Promotion");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error: " + ex.Message);
                }
            }

            ViewData["DesignationList"] = commonDynamicDropDown.GetAllOfficeDesignationList();

            return View(model);
        }

        public JsonResult GetEmployeeInfoByCode(string employeeCode)
        {
            try
            {
                var employee = _employeeService.GetByCode(employeeCode);
                if (employee == null)
                    return Json(new { Success = false, Message = "Employee not found" }, JsonRequestBehavior.AllowGet);

                // Office info
                var officeInfo = officeService.Get(b => b.OfficeId == employee.OfficeId);

                // Department
                var departmentName = employeeDepartmentService.GetById(Convert.ToInt32(employee.DepartmentId))?.DepartmentName ?? "";

                // Designation
                var currentDesignation = _employeeDesignationService.GetById(Convert.ToInt32(employee.DesignationId));
                var currentDesignationName = currentDesignation != null ? currentDesignation.DesignationName : "";

                // Grade
                string currentGrade = "";
                if (employee.GradeId.HasValue)
                {
                    var grade = employeeGradeListService.GetById(employee.GradeId.Value);
                    currentGrade = grade != null ? grade.GradeName : "";
                }

                // Promotion info
                var promotionInfo = employeePromotionService.GetPromotionInfo(employee.EmployeeId);
                var promotionDate = promotionInfo?.PromotionDate != null ? Convert.ToDateTime(promotionInfo.PromotionDate).ToString("dd-MMM-yyyy") : "";
                var nextReviewDate = promotionInfo?.NextReviewDate != null ? Convert.ToDateTime(promotionInfo.NextReviewDate).ToString("dd-MMM-yyyy") : "";

                // Salary
                decimal presentSalary = GetEmployeeCurrentSalary(Convert.ToInt32(employee.EmployeeId));

                return Json(new
                {
                    Success = true,
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = employee.EmployeeName ?? "",
                    CurrentDesignation = currentDesignationName,
                    Grade = currentGrade,
                    PresentSalary = presentSalary,
                    CotractDate = employee.AgreementToDate.HasValue ?
                        employee.AgreementToDate.Value.ToString("yyyy-MM-dd") : "",

                    // Extended info
                    // Extended info
                    JoiningDate = employee.FirstJoiningDate.ToString("dd-MMM-yyyy") ?? "",
                    ConfirmationDate = employee.ConfirmationDate?.ToString("dd-MMM-yyyy") ?? "",
                    DepartmentName = departmentName,
                    PromotionDate = promotionDate,
                    NextReviewDate = nextReviewDate,
                    OfficeId = officeInfo?.OfficeId ?? 0,
                    OfficeLocationId = officeInfo?.OfficeLocationId ?? 0,
                    EmployeeStatusId = employee.EmployeeStatusId,
                    DesignationId = employee.DesignationId,
                    BankAccountNo = employee.BankAccountNo ?? "",
                    BankName = employee.BankName ?? "",
                    BankBranchName = employee.BankBranchName ?? "",
                    PFTypeId = employee.PFTypeId,
                    IsOvertimeException = employee.IsOvertimeException,
                    PayrollConfigurationType = SessionHelper.PayrollConfigurationType,

                    LastGross = 0,
                    LastDear = 0
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private decimal GetEmployeeCurrentSalary(int employeeId)
        {
            // Generate salary components for the employee
            var salaryComponents = GenerateEmployeeSalary(
                empSalaryTypeId: 0,
                EmployeeStatusId: 0,
                grossSalary: 0,               // Placeholder; actual gross is calculated
                salaryGenerationType: "GR",   // Or from SessionHelper
                OfficeLocationId: 0,
                pfTypeId: "0",
                empid: employeeId
            ).Data as List<PRSalaryScaleViewModel>;

            if (salaryComponents != null && salaryComponents.Any())
            {
                // Sum all calculated amounts from components
                return salaryComponents.Sum(x => (decimal)x.CalculatedAmount);
            }

            // If no generated components exist, fallback to 0
            return 0;
        }

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
                        prSalaryList.ForEach(x => x.CalculatedAmount = (double)x.ComponentAmount);
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
                        prSalaryList = DistributeEmployeeSalaryInComponents(empSalaryTypeId, basicSalary, grossSalary,
                            EmployeeStatusId, OfficeLocationId, Convert.ToInt32(pfTypeId));
                    else
                    {
                    }

                }
            }

            catch (Exception ex)
            {
                var result = 0;
            }

            return Json(prSalaryList, JsonRequestBehavior.AllowGet);
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


        public ActionResult Index()
        {
            var model = new PRWorkAreaViewModel();

            //get salary date configuration
            var salaryDateConfiguration = salaryDateConfigService.GetCurrentSalaryDateConfig();
            if (salaryDateConfiguration == null)
                return Redirect("/SalaryDateConfig/Manage");

            model.SalaryDay = salaryDateConfiguration.DayOfMonthlySalary;

            ViewData["Months"] = Months();
            ViewData["Years"] = Years();

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;

            //Populate salary processed by employee office info
            model = PopulateSalaryProcessedByEmployeeOfficeInfo(model);

            MapDropDownList(model);
            MapIndexDropdown(model);

            return View(model);
        }


        public ActionResult Index_EmployeeCode()
        {
            var model = new PRWorkAreaViewModel();

            //get salary date configuration
            var salaryDateConfiguration = salaryDateConfigService.GetCurrentSalaryDateConfig();
            if (salaryDateConfiguration == null)
                return Redirect("/SalaryDateConfig/Manage");

            model.SalaryDay = salaryDateConfiguration.DayOfMonthlySalary;

            ViewData["Months"] = Months();
            ViewData["Years"] = Years();

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;

            //Populate salary processed by employee office info
            model = PopulateSalaryProcessedByEmployeeOfficeInfo(model);

            MapDropDownList(model);
            MapIndexDropdown(model);

            return View(model);
        }


        public ActionResult SalaryAdvice()
        {
            var model = new PRWorkAreaViewModel();

            //get salary date configuration
            var salaryDateConfiguration = salaryDateConfigService.GetCurrentSalaryDateConfig();
            if (salaryDateConfiguration == null)
                return Redirect("/SalaryDateConfig/Manage");

            model.SalaryDay = salaryDateConfiguration.DayOfMonthlySalary;

            ViewData["Months"] = Months();
            ViewData["Years"] = Years();

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;

            //Populate salary processed by employee office info
            model = PopulateSalaryProcessedByEmployeeOfficeInfo(model);

            MapDropDownList2(model);
            MapIndexDropdown(model);

            return View(model);
        }

        public ActionResult Index2()
        {
            var model = new PRWorkAreaViewModel();

            //get salary date configuration
            var salaryDateConfiguration = salaryDateConfigService.GetCurrentSalaryDateConfig();
            if (salaryDateConfiguration == null)
                return Redirect("/SalaryDateConfig/Manage");

            model.SalaryDay = salaryDateConfiguration.DayOfMonthlySalary;

            ViewData["Months"] = Months();
            ViewData["Years"] = Years();

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;

            //Populate salary processed by employee office info
            model = PopulateSalaryProcessedByEmployeeOfficeInfo(model);

            MapDropDownList2(model);
            MapIndexDropdown(model);

            return View(model);
        }


        public ActionResult CostCenter()
        {
            var model = new ChallanViewModel();
            return View(model);
        }

        public ActionResult getCostCenterData([DataSourceRequest] DataSourceRequest request )
        {
            try
            {              
                var list = employeeSPService.GetDataWithoutParameter("SP_GET_COSTCENTER");

                var monthlySalarys = list.Tables[0].AsEnumerable().Select(row => new View_EmployeeMonthlySalary()
                {
                    SalaryMonth = row.Field<int>("SalaryMonth"),                    
                    TransactionType = row.Field<string>("TransactionType"),
                    PRComponentAmount = row.Field<decimal>("PRComponentAmount")
                }).ToList();
              

                DataSourceResult result = monthlySalarys.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult Challan()
        {
            var model = new ChallanViewModel();
            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["OfficeList"] = items;
            ViewData["HOOfficeList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["OfficeListByType"] = items;
            ViewData["OfficeDeptByType"] = items;
            ViewData["OfficeType"] = LoggedInOfficeType;
            ViewData["LoggedInOfficeId"] = LoggedInOfficeID;
            var offc = officeService.GetById(Convert.ToInt32(LoggedInOfficeID));
            ViewData["SecondLevel"] = offc.SecondLevel;
            ViewData["SecondLevelId"] = officeService.GetByOfficeCode(offc.SecondLevel).OfficeId;
            ViewData["ThirdLevel"] = offc.ThirdLevel;
            ViewData["ThirdLevelId"] = officeService.GetByOfficeCode(offc.ThirdLevel).OfficeId;
            ViewData["FourthLevel"] = offc.FourthLevel;
            ViewData["FourthLevelId"] = officeService.GetByOfficeCode(offc.FourthLevel).OfficeId;
   

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.DepartmentList = commonDynamicDropDown.GetAllActiveDepartmentList();
            var employeeStatusList = commonDynamicDropDown.ddlEmployeeStatusList();
            employeeStatusList.RemoveAll(x => x.Value == "");
           
            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();

            ViewData["Months"] = Months();
            ViewData["Years"] = Years();

        
            ViewData["ComponentList"] = items;

            var PleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var yearList = new List<SelectListItem>();
            yearList.Add(PleaseSelect);
            for (int i = DateTime.Now.Year; i >= (DateTime.Now.Year) - 1; i--)
            {
                yearList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            model.YearList = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(PleaseSelect);
            for (var i = 1; i <= 12; i++)
            {
                monthList.Add(new SelectListItem { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Value = i.ToString() });
            }
            model.MonthList = monthList;


            return View(model);
        }

        #endregion

        #region Monthly Salary Approve

        public ActionResult MonthlySalaryApprove()
        {
            ViewData["Months"] = Months();
            ViewData["Years"] = Years();
            var model = new PRWorkAreaViewModel();
            MapDropDownList(model);
            return View(model);
        }

        public ActionResult MonthlySalaryApprove2()
        {
            ViewData["Months"] = Months();
            ViewData["Years"] = Years();
            var model = new PRWorkAreaViewModel();
            MapDropDownList(model);
            return View(model);
        }


        #endregion

        #region HTTP Requests

        public ActionResult getSalaryInformation([DataSourceRequest] DataSourceRequest request, int Month, int Year, long EmployeeId)
        {
            try
            {
                List<EmployeeMonthlySalaryViewModel> ListView = new List<EmployeeMonthlySalaryViewModel>();
                var pram = new { Month = Month, Year = Year, EmployeeId = EmployeeId };
                var salaryList = employeeSPService.GetDataWithParameter(pram, "SP_GetSalaryInformation");
                ListView = salaryList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeMonthlySalaryViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    SalaryId = row.Field<int>("SalaryId"),
                    ComponentName = row.Field<string>("ComponentName"),
                    PRComponentAmount = row.Field<decimal>("PRComponentAmount"),
                    TransactionType = row.Field<string>("TransactionType")

                }).ToList();

                DataSourceResult result = ListView.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult ChallanSave(string ChallnNo, string month, string year, string OfficeTypeId, string OfficeId, string ChallanDate)
        {
            string result = "";
            try
            {
                var param = new 
                            { 
                              ChallanNo = ChallnNo,
                              month = month,
                              year = year,
                              OfficeTypeId = OfficeTypeId,
                              OfficeId = OfficeId,
                              ChallanDate = ChallanDate,
                              CreateBy = SessionHelper.LoginUserEmployeeId,
                            };
                var challanSave = employeeSPService.GetDataWithParameter(param, "prl.sp_challan_save_from_page");

                result = "Save Successfully!";
            }
            catch(Exception ex)
            {
                result = "error" + ex.Message;
            }
           

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult MonthlySalaryProcess(string empType, string month, string salaryYear, int OfficeTypeId)
        {

            string result = "";

            var SalaryGen = new SalaryGenerateHelper(employeeSPService
            , employeeMonthlySalaryService
            , prComponentService
            , employeeMonthlySalaryApprovedService
            , prSalaryRegisterService
            , employeeMonthlySalaryExceptionService
            , employeeSalaryDepositService
            , prDepositService
            , employeeStatusHistoryService
            , officeTypeService
            , officeService
            , employeePromotionService
            , companyWisePayrollConfigService
            , salaryDateConfigService
            , employeeService
            , pfCollectionService
            , loanDisbursementService
            , loanPurposeService
            , loanRegisterService);

            string[] substringsToCheck = { "GT", "GSSB", "GTT", "NGF", "PIDIM", "GC","GUP", "NGF", "GMPF", "PSS" };
            //, "Prottyashi"
            bool containsSubstring = false;

            foreach (string substring in substringsToCheck)
            {
                if (SessionHelper.CompanyInfo.CompanyShortName.Contains(substring))
                {
                    containsSubstring = true;
                    break;
                }
            }

            if (containsSubstring)
            {
                result = SalaryGen.MonthlySalaryProcess(empType, month, salaryYear, OfficeTypeId);

                return Json(result, JsonRequestBehavior.AllowGet);
            } 
            else
            {
                bool isOperationSuccess = true;



                if (OfficeTypeId <= 0)
                    return Json("Office Type Required", JsonRequestBehavior.AllowGet);

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
                    return Json(result, JsonRequestBehavior.AllowGet);

                //get current monthly salary application employees [PRSalaryConfiguration]
                var salaryconfigurations = GetCurrentMonthSalaryApplicableEmployee(firstDate, lastDate, OfficeTypeId)/*.Where(x => x.EmployeeCode == "289")*/.ToList();//Method 03

                var promotionlstObj = new gHRMDBContext().EmployeePromotion.Where(x => x.IsActive && !x.IsReviewed && (
                                                         DbFunctions.TruncateTime(x.PromotionDate) >= DbFunctions.TruncateTime(firstDate)
                                                         && DbFunctions.TruncateTime(x.PromotionDate) <= DbFunctions.TruncateTime(lastDate)
                                                     )).Select(x => new EmployeePromotionModel { EmployeeId = x.EmployeeId, PromotionDate = x.PromotionDate }).ToList();

                if (!salaryconfigurations.Any())
                    return Json("Salary configuration not found. Please Configure salary first!", JsonRequestBehavior.AllowGet);

                //get salary date configuration
                var salaryDateConfiguration = salaryDateConfigService.GetCurrentSalaryDateConfig();

                if (salaryDateConfiguration == null)
                    return Json("Currently active salary Date configuration not found. Please Configure salary date first!", JsonRequestBehavior.AllowGet);

                //var lastDayofSalary = new DateTime(Convert.ToInt32(salaryYear), Convert.ToInt32(month), 1).AddMonths(1).AddDays(-1).Day;
                int lastDayofSalary = DateTime.DaysInMonth(int.Parse(salaryYear), int.Parse(month));
                if (salaryDateConfiguration.DayOfMonthlySalary > lastDayofSalary)
                    return Json("Salary date invalid. Please Configure valid salary date first!", JsonRequestBehavior.AllowGet);


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

                //bool hasPF = false, usedLoanModule = false;
                //if (ConfigurationManager.AppSettings["HasPF"] != null)
                //    hasPF = bool.Parse(ConfigurationManager.AppSettings["HasPF"].ToString());
                //if (ConfigurationManager.AppSettings["UsedLoanModule"] != null)
                //    usedLoanModule = bool.Parse(ConfigurationManager.AppSettings["UsedLoanModule"].ToString());


                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(2, 0, 0)))
                {
                    try
                    {
                        var components = prComponentService.GetMany(p => p.IsActive == true).ToList();

                        if (existingMonthlySalary.Any())
                        {
                            //let's insert monthly salary history [EmployeeMonthlySalaryHistory] and remove from employee monthly salary [EmployeeMonthlySalary]
                            InsertMonthlySalaryHistory(Convert.ToInt32(salaryYear), Convert.ToInt32(month), OfficeTypeId); //Method 07

                            //let's inactive salary details [EmployeeMonthlySalaryException]
                            InActiveExceptionalSalaryDetail(firstDate, lastDate, OfficeTypeId); //Method 08
                        }

                        #region Manual Loan Part
                        // if (!usedLoanModule) { }
                        //{
                        //    //get loan installment detail [LoanInstallmentDetail] 
                        //    var loanEmployees = GetExistingMonthlyLoanDeduction(firstDate, lastDate, OfficeTypeId); //Method 09

                    //    //let's insert loans in monthly salary [EmployeeMonthlySalary]
                    //    InsertLoansInMonthlySalary(loanEmployees, month, salaryYear, salaryDate, components, salaryconfigurations); //Method 10
                    //}




                    // middle confirmation problem                     
                    if(SessionHelper.CompanyInfo.CompanyShortName == "GTT")
                    {
                        var param = new
                        {
                            Year = salaryYear,
                            Month = month,
                        };
                        employeeSPService.GetDataWithParameter(param, "SP_SET_CONFIRMATION_PROBATIONARY_SALARY_IN_ALLOWANCE_DEDUCTION");
                    }

    


                    var loanLst = new LoanCalculationService().LoanCalculationForPayrollProcess(DateTime.Parse(salaryDate), int.Parse(month), int.Parse(salaryYear), OfficeTypeId);
                    if (loanLst.Any())
                    {
                        loanLst.ForEach(f => { f.CreatedBy = (SessionHelper.LoggedInEmployeeID ?? 0); f.UpdatedBy = (SessionHelper.LoggedInEmployeeID ?? 0); });
                        employeeMonthlySalaryService.AddEmployeeMonthlySalaryList(loanLst);
                    }
                    #region Fund
                        var fundLst = fundSetupService.GetAll();

                        if (fundLst.Any())
                        {
                            List<EmployeeMonthlySalary> lst = new List<EmployeeMonthlySalary>();

                            var empID_Lst = salaryconfigurations.Select(x => x.EmployeeID).Distinct().ToList();
                            var fundComponentIds = fundLst.Select(x => x.PRComponentId).ToArray();
                            var prComponentList = prComponentService.GetMany(x => fundComponentIds.Contains(x.ComponentPayrollId ?? 0));
                            string[] commonSalaryHeads = { "Basic Salary", "House Rent", "Conveyance", "Medical" };

                            foreach (var fund in fundLst)
                            {
                                var fundType = fund.FundType;
                                var fundObj = fundLst.Where(x => x.FundType == fundType).First();
                                var fundComponent = prComponentList.Where(x => x.ComponentPayrollId == fundObj.PRComponentId);

                                if (fundComponent.Any())
                                {
                                    foreach (var employeeId in empID_Lst)
                                    {
                                        var salaryConfig = salaryconfigurations.FirstOrDefault(x => x.EmployeeID == employeeId);
                                        if (salaryConfig != null)
                                        {
                                            var employeeComponent = fundComponent.Where(x => x.EmployeeTypeId == salaryConfig.EmployeeTypeId && x.EmployeeStatusId == salaryConfig.EmployeeStatusId);

                                            EmployeeMonthlySalary salary = new EmployeeMonthlySalary
                                            {
                                                CreatedBy = 0,
                                                ComponentCategory = "Deduction",
                                                CreateDate = DateTime.Now,
                                                IsActive = true,
                                                SalaryDate = DateTime.Parse(salaryDate),
                                                SalaryMonth = int.Parse(month),
                                                SalaryYear = int.Parse(salaryYear),
                                                TransactionType = "Dr",
                                                OfficeId = salaryConfig.OfficeID,
                                                UpdateDate = DateTime.Now,
                                                UpdatedBy = 0
                                            };

                                            if (employeeComponent.Any())
                                            {
                                                //salary.PRComponentId = employeeComponent.First().PRComponentID;

                                                salary.PRComponentId = employeeComponent.Where(z => z.OfficeLocationId == salaryConfig.OfficeLocationId).Select(q => q.PRComponentID).FirstOrDefault();


                                                salary.EmployeeId = employeeId;

                                                if (fundObj.ComponentType == "F")
                                                    salary.PRComponentAmount = fundObj.ComponentAmount;
                                                else if (fundObj.RatioBasedOn == "G")
                                                    salary.PRComponentAmount = salaryconfigurations.Where(x => x.EmployeeID == employeeId && x.TransactionType != "Dr" && commonSalaryHeads.Contains(x.ComponentName)).Sum(x => x.ComponentAmount) * (fundObj.ComponentAmount / 100);
                                                else if (fundObj.RatioBasedOn == "B") { }

                                                lst.Add(salary);
                                            }
                                        }
                                    }
                                }
                            }

                            employeeMonthlySalaryService.AddEmployeeMonthlySalaryList(lst);
                        }



                        #endregion Fund



                        #endregion Manual Loan Part
                        #region    Loan 

                        #endregion Loan 
                        //get employee salary deduction [EmployeeSalaryDeduction]
                        var deductedSalarys = GetEmployeesDeductedSalaryWithoutOtherImpactInSalary(firstDate, lastDate, OfficeTypeId); //Method 11
                                                                                                                                       //let's insert into employee monthly salary if any for employee salary deduction [EmployeeMonthlySalary]                                                                                                         
                        InsertRegularSalaryDeduction(deductedSalarys, month, salaryYear, salaryDate, components, salaryconfigurations); //Method 16

                        //get employee salary incentive [EmployeeSalaryIncentive]
                        var approvedincentives = GetExistingMonthlyIncentivesWithoutOtherImpactInSalary(firstDate, lastDate, OfficeTypeId); //Method 13
                                                                                                                                            //let's insert regular incentives [EmployeeMonthlySalary]
                        InsertRegularIncentives(approvedincentives, month, salaryYear, salaryDate, components, salaryconfigurations); //Method 15

                        //let's insert new monthly salary into [prl.EmployeeMonthlySalaryException and prl.EmployeeMonthlySalary]
                        var employeeMonthSalaryLst = InsertNewMonthlySalary(salaryconfigurations, components, OfficeTypeId, firstDate, lastDate, month, salaryYear, salaryDate, promotionlstObj); //Method 17

                        //if (hasPF)
                        //{
                        //    // PF Insert into Temporary table gcpf.TempPFCollection
                        //    var lst = InsertPFTemporary(int.Parse(month), int.Parse(salaryYear), DateTime.Parse(salaryDate), employeeMonthSalaryLst, components);
                        //    if (usedLoanModule)
                        //        // Loan
                        //        InsertLoanTemporary(int.Parse(month), int.Parse(salaryYear), DateTime.Parse(salaryDate), lst, employeeMonthSalaryLst, components);
                        //    pfCollectionService.AddBulk(lst);
                        //}

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

                    if (isOperationSuccess)
                        scope.Complete();

                    scope.Dispose();
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult MonthlySalaryProcess_EmpCode(string empType, string month, string salaryYear, int OfficeTypeId, string employeeCode = null)
        {

            string result = "";

            var SalaryGen = new SalaryGenerateHelper(employeeSPService
            , employeeMonthlySalaryService
            , prComponentService
            , employeeMonthlySalaryApprovedService
            , prSalaryRegisterService
            , employeeMonthlySalaryExceptionService
            , employeeSalaryDepositService
            , prDepositService
            , employeeStatusHistoryService
            , officeTypeService
            , officeService
            , employeePromotionService
            , companyWisePayrollConfigService
            , salaryDateConfigService
            , employeeService
            , pfCollectionService
            , loanDisbursementService
            , loanPurposeService
            , loanRegisterService);

            string[] substringsToCheck = { "GT", "GSSB", "GTT", "NGF", "PIDIM", "GC", "GUP", "NGF", "GMPF", "PSS" };
            //, "Prottyashi"
            bool containsSubstring = false;

            foreach (string substring in substringsToCheck)
            {
                if (SessionHelper.CompanyInfo.CompanyShortName.Contains(substring))
                {
                    containsSubstring = true;
                    break;
                }
            }

            if (containsSubstring)
            {
                result = SalaryGen.MonthlySalaryProcess(empType, month, salaryYear, OfficeTypeId);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                bool isOperationSuccess = true;



                if (OfficeTypeId <= 0)
                    return Json("Office Type Required", JsonRequestBehavior.AllowGet);

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
                    return Json(result, JsonRequestBehavior.AllowGet);

                //get current monthly salary application employees [PRSalaryConfiguration]
                var salaryconfigurations = GetCurrentMonthSalaryApplicableEmployee(firstDate, lastDate, OfficeTypeId)/*.Where(x => x.EmployeeCode == "289")*/.ToList();//Method 03


                // Filter for specific employee if employeeCode is provided
                if (!string.IsNullOrEmpty(employeeCode))
                {
                    salaryconfigurations = salaryconfigurations.Where(x => x.EmployeeCode == employeeCode).ToList();
                }
                else
                {
                    salaryconfigurations = salaryconfigurations.ToList();
                }



                var promotionlstObj = new gHRMDBContext().EmployeePromotion.Where(x => x.IsActive && !x.IsReviewed && (
                                                         DbFunctions.TruncateTime(x.PromotionDate) >= DbFunctions.TruncateTime(firstDate)
                                                         && DbFunctions.TruncateTime(x.PromotionDate) <= DbFunctions.TruncateTime(lastDate)
                                                     )).Select(x => new EmployeePromotionModel { EmployeeId = x.EmployeeId, PromotionDate = x.PromotionDate }).ToList();

                if (!salaryconfigurations.Any())
                    return Json("Salary configuration not found. Please Configure salary first!", JsonRequestBehavior.AllowGet);

                //get salary date configuration
                var salaryDateConfiguration = salaryDateConfigService.GetCurrentSalaryDateConfig();

                if (salaryDateConfiguration == null)
                    return Json("Currently active salary Date configuration not found. Please Configure salary date first!", JsonRequestBehavior.AllowGet);

                //var lastDayofSalary = new DateTime(Convert.ToInt32(salaryYear), Convert.ToInt32(month), 1).AddMonths(1).AddDays(-1).Day;
                int lastDayofSalary = DateTime.DaysInMonth(int.Parse(salaryYear), int.Parse(month));
                if (salaryDateConfiguration.DayOfMonthlySalary > lastDayofSalary)
                    return Json("Salary date invalid. Please Configure valid salary date first!", JsonRequestBehavior.AllowGet);


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

                //bool hasPF = false, usedLoanModule = false;
                //if (ConfigurationManager.AppSettings["HasPF"] != null)
                //    hasPF = bool.Parse(ConfigurationManager.AppSettings["HasPF"].ToString());
                //if (ConfigurationManager.AppSettings["UsedLoanModule"] != null)
                //    usedLoanModule = bool.Parse(ConfigurationManager.AppSettings["UsedLoanModule"].ToString());


                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(2, 0, 0)))
                {
                    try
                    {
                        var components = prComponentService.GetMany(p => p.IsActive == true).ToList();

                        if (existingMonthlySalary.Any())
                        {
                            //let's insert monthly salary history [EmployeeMonthlySalaryHistory] and remove from employee monthly salary [EmployeeMonthlySalary]
                            InsertMonthlySalaryHistory(Convert.ToInt32(salaryYear), Convert.ToInt32(month), OfficeTypeId); //Method 07

                            //let's inactive salary details [EmployeeMonthlySalaryException]
                            InActiveExceptionalSalaryDetail(firstDate, lastDate, OfficeTypeId); //Method 08
                        }

                        #region Manual Loan Part
                        // if (!usedLoanModule) { }
                        //{
                        //    //get loan installment detail [LoanInstallmentDetail] 
                        //    var loanEmployees = GetExistingMonthlyLoanDeduction(firstDate, lastDate, OfficeTypeId); //Method 09

                        //    //let's insert loans in monthly salary [EmployeeMonthlySalary]
                        //    InsertLoansInMonthlySalary(loanEmployees, month, salaryYear, salaryDate, components, salaryconfigurations); //Method 10
                        //}




                        // middle confirmation problem                     
                        if (SessionHelper.CompanyInfo.CompanyShortName == "GTT")
                        {
                            var param = new
                            {
                                Year = salaryYear,
                                Month = month,
                            };
                            employeeSPService.GetDataWithParameter(param, "SP_SET_CONFIRMATION_PROBATIONARY_SALARY_IN_ALLOWANCE_DEDUCTION");
                        }




                        var loanLst = new LoanCalculationService().LoanCalculationForPayrollProcess(DateTime.Parse(salaryDate), int.Parse(month), int.Parse(salaryYear), OfficeTypeId);
                        if (loanLst.Any())
                        {
                            loanLst.ForEach(f => { f.CreatedBy = (SessionHelper.LoggedInEmployeeID ?? 0); f.UpdatedBy = (SessionHelper.LoggedInEmployeeID ?? 0); });
                            employeeMonthlySalaryService.AddEmployeeMonthlySalaryList(loanLst);
                        }
                        #region Fund
                        var fundLst = fundSetupService.GetAll();

                        if (fundLst.Any())
                        {
                            List<EmployeeMonthlySalary> lst = new List<EmployeeMonthlySalary>();

                            var empID_Lst = salaryconfigurations.Select(x => x.EmployeeID).Distinct().ToList();
                            var fundComponentIds = fundLst.Select(x => x.PRComponentId).ToArray();
                            var prComponentList = prComponentService.GetMany(x => fundComponentIds.Contains(x.ComponentPayrollId ?? 0));
                            string[] commonSalaryHeads = { "Basic Salary", "House Rent", "Conveyance", "Medical" };

                            foreach (var fund in fundLst)
                            {
                                var fundType = fund.FundType;
                                var fundObj = fundLst.Where(x => x.FundType == fundType).First();
                                var fundComponent = prComponentList.Where(x => x.ComponentPayrollId == fundObj.PRComponentId);

                                if (fundComponent.Any())
                                {
                                    foreach (var employeeId in empID_Lst)
                                    {
                                        var salaryConfig = salaryconfigurations.FirstOrDefault(x => x.EmployeeID == employeeId);
                                        if (salaryConfig != null)
                                        {
                                            var employeeComponent = fundComponent.Where(x => x.EmployeeTypeId == salaryConfig.EmployeeTypeId && x.EmployeeStatusId == salaryConfig.EmployeeStatusId);

                                            EmployeeMonthlySalary salary = new EmployeeMonthlySalary
                                            {
                                                CreatedBy = 0,
                                                ComponentCategory = "Deduction",
                                                CreateDate = DateTime.Now,
                                                IsActive = true,
                                                SalaryDate = DateTime.Parse(salaryDate),
                                                SalaryMonth = int.Parse(month),
                                                SalaryYear = int.Parse(salaryYear),
                                                TransactionType = "Dr",
                                                OfficeId = salaryConfig.OfficeID,
                                                UpdateDate = DateTime.Now,
                                                UpdatedBy = 0
                                            };

                                            if (employeeComponent.Any())
                                            {
                                                //salary.PRComponentId = employeeComponent.First().PRComponentID;

                                                salary.PRComponentId = employeeComponent.Where(z => z.OfficeLocationId == salaryConfig.OfficeLocationId).Select(q => q.PRComponentID).FirstOrDefault();


                                                salary.EmployeeId = employeeId;

                                                if (fundObj.ComponentType == "F")
                                                    salary.PRComponentAmount = fundObj.ComponentAmount;
                                                else if (fundObj.RatioBasedOn == "G")
                                                    salary.PRComponentAmount = salaryconfigurations.Where(x => x.EmployeeID == employeeId && x.TransactionType != "Dr" && commonSalaryHeads.Contains(x.ComponentName)).Sum(x => x.ComponentAmount) * (fundObj.ComponentAmount / 100);
                                                else if (fundObj.RatioBasedOn == "B") { }

                                                lst.Add(salary);
                                            }
                                        }
                                    }
                                }
                            }

                            employeeMonthlySalaryService.AddEmployeeMonthlySalaryList(lst);
                        }



                        #endregion Fund



                        #endregion Manual Loan Part
                        #region    Loan 

                        #endregion Loan 
                        //get employee salary deduction [EmployeeSalaryDeduction]
                        var deductedSalarys = GetEmployeesDeductedSalaryWithoutOtherImpactInSalary(firstDate, lastDate, OfficeTypeId); //Method 11
                                                                                                                                       //let's insert into employee monthly salary if any for employee salary deduction [EmployeeMonthlySalary]                                                                                                         
                        InsertRegularSalaryDeduction(deductedSalarys, month, salaryYear, salaryDate, components, salaryconfigurations); //Method 16

                        //get employee salary incentive [EmployeeSalaryIncentive]
                        var approvedincentives = GetExistingMonthlyIncentivesWithoutOtherImpactInSalary(firstDate, lastDate, OfficeTypeId); //Method 13
                                                                                                                                            //let's insert regular incentives [EmployeeMonthlySalary]
                        InsertRegularIncentives(approvedincentives, month, salaryYear, salaryDate, components, salaryconfigurations); //Method 15

                        //let's insert new monthly salary into [prl.EmployeeMonthlySalaryException and prl.EmployeeMonthlySalary]
                        var employeeMonthSalaryLst = InsertNewMonthlySalary(salaryconfigurations, components, OfficeTypeId, firstDate, lastDate, month, salaryYear, salaryDate, promotionlstObj); //Method 17

                        //if (hasPF)
                        //{
                        //    // PF Insert into Temporary table gcpf.TempPFCollection
                        //    var lst = InsertPFTemporary(int.Parse(month), int.Parse(salaryYear), DateTime.Parse(salaryDate), employeeMonthSalaryLst, components);
                        //    if (usedLoanModule)
                        //        // Loan
                        //        InsertLoanTemporary(int.Parse(month), int.Parse(salaryYear), DateTime.Parse(salaryDate), lst, employeeMonthSalaryLst, components);
                        //    pfCollectionService.AddBulk(lst);
                        //}

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

                    if (isOperationSuccess)
                        scope.Complete();

                    scope.Dispose();
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ChallanViewData([DataSourceRequest] DataSourceRequest request, string ChallnNo, int month, int year, int officeTypeId, int OfficeId, string ChallanDate)
        {
            try
            {
                StringBuilder andCondition = new StringBuilder();
                if(ChallnNo !="")
                andCondition.Append(" AND ems.ChallnNo=" + ChallnNo);
                andCondition.Append(" AND ems.officeTypeId=" + officeTypeId);
                andCondition.Append(" AND ems.OfficeId=" + OfficeId);
                if(ChallanDate !="")
                andCondition.Append(" AND ems.ChallanDate=" + ChallanDate);
                andCondition.Append(" AND ems.SalaryYear=" + year);
                andCondition.Append(" AND ems.SalaryMonth=" + month);
                andCondition.Append(" AND ems.IsSendForApproval=1");
                andCondition.Append(" AND ems.IsApproved=1");
                andCondition.Append(" AND ems.IsRejected=0");

                //get emaployee monthly salary preview from prl.EmployeeMonthlySalary using view
                var empMonthlySalarys = GetSalarySummaryPreview_Challan(andCondition);

                DataSourceResult result = empMonthlySalarys.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult SalarySummaryPreviewBeforeSendForApproval([DataSourceRequest] DataSourceRequest request, int year, int month)
        {
            try
            {
                StringBuilder andCondition = new StringBuilder();
                andCondition.Append(" AND ems.SalaryYear=" + year);
                andCondition.Append(" AND ems.SalaryMonth=" + month);
                andCondition.Append(" AND ems.IsSendForApproval=0");
                andCondition.Append(" AND ems.IsApproved=0");
                andCondition.Append(" AND ems.IsRejected=0");

                //get emaployee monthly salary preview from prl.EmployeeMonthlySalary using view
                var empMonthlySalarys = GetSalarySummaryPreview(andCondition);

                DataSourceResult result = empMonthlySalarys.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult SalarySummaryPreviewAfterSendForApproval([DataSourceRequest] DataSourceRequest request, int year, int month)
        {
            try
            {
                StringBuilder andCondition = new StringBuilder();
                andCondition.Append(" AND ems.SalaryYear=" + year);
                andCondition.Append(" AND ems.SalaryMonth=" + month);
                andCondition.Append(" AND ems.IsSendForApproval=1");
                andCondition.Append(" AND ems.IsApproved=0");
                andCondition.Append(" AND ems.IsRejected=0");
                var empMonthlySalarys = GetSalarySummaryPreview(andCondition);

                DataSourceResult result = empMonthlySalarys.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult SalarySummaryPreviewAfterSendForApproval2([DataSourceRequest] DataSourceRequest request, int year, int month)
        {
            try
            {
                StringBuilder andCondition = new StringBuilder();
                andCondition.Append(" AND ems.SalaryYear=" + year);
                andCondition.Append(" AND ems.SalaryMonth=" + month);
                andCondition.Append(" AND ems.IsSendForApproval=1");
                andCondition.Append(" AND ems.IsApproved=0");
                andCondition.Append(" AND ems.IsRejected=0");
                var empMonthlySalarys = GetSalarySummaryPreview(andCondition);

                DataSourceResult result = empMonthlySalarys.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult SalarySendForApproval(int Year, int Month)
        {
            string message = "";
            try
            {
                StringBuilder andCondition = new StringBuilder();
                andCondition.Append(" AND ems.SalaryYear=" + Year);
                andCondition.Append(" AND ems.SalaryMonth=" + Month);
                andCondition.Append(" AND ems.IsSendForApproval=0");
                andCondition.Append(" AND ems.IsApproved=0");
                andCondition.Append(" AND ems.IsRejected=0");

                //get employee monthly from prl.EmployeeMonthlySalary when IsSendForApproval=0
                var existingMonthlySalary = GetExistingMonthlySalary(andCondition);//Method 01

                if (!existingMonthlySalary.Any(p => p.IsSendForApproval == false))
                    return Json("No Pending Salary Approval", JsonRequestBehavior.AllowGet);

                var param = new { SalaryYear = Year, SalaryMonth = Month, UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID) };

                //let's update prl.EmployeeMonthlySalary as IsSendForApproval=1
                employeeSPService.GetDataWithParameter(param, "prl.Update_SalarySendForApproval");
                message = "Send for Final Approval Done";
            }
            catch (Exception e)
            {
                message = "Error Occured";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MonthlySalaryApproveProcess(int salaryMonth, int salaryYear, string salaryApprovalDate)
        {
            bool isOperationSuccess = true;
            try
            {
                string result = "";
                var salaryList = new List<EmployeeMonthlySalaryModel>();

                //get "IS SEND FOR APPROVAL" for appr employee monthly salary for this month and year from [EmployeeMonthlySalary]
                salaryList = GetEmployeeMonthlySalaryForThisYearAndMonth(salaryMonth, salaryYear);

                if (!salaryList.Any())
                    return Json("No Pending Approval Found, Process Denied", JsonRequestBehavior.AllowGet);

                if (Convert.ToDateTime(salaryApprovalDate) < Convert.ToDateTime(salaryList[0].SalaryDate))
                    return Json("Salary Approval Date can not be smaller then salary Generation Date, Process Denied", JsonRequestBehavior.AllowGet);

                using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required, new System.TimeSpan(0, 30, 0)))
                {
                    try
                    {
                        var lstPRSalaryRegister = new List<PRSalaryRegister>();
                        var monthlySalaryApprovedList = new List<EmployeeMonthlySalaryApproved>();
                        var pfBackDataSetList = new List<EmployeeMonthlySalaryModel>();

                        //get provident fund enabled components from [prl.PRComponent]
                        var componentList = prComponentService.GetMany(p => p.IsActive == true && (p.IsProvidentFundComponent == true)).ToList();

                        //get employee monthly salary for "PF BackDate Deduction" and IsSendForApproval=1 from [EmployeeMonthlySalary]                        
                        pfBackDataSetList = GetEmployeeMonthlySalaryByPFBackDateDeductionAndIsSendForApproval(salaryMonth, salaryYear);

                        foreach (var item in salaryList)
                        {
                            var salary = new EmployeeMonthlySalaryApproved();
                            item.IsApproved = true;

                            // Inserting Salary data to salary Approved Table prl.EmployeeMonthlySalaryApproved
                            salary.SalaryId = item.SalaryId;
                            salary.SalaryMonth = item.SalaryMonth;
                            salary.SalaryYear = item.SalaryYear;
                            salary.SalaryDate = Convert.ToDateTime(salaryApprovalDate);
                            salary.EmployeeId = item.EmployeeId;
                            salary.PRSalaryConfigurationId = item.PRSalaryConfigurationId;
                            salary.PRComponentId = item.PRComponentId;

                            salary.OfficeId = item.OfficeId;
                            salary.PRComponentAmount = item.PRComponentAmount;
                            salary.ComponentCategory = item.ComponentCategory;
                            salary.TransactionType = item.TransactionType;
                            salary.IsActive = item.IsActive;

                            salary.OfficeTypeId = item.OfficeTypeId;
                            salary.DesignationId = item.DesignationId;
                            salary.DepartmentId = item.DepartmentId;
                            salary.EmployeeStatusId = item.EmployeeStatusId;
                            salary.BankCode = item.BankName;

                            salary.IsApproved = true;
                            salary.CreateDate = DateTime.Now;
                            salary.CreatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                            salary.UpdateDate = DateTime.Now;
                            salary.UpdatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);

                            monthlySalaryApprovedList.Add(salary);

                            var checkProvidentFundComponent = componentList.FirstOrDefault(p => p.PRComponentID == salary.PRComponentId);

                            if (checkProvidentFundComponent != null)
                            {
                                //populate Salary Register object for prl.PRSalaryRegister table
                                var entity = new PRSalaryRegister();
                                entity.SalaryYear = salaryYear;
                                entity.SalaryMonth = salaryMonth;
                                entity.SalaryDate = salary.SalaryDate;
                                entity.PRSalaryConfigurationID = salary.PRSalaryConfigurationId == null ? 0 : salary.PRSalaryConfigurationId;
                                entity.OfficeID = salary.OfficeId;
                                entity.EmployeeID = salary.EmployeeId;
                                entity.PRComponentID = salary.PRComponentId;

                                if (pfBackDataSetList.Any(p => p.EmployeeId == salary.EmployeeId))
                                {
                                    decimal backDateAmount = 0;

                                    if (checkProvidentFundComponent.ComponentName == ComponentPayrollConstants.Salary_PFEmployeeDeduction
                                        || checkProvidentFundComponent.ComponentName == ComponentPayrollConstants.Salary_PFOfficeContribution)
                                    {
                                        var pfBackDateDeduction = pfBackDataSetList.FirstOrDefault(p => p.EmployeeId == salary.EmployeeId);

                                        if (pfBackDateDeduction != null)
                                        {
                                            backDateAmount = pfBackDateDeduction.PRComponentAmount;
                                            entity.ComponentAmount = salary.PRComponentAmount + backDateAmount;
                                        }
                                        else
                                            entity.ComponentAmount = salary.PRComponentAmount;
                                    }
                                }
                                else
                                    entity.ComponentAmount = salary.PRComponentAmount;
                                entity.PRTranTypeID = 0;
                                entity.IsPosted = true;
                                entity.IsActive = true;
                                entity.CreateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                                entity.UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                                entity.CreateDate = DateTime.UtcNow;
                                entity.UpdateDate = DateTime.UtcNow;
                                entity.OfficeID = salary.OfficeId;
                                entity.ComponentName = checkProvidentFundComponent.ComponentName;

                                lstPRSalaryRegister.Add(entity);
                            }
                        }

                        //let's insert into [prl.EmployeeMonthlySalaryApproved]
                        employeeMonthlySalaryApprovedService.AddEmployeeMonthlyApprovedList(monthlySalaryApprovedList);



                        //let's insert into [prl.PRSalaryRegister]
                        prSalaryRegisterService.AddEmployeeMonthlySalaryRegister(lstPRSalaryRegister);

                        //let's update [EmployeeMonthlySalary] to set IsApproved=1
                        var paramUpdate = new { SalaryYear = salaryYear, SalaryMonth = salaryMonth, UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID) };
                        employeeSPService.GetDataWithParameter(paramUpdate, "prl.Update_SalaryApprove");

                        var firstDayOfMonth = new DateTime(salaryYear, salaryMonth, 1);
                        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                        var param = new { EffectiveStartDate = firstDayOfMonth, EffectiveEndDate = lastDayOfMonth, UpdateBy = Convert.ToInt64(LoggedInEmployeeId.ToString()) };

                        //let's update [prl.EmployeeMonthlySalaryException] to set IsApproved=1
                        employeeSPService.GetDataWithParameter(param, "prl.Update_SalaryExceptionApproval");

                        //bool hasPF = false;
                        //if (ConfigurationManager.AppSettings["HasPF"] != null)
                        //    hasPF = bool.Parse(ConfigurationManager.AppSettings["HasPF"].ToString());
                        //if (hasPF)
                        //{
                        //    var pf = new gHRMDBContext().Database.ExecuteSqlCommand("[dbo].[sp_PFContribution_LoanCollection] " + salaryMonth + "," + salaryYear + "," + LoggedInEmployeeId + ",'" + salaryApprovalDate + "'");
                        //}

                        if(SessionHelper.CompanyInfo.CompanyShortName == "Prottyashi")
                        {
                                var pf = new gHRMDBContext().Database.ExecuteSqlCommand("[dbo].[sp_PFContribution_LoanCollection] " + salaryMonth + "," + salaryYear + "," + LoggedInEmployeeId + ",'" + salaryApprovalDate + "'");
                        }

                        using (gHRMDBContext db = new gHRMDBContext())
                        {
                            // Provident Fund
                            var pf_Obj = db.OrganizationPFSetups.Where(x => x.IsActive);
                            if (pf_Obj.Any())
                            {
                                var pf_lst = (from pf in db.OrganizationPFSetups
                                              join self in db.PRComponents on pf.SelfContribution_ComponentPayrollId equals self.ComponentPayrollId into ps
                                              from self in ps.DefaultIfEmpty()
                                              join ofc in db.PRComponents on pf.OfficeContribution_ComponentPayrollId equals ofc.ComponentPayrollId into po
                                              from ofc in po.DefaultIfEmpty()
                                              where pf.IsActive
                                              select new
                                              {
                                                  Self_PRComponentID = self.PRComponentID,
                                                  Self_IsActive = self.IsActive,
                                                  Office_PRComponentID = ofc.PRComponentID,
                                                  Off_IsActive = ofc.IsActive,
                                              }).Distinct().ToList();
                                if (pf_lst.Any())
                                {
                                    int[] selfArr = pf_lst.Where(x => x.Self_IsActive && x.Self_PRComponentID > 0).Select(s => s.Self_PRComponentID).Distinct().ToArray();
                                    int[] OfcArr = pf_lst.Where(x => x.Off_IsActive && x.Office_PRComponentID > 0).Select(s => s.Office_PRComponentID).Distinct().ToArray();
                                    var emp_Lst = monthlySalaryApprovedList.Where(x => selfArr.Contains(x.PRComponentId) || OfcArr.Contains(x.PRComponentId)).Select(x => x.EmployeeId).Distinct();
                                    foreach (var e in emp_Lst)
                                    {
                                        ContributionRegister con_Obj = new ContributionRegister()
                                        {
                                            CreateDate = DateTime.UtcNow,
                                            CreateUser = (int)LoggedInEmployeeId,
                                            EmployeeId = e,
                                            TransactionDate = monthlySalaryApprovedList[0].SalaryDate,
                                            IsDeleted = false,
                                            TransactionType = PFTransactionTypeConstants.Contribution,
                                        };
                                        if (monthlySalaryApprovedList.Where(x => x.EmployeeId == e && selfArr.Contains(x.PRComponentId)).Any())
                                            con_Obj.SelfContribution = monthlySalaryApprovedList.Where(x => x.EmployeeId == e && selfArr.Contains(x.PRComponentId)).First().PRComponentAmount;

                                        if (monthlySalaryApprovedList.Where(x => x.EmployeeId == e && OfcArr.Contains(x.PRComponentId)).Any())
                                            con_Obj.OrgContribution = monthlySalaryApprovedList.Where(x => x.EmployeeId == e && OfcArr.Contains(x.PRComponentId)).First().PRComponentAmount;
                                        db.ContributionRegisters.Add(con_Obj);
                                        db.SaveChanges();
                                    }
                                }
                            }

                            //  Co-Operative Ledger insert
                            string query = $@"select s.EmployeeId,coo.Id AS SummaryID,s.PRComponentAmount 
                                        from prl.EmployeeMonthlySalary s INNER JOIN prl.PRComponent c on s.PRComponentId=c.PRComponentID
                                        INNER JOIN coo.CooperativeSummaryConfiguration coo on c.ComponentPayrollId=coo.ComponentId and coo.EmployeeId=s.EmployeeId
                                        where s.IsActive=1 and IsRejected=0 and IsApproved=0 and coo.ActivityStatus='A' and coo.EndDate is null AND s.SalaryMonth={monthlySalaryApprovedList[0].SalaryMonth} and s.SalaryYear={monthlySalaryApprovedList[0].SalaryYear}";
                            var cooperativeDataLst = db.Database.SqlQuery<CooperativeDataViewModel>(query).ToList();
                            if (cooperativeDataLst != null)
                            {
                                foreach (var l in cooperativeDataLst)
                                {
                                    CooperativeLedger obj = new CooperativeLedger()
                                    {
                                        CreateBy = (int)LoggedInEmployeeId,
                                        CreateDate = DateTime.UtcNow,
                                        Credit = l.PRComponentAmount ?? 0,
                                        Date = monthlySalaryApprovedList[0].SalaryDate,
                                        Debit = 0,
                                        InstallmentMonth = monthlySalaryApprovedList[0].SalaryMonth,
                                        InstallmentYear = monthlySalaryApprovedList[0].SalaryYear,
                                        InstallmentType = CoOperativeConstants.InstallmentType_Installment,
                                        SummaryMasterId = l.SummaryID ?? 0
                                    };
                                    cooperativeLedgerService.Create(obj);
                                }
                            }
                            // Loan
                            var loanLst = salaryList.Where(x => x.ComponentCategory == "Loan" && (x.LoanId ?? 0) > 0).ToList();
                            if (loanLst.Any())
                            {
                                loanLst.ForEach(x => x.CreateUser = LoggedInEmployeeId);
                                new LoanCalculationService().EmployeeMonthlySalaryApprovedProcess2(loanLst, db);
                            }
                        }


                        result = "Salary Approval Successfull";
                    }
                    catch (DbEntityValidationException ex)
                    {
                        isOperationSuccess = false;
                        result = "There was an error on Salary Approval!";
                        // Retrieve the error messages as a list of strings.
                        var errorMessages = ex.EntityValidationErrors
                                .SelectMany(x => x.ValidationErrors)
                                .Select(x => x.ErrorMessage);

                        // Join the list to a single string.
                        var fullErrorMessage = string.Join("; ", errorMessages);

                        // Combine the original exception message with the new one.
                        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                    }
                    catch (Exception ex)
                    {

                        isOperationSuccess = false;

                        // Find deepest inner exception
                        Exception inner = ex;
                        while (inner.InnerException != null)
                            inner = inner.InnerException;

                        result = "Error: " + inner.Message;

                        // Optional: log more context if you have a logger
                        System.Diagnostics.Debug.WriteLine(inner.StackTrace);

                        isOperationSuccess = false;
                        result = "There was an error on Salary Approval!";
                    }

                    if (isOperationSuccess)
                        tran.Complete();

                    tran.Dispose();
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException.ToString(), JsonRequestBehavior.AllowGet);
                throw;
            }
        }

        public JsonResult MonthlySalaryApprovalReject(int salaryMonth, int salaryYear, string salaryApprovalDate)
        {
            string result = "";
            bool isOperationSuccess = true;

            StringBuilder andCondition = new StringBuilder();
            andCondition.Append("AND ems.SalaryYear=" + salaryYear);
            andCondition.Append("AND ems.SalaryMonth=" + salaryMonth);
            andCondition.Append(" AND ems.IsSendForApproval=1");
            andCondition.Append(" AND ems.IsApproved=0");
            andCondition.Append(" AND ems.IsRejected=0");

            //get employee monthly salary [prl.EmployeeMonthlySalary]
            var salaryList = GetExistingMonthlySalary(andCondition);//Method 01
            if (!salaryList.Any())
            {
                result = "No Pending Approval";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (Convert.ToDateTime(salaryApprovalDate) < Convert.ToDateTime(salaryList[0].UpdateDate.Date))
            {
                result = "Salary Rejection Date can not be smaller then salary send for approval date, Process Denied";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    var firstDate = new DateTime(Convert.ToInt32(salaryYear), Convert.ToInt32(salaryMonth), 1);
                    DateTime firstOfNextMonth = new DateTime(Convert.ToInt32(salaryYear), Convert.ToInt32(salaryMonth), 1).AddMonths(1);
                    var lastDate = firstOfNextMonth.AddDays(-1);

                    //let's insert into EmployeeMonthlySalaryHistory
                    //then let's delete prl.EmployeeMonthlySalary for and IsSendForApproval=1 and IsApproved = 0
                    InsertMonthlySalaryHistoryRejected(salaryYear, salaryMonth);

                    //let's update prl.EmployeeMonthlySalaryException for IsActive=0 
                    InActiveExceptionalSalaryDetailRejected(firstDate, lastDate);

                    result = "Salary Rejection Successfull";
                }
                catch
                {
                    isOperationSuccess = false;
                    result = "Could not Approve, Error Occured";
                }

                if (isOperationSuccess)
                    tran.Complete();

                tran.Dispose();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult HoldSalary(int EmployeeId, int Year, int Month)
        {
            var result = 0;
            var message = "";
            try
            {
                if (EmployeeId > 0)
                {
                    //var checkSalaryStatus = employeeMonthlySalaryService.GetAll().Where(p => p.EmployeeId == EmployeeId && p.IsActive == true && p.SalaryMonth == Month && p.SalaryYear == Year).ToList();

                    if (employeeMonthlySalaryApprovedService.CheckAlreadyApprovedSalary(Month, Year).Any())
                    {
                        message = "This Month Salary is Already Approved, Salary Hold Denied";
                        return Json(new { result = 0, message = message }, JsonRequestBehavior.AllowGet);
                    }

                    var paramG = new { EmployeeId = Convert.ToInt64(EmployeeId), SalaryYear = Year, SalaryMonth = Month };
                    var listBA = employeeSPService.GetDataWithParameter(paramG, "prl.SP_PR_SalaryHoldForEmployee");

                    var checkSalaryStatus = listBA.Tables[0].AsEnumerable().Select(row => new EmployeeMonthlySalary()
                    {
                        SalaryMonth = row.Field<int>("SalaryMonth"),
                        SalaryYear = row.Field<int>("SalaryYear"),
                        EmployeeId = row.Field<long>("EmployeeId"),
                        SalaryId = row.Field<int>("SalaryId"),
                    }).ToList();

                    if (checkSalaryStatus.Where(p => p.IsApproved == true).ToList().Any())
                    {
                        result = 0;
                        message = "Salary already approved, Hold Denied";
                        //return Json("Salary already approved, Hold Denied", JsonRequestBehavior.AllowGet);
                    }
                    else if (checkSalaryStatus.Where(p => p.IsSendForApproval == true).ToList().Any())
                    {
                        result = 0;
                        message = "Salary Send For Approval, Hold Denied";
                        //return Json("Salary Send For Approval, Hold Denied", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var param = new { EmployeeId = EmployeeId };
                        employeeSPService.GetDataWithParameter(param, "prl.SP_HoldSalaryForEmployee");
                        result = 1;
                    }
                }
            }
            catch (Exception e)
            {
                result = 0;
                message = e.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult HoldSalaryFinalApproval(int EmployeeId, int Year, int Month)
        {
            var result = 0;
            var message = "";
            try
            {
                if (EmployeeId > 0)
                {
                    var checkSalaryStatus = employeeMonthlySalaryService.GetForHoldSalary(EmployeeId, Month, Year);
                    if (checkSalaryStatus.Where(p => p.IsApproved == true).ToList().Any())
                    {
                        result = 0;
                        message = "Salary already approved, Hold Denied";
                        //return Json("Salary already approved, Hold Denied", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var param = new { EmployeeId = EmployeeId };
                        employeeSPService.GetDataWithParameter(param, "prl.SP_HoldSalaryForEmployee");
                        result = 1;
                    }

                }

            }
            catch (Exception e)
            {
                result = 0;
                message = e.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DailyDataPosting(string empType, string month, string Year1, string Process1Date)
        {
            string result = "";
            try
            {
                Int64 CreateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                DateTime CreateDate = DateTime.Now;

                var OfficeID = (int)LoggedInOfficeID;
                var OfficeTypeID = (int)LoggedInOfficeType;

                var param = new
                {
                    SalaryYear = Year1,
                    SalaryMonth = month,
                    PostingDate = Process1Date,
                    OfficeTypeID = OfficeTypeID,
                    OfficeID = OfficeID,
                    PRTranTypeID = empType,
                    CreateUser = CreateUser,
                    CreateDate = CreateDate,
                    UpdateUser = CreateUser,
                    UpdateDate = CreateDate,
                    intErrorCode = 0
                };
                var val = employeeSPService.GetDataWithParameter(param, "prl.SP_PR_SET_DailyPosting");
                result = "Daily Posting Successfull";
            }
            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRelatedOfficeXOfficeType(int OfficeTypeId)
        {
            PRWorkAreaViewModel model = new PRWorkAreaViewModel();
            var pleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var loggedInOffice = officeService.GetById((int)LoggedInOfficeID);
            if (loggedInOffice != null)
            {
                model.OfficeName = loggedInOffice.OfficeName;
                model.OfficeCode = loggedInOffice.OfficeCode;
            }
            PopulateOfficeDropdownListNew(model, pleaseSelect, OfficeTypeId);
            return Json(model.OfficeList, JsonRequestBehavior.AllowGet);
        }
        #endregion

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
                            return paryrollCondition = false;
                    }
                    else
                        return paryrollCondition = false;
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
                        return false;
                }
            }

            return true;
        }

        //Method 03
        public List<PRSalaryConfigurationViewModel> GetCurrentMonthSalaryApplicableEmployee(DateTime firstDate, DateTime lastDate, int OfficeTypeId)
        {
            var param = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeId = OfficeTypeId };
            var salaryApplicableDS = employeeSPService.GetDataWithParameter(param, "prl.SP_GET_PREmployeeSalaryCurrentConfigurationAllEmployee");

            try
            {
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
                    SalaryRoundType = row.Field<string>("SalaryRoundType"),
                    ComponentName = row.Field<string>("ComponentName")
                }).ToList();

                return salaryconfigurations;
            }
            catch(Exception ex)
            {
                var tt = ex.Message;
                return null;
            }
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
        private void InsertLoansInMonthlySalary(List<LoanInstallmentDetail> loanEmployees, string month, string salaryYear, string salaryDate, List<PRComponent> components,
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
            //return loanForSalary;
            if (loanForSalary.Any())
            {
                employeeMonthlySalaryService.AddEmployeeMonthlySalaryList(loanForSalary);
            }

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
        private void InsertRegularIncentives(List<EmployeeSalaryIncentive> salaryIncentives, string month, string salaryYear, string salaryDate, List<PRComponent> components, List<PRSalaryConfigurationViewModel> salaryconfigurations)
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

            //let's add employee monthly salary
            if (objEmployeeMonthlySalaries.Any())
                employeeMonthlySalaryService.AddEmployeeMonthlySalaryList(objEmployeeMonthlySalaries);
        }

        // Method 16
        private void InsertRegularSalaryDeduction(List<EmployeeSalaryDeduction> salaryDiductions, string month, string salaryYear, string salaryDate, List<PRComponent> components, List<PRSalaryConfigurationViewModel> salaryconfigurations)
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

            employeeMonthlySalaryService.AddEmployeeMonthlySalaryList(objEmployeeMonthlySalaries);
        }

        // Method 17
        private List<EmployeeMonthlySalary> InsertNewMonthlySalary(List<PRSalaryConfigurationViewModel> salaryconfigurations
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
                var empSalInformation = salaryconfigurations.Where(p => p.EmployeeID == item.EmployeeID && p.IsActive == true).FirstOrDefault();

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

                double positiveImpactAmount = Convert.ToDouble(positives.Sum(p => p.PRComponentAmount));
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

                if (grossOrBasicSalary != grossOrBasicChangesAmount)
                {
                    //Re-generate temporary salary for the month
                    tempSalaryList = ReGenerateTemporarySalaryForTheMonth(employeeTypeId, employeeStatusId, grossOrBasicChangesAmount,
                                                    grossOrBasicSalary, employeeOfficeLocationid, PFTypeId, officeId, item.EmployeeID, month, salaryYear);  // Method 17-4

                    tempSalaryConfiguration = ReGenerateEmployeeSalaryForPositiveOrNegativeImpact(tempSalaryList, firstDate, lastDate,
                                                    empSalInformation);  // Method 17-5
                }

                //partial deduction Salary means: employee information with [PRSalaryConfiguration]
                var checkPartialGeneration = partialDeductionSalary.FirstOrDefault(p => p.EmployeeId == item.EmployeeID);

                #region Generate Salary when salary is configured after the first date of month

                // Generate Salary when salary is configured after the first date of month
                if (checkPartialGeneration != null)
                {
                    var dateDifference = 0;
                    var firstDayOfSalary = checkPartialGeneration.FirstJoiningDate.Day;

                    if (lastDate.Day <= 30)
                        dateDifference = (30 - firstDayOfSalary) + 1;
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

            employeeMonthlySalaryExceptionService.AddEmplyoeeSalaryExceptionList(empSalaryExceptions);
            employeeMonthlySalaryService.AddEmployeeMonthlySalaryList(objEmployeeMonthlySalaries);

            return objEmployeeMonthlySalaries;
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

        //private void InsertLoanTemporary(int salarymonth, int salaryYear, DateTime salaryDate
        //    , List<TempPFCollection> pf_modelLst, List<EmployeeMonthlySalary> employeeMonthSalaryLst, List<PRComponent> components)
        //{
        //    //  employeeMonthSalaryLst = employeeMonthSalaryLst.Where(x => x.EmployeeId == 29).ToList();

        //    var purposeLst = loanPurposeService.GetMany(x => x.IsActive);
        //    var componentLst = components.Where(x => x.ComponentCategory == "Loan");
        //    var loanCalculation = new gHRMDBContext().LoanCalculation.Where(x => (x.IsActive ?? false));

        //    if (purposeLst.Any() && componentLst.Any())
        //    {
        //        List<EmployeeMonthlySalary> objEmpMonthlySalary = new List<EmployeeMonthlySalary>();
        //        var disburseLst = loanDisbursementService.GetMany(x => !(x.IsDeleted ?? false) && !x.IsClose).OrderBy(x => x.EmployeeId);
        //        var empLst = (from e in employeeMonthSalaryLst
        //                      join d in disburseLst on e.EmployeeId equals d.EmployeeId
        //                      select e.EmployeeId).Distinct().OrderBy(x => x).ToList();
        //        var employeeLst = employeeService.GetMany(x => empLst.Contains(x.EmployeeId));
        //        var loanidLst = disburseLst.Select(x => x.LoanId).Distinct();
        //        var collectionLastDateLstFromLoanRegister = new gHRMDBContext().LoanRegister.Where(x => loanidLst.Contains(x.LoanId) && !(x.IsDeleted ?? false))
        //            .GroupBy(g => g.LoanId)
        //            .Select(s => new
        //            {
        //                LoanId = s.Key,
        //                TransactionDate = s.Max(x => x.TransactionDate),
        //                PaidAmount = s.Sum(x => x.LoanAmount),
        //                PaidInterestAmount = s.Sum(x => x.InterestAmount),
        //                InterestCharge = s.Sum(x => x.InterestCharge)
        //            });

        //        foreach (var emp in employeeLst)
        //        {
        //            foreach (var d in disburseLst.Where(x => x.EmployeeId == emp.EmployeeId))
        //            {
        //                //var purpose = purposeLst.First(x => x);
        //                var loancomponent = (from c in componentLst
        //                                     join l in loanCalculation on c.LoanCalculationId equals l.LoanCalculationId
        //                                     join p in purposeLst on c.ComponentName equals p.PurposeName
        //                                     where p.PurposeId == d.PurposeId //&& c.EmployeeStatusId == emp.EmployeeStatusId
        //                                     && c.EmployeeTypeId == emp.EmployeeTypeId
        //                                     select new
        //                                     {
        //                                         LoanCalculationName = l.LoanCalculationName,
        //                                         ComponentName = c.ComponentName,
        //                                         TransactionType = c.TransactionType,
        //                                         PRComponentID = c.PRComponentID,
        //                                         PurposeName = p.PurposeName,
        //                                         LoanType = p.LoanType,
        //                                         ComponentCategory = c.ComponentCategory
        //                                     });
        //                if (loancomponent.Any())
        //                {

        //                    decimal previousPrincipal = 0, presentPrincipal = 0, preInterestAmt = 0, presentInterestAmt = 0;
        //                    decimal preCharge = 0, pressentCharge = 0;

        //                    var com = loancomponent.First();

        //                    #region Collection Method wise Calculation
        //                    if (com.LoanCalculationName == "Amortization")
        //                    {
        //                        //var monthlyInterest = Math.Round((((d.DisburseAmount * d.IntersetRate) / 100) / d.NoOfInstallment), 2);
        //                        //var monthlyInstallment = monthlyPrincipal + monthlyInterest;
        //                    }
        //                    else if (com.LoanCalculationName == "Classic") { }
        //                    else if (com.LoanCalculationName == "Decline")
        //                    {
        //                        DateTime lastCollectionDate = salaryDate;

        //                        if (collectionLastDateLstFromLoanRegister.Any())
        //                        {
        //                            if (collectionLastDateLstFromLoanRegister.Where(x => x.LoanId == d.LoanId).Any())
        //                            {
        //                                lastCollectionDate = collectionLastDateLstFromLoanRegister.First(x => x.LoanId == d.LoanId).TransactionDate;
        //                                previousPrincipal = collectionLastDateLstFromLoanRegister.First(x => x.LoanId == d.LoanId).PaidAmount;
        //                                preInterestAmt = collectionLastDateLstFromLoanRegister.First(x => x.LoanId == d.LoanId).PaidInterestAmount;
        //                                preCharge = collectionLastDateLstFromLoanRegister.First(x => x.LoanId == d.LoanId).InterestCharge ?? 0;
        //                            }
        //                            else lastCollectionDate = d.DisburseDate;
        //                        }
        //                        else lastCollectionDate = d.DisburseDate;

        //                        var previousInstallment = previousPrincipal + preInterestAmt;

        //                        var totalDays = (int)(salaryDate - lastCollectionDate).TotalDays;
        //                        var monthlyPrincipal = Math.Round(Convert.ToDecimal(d.DisburseAmount / d.NoOfInstallment));
        //                        //var monthlyInterest = Math.Round((((d.DisburseAmount * d.IntersetRate) / 100) / d.NoOfInstallment),2);
        //                        //var monthlyInstallment = monthlyPrincipal + monthlyInterest;

        //                        presentPrincipal = (int)(((d.DisburseAmount - previousPrincipal) >= monthlyPrincipal) ? monthlyPrincipal : (d.DisburseAmount - previousPrincipal));

        //                        pressentCharge = Math.Round((presentPrincipal == 0 ? 0 : ((d.DisburseAmount - previousPrincipal) * d.IntersetRate * totalDays) / 36500), 2);

        //                        presentInterestAmt = presentPrincipal == monthlyPrincipal ? 0
        //                            : ((preCharge - preInterestAmt) >= monthlyPrincipal ? monthlyPrincipal : (preCharge - preInterestAmt));
        //                    }
        //                    else if (com.LoanCalculationName == "Flat")
        //                    {
        //                        //var monthlyInterest = Math.Round((((d.DisburseAmount * d.IntersetRate) / 100) / d.NoOfInstallment),2);
        //                        //var monthlyInstallment = monthlyPrincipal + monthlyInterest;
        //                    }
        //                    #endregion Collection Method wise Calculation

        //                    #region Tempory Table
        //                    if (pf_modelLst.Where(x => x.EmployeeId == d.EmployeeId).Any())
        //                    {
        //                        foreach (var pf in pf_modelLst.Where(x => x.EmployeeId == d.EmployeeId))
        //                        {
        //                            if (d.LoanType == "PF")
        //                            {
        //                                pf.PFLoanID = d.LoanId;
        //                                pf.PFLoanPrincipalColl = presentPrincipal;
        //                                pf.PFLoanInterestCharge = pressentCharge;
        //                                pf.PFLoanInterestColl = preInterestAmt;
        //                            }
        //                            if (d.LoanType == "Cl")
        //                            {
        //                                pf.CLLoanID = d.LoanId;
        //                                pf.CLLoanPrincipalColl = presentPrincipal;
        //                                pf.CLLoanInterestCharge = pressentCharge;
        //                                pf.CLLoanInterestColl = preInterestAmt;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        TempPFCollection nonPF = new TempPFCollection()
        //                        {
        //                            CLLoanCollection = presentInterestAmt + presentPrincipal,
        //                            CLLoanID = d.LoanId,
        //                            CLLoanPrincipalColl = presentPrincipal,
        //                            CLLoanInterestColl = presentInterestAmt,
        //                            CLLoanInterestCharge = pressentCharge,
        //                            EmployeeId = d.EmployeeId,
        //                            PFDistributionDate = salaryDate,
        //                            PFDistributionMonth = salarymonth,
        //                            PFDistributionYear = salaryYear
        //                        };
        //                        pf_modelLst.Add(nonPF);
        //                    }
        //                    #endregion Tempory Table

        //                    #region Salary Impact
        //                    EmployeeMonthlySalary loan = new EmployeeMonthlySalary()
        //                    {
        //                        SalaryMonth = salarymonth,
        //                        SalaryYear = salaryYear,
        //                        SalaryDate = salaryDate,
        //                        EmployeeId = d.EmployeeId,
        //                        PRComponentId = com.PRComponentID,
        //                        PRComponentAmount = presentPrincipal + presentInterestAmt,
        //                        IsActive = true,
        //                        IsApproved = false,
        //                        ComponentCategory = com.ComponentCategory,
        //                        TransactionType = com.TransactionType,
        //                        CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID),
        //                        UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID),
        //                        CreateDate = DateTime.Today,
        //                        UpdateDate = DateTime.Today,
        //                        OfficeId = emp.OfficeId
        //                    };
        //                    objEmpMonthlySalary.Add(loan);
        //                    #endregion Salary Impact
        //                }
        //            }
        //        }
        //        if (objEmpMonthlySalary.Any())
        //            employeeMonthlySalaryService.AddEmployeeMonthlySalaryList(objEmpMonthlySalary);
        //    }
        //}
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

        #region Dropdown Methods

        private void MapIndexDropdown(PRWorkAreaViewModel model)
        {
            var pleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };

            //Populate office types
            PopulateOfficeTypesNew(model, pleaseSelect);

            //Populate offices
            PopulateOfficeDropdownListNew(model, pleaseSelect, SessionHelper.LoggedInOfficeTypeId);
        }


        private void MapDropDownList2(PRWorkAreaViewModel entity)
        {
            var PleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var yearList = new List<SelectListItem>();
            yearList.Add(PleaseSelect);
            for (int i = DateTime.Now.Year; i >= (DateTime.Now.Year) - 1; i--)
            {
                yearList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            entity.YearList = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(PleaseSelect);
            for (var i = 1; i <= 12; i++)
            {
                monthList.Add(new SelectListItem { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Value = i.ToString() });
            }
            entity.MonthList = monthList;

            var branchList = new List<SelectListItem>();
            branchList.Add(PleaseSelect);
            entity.BranchList = branchList;

            var accountList = new List<SelectListItem>();
            accountList.Add(PleaseSelect);
            entity.AccountList = accountList;

            var salaryTypeList = new List<SelectListItem>();
            salaryTypeList.Add(PleaseSelect);
            salaryTypeList.Add(new SelectListItem() { Text = "Salary", Value = "Salary" });
            salaryTypeList.Add(new SelectListItem() { Text = "Bonus for Eid-ul-Fitre", Value = "Bonus for Eid-ul-Fitre" });
            salaryTypeList.Add(new SelectListItem() { Text = "Bonus for Eid-ul-Azha", Value = "Bonus for Eid-ul-Azha" });
            salaryTypeList.Add(new SelectListItem() { Text = "Incentive", Value = "Incentive" });
            entity.SalaryTypeList = salaryTypeList;

            var componentNameList = new List<SelectListItem>();
            componentNameList.Add(PleaseSelect);
            entity.ComponentNameList = componentNameList;

            var componentList = new List<SelectListItem>();
            componentList.Add(PleaseSelect);
            componentList.Add(new SelectListItem() { Text = "Incentive", Value = "In" });
            componentList.Add(new SelectListItem() { Text = "Deduction", Value = "De" });
            entity.ComponentTypeList = componentList;


            var applicationList = new List<SelectListItem>();
            applicationList.Add(PleaseSelect);
            applicationList.Add(new SelectListItem() { Text = "Fund Transfer Application", Value = "Application" });
            applicationList.Add(new SelectListItem() { Text = "Fund Transfer Advice", Value = "Advice" });
            applicationList.Add(new SelectListItem() { Text = "Component Wise Salary", Value = "Component" });
            entity.ReportTypeList = applicationList;

            if(SessionHelper.CompanyInfo.CompanyShortName == "GTT")
            {
                var param = new { OrgId = SessionHelper.CompanyInfo.CompanyShortName, OfficeId = LoginUserOfficeID, UserId = LoginUserOfficeID };
                var ReportList = employeeSPService.GetDataWithParameter(param, "SP_GET_PAYROLL_REPORT_LIST");
                var listItems = ReportList.Tables[0].AsEnumerable()
                   .Select(x => new SelectListItem
                   {
                       Value = x.Field<string>("Value"),
                       Text = x.Field<string>("Text")
                   });
                entity.ReportList = listItems;
            }
            else
            {
                var lists = new List<SelectListItem>();
                lists.Add(new SelectListItem() { Text = "Please Select", Value = "" });
                lists.Add(new SelectListItem() { Text = "Salary Before Approval (Pdf Format)", Value = "1" });
                lists.Add(new SelectListItem() { Text = "Salary Before Approval (Excel Format)", Value = "2" });
                //lists.Add(new SelectListItem() { Text = "PF Before Approval (Pdf Format)", Value = "9" });
                //lists.Add(new SelectListItem() { Text = "PF Before Approval (Excel Format)", Value = "10" });
                lists.Add(new SelectListItem() { Text = "Rejected Employees Salary (Pdf Format)", Value = "3" });
                lists.Add(new SelectListItem() { Text = "Approved Salary (Pdf Format)", Value = "4" });
                lists.Add(new SelectListItem() { Text = "Approved Salary (Excel Format)", Value = "5" });
                //lists.Add(new SelectListItem() { Text = "Approved PF (Pdf Format)", Value = "11" });
                lists.Add(new SelectListItem() { Text = "Approved Salary Group by Office(Pdf Format)", Value = "6" });
                lists.Add(new SelectListItem() { Text = "Approved Salary Group by Office (Excel Format)", Value = "7" });
                lists.Add(new SelectListItem() { Text = "Approved Salary Group by Zone Area", Value = "8" });

                lists.Add(new SelectListItem() { Text = " GC All Employee Salary Details Statement Before Approval (Pdf Format)", Value = "99" });

                entity.ReportList = lists;
            }

        }

        private void MapDropDownList(PRWorkAreaViewModel entity)
        {
            var PleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var yearList = new List<SelectListItem>();
            yearList.Add(PleaseSelect);
            for (int i = DateTime.Now.Year; i >= (DateTime.Now.Year) - 1; i--)
            {
                yearList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            entity.YearList = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(PleaseSelect);
            for (var i = 1; i <= 12; i++)
            {
                monthList.Add(new SelectListItem { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Value = i.ToString() });
            }
            entity.MonthList = monthList;

            var branchList = new List<SelectListItem>();
            branchList.Add(PleaseSelect);
            entity.BranchList = branchList;

            var accountList = new List<SelectListItem>();
            accountList.Add(PleaseSelect);
            entity.AccountList = accountList;

            var salaryTypeList = new List<SelectListItem>();
            salaryTypeList.Add(PleaseSelect);
            salaryTypeList.Add(new SelectListItem() { Text = "Salary", Value = "Salary" });
            salaryTypeList.Add(new SelectListItem() { Text = "Bonus for Eid-ul-Fitre", Value = "Bonus for Eid-ul-Fitre" });
            salaryTypeList.Add(new SelectListItem() { Text = "Bonus for Eid-ul-Azha", Value = "Bonus for Eid-ul-Azha" });
            salaryTypeList.Add(new SelectListItem() { Text = "Incentive", Value = "Incentive" });
            entity.SalaryTypeList = salaryTypeList;

            var componentNameList = new List<SelectListItem>();
            componentNameList.Add(PleaseSelect);
            entity.ComponentNameList = componentNameList;

            var componentList = new List<SelectListItem>();
            componentList.Add(PleaseSelect);
            componentList.Add(new SelectListItem() { Text = "Incentive", Value = "In" });
            componentList.Add(new SelectListItem() { Text = "Deduction", Value = "De" });
            entity.ComponentTypeList = componentList;


            var applicationList = new List<SelectListItem>();
            applicationList.Add(PleaseSelect);
            applicationList.Add(new SelectListItem() { Text = "Fund Transfer Application", Value = "Application" });
            applicationList.Add(new SelectListItem() { Text = "Fund Transfer Advice", Value = "Advice" });
            applicationList.Add(new SelectListItem() { Text = "Component Wise Salary", Value = "Component" });
            entity.ReportTypeList = applicationList;

            var lists = new List<SelectListItem>();
            lists.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            lists.Add(new SelectListItem() { Text = "Salary Before Approval (Pdf Format)", Value = "1" });
            lists.Add(new SelectListItem() { Text = "Salary Before Approval (Excel Format)", Value = "2" });
            //lists.Add(new SelectListItem() { Text = "PF Before Approval (Pdf Format)", Value = "9" });
            //lists.Add(new SelectListItem() { Text = "PF Before Approval (Excel Format)", Value = "10" });
            lists.Add(new SelectListItem() { Text = "Rejected Employees Salary (Pdf Format)", Value = "3" });
            lists.Add(new SelectListItem() { Text = "Approved Salary (Pdf Format)", Value = "4" });
            lists.Add(new SelectListItem() { Text = "Approved Salary (Excel Format)", Value = "5" });
            //lists.Add(new SelectListItem() { Text = "Approved PF (Pdf Format)", Value = "11" });
            lists.Add(new SelectListItem() { Text = "Approved Salary Group by Office(Pdf Format)", Value = "6" });
            lists.Add(new SelectListItem() { Text = "Approved Salary Group by Office (Excel Format)", Value = "7" });
            lists.Add(new SelectListItem() { Text = "Approved Salary Group by Zone Area", Value = "8" });

            lists.Add(new SelectListItem() { Text = " GC All Employee Salary Details Statement Before Approval (Pdf Format)", Value = "99" });

            entity.ReportList = lists;
        }

        private List<SelectListItem> Months()
        {
            List<SelectListItem> items3 = new List<SelectListItem>();
            items3.Add(new SelectListItem
            {
                Text = "Please Select",
                Value = "0"
            });
            items3.Add(new SelectListItem
            {
                Text = "January",
                Value = "1"
            });
            items3.Add(new SelectListItem
            {
                Text = "February",
                Value = "2"
            });
            items3.Add(new SelectListItem
            {
                Text = "March",
                Value = "3"
            });
            items3.Add(new SelectListItem
            {
                Text = "April",
                Value = "4"
            });
            items3.Add(new SelectListItem
            {
                Text = "May",
                Value = "5"
            });
            items3.Add(new SelectListItem
            {
                Text = "June",
                Value = "6"
            });
            items3.Add(new SelectListItem
            {
                Text = "July",
                Value = "7"
            });
            items3.Add(new SelectListItem
            {
                Text = "August",
                Value = "8"
            });
            items3.Add(new SelectListItem
            {
                Text = "September",
                Value = "9"
            });
            items3.Add(new SelectListItem
            {
                Text = "October",
                Value = "10"
            });
            items3.Add(new SelectListItem
            {
                Text = "November",
                Value = "11"
            });
            items3.Add(new SelectListItem
            {
                Text = "December",
                Value = "12"
            });

            return items3;
        }

        private List<SelectListItem> Years()
        {
            List<SelectListItem> items2 = new List<SelectListItem>();
            items2.Add(new SelectListItem
            {
                Text = "Please Select",
                Value = "0"
            });
            for (int year = DateTime.Now.Year; year >= (DateTime.Now.Year) - 5; year--)
            {
                items2.Add(new SelectListItem
                {
                    Text = Convert.ToString(year),
                    Value = Convert.ToString(year)
                });
            }

            return items2;
        }

        #endregion

        #region Private Methods

        private PRWorkAreaViewModel PopulateSalaryProcessedByEmployeeOfficeInfo(PRWorkAreaViewModel model)
        {
            model.EmployeeName = LoggedInEmployee.EmployeeName;
            model.OfficeId = (int)LoggedInOfficeID;

            var officeType = officeTypeService.GetById((int)LoggedInOfficeType);
            if (officeType != null)
                model.IsHeadOffice = officeType.OfficeTypeCode == OfficeTypeConstants.HeadOffice;

            model.OfficeTypeId = officeType.OfficeTypeId;

            var loggedInOffice = officeService.GetById((int)LoggedInOfficeID);

            if (loggedInOffice != null)
            {
                model.OfficeName = loggedInOffice.OfficeName;
                model.OfficeCode = loggedInOffice.OfficeCode;
            }

            return model;
        }

        private void PopulateOfficeTypes(PRWorkAreaViewModel model, SelectListItem pleaseSelect)
        {
            var view_list = new List<SelectListItem>();

            view_list.Add(pleaseSelect);
            var officeTypeList = officeTypeService.GetMany(x => x.IsActive == true);

            if (model.IsHeadOffice)
            {
                var listHeadOffice = officeTypeList.AsEnumerable().Select(item => new SelectListItem
                {
                    Text = item.OfficeTypeName,
                    Value = item.OfficeTypeId.ToString(),
                    Disabled = false,
                    Selected = item.OfficeTypeId == model.OfficeTypeId
                }).ToList();

                view_list.AddRange(listHeadOffice);

                model.OfficeTypeList = view_list;
                return;
            }

            var listUnderHeadOffice = officeTypeList.AsEnumerable().Select(item => new SelectListItem
            {
                Text = item.OfficeTypeName,
                Value = item.OfficeTypeId.ToString(),
                Disabled = item.OfficeTypeId != model.OfficeTypeId,
                Selected = item.OfficeTypeId == model.OfficeTypeId
            }).ToList();

            view_list.AddRange(listUnderHeadOffice);

            model.OfficeTypeList = view_list;
            return;
        }

        private void PopulateOfficeTypesNew(PRWorkAreaViewModel model, SelectListItem pleaseSelect)
        {
            var view_list = new List<SelectListItem>();

            view_list.Add(pleaseSelect);
            var officeTypeList = officeTypeService.GetMany(x => x.IsActive == true && x.OfficeTypeId >= SessionHelper.LoggedInOfficeTypeId);

            //if (model.IsHeadOffice)
            //{
            //    var listHeadOffice = officeTypeList.AsEnumerable().Select(item => new SelectListItem
            //    {
            //        Text = item.OfficeTypeName,
            //        Value = item.OfficeTypeId.ToString(),
            //        Disabled = false,
            //        Selected = item.OfficeTypeId == model.OfficeTypeId
            //    }).ToList();

            //    view_list.AddRange(listHeadOffice);

            //    model.OfficeTypeList = view_list;
            //    return;
            //}

            var listUnderHeadOffice = officeTypeList.AsEnumerable().Select(item => new SelectListItem
            {
                Text = item.OfficeTypeName,
                Value = item.OfficeTypeId.ToString(),
                //Disabled = item.OfficeTypeId != model.OfficeTypeId,
                Selected = item.OfficeTypeId == model.OfficeTypeId
            }).ToList();

            view_list.AddRange(listUnderHeadOffice);

            model.OfficeTypeList = view_list;
            return;
        }

        private void PopulateOfficeDropdownList(PRWorkAreaViewModel model, SelectListItem pleaseSelect)
        {
            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(pleaseSelect);

            var officeList = officeService.GetOfficeAndRelatedOffices(model.OfficeCode);

            var listHeadOffice = officeList.AsEnumerable().Select(item => new SelectListItem
            {
                Text = item.OfficeName,
                Value = item.OfficeId.ToString(),
                Disabled = false,
                Selected = item.OfficeId == model.OfficeId
            }).ToList();

            selectListItems.AddRange(listHeadOffice);

            model.OfficeList = selectListItems;
            return;
        }
        private void PopulateOfficeDropdownListNew(PRWorkAreaViewModel model, SelectListItem pleaseSelect, int officetypeid)
        {
            var selectListItems = new List<SelectListItem>();
            selectListItems.Add(pleaseSelect);

            //var officeList = officeService.GetOfficeAndRelatedOffices(model.OfficeCode);
            var officelst = new gHRMDBContext().Database.SqlQuery<Office>($"[dbo].[sp_GetOfficeTypeXRelatedOffice] '{model.OfficeCode}',{officetypeid}").ToList();
            var listHeadOffice = officelst.AsEnumerable().Select(item => new SelectListItem
            {
                Text = item.OfficeName,
                Value = item.OfficeId.ToString(),
                Disabled = false,
                Selected = item.OfficeId == model.OfficeId
            }).ToList();

            selectListItems.AddRange(listHeadOffice);

            model.OfficeList = selectListItems;
            return;
        }

        private List<EmployeeMonthlySalaryModel> GetEmployeeMonthlySalaryByPFBackDateDeductionAndIsSendForApproval(int salaryMonth, int salaryYear)
        {
            List<EmployeeMonthlySalaryModel> pfBackDataSetList;
            using (var db = new gHRMDBContext())
            {
                //get employee monthly salary for "PF BackDate Deduction" and IsSendForApproval=1 from [EmployeeMonthlySalary]
                var sqlCommand = $@"prl.SP_PR_GET_MonthlySalary_PF_BackDatedEntry
                                        {salaryYear},
                                        {salaryMonth}
                                        ";

                pfBackDataSetList = db.Database.SqlQuery<EmployeeMonthlySalaryModel>(sqlCommand).AsParallel().ToList();
            }

            return pfBackDataSetList;
        }

        private List<EmployeeMonthlySalaryModel> GetEmployeeMonthlySalaryForThisYearAndMonth(int salaryMonth, int salaryYear)
        {
            List<EmployeeMonthlySalaryModel> salaryList;
            using (var db = new gHRMDBContext())
            {
                var sqlCommand = $@"prl.SP_PR_GET_EmployeeMonthlySalaryView
                                        {salaryYear},
                                        {salaryMonth}
                                        ";
                //get "IS SEND FOR APPROVAL" for appr employee monthly salary for this month and year from [EmployeeMonthlySalary]
                salaryList = db.Database.SqlQuery<EmployeeMonthlySalaryModel>(sqlCommand).AsParallel().ToList();
            }

            return salaryList;
        }


        private List<TmpEmployeeSalaryView_Challan> GetSalarySummaryPreview_Challan(StringBuilder andCondition)
        {
            var param = new { @AndCondition = andCondition.ToString() };
            var list = employeeSPService.GetDataWithParameter(param, "prl.SP_PR_GET_EmployeeSalaryView_Challan");

            var monthlySalarys = list.Tables[0].AsEnumerable().Select(row => new View_EmployeeMonthlySalary_Challan()
            {
                SalaryMonth = row.Field<int>("SalaryMonth"),
                SalaryYear = row.Field<int>("SalaryYear"),
                EmployeeId = row.Field<long>("EmployeeId"),
                EmployeeName = row.Field<string>("EmployeeName"),
                EmployeeCode = row.Field<string>("EmployeeCode"),
                DesignationName = row.Field<string>("DesignationName"),
                DepartmentName = row.Field<string>("DepartmentName"),
                ComponentCategory = row.Field<string>("ComponentCategory"),
                TransactionType = row.Field<string>("TransactionType"),
                PRComponentAmount = row.Field<decimal>("PRComponentAmount"),
                IsActive = row.Field<bool>("IsActive"),
                IsSendForApproval = row.Field<bool>("IsSendForApproval"),
                IsApproved = row.Field<bool>("IsApproved"),
                IsRejected = row.Field<bool>("IsRejected"),
                ChallanDate = row.Field<string>("ChallanDate"),
                ChallanNo = row.Field<string>("ChallanNo"),
            }).ToList();

            var empMonthlySalarys = new List<TmpEmployeeSalaryView_Challan>();
            var employeeList = new List<TmpEmployeeDuplicateCheck_Challan>();
            foreach (var item in monthlySalarys)
            {
                if (employeeList.Where(p => p.EmployeeId == item.EmployeeId).ToList().Count <= 0)
                {
                    var monthlySalary = new TmpEmployeeSalaryView_Challan();
                    var emp = new TmpEmployeeDuplicateCheck_Challan();

                    emp.EmployeeId = item.EmployeeId;
                    employeeList.Add(emp);

                    var employeeIndividualEarning = monthlySalarys.Where(p => p.EmployeeId == emp.EmployeeId && p.TransactionType == SalaryAccountTransactionTypeConstants.Addition).ToList();
                    var employeeIndividualDeduction = monthlySalarys.Where(p => p.EmployeeId == emp.EmployeeId && p.TransactionType == SalaryAccountTransactionTypeConstants.Deduction).ToList();
                    monthlySalary.TotalEarning = Convert.ToDouble(employeeIndividualEarning.Sum(p => p.PRComponentAmount));
                    monthlySalary.TotalDeduction = Convert.ToDouble(employeeIndividualDeduction.Sum(p => p.PRComponentAmount));
                    monthlySalary.NetPayable = (monthlySalary.TotalEarning - monthlySalary.TotalDeduction);
                    monthlySalary.rowSl = item.rowSl;
                    monthlySalary.EmployeeId = item.EmployeeId;
                    monthlySalary.EmployeeCode = item.EmployeeCode;
                    monthlySalary.EmployeeName = item.EmployeeName;
                    monthlySalary.Department = item.DepartmentName;
                    monthlySalary.Designation = item.DesignationName;
                    monthlySalary.ChallanNo = item.ChallanNo;
                    monthlySalary.ChallanDate = item.ChallanDate;
                    empMonthlySalarys.Add(monthlySalary);
                }
            }

            return empMonthlySalarys;
        }



        private List<TmpEmployeeSalaryView> GetSalarySummaryPreview(StringBuilder andCondition)
        {
            var param = new { @AndCondition = andCondition.ToString() };
            var list = employeeSPService.GetDataWithParameter(param, "prl.SP_PR_GET_EmployeeSalaryView");

            var monthlySalarys = list.Tables[0].AsEnumerable().Select(row => new View_EmployeeMonthlySalary()
            {
                SalaryMonth = row.Field<int>("SalaryMonth"),
                SalaryYear = row.Field<int>("SalaryYear"),
                EmployeeId = row.Field<long>("EmployeeId"),
                EmployeeName = row.Field<string>("EmployeeName"),
                EmployeeCode = row.Field<string>("EmployeeCode"),
                DesignationName = row.Field<string>("DesignationName"),
                DepartmentName = row.Field<string>("DepartmentName"),
                ComponentCategory = row.Field<string>("ComponentCategory"),
                TransactionType = row.Field<string>("TransactionType"),
                PRComponentAmount = row.Field<decimal>("PRComponentAmount"),
                IsActive = row.Field<bool>("IsActive"),
                IsSendForApproval = row.Field<bool>("IsSendForApproval"),
                IsApproved = row.Field<bool>("IsApproved"),
                IsRejected = row.Field<bool>("IsRejected"),
            }).ToList();

            var empMonthlySalarys = new List<TmpEmployeeSalaryView>();
            var employeeList = new List<TmpEmployeeDuplicateCheck>();
            foreach (var item in monthlySalarys)
            {
                if (employeeList.Where(p => p.EmployeeId == item.EmployeeId).ToList().Count <= 0)
                {
                    var monthlySalary = new TmpEmployeeSalaryView();
                    var emp = new TmpEmployeeDuplicateCheck();

                    emp.EmployeeId = item.EmployeeId;
                    employeeList.Add(emp);

                    var employeeIndividualEarning = monthlySalarys.Where(p => p.EmployeeId == emp.EmployeeId && p.TransactionType == SalaryAccountTransactionTypeConstants.Addition).ToList();
                    var employeeIndividualDeduction = monthlySalarys.Where(p => p.EmployeeId == emp.EmployeeId && p.TransactionType == SalaryAccountTransactionTypeConstants.Deduction).ToList();
                    monthlySalary.TotalEarning = Convert.ToDouble(employeeIndividualEarning.Sum(p => p.PRComponentAmount));
                    monthlySalary.TotalDeduction = Convert.ToDouble(employeeIndividualDeduction.Sum(p => p.PRComponentAmount));
                    monthlySalary.NetPayable = (monthlySalary.TotalEarning - monthlySalary.TotalDeduction);
                    monthlySalary.rowSl = item.rowSl;
                    monthlySalary.EmployeeId = item.EmployeeId;
                    monthlySalary.EmployeeCode = item.EmployeeCode;
                    monthlySalary.EmployeeName = item.EmployeeName;
                    monthlySalary.Department = item.DepartmentName;
                    monthlySalary.Designation = item.DesignationName;
                    empMonthlySalarys.Add(monthlySalary);
                }
            }

            return empMonthlySalarys;
        }

        private List<TmpEmployeeSalaryView> GetSalarySummaryPreview2(StringBuilder andCondition)
        {
            var param = new { @AndCondition = andCondition.ToString() };
            var list = employeeSPService.GetDataWithParameter(param, "prl.SP_PR_GET_EmployeeSalaryView");

            var monthlySalarys = list.Tables[0].AsEnumerable().Select(row => new View_EmployeeMonthlySalary()
            {
                SalaryMonth = row.Field<int>("SalaryMonth"),
                SalaryYear = row.Field<int>("SalaryYear"),
                EmployeeId = row.Field<long>("EmployeeId"),
                EmployeeName = row.Field<string>("EmployeeName"),
                EmployeeCode = row.Field<string>("EmployeeCode"),
                DesignationName = row.Field<string>("DesignationName"),
                DepartmentName = row.Field<string>("DepartmentName"),
                ComponentCategory = row.Field<string>("ComponentCategory"),
                TransactionType = row.Field<string>("TransactionType"),
                PRComponentAmount = row.Field<decimal>("PRComponentAmount"),
                IsActive = row.Field<bool>("IsActive"),
                IsSendForApproval = row.Field<bool>("IsSendForApproval"),
                IsApproved = row.Field<bool>("IsApproved"),
                IsRejected = row.Field<bool>("IsRejected"),
            }).ToList();

            var empMonthlySalarys = new List<TmpEmployeeSalaryView>();
            var employeeList = new List<TmpEmployeeDuplicateCheck>();
            foreach (var item in monthlySalarys)
            {
                if (employeeList.Where(p => p.EmployeeId == item.EmployeeId).ToList().Count <= 0)
                {
                    var monthlySalary = new TmpEmployeeSalaryView();
                    var emp = new TmpEmployeeDuplicateCheck();

                    emp.EmployeeId = item.EmployeeId;
                    employeeList.Add(emp);

                    var employeeIndividualEarning = monthlySalarys.Where(p => p.EmployeeId == emp.EmployeeId && p.TransactionType == SalaryAccountTransactionTypeConstants.Addition).ToList();
                    var employeeIndividualDeduction = monthlySalarys.Where(p => p.EmployeeId == emp.EmployeeId && p.TransactionType == SalaryAccountTransactionTypeConstants.Deduction).ToList();
                    monthlySalary.TotalEarning = Convert.ToDouble(employeeIndividualEarning.Sum(p => p.PRComponentAmount));
                    monthlySalary.TotalDeduction = Convert.ToDouble(employeeIndividualDeduction.Sum(p => p.PRComponentAmount));
                    monthlySalary.NetPayable = (monthlySalary.TotalEarning - monthlySalary.TotalDeduction);
                    monthlySalary.rowSl = item.rowSl;
                    monthlySalary.EmployeeId = item.EmployeeId;
                    monthlySalary.EmployeeCode = item.EmployeeCode;
                    monthlySalary.EmployeeName = item.EmployeeName;
                    monthlySalary.Department = item.DepartmentName;
                    monthlySalary.Designation = item.DesignationName;
                    empMonthlySalarys.Add(monthlySalary);
                }
            }

            return empMonthlySalarys;
        }


        private void InsertMonthlySalaryHistoryRejected(int salaryYear, int salaryMonth)
        {
            var param = new { SalaryYear = salaryYear, SalaryMonth = salaryMonth, UserAction = "Salary Rejected", UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID) };
            var val = employeeSPService.GetDataWithParameter(param, "prl.SP_InsertMonthlySalaryHistoryRejected");
        }

        private void InActiveExceptionalSalaryDetailRejected(DateTime firstDate, DateTime lastDate)
        {
            var param = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate };
            var list = employeeSPService.GetDataWithParameter(param, "prl.SP_InActive_ExceptionalSalaryRejected");
        }



        #endregion
    }
}