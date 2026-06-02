using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.EmployeeMonthlySalary")]
    public class EmployeeMonthlySalary
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]//
        public int SalaryId { get; set; }
        public int SalaryMonth { get; set; }
        public int SalaryYear { get; set; }
        public System.DateTime SalaryDate { get; set; }
        public long EmployeeId { get; set; }
        public Nullable<long> PRSalaryConfigurationId { get; set; }
        public int PRComponentId { get; set; }
        public decimal PRComponentAmount { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public bool IsSendForApproval { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string ComponentCategory { get; set; }
        public string TransactionType { get; set; }
        public bool IsRejected { get; set; }
        public int? OfficeId { get; set; }
        public int? loanId { get; set; }
    }
}
