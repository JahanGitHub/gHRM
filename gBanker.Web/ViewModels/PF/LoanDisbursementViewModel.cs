using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.PF
{
    public class LoanDisbursementViewModel : PFBaseModel
    {
        public long LoanId { get; set; }

        [Display(Name = "Loan Term")]
        public int LoanTerm { get; set; }

        [Display(Name = "Disburse Date")]
        public string DisburseDate { get; set; }

        [Display(Name = "Loan Paid")]
        public string LoanPaid { get; set; }

        [Display(Name = "Interest Paid")]
        public string InterestPaid { get; set; }

        [Display(Name = "Interest Charge")]
        public string InterestCharge { get; set; }

        [Display(Name = "Last Transaction Date")]
        public string LastInstallmentDate { get; set; }

        [Display(Name = "Is Installment Over")]
        public bool IsInstallmentOver { get; set; }

        [Display(Name = "Paid Off Loan")]
        public string FinishedLoan { get; set; }

        [Display(Name = "Employee Id")]
        public string EmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Loan Type Id")]
        public int LoanTypeId { get; set; }

        [Display(Name = "Disburse Amount")]
        public string DisburseAmount { get; set; }
        [Display(Name = "Interest Rate")]
        public string IntersetRate { get; set; }
        [Display(Name = "No Of Installment")]
        public string NoOfInstallment { get; set; }
        [Display(Name = "Monthly Installment")]
        public string MonthlyInstallment { get; set; }
        [Display(Name = "Paid Off Date")]
        public string PaidOffDate { get; set; }
        [Display(Name = "Max Loan Limit")]
        public string MaxLoanLimit { get; set; }
        [Display(Name = "Loan Type")]
        public IEnumerable<SelectListItem> LoanTypeList { get; set; }

        //additional
        [Display(Name = "Amount")]
        public string Amount { get; set; }

        public decimal TodaysPrinCollBeforeDayEnd { get; set; }

        public decimal TodaysIntCollBeforeDayEnd { get; set; }

        public decimal LoanInstallment { get; set; }

        public decimal InterestInstallment { get; set; }

        public decimal CurrentInterestCharge { get; set; }

        public decimal TotalInterestCharge { get; set; }

        public decimal TotalReceivable { get; set; }

        public string Message { get; set; }

        [Display(Name ="From Date")]
        public string FromDate { get; set; }

        [Display(Name = "To Date")]
        public string ToDate { get; set; }

        [Display(Name = "Loan State")]
        public int LoanState { get; set; }
        
        public int TotalCount { get; set; }
    }
}