using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Loan
{
    public class EmployeeLoanInstallmentDetailViewModel
    {
        public int LoanDetailId { get; set; }
        public int LoanId { get; set; }

        [Display(Name = "Installment Date")]
        public System.DateTime InstallmentDate { get; set; }

        [Display(Name = "Installment Amount")]
        public decimal InstallmentAmount { get; set; }

        [Display(Name = "Is Installment Paid")]
        public bool IsInstallmentPaid { get; set; }

        [Display(Name = "Employee")]
        public long EmployeeId { get; set; }

        [Display(Name ="Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Office")]
        public string OfficeName { get; set; }

        [Display(Name = "Department")]
        public string DepartmentName { get; set; }

        [Display(Name = "Designation")]
        public string DesignationName { get; set; }

        [Display(Name = "Component")]
        public int PRComponentId { get; set; }

        [Display(Name = "Ending Balance")]
        public decimal EndingBalance { get; set; }

        [Display(Name = "Principal Amount")]
        public decimal PrincipalAmount { get; set; }

        [Display(Name = "Interest Amount")]
        public decimal InterestAmount { get; set; }

        [Display(Name = "Interest Charge")]
        public decimal InterestCharge { get; set; }

        [Display(Name = "Total Loan Amount")]
        public decimal TotalLoanAmt { get; set; }

        [Display(Name = "Loan Opening Amount")]
        public decimal LoanOpeningAmt { get; set; }

        [Display(Name = "No. of Installment")]
        public int NoOfInstallMent { get; set; }

        [Display(Name = "Rest No. of Installment")]
        public int RestNoOfInstallMent { get; set; }

        [Display(Name = "Year Total")]
        public int YearTotal { get; set; }

        [Display(Name = "Loan Start Date")]
        public DateTime LoanStartDate { get; set; }

        [Display(Name = "Loan End Date")]
        public DateTime LoanEndDate { get; set; }

        [Display(Name = "Installment Interval")]
        public int InstallmentInterval { get; set; }
        public IEnumerable<SelectListItem> InstallmentIntervalList { get; set; }

        [Display(Name = "Loan Type")]
        public int LoanTypeId { get; set; }
        public IEnumerable<SelectListItem> LoanTypeList { get; set; }

        [Display(Name = "Loan Type")]
        public string LoanType { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Loan Opening")]
        public decimal LoanOpening { get; set; }

        [Display(Name = "Interest Rate")]
        public decimal InterestRate { get; set; }

        [Display(Name = "Loan Scheme")]
        public string LoanScheme { get; set; }
        public IEnumerable<SelectListItem> LoanSchemeList { get; set; }

        public IEnumerable<SelectListItem> LoanComponentList { get; set; }
        public string rowSl { get; set; }

        [Display(Name = "Loan Start Date")]
        public string LoanStartDateMsg { get; set; }

        [Display(Name = "Installment Date")]
        public string InstallmentDateMsg { get; set; }

        [Display(Name = "Loan End Date")]
        public string LoanEndDateMsg { get; set; }

        [Display(Name = "Loan End Date")]
        public string LoanStatus { get; set; }

        public string ApprovedStatus { get; set; }
    }
}


namespace gHRM.Web.ViewModels.Loan
{
    public class EmployeeSalaryIncrementViewModel
    {
        public long Id { get; set; } // If your SP returns an ID
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; } // If returned
        public decimal IncrementAmount { get; set; }
        public string IncrementAmountDate { get; set; }
        public string OfficeName { get; set; } // Only if you have this

        public string DesignationName { get; set; }

        public string DepartmentName { get; set; }

        public decimal GrossSalary { get; set; }
        public decimal CurrentSalary { get;  set; }
        public decimal TotalIncrement { get;  set; }
    }

}