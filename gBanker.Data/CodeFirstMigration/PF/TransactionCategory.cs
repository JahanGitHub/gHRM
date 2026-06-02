using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
    [Table("gcpf.TransactionCategory")]
    public class TransactionCategory
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TransCategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string TransCategoryName { get; set; }

        [Required]
        [StringLength(2)]
        public string TransGroupName { get; set; }

        public int? AccountId { get; set; }

        [Required]
        [StringLength(2)]
        public string TransactionType { get; set; }

        [Required]
        [StringLength(150)]
        public string Particulars { get; set; }

        public int ReverseAccountId { get; set; }

        [Required]
        [StringLength(2)]
        public string ReverseTransactionType { get; set; }

        [Required]
        [StringLength(150)]
        public string ReverseParticulars { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public bool IsDeleted { get; set; }

        public long? DeletedUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DeleteDate { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Collection> Collections { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ContributionRegister> ContributionRegisters { get; set; }


        //Old
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        //public int TransCategoryId { get; set; }
        //[Required]
        //[MaxLength(50)]
        //public string TransCategoryName { get; set; }
        //[Required]
        //[MaxLength(2)]
        //public string TransGroupName { get; set; }

        //[Required]
        //public int AccountId { get; set; }
        //[Required]
        //[MaxLength(2)]
        //public string TransactionType { get; set; }

        //[Required]
        //[MaxLength(150)]
        //public string Particulars { get; set; }


        //[Required]
        //public int ReverseAccountId { get; set; }
        //[Required]
        //[MaxLength(2)]
        //public string ReverseTransactionType { get; set; }

        //[Required]
        //[MaxLength(150)]
        //public string ReverseParticulars { get; set; }
        

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
