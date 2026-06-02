namespace gHRM.Data.CodeFirstMigration.Discipline
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("disc.DiscCaseDetail")]
    public partial class DiscCaseDetail
    {
        [Key]
        public int CaseDetailsId { get; set; }

        public int CaseMasterId { get; set; }

        public long? AnnexationId { get; set; }

        public long EmployeeId { get; set; }

        public int CrimeId { get; set; }
        public DateTime? CrimeDateFrom { get; set; }
        public DateTime? CrimeDateTo { get; set; }

        public decimal? AnnexationAmount { get; set; }
        public decimal? ReturnAmount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReturnNoticeDate { get; set; }
        public int? PunishmentId { get; set; }
        public DateTime? PunishmentDt { get; set; }

        [StringLength(500)]
        public string DispatchNo { get; set; }

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



    }// End Class
}// ENd Namespace
