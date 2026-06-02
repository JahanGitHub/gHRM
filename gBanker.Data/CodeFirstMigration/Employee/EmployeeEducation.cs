using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeEducation")]
    public partial class EmployeeEducation
    {

         
        [Key]
        public long EducationId { get; set; }

        public long EmployeeId { get; set; }

        [Required]
        [StringLength(250)]
        public string DegreeTitle { get; set; }

        [StringLength(450)]
        public string Concentration { get; set; }

        [Required]
        [StringLength(500)]
        public string InstitutionName { get; set; }

       // [Required]
        [StringLength(50)]
        public string PassingYear { get; set; }

        [Required]
        [StringLength(50)]
        public string ResultType { get; set; }

        [StringLength(50)]
        public string Division { get; set; }

        [StringLength(11)]
        public string MarksPercentage { get; set; }

        [StringLength(10)]
        public string CGPA { get; set; }

        [StringLength(10)]
        public string CGPAScale { get; set; }

        [StringLength(20)]
        public string Duration { get; set; }

        [StringLength(500)]
        public string Acheivements { get; set; }

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

        
    }
}
