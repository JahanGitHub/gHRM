using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeDocument")]
    public partial class EmployeeDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeDocumentId { get; set; }

        [Required(ErrorMessage = "Required")]
        public int EmployeeId { get; set; }

        public string DocumentType { get; set; }

        public string DocumentUrl { get; set; }

        public string DocumentRemarks { get; set; }

        [Required(ErrorMessage = "Required")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Required")]
        public Int64 CreateUser { get; set; }

        [Required(ErrorMessage = "Required")]
        public DateTime CreateDate { get; set; }

        public Int64? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
