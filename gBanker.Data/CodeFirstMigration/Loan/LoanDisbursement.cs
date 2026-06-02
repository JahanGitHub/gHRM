using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration.Loan
{
    [Table("loan.LoanDisbursement")]
    public class LoanDisbursement
    {
        [Key]
        public int LoanId { get; set; }
        public string LoanType { get; set; }
        public int PurposeId { get; set; }
        public int ApplicantId { get; set; }
        public long EmployeeId { get; set; }
        public string MethodType { get; set; }
        public int GracePeriod { get; set; }
        public string LoanNo { get; set;  }
        public DateTime DisburseDate { get; set; }
        public int DisburseAmount { get; set; }
        public decimal IntersetRate { get; set; }
        public decimal InterestCharge { get; set; }
        public int NoOfInstallment { get; set; }
        public decimal InstallmentPrincipal { get; set; }
        public decimal InstallmentInterest { get; set; }
        public decimal MonthlyInstallment { get; set; }
       
        public decimal LoanPaid { get; set; }
        public decimal InterestPaid { get; set; }
        
        public DateTime LastInstallmentDate { get; set; }
        public DateTime? PaidOffDate { get; set; }
        public bool IsInstallmentOver { get; set; }
        public bool IsClose { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }
        public bool? IsDeleted { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? DeleteDate { get; set; }
        public int? DeletedBy { get; set; }
    }
}
