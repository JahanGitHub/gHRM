using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.PRComponent")]
    public partial class PRComponent_designation
    {
        [Key]
        public int PRComponentID { get; set; }

        public int? ComponentPayrollId { get; set; }
        [Required]
        [StringLength(100)]
        public string ComponentName { get; set; }

        [Required]
        [StringLength(2)]
        public string ComponentType { get; set; }

        public decimal ComponentAmount { get; set; }

        [Required]
        [StringLength(2)]
        public string TransactionType { get; set; }

        public string SalaryAccCode { get; set; }
        public string LoanAccCode { get; set; }

        public string InterestAccCode { get; set; }

        public string IncomeAccCode { get; set; }

        public int? EmployeeStatusId { get; set; }
        public decimal InterestRate { get; set; }

        //[Required]
        //[StringLength(50)]
        //public string AccountCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime EffectiveStartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EffectiveEndDate { get; set; }

        public int PRComponentGroupID { get; set; }

        [StringLength(10)]
        public string ComponentCategory { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public int? EmployeeTypeId { get; set; }
        public string RatioBasedOn { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public Nullable<decimal> MaximumLimit { get; set; }
        public Nullable<decimal> MinimumLimit { get; set; }
        public bool IsProductDependent { get; set; }
        public bool? SalaryEffect { get; set; }
        public int? OfficeLocationId { get; set; }
        public int? MinDuration { get; set; }
        public int? MaxDuration { get; set; }

        //public decimal? MinLoanAmount { get; set; }
        //public decimal? MaxLoanAmount { get; set; }

        public bool? IsAdjustable { get; set; }
        public int? LoanCalculationId { get; set; }
        public string SalaryChangesByComponent { get; set; }
        public bool? IsSalaryImpactProhibited { get; set; }
        public string SalaryRoundType { get; set; }
        public bool? IsProvidentFundComponent { get; set; }
        public int? PFTypeId { get; set; }

        public int? DesignationId { get; set; }

    }
}
