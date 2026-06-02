using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeOtherQualification")]
    public partial class EmployeeOtherQualification
    {
        [Key]
        public int QualificationId { get; set; }

        public long EmployeeId { get; set; }
        public string Language { get; set; }
        public string FluencyLevel { get; set; }
        //public string EducationalQualification { get; set; }
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
