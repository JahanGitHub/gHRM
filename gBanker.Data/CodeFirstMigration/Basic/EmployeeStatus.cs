using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeStatus")]
    public class EmployeeStatus
    {
        [Key]
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusValue { get; set; }
        public int ViewOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsValid { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        
        public bool? IsSalaryApplicable { get; set; }
    }
}
