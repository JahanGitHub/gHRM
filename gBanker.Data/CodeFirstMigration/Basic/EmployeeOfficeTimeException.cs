using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeOfficeTimeException")]
    public class EmployeeOfficeTimeException
    {
        [Key]
        public int Id { get; set; }
        public int OfficeTypeId { get; set; }
        public int OfficeId { get; set; }
        public string TimeExceptionReason { get; set; }
        public DateTime? LogInTime { get; set; }
        public DateTime? LastLogInTime { get; set; }
        public DateTime? LogOutTime { get; set; }
        public DateTime? EffectiveDateFrom { get; set; }
        public DateTime? EffectiveDateTo { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public long CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long UpdateBy { get; set; }
        public int TimeKeepingRosterId { get; set; }
    }
}
