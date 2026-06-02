using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Loan
{
    [Table("LoanCollection", Schema = "loan")]
    public partial class LoanCollection
    {
        [Key]
        public long CollectionId { get; set; }
        public int LoanId { get; set; }
        public decimal Coll_LoanAmount { get; set; }
        public decimal Coll_InterestAmount { get; set; }
        public decimal InterestCharge { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Comments { get; set; }
        public string VoucherTypeID { get; set; }
        public string VoucherNo { get; set; }
        public decimal? Sundry { get; set; }

        public long? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeletedUser { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
