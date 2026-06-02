using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.PF
{
    public class LoanCollectionViewModel : PFBaseModel
    {
        public string LoanId { get; set; }

        [Display(Name = "Collection Id")]
        public string CollectionId { get; set; }

        [Display(Name = "Employee Id")]
        public string EmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        public IEnumerable<SelectListItem> EmployeeConfigList { get; set; }

        public int CollectionTypeId { get; set; }

        [Display(Name = "Collection Type")]
        public string CollectionType { get; set; }

        [Display(Name = "Vouche rNo")]
        public long VoucherNo { get; set; }

        [Display(Name = "Loan Amount")]
        public string LoanAmount { get; set; }

        [Display(Name = "Interest Amount")]
        public string InterestAmount { get; set; }

        [Display(Name = "Total Installment")]
        public string TotalInstallment { get; set; }

        [Display(Name = "Transaction Type")]
        public string TransactionType { get; set; }

        [Display(Name = "Sundry")]
        public string Sundry { get; set; }

        [StringLength(200)]
        [Display(Name = "Comment")]
        public string Comment { get; set; }

        public IEnumerable<SelectListItem> TransactionTypeList { get; set; }

        [DataType("smalldatetime")]
        public IEnumerable<SelectListItem> TransactionCatList { get; set; }

        //Newly Added

        [Display(Name = "Loan Type")]
        public int LoanTypeId { get; set; }
        
        [Display(Name = "Report Type")]
        public string ReportType { get; set; }
        public IEnumerable<SelectListItem> ReportTypeList { get; set; }

        public IEnumerable<SelectListItem> LoanTypeList { get; set; }

        [Display(Name = "Amount")]
        public string Amount { get; set; }

        public string AmountOld { get; set; }

        [Display(Name = "Principal Installment")]
        public string LoanInstallment { get; set; }

        [Display(Name = "Interest Installment")]
        public string InterestInstallment { get; set; }

        [Display(Name = "Interest Charge")]
        public string InterestCharge { get; set; }

        [Display(Name = "Current Interest")]
        public string CurrentInterestCharge { get; set; }

        [Display(Name = "Total Interest Charge")]
        public string TotalInterestCharge { get; set; }

        [Display(Name = "Disburse Amount")]
        public string DisburseAmount { get; set; }

        [Display(Name = "Interset Amount")]
        public string IntersetAmount { get; set; }

        [Display(Name = "Principal Paid")]
        public string LoanPaid { get; set; }

        [Display(Name = "Interest Paid")]
        public string InterestPaid { get; set; }

        //Newly Added
        [Display(Name = "Disburse Date")]
        public string DisburseDate { get; set; }

        [Display(Name = "Interest Rate")]
        public string IntersetRate { get; set; }

        [Display(Name = "No Of Installment")]
        public string NoOfInstallment { get; set; }

        [Display(Name = "Monthly Installment")]
        public string MonthlyInstallment { get; set; }

        [Display(Name = "Principal Due")]
        public string PrincipalDue { get; set; }

        [Display(Name = "Interest Due")]
        public string InterestDue { get; set; }

        [Display(Name = "Total Due")]
        public string TotalDue { get; set; }

        [Display(Name = "From Date")]
        public string FromDate { get; set; }

        [Display(Name = "To Date")]
        public string ToDate { get; set; }

        [Display(Name = "Loan")]
        public string LoanList { get; set; }

        public string LoanStatus { get; set; }

        [Display(Name = "Interest Upto On")]
        public string InterestUptoOn { get; set; }

        [Display(Name = "Interest Upto")]
        public string InterestUpto { get; set; }       

        [Display(Name = "Total Due Amount Upto")]
        public string TotalDueAmountUpto { get; set; }

        public decimal TodaysLoanCollectionAmount { get; set; }

        public decimal TodaysInterestCollectionAmount { get; set; }


        public int TotalInstallmentNo { get; set; }

        public int TotalComplete { get; set; }

        public int TotalNoDue { get; set; }

        public string CurrentStatus { get; set; }
    }
}