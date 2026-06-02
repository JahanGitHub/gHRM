using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
   [Table("gcpf.LoanDisbursement")]
   public partial class LoanDisbursement
    {
       [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LoanId { get; set; }

        public long EmployeeId { get; set; }

        public int LoanTypeId { get; set; }
        public int LoanTerm { get; set; }

        public decimal DisburseAmount { get; set; }

        public decimal IntersetRate { get; set; }

        public int NoOfInstallment { get; set; }

        public decimal MonthlyInstallment { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime DisburseDate { get; set; }

        public decimal? LoanPaid { get; set; }

        public decimal? InterestPaid { get; set; }

        public decimal? InterestCharge { get; set; }

        [Column(TypeName = "date")]
        public DateTime LastInstallmentDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PaidOffDate { get; set; }

        public bool IsInstallmentOver { get; set; }

        public long CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public bool? IsDeleted { get; set; }

        public long? DeletedUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DeleteDate { get; set; }        
    }
}
