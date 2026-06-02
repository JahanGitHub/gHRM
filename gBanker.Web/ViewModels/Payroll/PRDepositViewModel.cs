using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Payroll
{
    public class PRDepositViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Component")]
        [Required]
        public int ComponentPayrollId { get; set; }
        public int PRComponentId { get; set; }
        public string ComponentName { get; set; }

        [Display(Name = "Component Group")]
        [Required]
        public int ComponentGroupId { get; set; }
        public string ComponentGroup { get; set; }

        [Display(Name = "Employee Type")]
        [Required]
        public int EmployeeType { get; set; }

        public string EmployeeTypeName { get; set; }
        
        [Display(Name = "Employee Status")]
        [Required]
        public int EmployeeStatusId { get; set; }

        public string EmployeeStatusName { get; set; }

        [Display(Name = "Is Salary Applicable")]
        [Required]
        public bool IsSalaryApplicable { get; set; }

        [Display(Name = "Is Deposit Required")]
        [Required]
        public int IsDepositRequired { get; set; }

        [Display(Name = "Deposite Type")]
        [Required]
        public string DepositeType { get; set; }

        [Display(Name = "Office Location")]
        [Required]
        public int OfficeLocationId { get; set; }
        
        [Display(Name = "Return Deposite On Employee Status")]
        public int ReturnDepositeOnEmployeeStatusId { get; set; }

        public string ReturnDepositeOn { get; set; }
        public string ReturnDepositeOnEmployeeStatus { get; set; }

        public decimal MaximumLimit { get; set; }
        public decimal MinimumLimit { get; set; }

        [Display(Name = "No of Salary Days")]
        public int? NoOfSalaryDays { get; set; }

        [Display(Name = "Effective Start Date")]
        [Required]
        public DateTime EffectiveStartDate { get; set; }

        [Display(Name = "Effective End Date")]
        [Required]
        public DateTime EffectiveEndDate { get; set; }

        [Display(Name = "Transaction Type")]
        [Required]
        public string TransactionType { get; set; }

        [Display(Name = "Salary Deposit And RefundType")]    
        public string SalaryDepositAndRefundType { get; set; }

        public decimal GrossSalary { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EffectiveStartDateView { get; set; }
        public string EffectiveEndDateView { get; set; }
        public string EffectiveDate { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }


        public bool IsRefundRequired { get; set; }
        public string EmployeeName { get; set; }
        public decimal RefundAmount { get; set; }
        public int RefundDays { get; set; }
        public decimal DepositeAmount { get; set; }
        public string NoOfSalaryDaysInText { get; set; }

        [Required]
        public int SalaryYear { get; set; }
        [Required]
        public int SalaryMonth { get; set; }
        public string ComponentCategory { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public IEnumerable<SelectListItem> IsDepositRequiredList { get; set; }
        public IEnumerable<SelectListItem> EmployeeStatusIdList { get; set; }
        public IEnumerable<SelectListItem> EmployeeTypeList { get; set; }
        public IEnumerable<SelectListItem> ComponentList { get; set; }
        public IEnumerable<SelectListItem> DepositeTypeList { get; set; }
        public IEnumerable<SelectListItem> SalaryDepositAndRefundTypeList { get; set; }
        public IEnumerable<SelectListItem> TransactionTypeList { get; set; }
        public IEnumerable<SelectListItem> ComponentGroupList { get; set; }
        public IEnumerable<SelectListItem> OfficeLocationList { get; set; }
        public string OfficeLocationName { get; set; }
    }

    public class TempDepositRequiredEmployee
    {
        public int PRComponentId { get; set; }
        public int EmployeeType { get; set; }
        public string EmployeeTypeName { get; set; }
        public int EmployeeStatus { get; set; }
        public string EmployeeStatusName { get; set; }
        public string DepositeType { get; set; }
        public int NoOfSalaryDays { get; set; }
        public decimal GrossSalary { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public decimal DepositeAmount { get; set; }
        public string EmployeeName { get; set; }
        public string TransactionType { get; set; }
        public string ComponentGroup { get; set; }
        public string ComponentName { get; set; }
        public int OfficeLocationId { get; set; }
    }

    public class TempRefundRequiredEmployee
    {
        public int PRComponentId { get; set; }
        public int? EmployeeType { get; set; }
        public string EmployeeTypeName { get; set; }
        public int EmployeeStatusId { get; set; }
        public string EmployeeStatusName { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public decimal RefundAmount { get; set; }
        public int RefundDays { get; set; }
        public string EmployeeName { get; set; }
        public string TransactionType { get; set; }
        public string ComponentGroup { get; set; }
        public string ComponentName { get; set; }
        public decimal? GrossSalary { get; set; }
        public string StatusName { get; set; }
        public int StatusId { get; set; }
        public string StatusValue { get; set; }
    }
}