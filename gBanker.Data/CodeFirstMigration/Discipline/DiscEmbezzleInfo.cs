namespace gHRM.Data.CodeFirstMigration.Discipline
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    [Table("disc.DiscEmbezzleInfo")]
    public partial class DiscEmbezzleInfo
    {
        [Key]
        public int EmbezzleId { get; set; }

        public int CaseMasterId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EmbezzleRcvDt { get; set; }

        public int? OfficeId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AuditDateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AuditDateTo { get; set; }

        public string BranchAuditNo { get; set; }

        public int? NoOfBMAccused { get; set; }

        public int? NoOfSignatoryAccussed { get; set; }

        public int? NoOfCMAccussed { get; set; }

        public decimal? TotEmbezzledAmount { get; set; }

        public decimal? TotReturnAmount { get; set; }

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
