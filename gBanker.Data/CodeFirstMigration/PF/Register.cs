using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
    [Table("gcpf.Register")]
   public partial class Register
    {
    //   [TransactionRegisterId] [bigint] NOT NULL,
    //[AccountCode] [varchar](25) NOT NULL,
    //[VoucherNo] [bigint] NOT NULL,
    //[Amount] [decimal](18, 10) NOT NULL,
    //[TransactionType] [nvarchar](2) NOT NULL,
    //[TransactionDate] [smalldatetime] NULL,
    //[Particulars] [nvarchar](100) NOT NULL,

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TransactionRegisterId { get; set; }
        [Required]
        [MaxLength(25)]
        public string AccountCode { get; set; }
        [Required]
        public long VoucherNo { get; set; }
        [Required]
        //("18,10")
        public decimal Amount { get; set; }
        [Required]
        [MaxLength(2)]
        public string TransactionType { get; set; }
        [DataType("smalldatetime")]
        public DateTime TransactionDate { get; set; }
        [Required]
        [MaxLength(100)]
        public string Particulars { get; set; }
       
       



        [Required]
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
    }
}
