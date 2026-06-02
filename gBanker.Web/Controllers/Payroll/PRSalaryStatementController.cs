
#region Usings

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service;
using gHRM.Service.Basic;
using gHRM.Service.Payroll;
using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.Payroll;
using Microsoft.Ajax.Utilities;
using gHRM.Web.Infrastucture.Utility;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;

#endregion

namespace gHRM.Web.Controllers.Payroll
{
    public class PRSalaryStatementController : BaseController
    {
        #region Private Members
        private readonly IPRComponentService pRComponentService;
        private readonly IEmployeeSPService employeeSpService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IEmployeeDesignationService employeeDesignationService;
        private readonly IBankNameService bankNameService;
        private readonly IBankBranchService bankBranchService;
        private readonly IBankAccountService bankAccountService;
        private readonly IOfficeService officeService;
        private readonly IComponentPayrollService componentPayrollService;
        private CommonStaticDropDown commonStaticDropDown;
        private CommonDynamicDropDown commonDynamicDropDown;
        #endregion

        #region Ctor
        public PRSalaryStatementController(
                  IPRComponentService pRComponentService
                , IEmployeeSPService employeeSpService
                , IOfficeTypeService officeTypeService
                , IEmployeeService employeeService
                , IEmployeeDepartmentService employeeDepartmentService
                , IEmployeeDesignationService employeeDesignationService
                , IBankNameService bankNameService
                , IBankBranchService bankBranchService
                , IBankAccountService bankAccountService
                , IOfficeService officeService
                , IComponentPayrollService componentPayrollService
        )
        {
            this.pRComponentService = pRComponentService;
            this.employeeSpService = employeeSpService;
            this.officeTypeService = officeTypeService;
            this.employeeService = employeeService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.employeeDesignationService = employeeDesignationService;
            this.bankNameService = bankNameService;
            this.bankBranchService = bankBranchService;
            this.bankAccountService = bankAccountService;
            this.officeService = officeService;
            this.componentPayrollService = componentPayrollService;
            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion

        #region ActionMethods

        public ActionResult Report()
        {
            var model = new ComponentPayrollViewModel();
            MapDropdownForComponentPayrollReport(model);
            return View(model);
        }

        public ActionResult YearlySalaryStatement()
        {
            var model = new ComponentPayrollViewModel();
            MapDropdownForComponentPayrollReport(model);

            var currentDate = DateTime.Now;
            var fromDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            model.DateFrom = fromDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
            model.DateTo = (fromDate.AddMonths(1).AddDays(-1)).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

            return View(model);
        }

        public ActionResult OfficeWiseSalaryStatement()
        {
            var model = new ComponentPayrollViewModel();
            MapDropdownForComponentPayrollReport(model);
            return View(model);
        }

        public ActionResult StatementOfBankAdvice()
        {
            var model = new ComponentPayrollViewModel();
            MapDropdownForComponentPayrollReport(model);
            return View(model);
        }

        public ActionResult GroupBySalaryStatement()
        {
            var model = new ComponentPayrollViewModel();
            MapDropdownForGroupBySalaryStatement(model);
            return View(model);
        }

        public ActionResult EmployeeSalaryStatement()
        {
            var model = new ComponentPayrollViewModel();
            MapDropdownForComponentPayrollReport(model);

            var currentDate = DateTime.Now;
            var fromDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            model.DateFrom = fromDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
            model.DateTo = (fromDate.AddMonths(1).AddDays(-1)).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

            return View(model);
        }

        // Employee Payroll Statement        
        public ActionResult EmployeePayslipStatement()
        {
            var model = new ComponentPayrollViewModel();
            MapDropdownForComponentPayrollReport(model);

            var currentDate = DateTime.Now;
            var fromDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            model.DateFrom = fromDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
            model.DateTo = (fromDate.AddMonths(1).AddDays(-1)).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

            return View(model);
        }

        public JsonResult GetRelatedOfficeXOfficeType(int OfficeTypeId)
        {
            PRWorkAreaViewModel model = new PRWorkAreaViewModel(); // jahan will not call?
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

        public ActionResult OfficeWiseSalarySummary()
        {
            var model = new ComponentPayrollViewModel();
            MapDropdownForOfficeWiseSalarySummaryReport(model);
            return View(model);
        }

        //Salary Transfer Advice
        public ActionResult ApprovedReports()
        {
            ViewData["Months"] = Months();
            ViewData["Years"] = Years();

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;
            var model = new PRWorkAreaViewModel();
            mapBankDropDown(model);

            return View(model);
        }

        public ActionResult ApprovedReports2()
        {
            ViewData["Months"] = Months();
            ViewData["Years"] = Years();

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;
            var model = new PRWorkAreaViewModel();
            mapBankDropDown3(model);

            return View(model);
        }

        public ActionResult ApprovedReports3()
        {
            ViewData["Months"] = Months();
            ViewData["Years"] = Years();

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;
            var model = new PRWorkAreaViewModel();
            mapBankDropDown3(model);

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

            // Let's assume you're using Entity Framework or some data source




            return View(model);
        }


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

        //Fund Transfer Advice
        public ActionResult PayrollReports()  // jahan
        {
            var model = new PRWorkAreaViewModel();

            MapDropdownForOfficeTypeList(model);
            MapDropdownForOfficeList(model);

            ViewData["OfficeTypeList"] = model.OfficeTypeList;
            ViewData["OfficeList"] = model.OfficeList;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;
            //Populate salary processed by employee office info
            model = PopulateSalaryProcessedByEmployeeOfficeInfo(model);
            mapDropDownList(model);
            mapBankDropDown(model);
            return View(model);
        }

        public ActionResult PayrollReports3()
        {
            var entity = new PRWorkAreaViewModel();
            mapDropDownList(entity);
            mapBankDropDown3(entity);
            return View(entity);
        }

        public ActionResult PayrollReports2()
        {
            var entity = new PRWorkAreaViewModel();
            mapDropDownList2(entity);
            mapBankDropDown2(entity);
            return View(entity);
        }


        public ActionResult PayrollReports4()
        {
            var entity = new PRWorkAreaViewModel();
            mapDropDownList2(entity);
            mapBankDropDown2(entity);
            return View(entity);
        }


        //public ActionResult OfficeWiseApprovedSalary()
        //{
        //    var EmpId = SessionHelper.LoggedInEmployeeID;
        //    var entity = employeeService.GetByEmpId(Convert.ToInt64(EmpId));
        //    var model = new PRWorkAreaViewModel();
        //    var officeTypeId = officeService.GetById(Convert.ToInt32(entity.OfficeId)).OfficeTypeId;
        //    model.OfficeTypeId = officeTypeId;
        //    if (officeTypeId == 6)
        //    {
        //        var office = officeService.GetById(Convert.ToInt32(entity.OfficeId));
        //        model.AreaId =
        //            Convert.ToInt32(officeService.GetMany(o => o.OfficeCode == office.ThirdLevel).FirstOrDefault().OfficeId);

        //        model.ZoneId =
        //            Convert.ToInt32(officeService.GetMany(o => o.OfficeCode == office.SecondLevel).FirstOrDefault().OfficeId);
        //        model.UnitId = entity.OfficeId;
        //    }
        //    else if (officeTypeId == 5)
        //    {
        //        var office = officeService.GetById(Convert.ToInt32(entity.OfficeId));
        //        model.AreaId = entity.OfficeId;

        //        var a = officeService.GetMany(o => o.OfficeCode == office.SecondLevel.Trim());
        //        model.ZoneId =
        //            Convert.ToInt32(officeService.GetMany(o => o.OfficeCode == office.SecondLevel.Trim()).FirstOrDefault().OfficeId);
        //    }
        //    else if (officeTypeId == 4)
        //    {
        //        model.ZoneId = entity.OfficeId;
        //    }
        //    else if (officeTypeId == 3)
        //    {
        //        model.ProjectId = entity.OfficeId;
        //    }
        //    else if (officeTypeId == 1)
        //    {
        //        model.HeadOfficeId = entity.OfficeId;
        //    }
        //    ViewData["Months"] = Months();
        //    ViewData["Years"] = Years();

        //    IEnumerable<SelectListItem> items = new SelectList(" ");
        //    ViewData["ComponentList"] = items;
        //    mapBankDropDown(model);
        //    MapDropDownForOfficeNavigationPane(model);
        //    return View(model);
        //}


        #endregion

        #region HTTPRequest


        public JsonResult GetBankAccountList(int BranchId)
        {
            var finalList = new List<SelectListItem>();
            finalList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            try
            {
                //var bankCode=BankCode.Trim();
                //var bankId = bankNameService.Get(x => x.IsActive == true && x.BankCode == bankCode).Id;
                var accountList = bankAccountService.GetMany(x => x.IsActive == true && x.BranchId == BranchId);
                var viewList = accountList.AsEnumerable().Select(row => new SelectListItem
                {
                    Text = row.AccountNo,
                    Value = row.AccountId.ToString()
                }).ToList();
                finalList.AddRange(viewList);

            }
            catch (Exception e)
            {
                throw;
            }
            return Json(finalList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBankAccountList2(int BranchId)
        {
            var finalList = new List<SelectListItem>();
            //finalList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            try
            {
                //var bankCode=BankCode.Trim();
                //var bankId = bankNameService.Get(x => x.IsActive == true && x.BankCode == bankCode).Id;
                var accountList = bankAccountService.GetMany(x => x.IsActive == true && x.BranchId == BranchId);
                var viewList = accountList.AsEnumerable().Select(row => new SelectListItem
                {
                    Text = row.AccountNo,
                    Value = row.AccountId.ToString()
                }).ToList();
                finalList.AddRange(viewList);

            }
            catch (Exception e)
            {
                throw;
            }
            return Json(finalList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBankBranchList(string BankCode)
        {
            var finalList = new List<SelectListItem>();
            finalList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            try
            {
                var bankCode = BankCode.Trim();
                var bankId = bankNameService.Get(x => x.IsActive == true && x.BankCode == bankCode).Id;
                var branchList = bankBranchService.GetMany(x => x.IsActive == true && x.BankId == bankId);
                var viewList = branchList.AsEnumerable().Select(row => new SelectListItem
                {
                    Text = row.BranchName,
                    Value = row.BranchId.ToString()
                }).ToList();
                finalList.AddRange(viewList);

            }
            catch (Exception e)
            {
                throw;
            }
            return Json(finalList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBankBranchList2(string BankCode)
        {
            var finalList = new List<SelectListItem>();
            //finalList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            try
            {
                var bankCode = BankCode.Trim();
                var bankId = bankNameService.Get(x => x.IsActive == true && x.BankCode == bankCode).Id;
                var branchList = bankBranchService.GetMany(x => x.IsActive == true && x.BankId == bankId);
                var viewList = branchList.AsEnumerable().Select(row => new SelectListItem
                {
                    Text = row.BranchName,
                    Value = row.BranchId.ToString()
                }).ToList();
                finalList.AddRange(viewList);

            }
            catch (Exception e)
            {
                throw;
            }
            return Json(finalList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadTypeWiseComponentList(string componentType)
        {
            var compList = new List<ComponentPayroll>();
            var viewCompList = new List<SelectListItem>();

            if (componentType == "In")
            {
                compList = componentPayrollService.GetMany(p => p.ComponentCategory == "Allowance").ToList();
                viewCompList = compList.AsEnumerable().Select(p => new SelectListItem
                {
                    Text = p.ComponentName,
                    Value = p.ComponentName
                }).ToList();
            }
            if (componentType == "De")
            {
                compList = componentPayrollService.GetMany(p => p.ComponentCategory == "Deduction").ToList();
                viewCompList = compList.AsEnumerable().Select(p => new SelectListItem
                {
                    Text = p.ComponentName,
                    Value = p.ComponentName
                }).ToList();
            }

            return Json(viewCompList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Fund Transfer Advice Reports
        //jahan Fund Transfer Application
        public ActionResult PrintFundTransferApplicationReport(int OfficeTypeId, string BankCode, int BranchId, string AccountId, int Month, int Year, string SalaryType, int OfficeId)

        {
            try
            {

                StringBuilder sb = new StringBuilder();

                var employee = employeeService.GetByEmpId(Convert.ToInt64(LoggedInEmployeeId));
                var employeeName = employee.EmployeeName?.ToString() ?? string.Empty;
                var email = employee.Email?.ToString() ?? string.Empty;
                var mobileNumber = employee.ContactNo1?.ToString() ?? string.Empty;
                var companyName = SessionHelper.CompanyName?.ToString() ?? string.Empty;

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("EmployeeName", employeeName);
                reportParam.Add("Email", email);
                reportParam.Add("MobileNumber", mobileNumber);
                reportParam.Add("CompanyName", companyName);

                if (SalaryType == "Salary")
                {
                    var reportParama = new Dictionary<string, object>();

                    if (OfficeTypeId == 10000)
                    {
                        var parama = new { BankCode = BankCode.Trim(), BankAccountId = AccountId, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId };
                        var dataa = employeeSpService.GetDataWithParameter(parama, "prl.SP_Report_Salary_FundTransferApplicationForAllOffice");


                        if (SessionHelper.CompanyInfo.CompanyShortName == GHRMPlusCompanyConstants.GrameenCommunications)
                            ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationToBankForGC.rpt", dataa.Tables[0], reportParama);
                        else
                            ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationToBank.rpt", dataa.Tables[0], reportParam);

                        return Content(string.Empty);
                    }
             //jahan
                    var param = new { BankCode = BankCode.Trim(), BankAccountId = AccountId, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId, OfficeId = OfficeId };

                    DataSet data;

                    if (SessionHelper.CompanyInfo.CompanyShortName == GHRMPlusCompanyConstants.GT) data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Salary_FundTransferApplication_GT");
                    else
                        data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Salary_FundTransferApplication");




                    if (SessionHelper.CompanyInfo.CompanyShortName == GHRMPlusCompanyConstants.GrameenCommunications)
                    {
                        ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationToBankForGC.rpt", data.Tables[0], reportParama);
                    }
                    else if (SessionHelper.CompanyInfo.CompanyShortName == GHRMPlusCompanyConstants.GT)
                    {
                        reportParam.Add("Month", Month);
                        reportParam.Add("Year", Year);
                        ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationToBank_GT.rpt", data.Tables[0], reportParam);
                    }
                    else
                    {
                        ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationToBank.rpt", data.Tables[0], reportParam);
                    }

                }
                if (SalaryType == "Bonus for Eid-ul-Fitre")
                {
                    if (OfficeTypeId == 1)
                    {
                        var parama = new { BankCode = BankCode.Trim(), BankAccountId = AccountId, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId, ComponentName = "Eid-Ul-Fitr Bonus",OfficeId=OfficeId };
                        var dataa = employeeSpService.GetDataWithParameter(parama, "prl.SP_Report_Salary_Bonus_FundTransferApplicationForAllOffice");
                        var reportParama = new Dictionary<string, object>();
                        reportParama.Add("EmployeeName", employeeName);
                        reportParama.Add("Email", email);
                        reportParama.Add("MobileNumber", mobileNumber);
                        reportParama.Add("CompanyName", companyName);
                        ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationToBank.rpt", dataa.Tables[0], reportParama);
                    }
                    var param = new { BankCode = BankCode.Trim(), BankAccountId = AccountId, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId, ComponentName = "Eid-Ul-Fitr Bonus", OfficeId = OfficeId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Salary_Bonus_FundTransferApplication");
                    reportParam = new Dictionary<string, object>();
                    reportParam.Add("EmployeeName", employeeName);
                    reportParam.Add("Email", email);
                    reportParam.Add("MobileNumber", mobileNumber);
                    reportParam.Add("CompanyName", companyName);
                    ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationToBank.rpt", data.Tables[0], reportParam);
                }

                if (SalaryType == "Bonus for Eid-ul-Azha")
                {
                    if (OfficeTypeId == 1)
                    {
                        var parama = new { BankCode = BankCode.Trim(), BankAccountId = AccountId, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId, ComponentName = "Eid-Ul-Azha Bonus", OfficeId = OfficeId };
                        var dataa = employeeSpService.GetDataWithParameter(parama, "prl.SP_Report_Salary_Bonus_FundTransferApplicationForAllOffice");
                        var reportParama = new Dictionary<string, object>();
                        reportParama.Add("EmployeeName", employeeName);
                        reportParama.Add("Email", email);
                        reportParama.Add("MobileNumber", mobileNumber);
                        reportParama.Add("CompanyName", companyName);
                        ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationToBank.rpt", dataa.Tables[0], reportParama);
                    }
                    var param = new { BankCode = BankCode.Trim(), BankAccountId = AccountId, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId, ComponentName = "Eid-Ul-Azha Bonus", OfficeId = OfficeId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Salary_Bonus_FundTransferApplication");
                    reportParam = new Dictionary<string, object>();
                    reportParam.Add("EmployeeName", employeeName);
                    reportParam.Add("Email", email);
                    reportParam.Add("MobileNumber", mobileNumber);
                    reportParam.Add("CompanyName", companyName);
                    ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationToBank.rpt", data.Tables[0], reportParam);
                }

                if (SalaryType == "Incentive")
                {
                    if (OfficeTypeId == 1)
                    {
                        var parama = new { BankCode = BankCode.Trim(), BankAccountId = AccountId, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId, ComponentName = "Eid-Ul-Fitr Bonus", OfficeId = OfficeId };
                        var dataa = employeeSpService.GetDataWithParameter(parama, "prl.SP_Report_Salary_Bonus_FundTransferApplicationForAllOffice");
                        var reportParama = new Dictionary<string, object>();
                        reportParama.Add("EmployeeName", employeeName);
                        reportParama.Add("Email", email);
                        reportParama.Add("MobileNumber", mobileNumber);
                        reportParama.Add("CompanyName", companyName);
                        ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationToBank.rpt", dataa.Tables[0], reportParama);
                    }
                    var param = new { BankCode = BankCode.Trim(), BankAccountId = AccountId, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId, ComponentName = "Incentive Bonus", OfficeId = OfficeId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Salary_Bonus_FundTransferApplication");
                    reportParam = new Dictionary<string, object>();
                    reportParam.Add("EmployeeName", employeeName);
                    reportParam.Add("Email", email);
                    reportParam.Add("MobileNumber", mobileNumber);
                    reportParam.Add("CompanyName", companyName);
                    ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationToBank.rpt", data.Tables[0], reportParam);
                }

                return Content(string.Empty);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public ActionResult PrintFundTransferAdviceReport(int OfficeTypeId, string BankCode, int BranchId, int Month, int Year, string SalaryType, string accountId, int officeId)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (SalaryType == "Salary")
                {
                    var param = new { SalaryYear = Year, SalaryMonth = Month, BankName = BankCode, OfficeTypeID = OfficeTypeId, officeId= officeId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.Report_Salary_ByOfficeType");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);

                    if (SessionHelper.CompanyInfo.CompanyShortName == GHRMPlusCompanyConstants.GT)
                    {
                        reportParam.Add("Month", Month);
                        reportParam.Add("Year", Year);
                        reportParam.Add("accountId", accountId);
                        reportParam.Add("officeId", officeId); //jahan O capital or small?

                        ReportHelper.PrintReport("Payroll/rpt_SalaryAdviceForBank_GT.rpt", data.Tables[0], reportParam);
                    }
                    else
                    {//parameter should here?
                        ReportHelper.PrintReport("Payroll/rpt_SalaryAdviceForBank.rpt", data.Tables[0], reportParam);
                    }

                }
                if (SalaryType == "Bonus for Eid-ul-Fitre")
                {
                    var param = new { SalaryYear = Year, SalaryMonth = Month, BankName = BankCode, OfficeTypeID = OfficeTypeId, BonusComopnentName = "Eid-Ul-Fitr Bonus", OfficeId=officeId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.Report_Salary_Bonus_ByOfficeType");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                    ReportHelper.PrintReport("Payroll/rpt_SalaryAdviceForBank.rpt", data.Tables[0], reportParam);
                }

                if (SalaryType == "Bonus for Eid-ul-Azha")
                {
                    var param = new { SalaryYear = Year, SalaryMonth = Month, BankName = BankCode, OfficeTypeID = OfficeTypeId, officeId=officeId, BonusComopnentName = "Eid-Ul-Azha Bonus", OfficeId = officeId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.Report_Salary_Bonus_ByOfficeType");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                    ReportHelper.PrintReport("Payroll/rpt_SalaryAdviceForBank.rpt", data.Tables[0], reportParam);
                }

                if (SalaryType == "Incentive")
                {
                    var param = new { SalaryYear = Year, SalaryMonth = Month, BankName = BankCode, OfficeTypeID = OfficeTypeId, BonusComopnentName = "Incentive Bonus",OfficeId = officeId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.Report_Salary_Bonus_ByOfficeType");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                    ReportHelper.PrintReport("Payroll/rpt_SalaryAdviceForBank.rpt", data.Tables[0], reportParam);
                }

                return Content(string.Empty);
                //if (OfficeTypeId == 1)
                //{
                //    sb.Append(" AND ve.OfficeTypeId=1");
                //}
                //else
                //{
                //    sb.Append(" AND ve.OfficeTypeId!=1");
                //}

                //if(SalaryType=="")
                //{ }

                //var param = new { BankName = BankCode.Trim(), SalaryYear = Year, SalaryMonth = Month, SalaryType = SalaryType.Trim(), AndCondition = sb.ToString() };
                //var data = employeeSpService.GetDataWithParameter(param, "SP_rpt_BankSalaryAdvice");
                //var reportParam = new Dictionary<string, object>();
                //reportParam.Add("CompanyName", SessionHelper.CompanyName);
                //reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                //ReportHelper.PrintReport("Payroll/rpt_SalaryAdviceForBank.rpt", data.Tables[0], reportParam);
                //return Content(string.Empty);

            }
            catch (Exception e)
            {
                throw;
            }
        }


        public ActionResult PrintFundTransferAdviceReport_excel(int OfficeTypeId, string BankCode, int BranchId, int Month, int Year, string SalaryType, string accountId, int officeId)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (SalaryType == "Salary")
                {
                    var param = new { SalaryYear = Year, SalaryMonth = Month, BankName = BankCode, OfficeTypeID = OfficeTypeId, OfficeId = officeId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.Report_Salary_ByOfficeType");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);

                    if (SessionHelper.CompanyInfo.CompanyShortName == GHRMPlusCompanyConstants.GT)
                    {
                        reportParam.Add("Month", Month);
                        reportParam.Add("Year", Year);
                        reportParam.Add("accountId", accountId);

                        ReportHelper.ExportExcelReport("Payroll/rpt_SalaryAdviceForBank_GT_Excel.rpt", data.Tables[0], reportParam);
                    }
                    else
                    {
                        ReportHelper.ExportExcelReport("Payroll/rpt_SalaryAdviceForBank.rpt", data.Tables[0], reportParam);
                    }

                }
                if (SalaryType == "Bonus for Eid-ul-Fitre")
                {
                    var param = new { SalaryYear = Year, SalaryMonth = Month, BankName = BankCode, OfficeTypeID = OfficeTypeId, BonusComopnentName = "Eid-Ul-Fitr Bonus", OfficeId = officeId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.Report_Salary_Bonus_ByOfficeType");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                    ReportHelper.ExportExcelReport("Payroll/rpt_SalaryAdviceForBank.rpt", data.Tables[0], reportParam);
                }

                if (SalaryType == "Bonus for Eid-ul-Azha")
                {
                    var param = new { SalaryYear = Year, SalaryMonth = Month, BankName = BankCode, OfficeTypeID = OfficeTypeId, BonusComopnentName = "Eid-Ul-Azha Bonus", OfficeId = officeId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.Report_Salary_Bonus_ByOfficeType");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                    ReportHelper.ExportExcelReport("Payroll/rpt_SalaryAdviceForBank.rpt", data.Tables[0], reportParam);
                }

                if (SalaryType == "Incentive")
                {
                    var param = new { SalaryYear = Year, SalaryMonth = Month, BankName = BankCode, OfficeTypeID = OfficeTypeId, BonusComopnentName = "Incentive Bonus", OfficeId = officeId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.Report_Salary_Bonus_ByOfficeType");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                    ReportHelper.ExportExcelReport("Payroll/rpt_SalaryAdviceForBank.rpt", data.Tables[0], reportParam);
                }

                return Content(string.Empty);
                //if (OfficeTypeId == 1)
                //{
                //    sb.Append(" AND ve.OfficeTypeId=1");
                //}
                //else
                //{
                //    sb.Append(" AND ve.OfficeTypeId!=1");
                //}

                //if(SalaryType=="")
                //{ }

                //var param = new { BankName = BankCode.Trim(), SalaryYear = Year, SalaryMonth = Month, SalaryType = SalaryType.Trim(), AndCondition = sb.ToString() };
                //var data = employeeSpService.GetDataWithParameter(param, "SP_rpt_BankSalaryAdvice");
                //var reportParam = new Dictionary<string, object>();
                //reportParam.Add("CompanyName", SessionHelper.CompanyName);
                //reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                //ReportHelper.PrintReport("Payroll/rpt_SalaryAdviceForBank.rpt", data.Tables[0], reportParam);
                //return Content(string.Empty);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public ActionResult PrintFundTransferAdviceReportExcel(int OfficeTypeId, string BankCode, int BranchId, int Month, int Year, string SalaryType, int officeId)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (SalaryType == "Salary")
                {
                    var param = new { SalaryYear = Year, SalaryMonth = Month, BankName = BankCode, OfficeTypeID = OfficeTypeId, OfficeId = officeId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.Report_Salary_ByOfficeType");

                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                    ReportHelper.ExportExcelReport("Payroll/rpt_SalaryAdviceForBank.rpt", data.Tables[0], reportParam);
                }

                if (SalaryType == "Bonus for Eid-ul-Fitre")
                {
                    var param = new { SalaryYear = Year, SalaryMonth = Month, BankName = BankCode, OfficeTypeID = OfficeTypeId, BonusComopnentName = "Eid-Ul-Fitr Bonus", OfficeId = officeId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.Report_Salary_Bonus_ByOfficeType");

                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                    ReportHelper.ExportExcelReport("Payroll/rpt_SalaryAdviceForBank.rpt", data.Tables[0], reportParam);
                }

                if (SalaryType == "Bonus for Eid-ul-Azha")
                {
                    var param = new { SalaryYear = Year, SalaryMonth = Month, BankName = BankCode, OfficeTypeID = OfficeTypeId, BonusComopnentName = "Eid-Ul-Azha Bonus", OfficeId = officeId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.Report_Salary_Bonus_ByOfficeType");

                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                    ReportHelper.ExportExcelReport("Payroll/rpt_SalaryAdviceForBank.rpt", data.Tables[0], reportParam);
                }

                if (SalaryType == "Incentive")
                {
                    var param = new { SalaryYear = Year, SalaryMonth = Month, BankName = BankCode, OfficeTypeID = OfficeTypeId, BonusComopnentName = "Incentive Bonus", OfficeId = officeId }; //
                    var data = employeeSpService.GetDataWithParameter(param, "prl.Report_Salary_Bonus_ByOfficeType");

                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", SessionHelper.CompanyName);
                    reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                    ReportHelper.ExportExcelReport("Payroll/rpt_SalaryAdviceForBank.rpt", data.Tables[0], reportParam);
                }

                return Content(string.Empty);

            }
            catch (Exception e)
            {
                throw;
            }
        }


        public ActionResult PrintFundTransferApplicationAndAdviceReport2(int OfficeTypeId, string BankCode, int? BranchId, int? AccountId, int Month, int Year, string SalaryType, string PersonToContactFromBankId, string format)

        {
            try
            {
                if (null == BranchId) BranchId = 0;
                if (null == AccountId) AccountId = 0;
                StringBuilder sb = new StringBuilder();
                string FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE = AppSetting.Get(AppSetting.FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE, HttpContext);

                var employee = employeeService.GetByEmpId(Convert.ToInt64(LoggedInEmployeeId));
                var employeeName = employee.EmployeeName?.ToString() ?? string.Empty;
                var email = employee.Email?.ToString() ?? string.Empty;
                var mobileNumber = employee.ContactNo1?.ToString() ?? string.Empty;
                var companyName = SessionHelper.CompanyName?.ToString() ?? string.Empty;

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("EmployeeName", employeeName);
                reportParam.Add("Email", email);
                reportParam.Add("MobileNumber", mobileNumber);
                reportParam.Add("CompanyName", companyName);

                if (SalaryType == "Salary")
                {
                    var reportParama = new Dictionary<string, object>();

                    var param = new { BankCode = BankCode.Trim(), BankAccountId = AccountId.Value, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId.Value, PersonToContactFromBankId = PersonToContactFromBankId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Salary_FundTransferApplicationAndAdvice_test");

                    if ("GSSB" == FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE)
                    {
                        if (format.ToLower() == "pdf")
                            ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GSSB.rpt", data.Tables[0], reportParam);
                        else if (format.ToLower() == "excel")
                            ReportHelper.ExportExcelReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GSSB.rpt", data.Tables[0], reportParam);
                    }
                    else if (SessionHelper.CompanyInfo.CompanyShortName == "GT")
                    {
                        ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GT.rpt", data.Tables[0], reportParam);
                    }
                    else
                    {
                        if(format == "excel")
                        ReportHelper.ExportExcelReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GTT.rpt", data.Tables[0], reportParam);
                        else 
                        ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GTT.rpt", data.Tables[0], reportParam);
                    }
                }
                else if (SalaryType == "Bonus for Eid-ul-Fitre" || SalaryType == "Bonus for Eid-ul-Azha")
                {
                    var reportParama = new Dictionary<string, object>();

                    var param = new { BankCode = BankCode.Trim(), BankAccountId = AccountId.Value, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId.Value, PersonToContactFromBankId = PersonToContactFromBankId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Bonus_FundTransferApplicationAndAdvice");

                    if ("GSSB" == FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE)
                    {
                        if (format.ToLower() == "pdf")
                            ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GSSB.rpt", data.Tables[0], reportParam);
                        else if (format.ToLower() == "excel")
                            ReportHelper.ExportExcelReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GSSB.rpt", data.Tables[0], reportParam);
                    }
                    else
                    {
                        ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank.rpt", data.Tables[0], reportParam);
                    }
                }

                return Content(string.Empty);
            }
            catch (Exception e)
            {
                throw;
            }
        }




        public ActionResult PrintFundTransferApplicationAndAdviceReport_2025(int OfficeTypeId, string BankCode, int? BranchId, int? AccountId, int Month, int Year, string SalaryType, string PersonToContactFromBankId, string format)
        {
            try
            {
               

                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryYear", Value = Year.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryMonth", Value = Month.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (OfficeTypeId).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (BranchId ?? 0).ToString() });



                var salaryDate = db.SalaryDateConfigs.Select(z => z.DayOfMonthlySalary).FirstOrDefault();
                var SalaryDate = Year + "-" + Month + "-" + salaryDate;

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryDate", Value = SalaryDate });

                // Signature 1
                var asigId1 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ASignatureId).FirstOrDefault();
                var Signature1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.EmployeeName).FirstOrDefault();
                var desigId1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.DesignationId).FirstOrDefault();
                var Designation1 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId1).Select(z => z.DesignationName).FirstOrDefault();

                // Signature 2
                var asigId2 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.BSignatureId).FirstOrDefault();
                var Signature2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.EmployeeName).FirstOrDefault();
                var desigId2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.DesignationId).FirstOrDefault();
                var Designation2 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId2).Select(z => z.DesignationName).FirstOrDefault();

                // Signature 3
                var asigId3 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.CSignatureId).FirstOrDefault();
                var Signature3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.EmployeeName).FirstOrDefault();
                var desigId3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.DesignationId).FirstOrDefault();
                var Designation3 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId3).Select(z => z.DesignationName).FirstOrDefault();

                // Signature 4
                var asigId4 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.DSignatureId).FirstOrDefault();
                var Signature4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.EmployeeName).FirstOrDefault();
                var desigId4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.DesignationId).FirstOrDefault();
                var Designation4 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId4).Select(z => z.DesignationName).FirstOrDefault();

                // Signature 5
                var asigId5 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ESignatureId).FirstOrDefault();
                var Signature5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.EmployeeName).FirstOrDefault();
                var desigId5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.DesignationId).FirstOrDefault();
                var Designation5 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId5).Select(z => z.DesignationName).FirstOrDefault();

                // Signature 6
                var asigId6 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.FSignatureId).FirstOrDefault();
                var Signature6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.EmployeeName).FirstOrDefault();
                var desigId6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.DesignationId).FirstOrDefault();
                var Designation6 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId6).Select(z => z.DesignationName).FirstOrDefault();

                // Add Signatures to Parameters
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature1", Value = Signature1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature2", Value = Signature2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature3", Value = Signature3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature4", Value = Signature4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature5", Value = Signature5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature6", Value = Signature6 });

