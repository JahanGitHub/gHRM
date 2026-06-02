using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeInformationApproval")]
    public class EmployeeInformationApproval
    {
        [Key]
        public int  Id { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public bool IsActive { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
