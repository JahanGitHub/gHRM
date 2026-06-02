using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeReportOptionJCF")]
    public class EmployeeReportOptionJCF
    {
        [Key]
        public int Id { get; set; }
        public int EmpReportTypeId { get; set; }
        public string EmpReportTypeName { get; set; }
        public int DisplaySL { get; set; }
        public bool IsActive { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
