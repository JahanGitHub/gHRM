using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Loan
{
    public class PRLoanDisburseViewModel : BaseModel
    {
        public long SALoanDisburseID { get; set; }
        public int? OfficeID { get; set; }
        public long EmployeeID { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        public int PRComponentID { get; set; }
        [Display(Name = "Component Name")]
        public string ComponentName { get; set; }
        public int LoanTerm { get; set; }
        [Display(Name = "Disburse Type")]
        public int DisburseType { get; set; }
        [Display(Name = "Payment Method")]
        public int PaymentMethod { get; set; }
        [Display(Name = "Loan Amount")]
        public decimal LoanAmount { get; set; }
        //[Display(Name = "Approved Date")]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //public string ApprovedDate { get; set; }
        [Display(Name = "Disburse Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string DisburseDate { get; set; }
        [Display(Name = "Installment Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string InstallmentStartDate { get; set; }
        [Display(Name = "Disburse Amount")]
        public decimal DisburseAmount { get; set; }
        [Display(Name = "Principal Amount")]
        public decimal PrincipalAmount { get; set; }
        [Display(Name = "Loan Duration")]
        public int LoanDuration { get; set; }
        [Display(Name = "Interest Rate")]
        public Nullable<decimal> InterestRate { get; set; }
        [Display(Name = "Loan Installment")]
        public decimal LoanInstallment { get; set; }
        [Display(Name = "Loan Installment Next")]
        public decimal LoanInstallmentNext { get; set; }
        [Display(Name = "Interest Installment")]
        public decimal InterestInstallment { get; set; }
        [Display(Name = "Interest Installment Next")]
        public decimal InterestInstallmentNext { get; set; }
        [Display(Name = "Loan Insurance")]
        public decimal LoanInsurance { get; set; }
        [Display(Name = "Previous Interest")]
        public decimal PreviousInterest { get; set; }
        [Display(Name = "Current Interest")]
        public decimal CurrentInterest { get; set; }
        [Display(Name = "Loan Paid")]
        public decimal LoanPaid { get; set; }
        [Display(Name = "Previous Interest Paid")]
        public decimal PreviousInterestPaid { get; set; }
        [Display(Name = "Current Interest Paid")]
        public decimal CurrentInterestPaid { get; set; }
        [Display(Name = "Installment Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string InstallmentDate { get; set; }
        [Display(Name = "Interest Calculation Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string InterestCalculationDate { get; set; }
        [Display(Name = "Charge Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string LastChargeDate { get; set; }
        [Display(Name = "Loan Closing Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string LoanClosingDate { get; set; }
        [Display(Name = "Loan Status")]
        public int LoanStatus { get; set; }

        [Display(Name = "Payment Amount")]
        public decimal PaymentAmount { get; set; }


        public Nullable<int> PRWorkAreaID { get; set; }

        public long PRSalaryRegisterID { get; set; }
        [Display(Name = "Salary Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string SalaryDate { get; set; }
        [Display(Name = "Salary Year")]
        public int SalaryYear { get; set; }
        [Display(Name = "Salary Month")]
        public int SalaryMonth { get; set; }
        [Display(Name = "Component Amount")]
        public decimal ComponentAmount { get; set; }
        public long rowSl { get; set; }
        public int EmployeeStatusId { get; set; }
        public List<SelectListItem> YearList { get; set; }
        public List<SelectListItem> MonthList { get; set; }
        public List<SelectListItem> ComponentList { get; set; }

    } // End of Class 
} // End of Namespace