using gHRM.Service;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using gHRM.Web.ViewModels.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers.Loan
{
    public class LoanReport2Controller : BaseController
    {
        private readonly IAspNetRoleService aspNetRoleService;
        private readonly IOfficeService officeService;
        private readonly IOfficeTypeService officeTypeService;
        private CommonDynamicDropDown commonDynamicDropDown;
        public LoanReport2Controller(IAspNetRoleService aspNetRoleService, IOfficeTypeService officeTypeService, IOfficeService officeService)
        {
           this.aspNetRoleService = aspNetRoleService;
           this.officeService = officeService;
           this.officeTypeService = officeTypeService;
            commonDynamicDropDown = new CommonDynamicDropDown();
        }
        // GET: LoanReport
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");

           
            var model = new OfficeNevigationPartialViewModel();
            
            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();
            return View(model);
        }

        public ActionResult Index_Verc()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");


            var model = new OfficeNevigationPartialViewModel();

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();
            return View(model);
        }

        public ActionResult Opening()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");


            var model = new OfficeNevigationPartialViewModel();

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();
            return View(model);
        }


        public ActionResult Index2()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");


            var model = new OfficeNevigationPartialViewModel();

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();
            return View(model);
        }

        public ActionResult CommonReports(string reportName,string fromDate,string toDate,string uptoDate, int? officeid,int? loanid)
        {
            if(string.IsNullOrEmpty(reportName))
            return Content("Report Type is required.");
            try
            {
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                if(reportName== "topsheet")
                {
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officeId", Value = (officeid??0) });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "fromDate", Value = fromDate });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "toDate", Value = toDate });
                    reportName = "LoanTopSheetReport";
                }
                else if (reportName == "coll")
                {
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officeId", Value = (officeid ?? 0) });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "fromDate", Value = fromDate });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "toDate", Value = toDate });
                    reportName = "LoanCollectionReport";
                }
                else if (reportName == "mon_ins")
                {
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officeId", Value = (officeid ?? 0) });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "uptoDate", Value = uptoDate });
                    reportName = "MonthlyInstallmentReport";
                }
                else if (reportName == "ledger")
                {
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "loanId", Value = (loanid ?? 0) });
                    reportName = "LoanLedgerReport";
                }
                PrintSSRSMultiformat("PDF", $"/gHRMPlus_Reports/{reportName}", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        public ActionResult CommonReports_verc(string reportName, string fromDate, string toDate, string uptoDate, int? officeid, int? loanid)
        {
            if (string.IsNullOrEmpty(reportName))
                return Content("Report Type is required.");
            try
            {
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                if (reportName == "topsheet")
                {
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officeId", Value = (officeid ?? 0) });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "fromDate", Value = fromDate });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "toDate", Value = toDate });
                    reportName = "LoanTopSheetReport";
                }
                else if (reportName == "coll")
                {
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officeId", Value = (officeid ?? 0) });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "fromDate", Value = fromDate });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "toDate", Value = toDate });
                    reportName = "LoanCollectionReport";
                }
                else if (reportName == "mon_ins")
                {
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officeId", Value = (officeid ?? 0) });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "uptoDate", Value = uptoDate });
                    reportName = "MonthlyInstallmentReport";
                }
                else if (reportName == "ledger")
                {
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "loanId", Value = (loanid ?? 0) });
                    reportName = "LoanLedgerReport";
                }
                PrintSSRSMultiformat("PDF", $"/gHRMPlus_Reports/{reportName}", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult CommonReports4(string reportName, string fromDate, string toDate, string uptoDate, int? officeid, int? loanid)
        {
            if (string.IsNullOrEmpty(reportName))
                return Content("Report Type is required.");
            try
            {
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                if (reportName == "topsheet")
                {
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officeId", Value = (officeid ?? 0) });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "fromDate", Value = fromDate });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "toDate", Value = toDate });
                    reportName = "LoanTopSheetReport";
                }
                else if (reportName == "coll")
                {
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officeId", Value = (officeid ?? 0) });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "fromDate", Value = fromDate });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "toDate", Value = toDate });
                    reportName = "LoanCollectionReport";
                }
                else if (reportName == "mon_ins")
                {
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officeId", Value = (officeid ?? 0) });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "uptoDate", Value = uptoDate });
                    reportName = "MonthlyInstallmentReport";
                }
                else if (reportName == "ledger")
                {
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "loanId", Value = (loanid ?? 0) });
                    reportName = "LoanLedgerReport";
                }
                PrintSSRSMultiformat("PDF", $"/gHRMPlus_Reports/{reportName}", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }



        public ActionResult CommonReports2(string reportName, string fromDate, string toDate, string uptoDate, int? officeid, int? loanid)
        {
            if (string.IsNullOrEmpty(reportName))
                return Content("Report Type is required.");
            try
            {
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                if (reportName == "balance")
                {
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officeId", Value = (officeid ?? 0) });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "fromDate", Value = fromDate });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "toDate", Value = toDate });
                    reportName = "LoanBalanceSheetReport";
                }
                else if (reportName == "coll")
                {
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officeId", Value = (officeid ?? 0) });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "fromDate", Value = fromDate });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "toDate", Value = toDate });
                    reportName = "LoanCollectionReport";
                }
                else if (reportName == "mon_ins")
                {
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officeId", Value = (officeid ?? 0) });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "uptoDate", Value = uptoDate });
                    reportName = "MonthlyInstallmentReport";
                }
                else if (reportName == "ledger2")
                {
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "loanId", Value = (loanid ?? 0) });
                    reportName = "LoanLedgerReport2";
                }
                PrintSSRSMultiformat("PDF", $"/gHRMPlus_Reports/{reportName}", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

    }
}