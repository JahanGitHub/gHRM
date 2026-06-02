using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
   [Table("gcpf.Collection")]
   public partial class Collection
    {

        //Old and Right
       // [Key]
       // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       // public long CollectionId { get; set; }

       // [Required]
       // [ForeignKey("EmployeeConfiguration")]
       // public long EmployeeId { get; set; }
       //// public EmployeeConfiguration EmployeeConfiguration { get; set; }


       // public int CollectionTypeId { get; set; }
       // [ForeignKey("CollectionTypeId")]
       // public TransactionCategory TransactionCategory { get; set; }

       // public long? LoanId { get; set; }

       // [Required]
       // public long VoucherNo { get; set; }
       // //(18,6)
       // public decimal SelfContribution { get; set; }
       // //(18,6)
       // public decimal OrgContribution { get; set; }
       // //(18,6)
       // public decimal LoanAmount { get; set; }
       // //(18,6)
       // public decimal InterestAmount { get; set; }
       // [Required]
       // [MaxLength(2)]
       // public string TransactionType { get; set; }
       // [Required]
       // [DataType("smalldatetime")]
       // public DateTime TransactionDate { get; set; }

       // [Required]
       // public long CreateUser { get; set; }
       // [Column(TypeName = "smalldatetime")]
       // public DateTime? CreateDate { get; set; }
       // public long? UpdateUser { get; set; }
       // [Column(TypeName = "smalldatetime")]
       // public DateTime? UpdateDate { get; set; }
       // public bool IsDeleted { get; set; }
       // public long? DeletedUser { get; set; }
       // [Column(TypeName = "smalldatetime")]
       // public DateTime? DeleteDate { get; set; }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CollectionId { get; set; }

        public long EmployeeId { get; set; }

        public int CollectionTypeId { get; set; }

        public long? LoanId { get; set; }

        //public long VoucherNo { get; set; }
        public string VoucherNo { get; set; }

        public decimal SelfContribution { get; set; }

        public decimal OrgContribution { get; set; }

        public decimal LoanAmount { get; set; }

        public decimal InterestAmount { get; set; }
        public decimal? InterestCharge { get; set; }
       

        [Required]
        [StringLength(2)]
        public string TransactionType { get; set; }

        [StringLength(200)]
        public string Comments { get; set; }

        [StringLength(2)]
        public string VoucherTypeID { get; set; }
       

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

        public virtual Employee Employee { get; set; }

        //public virtual TransactionCategory TransactionCategory { get; set; }

       
       
    }
}
