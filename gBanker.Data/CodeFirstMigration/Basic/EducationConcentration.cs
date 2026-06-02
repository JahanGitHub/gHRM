using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EducationConcentration")]
    public class EducationConcentration
    {
        [Key]
        public int ConcentrationId { get; set; }
        public string DegreeCode { get; set; }

        public string ConcentrationCode { get; set; }
        public string ConcentrationName { get; set; }
        public int CompanyId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public long? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
