using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Basic
{
    public class EmployeeProfileReportViewModel
    {
        public int EmployeeProfileId { get; set; }
        public IEnumerable<SelectListItem> EmployeeProfileList { get; set; }

        public long EmployeeId { get; set; }
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }
        [Display(Name = "Name")]
        public string EmployeeName { get; set; }
        public int EmployeeCurrentOfficeId { get; set; }
        [Display(Name = "Office Name")]
        public string EmployeeCurrentOfficeName { get; set; }
        [Display(Name = "Department Name")]
        public string EmployeeCurrentDepartmentName { get; set; }
        [Display(Name = "Responsibility")]
        public string EmployeeCurrentDesignation { get; set; }
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        [Display(Name = "Designation")]
        public int OfficeDesignationId { get; set; }
        public string OfficeDesignationName { get; set; }


        public string CurrentOfficeType { get; set; }
        //public string CurrentOfficeName { get; set; }
        public int OfficeTypeId { get; set; }

        public int HeadOfficeId { get; set; }
        public int ProjectId { get; set; }
        public int ZoneId { get; set; }
        public int AreaId { get; set; }
        public int UnitId { get; set; }
        public string EntryType { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public List<SelectListItem> ZoneList { get; set; }
        public List<SelectListItem> AreaList { get; set; }
        public List<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> DepartmentNameList { get; set; }
        public IEnumerable<SelectListItem> RankList { get; set; }
        public IEnumerable<SelectListItem> YesNoList { get; set; }
        public IEnumerable<SelectListItem> EntryTypeList { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
    }
}




