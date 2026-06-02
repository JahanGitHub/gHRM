using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Payroll
{
    public class PRSalaryConfigurationViewModel : BaseModel
    {
        public long PRSalaryConfigurationID { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        public int OfficeID { get; set; }
        public long EmployeeID { get; set; }
        public int PRComponentID { get; set; }
        public int PRWorkAreaID { get; set; }
        [Display(Name = "Component Name")]
        public string ComponentName { get; set; }
        public int ScaleOrHouseRentID { get; set; } // No Need
        [Display(Name = "Component Amount")]
        public decimal ComponentAmount { get; set; }
        public decimal BasicSalary { get; set; }
        [Display(Name = "Effective Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string EffectiveStartDateInstring { get; set; }
        [Display(Name = "Effective End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string EffectiveEndDateInString { get; set; }


        public int EmployeeTypeId { get; set; }
        public string CreateDateTime { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }

        public decimal InitialAmount { get; set; }
        public decimal AmountPerIncrement { get; set; }
        [Display(Name = "Gross Salary")]
        public decimal GrossSalary { get; set; }
        [Display(Name = "Bank Account No")]
        public String BankAccountNo { get; set; }
        [Display(Name = "Salary Type")]
        public IEnumerable<SelectListItem> EmployeeSalaryType { get; set; }

        [Display(Name = "Promotion Type")]
        public IEnumerable<SelectListItem> PromotionTypeList { get; set; }

        public IEnumerable<SelectListItem> GradeList { get; set; }
        [Display(Name = "Step")]
        public IEnumerable<SelectListItem> SalaryScaleList { get; set; }
        [Display(Name = "Generation Type")]
        public string SalaryGenerationType { get; set; }
        public IEnumerable<SelectListItem> SalaryGenerationTypeList { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public DateTime LastLoginTime { get; set; }

        [Display(Name = "Increment Month")]
        public int IncrementMonth { get; set; }
        public decimal OvertimehourPerMonth { get; set; }
        public decimal OvertimeRate { get; set; }
        public decimal MaxOvertimePerDay { get; set; }
        public decimal MaxOvertimePerMonth { get; set; }
        [Display(Name = "Overtime Applicable?")]
        public bool IsOverTime { get; set; }
        public IEnumerable<SelectListItem> OverTimeList { get; set; }
        public double IncomeTax { get; set; }
        public string ComponentCategory { get; set; }
        public string TransactionType { get; set; }
        public string EmployeeStatusName { get; set; }
        public int? EmployeeStatusId { get; set; }
        public string JoiningDate { get; set; }
        public string ConfirmationDate { get; set; }
        [Display(Name = "Increment Year")]
        public int? IncrementYearFrom { get; set; }
        public IEnumerable<SelectListItem> IncrementYearFromList { get; set; }
        [Display(Name = "Effective Start Date")]
        public DateTime EffectiveStartDate { get; set; }
        [Display(Name = "Effective End Date")]
        public DateTime EffectiveEndDate { get; set; }
        public string BankName { get; set; }
        [Display(Name = "Bank Branch Name")]
        public string BankBranchName { get; set; }
        public string BankCode { get; set; }
        public IEnumerable<SelectListItem> BankList { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public int? OfficeLocationId { get; set; }

        public string StatusName { get; set; }
        public int OfficeTypeId { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public int PromotionId { get; set; }

        [Display(Name = "Payroll Designation")]
        public IEnumerable<SelectListItem> DesignationList { get; set; }

        public string EmployeeRank { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime FirstJoiningDate { get; set; }
        public decimal TotalEarnings { get; set; }
        public int CompanyId { get; set; }
        public int GradeId { get; set; }
        public int Step { get; set; }


        public decimal SalaryAmount { get; set; }
        public string CreateDateMsg { get; set; }
        public string EffectiveStartDateMsg { get; set; }
        public string EffectiveEndDateMsg { get; set; }

        [Display(Name = "Provident Fund Type")]
        public IEnumerable<SelectListItem> PFTypeList { get; set; }

        [Display(Name = "Provident Fund Type")]
        public int PFTypeId { get; set; }

        public string SalaryRoundType { get; set; }

        public int? Id { get; set; }
        public int? StepFrom { get; set; }
        public int? StepTo { get; set; }
        public string RatioOn { get; set; }
        public int? AmountOrPercent { get; set; }
        public IEnumerable<SelectListItem> EmployeeStatusList { get; set; }

    }

    public class PRSalaryConfigurationViewModel2 : BaseModel
    {
        public long PRSalaryConfigurationID { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        public int OfficeID { get; set; }
        public long EmployeeID { get; set; }
        public int PRComponentID { get; set; }
        public int PRWorkAreaID { get; set; }
        [Display(Name = "Component Name")]
        public string ComponentName { get; set; }
        public int ScaleOrHouseRentID { get; set; } // No Need
        [Display(Name = "Component Amount")]
        public decimal ComponentAmount { get; set; }
        public decimal BasicSalary { get; set; }
        [Display(Name = "Effective Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string EffectiveStartDateInstring { get; set; }
        [Display(Name = "Effective End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string EffectiveEndDateInString { get; set; }


        public int EmployeeTypeId { get; set; }
        public string CreateDateTime { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }

        public decimal InitialAmount { get; set; }
        public decimal AmountPerIncrement { get; set; }
        [Display(Name = "Gross Salary")]
        public decimal GrossSalary { get; set; }
        [Display(Name = "Bank Account No")]
        public String BankAccountNo { get; set; }
        [Display(Name = "Salary Type")]
        public IEnumerable<SelectListItem> EmployeeSalaryType { get; set; }

        [Display(Name = "Promotion Type")]
        public IEnumerable<SelectListItem> PromotionTypeList { get; set; }

        public IEnumerable<SelectListItem> GradeList { get; set; }
        [Display(Name = "Step")]
        public IEnumerable<SelectListItem> SalaryScaleList { get; set; }
        [Display(Name = "Generation Type")]
        public string SalaryGenerationType { get; set; }
        public IEnumerable<SelectListItem> SalaryGenerationTypeList { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public DateTime LastLoginTime { get; set; }

        [Display(Name = "Increment Month")]
        public int IncrementMonth { get; set; }
        public decimal OvertimehourPerMonth { get; set; }
        public decimal OvertimeRate { get; set; }
        public decimal MaxOvertimePerDay { get; set; }
        public decimal MaxOvertimePerMonth { get; set; }
        [Display(Name = "Overtime Applicable?")]
        public bool IsOverTime { get; set; }
        public IEnumerable<SelectListItem> OverTimeList { get; set; }
        public double IncomeTax { get; set; }
        public string ComponentCategory { get; set; }
        public string TransactionType { get; set; }
        public string EmployeeStatusName { get; set; }
        public int? EmployeeStatusId { get; set; }
        public string JoiningDate { get; set; }
        public string ConfirmationDate { get; set; }
        [Display(Name = "Increment Year")]
        public int? IncrementYearFrom { get; set; }
        public IEnumerable<SelectListItem> IncrementYearFromList { get; set; }
        [Display(Name = "Effective Start Date")]
        public DateTime EffectiveStartDate { get; set; }
        [Display(Name = "Effective End Date")]
        public DateTime EffectiveEndDate { get; set; }
        public string BankName { get; set; }
        [Display(Name = "Bank Branch Name")]
        public string BankBranchName { get; set; }
        public string BankCode { get; set; }
        public IEnumerable<SelectListItem> BankList { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public int? OfficeLocationId { get; set; }

        public string StatusName { get; set; }
        public int OfficeTypeId { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public int PromotionId { get; set; }

        [Display(Name = "Payroll Designation")]
        public IEnumerable<SelectListItem> DesignationList { get; set; }

        public string EmployeeRank { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime FirstJoiningDate { get; set; }
        public decimal TotalEarnings { get; set; }
        public int CompanyId { get; set; }
        public int GradeId { get; set; }
        public int Step { get; set; }


        public decimal SalaryAmount { get; set; }
        public string CreateDateMsg { get; set; }
        public string EffectiveStartDateMsg { get; set; }
        public string EffectiveEndDateMsg { get; set; }

        [Display(Name = "Provident Fund Type")]
        public IEnumerable<SelectListItem> PFTypeList { get; set; }

        [Display(Name = "Provident Fund Type")]
        public int PFTypeId { get; set; }

        public string SalaryRoundType { get; set; }

        public int? Id { get; set; }
        public int? StepFrom { get; set; }
        public int? StepTo { get; set; }
        public string RatioOn { get; set; }
        public int? AmountOrPercent { get; set; }

        // add new routing no for prottasyi

        public string RoutingNo { get; set; }
        public IEnumerable<SelectListItem> EmployeeStatusList { get; set; }

    }


    public class PRSalaryConfigurationViewModel3 : BaseModel
    {
        public bool IsEmployeeCodeDisabled { get; set; } // Added property
    
        public long PRSalaryConfigurationID { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        public int OfficeID { get; set; }
        public long EmployeeID { get; set; }
        public int PRComponentID { get; set; }
        public int PRWorkAreaID { get; set; }
        [Display(Name = "Component Name")]
        public string ComponentName { get; set; }
        public int ScaleOrHouseRentID { get; set; } // No Need
        [Display(Name = "Component Amount")]
        public decimal ComponentAmount { get; set; }
        public decimal BasicSalary { get; set; }
        [Display(Name = "Effective Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string EffectiveStartDateInstring { get; set; }
        [Display(Name = "Effective End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string EffectiveEndDateInString { get; set; }


        public int EmployeeTypeId { get; set; }
        public string CreateDateTime { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }

        public decimal InitialAmount { get; set; }
        public decimal AmountPerIncrement { get; set; }
        [Display(Name = "Gross Salary")]
        public decimal GrossSalary { get; set; }
        [Display(Name = "Bank Account No")]
        public String BankAccountNo { get; set; }
        [Display(Name = "Salary Type")]
        public IEnumerable<SelectListItem> EmployeeSalaryType { get; set; }

        [Display(Name = "Promotion Type")]
        public IEnumerable<SelectListItem> PromotionTypeList { get; set; }

        public IEnumerable<SelectListItem> GradeList { get; set; }
        [Display(Name = "Step")]
        public IEnumerable<SelectListItem> SalaryScaleList { get; set; }
        [Display(Name = "Generation Type")]
        public string SalaryGenerationType { get; set; }
        public IEnumerable<SelectListItem> SalaryGenerationTypeList { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public DateTime LastLoginTime { get; set; }

        [Display(Name = "Increment Month")]
        public int IncrementMonth { get; set; }
        public decimal OvertimehourPerMonth { get; set; }
        public decimal OvertimeRate { get; set; }
        public decimal MaxOvertimePerDay { get; set; }
        public decimal MaxOvertimePerMonth { get; set; }
        [Display(Name = "Overtime Applicable?")]
        public bool IsOverTime { get; set; }
        public IEnumerable<SelectListItem> OverTimeList { get; set; }
        public double IncomeTax { get; set; }
        public string ComponentCategory { get; set; }
        public string TransactionType { get; set; }
        public string EmployeeStatusName { get; set; }
        public int? EmployeeStatusId { get; set; }
        public string JoiningDate { get; set; }
        public string ConfirmationDate { get; set; }
        [Display(Name = "Increment Year")]
        public int? IncrementYearFrom { get; set; }
        public IEnumerable<SelectListItem> IncrementYearFromList { get; set; }
        [Display(Name = "Effective Start Date")]
        public DateTime EffectiveStartDate { get; set; }
        [Display(Name = "Effective End Date")]
        public DateTime EffectiveEndDate { get; set; }
        public string BankName { get; set; }
        [Display(Name = "Bank Branch Name")]
        public string BankBranchName { get; set; }
        public string BankCode { get; set; }
        public IEnumerable<SelectListItem> BankList { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public int? OfficeLocationId { get; set; }

        public string StatusName { get; set; }
        public int OfficeTypeId { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public int PromotionId { get; set; }

        [Display(Name = "Payroll Designation")]
        public IEnumerable<SelectListItem> DesignationList { get; set; }

        public string EmployeeRank { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime FirstJoiningDate { get; set; }
        public decimal TotalEarnings { get; set; }
        public int CompanyId { get; set; }
        public int GradeId { get; set; }
        public int Step { get; set; }


        public decimal SalaryAmount { get; set; }
        public string CreateDateMsg { get; set; }
        public string EffectiveStartDateMsg { get; set; }
        public string EffectiveEndDateMsg { get; set; }

        [Display(Name = "Provident Fund Type")]
        public IEnumerable<SelectListItem> PFTypeList { get; set; }

        [Display(Name = "Provident Fund Type")]
        public int PFTypeId { get; set; }

        public string SalaryRoundType { get; set; }

        public int? Id { get; set; }
        public int? StepFrom { get; set; }
        public int? StepTo { get; set; }
        public string RatioOn { get; set; }
        public int? AmountOrPercent { get; set; }
        public IEnumerable<SelectListItem> EmployeeStatusList { get; set; }

    }
}