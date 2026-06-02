using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.SalaryGenerationLog")]
    public class SalaryGenerationLog
    {
        [Key]
        public int ID { get; set; }
        public int SalaryYear { get; set; }
        public int SalaryMonth { get; set; }
        public int OfficeTypeId { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsSendForApproval { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }       
        public long CreateBy { get; set; }
        public long UpdateBy { get; set; }
    }
}
