using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.NobinAccMapper
{
    public class OfficeXReverseAccountMappingWithhNobin
    {
        public string OfficeCode { get; set; }
        public string ReverseNobinAccCodeForDR { get; set; }
        public string ReverseNobinAccCodeForCR { get; set; }
        public string ReverseVoucherNaration { get; set; }
        public bool? IsActive { get; set; }
        public long? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
