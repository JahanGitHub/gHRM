namespace gHRM.Data.CodeFirstMigration.Discipline
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("disc.DiscCrime")]
    public partial class DiscCrime
    {
        [Key]
        public int CrimeId { get; set; }
        public int? CrimeType { get; set; }

        [StringLength(50)]
        public string CrimeCode { get; set; }

        [Required]
        [StringLength(800)]
        public string CrimeName { get; set; }
        public int? SortOrder { get; set; }

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
