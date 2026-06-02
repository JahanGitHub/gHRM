using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("ReportSignature")]
    public class ReportSignature
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public long ASignatureId { get; set; }
        public long? BSignatureId { get; set; }
        public long? CSignatureId { get; set; }
        public long? DSignatureId { get; set; }
        public long? ESignatureId { get; set; }
        public long? FSignatureId { get; set; }
        public bool IsActive { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
