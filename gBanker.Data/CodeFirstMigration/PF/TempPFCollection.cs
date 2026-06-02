using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
    [Table("TempPF_LoanCollection")]
    public partial class TempPFCollection
    {
        [Key, Column(Order = 0)]
        public long EmployeeId { get; set; }
        public int OfficeID { get; set; }
        [Key, Column(Order = 1)]
        public int PFDistributionMonth { get; set; }
        [Key, Column(Order = 2)]
        public int PFDistributionYear { get; set; }
        public DateTime PFDistributionDate { get; set; }
        public decimal? EmployeeContribution { get; set; }
        public decimal? OfficeContribution { get; set; }
        public int? PFLoanID { get; set; }
        public decimal? PFLoanPrincipalColl { get; set; }
        public decimal? PFLoanInterestColl { get; set; }
        public decimal? PFLoanInterestCharge { get; set; }
        public decimal? PFLoanCollection { get; set; }
        public int? CLLoanID { get; set; }
        public decimal? CLLoanPrincipalColl { get; set; }
        public decimal? CLLoanInterestColl { get; set; }
        public decimal? CLLoanInterestCharge { get; set; }
        public decimal? CLLoanCollection { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }
}
