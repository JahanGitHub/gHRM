using gHRM.Core.Utilities.Constants;
using gHRM.Service;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers
{
    public class GratuityReportController : BaseController
    {
        IOfficeTypeService _OfficeTypeService;
        IOfficeService _OfficeService;
        IGratuityConfigService _GratuityConfigService;
        IEmployeeService _EmployeeService;

        public GratuityReportController(IOfficeTypeService _OfficeTypeService, IOfficeService _OfficeService, IGratuityConfigService _GratuityConfigService, IEmployeeService _EmployeeService)
        {
            this._OfficeTypeService = _OfficeTypeService;
            this._OfficeService = _OfficeService;
            this._GratuityConfigService = _GratuityConfigService;
            this._EmployeeService = _EmployeeService;
        }

        public ActionResult Index()
        {
            GratuityReportViewModel model = new GratuityReportViewModel();
            MapDropDown(model);
            return View(model);
        }
        public ActionResult Index2()
        {
            GratuityReportViewModel model = new GratuityReportViewModel();
            MapDropDown(model);
            return View(model);
        }


        public void MapDropDown(GratuityReportViewModel model)
        {
            var reportList = new List<SelectListItem>();
            reportList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            reportList.Add(new SelectListItem() { Text = "Employee Code", Value = "EC" });
            reportList.Add(new SelectListItem() { Text = "Summary", Value = "SM" });
            model.ReportTypeList = reportList;

            var MonthList = new List<SelectListItem>();
            MonthList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 1; i <= 12; i++)
            {
                MonthList.Add(new SelectListItem { Text = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Value = i.ToString() });
            }
            model.MonthList = MonthList;

            var officeType = _OfficeTypeService.GetAll().Where(w => w.IsActive == true);
            var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeTypeId.ToString(),
                Text = string.Format("{0}", x.OfficeTypeName)
            });
            var officeType_items = new List<SelectListItem>();
            officeType_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            officeType_items.AddRange(viewofficeType);
            model.OfficeTypeList = officeType_items;

            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            model.OfficeList = ofc_items;

            var ZoneList = _OfficeService.GetAll().Where(x => x.OfficeTypeId == 4 && x.IsActive == true);
            var viewZoneList = ZoneList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = x.OfficeName.ToString()
            });
            var zone_items = new List<SelectListItem>();
            zone_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            zone_items.AddRange(viewZoneList);
            model.ZoneList = zone_items;

            var area_items = new List<SelectListItem>();
            area_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            model.AreaList = area_items;

            var unit_items = new List<SelectListItem>();
            unit_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            model.UnitList = unit_items;

            //department           
            var departmentItems = new List<SelectListItem>();
            departmentItems.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            model.DepartmentList = departmentItems;

            //section           
            var sectionItems = new List<SelectListItem>();
            sectionItems.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            model.SectionList = sectionItems;
        }

        public ActionResult GratuityIndividualReport(string EmpCode)
        {
            try
            {
                string format = "pdf";
                DataSet mainDataSource = GetGratuityIndividualReportData(EmpCode);
                DateTime? FirstJoiningDate = _EmployeeService.GetFirstJoiningDateByCode(EmpCode);
                var parameters = new Dictionary<string, object>();
                parameters.Add("CompanyName", SessionHelper.CompanyName);
                parameters.Add("CompanyAddress", SessionHelper.CompanyAddress);
                parameters.Add("JoiningDate", FirstJoiningDate.Value);

                var reportDataSourceName = "GratuityIndividualReport";

                string reportTitle = "Gratuity Individual Report";
                string reportPath = "~/Reports/RDLC/Gratuity/GratuityIndividualReport.rdlc";
                string reportViewMode = ReportViewModeConstants.Potrait;

                return Report(mainDataSource.Tables[0], reportDataSourceName, parameters, reportTitle, reportPath, format, "view", reportViewMode);
            }
            catch (Exception ex)
            {
                return Redirect("/CommonReportGenerator/CommonReportGenerationError");
            }
        }

        public ActionResult GratuitySummaryReport(long OfficeTypeId, long OfficeId)
        {
            try
            {
                string format = "pdf";
                DataSet mainDataSource = GetGratuitySummaryReportData(OfficeTypeId, OfficeId);
                var parameters = new Dictionary<string, object>();
                parameters.Add("CompanyName", SessionHelper.CompanyName);
                parameters.Add("CompanyAddress", SessionHelper.CompanyAddress);

                var reportDataSourceName = "GratuitySummaryReport";

                string reportTitle = "Gratuity Summary Report";
                string reportPath = "~/Reports/RDLC/Gratuity/GratuitySummaryReport.rdlc";
                string reportViewMode = ReportViewModeConstants.Potrait;

                return Report(mainDataSource.Tables[0], reportDataSourceName, parameters, reportTitle, reportPath, format, "view", reportViewMode);
            }
            catch (Exception ex)
            {
                return Redirect("/CommonReportGenerator/CommonReportGenerationError");
            }
        }

        public ActionResult GratuitySummaryReport2(long OfficeTypeId, long OfficeId)
        {
            try
            {
                string format = "pdf";
                DataSet mainDataSource = GetGratuitySummaryReportData2(OfficeTypeId, OfficeId);
                var parameters = new Dictionary<string, object>();
                parameters.Add("CompanyName", SessionHelper.CompanyName);
                parameters.Add("CompanyAddress", SessionHelper.CompanyAddress);

                var reportDataSourceName = "GratuitySummaryReport";

                string reportTitle = "Gratuity Summary Report";
                string reportPath = "~/Reports/RDLC/Gratuity/GratuitySummaryReport2.rdlc";
                string reportViewMode = ReportViewModeConstants.Potrait;

                return Report(mainDataSource.Tables[0], reportDataSourceName, parameters, reportTitle, reportPath, format, "view", reportViewMode);
            }
            catch (Exception ex)
            {
                return Redirect("/CommonReportGenerator/CommonReportGenerationError");
            }
        }

        private DataSet GetGratuityIndividualReportData(string EmpCode)
        {
            var param = new { EmployeeCode = EmpCode };
            var mainDataSource = _GratuityConfigService.GetDataWithParameter(param, "gr.SP_GratuityIndividualReport");
            return mainDataSource;
        }

        private DataSet GetGratuitySummaryReportData(long OfficeTypeId, long OfficeId)
        {
            var param = new { OfficeTypeId = OfficeTypeId, OfficeId = OfficeId };
            var mainDataSource = _GratuityConfigService.GetDataWithParameter(param, "gr.SP_GratuitySummaryReport");
            return mainDataSource;
        }

        private DataSet GetGratuitySummaryReportData2(long OfficeTypeId, long OfficeId)
        {
            var param = new { OfficeTypeId = OfficeTypeId, OfficeId = OfficeId };
            var mainDataSource = _GratuityConfigService.GetDataWithParameter(param, "gr.SP_GratuitySummaryReport2");
            return mainDataSource;
        }
    }
}