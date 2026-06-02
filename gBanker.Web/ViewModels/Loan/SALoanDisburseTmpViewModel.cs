using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Loan
{
    public class SALoanDisburseTmpViewModel
    {
        public long SALoanDisburseID { get; set; }
        public int OfficeID { get; set; }
        public long EmployeeID { get; set; }
        public int PRComponentID { get; set; }
        public int LoanTerm { get; set; }
        public int DisburseType { get; set; }
        public int PaymentMethod { get; set; }
        public string DisburseDate { get; set; }
        public string InstallmentStartDate { get; set; }
        public decimal DisburseAmount { get; set; }
        public decimal LoanAmount { get; set; }
        public int LoanDuration { get; set; }
        public decimal InterestRate { get; set; }
        public decimal LoanInstallment { get; set; }
        public decimal LoanInstallmentNext { get; set; }
        public decimal InterestInstallment { get; set; }
        public decimal InterestInstallmentNext { get; set; }
        public decimal LoanInsurance { get; set; }
        public decimal PreviousInterest { get; set; }
        public decimal CurrentInterest { get; set; }
        public decimal LoanPaid { get; set; }
        public decimal PreviousInterestPaid { get; set; }
        public decimal CurrentInterestPaid { get; set; }
        public string InstallmentDate { get; set; }
        public string InterestCalculationDate { get; set; }
        public string LoanClosingDate { get; set; }
        public int LoanStatus { get; set; }
    }
}