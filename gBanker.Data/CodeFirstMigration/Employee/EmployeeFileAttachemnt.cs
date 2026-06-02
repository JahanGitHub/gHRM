using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeFileAttachemnt")]
    public class EmployeeFileAttachemnt
    {
        [Key]
        public long AttachmentId { get; set; }
        public long EmployeeId { get; set; }        
        public int DocumentTypeId { get; set; }
        public string FileName { get; set; }
        public string FileLocation { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? CreateBy { get; set; }
        public long? UpdateBy { get; set; }
        //public int MyProperty { get; set; }
        //public int MyProperty { get; set; }
    }
}
