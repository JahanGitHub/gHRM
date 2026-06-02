using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.NEP
{
    [Table("NEPVoucherMapping")]
    //[Keyless]
    public class NEPVoucherMapping
    {
        public string VoucherType { get; set; }
        public long? NEPVoucherId { get; set; }
        public DateTime? VoucherDate { get; set; }
        public int? PMonth { get; set; }
        public int? PYear { get; set; }
        public long? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? LastSendBy { get; set; }
        public DateTime? LastSendDate { get; set; }
    }
}
