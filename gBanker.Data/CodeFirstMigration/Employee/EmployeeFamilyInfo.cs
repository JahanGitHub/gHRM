using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeFamilyInfo")]
    public partial class EmployeeFamilyInfo
    {
        [Key]
        public long FamilyInfoId { get; set; }

        public long EmployeeId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string Relation { get; set; }

        [Required]
        [StringLength(7)]
        public string Gender { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(50)]
        public string Occupation { get; set; }
        public string EducationalQualification { get; set; }
        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public virtual Employee Employee { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
        public int? ApprovedOrRejectedBy { get; set; }
        public DateTime? ApprovalOrRejectDate { get; set; }

        [NotMapped]
        public int FamilyInfoType { get; set; }
    }
}
