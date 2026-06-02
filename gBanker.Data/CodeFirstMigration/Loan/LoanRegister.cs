using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration.Loan
{
    [Table("loan.LoanRegister")]
    public class LoanRegister
    {
        [Key]
        public long LoanRegisterId { get; set; }
        public long CollectionId { get; set; }
        public int CollectionTypeId { get; set; }
        public int LoanId { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public long? VoucherNo { get; set; }
        public int LoanAmount { get; set; }
        public int InterestAmount { get; set; }
        public decimal? InterestCharge { get; set; }
        public decimal? Sundry { get; set; }
        public string Comments { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
    }
}
