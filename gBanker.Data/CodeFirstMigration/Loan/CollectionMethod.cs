using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration.Loan
{
    [Table("loan.CollectionMethod")]
    public class CollectionMethod
    {
        [Key]
        public int Id { get; set; }
        public string LoanType { get; set; }
        public string MethodType { get; set; }
        public string CollectionFormat { get; set; }
        public int Principal { get; set; }
        public int Interest { get; set; }
        public bool IsActive { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
