using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration.Loan
{
    [Table("loan.ApplicantNominee")]
    public class ApplicantNominee
    {
        [Key]
        public int NomineeId { get; set; }
        public int ApplicantId { get; set; }
        [Required]
        public string NomineeName { get; set; }
        public string Address { get; set; }
        [Required]
        public string Relation { get; set; }
        [Required]
        public string IdentificationType { get; set; }
        public string IdentificationNo { get; set; }
        public string ContactNo { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }
    }
}
