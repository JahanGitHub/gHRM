using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class TransferReportViewModel
    {
        public int CCForOfficeOrderId { get; set; }

        public string CCForOfficeOrderName { get; set; }

        public string CCForOfficeOrderNameView { get; set; }

        public int? ViewOrder { get; set; }


        public int ActiveInactive { get; set; }

        public long? OrderNo { get; set; }

        public int ReportId { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeCode { get; set; }

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

        public string EmployeeStatus { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }

        [Display(Name = "Department (বিভাগ)")]
        public int DepartmentId { get; set; }

        [Display(Name = "Employment Type (কাজের ধরন)")]
        public string EmploymentType { get; set; }
     
        [Display(Name = "Designation (পদবী)")]
        public int DesignationId { get; set; }

        [Display(Name = "Section (সেকশন)")]
        public string Section { get; set; }

        [Display(Name = "Responsibility (দায়িত্ব)")]
        public int ResponsibilityId { get; set; }

        [Display(Name = "Service Duration in Company")]
        public string Age { get; set; }

        [Display(Name = "Service Duration in Office")]
        public string AgeOffice { get; set; }

        [Display(Name = "Name")]
        public string EmployeeName { get; set; }

        public string CurrentOfficeType { get; set; }

        [Display(Name = "Office Name")]
        public string EmployeeCurrentOfficeName { get; set; }

        [Display(Name = "Department Name")]
        public string EmployeeCurrentDepartmentName { get; set; }

        [Display(Name = "Responsibility")]
        public string EmployeeCurrentDesignation { get; set; }

        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public List<SelectListItem> SectionList { get; set; }
        public IEnumerable<SelectListItem> ResponsibilityList { get; set; }
        public IEnumerable<SelectListItem> DesignationList { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> EmployeeStatusList { get; set; }
        public List<SelectListItem> ReportList { get; set; }
        public List<SelectListItem> ReportListStatic { get; set; }
        public List<SelectListItem> BloodGroupList { get; set; }
        public List<SelectListItem> EmploymentTypeList { get; set; }
        public IEnumerable<SelectListItem> ReportTypeList { get; set; }
        public IEnumerable<SelectListItem> ActiveInactiveList { get; set; }

        public string ReportPlacementType { get; set; }
        public IEnumerable<SelectListItem> ReportPlacementList { get; set; }
    }
}