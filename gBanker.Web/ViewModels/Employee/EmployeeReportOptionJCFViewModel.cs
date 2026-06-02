using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeReportOptionJCFViewModel
    {
        //Employee Report Option JCF
        public int Id { get; set; }
        public int EmpReportTypeId { get; set; }
        public string EmpReportTypeName { get; set; }
        public bool IsActive { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }


        /// <summary>
        /// ////////////
        /// </summary>
        /// 

        //public int ReportId { get; set; }
        //public int EmployeeId { get; set; }
        //public string EmployeeCode { get; set; }
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
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public string EducationDegreeCode { get; set; }
        public string ConcentrationCode { get; set; }
        public IEnumerable<SelectListItem> EducationConcentrationList { get; set; }
        public string EmploymentType { get; set; }
        [Display(Name = "Responsibility")]
        public int ResponsibilityId { get; set; }
        public IEnumerable<SelectListItem> EducationDegreeList { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> DesignationList { get; set; }
        public IEnumerable<SelectListItem> ResponsibilityList { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> EmployeeStatusList { get; set; }
        public List<SelectListItem> ReportList { get; set; }
        //public List<SelectListItem> BloodGroupList { get; set; }
        public List<SelectListItem> EmploymentTypeList { get; set; }
        [Display(Name = "Designation (পদবী)")]
        public int DesignationId { get; set; }
        public int ActiveInactive { get; set; }
        public IEnumerable<SelectListItem> ReportTypeList { get; set; }
        public IEnumerable<SelectListItem> ActiveInactiveList { get; set; }
        public string Section { get; set; }
        public List<SelectListItem> SectionList { get; set; }
        public string Gender { get; set; }
        public IEnumerable<SelectListItem> GenderList { get; set; }
        public string Age { get; set; }
        public int DisplaySL { get; set; }

    }
}