using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace gHRM.Web.ViewModels.Payroll
{
    public class PRComponentViewModel_designation : BaseModel
    {
        public int PRComponentID { get; set; }

        [Display(Name = "Account Name")]
        public string AccountName { get; set; }

        [Display(Name = "Component Name")]
        public int? ComponentPayrollId { get; set; }

        [Display(Name = "Component Name")]
        public string ComponentName { get; set; }

        [Display(Name = "Component Type")]
        public string ComponentType { get; set; }

        [Display(Name = "Component Amount")]
        public decimal ComponentAmount { get; set; }

        [Display(Name = "Transaction Type")]
        public string TransactionType { get; set; }

        //[Display(Name = "Account Code")]
        //public string AccountCode { get; set; }

        [Display(Name = "Integration with Account Code")]
        public string SalaryAccCode { get; set; }

        public string LoanAccCode { get; set; }

        public string InterestAccCode { get; set; }

        public string IncomeAccCode { get; set; }

        [Display(Name = "Effective Start Date")]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EffectiveStartDate { get; set; }

        public string EffectiveStartDateMsg { get; set; }

        [Display(Name = "Effective End Date")]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EffectiveEndDate { get; set; }

        public string EffectiveEndDateMsg { get; set; }

        [Display(Name = "Component Group")]
        public int PRComponentGroupID { get; set; }

        [Display(Name = "Component Category")]
        public string ComponentCategory { get; set; }

        [Display(Name = "Employee Type")]
        public int? EmployeeTypeId { get; set; }

        [Display(Name = "Ratio Based On")]
        public string RatioBasedOn { get; set; }

        // public string EmployeeStatus { get; set; }

        [Display(Name = "Employee Status")]
        public int? EmployeeStatusId { get; set; }

        public string[] EmpStatusList { get; set; }

        public int[] EmployeeStatusIdList { get; set; }

        [Display(Name = "Component Group")]
        public string ComponentGroupName { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        [Display(Name = "Validate Duration")]
        public string ValidateDurtion { get; set; }

        [Display(Name = "Changes in Regular Configured Salary?")]
        public string SalaryChangesByComponent { get; set; }

        [Display(Name = "Maximum Amount")]
        public Nullable<decimal> MaximumLimit { get; set; }

        [Display(Name = "Minimum Amount")]
        public Nullable<decimal> MinimumLimit { get; set; }

        [Display(Name = "Office Location")]
        public int? OfficeLocationId { get; set; }

        public decimal? InterestRate { get; set; }

        public int? LoanCalculationId { get; set; }

        public string OfficeLocationName { get; set; }

        public int[] OffLocationList { get; set; }

        [Display(Name = "Minimum Loan Duration")]
        public int? MinDuration { get; set; }

        [Display(Name = "Maximum Loan Duration")]
        public int? MaxDuration { get; set; }

       

        [Display(Name = "Need Product Serial?")]
        public bool IsProductDependent { get; set; }

        [Display(Name = "Value Effects Regular Salary Component?")]
        public bool? SalaryEffect { get; set; }

       
        public bool IsSalaryEffect { get; set; }

        [Display(Name = "Provident Fund Integration Required")]
        public bool? IsProvidentFundComponent { get; set; }

        [Display(Name = "Loan Configuration Changable?")]
        public bool? IsAdjustable { get; set; }

        [Display(Name = "Deny Impact on Regular Salary Component?")]
        public bool? IsSalaryImpactProhibited { get; set; }

        [Display(Name = "Round Type")]
        public string SalaryRoundType { get; set; }

        [Display(Name = "Provident Fund Type")]
        public int? PFTypeId { get; set; }


        [Display(Name = "Responsibility (অফিস পদবী)")]
        public string EmployeeRank { get; set; }


        [Display(Name = "Payroll Position (পদবী)")]
        public int DesignationId { get; set; }

        [Display(Name = "Designation (পদবী)")]
        public string DeptDesigStatus { get; set; }


        public string Designation { get; set; }

        public string OrnamentalDesignationName { get; set; }

        public int? OfficeDesignationId { get; set; }

        public string OfficeDesignationName { get; set; }

        public string DesignationName { get; set; }

        public int? GradeId { get; set; }


        #region List

        public IEnumerable<SelectListItem> EmployeeTypeList { get; set; }
        public IEnumerable<SelectListItem> EmployeeStatusList { get; set; }
        public IEnumerable<SelectListItem> ComponentList { get; set; }
        public IEnumerable<SelectListItem> ComponentTypeList { get; set; }
        public IEnumerable<SelectListItem> ComponentCategoryList { get; set; }
        public IEnumerable<SelectListItem> ComponentGroupList { get; set; }
        public IEnumerable<SelectListItem> OfficeLocationList { get; set; }
        public IEnumerable<SelectListItem> RatioBasedList { get; set; }
        public IEnumerable<SelectListItem> DurationList { get; set; }
        public IEnumerable<SelectListItem> TransactionTypeList { get; set; }
        public IEnumerable<SelectListItem> ProductdependentList { get; set; }
        public IEnumerable<SelectListItem> SalaryEffectList { get; set; }
        public IEnumerable<SelectListItem> SalaryChangesByComponentList { get; set; }
        public IEnumerable<SelectListItem> IsAdjustableList { get; set; }
        public IEnumerable<SelectListItem> LoneCalculationList { get; set; }
        public IEnumerable<SelectListItem> ProvidentFundComponentList { get; set; }
        public IEnumerable<SelectListItem> SalaryImpactProhibitedList { get; set; }
        public IEnumerable<SelectListItem> SalaryRoundTypeList { get; set; }

        public IEnumerable<SelectListItem> PFTypeList { get; set; }


        public IEnumerable<SelectListItem> DesignationList { get; set; }

        public IEnumerable<SelectListItem> OfficeDesignationList { get; set; }

        public IEnumerable<SelectListItem> RankList { get; set; }

        #endregion

    }// End of Class
}