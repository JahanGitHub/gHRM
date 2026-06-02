using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.PF
{
    public class PayrollInstallmentCollectionViewModel
    {
        [Display(Name = "Process")]
        public string ProcessId { get; set; }
        public int MonthId { get; set; }
        [Display(Name = "Month")]
        public string Month { get; set; }
        [Display(Name = "Year")]
        public string Year { get; set; }
        [Display(Name = "Is Processed")]
        public bool IsProcessed { get; set; }
        public int ContributionCollTypeId { get; set; }
        public string ContributionTransType { get; set; }
        public int LoanCollTypeId { get; set; }
        public string LoanTransType { get; set; }
        [Display(Name = "TransactionDate")]
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        [Display(Name = "Create Date")]
        public string CDate { get; set; }
        [Display(Name = "Day Status")]
        public string DayStatus { get; set; }
        [Display(Name = "Transaction Date")]
        public string TransactionDate { get; set; }
        public bool IsOpen { get; set; }
        public string InstProcStatus { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }       

        [Display(Name = "Month")]
        public IEnumerable<SelectListItem> MonthList { get; set; }
        [Display(Name = "Year")]
        public IEnumerable<SelectListItem> YearList { get; set; }
    }
}