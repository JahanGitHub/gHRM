using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.PerformanceEvaluations
{
    public class AddOrEditPerformanceEvaluationViewModel
    {
        public int PerformanceEvaluationId { get; set; }
        public Int64 EmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        [Required(ErrorMessage = "{0} is Required")]
        public string EmployeeCode { get; set; }
        
        [Display(Name = "Evaluation Year")]
        [Required(ErrorMessage = "{0} is Required")]
        public int EvaluationYear { get; set; }

        [Display(Name = "Evaluation Month")]
        [Required(ErrorMessage = "{0} is Required")]
        public int EvaluationMonth { get; set; }

        [Display(Name = "Total Samity")]
        [Required(ErrorMessage = "{0} is Required")]
        //[Range(1, 9999)]
        public int TotalSamity { get; set; }

        [Display(Name = "Total Member")]
        [Required(ErrorMessage = "{0} is Required")]
        //[Range(1, 99999)]
        public int TotalMember { get; set; }

        [Display(Name = "Total Loanee")]
        [Required(ErrorMessage = "{0} is Required")]
        //[Range(1, 99999)]
        public int TotalLoanee { get; set; }

        [Display(Name = "OSP")]
        [Required(ErrorMessage = "{0} is Required")]
        //[RegularExpression(@"^\d+(\.\d{0,2})?$", ErrorMessage = "It cannot have more than 2 decimal point value")]
        //[Range(0.1, 999999999)]
        public decimal OSP { get; set; }

        [Display(Name = "Special Savings")]
        [Required(ErrorMessage = "{0} is Required")]
        //[RegularExpression(@"^\d+(\.\d{0,2})?$", ErrorMessage = "It cannot have more than 2 decimal point value")]
        //[Range(0.1, 999999999)]
        public decimal SpecialSavings { get; set; }

        [Display(Name = "General Savings")]
        [Required(ErrorMessage = "{0} is Required")]
        //[RegularExpression(@"^\d+(\.\d{0,2})?$", ErrorMessage = "It cannot have more than 2 decimal point value")]
        //[Range(0.1, 999999999)]
        public decimal GeneralSavings { get; set; }

        [Display(Name = "Loan Disburse")]
        [Required(ErrorMessage = "{0} is Required")]
        //[RegularExpression(@"^\d+(\.\d{0,2})?$", ErrorMessage = "It cannot have more than 2 decimal point value")]
        //[Range(0.1, 999999999)]
        public decimal LoanDisburse { get; set; }

        [Display(Name = "Loan Repaid")]
        [Required(ErrorMessage = "{0} is Required")]
        //[RegularExpression(@"^\d+(\.\d{0,2})?$", ErrorMessage = "It cannot have more than 2 decimal point value")]
        //[Range(0.1, 999999999)]
        public decimal LoanRepaid { get; set; }

        [Display(Name = "Loan Outstanding")]
        [Required(ErrorMessage = "{0} is Required")]
        //[RegularExpression(@"^\d+(\.\d{0,2})?$", ErrorMessage = "It cannot have more than 2 decimal point value")]
        //[Range(0.1, 999999999)]
        public decimal LoanOutstanding { get; set; }

        [Display(Name = "Current Due No")]        
        //[Range(1, 999)]
        public int? CurrentDueNo { get; set; }

        
        [Display(Name = "Current Due Amount")]
        //[RegularExpression(@"^\d+(\.\d{0,2})?$", ErrorMessage = "It cannot have more than 2 decimal point value")]
        //[Range(0.1, 999999999)]
        public decimal? CurrentDue { get; set; }

        [Display(Name = "Over Due No")]
        //[Range(1, 999)]
        public int? OverDueNo { get; set; }

        [Display(Name = "Over Due Amount")]
        //[RegularExpression(@"^\d+(\.\d{0,2})?$", ErrorMessage = "It cannot have more than 2 decimal point value")]
        //[Range(0.1, 999999999)]
        public decimal? OverDue { get; set; }


        //additional
        public List<SelectListItem> Years { get; set; }
        public List<SelectListItem> Months { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Department")]
        public string EmployeeDepartment { get; set; }

        [Display(Name = "Designation Status")]
        public string EmployeeDesignationStatus { get; set; }

        [Display(Name = "Employee Status")]
        public string EmployeeEmployeeStatus { get; set; }

        public string DepartmentId { get; set; }

        [Display(Name = "Office Name অফিসের নাম")]
        public int? OfficeId { get; set; }

        [Display(Name = "Office Type (অফিসের ধরণ)")]
        [Required(ErrorMessage = "{0} is Required")]
        public int OfficeTypeId { get; set; }

        [Display(Name = "Head Office (প্রধান কার্য্যালয়)")]
        public int? PVHeadOfficeId { get; set; }

        [Display(Name = "Project Office (প্রোজেক্ট অফিস)")]
        public int? PVProjectId { get; set; }        

        [Display(Name = "Zone Name (যোনের নাম)")]
        public int? ZoneId { get; set; }

        [Display(Name = "Area Name (এরিয়ার নাম)")]
        public int? AreaId { get; set; }

        [Display(Name = "Unit/Branch Name (ইউনিট/ব্রাঞ্চের নাম)")]//"Branch Name (শাখার নাম)"
        public int? UnitId { get; set; }

        public int? ProjectId { get; set; }
        public int? HeadOfficeId { get; set; }


        public IEnumerable<SelectListItem> HOList { get; set; }

        public IEnumerable<SelectListItem> ZoneList { get; set; }

        public IEnumerable<SelectListItem> AreaList { get; set; }

        public IEnumerable<SelectListItem> UnitList { get; set; }

        public IEnumerable<SelectListItem> BranchList { get; set; }

        public IEnumerable<SelectListItem> OfficeList { get; set; }

        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
    }
}