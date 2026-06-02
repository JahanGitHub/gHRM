using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.NobinAccMapper
{
    public class AccountMappingWithNobin
    {
        public string ComponentName { get; set; }
        public string NobinAccCode { get; set; }
        public string VoucherNaration { get; set; }
        public bool? IsActive { get; set; }
        public long? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
