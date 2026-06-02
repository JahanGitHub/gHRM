using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Loan
{
    public class LoanCollectionViewModel
    {
        public int LoanId { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? UptoDate { get; set; }
        public decimal TransactionAmount { get; set; }
        public string Narration { get; set; }
        public decimal TotalDue { get; set; }
        public decimal InterestCharge { get; set; }
    }
}