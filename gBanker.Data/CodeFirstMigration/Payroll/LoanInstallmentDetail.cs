using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.LoanInstallmentDetail")]
    public class LoanInstallmentDetail
    {
        [Key]
        public int Id { get; set; }
        public long EmployeeId { get; set; }
        public int PRComponentId { get; set; }
        public decimal LoanDisburseAmount { get; set; }
        public DateTime DisburseDate { get; set; }
        public decimal InstallmentAmount { get; set; }
        public DateTime InstallmentStartDate { get; set; }
        public DateTime InstallmentEndDate { get; set; }
        public string LoanStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public long CreateBy { get; set; }
        public long UpdateBy { get; set; }
        
    }
}
