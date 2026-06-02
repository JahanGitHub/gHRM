
#region Usings
using gHRM.Core.Filters.Payroll;
using gHRM.Core.Utilities;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service;
using gHRM.Service.Payroll;
using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.Infrastructure.Date;
using gHRM.Web.ViewModels.Payroll;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers.Payroll
{
    public class ImportSalaryAllowanceController : Controller
    {
        #region Private Variables

        private IPRComponentService prComponentService;
        private readonly IPRSalaryConfigurationService prSalaryConfigurationService;
        private IEmployeeSalaryIncentiveService empSalaryIncentiveService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IEmployeeSalaryDeductionService employeeSalaryDeductionService;
        private readonly IView_EmployeeSalaryConfigurationService viewSalaryConfigurationService;
        private readonly IEmployeeSPService employeeSpService;
        private readonly IProductTypeService productTypeService;
        private readonly IEmployeeMonthlySalaryApprovedService employeeMonthlySalaryApprovedService;
        private readonly IEmployeeMonthlySalaryService employeeMonthlySalaryService;
        private readonly IEmployeeShortInfoService employeeShortInfoService;
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;
        private readonly IEmployeeService employeeService;
        private readonly IOfficeService officeService;
        private readonly ISalaryDateConfigService salaryDateConfigService;
        public ImportSalaryAllowanceController(
              IView_EmployeeSalaryConfigurationService viewSalaryConfigurationService
            , IEmployeeSalaryDeductionService employeeSalaryDeductionService, IPRComponentService prComponentService
            , IPRSalaryConfigurationService prSalaryConfigurationService
            , IEmployeeSalaryIncentiveService empSalaryIncentiveService
            , IEmployeeSPService employeeSPService
            , IEmployeeSPService employeeSpService
            , IProductGroupService productGroupService
            , IProductTypeService productTypeService
            , IProductItemService productItemService
            , IEmployeeMonthlySalaryApprovedService employeeMonthlySalaryApprovedService
            , IEmployeeMonthlySalaryService employeeMonthlySalaryService
            , IEmployeeShortInfoService employeeShortInfoService
            , IEmployeeService employeeService
            , IOfficeService officeService
            , ISalaryDateConfigService salaryDateConfigService)
        {
            this.employeeSPService = employeeSPService;
            this.prSalaryConfigurationService = prSalaryConfigurationService;
            this.prComponentService = prComponentService;
            this.empSalaryIncentiveService = empSalaryIncentiveService;
            this.employeeSalaryDeductionService = employeeSalaryDeductionService;
            this.viewSalaryConfigurationService = viewSalaryConfigurationService;
            this.employeeSpService = employeeSpService;
            this.productTypeService = productTypeService;
            this.employeeMonthlySalaryApprovedService = employeeMonthlySalaryApprovedService;
            this.employeeMonthlySalaryService = employeeMonthlySalaryService;
            this.employeeShortInfoService = employeeShortInfoService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
            this.employeeService = employeeService;
            this.officeService = officeService;
            this.salaryDateConfigService = salaryDateConfigService;
        }

        #endregion       

        #region Salary Allowance

        public ActionResult SalaryAllowanceDeduction()
        {
            var model = new PRImportSalaryAllowanceDeductionViewModel
            {
                Years = DateHelper.GetYears(3, 15),
                Months = DateHelper.GetMonths(),
            };
            ViewBag.IsSuccess = true;
            ViewBag.Message = "";
            return View(model);
        }

        public ActionResult SalaryAllowanceDeductionProcess()
        {
            var model = new PRImportSalaryAllowanceDeductionViewModel
            {
                Years = DateHelper.GetYears(3, 15),
                Months = DateHelper.GetMonths(),
            };
            ViewBag.IsSuccess = true;
            ViewBag.Message = "";
            return View(model);
        }

        public ActionResult SalaryAllowanceDeduction2()
        {
            var model = new PRImportSalaryAllowanceDeductionViewModel2
            {
                Years = DateHelper.GetYears(3, 15),
                Months = DateHelper.GetMonths(),
            };
            model.ComponentList = commonStaticDropDown.ddlInitial();

            var componentList = new List<SelectListItem>();
            componentList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            componentList.Add(new SelectListItem() { Text = ComponentCategoryConstants.GetText(ComponentCategoryConstants.Allowance), Value = ComponentCategoryConstants.Allowance });
            componentList.Add(new SelectListItem() { Text = ComponentCategoryConstants.GetText(ComponentCategoryConstants.Deduction), Value = ComponentCategoryConstants.Deduction });
            model.ComponentCategoryList = componentList;


            ViewBag.IsSuccess = true;
            ViewBag.Message = "";
            return View(model);
        }


        [HttpPost]
        public ActionResult SalaryAllowanceDeduction(PRImportSalaryAllowanceDeductionViewModel model)
        {
            ViewBag.IsSuccess = false;
            ViewBag.Message = "";
            model.Years = DateHelper.GetYears(3, 15);
            model.Months = DateHelper.GetMonths();
            try
            {
                string validationMessage;
                var importSalaryAllowanceDeductionErrorList = "";

                if (!ModelState.IsValid)
                {
                    ViewBag.Message = "Error on file, Please try again";
                    return View(model);
                }

                if (Request.Files.Count <= 0)
                {
                    ViewBag.Message = "File not found. Please try again.";
                    return View(model);
                }

                var file = Request.Files[0];

                // Generate dataset
                var ds = GetMemberDatasetFromFile(file, out validationMessage);

                if (ds == null)
                {
                    ViewBag.Message = validationMessage;
                    return View(model);
                }

                if (!string.IsNullOrWhiteSpace(validationMessage))
                {
                    ViewBag.Message = validationMessage;
                    return View(model);
                }

                var salaryAllowanceDeductionList = new List<PRSalaryAllowanceDeductionViewModel>();
                long createdBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);

                // Generate member list
                validationMessage = GenerateAllowanceDeductionList(salaryAllowanceDeductionList, createdBy, ds);

                if (salaryAllowanceDeductionList.Count == 0 &&
                    !string.IsNullOrWhiteSpace(validationMessage))
                {
                    importSalaryAllowanceDeductionErrorList = GetImportSalaryAllowanceDeductionErrorList(validationMessage);

                    ViewBag.Message = "Error occurred. Please seee details in validation message section.";
                    ViewBag.ErrorList = importSalaryAllowanceDeductionErrorList;
                    return View(model);
                }

                if (salaryAllowanceDeductionList.Count == 0)
                {
                    ViewBag.Message = "No employees were found to import.";
                    return View(model);
                }

                var currentSalaryDateConfig = salaryDateConfigService.GetCurrentSalaryDateConfig();
                if (currentSalaryDateConfig == null)
                {
                    ViewBag.Message = "Salary Day not configured! Please configure first.";
                    return View(model);
                }

                model.SalaryMonth = ((DateTime)model.StartDate).Month;
                model.SalaryYear = ((DateTime)model.StartDate).Year;
                model.SalaryDay = currentSalaryDateConfig.DayOfMonthlySalary;
                foreach (var s in salaryAllowanceDeductionList)
                {

                    var checkDuplicateIncentive = empSalaryIncentiveService.CheckAllowanceExist(s.EmployeeCode, s.ComponentName, s.ComponentCategory, model.StartDate.Value, model.EndDate.Value);

                    if (checkDuplicateIncentive)
                    {
                        ViewBag.Message = $@"Duplicate entry denied, employeeid: {s.EmployeeCode} & Component:{s.ComponentName}";
                        return View(model);
                    }

                }


                //let's create incentive for both incentive and deduction
                var response = CreateIncentive(model, salaryAllowanceDeductionList);
                if (!response.IsSuccess)
                {
                    ViewBag.Message = response.Message;
                    return View(model);
                }
                ViewBag.IsSuccess = true;
                ViewBag.Message = "Import Existing Salary Allowance Deduction successfull!.";
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Message = Funct.GetError(ex);
                return View(model);
            }
        }


        [HttpPost]
        public ActionResult SalaryAllowanceDeduction2(PRImportSalaryAllowanceDeductionViewModel2 model)
        {
            ViewBag.IsSuccess = false;
            ViewBag.Message = "";
            model.Years = DateHelper.GetYears(3, 15);
            model.Months = DateHelper.GetMonths();
            try
            {
                string validationMessage;
                var importSalaryAllowanceDeductionErrorList = "";

     
                if (Request.Files.Count <= 0)
                {
                    ViewBag.IsSuccess = true;
                    ViewBag.Message = "File not found. Please try again.";
                    return View(model);
                }
                var SalaryExists = employeeMonthlySalaryService.GetAll().Where(z => z.SalaryMonth == model.SalaryMonth && z.SalaryYear == model.SalaryYear);

                if(!SalaryExists.Any())
                {
                    string monthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.SalaryMonth);
                    ViewBag.IsSuccess = true;
                    ViewBag.Message = "Pls Generate Salary for the month of ."+ monthName;
                    return View(model);
                }


                var file = Request.Files[0];

                // Generate dataset
                var ds = GetMemberDatasetFromFile2(file, out validationMessage);

                if (ds == null)
                {
                    ViewBag.Message = validationMessage;
                    return View(model);
                }

                if (!string.IsNullOrWhiteSpace(validationMessage))
                {
                    ViewBag.Message = validationMessage;
                    return View(model);
                }

                var salaryAllowanceDeductionList = new List<PRSalaryAllowanceDeductionViewModel>();
                long createdBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);

                // Generate member list
                validationMessage = GenerateAllowanceDeductionList2(model, createdBy, ds);

           
                importSalaryAllowanceDeductionErrorList = GetImportSalaryAllowanceDeductionErrorList2(validationMessage);

           


                model.ComponentList = commonStaticDropDown.ddlInitial();

                var componentList = new List<SelectListItem>();
                componentList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
                componentList.Add(new SelectListItem() { Text = ComponentCategoryConstants.GetText(ComponentCategoryConstants.Allowance), Value = ComponentCategoryConstants.Allowance });
                componentList.Add(new SelectListItem() { Text = ComponentCategoryConstants.GetText(ComponentCategoryConstants.Deduction), Value = ComponentCategoryConstants.Deduction });
                model.ComponentCategoryList = componentList;


                ViewBag.IsSuccess = true;
                ViewBag.Message = "Import Existing Salary Allowance Deduction successfull!.";
                //ViewBag.ErrorList = importSalaryAllowanceDeductionErrorList;

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Message = Funct.GetError(ex);

                model.ComponentList = commonStaticDropDown.ddlInitial();

                var componentList = new List<SelectListItem>();
                componentList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
                componentList.Add(new SelectListItem() { Text = ComponentCategoryConstants.GetText(ComponentCategoryConstants.Allowance), Value = ComponentCategoryConstants.Allowance });
                componentList.Add(new SelectListItem() { Text = ComponentCategoryConstants.GetText(ComponentCategoryConstants.Deduction), Value = ComponentCategoryConstants.Deduction });
                model.ComponentCategoryList = componentList;

                return View(model);
            }


        }


        [HttpPost]
        public ActionResult SalaryAllowanceDeductionProcess(PRImportSalaryAllowanceDeductionViewModel model)
        {
            ViewBag.IsSuccess = false;
            ViewBag.Message = "";
            model.Years = DateHelper.GetYears(3, 15);
            model.Months = DateHelper.GetMonths();

            try
            {
                string validationMessage;
                var importSalaryAllowanceDeductionErrorList = "";

                var param = new
                {
                    Year = model.SalaryYear,
                    Month = model.SalaryMonth,
                };
                employeeSPService.GetDataWithParameter(param, "SP_SET_CONFIRMATION_PROBATIONARY_SALARY_IN_ALLOWANCE_DEDUCTION");

                ViewBag.IsSuccess = true;
                ViewBag.Message = "Confirmation in Current Month  Process successfull!.";

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Message = Funct.GetError(ex);
                return View(model);
            }
        }


        #endregion

        #region Private Methods

        private BaseResponse CreateIncentive(PRImportSalaryAllowanceDeductionViewModel model, List<PRSalaryAllowanceDeductionViewModel> allowanceDeductionListing)
        {
            bool isOperationSuccess = true;
            var response = new BaseResponse();
            string result = string.Empty;

            var sDate = Convert.ToDateTime(model.StartDate);
            var eDate = Convert.ToDateTime(model.EndDate);

            var firstDate = new DateTime(sDate.Year, sDate.Month, 1);
            DateTime firstDateOfNextMonth = new DateTime(sDate.Year, sDate.Month, 1).AddMonths(1);
            var lastDateOfMonth = firstDateOfNextMonth.AddDays(-1);

            var endDateGeneration = new DateTime(Convert.ToDateTime(model.EndDate).Year, Convert.ToDateTime(model.EndDate).Month, 1);
            DateTime firstDateOEndDateNextMonth = new DateTime(Convert.ToDateTime(model.EndDate).Year, Convert.ToDateTime(model.EndDate).Month, 1).AddMonths(1);
            var endDateLastDate = firstDateOEndDateNextMonth.AddDays(-1);

            if (Convert.ToDateTime(model.EndDate) < endDateLastDate)
            {
                result = "End Date Must be last date of the Month";
                response = new BaseResponse { IsSuccess = false, Message = result };
                return response;
            }

            if (sDate < firstDate)
            {
                result = "Effective Start Date can't be smaller than current month";
                response = new BaseResponse { IsSuccess = false, Message = result };
                return response;
            }
            if (sDate > lastDateOfMonth)
            {
                result = "Effective Start Date can't be bigger than current month";
                response = new BaseResponse { IsSuccess = false, Message = result };
                return response;
            }

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(2, 0, 0)))
            {
                try
                {
                    foreach (var item in allowanceDeductionListing)
                    {
                        var employee = employeeService.GetEmployeeByEmployeeCode(item.EmployeeCode);

                        if (employee == null)
                            continue;

                        int employeeId = Convert.ToInt32(employee.EmployeeId);

                        var officeDetail = officeService.GetById((int)employee.OfficeId);
                        if (officeDetail == null)
                            continue;

                        var componentSearchFilter = new PRComponentSearchFilter
                        {
                            OfficeLocationId = (int)officeDetail.OfficeLocationId,
                            EmployeeTypeId = (int)employee.EmployeeTypeId,
                            EmployeeStatusId = employee.EmployeeStatusId,
                            ComponentName = item.ComponentName,
                            ComponentCategory = item.ComponentCategory
                        };

                        //get component from [prl.PRComponent]
                        var component = prComponentService.GetSingleComponentByFilter(componentSearchFilter);
                        if (component == null)
                            continue;

                        item.PrComponentId = Convert.ToInt32(component.PRComponentID);

                        //check salary configuration bet start and end for [prl.PRSalaryConfiguration]
                        var checkSalaryConfigured = prSalaryConfigurationService.GetPREmployeeSalaryCurrentConfigurationAllowanceAndDeduction
                        (Convert.ToInt64(employeeId), Convert.ToDateTime(model.StartDate), Convert.ToDateTime(model.EndDate));

                        if (!checkSalaryConfigured.Any())
                            continue;

                        //if allowance then incentive. 
                        if (item.ComponentCategory.Trim() == ComponentCategoryConstants.Allowance)
                        {
                            //let's add into  [EmployeeSalaryIncentive] and [prl.EmployeeMonthlySalary] 
                            response = IncentiveCreate(employeeId, ((DateTime)model.StartDate).ToString("dd-MMM-yyyy"), ((DateTime)model.EndDate).ToString("dd-MMM-yyyy"),
                                item.PrComponentId, item.PrComponentAmount, item.PrComponentHour, item.ProductId, item.SerialId,
                                checkSalaryConfigured[0].OfficeID, item.Remark, model.SalaryMonth, model.SalaryYear, model.SalaryDay);

                            if (!response.IsSuccess && !response.ContinueProcess)
                            {
                                isOperationSuccess = false;
                                break;
                            }

                        } //if deduction then deduction. 
                        else if (item.ComponentCategory.Trim() == ComponentCategoryConstants.Deduction)
                        {
                            response = DeductionCreate(employeeId, ((DateTime)model.StartDate).ToString("dd-MMM-yyyy"), ((DateTime)model.EndDate).ToString("dd-MMM-yyyy"),
                                item.PrComponentId, item.PrComponentAmount, item.DeductionDays,
                                item.ProductId, item.SerialId, checkSalaryConfigured[0].OfficeID, item.Remark,
                                model.SalaryMonth, model.SalaryYear, model.SalaryDay);

                            if (!response.IsSuccess && !response.ContinueProcess)
                            {
                                isOperationSuccess = false;
                                break;
                            }
                        }
                        else
                        {
                            continue;
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
                {
                    response.IsSuccess = true;
                    scope.Complete();
                }

                scope.Dispose();
            }

            return response;
        }

        private BaseResponse CreateIncentive2(PRImportSalaryAllowanceDeductionViewModel2 model, List<PRSalaryAllowanceDeductionViewModel> allowanceDeductionListing)
        {
            bool isOperationSuccess = true;
            var response = new BaseResponse();
            string result = string.Empty;

            var sDate = Convert.ToDateTime(model.StartDate);
            var eDate = Convert.ToDateTime(model.EndDate);

            var firstDate = new DateTime(sDate.Year, sDate.Month, 1);
            DateTime firstDateOfNextMonth = new DateTime(sDate.Year, sDate.Month, 1).AddMonths(1);
            var lastDateOfMonth = firstDateOfNextMonth.AddDays(-1);

            var endDateGeneration = new DateTime(Convert.ToDateTime(model.EndDate).Year, Convert.ToDateTime(model.EndDate).Month, 1);
            DateTime firstDateOEndDateNextMonth = new DateTime(Convert.ToDateTime(model.EndDate).Year, Convert.ToDateTime(model.EndDate).Month, 1).AddMonths(1);
            var endDateLastDate = firstDateOEndDateNextMonth.AddDays(-1);

            if (Convert.ToDateTime(model.EndDate) < endDateLastDate)
            {
                result = "End Date Must be last date of the Month";
                response = new BaseResponse { IsSuccess = false, Message = result };
                return response;
            }

            if (sDate < firstDate)
            {
                result = "Effective Start Date can't be smaller than current month";
                response = new BaseResponse { IsSuccess = false, Message = result };
                return response;
            }
            if (sDate > lastDateOfMonth)
            {
                result = "Effective Start Date can't be bigger than current month";
                response = new BaseResponse { IsSuccess = false, Message = result };
                return response;
            }

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(2, 0, 0)))
            {
                try
                {
                    foreach (var item in allowanceDeductionListing)
                    {
                        var employee = employeeService.GetEmployeeByEmployeeCode(item.EmployeeCode);

                        if (employee == null)
                            continue;

                        int employeeId = Convert.ToInt32(employee.EmployeeId);

                        var officeDetail = officeService.GetById((int)employee.OfficeId);
                        if (officeDetail == null)
                            continue;

                        var componentSearchFilter = new PRComponentSearchFilter
                        {
                            OfficeLocationId = (int)officeDetail.OfficeLocationId,
                            EmployeeTypeId = (int)employee.EmployeeTypeId,
                            EmployeeStatusId = employee.EmployeeStatusId,
                            ComponentName = item.ComponentName,
                            ComponentCategory = item.ComponentCategory
                        };

                        //get component from [prl.PRComponent]
                        var component = prComponentService.GetSingleComponentByFilter(componentSearchFilter);
                        if (component == null)
                            continue;

                        item.PrComponentId = Convert.ToInt32(component.PRComponentID);

                        //check salary configuration bet start and end for [prl.PRSalaryConfiguration]
                        var checkSalaryConfigured = prSalaryConfigurationService.GetPREmployeeSalaryCurrentConfigurationAllowanceAndDeduction
                        (Convert.ToInt64(employeeId), Convert.ToDateTime(model.StartDate), Convert.ToDateTime(model.EndDate));

                        if (!checkSalaryConfigured.Any())
                            continue;

                        //if allowance then incentive. 
                        if (item.ComponentCategory.Trim() == ComponentCategoryConstants.Allowance)
                        {
                            //let's add into  [EmployeeSalaryIncentive] and [prl.EmployeeMonthlySalary] 
                            response = IncentiveCreate(employeeId, ((DateTime)model.StartDate).ToString("dd-MMM-yyyy"), ((DateTime)model.EndDate).ToString("dd-MMM-yyyy"),
                                item.PrComponentId, item.PrComponentAmount, item.PrComponentHour, item.ProductId, item.SerialId,
                                checkSalaryConfigured[0].OfficeID, item.Remark, model.SalaryMonth, model.SalaryYear, model.SalaryDay);

                            if (!response.IsSuccess && !response.ContinueProcess)
                            {
                                isOperationSuccess = false;
                                break;
                            }

                        } //if deduction then deduction. 
                        else if (item.ComponentCategory.Trim() == ComponentCategoryConstants.Deduction)
                        {
                            response = DeductionCreate(employeeId, ((DateTime)model.StartDate).ToString("dd-MMM-yyyy"), ((DateTime)model.EndDate).ToString("dd-MMM-yyyy"),
                                item.PrComponentId, item.PrComponentAmount, item.DeductionDays,
                                item.ProductId, item.SerialId, checkSalaryConfigured[0].OfficeID, item.Remark,
                                model.SalaryMonth, model.SalaryYear, model.SalaryDay);

                            if (!response.IsSuccess && !response.ContinueProcess)
                            {
                                isOperationSuccess = false;
                                break;
                            }
                        }
                        else
                        {
                            continue;
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
                {
                    response.IsSuccess = true;
                    scope.Complete();
                }

                scope.Dispose();
            }

            return response;
        }

        private string GetImportSalaryAllowanceDeductionErrorList2(string validationMessage)
        {
            var result = employeeSpService.GetDataWithoutParameter("SP_SET_CALCULATION_FROM_EXCEL_ALLOWANCE_DEDUCTION");

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

            //foreach (DataRow row in result.Tables[0].Rows)
            //{
            //    var error = row["Result"]?.ToString();
            //    if (!string.IsNullOrWhiteSpace(error))
            //    {
            //        htmlContent += $@"<li class='list-group-item'>{index}. {error}</li>";
            //        index++;
            //    }
            //}

            htmlContent += $@"</ul>
                    </div>
                </div>
            </div>
        </div>";

            return htmlContent;
        }


        private string GetImportSalaryAllowanceDeductionErrorList(string validationMessage)
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

        private BaseResponse IncentiveCreate(int employeeId, string startDate, string dateEndTo,
           int prComponentId, string prComponentAmount, string prComponentHour, int productId,
           int serialId, int officeId, string remark, int salaryMonth, int salaryYear, int salaryDay)
        {
            var response = new BaseResponse();
            var result = "";
            var hour = 0;
            var isOperationSuccess = true;

            var entity = new EmployeeSalaryIncentive();
            DateTime dateStart = Convert.ToDateTime(startDate);
            var dateEnd = Convert.ToDateTime(dateEndTo);
            var startMonth = dateStart.Month;
            var endMonth = dateEnd.Month;

            var firstDayOfStartMonth = new DateTime(dateStart.Year, dateStart.Month, 1);
            var firstDayOfEndMonth = new DateTime(dateEnd.Year, dateEnd.Month, 1);
            var lastDayOfEndMonth = firstDayOfEndMonth.AddMonths(1).AddDays(-1);

            //check duplicate incentive between start and end date by prComponentId for [EmployeeSalaryIncentive]
            var checkDuplicateIncentive = empSalaryIncentiveService
                .CheckEmployeeSalaryIncentive(employeeId, firstDayOfStartMonth, lastDayOfEndMonth
                                              , prComponentId, productId, serialId);

            if (checkDuplicateIncentive)
            {
                result = "Already this incentive is configured for the employee for this month, Duplicate entry denied";
                response.IsSuccess = false;
                response.Message = result;
                response.ContinueProcess = true;
                return response;
            }

            entity.StartDate = Convert.ToDateTime(startDate);
            entity.EndDate = Convert.ToDateTime(dateEndTo);
            entity.EmployeeId = employeeId;
            entity.PRComponentId = prComponentId;
            entity.PRComponentAmount = Convert.ToDecimal(prComponentAmount);
            entity.Remark = remark;

            if (!string.IsNullOrWhiteSpace(prComponentHour))
                hour = Convert.ToInt32(prComponentHour);

            entity.PRComponentHour = Convert.ToDecimal(hour);
            entity.ProductId = productId;
            entity.IsActive = true;
            entity.IsApproved = true;
            entity.CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
            entity.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
            entity.CreateDate = DateTime.UtcNow;
            entity.UpdateDate = DateTime.UtcNow;
            entity.ProductId = productId;
            entity.SerialId = serialId;

            //let's check employee monthly salary for this date range [prl.EmployeeMonthlySalary]
            var checkEmployeeMonthlySalaryFound = employeeMonthlySalaryService.CheckEmployeeMonthlySalary
                                            (entity.EmployeeId, entity.StartDate, entity.EndDate);

            if (!checkEmployeeMonthlySalaryFound)
            {
                result = "Employee monthly salary not found. Please try again!";
                response.IsSuccess = false;
                response.Message = result;
                return response;
            }

            //get component details by component id
            var componentDetail = prComponentService.GetById(entity.PRComponentId);
            if (componentDetail == null)
            {
                result = "No component found. Please try another!";
                response.IsSuccess = false;
                response.Message = result;
                return response;
            }

            using (var ts = new TransactionScope())
            {
                try
                {
                    //let's add into [EmployeeSalaryIncentive]
                    var newEmployeeSalaryIncentive = empSalaryIncentiveService.Create(entity);

                    if (newEmployeeSalaryIncentive == null || newEmployeeSalaryIncentive.SalaryIncentiveId <= 0)
                    {
                        result = "There was an error while adding incentive. Please try again!";
                        response.IsSuccess = false;
                        response.Message = result;
                        isOperationSuccess = false;
                    }

                    if (isOperationSuccess)
                    {
                        //check existing monthly salary in [[prl.EmployeeMonthlySalary]]
                        var isExistThisComponentForThisMonth = employeeMonthlySalaryService.CheckMonthlySalaryByComponent(salaryMonth, salaryYear, Convert.ToInt32(entity.EmployeeId), entity.PRComponentId);
                        if (!isExistThisComponentForThisMonth)
                        {
                            var salary = new EmployeeMonthlySalary();

                            //populate Employee monthly salary
                            salary = PopulateEmployeeMonthlySalary(officeId, entity, componentDetail, salaryMonth, salaryYear, salaryDay);

                            //let's add employee monthly salary [prl.EmployeeMonthlySalary]
                            var newEmployeeMonthlySalary = employeeMonthlySalaryService.Create(salary);

                            if (newEmployeeMonthlySalary == null)
                            {
                                result = "There was an error while generating incentive";
                                response.IsSuccess = false;
                                response.Message = result;
                                isOperationSuccess = false;
                            }
                        }
                    }

                    if (isOperationSuccess)
                    {
                        result = "Incentive Successfully Generated";
                        response.Message = result;
                        response.IsSuccess = true;
                    }
                }
                catch (Exception e)
                {
                    response.Message = e.Message;
                    response.IsSuccess = false;
                    isOperationSuccess = false;
                }

                if (isOperationSuccess)
                    ts.Complete();

                ts.Dispose();
            }

            return response;
        }

        private BaseResponse DeductionCreate(int employeeId, string dateStartFrom, string dateEndTo,
            int prComponentId, string prComponentAmount, string deductionDays, int productId,
            int serialId, int officeId, string remark, int salaryMonth, int salaryYear, int salaryDay)
        {
            var response = new BaseResponse();
            var result = "";
            var days = 0;
            var isOperationSuccess = true;

            var entity = new EmployeeSalaryDeduction();
            DateTime dateStart = Convert.ToDateTime(dateStartFrom);
            var dateEnd = Convert.ToDateTime(dateEndTo);
            var firstDayOfStartMonth = new DateTime(dateStart.Year, dateStart.Month, 1);
            var firstDayOfEndMonth = new DateTime(dateEnd.Year, dateEnd.Month, 1);
            var lastDayOfEndMonth = firstDayOfEndMonth.AddMonths(1).AddDays(-1);

            //check employee salary deduction in [prl.EmployeeSalaryDeduction]                
            var checkDuplicateDeduction = employeeSalaryDeductionService.CheckEmployeeSalaryDeduction(employeeId,
                                            firstDayOfStartMonth, lastDayOfEndMonth, prComponentId, productId, serialId);

            if (checkDuplicateDeduction)
            {
                result = "Already this deduction is configured for the employee for this month, Duplicate entry denied";
                response.Message = result;
                response.ContinueProcess = true;
                return response;
            }

            entity.StartDate = Convert.ToDateTime(dateStartFrom);
            entity.EndDate = Convert.ToDateTime(dateEndTo);
            entity.EmployeeId = employeeId;
            entity.ComponentId = prComponentId;
            entity.DeductedAmount = Convert.ToDecimal(prComponentAmount);
            entity.Remark = remark;

            if (!string.IsNullOrWhiteSpace(deductionDays))
                days = Convert.ToInt32(deductionDays);

            entity.DeductionDays = Convert.ToInt32(days);
            entity.IsActive = true;
            entity.IsApproved = true;
            entity.CreatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
            entity.UpdatedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
            entity.CreateDate = DateTime.UtcNow;
            entity.UpdateDate = DateTime.UtcNow;
            entity.ProductId = productId;
            entity.SerialId = serialId;
            var checkEmployeeMonthlySalaryFound = employeeMonthlySalaryService.CheckEmployeeMonthlySalary
                                               (entity.EmployeeId, entity.StartDate, entity.EndDate);

            if (!checkEmployeeMonthlySalaryFound)
            {
                result = "Employee monthly salary not found!";
                response.Message = result;
                return response;
            }

            var componentDetail = prComponentService.GetById(entity.ComponentId);
            if (componentDetail == null)
            {
                result = "Component not found. Please try another!";
                response.Message = result;
                return response;
            }

            using (var ts = new TransactionScope())
            {
                try
                {
                    //let's add into [prl.EmployeeSalaryDeduction]
                    var newEmployeeSalaryDeduction = employeeSalaryDeductionService.Create(entity);

                    if (newEmployeeSalaryDeduction == null || newEmployeeSalaryDeduction.Id <= 0)
                    {
                        result = "There was an error while adding employee salary deduction. Please try again!";
                        response.Message = result;
                        isOperationSuccess = false;
                    }

                    if (isOperationSuccess)
                    {
                        //check existing monthly salary in [[prl.EmployeeMonthlySalary]]
                        var isExistThisComponentForThisMonth = employeeMonthlySalaryService.CheckMonthlySalaryByComponent(salaryMonth, salaryYear, Convert.ToInt32(entity.EmployeeId), entity.ComponentId);
                        if (!isExistThisComponentForThisMonth)
                        {
                            //populate deduction employee monthly salary
                            var salary = PopulateDeductionEmployeeMonthlySalary(officeId, entity, componentDetail, salaryMonth, salaryYear, salaryDay);

                            //lets add into [prl.EmployeeMonthlySalary]
                            var newEmployeeMonthlySalary = employeeMonthlySalaryService.Create(salary);

                            if (newEmployeeMonthlySalary == null)
                            {
                                result = "There was an error while generating deduction. Please try another!";
                                response.Message = result;
                                isOperationSuccess = false;
                            }
                        }
                    }

                    if (isOperationSuccess)
                    {
                        result = "Deduction Successfully Generated";
                        response.Message = result;
                        response.IsSuccess = true;
                    }
                }
                catch (Exception e)
                {
                    response.Message = e.Message;
                    response.IsSuccess = false;
                    isOperationSuccess = false;
                }

                if (isOperationSuccess)
                    ts.Complete();

                ts.Dispose();
            }

            return response;
        }

        private EmployeeMonthlySalary PopulateDeductionEmployeeMonthlySalary(int officeId,
            EmployeeSalaryDeduction entity, PRComponent componentDetail, int salaryMonth,
            int salaryYear, int salaryDay)
        {
            var salary = new EmployeeMonthlySalary();
            salary.SalaryMonth = salaryMonth;
            salary.SalaryYear = salaryYear;
            salary.SalaryDate = new DateTime(salaryYear, salaryMonth, salaryDay);
            salary.EmployeeId = entity.EmployeeId;
            salary.PRComponentId = entity.ComponentId;
            salary.PRSalaryConfigurationId = null;
            salary.PRComponentAmount = entity.DeductedAmount;
            salary.ComponentCategory = componentDetail.ComponentCategory;
            salary.TransactionType = componentDetail.TransactionType;
            salary.OfficeId = officeId;
            salary.IsActive = true;
            salary.IsSendForApproval = false;
            salary.IsApproved = false;
            salary.IsRejected = false;
            salary.CreateDate = DateTime.UtcNow;
            salary.UpdateDate = DateTime.UtcNow;
            salary.CreatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            salary.UpdatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            return salary;
        }

        private EmployeeMonthlySalary PopulateEmployeeMonthlySalary(int officeId, EmployeeSalaryIncentive entity,
            PRComponent componentDetail, int salaryMonth, int salaryYear, int salaryDay)
        {
            var employeeMonthlySalary = new EmployeeMonthlySalary();

            employeeMonthlySalary.SalaryMonth = salaryMonth;
            employeeMonthlySalary.SalaryYear = salaryYear;
            employeeMonthlySalary.SalaryDate = new DateTime(salaryYear, salaryMonth, salaryDay);
            employeeMonthlySalary.EmployeeId = entity.EmployeeId;
            employeeMonthlySalary.PRComponentId = entity.PRComponentId;
            employeeMonthlySalary.PRSalaryConfigurationId = null;
            employeeMonthlySalary.PRComponentAmount = entity.PRComponentAmount;
            employeeMonthlySalary.ComponentCategory = componentDetail.ComponentCategory;
            employeeMonthlySalary.TransactionType = componentDetail.TransactionType;
            employeeMonthlySalary.IsActive = true;
            employeeMonthlySalary.IsSendForApproval = false;
            employeeMonthlySalary.IsApproved = false;
            employeeMonthlySalary.IsRejected = false;
            employeeMonthlySalary.OfficeId = officeId;
            employeeMonthlySalary.CreateDate = DateTime.UtcNow;
            employeeMonthlySalary.UpdateDate = DateTime.UtcNow;
            employeeMonthlySalary.CreatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            employeeMonthlySalary.UpdatedBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);

            return employeeMonthlySalary;
        }

        private void MapDropDownList(EmployeeSalaryIncentiveViewModel model)
        {
            model.ComponentList = commonStaticDropDown.ddlInitial(); ;
            model.YearList = commonStaticDropDown.NumberSerialDropDown(DateTime.Now.Year, DateTime.Now.Year, false);
            model.MonthList = commonStaticDropDown.MonthList();
            var componentList = new List<SelectListItem>();
            componentList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            componentList.Add(new SelectListItem() { Text = ComponentCategoryConstants.GetText(ComponentCategoryConstants.Allowance), Value = ComponentCategoryConstants.Allowance });
            componentList.Add(new SelectListItem() { Text = ComponentCategoryConstants.GetText(ComponentCategoryConstants.Deduction), Value = ComponentCategoryConstants.Deduction });
            model.ComponentCategoryList = componentList;
            model.productType_List = commonStaticDropDown.ddlInitial();
            model.productList = commonStaticDropDown.ddlInitial();
            model.serialList = commonStaticDropDown.ddlInitial();
            model.ProductGroupList = commonDynamicDropDown.GetPayrollProductGroup();
        }

        private List<ProductType> GetProductType(int productGroupId)
        {
            return productTypeService.GetAll().Where(p => p.ProductGroupId == productGroupId).ToList();
        }

        private string GenerateAllowanceDeductionList(ICollection<PRSalaryAllowanceDeductionViewModel> salaryAllowanceDeductionList,
                                                    long createdBy,
                                                    DataSet ds)
        {
            var validationMessage = "";

            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows == null)
                return "There is an issue reading data from this file. Please try again.";

            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var j = 0;
                var errorMessage = "";
                var newSalaryAllowanceDeduction = new PRSalaryAllowanceDeductionViewModel();

                //employee code
                var employeeCode = ds.Tables[0].Rows[i][j++].ToString();

                if (string.IsNullOrWhiteSpace(employeeCode))
                    continue;

                //if (!string.IsNullOrWhiteSpace(employeeCode))
                newSalaryAllowanceDeduction.EmployeeCode = SessionHelper.CompanyCode == GHRMPlusCompanyConstants.JagoraniChakraFoundation ?
                              CommonHelper.GetFormattedEmployeeCodeWithSixDigit(employeeCode)
                            : CommonHelper.GetFormattedEmployeeCodeWithFourDigit(employeeCode);

                //Componet Category
                var componetCategory = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(componetCategory))
                    newSalaryAllowanceDeduction.ComponentCategory = componetCategory;
                else
                    errorMessage += " Error: Componet Category not found in the file. " +
                                         "Row is " + (1 + i) + " and column is " + j;

                //Componet Name
                var componentName = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(componentName))
                    newSalaryAllowanceDeduction.ComponentName = componentName;
                else
                    errorMessage += " Error: Componet Name not found in the file. " +
                                         "Row is " + (1 + i) + " and column is " + j;

                //Component Amount
                var componentAmount = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(componentAmount))
                    newSalaryAllowanceDeduction.PrComponentAmount = componentAmount;

                // Remark
                var remark = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(remark))
                    newSalaryAllowanceDeduction.Remark = remark;

                if (string.IsNullOrEmpty(errorMessage))
                    salaryAllowanceDeductionList.Add(newSalaryAllowanceDeduction);
                else
                    validationMessage += errorMessage;
            }

            return validationMessage;
        }


        private string GenerateAllowanceDeductionList2(PRImportSalaryAllowanceDeductionViewModel2 model,
                                                  long createdBy,
                                                  DataSet ds)
        {
            var validationMessage = "";
            var salaryDateConfiguration = salaryDateConfigService.GetCurrentSalaryDateConfig();
            model.SalaryDay = salaryDateConfiguration.DayOfMonthlySalary;


            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows == null)
                return "There is an issue reading data from this file. Please try again.";

            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var j = 0;
                var errorMessage = "";
         

                //employee code
                var employeeCode = ds.Tables[0].Rows[i][j++].ToString();


                //EffectiveStartDate
                var componetCategory = ds.Tables[0].Rows[i][j++].ToString();


                //EffectiveEndDate
                var componentName = ds.Tables[0].Rows[i][j++].ToString();


                var param = new 
                {
                    ComponetnCategory = model.ComponentCategory,
                    ComponentName = model.PRComponentId,
                    SalaryYear = model.SalaryYear,
                    SalaryMonth = model.SalaryMonth,
                    SalaryDay = model.SalaryDay,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    EmployeeCode = employeeCode ,
                    EffectiveStartDate = componetCategory,
                    EffectiveEndDate = componentName,
                    CreateBy = SessionHelper.LoggedInEmployeeID,
                };

                employeeSpService.GetDataWithParameter(param, "SP_INSERT_FROM_EXCEL_FOR_ALLOWANCE_DEDUCTION");




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

                    var serverMappedPath = Server.MapPath("~/WebShared/Uploads/SalaryAllowanceDeductionImport/");
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

                    //var excelSheets = new string[dt.Rows.Count];
                    //var t = 0;

                    ////excel data saves in temp file here.
                    //foreach (DataRow row in dt.Rows)
                    //{
                    //    excelSheets[t] = row["TABLE_NAME"].ToString();
                    //    t++;
                    //}

                    var excelConnection1 = new OleDbConnection(excelConnectionString);

                    var query = string.Format("Select * from [Allawance_Deduction$]");
                    //var query = string.Format("Select * from [{0}]", excelSheets[0]);

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


        private DataSet GetMemberDatasetFromFile22(HttpPostedFileBase file, out string validationMessage)
        {
            var ds = new DataSet();
            validationMessage = "";

            if (file != null && file.ContentLength > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    var ticks = DateTime.Now.Ticks;
                    var serverMappedPath = Server.MapPath("~/WebShared/Uploads/SalaryAllowanceDeductionImport/");
                    var fileLocation = $"{serverMappedPath}{ticks}/{file.FileName}";
                    var directory = $"{serverMappedPath}{ticks}";

                    try
                    {
                        // Ensure directory exists and save the file
                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }

                        file.SaveAs(fileLocation);
                    }
                    catch
                    {
                        validationMessage = "Error processing file. Please try again.";
                        return null;
                    }

                    // Updated connection string with TypeGuessRows=0 and ImportMixedTypes=Text
                    var excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation +
                        ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2;TypeGuessRows=0;ImportMixedTypes=Text\"";

                    using (var excelConnection = new OleDbConnection(excelConnectionString))
                    {
                        try
                        {
                            excelConnection.Open();
                            var query = "Select * from [Allawance_Deduction$]"; // Target worksheet name

                            using (var dataAdapter = new OleDbDataAdapter(query, excelConnection))
                            {
                                dataAdapter.Fill(ds); // Data is read as text, preserving alphanumeric values
                            }
                        }
                        catch (Exception ex)
                        {
                            validationMessage = $"Error reading file: {ex.Message}";
                            return null;
                        }
                    }
                }
                else
                {
                    validationMessage = "Invalid file format. Please use .xls or .xlsx files.";
                    return null;
                }
            }
            else
            {
                validationMessage = "No file uploaded.";
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

                    var serverMappedPath = Server.MapPath("~/WebShared/Uploads/SalaryAllowanceDeductionImport/");
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

                    //var excelSheets = new string[dt.Rows.Count];
                    //var t = 0;

                    ////excel data saves in temp file here.
                    //foreach (DataRow row in dt.Rows)
                    //{
                    //    excelSheets[t] = row["TABLE_NAME"].ToString();
                    //    t++;
                    //}

                    var excelConnection1 = new OleDbConnection(excelConnectionString);

                    var query = string.Format("Select * from [Allawance_Deduction$]");
                    //var query = string.Format("Select * from [{0}]", excelSheets[0]);

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


        private DataSet GetMemberDatasetFromFile3(HttpPostedFileBase file, out string validationMessage)
        {
            var ds = new DataSet();
            validationMessage = "";

            if (file == null || file.ContentLength <= 0)
            {
                validationMessage = "Error on file. Please try again.";
                return null;
            }

            var fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
            {
                validationMessage = "Error! Please import a correct Excel file (.xls or .xlsx).";
                return null;
            }

            try
            {
                var ticks = DateTime.Now.Ticks;
                var serverMappedPath = Server.MapPath("~/WebShared/Uploads/SalaryAllowanceDeductionImport/");
                var directory = Path.Combine(serverMappedPath, ticks.ToString());
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

                var fileLocation = Path.Combine(directory, file.FileName);
                file.SaveAs(fileLocation);

                using (var package = new ExcelPackage(new FileInfo(fileLocation)))
                {
                    foreach (var worksheet in package.Workbook.Worksheets)
                    {
                        var dt = new DataTable(worksheet.Name);

                        // Add columns
                        for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                        {
                            var columnName = worksheet.Cells[1, col].Text.Trim();
                            if (string.IsNullOrEmpty(columnName))
                                columnName = "Column" + col;
                            dt.Columns.Add(columnName, typeof(string)); // use string to handle alphanumeric codes
                        }

                        // Add rows
                        for (int row = worksheet.Dimension.Start.Row + 1; row <= worksheet.Dimension.End.Row; row++)
                        {
                            var dr = dt.NewRow();
                            for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                            {
                                dr[col - 1] = worksheet.Cells[row, col].Text.Trim();
                            }
                            dt.Rows.Add(dr);
                        }

                        ds.Tables.Add(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                validationMessage = "Error processing file: " + ex.Message;
                return null;
            }

            return ds;
        }



        private DataSet GetMemberDatasetFromFile4(HttpPostedFileBase file, out string validationMessage)
        {
            var ds = new DataSet();
            validationMessage = "";

            if (file == null || file.ContentLength <= 0)
            {
                validationMessage = "Error on file. Please try again.";
                return null;
            }

            var fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
            {
                validationMessage = "Error! Please import a correct Excel file (.xls or .xlsx).";
                return null;
            }

            try
            {
                var ticks = DateTime.Now.Ticks;
                var serverMappedPath = Server.MapPath("~/WebShared/Uploads/SalaryAllowanceDeductionImport/");
                var directory = Path.Combine(serverMappedPath, ticks.ToString());
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

                var fileLocation = Path.Combine(directory, file.FileName);
                file.SaveAs(fileLocation);

                using (var package = new ExcelPackage(new FileInfo(fileLocation)))
                {
                    foreach (var worksheet in package.Workbook.Worksheets)
                    {
                        if (worksheet.Dimension == null) continue; // skip empty sheet

                        var dt = new DataTable(worksheet.Name);

                        // Create columns from first row
                        for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                        {
                            var columnName = worksheet.Cells[1, col].Text.Trim();
                            if (string.IsNullOrEmpty(columnName))
                                columnName = "Column" + col;
                            dt.Columns.Add(columnName, typeof(string)); // keep as string
                        }

                        // Fill rows
                        for (int row = worksheet.Dimension.Start.Row + 1; row <= worksheet.Dimension.End.Row; row++)
                        {
                            var dr = dt.NewRow();
                            for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                            {
                                // Force raw text without auto date/number formatting
                                dr[col - 1] = worksheet.Cells[row, col].Text.Trim();
                            }
                            dt.Rows.Add(dr);
                        }

                        ds.Tables.Add(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                validationMessage = "Error processing file: " + ex.Message;
                return null;
            }

            return ds;
        }



        #endregion
    }
}