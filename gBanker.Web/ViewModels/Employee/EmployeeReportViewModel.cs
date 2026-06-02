using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeReportViewModel
    {
        public int ReportId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeCodeMultiple { get; set; }
        public string ReportType { get; set; }
        public string BloodGroup { get; set; }

        [Display(Name = "Office Type")]
        public int OfficeTypeId { get; set; }

        [Display(Name = "Zone Name")]
        public string ZoneId { get; set; }
        [Display(Name = "Area Name")]
        public string AreaId { get; set; }
        [Display(Name = "Unit Name")]
        public string UnitId { get; set; }

        public int? HeadOfficeId { get; set; }
        public int? ProjectId { get; set; }
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
        [Display(Name = "Employee Status (কর্মসংস্থানের অবস্থা)")]
        public string EmployeeStatus { get; set; }

        [Display(Name = "Date From")]
        public string DateFrom { get; set; }

        [Display(Name = "Date To")]
        public string DateTo { get; set; }
        public int DepartmentId { get; set; }
        public string EmploymentType { get; set; }
        public string InstituteName { get; set; }
        public string TrainingTitle { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> DesignationList { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> EmployeeStatusList { get; set; }
        public List<SelectListItem> ReportList { get; set; }
        public List<SelectListItem> BloodGroupList { get; set; }
        public List<SelectListItem> EmploymentTypeList { get; set; }
        [Display(Name = "Designation (পদবী)")]
        public int DesignationId { get; set; }
        public int ActiveInactive { get; set; }

        public IEnumerable<SelectListItem> ReportTypeList { get; set; }
        public IEnumerable<SelectListItem> ActiveInactiveList { get; set; }
        public IEnumerable<SelectListItem> InstituteNameList { get; set; }
        public IEnumerable<SelectListItem> TrainingTitleList { get; set; }


        // Employee Other Report Properties starts

        [Display(Name = "Reason (কারণ)")]
        [Required(ErrorMessage ="{0} is Required")]
        public int ReasonId { get; set; }

        [Display(Name = "Report Type (রিপোর্টের ধরন)")]
        [Required(ErrorMessage = "{0} is Required")]
        public string ReportTypeOther { get; set; }

        [Display(Name= "Office Type")]
        public int OfficeTypeIdNew { get; set; }

        [Display(Name = "Date From (তারিখ হতে)")]
        public string DateFromNew { get; set; }

        [Display(Name = "Date To (তারিখ পর্যন্ত)")]
        public string DateToNew { get; set; }
        public IEnumerable<SelectListItem> ReasonList { get; set; }


        public List<SelectListItem> SectionList { get; set; }

        public IEnumerable<SelectListItem> OfficeDesignationList { get; set; }

        public int? OfficeDesignationId { get; set; }
        public int EmployeeStatusId { get; set; }

        public string Section { get; set; }

        public int? SectionId { get; set; }

    




    }
}