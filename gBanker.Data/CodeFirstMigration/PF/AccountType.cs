using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
    [Table("gcpf.AccountType")]
   public class AccountType
    {
       [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.None)]
       public int AccountTypeId { get; set; }
       [Required]
       [MaxLength(3)]
       public string AccountTypeCode { get; set; }
       [Required]
       [MaxLength(150)]
       public string AccountTypeName { get; set; }

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
