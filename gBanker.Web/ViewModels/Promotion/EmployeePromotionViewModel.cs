using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeePromotionViewModel : BaseModel
    {
        public int rowSl { get; set; }
        public int? RowSl { get; set; }

        public long PromotionId { get; set; }
        public int? DesignationId { get; set; }

        public long EmployeeId { get; set; }

        [Display(Name = "Promotion Type")]
        public string PromotionType { get; set; }
        public int? IsReviewed { get; set; }

        public string IsReviewedString { get; set; }
        public decimal? Pay { get; set; }
        public string EmployeeCode { get; set; }
        [Display(Name = "Employee")]
        public string EmployeeName { get; set; }
        public string OfficeName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HouseRent { get; set; }
        public decimal Medical { get; set; }
        public decimal BonusAmount { get; set; }
        public string PromotionDateMsg { get; set; }
        [Display(Name = "Promotion Date")]
        public DateTime? PromotionDate { get; set; }
        public string NextReviewDateMsg { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public string FirstJoiningDate { get; set; }
        public string EmployeeTypeName { get; set; }
        public string LastPromotionDate { get; set; }
        public string OfficeLocationName { get; set; }

        //public string DesignationName { get; set; }

        [Display(Name = "Payroll Designation")]
        public IEnumerable<SelectListItem> DesignationList { get; set; }

        [Display(Name = "Office Type")]
        public string CurrentOfficeType { get; set; }
        public int OfficeTypeId { get; set; }
        public int HeadOfficeId { get; set; }
        public int ProjectId { get; set; }
        public int ZoneId { get; set; }
        public int AreaId { get; set; }
        public int UnitId { get; set; }
        public string EntryType { get; set; }
        public string Section { get; set; }
        public int? SectionId { get; set; }
        [Display(Name = "From")]
        public string FromDateStr { get; set; }
        [Display(Name = "To")]
        public string ToDateStr { get; set; }
        public List<SelectListItem> SectionList { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public List<SelectListItem> ZoneList { get; set; }
        public List<SelectListItem> AreaList { get; set; }
        public List<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> DepartmentNameList { get; set; }
        public IEnumerable<SelectListItem> RankList { get; set; }
        public IEnumerable<SelectListItem> YesNoList { get; set; }
        public IEnumerable<SelectListItem> EntryTypeList { get; set; }
        public IEnumerable<SelectListItem> DurationList { get; set; }
        public int OfficeId { get; set; }
        public int DepartmentId { get; set; }
        public int EmployeeCurrentOfficeId { get; set; }

        [Display(Name = "Office Name")]
        public string EmployeeCurrentOfficeName { get; set; }
        [Display(Name = "Department Name")]
        public string EmployeeCurrentDepartmentName { get; set; }
        [Display(Name = "Responsibility")]
        public string EmployeeCurrentDesignation { get; set; }
        [Display(Name = "Duration Month")]
        public int? StatusPeriodInMonth { get; set; }

        public IEnumerable<SelectListItem> StatusPeriodInMonthList { get; set; }

        public IEnumerable<SelectListItem> EmployeeSalaryType { get; set; }
        [Display(Name = "Overtime Applicable?")]
        public bool IsOverTime { get; set; }
        public IEnumerable<SelectListItem> OverTimeList { get; set; }
        public IEnumerable<SelectListItem> GradeList { get; set; }
        [Display(Name = "Step")]
        public IEnumerable<SelectListItem> SalaryScaleList { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public IEnumerable<SelectListItem> SalaryGenerationTypeList { get; set; }
        public IEnumerable<SelectListItem> BankList { get; set; }
        public int? IncrementYearFrom { get; set; }
        public IEnumerable<SelectListItem> IncrementYearFromList { get; set; }
        public IEnumerable<SelectListItem> PFTypeList { get; set; }
        public IEnumerable<SelectListItem> PromotionTypeList { get; set; }
        public int? PromotionTypeId { get; set; }
        public string PromotionTypeName { get; set; }
        public string Remarks { get; set; }

        public IEnumerable<SelectListItem> EmployeeStatusList { get; set; }

        public IEnumerable<SelectListItem> BasicOfficeTypeList { get; set; }

        public int BasicOfficeTypeId { get; set; }

        public int? AssessmentYear { get; set; }
        public int? Score { get; set; }





    }// End Class


    public class EmployeePromotionViewModel3 : BaseModel
    {

        [Display(Name = "Office Name")]
        public string EmployeeCurrentOfficeName { get; set; }
        [Display(Name = "Department Name")]
        public string EmployeeCurrentDepartmentName { get; set; }
        [Display(Name = "Responsibility")]
        public string EmployeeCurrentDesignation { get; set; }
        [Display(Name = "Duration Month")]
        public int? StatusPeriodInMonth { get; set; }



        public int EmployeeCurrentOfficeId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public decimal GrossSalary { get; set; }
        public string EmployeeCode { get; set; }

        [Display(Name = "Office Type")]
        public string CurrentOfficeType { get; set; }
        public int OfficeTypeId { get; set; }
        public int HeadOfficeId { get; set; }
        public int ProjectId { get; set; }
        public int ZoneId { get; set; }
        public int AreaId { get; set; }
        public int UnitId { get; set; }

    }// End Class
// End Namespace
}// End Namespace
