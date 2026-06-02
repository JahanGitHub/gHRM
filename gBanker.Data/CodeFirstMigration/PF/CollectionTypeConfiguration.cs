using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
   [Table("gcpf.CollectionTypeConfiguration")]
   public partial class CollectionTypeConfiguration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CollectionType { get; set; }
        public int PrincipalInPer { get; set; }
        public int InterestInPer { get; set; }
        public bool IsActive { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}
