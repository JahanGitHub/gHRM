using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EducationDegree")]
    public class EducationDegree
    {
        [Key]
        public int DegreeId { get; set; }
        public int DegreeLevelId { get; set; }
        public string DegreeLevel { get; set; }
        public string DegreeCode { get; set; }
        public string DegreeName { get; set; }
        public int CompanyId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public long? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
