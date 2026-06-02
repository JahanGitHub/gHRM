using System.Data;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using gHRM.Web.ViewModels;
using gHRM.Core.Filters.Employee;
using gHRM.Core.Utilities;
using gHRM.Web.Infrastucture.Utility;
using Microsoft.Reporting.WebForms;
using System.Threading.Tasks;
using System.IO;
using ZXing;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Ajax.Utilities;
using System.Globalization;

namespace gHRM.Web.Controllers.Promotion
{

    public class PromotionTransferReportController : BaseController
    {

        #region Private Methods

        private readonly IEmployeeDocumentService employeeDocumentService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSpService;
        private readonly IOfficeService officeService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IEmployeeStatusService employeeStatusService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IEmployeeReportOptionService employeeReportOptionService;
        private readonly IEmployeeDesignationService employeeDesignationService;
        private readonly IEmployementTypeService employementTypeService;
        private readonly IEmployeeTrainingService employeeTrainingService;
        private readonly IEmployeeTranningDropDownService employeeTranningDropDownService;
        #endregion

        #region Ctor

        public PromotionTransferReportController(
           IEmployeeService employeeService,
           IEmployeeSPService employeeSpService,
           IOfficeService officeService,
           IOfficeTypeService officeTypeService,
           IEmployeeStatusService employeeStatusService,
           IEmployeeDepartmentService employeeDepartmentService,
           IEmployeeReportOptionService employeeReportOptionService,
           IEmployeeDesignationService employeeDesignationService,
           IEmployementTypeService employementTypeService,
           IEmployeeTrainingService employeeTrainingService,
           IEmployeeDocumentService employeeDocumentService,
           IEmployeeTranningDropDownService employeeTranningDropDownService

           )
        {
            this.employeeDocumentService = employeeDocumentService;
            this.employeeService = employeeService;
            this.employeeSpService = employeeSpService;
            this.officeService = officeService;
            this.officeTypeService = officeTypeService;
            this.employeeStatusService = employeeStatusService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.employeeReportOptionService = employeeReportOptionService;
            this.employeeDesignationService = employeeDesignationService;
            this.employementTypeService = employementTypeService;
            this.employeeTrainingService = employeeTrainingService;
            this.employeeTranningDropDownService = employeeTranningDropDownService;
        }
        #endregion


        public ActionResult EmployeeTransferHistory(string Code)
        {
            var model = new EmployeeOtherInformationViewModel();
            model.EmployeeCode = Code;


            return View(model);
        }

        public ActionResult IndividualEmployeePostingHistory(string EmployeeCode)
        {
            try
            {

                var param = new { EmployeeCode = EmployeeCode };

                var mainReport = employeeSpService.GetDataWithParameter(param, "[Emp].Nishan1EmployeePostingHistory");

                var dtCompanyInfo = WebHelper.GetCompanyInfo();

                var reportParam = new Dictionary<string, object>();
               
                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";

                var reportPartialPath = "Employee/IndividualEmployeePostHist.rpt";

                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, mainReport.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);
 
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult PossibleIncrementable()
        {
            ViewData["Months"] = Months();
            ViewData["Years"] = Years();

            var model = new EmployeeOtherInformationViewModel();
            return View();
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
                Value = "January"
            });
            items3.Add(new SelectListItem
            {
                Text = "February",
                Value = "February"
            });
            items3.Add(new SelectListItem
            {
                Text = "March",
                Value = "March"
            });
            items3.Add(new SelectListItem
            {
                Text = "April",
                Value = "April"
            });
            items3.Add(new SelectListItem
            {
                Text = "May",
                Value = "May"
            });
            items3.Add(new SelectListItem
            {
                Text = "June",
                Value = "June"
            });
            items3.Add(new SelectListItem
            {
                Text = "July",
                Value = "July"
            });
            items3.Add(new SelectListItem
            {
                Text = "August",
                Value = "August"
            });
            items3.Add(new SelectListItem
            {
                Text = "September",
                Value = "September"
            });
            items3.Add(new SelectListItem
            {
                Text = "October",
                Value = "October"
            });
            items3.Add(new SelectListItem
            {
                Text = "November",
                Value = "November"
            });
            items3.Add(new SelectListItem
            {
                Text = "December",
                Value = "December"
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

            int year = DateTime.Now.Year; //Current Year.
            int lowYear = year - 5;


            for (; year >= lowYear; year--)
            {
                items2.Add(new SelectListItem
                {
                    Text = Convert.ToString(year),
                    Value = Convert.ToString(year)
                });
            }

            return items2;
        }// End of Years


        public ActionResult IncrementElegibleEmpList(string MonthName, string Year)
        {
            try
            {
                // var currentDateOfMonth = DateTime.Now; //.Day;
                // var nextPromotionMaxDate = DateTime.Parse($"01-{currentDateOfMonth.Month}-{currentDateOfMonth.Year}").AddMonths(1).AddDays(-1);

                //var Dates =  nextPromotionMaxDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

                string selectedDate = GetNextPromotionDate(Year, MonthName);
                var param = new { selectedDate = selectedDate };
                var mainReport = employeeSpService.GetDataWithParameter(param, "[EMP].IncrementElegibleEmpList");
                var reportParam = new Dictionary<string, object>();

                reportParam.Add("MonthName", MonthName);
                reportParam.Add("Year", Year);

                var dtCompanyInfo = WebHelper.GetCompanyInfo();
                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";

                var reportPartialPath = "Employee/IncrementElegibleEmpList.rpt";
                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, mainReport.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);
 
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        private string GetNextPromotionDate(string Year, string MonthName)
        {
            var currentDateOfMonth = DateTime.Now.Day;
            var nextPromotionMaxDate = DateTime.Parse($"01-{MonthName}-{Year}").AddMonths(1).AddDays(-1);

            return nextPromotionMaxDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
        }

    }//end class
}//end namespace