                // Add Designations to Parameters
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation1", Value = Designation1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation2", Value = Designation2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation3", Value = Designation3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation4", Value = Designation4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation5", Value = Designation5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation6", Value = Designation6 });


                PrintSSRSReport("/gHRMPlus_Reports/SSRS_SalaryAdvice", paramValues.ToArray());

                return Content(string.Empty);
            }
            catch (Exception e)
            {
                throw;
            }
        }



        public ActionResult PrintFundTransferApplicationAndAdviceReport(int OfficeTypeId, string BankCode, int? BranchId, int? AccountId, int Month, int Year, string SalaryType, long PersonToContactFromBankId, string format,int OfficeId)

        {
            try
            {
                if (null == BranchId) BranchId = 0;
                if (null == AccountId) AccountId = 0;
                StringBuilder sb = new StringBuilder();
                string FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE = AppSetting.Get(AppSetting.FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE, HttpContext);

                var employee = employeeService.GetByEmpId(Convert.ToInt64(LoggedInEmployeeId));
                var employeeName = employee.EmployeeName?.ToString() ?? string.Empty;
                var email = employee.Email?.ToString() ?? string.Empty;
                var mobileNumber = employee.ContactNo1?.ToString() ?? string.Empty;
                var companyName = SessionHelper.CompanyName?.ToString() ?? string.Empty;

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("EmployeeName", employeeName);
                reportParam.Add("Email", email);
                reportParam.Add("MobileNumber", mobileNumber);
                reportParam.Add("CompanyName", companyName);

                if (SalaryType == "Salary")
                {
                    var reportParama = new Dictionary<string, object>();

                    var param = new { BankCode = BankCode.Trim(), BankAccountId = AccountId.Value, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId.Value, PersonToContactFromBankId = PersonToContactFromBankId,OfficeId=OfficeId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Salary_FundTransferApplicationAndAdvice");

                    if ("GSSB" == FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE)
                    {
                        if (format.ToLower() == "pdf")
                            ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GSSB.rpt", data.Tables[0], reportParam);
                        else if (format.ToLower() == "excel")
                            ReportHelper.ExportExcelReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GSSB.rpt", data.Tables[0], reportParam);
                    } 
                    else if(SessionHelper.CompanyInfo.CompanyShortName == "GT")
                    {
                        ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GT.rpt", data.Tables[0], reportParam);
                    }
                    else
                    {
                        ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank.rpt", data.Tables[0], reportParam);
                    }
                }
                else if (SalaryType == "Bonus for Eid-ul-Fitre" || SalaryType == "Bonus for Eid-ul-Azha")//jahan
                {
                    var reportParama = new Dictionary<string, object>();

                    var param = new { BankCode = BankCode.Trim(), BankAccountId = AccountId.Value, OfficeTypeId = OfficeTypeId, SalaryType = SalaryType.Trim(), SalaryYear = Year, SalaryMonth = Month, BranchId = BranchId.Value, PersonToContactFromBankId = PersonToContactFromBankId };
                    var data = employeeSpService.GetDataWithParameter(param, "prl.SP_Report_Bonus_FundTransferApplicationAndAdvice");

                    if ("GSSB" == FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE)
                    {
                        if (format.ToLower() == "pdf")
                            ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GSSB.rpt", data.Tables[0], reportParam);
                        else if (format.ToLower() == "excel")
                            ReportHelper.ExportExcelReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank_GSSB.rpt", data.Tables[0], reportParam);
                    }
                    else
                    {
                        ReportHelper.PrintReport("Payroll/rpt_FundTransferApplicationAndAdviceToBank.rpt", data.Tables[0], reportParam);
                    }
                }                

                return Content(string.Empty);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
        //jahan
        public ActionResult PrintComponentWiseSalary(int month, int year, string componentName, string componentType, int officeId)
        {
            try
            {
                var firsDateOfMonth = new DateTime(year, month, 1);
                DateTime firstOfNextMonth = new DateTime(year, month, 1).AddMonths(1);
                var lastDateOfMonth = firstOfNextMonth.AddDays(-1);
                var param = new { FirsDateOfMonth = firsDateOfMonth, LastDateOfMonth = lastDateOfMonth, ComponentName = componentName, ComponentType = componentType, OfficeId = officeId };
                var dataSet = employeeSpService.GetDataWithParameter(param, "prl.SP_rpt_GetComponentWiseSalary");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", SessionHelper.CompanyName);
                reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                ReportHelper.PrintReport("Payroll/rpt_ComponentWiseSalary.rpt", dataSet.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion

        #region Salary Transfer Advice Reports

        public ActionResult PrintSalaryBankSummaryReportBeforeApproval(int Year, int Month, int OfficeTypeId, string BankName)
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId, BankName = BankName };
                var salaryData = employeeSpService.GetDataWithParameter(param, "prl.SP_rpt_View_EmployeePaySleepBeforeApproval");

                var reportParam = new Dictionary<string, object>();
                ReportHelper.ExportExcelReport("Payroll/RPT_View_EmployeePaySleep.rpt", salaryData.Tables[0], reportParam);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintSalaryBankSummaryReportBeforeApproval2(int Year, int Month, int OfficeTypeId, string BankName, int OfficeId )
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId, BankName = BankName, OfficeId = OfficeId };
                var salaryData = employeeSpService.GetDataWithParameter(param, "prl.SP_rpt_View_EmployeePaySleepBeforeApproval2");

                var reportParam = new Dictionary<string, object>();
                ReportHelper.ExportExcelReport("Payroll/RPT_View_EmployeePaySleep.rpt", salaryData.Tables[0], reportParam);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintSalaryBankSummaryReportBeforeApprovalPDF(int Year, int Month, int OfficeTypeId, string BankName)
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId, BankName = BankName };
                var salaryData = employeeSpService.GetDataWithParameter(param, "prl.SP_rpt_View_EmployeePaySleepBeforeApproval");

                var reportParam = new Dictionary<string, object>();
                if(SessionHelper.CompanyInfo.CompanyShortName == "Prottyashi")
                    ReportHelper.PrintReport("Payroll/RPT_View_EmployeePaySleep_Prottayshi.rpt", salaryData.Tables[0], reportParam);
                else
                    ReportHelper.PrintReport("Payroll/RPT_View_EmployeePaySleep.rpt", salaryData.Tables[0], reportParam);

                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintSalaryBankSummaryReportBeforeApprovalPDF2(int Year, int Month, int OfficeTypeId, string BankName, int OfficeId )
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId, BankName = BankName, OfficeId = OfficeId };
                var salaryData = employeeSpService.GetDataWithParameter(param, "prl.SP_rpt_View_EmployeePaySleepBeforeApproval2");

                var reportParam = new Dictionary<string, object>();
                if (SessionHelper.CompanyInfo.CompanyShortName == "Prottyashi")
                    ReportHelper.PrintReport("Payroll/RPT_View_EmployeePaySleep_Prottayshi.rpt", salaryData.Tables[0], reportParam);
                else
                    ReportHelper.PrintReport("Payroll/RPT_View_EmployeePaySleep.rpt", salaryData.Tables[0], reportParam);

                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintSalaryBankSummaryReportAfterApproval(int Year, int Month, int OfficeTypeId, string BankName)
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId, BankName = BankName };
                var salaryData = employeeSpService.GetDataWithParameter(param, "prl.SP_rpt_View_EmployeePaySleepAfterApproval");

                var reportParam = new Dictionary<string, object>();
                ReportHelper.ExportExcelReport("Payroll/RPT_View_EmployeePaySleep.rpt", salaryData.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintSalaryBankSummaryReportAfterApproval2(int Year, int Month, int OfficeTypeId, string BankName, int OfficeId )
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId, BankName = BankName, OfficeId = OfficeId };
                var salaryData = employeeSpService.GetDataWithParameter(param, "prl.SP_rpt_View_EmployeePaySleepAfterApproval2");

                var reportParam = new Dictionary<string, object>();
                ReportHelper.ExportExcelReport("Payroll/RPT_View_EmployeePaySleep.rpt", salaryData.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintSalaryBankSummaryReportAfterApprovalPDF(int Year, int Month, int OfficeTypeId, string BankName)
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId, BankName = BankName };
                var salaryData = employeeSpService.GetDataWithParameter(param, "prl.SP_rpt_View_EmployeePaySleepAfterApproval");

                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Payroll/RPT_View_EmployeePaySleep.rpt", salaryData.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult PrintSalaryBankSummaryReportAfterApprovalPDF2(int Year, int Month, int OfficeTypeId, string BankName, int OfficeId )
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId, BankName = BankName , OfficeId = OfficeId };
                var salaryData = employeeSpService.GetDataWithParameter(param, "prl.SP_rpt_View_EmployeePaySleepAfterApproval2");

                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Payroll/RPT_View_EmployeePaySleep.rpt", salaryData.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Salary statement

        public JsonResult GetDepartmentAndDesignationByEmployee(int DepartmentId, int DesignationId)
        {
            var EmployeeList = employeeService.GetMany(w => w.DepartmentId == DepartmentId && w.DesignationId == DesignationId && w.IsActive == true).ToList();

            var viewEmployeeList = EmployeeList.OrderBy(x => x.EmployeeId).Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.EmployeeId.ToString(),
                Text = x.EmployeeName.ToString()
            });

            var Employee_items = new List<SelectListItem>();
            Employee_items.AddRange(viewEmployeeList);

            return Json(Employee_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCategoryByCategoryGroup(string ComponentCategory)
        {
            var ComponentCategoryList = pRComponentService.GetMany(w => w.ComponentCategory == ComponentCategory && w.IsActive == true).ToList();

            var viewComponentCategoryList = ComponentCategoryList.DistinctBy(d => d.ComponentName).OrderBy(x => x.ComponentName).Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.PRComponentID.ToString(),
                Text = x.ComponentName.ToString()
            });

            var Component_items = new List<SelectListItem>();
            Component_items.AddRange(viewComponentCategoryList);

            return Json(Component_items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmployeeSalaryStatementReport(string DateFrom, string DateTo, string EmployeeCode, string EmployeeNameSig, string DepartmentNameSig, string DesignationNameSig)
        {
            try
            {
                var param = new { DateFrom = DateFrom, DateTo = DateTo, EmployeeCode = EmployeeCode, EmployeeNameSig = EmployeeNameSig, DepartmentNameSig = DepartmentNameSig, DesignationNameSig = DesignationNameSig };
                var dataSource = employeeSpService.GetDataWithParameter(param, "prl.SP_EmployeeSalaryStatement");             
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                reportParam.Add("EmployeeNameSig", EmployeeNameSig);
                reportParam.Add("DepartmentNameSig", DepartmentNameSig);
                reportParam.Add("DesignationNameSig", DesignationNameSig);

                var dtCompanyInfo = WebHelper.GetCompanyInfo();

                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";
                var reportPartialPath = "Payroll/rpt_EmployeeSalaryStatement.rpt";

                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, dataSource.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }




        public ActionResult EmployeeWiseLeaveReportPrint2(string dateFrom = "2025-01-01", string dateTo = "2025-01-01", string employeeCode = "240020")
        {
            try
            {
                string empCode;

                using (var db = new gHRMDBContext())
                {
                    if (string.IsNullOrWhiteSpace(employeeCode))
                    {
                        empCode = db.Employees
                                    .Where(z => z.EmployeeId == LoggedInEmployeeId)
                                    .Select(k => k.EmployeeCode)
                                    .FirstOrDefault();
                    }
                    else
                    {
                        empCode = employeeCode;
                    }
                }

                empCode = string.IsNullOrWhiteSpace(empCode) ? "0" : empCode;

                var paramValues = new List<Service.ReportExecutionService.ParameterValue>
        {
            new Service.ReportExecutionService.ParameterValue { Name = "CompanyName", Value = SessionHelper.CompanyName },
            new Service.ReportExecutionService.ParameterValue { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress },
            new Service.ReportExecutionService.ParameterValue { Name = "EmployeeCode", Value = empCode },
            new Service.ReportExecutionService.ParameterValue { Name = "Employee_Code", Value = empCode },
            new Service.ReportExecutionService.ParameterValue { Name = "DateFrom", Value = dateFrom },
            new Service.ReportExecutionService.ParameterValue { Name = "DateTo", Value = dateTo },
            new Service.ReportExecutionService.ParameterValue { Name = "Date_From", Value = dateFrom },
            new Service.ReportExecutionService.ParameterValue { Name = "Date_To", Value = dateTo }
        };

                PrintSSRSReport("/gHRMPlus_Reports/LeaveSummeryAndDetails", paramValues.ToArray());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                var errorMessage = SessionHelper.UserFullName.ToLower().Contains("super")
                    ? ex.Message
                    : "No Data Found";

                return Json(new { Result = "ERROR", Message = errorMessage }, JsonRequestBehavior.AllowGet);
            }
        }




        public ActionResult GroupBySalaryStatementReport(string DateFrom, int OfficeType)
        {
            try
            {
                var param = new { DateFrom = DateFrom, OfficeType = OfficeType };
                var MainReport = employeeSpService.GetDataWithParameter(param, "prl.SP_rptGroupBySalaryStatement");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);

                ReportHelper.PrintReport("Payroll/rpt_GroupBySalaryStatement.rpt", MainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult OfficeWiseSalaryStatementReport(string DateFrom, string DateTo, string Print)
        {
            try
            {
                var param = new { DateFrom = DateFrom, DateTo = DateTo };
                var MainReport = employeeSpService.GetDataWithParameter(param, "prl.SP_OfficeWiseSalaryStatement");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                if (Print == "Print")
                {
                    ReportHelper.PrintReport("Payroll/rpt_OfficeWiseSalaryStatement_CrossTab.rpt", MainReport.Tables[0], reportParam);
                }
                else if (Print == "Excel Download")
                {
                    ReportHelper.ExportExcelReport("Payroll/rpt_OfficeWiseSalaryStatement_CrossTab.rpt", MainReport.Tables[0], reportParam);
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult OfficeWiseSalarySummaryReport(string DateFrom, string DateTo, string officetype)
        {
            try
            {
                var param = new { DateFrom = DateFrom, DateTo = DateTo, officetype = officetype };
                var MainReport = employeeSpService.GetDataWithParameter(param, "prl.SP_RPT_OfficeWiseSalarySummaryReport");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Payroll/rpt_OfficeWiseSalarySummary.rpt", MainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ComponentPayrollReport(string DateFrom, string DateTo, string ComponentCategory, string ComponentName, int IsApproved, string EmployeeCode)
        {
            try
            {
                int _companyId = CompanyID.Value;
                var param = new { DateFrom = DateFrom, DateTo = DateTo, ComponentCategory = ComponentCategory, ComponentName = ComponentName, IsApproved = IsApproved, EmployeeCode = EmployeeCode };
                var dataSource = employeeSpService.GetDataWithParameter(param, "prl.SP_RPT_ComponentPayroll");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                reportParam.Add("ComponentCategory", ComponentCategory);
                reportParam.Add("ComponentName", ComponentName);

                var dtCompanyInfo = WebHelper.GetCompanyInfo();

                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";
                var reportPartialPath = "Payroll/rpt_ComponentPayroll.rpt";

                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, dataSource.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult StatementOfBankAdviceReport(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { DateFrom = DateFrom, DateTo = DateTo };
                var MainReport = employeeSpService.GetDataWithParameter(param, "prl.SP_StatementOfBankAdvice");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Payroll/rpt_StatementOfBankAdviceReport.rpt", MainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //single report
        //public ActionResult YearlySalaryStatementReport(string DateFrom, string DateTo, string EmployeeCode)
        //{
        //    try
        //    {
        //        var param = new { DateFrom = DateFrom, DateTo = DateTo, EmployeeCode = EmployeeCode};
        //        var MainReport = employeeSpService.GetDataWithParameter(param, "SP_YearlySalaryStatement");
        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("DateFrom", DateFrom);
        //        reportParam.Add("DateTo", DateTo);
        //        ReportHelper.PrintReport("Payroll/rpt_YearlySalaryStatement.rpt", MainReport.Tables[0], reportParam);
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //subreport
        public ActionResult YearlySalaryStatementReport(string DateFrom, string DateTo, string EmployeeCode, string EmployeeNameSig, string DepartmentNameSig, string DesignationNameSig)
        {
            try
            {
                var param = new { DateFrom = DateFrom, DateTo = DateTo, EmployeeCode = EmployeeCode, EmployeeNameSig = EmployeeNameSig, DepartmentNameSig = DepartmentNameSig, DesignationNameSig = DesignationNameSig };
                var sparam = new { EmployeeNameSig = EmployeeNameSig, DepartmentNameSig = DepartmentNameSig, DesignationNameSig = DesignationNameSig };
                var MainReport = employeeSpService.GetDataWithParameter(param, "prl.SP_YearlySalaryStatement");
                var subReport = employeeSpService.GetDataWithParameter(sparam, "prl.SP_ReportPayOrder");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("rpt_ReportPayOrder_Sub", subReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);

                var dtCompanyInfo = WebHelper.GetCompanyInfo();

                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";

                ReportHelper.PrintReportWithSubReportAndMultipleDataSource("Payroll/rpt_YearlySalaryStatement.rpt", MainReport.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam, subReportDb);
                //ReportHelper.PrintWithSubReport("Payroll/rpt_YearlySalaryStatement.rpt", MainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        // All Emplyee Monthly Salary Report

        public ActionResult AllEmployeeSalaryStatement()
        {
            var entity = new PRWorkAreaViewModel();           
            mapDropDownList(entity);
            mapBankDropDown(entity);
            return View(entity);           
        }


        public ActionResult AllEmployeeSalaryStatementReport(int month, int year, string fromDate, string toDate)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                //paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                //paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Year", Value = year });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Month", Value = month });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateFrom", Value = fromDate });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateTo", Value = toDate });
   
                PrintSSRSReport("/gHRMPlus_Reports/GCAllEmployeeMonthlySalary", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        // Employee Payslip Report
        public ActionResult EmployeePayslipStatementReport(string DateFrom, string DateTo, string EmployeeCode, string EmployeeNameSig, string DepartmentNameSig, string DesignationNameSig)
        {
            try
            {             
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();            
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateFrom", Value = (String.IsNullOrEmpty(DateFrom) ? "0" : DateFrom) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateTo", Value = (String.IsNullOrEmpty(DateTo) ? "0" : DateTo) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = (String.IsNullOrEmpty(EmployeeCode) ? "0" : EmployeeCode) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeNameSig", Value = (String.IsNullOrEmpty(EmployeeNameSig) ? "" : EmployeeNameSig) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentNameSig", Value = (String.IsNullOrEmpty(DepartmentNameSig) ? "0" : DepartmentNameSig) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationNameSig", Value = (String.IsNullOrEmpty(DesignationNameSig) ? "0" : DesignationNameSig) });
       
                PrintSSRSReport("/gHRMPlus_Reports/Employee_PaySlip_Report", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }





        #endregion

        #region MapDropDown


        public void MapDropdownForComponentPayrollReport(ComponentPayrollViewModel model)
        {
            var pRComponentCategoryLists = pRComponentService.GetAll().Where(p => p.IsActive == true).DistinctBy(d => d.ComponentCategory);
            var viewpRComponentCategory = pRComponentCategoryLists.Select(a => new SelectListItem()
            {
                Value = a.PRComponentID.ToString(),
                Text = a.ComponentCategory
            });
            var listOfpRComponentCategory = new List<SelectListItem>();
            listOfpRComponentCategory.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            listOfpRComponentCategory.AddRange(viewpRComponentCategory);
            model.ComponentCategoryList = listOfpRComponentCategory;


            var listOfpRComponent = new List<SelectListItem>();
            listOfpRComponent.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            model.ComponentNameList = listOfpRComponent;

            var IsApproved = new List<SelectListItem>();
            IsApproved.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            IsApproved.Add(new SelectListItem() { Text = "Yes", Value = "1" });
            IsApproved.Add(new SelectListItem() { Text = "No", Value = "0" });
            model.IsApprovedList = IsApproved;


            var empCodeList = new List<SelectListItem>();
            empCodeList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            empCodeList.Add(new SelectListItem() { Text = "Employee Code", Value = "1" });
            model.IsEmpCodeList = empCodeList;


            var departlist = employeeDepartmentService.GetAll().Where(p => p.IsActive == true).DistinctBy(d => d.DepartmentId);
            var viewdepartlist = departlist.Select(a => new SelectListItem()
            {
                Value = a.DepartmentId.ToString(),
                Text = a.DepartmentName
            });
            var listviewdepartlist = new List<SelectListItem>();
            listviewdepartlist.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            listviewdepartlist.AddRange(viewdepartlist);
            model.DepartmentNameList = listviewdepartlist;

            var designationlist = employeeDesignationService.GetAll().Where(p => p.IsActive == true).DistinctBy(d => d.DesignationId);
            var viewdesignationlist = designationlist.Select(a => new SelectListItem()
            {
                Value = a.DesignationId.ToString(),
                Text = a.DesignationName
            });
            var listviewdesignationlist = new List<SelectListItem>();
            listviewdesignationlist.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            listviewdesignationlist.AddRange(viewdesignationlist);
            model.DesignationNameList = listviewdesignationlist;


            //var emplist = employeeService.GetAll().Where(p => p.IsActive == true).DistinctBy(d => d.EmployeeId);
            //var viewemplist = emplist.Select(a => new SelectListItem()
            //{
            //    Value = a.EmployeeId.ToString(),
            //    Text = a.EmployeeName
            //});
            //var listofviewemplist = new List<SelectListItem>();
            //listofviewemplist.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            //listofviewemplist.AddRange(viewemplist);
            //model.EmployeeNameList = listofviewemplist;

            var listOfEmployee = new List<SelectListItem>();
            listOfEmployee.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            model.EmployeeNameList = listOfpRComponent;
        }

        public void MapDropdownForGroupBySalaryStatement(ComponentPayrollViewModel model)
        {
            var officeTypeLists = officeTypeService.GetAll().Where(p => p.IsActive == true);
            var viewofficeTypeLists = officeTypeLists.Select(a => new SelectListItem()
            {
                Value = a.OfficeTypeId.ToString(),
                Text = a.OfficeTypeName
            });
            var listofOfficeTypeList = new List<SelectListItem>();
            listofOfficeTypeList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            listofOfficeTypeList.AddRange(viewofficeTypeLists);
            model.OfficeTypeList = listofOfficeTypeList;
        }

        public void MapDropdownForOfficeWiseSalarySummaryReport(ComponentPayrollViewModel model)
        {
            var officeTypeLists = officeTypeService.GetAll().Where(p => p.IsActive == true);
            var viewofficeTypeLists = officeTypeLists.Select(a => new SelectListItem()
            {
                Value = a.OfficeTypeId.ToString(),
                Text = a.OfficeTypeName
            });
            var listofOfficeTypeList = new List<SelectListItem>();
            listofOfficeTypeList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            listofOfficeTypeList.AddRange(viewofficeTypeLists);
            model.OfficeTypeList = listofOfficeTypeList;
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
        }// End of Month

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
        }// End of Years


        private void mapBankDropDown(PRWorkAreaViewModel model)
        {
            var pleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };

            var bankList = bankNameService.GetMany(x => x.IsActive == true);
            var viewList = bankList.AsEnumerable().Select(row => new SelectListItem
            {
                Text = row.BankFullName,
                Value = row.BankCode
            }).ToList();

            var viewBankList = new List<SelectListItem>();
            viewBankList.Add(pleaseSelect);
            viewBankList.AddRange(viewList);

            model.BankList = viewBankList;

            var officeTypeList = new List<SelectListItem>();
          //  officeTypeList.Add(PleaseSelect);
            officeTypeList.Add(new SelectListItem() { Text = "Head Office", Value = "1" });
            officeTypeList.Add(new SelectListItem() { Text = "Field Office", Value = "2" });
            //model.OfficeTypeList = officeTypeList; 




            //var officeType = officeTypeService.GetMany(w => w.IsActive == true); ;
            //var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.OfficeTypeId.ToString(),
            //    Text = string.Format("{0}", x.OfficeTypeName)
            //});
            //var officeType_items = new List<SelectListItem>();
            //officeType_items.Add(new SelectListItem() { Text = "All", Value = "10000", Selected = true });
            //officeType_items.AddRange(viewofficeType);
            //model.OfficeTypeList = officeType_items;



        }

        private void mapBankDropDown2(PRWorkAreaViewModel model)
        {
            //var pleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };

            var bankList = bankNameService.GetMany(x => x.IsActive == true);
            var viewList = bankList.AsEnumerable().Select(row => new SelectListItem
            {
                Text = row.BankFullName,
                Value = row.BankCode
            }).ToList();

            var viewBankList = new List<SelectListItem>();
            //viewBankList.Add(pleaseSelect);
            viewBankList.AddRange(viewList);

            model.BankList = viewBankList;

            var officeTypeList = new List<SelectListItem>(); //jahan need to clear
            //  officeTypeList.Add(PleaseSelect);
            officeTypeList.Add(new SelectListItem() { Text = "Head Office", Value = "1" });
            officeTypeList.Add(new SelectListItem() { Text = "Field Office", Value = "2" });
            model.OfficeTypeList = officeTypeList;




            //var officeType = officeTypeService.GetMany(w => w.IsActive == true); ;
            //var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.OfficeTypeId.ToString(),
            //    Text = string.Format("{0}", x.OfficeTypeName)
            //});
            //var officeType_items = new List<SelectListItem>();
            //officeType_items.Add(new SelectListItem() { Text = "All", Value = "10000", Selected = true });
            //officeType_items.AddRange(viewofficeType);
            //model.OfficeTypeList = officeType_items;



        }

        private void mapBankDropDown3(PRWorkAreaViewModel model)
        {
            //var pleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };

            var bankList = bankNameService.GetMany(x => x.IsActive == true);
            var viewList = bankList.AsEnumerable().Select(row => new SelectListItem
            {
                Text = row.BankFullName,
                Value = row.BankCode
            }).ToList();

            var viewBankList = new List<SelectListItem>();
            //viewBankList.Add(pleaseSelect);
            viewBankList.AddRange(viewList);

            model.BankList = viewBankList;

            var officeTypeList = new List<SelectListItem>();
            //  officeTypeList.Add(PleaseSelect);
            //officeTypeList.Add(new SelectListItem() { Text = "Head Office", Value = "1" });
            //officeTypeList.Add(new SelectListItem() { Text = "Field Office", Value = "2" });
            //model.OfficeTypeList = officeTypeList;




            var officeType = officeTypeService.GetMany(w => w.IsActive == true); ;
            var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeTypeId.ToString(),
                Text = string.Format("{0}", x.OfficeTypeName)
            });
            var officeType_items = new List<SelectListItem>();
            officeType_items.Add(new SelectListItem() { Text = "All", Value = "10000", Selected = true });
            officeType_items.AddRange(viewofficeType);
            model.OfficeTypeList = officeType_items;



        }

        private void mapDropDownList(PRWorkAreaViewModel entity)
        {
            var PleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var yearList = new List<SelectListItem>();
            yearList.Add(PleaseSelect);
            for (int i = DateTime.Now.Year; i >= (DateTime.Now.Year) - 5; i--)
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
            applicationList.Add(new SelectListItem() { Text = "Fund Transfer Application & Advice", Value = "ApplicationAdvice" });
            applicationList.Add(new SelectListItem() { Text = "Component Wise Salary", Value = "Component" });
            entity.ReportTypeList = applicationList;

            var lists = new List<SelectListItem>();
            lists.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            lists.Add(new SelectListItem() { Text = "Salary Befor Approval (Pdf Format)", Value = "1" });
            lists.Add(new SelectListItem() { Text = "Salary Befor Approval (Excel Format)", Value = "2" });
            lists.Add(new SelectListItem() { Text = "Rejected Employees Salary (Pdf Format)", Value = "3" });
            lists.Add(new SelectListItem() { Text = "Approved Salary (Pdf Format)", Value = "4" });
            lists.Add(new SelectListItem() { Text = "Approved Salary (Excel Format)", Value = "5" });
            lists.Add(new SelectListItem() { Text = "Approved Salary Group by Office(Pdf Format)", Value = "6" });
            lists.Add(new SelectListItem() { Text = "Approved Salary Group by Office (Excel Format)", Value = "7" });
            lists.Add(new SelectListItem() { Text = "Approved Salary Group by Zone Area", Value = "8" });

            entity.ReportList = lists;
        }

        private void mapDropDownList2(PRWorkAreaViewModel entity)
        {
            var PleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var yearList = new List<SelectListItem>();
            yearList.Add(PleaseSelect);
            for (int i = DateTime.Now.Year; i >= (DateTime.Now.Year) - 5; i--)
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
            //salaryTypeList.Add(PleaseSelect);
            salaryTypeList.Add(new SelectListItem() { Text = "Salary", Value = "Salary" });
            //salaryTypeList.Add(new SelectListItem() { Text = "Bonus for Eid-ul-Fitre", Value = "Bonus for Eid-ul-Fitre" });
            //salaryTypeList.Add(new SelectListItem() { Text = "Bonus for Eid-ul-Azha", Value = "Bonus for Eid-ul-Azha" });
            //salaryTypeList.Add(new SelectListItem() { Text = "Incentive", Value = "Incentive" });
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
            //applicationList.Add(PleaseSelect);
            //applicationList.Add(new SelectListItem() { Text = "Fund Transfer Application", Value = "Application" });
            //applicationList.Add(new SelectListItem() { Text = "Fund Transfer Advice", Value = "Advice" });
            applicationList.Add(new SelectListItem() { Text = "Fund Transfer Application & Advice", Value = "ApplicationAdvice" });
            //applicationList.Add(new SelectListItem() { Text = "Component Wise Salary", Value = "Component" });
            entity.ReportTypeList = applicationList;

            var lists = new List<SelectListItem>();
            lists.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            lists.Add(new SelectListItem() { Text = "Salary Befor Approval (Pdf Format)", Value = "1" });
            lists.Add(new SelectListItem() { Text = "Salary Befor Approval (Excel Format)", Value = "2" });
            lists.Add(new SelectListItem() { Text = "Rejected Employees Salary (Pdf Format)", Value = "3" });
            lists.Add(new SelectListItem() { Text = "Approved Salary (Pdf Format)", Value = "4" });
            lists.Add(new SelectListItem() { Text = "Approved Salary (Excel Format)", Value = "5" });
            lists.Add(new SelectListItem() { Text = "Approved Salary Group by Office(Pdf Format)", Value = "6" });
            lists.Add(new SelectListItem() { Text = "Approved Salary Group by Office (Excel Format)", Value = "7" });
            lists.Add(new SelectListItem() { Text = "Approved Salary Group by Zone Area", Value = "8" });

            entity.ReportList = lists;
        }
        #endregion

        private void MapDropdownForOfficeTypeList(PRWorkAreaViewModel model)
        {
            var officeTypeList = officeTypeService.GetAll().Where(p => p.IsActive == true);
            var viewOfficeTypeList = officeTypeList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.OfficeTypeName,
                Value = p.OfficeTypeId.ToString()
            });
            var officeType = new List<SelectListItem>();
            officeType.Add(new SelectListItem { Text = "Please Select", Value = "" });
            officeType.AddRange(viewOfficeTypeList);
            model.OfficeTypeList = officeType;
        }
        private void MapDropdownForOfficeList(PRWorkAreaViewModel model)
        {
            var officeList = officeService.GetAll().Where(p => p.IsActive == true);
            var viewOfficeList = officeList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.OfficeName,
                Value = p.OfficeId.ToString()
            });
            var office = new List<SelectListItem>();
            office.Add(new SelectListItem { Text = "Please Select", Value = "" });
            office.AddRange(viewOfficeList);
            model.OfficeList = office;
        }

        public JsonResult GetOfficeByOfficeType(int officeTypeId) // 
        {
            var officeList = officeService.GetAll().Where(p => p.IsActive == true)
                .Where(x => x.OfficeTypeId == officeTypeId)
                .Select(x => new SelectListItem
                {
                    Value = x.OfficeId.ToString(),
                    Text = x.OfficeName
                }).ToList();

            return Json(officeList, JsonRequestBehavior.AllowGet);
        }
    }
}