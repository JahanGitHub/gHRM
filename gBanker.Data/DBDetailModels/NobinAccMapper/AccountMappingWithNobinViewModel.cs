using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.NobinAccMapper
{
    public class AccountMappingWithNobinViewModel
    {
        public string ComponentName { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public string UnitName { get; set; }
        public string NobinAccCode { get; set; }
        public string AccName { get; set; }
        public string TransactionType { get; set; }
        //public string NobinAccCodeForDR { get; set; }
        //public string NobinAccNameForDR { get; set; }
        //public string NobinAccCodeForCR { get; set; }
        //public string NobinAccNameForCR { get; set; }
        public string VoucherNaration { get; set; }
        public string ReverseNobinAccCodeForDR { get; set; }
        public string ReverseNobinAccNameForDR { get; set; }
        public string ReverseNobinAccCodeForCR { get; set; }
        public string ReverseNobinAccNameForCR { get; set; }
        public string ReverseVoucherNaration { get; set; }
    }
}
