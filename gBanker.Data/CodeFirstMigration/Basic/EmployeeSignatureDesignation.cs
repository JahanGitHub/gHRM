using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeSignatureDesignation")]
    public class EmployeeSignatureDesignation
    {
        [Key]
        public int SignatureId { get; set; }
        public string SignatureCode { get; set; }
        public string SignatureName { get; set; }
        public bool IsActive { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
