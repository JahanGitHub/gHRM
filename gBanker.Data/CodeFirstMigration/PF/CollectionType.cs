using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
    [Table("gcpf.CollectionType")]
    public partial class CollectionType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CollectionTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string CollectionTypeName { get; set; }

        [Required]
        [MaxLength(2)]
        public string Group { get; set; }

        [Required]
        [MaxLength(2)]
        public string TransactionType { get; set; }

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
