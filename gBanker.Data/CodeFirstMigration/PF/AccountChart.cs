using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
    [Table("gcpf.AccountChart")]
    public partial class AccountChart
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public AccountChart()
        //{
        //    TransactionRegisters = new HashSet<TransactionRegister>();
        //    Vouchers = new HashSet<Voucher>();
        //}

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        [Key]
        [StringLength(25)]
        public string AccountCode { get; set; }

        [Required]
        [StringLength(3)]
        public string AccountTypeCode { get; set; }

        [Required]
        [StringLength(50)]
        public string AccountName { get; set; }

        public int GLLevelId { get; set; }

        public bool IsVoucher { get; set; }

        [StringLength(25)]
        public string ParentAccountCode { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public bool? IsDeleted { get; set; }

        public long? DeletedUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DeleteDate { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<TransactionRegister> TransactionRegisters { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Voucher> Vouchers { get; set; }

       
        
        //Old
        //[Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int AccountId { get; set; }

        //[Key]
        //[Required]
        //[MaxLength(25)]
        //public string AccountCode { get; set; }
        //[Required]
        //[MaxLength(50)]
        //public string AccountName { get; set; }
        //[Required]
        //[MaxLength(3)]
        //public string AccountTypeCode { get; set; }
        //[Required]
        //public int GLLevelId { get; set; }
        //[Required]
        //public bool IsVoucher { get; set; }
        //[MaxLength(25)]
        //public string ParentAccountCode { get; set; }

       
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
