namespace gHRM.Data.CodeFirstMigration.Discipline
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("disc.DiscCasePunishmentDetail")]
    public partial class DiscCasePunishmentDetail
    {
        [Key]
        public int PunishmentDetailId { get; set; }

        public int PunishmentMasterId { get; set; }

        public int CaseMasterId { get; set; }
        public int? CaseDetailId { get; set; }
        public int CrimeId { get; set; }

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
