using gHRM.Core.Filters.PerformanceEvaluations;
using gHRM.Core.Utilities.Constants;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.Infrastructure.Date;
using gHRM.Web.Infrastucture.Utility;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers
{
    public class PerformanceEvaluationReportController : BaseController
    {
        private readonly IEmployeeSPService employeeSPService;
        public CommonDynamicDropDown commonDynamicDropDown;
        public IEmployeeService employeeService;
        private readonly IOfficeService officeService;
        public PerformanceEvaluationReportController(IEmployeeSPService employeeSPService, IEmployeeService employeeService, IOfficeService officeService)
        {
            this.employeeSPService = employeeSPService;
            commonDynamicDropDown = new CommonDynamicDropDown();
            this.employeeService = employeeService;
            this.officeService = officeService;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PerformanceEvaluationHistory()
        {
            var model = new PerformanceEvaluationViewModel();
            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();

            var filter = new PerformanceEvaluationSearchFilter
            {

            };
            model.SearchFilter = filter;
            model.Years = DateHelper.GetYears(3, 15);
            model.Months = DateHelper.GetMonths();

            return View(model);
        }

        public ActionResult PerformanceEvaluationHistoryReportPrint(string dateFrom, string dateTo, int officeId = 0, long employeeCode = 0, bool isLedger = false, int officeTypeId = 0)
        {
            try
            {
                string format = "pdf";
                string type = "view";
                long employeeId = 0;
                DataSet mainDataSource;

                string[] fromDate = dateFrom.Split('/');
                var fromMonthName = fromDate[0];
                int fromDateMonth = DateTime.ParseExact(fromMonthName, "MMMM", CultureInfo.InvariantCulture).Month;
                var fromDateYear = fromDate[1];

                string[] toDate = dateTo.Split('/');
                var toMonthName = toDate[0];
                int toDateMonth = DateTime.ParseExact(toMonthName, "MMMM", CultureInfo.InvariantCulture).Month;

                var toDateYear = toDate[1];

                var employee = employeeService.GetMany(p => p.EmployeeCode == employeeCode.ToString()).FirstOrDefault();
                if (employee != null)
                {
                    employeeId = employee.EmployeeId;
                }

                var officeName = "";

                if (officeId > 0)
                {
                    officeName = officeService.GetById(officeId).OfficeName;
                }
                else
                    officeName = "All Office";


                var companyInfo = WebHelper.GetCompanyDetails();

                var parameters = new Dictionary<string, object>();
                parameters.Add("SelectedOffice", officeName);
                parameters.Add("DateFrom", dateFrom);
                parameters.Add("DateTo", dateTo);
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("CompanyName", companyInfo.CompanyName);
                parameters.Add("CompanyAddress", companyInfo.CompanyAddress);
                parameters.Add("CompanyLogo", companyInfo.CompanyLogoURI);

                string reportTitle = "";
                string reportPath = "";
                string reportViewMode = ReportViewModeConstants.Landscape;

                var reportDataSourceName = "";

                if (isLedger == false)
                {
                    var param1 = new { @OfficeTypeId = officeTypeId, @OfficeId = officeId, @FromDateMonth = fromDateMonth, @FromDateYear = fromDateYear, @ToDateMonth = toDateMonth, @ToDateYear = toDateYear, @EmployeeId = employeeId };
                    mainDataSource = employeeSPService.GetDataWithParameter(param1, "PerformanceEvaluationHistoryReport");
                    reportDataSourceName = "PerformanceEvaluationHistoryDataSet";
                    reportTitle = "Performance Evaluation History Report";
                    reportPath = "~/Reports/RDLC/Employee/PerformanceEvaluationHistoryReport.rdlc";
                    reportViewMode = ReportViewModeConstants.Landscape;
                    return Report(mainDataSource.Tables[0], reportDataSourceName, parameters, reportTitle, reportPath, format = "pdf", type = "view", reportViewMode);
                }
                var param2 = new { @OfficeTypeId = officeTypeId, @OfficeId = officeId, @FromDateMonth = fromDateMonth, @FromDateYear = fromDateYear, @ToDateMonth = toDateMonth, @ToDateYear = toDateYear, @EmployeeId = employeeId };
                mainDataSource = employeeSPService.GetDataWithParameter(param2, "MonthWiseStaffPerformanceReport");

                reportDataSourceName = "MonthwisePerformanceDataSet";
                reportTitle = "Month Wise Performance Report";
                reportPath = "~/Reports/RDLC/Employee/MonthWiseStaffPerformanceReport.rdlc";
                reportViewMode = ReportViewModeConstants.Landscape;
                return Report(mainDataSource.Tables[0], reportDataSourceName, parameters, reportTitle, reportPath, format = "pdf", type = "view", reportViewMode);

            }
            catch (Exception ex)
            {
                return RedirectToAction("CommonReportGenerationError");
            }
        }
    }
}