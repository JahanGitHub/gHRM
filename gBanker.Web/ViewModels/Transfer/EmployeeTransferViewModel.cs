using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeTransferViewModel
    {
        public int Id { get; set; }
        public int? RowSl { get; set; }
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
        public string GradeName { get; set; }
        public string EmployeeStatus { get; set; }

        public string PreviousOfficeType { get; set; }
        public string EmployeePreviousOfficeName { get; set; }
        public string EmployeePreviousDepartmentName { get; set; }
        public string EmployeePreviousSectionName { get; set; }
        public string EmployeePreviousDesignation { get; set; }

        public string GrossSalary { get; set; }

        public int OfficeId { get; set; }

        public string OfficeName { get; set; }
        [Display(Name = "Department")]

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        [Display(Name = "Responsibility")]
        public int OfficeDesignationId { get; set; }

        public string OfficeDesignationName { get; set; }

        [Display(Name = "Order No")]
        public long OrderNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        [Display(Name = "Order Date")]
        public DateTime? OrderDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        [Display(Name = "Planned Joining Date")]
        public DateTime? PlannedJoiningDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        [Display(Name = "Planned Release Date")]
        public DateTime? PlannedReleaseDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        [Display(Name = "Joining Date")]
        public DateTime? JoiningDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        [Display(Name = "Release Date")]
        public DateTime? ReleaseDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        [Display(Name = "Confirmation Date")]
        public DateTime? ConfirmationDate { get; set; }

        public string ConfirmationDateMsg { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        [Display(Name = "Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }

        public string DateOfBirthMsg { get; set; }

        [Display(Name = "Mutual")]
        public bool IsMutual { get; set; }

        public bool? HasJoined { get; set; }

        [Display(Name = "TADA Applicable")]
        public bool IsTADAApplicable { get; set; }

        [Display(Name = "Office Type")]
        public string CurrentOfficeType { get; set; }

        [Display(Name = "Changing Status")]
        public string ChangingStatus { get; set; }
        public int OfficeTypeId { get; set; }

        public int HeadOfficeId { get; set; }
        public int ProjectId { get; set; }
        public int ZoneId { get; set; }
        public int AreaId { get; set; }
        public int UnitId { get; set; }
        public string EntryType { get; set; }
        public string Section { get; set; }

        public int? SectionId { get; set; }

        public string EmployeeRank { get; set; }
        public List<SelectListItem> SectionList { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public List<SelectListItem> ZoneList { get; set; }
        public List<SelectListItem> AreaList { get; set; }
        public List<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> DepartmentNameList { get; set; }
        public IEnumerable<SelectListItem> RankList { get; set; }
        public IEnumerable<SelectListItem> YesNoList { get; set; }
        public IEnumerable<SelectListItem> EntryTypeList { get; set; }

        public int NotificationId1 { get; set; }
        public IEnumerable<SelectListItem> NotificationList1 { get; set; }
        public int NotificationId2 { get; set; }
        public IEnumerable<SelectListItem> NotificationList2 { get; set; }
    }
}