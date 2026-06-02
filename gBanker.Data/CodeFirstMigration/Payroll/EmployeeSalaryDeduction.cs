using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.EmployeeSalaryDeduction")]
    public class EmployeeSalaryDeduction
    {
        [Key]
        public int Id { get; set; }
        public long EmployeeId { get; set; }
        public int ComponentId { get; set; }
        public int? ProductId { get; set; }
        public int? SerialId { get; set; }
        public decimal DeductedAmount { get; set; }
        public int DeductionDays { get; set; }
        //public int SalaryMonth { get; set; }
        //public int SalaryYear { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Remark { get; set; }

    }
}
