using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.EmployeeLoanInstallmentDetail")]
    public class EmployeeLoanInstallmentDetail
    {
        [Key]
        public int LoanDetailId { get; set; }
        public int LoanId { get; set; }
        public System.DateTime InstallmentDate { get; set; }
        public decimal InstallmentAmount { get; set; }
        public bool IsActive { get; set; }
        public bool IsInstallmentPaid { get; set; }
        public long EmployeeId { get; set; }
        public int PRComponentId { get; set; }
        public decimal EndingBalance { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestAmount { get; set; }
        //public decimal InterestCharge { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public string ApprovalStatus { get; set; }  

    }
}
