using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeDepartmentSection")]
    public class EmployeeDepartmentSection
    {
        [Key]
        public int SectionId { get; set; }
        public int DepartmentId { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public bool IsActive { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
