using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
    [Table("gcpf.Voucher")]
   public partial class Voucher
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SerialNo { get; set; }

        [Required]
        [StringLength(25)]
        public string AccountCode { get; set; }

        public long VoucherNo { get; set; }

        public decimal Amount { get; set; }

        [Required]
        [StringLength(2)]
        public string TransactionType { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime TransactionDate { get; set; }

        [Required]
        [StringLength(100)]
        public string Particulars { get; set; }

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

        public virtual AccountChart AccountChart { get; set; }

        //Old
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int SerialNo { get; set; }
        //[Required]
        //[MaxLength(25)]
        //public string AccountCode { get; set; }
        //[Required]
        //public long VoucherNo { get; set; }
        //[Required]
        ////("18,10")
        //public decimal Amount { get; set; }
        //[Required]
        //[MaxLength(2)]
        //public string TransactionType { get; set; }
        //[DataType("smalldatetime")]
        //public DateTime TransactionDate { get; set; }
        //[Required]
        //[MaxLength(100)]
        //public string Particulars { get; set; }

        ////Additional 3 columns for Reporting
        //[NotMapped]
        //public string AccountName { get; set; }
        //[NotMapped]
        //public decimal Dr { get; set; }
        //[NotMapped]
        //public decimal Cr { get; set; }



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
