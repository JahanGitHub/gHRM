
#region Usings

using gHRM.Service.PF;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels.PF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using gHRM.Web.Helpers;
using gHRM.Core.Utilities.Constants;

#endregion

namespace gHRM.Web.Controllers
{
    public class PFPayrollInstallmentCollectionController : BaseController
    {
        #region Private Methods

        private readonly IProcessLogService processLogService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IPRInstallmentProcessLogService instProcessLogService;
        private readonly ICollectionService collectionService;
        private readonly int contributionTransCatId = 1;    //Contribution from Payroll	Cr
        private readonly int loanTransCatId = 6;            //Loan Installment from Payroll	Cr

        #endregion

        #region Ctor
        public PFPayrollInstallmentCollectionController(IProcessLogService processLogService, IEmployeeSPService employeeSPService, IPRInstallmentProcessLogService instProcessLogService, ICollectionService collectionService)
        {
            this.processLogService = processLogService;
            this.employeeSPService = employeeSPService;
            this.instProcessLogService = instProcessLogService;
            this.collectionService = collectionService;
        }
        #endregion

        #region Listings

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetContributionCollectionList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = "", string filterColumn = "", string filterValue = "", string employeeCode = "", string employeeName = "", string collectionTypeId = "")
        {
            string message = "Sorry for inconvenience! please try again later";
                       
            if (string.IsNullOrEmpty(collectionTypeId))
                return Json(new { Result = "ERROR", Message = message }, JsonRequestBehavior.AllowGet);

            try
            {
                //get listing from [gcpf.Collection]
                var dataset = collectionService.GetCollections(employeeCode, employeeName, Convert.ToInt32(collectionTypeId));

                var List_ViewModel = dataset.Tables[0].AsEnumerable()
                .Select(row => new ContributionCollectionViewModel
                {
                    CollectionId = row.Field<Int64>("CollectionId").ToString(),
                    EmployeeId = row.Field<Int64>("EmployeeId").ToString(),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    LoanId = row.Field<Int64>("LoanId"),
                    LoanTerm = row.Field<int>("LoanTerm"),
                    CollectionType = row.Field<string>("CollectionType"),
                    TransactionType = row.Field<string>("TransactionType"),
                    TransactionDateString = Convert.ToDateTime(row.Field<DateTime>("TransactionDate")).ToString("dd-MMM-yyyy"),
                    SelfContribution = row.Field<decimal>("SelfContribution") == 0 ? "0" : Math.Round(row.Field<decimal>("SelfContribution"), 2).ToString(),
                    OrgContribution = row.Field<decimal>("OrgContribution") == 0 ? "0" : Math.Round(row.Field<decimal>("OrgContribution"), 2).ToString(),
                    LoanAmount = row.Field<decimal>("LoanAmount") == 0 ? "0" : Math.Round(row.Field<decimal>("LoanAmount"), 2).ToString(),
                    InterestAmount = row.Field<decimal>("InterestAmount") == 0 ? "0" : Math.Round(row.Field<decimal>("InterestAmount"), 2).ToString(),
                    InterestCharge = row.Field<decimal>("InterestCharge") == 0 ? "0" : Math.Round(row.Field<decimal>("InterestCharge"), 2).ToString(),
                    Sundry = row.Field<decimal>("Sundry") == 0 ? "0" : Math.Round(row.Field<decimal>("Sundry"), 2).ToString()
                }).ToList();

                var currentPageRecords = List_ViewModel.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return this.Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Create

        public ActionResult Create()
        {
            PayrollInstallmentCollectionViewModel model = new PayrollInstallmentCollectionViewModel();

            try
            {
                var objProcessLog = processLogService.GetDayStatus();
                if (objProcessLog != null)
                {
                    model.TransactionDate = objProcessLog.TransactionDateString;
                    model.IsOpen = objProcessLog.IsOpen;
                    model.DayStatus = objProcessLog.DayStatus;
                }

                var insProcessLog = instProcessLogService.GetAll().Where(x => x.IsDeleted == false && x.IsProcessed == true).OrderByDescending(x => x.Year).ThenByDescending(x => x.Month).Take(1).FirstOrDefault();
                if (insProcessLog != null)
                {
                    model.Month = insProcessLog.Month >= 1 && insProcessLog.Month <= 12 ? CultureInfo.InvariantCulture.DateTimeFormat.AbbreviatedMonthNames[insProcessLog.Month - 1] : string.Empty;
                    model.Year = insProcessLog.Year.ToString();
                }
                else
                {
                    model.InstProcStatus = "It is first time for processing installment";
                }
            }
            catch (Exception ex)
            {

            }

            MapDropDownList(model);
            return View(model);
        }

        public JsonResult SavePayrollInstallment(string monthId, string year)
        {
            var model = new PayrollInstallmentCollectionViewModel();
            try
            {
                bool isProcessed = false;
                decimal selfContribution = 0;
                int selfContributor = 0;
                decimal orgContribution = 0;
                int orgContributor = 0;
                decimal loanAmount = 0;
                int loanee = 0;

                if (!processLogService.IsDayOpen())
                    return Json(new { message = "Day Clossed" }, JsonRequestBehavior.AllowGet);

                int mId = Convert.ToInt32(monthId);
                int yId = Convert.ToInt32(year);

                //from [gcpf.PRInstallmentProcessLog]
                var instProcessLog = instProcessLogService.GetMany(x => x.IsDeleted == false).OrderByDescending(x => x.ProcessId).Take(1).FirstOrDefault();
                if (instProcessLog != null)
                {
                    //year validation
                    //month validation: Within Year
                    if ((instProcessLog.Month != 12))
                    {
                        if ((instProcessLog.Year == yId) && (instProcessLog.Month >= mId) && (instProcessLog.IsProcessed == true))
                            return Json(new { message = "Already Collected" }, JsonRequestBehavior.AllowGet);

                        if ((instProcessLog.Year == yId) && (instProcessLog.Month != mId - 1) && (instProcessLog.IsProcessed == true))
                            return Json(new { message = "Select correct month, you have skipped" }, JsonRequestBehavior.AllowGet);

                        if ((instProcessLog.Year != yId) && (instProcessLog.IsProcessed == true))
                            return Json(new { message = "Select correct year" }, JsonRequestBehavior.AllowGet);

                    }
                    //month validation: Year Ahead
                    if ((instProcessLog.Month == 12))
                    {
                        if ((instProcessLog.Year >= yId) && (instProcessLog.IsProcessed == true))
                            return Json(new { message = "Already Collected" }, JsonRequestBehavior.AllowGet);

                        if ((instProcessLog.Year != yId - 1) && (instProcessLog.IsProcessed == true))
                            return Json(new { message = "Please select year, you have skipped" }, JsonRequestBehavior.AllowGet);

                        if ((instProcessLog.Year == yId - 1) && (mId != 1) && (instProcessLog.IsProcessed == true))
                            return Json(new { message = "Please select year, you have skipped" }, JsonRequestBehavior.AllowGet);
                    }
                }

                //Note: get payroll installment status for this month and year from [PRL.EmployeeMonthlySalaryApproved]
                //get installment status like isProcessed, selfContribution, selfContributor, orgContribution, orgContributor, 
                //loanAmount, loanee
                GetInstallmentStatus(Convert.ToInt32(monthId), Convert.ToInt32(year), out isProcessed, out selfContribution, 
                    out selfContributor, out orgContribution, out orgContributor, out loanAmount, out loanee);

                if (isProcessed == false)
                    return Json(new { message = "Salary has not been disbursed yet" }, JsonRequestBehavior.AllowGet);
                
                model.MonthId = Convert.ToInt32(monthId);
                model.Year = year;
                model.ContributionCollTypeId = contributionTransCatId;
                model.ContributionTransType = TransactionTypeConstants.Credit;

                model.LoanCollTypeId = loanTransCatId;
                model.LoanTransType = TransactionTypeConstants.Credit;
                model.CreateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                model.CreateDate = DateTime.Now;

                //let's process payroll installment collection {insert into [gcpf.PRInstallmentProcessLog]}
                SaveInstallment(model);
            }
            catch (Exception ex)
            {
                return Json(new { message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Others Http Requests

        public JsonResult GetPayrollInstallmentLogList(string ProcessId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                //get payroll installment collection log from [gcpf.PRInstallmentProcessLog]
                var objInstProcessLogList = instProcessLogService.GetAll().Where(x => x.IsDeleted == false).OrderByDescending(x => x.Year).ThenByDescending(x => x.Month);

                var List_ViewModel = objInstProcessLogList.AsEnumerable()
               .Select(row => new PayrollInstallmentCollectionViewModel
               {
                   ProcessId = row.ProcessId.ToString(),
                   Year = row.Year.ToString(),
                   Month = row.Month >= 1 && row.Month <= 12 ? CultureInfo.InvariantCulture.DateTimeFormat.AbbreviatedMonthNames[row.Month - 1] : string.Empty,
                   IsProcessed = row.IsProcessed,
                   CDate = row.CreateDate.HasValue ? row.CreateDate.Value.ToString("dd-MMM-yyyy",CultureInfo.InvariantCulture) : string.Empty

               }).ToList();

                var currentPageRecords = List_ViewModel.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult VerifyPayrollInstallment(string monthId, string year)
        {
            bool isProcessed = false;
            decimal selfContribution = 0;
            int selfContributor = 0;
            decimal orgContribution = 0;
            int orgContributor = 0;
            decimal loanAmount = 0;
            int loanee = 0;
            try
            {
                GetInstallmentStatus(Convert.ToInt32(monthId), Convert.ToInt32(year), out isProcessed, out selfContribution, out selfContributor, out orgContribution, out orgContributor, out loanAmount, out loanee);
            }
            catch (Exception ex)
            {
                return Json(new { isProcessed = isProcessed, selfContribution = selfContribution, selfContributor = selfContributor, orgContribution = orgContribution, orgContributor = orgContributor, loanAmount = loanAmount, loanee = loanee, message = "Sorry for inconvenience! please try again later" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { isProcessed = isProcessed, selfContribution = selfContribution, selfContributor = selfContributor, orgContribution = orgContribution, orgContributor = orgContributor, loanAmount = loanAmount, loanee = loanee, message = "" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Report

        [HttpGet]
        public ActionResult PrintReport(string employeeCode = "", string employeeName = "", string collectionTypeId = "")
        {
            try
            {
                var dataset = collectionService.GetCollections(employeeCode, employeeName, Convert.ToInt32(collectionTypeId));

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", SessionHelper.CompanyName);
                reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);

                if (collectionTypeId == "1")
                    ReportHelper.PrintReport("PF_RPT_PayrollContributionInstallment.rpt", dataset.Tables[0], reportParam);
                if (collectionTypeId == "6")
                    ReportHelper.PrintReport("PF_RPT_PayrollLoanInstallment.rpt", dataset.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion
        
        #region Private Methods

        public void MapDropDownList(PayrollInstallmentCollectionViewModel model)
        {
            model.MonthList = GetMonthList();
            model.YearList = GetYearList();
        }
        public IEnumerable<SelectListItem> GetMonthList()
        {
            var months = Enumerable.Range(1, 12).Select(x =>
                 new SelectListItem()
                 {
                     Text = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[x - 1],// + " (" + x + ")",
                     Value = x.ToString()
                 });

            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem() { Text = "Select Month", Value = "", Selected = true });
            monthList.AddRange(months);
            return monthList;
        }
        public IEnumerable<SelectListItem> GetYearList()
        {
            var years = Enumerable.Range(DateTime.Today.Year - 5, 15).Select(x =>
                new SelectListItem()
                {
                    Text = x.ToString(),
                    Value = x.ToString()            
                });

            var yearList = new List<SelectListItem>();
            yearList.Add(new SelectListItem() { Text = "Select Year", Value = "", Selected = true });
            yearList.AddRange(years);
            return yearList;
        }

        private void SaveInstallment(PayrollInstallmentCollectionViewModel model)
        {           
            var processLog = processLogService.GetLastProcessLog();
        
            var param = new
            {
                MonthId = model.MonthId,
                Year = Convert.ToInt32(model.Year),
                ContributionCollTypeId = model.ContributionCollTypeId,
                ContributionTransType = model.ContributionTransType,
                TransactionDate = processLog.StartDate,
                LoanCollTypeId = model.LoanCollTypeId,
                LoanTransType = model.LoanTransType,
                CreateUser = model.CreateUser,
                CreateDate = model.CreateDate
            };

            //let's process payroll installment collection {insert into gcpf.[Collection] and [gcpf.PRInstallmentProcessLog]}
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_MS_ProcessPayrollInstCollection");
        }

        private void GetInstallmentStatus(int monthId, int year, out bool isProcessed, out decimal selfContribution, out int selfContributor, out decimal orgContribution, out int orgContributor, out decimal loanAmount, out int loanee)
        {            
            var processLog = processLogService.GetLastProcessLog();
   
            var param = new
            {
                MonthId = monthId,
                Year = year,
                TransactionDate = processLog.StartDate
            };

            //get payroll installment status for this month and year from [PRL.EmployeeMonthlySalaryApproved]
            var val = employeeSPService.GetDataWithParameter(param, "gcpf.SP_GetPR_InstallmentStatus");

            isProcessed = val.Tables[0].AsEnumerable().Select(row => row.Field<bool>("IsProcessed")).FirstOrDefault();
            selfContribution = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("selfContribution")).FirstOrDefault();
            selfContributor = val.Tables[0].AsEnumerable().Select(row => row.Field<int>("SelfContributor")).FirstOrDefault();

            orgContribution = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("OrgContribution")).FirstOrDefault();
            orgContributor = val.Tables[0].AsEnumerable().Select(row => row.Field<int>("OrgContributor")).FirstOrDefault();

            loanAmount = val.Tables[0].AsEnumerable().Select(row => row.Field<decimal>("LoanAmount")).FirstOrDefault();
            loanee = val.Tables[0].AsEnumerable().Select(row => row.Field<int>("Loanee")).FirstOrDefault();            
        }

        #endregion
    }
}
