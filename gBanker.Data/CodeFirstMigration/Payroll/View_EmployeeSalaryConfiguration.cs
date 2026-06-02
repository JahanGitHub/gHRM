using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.View_EmployeeSalaryConfiguration")]
    public class View_EmployeeSalaryConfiguration
    {
        [Key]
        public Nullable<int> rowSl { get; set; }
        public int OfficeID { get; set; }
        public long EmployeeID { get; set; }
        public int PRComponentId { get; set; }
        public string EmployeeTypeName { get; set; }
        public string ComponentGroupName { get; set; }
        public string ComponentName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsOverTime { get; set; }
        public Nullable<decimal> MaxOvertimePerDay { get; set; }
        public Nullable<decimal> MaxOvertimePerMonth { get; set; }

        public decimal CalculatedAmount { get; set; }
        public string ComponentType { get; set; }
        public string RatioBasedOn { get; set; }
        public int EmployeeTypeId { get; set; }
        public string EffectiveStartDate { get; set; }
        public string EffectiveEndDate { get; set; }
        public Nullable<decimal> GrossSalary { get; set; }
        public Nullable<decimal> BasicSalary { get; set; }
        public string BankAccountNo { get; set; }
        public Nullable<int> Step { get; set; }
        public Nullable<decimal> FractionStep { get; set; }
        public Nullable<int> GradeId { get; set; }
        public string LogInTime { get; set; }
        public string LogOutTime { get; set; }
        public string LastLoginTime { get; set; }
        public Nullable<decimal> OvertimeRate { get; set; }
        //public Nullable<int> IncrementMonth { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameBng { get; set; }
        public long PRSalaryConfigurationId { get; set; }
        public string ComponentCategory { get; set; }
        public Nullable<decimal> MaximumLimit { get; set; }
        public Nullable<decimal> MinimumLimit { get; set; }
        public string TransactionType { get; set; }
        public string TransactionTypeView { get; set; }
        public DateTime FirstJoiningDate { get; set; }
        // public int IncrementYear { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public int? EmployeeStatusId { get; set; }
        public string EmployeeStatusName { get; set; }
        public string EmployeeStatusValue { get; set; }
        public bool? IsSalaryApplicable { get; set; }
        public int? OfficeLocationId { get; set; }

        public int? PFTypeId { get; set; }

        //public string DepartmentName { get; set; }
        //public string DesignationName { get; set; }
        //public int? IncrementYearFrom { get; set; }
        //public string FirstJoiningDateMsg { get; set; }
        //public string ConfirmationDateMsg { get; set; }

        //public decimal? SalaryAmount { get; set; }
        //public DateTime CreateDate { get; set; }
        //public string CreateDateMsg { get; set; }


    }
}
