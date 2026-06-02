using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.PF
{
    public class ContributionCollectionViewModel: PFBaseModel
    {
        [Display(Name = "Collection Id")]
        public string CollectionId { get; set; }
        public long LoanId { get; set; }
        public int LoanTerm { get; set; }
        [Display(Name = "Employee Id")]
        public string EmployeeId { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        
        public IEnumerable<SelectListItem> EmployeeConfigList { get; set; }
        [Display(Name = "Collection Type Id")]
        public int CollectionTypeId { get; set; }
        [Display(Name = "Collection Type")]
        public string CollectionType { get; set; }
        [Display(Name = "Voucher No")]
        public long VoucherNo { get; set; }
        [Display(Name = "Self Contribution")]
        public string SelfContribution { get; set; }
        [Display(Name = "Org. Contribution")]
        public string OrgContribution { get; set; }

        public string LoanAmount { get; set; }
        public string InterestAmount { get; set; }
        public string InterestCharge { get; set; }
        public string Sundry { get; set; }

        [Display(Name = "Transaction Type")]
        public string TransactionType { get; set; }
        public IEnumerable<SelectListItem> TransactionTypeList { get; set; }
        [DataType("smalldatetime")]
        [Display(Name = "Transaction Date")]
        public string TransactionDateString { get; set; }
        [StringLength(200)]
        [Display(Name = "Comment")]
        public string Comment { get; set; }
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

         [Display(Name = "Collectiuon Type")]
        public IEnumerable<SelectListItem> TransactionCatList { get; set; }
         [Display(Name = "Month")]
         public IEnumerable<SelectListItem> MonthList { get; set; }
         [Display(Name = "Year")]
         public IEnumerable<SelectListItem> YearList { get; set; }

         [Display(Name = "Month")]
         public int MonthIdES { get; set; }
        [Display(Name = "Year")]
         public int YearES { get; set; }
        public int OfficeType { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        [Display(Name = "Zone Office")]
         public IEnumerable<SelectListItem> ZoneOfficeList { get; set; }
         [Display(Name = "Area Office")]
         public IEnumerable<SelectListItem> AreaOfficeList { get; set; }
        [Display(Name = "Branch Office")]
         public IEnumerable<SelectListItem> BranchOfficeList { get; set; }
        [Display(Name = "HO Deparement")]
         public IEnumerable<SelectListItem> HODeparementList { get; set; }
        [Display(Name = "Zone Audit Office")]
         public IEnumerable<SelectListItem> ZoneAuditList { get; set; }
        
    }
}