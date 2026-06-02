using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("GradeXSalaryStep", Schema = "prl")]
    public class GradeXSalaryStep
    {
        [Key]
        public int Id { get; set; }
        public int GradeId { get; set; }
        public int StepFrom { get; set; }
        public int StepTo { get; set; }
        public string RatioOn { get; set; }
        public int AmountOrPercent { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
