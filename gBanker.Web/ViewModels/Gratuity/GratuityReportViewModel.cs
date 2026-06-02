using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class GratuityReportViewModel
    {
        [Display(Name = "EmployeeCode")]
        public string EmployeeCode { get; set; }
        public string ReportType { get; set; }
        public int DepartmentId { get; set; }
        public int SectionId { get; set; }
        public IEnumerable<SelectListItem> ReportTypeList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> SectionList { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
    }
}