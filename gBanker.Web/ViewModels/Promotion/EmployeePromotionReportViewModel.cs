using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeePromotionReportViewModel
    {
        public string ReportType { get; set; }
        public string Date { get; set; }
        public int BasicOfficeTypeId { get; set; }
        public int DesignationId { get; set; }
        public string TotalServiceYear { get; set; }
        public string ServiceYearFromLastPromotion { get; set; }
        public IEnumerable<SelectListItem> ReportTypeList { get; set; }
        public IEnumerable<SelectListItem> BasicOfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> DesignationList { get; set; }

        public string EmployeeCode { get; set; }
        public string DateTo { get; set; }
    }
}