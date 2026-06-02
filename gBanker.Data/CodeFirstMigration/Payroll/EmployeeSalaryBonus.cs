using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.EmployeeSalaryBonus")]
    public class EmployeeSalaryBonus
    {
        [Key]
        public int ESBonusId { get; set; }
        public long EmployeeId { get; set; }
        public int OfficeId { get; set; }
        public int OfficeTypeId { get; set; }
        public int DesignationId { get; set; }
        public int DepartmentId { get; set; }
        public string BankCode { get; set; }
        public int RevStampDeduction { get; set; }
        public int EmployeeStatusId { get; set; }
        public int ComponentId { get; set; }
        public double BonusAmount { get; set; }
        public int SalaryYear { get; set; }
        public int SalaryMonth { get; set; }
        public DateTime BonusProcessingDate { get; set; }
        public int IsActive { get; set; }
        public int IsSendForApproval { get; set; }
        public int IsApproved { get; set; }
        public int IsRejected { get; set; }

        public long CreateBy { get; set; }
        public long UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
