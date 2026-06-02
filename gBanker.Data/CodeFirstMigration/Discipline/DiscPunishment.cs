namespace gHRM.Data.CodeFirstMigration.Discipline
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("disc.DiscPunishment")]
    public partial class DiscPunishment
    {
        [Key]
        public int PunishmentId { get; set; }
        public int? PunishmentType { get; set; }

        [StringLength(50)]
        public string PunishmentCode { get; set; }

        [Required]
        [StringLength(500)]
        public string PunishmentName { get; set; }

        public int? SeniorityLossDay { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

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
