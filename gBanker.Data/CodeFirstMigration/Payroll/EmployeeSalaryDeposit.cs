using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Payroll
{
    [Table("prl.EmployeeSalaryDeposit")]
    public class EmployeeSalaryDeposit
    {
        [Key]
        public int Id { get; set; }
        public int PRComponentId { get; set; }
        public int DepositComponentId { get; set; }
        public int PRComponentRefundId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string TransactionType { get; set; }
        public string ComponentGroup { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal DepositOnGrossSalary { get; set; }
        public int NoOfSalaryDays { get; set; }
        public bool DepositDone { get; set; }
        public bool RefundDone { get; set; }
        public DateTime? DepositDate { get; set; }
        public DateTime? RefundDate { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public DateTime? RefundStartDate { get; set; }
        public DateTime? RefundEndDate { get; set; }
        public bool? IsDepositRequired { get; set; }
        public bool? IsRefundRequired { get; set; }
        public bool IsActive { get; set; }
        public long? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
