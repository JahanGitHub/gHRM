using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.EmployeeSalaryConfigurationHistory")]
    public class EmployeeSalaryConfigurationHistory
    {
        [Key]
        public int Id { get; set; }
        public long EmployeeId { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime EffectiveDateTo { get; set; }
        //public double BasicSalary { get; set; }//
        public double GrossSalary { get; set; }
        public double TotalSalary { get; set; }
        public string BankAccount { get; set; }
        public bool? IsOvertime { get; set; }
        public decimal? MaxOvertimePerMonth { get; set; }
        //public decimal? OvertimehourPerMonth { get; set; }
        public decimal? OvertimeRate { get; set; }
        public int? PFTypeId { get; set; }
        public bool IsActive { get; set; }
        public long CreateBy { get; set; }
        public long UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
