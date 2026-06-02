using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.EmployeeMonthlySalaryApproved")]
    public class EmployeeMonthlySalaryApproved
    {
        [Key]
        public int Id { get; set; }

        public int SalaryId { get; set; }

        public int SalaryMonth { get; set; }

        public int SalaryYear { get; set; }

        public System.DateTime SalaryDate { get; set; }

        public long EmployeeId { get; set; }

        public int? OfficeId { get; set; }

        public int? OfficeTypeId { get; set; }

        public int? DesignationId { get; set; }

        public int? DepartmentId { get; set; }

        //public string EmployeeStatus { get; set; }
        public int EmployeeStatusId { get; set; }

        public string BankCode { get; set; }

        public string TransactionType { get; set; }

        public decimal PRComponentAmount { get; set; }

        public int PRComponentId { get; set; }

        public Nullable<long> PRSalaryConfigurationId { get; set; }

        public bool IsActive { get; set; }

        public bool IsApproved { get; set; }

        public long CreatedBy { get; set; }

        public long UpdatedBy { get; set; }

        public System.DateTime CreateDate { get; set; }

        public System.DateTime UpdateDate { get; set; }

        public string ComponentCategory { get; set; }

        // public bool IsRejected { get; set; }
        // public bool IsSendForApproval { get; set; }
    }
}
