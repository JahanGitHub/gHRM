namespace gHRM.Data.CodeFirstMigration.Discipline
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("disc.DiscCasePunishmentMaster")]
    public partial class DiscCasePunishmentMaster
    {
        [Key]
        public int PunishmentMasterId { get; set; }

        public long EmployeeId { get; set; }

        public int PunishmentId { get; set; }

        public DateTime? PunishmentDate { get; set; }

        public DateTime? ActivatedDt { get; set; }
        public DateTime? FirstIncSuspendDt { get; set; }
        public DateTime? SecondIncSuspendDt { get; set; }
        public DateTime? ThirdIncSuspendDt { get; set; }
        public DateTime? FourthIncSuspendDt { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }
        public int? DaysLose { get; set; }
        public string DespatchNo { get; set; }
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
