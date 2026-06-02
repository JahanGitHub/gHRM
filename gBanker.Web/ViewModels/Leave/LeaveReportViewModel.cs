using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class LeaveReportViewModel
    {
        public int LeaveStatusYear { get; set; }
        public string LeaveStatusMonth { get; set; }
        public string ReportType { get; set; }
        
        [Display(Name="Employee Code")]
        public string EmployeeCode { get; set; }
        public List<SelectListItem> YearList { get; set; }
        public List<SelectListItem> MonthList { get; set; }
        public List<SelectListItem> ReportTypeList { get; set; }

        [Display(Name = "Employee Status (কর্মসংস্থানের অবস্থা)")]
        public string EmployeeStatus { get; set; }

        public int? OfficeTypeId { get; set; }
        public int? HeadOfficeId { get; set; }
        public int? ZoneId { get; set; }
        public int? AreaId { get; set; }
        public int? UnitId { get; set; }

        [Display(Name = "Date From")]
        public string DateFrom { get; set; }

        [Display(Name = "Date To")]
        public string DateTo { get; set; }


        public int EmployeeStatusId { get; set; }

        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> HOList { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }

        public IEnumerable<SelectListItem> EmployeeStatusList { get; set; }
    } 
}