using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("att.EmployeeTimeKeepingException")]
    public class EmployeeTimeKeepingException
    {
        [Key]
        public int Id { get; set; }
        public long EmployeeId { get; set; }
        public int AttendenceTypeId { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime EventDate { get; set; }
        public string Justification { get; set; }
        public long CreateBy { get; set; }
        public long UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }

    }
}
