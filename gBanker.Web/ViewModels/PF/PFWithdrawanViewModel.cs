using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.PF
{
    public class PFWithdrawanViewModel: PFBaseModel
    {
        public string WithdrawanId { get; set; }
        [Display(Name="Employee Id")]
        public string EmployeeId { get; set; }
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        
        [Display(Name = "Withdrawan Amount")]
        public string WithdrawanAmount { get; set; }
        [Display(Name = "Total PF Payable")]
        public string TotalPayable { get; set; }

        //[Display(Name = "Self Contribution Payable")]
        //public string SelfContribution { get; set; }
        //[Display(Name = "Org Contribution Payable")]
        //public string OrgContribution { get; set; }

        [Display(Name = "Interest")]
        public string InterestAmount { get; set; }
        [Display(Name = "Self Interest Payable")]
        public string SelfInterestAmount { get; set; }
        [Display(Name = "Org Interest Payable")]
        public string OrgInterestAmount { get; set; }
        [Display(Name = "Loan Receivable")]
        public string LoanDue { get; set; }
        [Display(Name = "Withdrawn Date")]
        public string WithdrawnDate { get; set; }

        //New Fields for View

        [Display(Name = "Self Contribution")]
        public decimal SelfContribution { get; set; }
        [Display(Name = "Org. Contribution")]
        public decimal OrgContribution { get; set; }
        [Display(Name = "Contribution")]
        public decimal Contribution { get; set; }
        [Display(Name = "Self Int. Upto Interim")]
        public decimal SelfInterestUptoInterim { get; set; }
        [Display(Name = "Org. Int. Upto Interim")]
        public decimal OrgInterestUptoInterim { get; set; }
        [Display(Name = "WithdrawnDate")]
        public decimal InterestUptoInterim { get; set; }
        [Display(Name = "Self. Int. After. Interim")]

        public decimal SelfInterestAftInterim { get; set; }
        [Display(Name = "Org. Int. After Interim")]
        public decimal OrgInterestAftInterim { get; set; }
        [Display(Name = "Int. After Interim")]
        public decimal InterestAftInterim { get; set; }

        public long LoanId { get; set; }
        [Display(Name = "Principal Balance")]
        public decimal PrincipalBalance { get; set; }
        [Display(Name = "Interest Balance")]
        public decimal InterestBalance { get; set; }
        [Display(Name = "Interest Income")]
         public decimal InterestIncome { get; set; }
        [Display(Name = "Fund")]
        public decimal Fund { get; set; }
        [Display(Name = "Loan Out Standing")]
        public decimal OutStanding { get; set; }
        [Display(Name = "Payable")]
        public decimal Payable { get; set; }
        [Display(Name = "Calculation Date")]
        public string CalculationDate { get; set; }
        public int TotalCount { get; set; }
    }
}