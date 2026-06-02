namespace gHRM.Data.CodeFirstMigration.Discipline
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("disc.DiscCaseAnnexation")]
    public partial class DiscCaseAnnexation
    {

        [Key]
        public long AnnexationId { get; set; }

        public int CaseMasterId { get; set; }

        public int CrimeId { get; set; }

        public decimal? TotAnnexationAmount { get; set; }
        public decimal? TotReturnAmount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReturnNoticeDate { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }




    }// END Class
}// ENd Namespace
