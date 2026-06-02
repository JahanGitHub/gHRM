using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.ComponentPayroll")]
    public class ComponentPayroll
    {
        [Key]
        public int Id { get; set; }
        public string ComponentName { get; set; }
        public string ComponentCategory { get; set; }
        public bool? IsChangeable { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? CreateBy { get; set; }

        public long? UpdateBy { get; set; }

    }
}
