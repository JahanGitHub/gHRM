using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
    [Table("gcpf.ProfitDistProcessLog")]
    public partial class ProfitDistProcessLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProcessLogId { get; set; }

        [Required]
        [Column(TypeName = "smalldatetime")]
        public DateTime YearStartDate { get; set; }

        [Required]
        [Column(TypeName = "smalldatetime")]
        public DateTime YearEndDate { get; set; }

        [Required]
        public bool IsProcessed { get; set; }

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
