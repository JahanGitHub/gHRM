using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace gHRM.Data.CodeFirstMigration.Discipline
{
    [Table("disc.DiscMemorendumMaster")]
    public partial class DiscMemorendumMaster
    {
        [Key]
        public int MemorendumMasterId { get; set; }

        [Required]
        [StringLength(150)]
        public string MemorendumNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime MemorendumDate { get; set; }

        public long EmployeeId { get; set; }

        [StringLength(100)]
        public string DispatchNo { get; set; }

        public int? PunishmentId { get; set; }

        public bool IsPunishmentRunning { get; set; }

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
