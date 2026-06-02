
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeEquivalentDesignation")]
    public partial class EmployeeEquivalentDesignation
    {
        [Key]
        public int EquivalentDesigId { get; set; }
        public string EquivalentDesignationName { get; set; }
        public bool IsActive { get; set; }
        public long? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
