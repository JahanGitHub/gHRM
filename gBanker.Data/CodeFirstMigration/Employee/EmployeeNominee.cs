using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeNominee")]
    public partial class EmployeeNominee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NomineeId { get; set; }

        [Required]
        public int NomineeTypeId { get; set; }

        [Required]
        public long EmployeeId { get; set; }

        [StringLength(250)]
        public string NomineeName { get; set; }

        [StringLength(500)]
        public string NomineeAddress { get; set; }

        public int? NomineeAge { get; set; }

        public int NomineeRelationId { get; set; }

        public decimal? NomineePercentage { get; set; }

        [StringLength(50)]
        public string NomineeNationalId { get; set; }
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }
        public string BirthCertificateNo { get; set; }

        [StringLength(250)]
        public string NomineeRemarks { get; set; }

        public byte[] NomineeImage { get; set; }

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
