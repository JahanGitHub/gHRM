using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
   [Table("gcpf.GLLevel")]
   public partial class GLLevel
    {
       [Required]
       public int GLLevelId { get; set; }
       [Required]
       [MaxLength(10)]
       public string GLLevelName { get; set; }

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
