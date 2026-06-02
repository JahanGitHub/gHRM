using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("leave.LeaveCategory")]
    public class LeaveCategory
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public string Detail { get; set; }
        public bool IsActive { get; set; }
    }
}
