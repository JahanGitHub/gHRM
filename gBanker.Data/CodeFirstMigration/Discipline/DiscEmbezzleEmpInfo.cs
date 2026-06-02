using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace gHRM.Data.CodeFirstMigration.Discipline
{
    [Table("disc.DiscEmbezzleEmpInfo")]
    public partial class DiscEmbezzleEmpInfo
    {
        [Key]
        public int EmbezzleEmpId { get; set; }

        public long EmployeeId { get; set; }
        public int? EmbezzleId { get; set; }
        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
    }
}
