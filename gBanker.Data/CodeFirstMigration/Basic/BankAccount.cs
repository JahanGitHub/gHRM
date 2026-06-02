using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.Basic
{
    [Table("BankAccount")]
    public class BankAccount
    {
        [Key]
        public int AccountId { get; set; }
        public int BankId { get; set; }
        public int BranchId { get; set; }
        public string AccountNo { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? CreateBy { get; set; }
        public long? UpdateBy { get; set; }
    }
}
