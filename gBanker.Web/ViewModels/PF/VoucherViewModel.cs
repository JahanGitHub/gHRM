using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.PF
{
 
    public class VoucherViewModel
    {
        public int SerialNo { get; set; }
        public string TransactionDate { get; set; }
        public long VoucherNo { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public decimal Dr { get; set; }
        public decimal Cr { get; set; }
        public string TransactionType { get; set; }
        public string Particulars { get; set; }
    }
}