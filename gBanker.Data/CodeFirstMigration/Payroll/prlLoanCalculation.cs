using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Loan
{
    [Table("prl.LoanCalculation")]
    public partial class prlLoanCalculation
    {
        [Key]
        public int LoanCalculationId { get; set; }

        [StringLength(50)]
        public string LoanCalculationName { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public long? CreateBy { get; set; }

        public long? UpdateBy { get; set; }
    }
}
