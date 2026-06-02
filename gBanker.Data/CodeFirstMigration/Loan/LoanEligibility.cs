using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace gHRM.Data.CodeFirstMigration.Loan
{
    [Table("loan.LoanEligibility")]
    public class LoanEligibility
    {
        [Key]
        public int Id { get; set; }
        public string LoanType { get; set; }
        public int? PurposeId { get; set; }

        public decimal MinmumJobAge { get; set; }
        public decimal MaximumJobAge { get; set; }
        public string PFContribution { get; set; }
        public decimal LoanEligibleInPercent { get; set; }
       // public int? Percentage { get; set; }
        public bool IsActive { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
