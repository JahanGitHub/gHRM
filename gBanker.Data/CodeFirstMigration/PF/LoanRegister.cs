using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
    [Table("gcpf.LoanRegister")]
  public partial class LoanRegister
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LoanRegisterId { get; set; }

        public long EmployeeId { get; set; }

        public long CollectionId { get; set; }

        public int CollectionTypeId { get; set; }

        public long LoanId { get; set; }

        public long? VoucherNo { get; set; }

        public decimal LoanAmount { get; set; }

        public decimal InterestAmount { get; set; }
        public decimal InterestCharge { get; set; }
        

        [Required]
        [StringLength(2)]
        public string TransactionType { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime TransactionDate { get; set; }

        public long CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public bool IsDeleted { get; set; }

        public long? DeletedUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DeleteDate { get; set; }

        public virtual LoanDisbursement LoanDisbursement { get; set; }

        //Old
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public long LoanRegisterId { get; set; }
        //[Required]
        //public long LoanId { get; set; }
        //[Required]
        //public int LoanTypeId { get; set; }

        //[Required]
        ////(18,6)
        //public decimal LoanAmount { get; set; }
        //[Required]
        ////(18,6)
        //public decimal InterestAmount { get; set; }
        //[Required]
        //[MaxLength(2)]
        //public string TransactionType { get; set; }
        //[Column(TypeName = "smalldatetime")]
        //public DateTime TransactionDate { get; set; }



        //[Required]
        //public long CreateUser { get; set; }
        //[Column(TypeName = "smalldatetime")]
        //public DateTime? CreateDate { get; set; }
        //public long? UpdateUser { get; set; }
        //[Column(TypeName = "smalldatetime")]
        //public DateTime? UpdateDate { get; set; }
        //public bool IsDeleted { get; set; }
        //public long? DeletedUser { get; set; }
        //[Column(TypeName = "smalldatetime")]
        //public DateTime? DeleteDate { get; set; }
    }
}
