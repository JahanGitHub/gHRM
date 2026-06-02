using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Loan
{
    public class ApplicantInfoViewModel : BaseModel
    {
        public int Id { get; set; }
        [Display(Name = "Loan Type")]
        public string LoanType { get; set; }
        [Display(Name = "Loan Purpose")]
        public int PurposeId { get; set; }
        public long EmployeeId { get; set; }
        [Display(Name = "Emp. ID")]
        public string EmployeeCode { get; set; }
        [Display(Name = "Emp. name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Loan Amount")]
        public int LoanAmount { get; set; }
        [Display(Name = "No. of Installment")]
        public int InstallmentNo { get; set; } = 1;
        [Display(Name = "Interest Rate")]
        public decimal InterestRate { get; set; }
        [Display(Name = "Interest Amt.")]
        public decimal InterestAmount { get; set; }
        [Display(Name = "Ins. Principal")]
        public decimal InstallmentPrincipal { get; set; }
        [Display(Name = "Ins. Interest")]
        public decimal InstallmentInterest { get; set; }
        
        [Display(Name = "Installment")]
        public decimal InstallmentAmount { get; set; }
        [Display(Name = "Max. Amount")]
        public decimal MaxLoanAmount { get; set; }
        public string Remark { get; set; }
        public int? PreviousLoanID { get; set; }
        [Display(Name = "Pre. Loan no")]
        public string PreviousLoanNo { get; set; }
        [Display(Name = "Pre. Loan amount")]
        public int? PreviousLoanAmount { get; set; }
        [Display(Name = "Nominee Name")]
        public string NomineeName { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        public string Relation { get; set; }
        [Display(Name = "Type")]
        public string IdentificationType { get; set; }
        [StringLength(20, ErrorMessage = "Maximum length is {1}")]
        [Display(Name = "Identity No")]
        public string IdentificationNo { get; set; }
        public string MethodType { get; set; }
        [Display(Name = "Grace Period")]
        public int GracePeriod { get; set; }
        [StringLength(15, ErrorMessage = "Maximum length is {1}")]
        public string ContactNo { get; set; }
        public List<SelectListItem> LoanTypeLst { get; set; }
        public List<SelectListItem> PurposeLst { get; set; }
        public List<SelectListItem> GracePeriodLst { get; set; }
    }

    public class ApplicantInfoViewModel2 : BaseModel
    {
        public int Id { get; set; }
        [Display(Name = "Loan Type")]
        public string LoanType { get; set; }
        [Display(Name = "Loan Purpose")]
        public int PurposeId { get; set; }
        public long EmployeeId { get; set; }
        [Display(Name = "Emp. ID")]
        public string EmployeeCode { get; set; }
        [Display(Name = "Emp. name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Loan Amount")]
        public decimal LoanAmount { get; set; }
        [Display(Name = "No. of Installment")]
        public int InstallmentNo { get; set; } = 1;
        [Display(Name = "Interest Rate")]
        public decimal InterestRate { get; set; }
        [Display(Name = "Interest Amt.")]
        public decimal InterestAmount { get; set; }
        [Display(Name = "Ins. Principal")]
        public decimal InstallmentPrincipal { get; set; }
        [Display(Name = "Ins. Interest")]
        public decimal InstallmentInterest { get; set; }

        [Display(Name = "Installment")]
        public decimal InstallmentAmount { get; set; }
        [Display(Name = "Max. Amount")]
        public decimal MaxLoanAmount { get; set; }
        public string Remark { get; set; }
        public int? PreviousLoanID { get; set; }
        [Display(Name = "Pre. Loan no")]
        public string PreviousLoanNo { get; set; }
        [Display(Name = "Pre. Loan amount")]
        public decimal PreviousLoanAmount { get; set; }
        [Display(Name = "Nominee Name")]
        public string NomineeName { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        public string Relation { get; set; }
        [Display(Name = "Type")]
        public string IdentificationType { get; set; }
        [StringLength(20, ErrorMessage = "Maximum length is {1}")]
        [Display(Name = "Identity No")]
        public string IdentificationNo { get; set; }
        public string MethodType { get; set; }
        [Display(Name = "Grace Period")]
        public int GracePeriod { get; set; }
        [StringLength(15, ErrorMessage = "Maximum length is {1}")]
        public string ContactNo { get; set; }

        [Display(Name = "Application Date")]
        public DateTime DisburseDate { get; set; }

        public List<SelectListItem> LoanTypeLst { get; set; }
        public List<SelectListItem> PurposeLst { get; set; }
        public List<SelectListItem> GracePeriodLst { get; set; }
    }

    public class ApplicantInfoViewModel3 : BaseModel
    {
        public int Id { get; set; }
        [Display(Name = "Loan Type")]
        public string LoanType { get; set; }
        [Display(Name = "Loan Purpose")]
        public int PurposeId { get; set; }
        public long EmployeeId { get; set; }
        [Display(Name = "Emp. ID")]
        public string EmployeeCode { get; set; }
        [Display(Name = "Emp. name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Loan Amount")]
        public int LoanAmount { get; set; }
        [Display(Name = "No. of Installment")]
        public int InstallmentNo { get; set; } = 1;
        [Display(Name = "Interest Rate")]
        public decimal InterestRate { get; set; }
        [Display(Name = "Interest Amt.")]
        public decimal InterestAmount { get; set; }
        [Display(Name = "Ins. Principal")]
        public decimal InstallmentPrincipal { get; set; }
        [Display(Name = "Ins. Interest")]
        public decimal InstallmentInterest { get; set; }

        [Display(Name = "Installment")]
        public decimal InstallmentAmount { get; set; }
        [Display(Name = "Max. Amount")]
        public decimal MaxLoanAmount { get; set; }
        public string Remark { get; set; }
        public int? PreviousLoanID { get; set; }
        [Display(Name = "Pre. Loan no")]
        public string PreviousLoanNo { get; set; }
        [Display(Name = "Pre. Loan amount")]
        public int? PreviousLoanAmount { get; set; }
        [Display(Name = "Nominee Name")]
        public string NomineeName { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        public string Relation { get; set; }
        [Display(Name = "Type")]
        public string IdentificationType { get; set; }
        [StringLength(20, ErrorMessage = "Maximum length is {1}")]
        [Display(Name = "Identity No")]
        public string IdentificationNo { get; set; }
        public string MethodType { get; set; }
        [Display(Name = "Grace Period")]
        public int GracePeriod { get; set; }
        [StringLength(15, ErrorMessage = "Maximum length is {1}")]
        public string ContactNo { get; set; }

        public DateTime ApplicationDate { get; set; }

        public List<SelectListItem> LoanTypeLst { get; set; }
        public List<SelectListItem> PurposeLst { get; set; }
        public List<SelectListItem> GracePeriodLst { get; set; }
    }
}