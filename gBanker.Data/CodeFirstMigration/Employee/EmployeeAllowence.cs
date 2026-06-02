using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeAllowence",Schema = "prl")]
    public partial class EmployeeAllowence
    {
        [Key]       
        public int Id { get; set; }
        public int GradeId { get; set; }
        //public int EmployeeTypeId { get; set; }
        public int EmployeeStatusId { get; set; }
        public int ComponentId { get; set; }
        public string RatioOn { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be numeric")]
        public decimal Allowance { get; set; }
        
        public bool IsActive { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
