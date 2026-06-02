using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Payroll
{
    public class PRSalaryScaleViewModel : BaseModel
    {
        public int PRSalaryScaleID { get; set; }
        [Display(Name = "Effective Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EffectiveDate { get; set; }
        [Display(Name = "Scale Grade")]
        public int ScaleGrade { get; set; }
        [Display(Name = "Scale Step")]
        public int ScaleStep { get; set; }
        [Display(Name = "Scale Amount")]
        public decimal ScaleAmount { get; set; }
        [Display(Name = "Basic Amount")]
        public decimal BasicAmount { get; set; }
        public bool ScaleType { get; set; }       
        public string EmployeeTypeName { get; set; }
        public string ComponentGroupName { get; set; }
        public string ComponentName { get; set; }
        public string ComponentType { get; set; }
        public decimal ComponentAmount { get; set; }
        public string RatioBasedOn { get; set; }
        public int EmployeeTypeId { get; set; }//ComponentType,,,
        public double CalculatedAmount { get; set; }
        public int PRComponentId { get; set; }
        public string EffectiveStartDateMsg { get; set; }
        public string EffectiveEndDateMsg { get; set; }
        public string EffectiveDate2 { get; set; }

        public decimal MinimumLimit { get; set; }
        public decimal MaximumLimit { get; set; }

        public string ComponentCategory { get; set; }
        public string TransactionType { get; set; }
        public string EmployeeStatusName { get; set; }
        public int? EmployeeStatusId { get; set; }
        public int? ComponentPayrollId { get; set; }
        public string TransactionTypeView { get; set; }

        public string SalaryChangesByComponent { get; set; }
        public int OfficeId { get; internal set; }
    }


    public class SalaryBeforeApproveUploadModel
    {
        public int Sl { get; set; }
        public string EmployeeCode { get; set; }
    }

    public class PromotionViewModel
    {
        // Existing properties
        //[Required]
        [Display(Name = "Employee ID")]
        public string EmplD { get; set; }

        [Display(Name = "Employee Name")]
        public string EmpName { get; set; }

        //[Required]
        [Display(Name = "Employee Code")]
        public string EmpCode { get; set; }

        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [Display(Name = "Previous Salary")]
        public decimal PreSalary { get; set; }

        [Display(Name = "Increment Amount")]
        public decimal Increment { get; set; }

        //[Required]
        [Display(Name = "New Salary")]
        public decimal NewSalary { get; set; }

        //[Required]
        [Display(Name = "Effect Date")]
        public DateTime EffectDate { get; set; }

        //[Required]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Arear Salary")]
        public decimal? ArearSalary { get; set; }

        [Display(Name = "Arear Dearness Allowance")]
        public decimal? ArearDear { get; set; }

        [Display(Name = "Arear Bonus")]
        public decimal? ArearBonus { get; set; }

        [Display(Name = "Last Salary")]
        public decimal LastSalary { get; set; }

        [Display(Name = "Last Dearness Allowance")]
        public decimal LastDear { get; set; }

        // Additional existing properties
        public string CurrentDesignation { get; set; }
        public string Grade { get; set; }
        public DateTime NextIncrement { get; set; }

        public IEnumerable<SelectListItem> GradeList { get; set; }
        public IEnumerable<SelectListItem> DesignationList { get; set; }
    }
}