using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration.Loan
{
    [Table("loan.ApplicantInfo")]
    public class ApplicantInfo
    {
        [Key]
        public int Id { get; set; }
        public string LoanType { get; set; }
        public int PurposeId { get; set; }
        public long EmployeeId { get; set; }
        public int LoanAmount { get; set; }
        public int InstallmentNo { get; set; }
        public int GracePeriod { get; set; }
        public decimal InterestRate { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal InstallmentPrincipal { get; set; }
        public decimal InstallmentInterest { get; set; }
        public decimal InstallmentAmount { get; set; }
        public string Remark { get; set; }
        public int? PreviousLoanID { get; set; }
        public int? PreviousLoanAmount { get; set; }
        public int LevelPosition { get; set; }
        public string NotificationStatus { get; set; }// Accounts=ACC, Disbursement approval=DAP
        public string ApplicationStatus { get; set; } // Active,Delete,Reject,Disburse
        public DateTime? CreateDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }

    }

    [Table("loan.ApplicantInfo")]
    public class ApplicantInfo2
    {
        [Key]
        public int Id { get; set; }
        public string LoanType { get; set; }
        public int PurposeId { get; set; }
        public long EmployeeId { get; set; }
        public int LoanAmount { get; set; }
        public int InstallmentNo { get; set; }
        public int GracePeriod { get; set; }
        public decimal InterestRate { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal InstallmentPrincipal { get; set; }
        public decimal InstallmentInterest { get; set; }
        public decimal InstallmentAmount { get; set; }
        public string Remark { get; set; }
        public int? PreviousLoanID { get; set; }
        public int? PreviousLoanAmount { get; set; }
        public int LevelPosition { get; set; }
        public string NotificationStatus { get; set; }// Accounts=ACC, Disbursement approval=DAP
        public string ApplicationStatus { get; set; } // Active,Delete,Reject,Disburse
        public DateTime? CreateDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }

        public DateTime? ApplicationDate { get; set; }
    }
}
