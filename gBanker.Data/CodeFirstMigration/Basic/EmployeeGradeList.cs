using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeGradeList")]
    public class EmployeeGradeList
    {
        [Key]
        public int Id { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public string GradeDescription { get; set; }
        public decimal InitialAmount { get; set; }
        public decimal AmountPerIncrement { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime EffectiveDateTo { get; set; }
        public string RatioOn { get; set; }
        public decimal Percentage { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
       // public string ComponentName { get; set; }

    }
}
