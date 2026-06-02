using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.EmployeeLoanRegister")]
    public class EmployeeLoanRegister
    {
        [Key]
        public int Id { get; set; }
        public int LoanId { get; set; }
        public long EmployeeId { get; set; }
        public int PRComponentId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int NoOfInstallMent { get; set; }
        public int YearTotal { get; set; }
        public System.DateTime LoanStartDate { get; set; }
        public System.DateTime LoanClosingDate { get; set; }
        public string LoanType { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public decimal Capital { get; set; }
        public decimal TotalInterestAmount { get; set; }
        public Nullable<decimal> LoanOpening { get; set; }
        public bool IsExcelGenerated { get; set; }
    }
}
