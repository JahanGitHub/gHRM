using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.View_PRComponentConfiguration")]
    public partial class View_PRComponentConfiguration
    {
        [Key]
        public int? RowSl { get; set; }
        public int PRComponentID { get; set; }
        public string ComponentName { get; set; }
        public int? ComponentPayrollId { get; set; }
        public string ComponentType { get; set; }
        public decimal ComponentAmount { get; set; }
        public string TransactionType { get; set; }
        public int PRComponentGroupID { get; set; }
        public string ComponentGroupName { get; set; }
        public string ComponentCategory { get; set; }
        public string SalaryAccCode { get; set; }
        public string AccountName { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public bool IsActive { get; set; }
        public bool? IsProductDependent { get; set; }

        public decimal MaximumLimit { get; set; }

        public decimal MinimumLimit { get; set; }

        public int? EmployeeTypeId { get; set; }
        //     public string EmployeeStatus { get; set; }
        public int? EmployeeStatusId { get; set; }
        
        public string RatioBasedOn { get; set; }
        //public bool? IsSalaryEffect { get; set; }
        //public int Productdependent { get; set; }
        public bool SalaryEffect { get; set; }

        public int? MinDuration { get; set; }
        public int? MaxDuration { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? MinLoanAmount { get; set; }
        public decimal? MaxLoanAmount { get; set; }
        public int? OfficeLocationId { get; set; }
        public bool? IsAdjustable { get; set; }
        public int? LoanCalculationId { get; set; }
        public string OfficeLocationName { get; set; }
        public string SalaryChangesByComponent { get; set; }

        public string SalaryRoundType { get; set; }

        public bool IsProvidentFundComponent { get; set; }
        public bool IsSalaryImpactProhibited { get; set; }
        public int? PFTypeId { get; set; }
    }
}
