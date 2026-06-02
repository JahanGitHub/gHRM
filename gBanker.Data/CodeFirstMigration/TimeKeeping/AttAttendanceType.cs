using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("att.AttAttendanceType")]
    public class AttAttendanceType
    {
        [Key]
        public int AttAttendanceTypeId { get; set; }

        public string AttenTypeShortName { get; set; }

        public string AttenTypeFullName { get; set; }

        public bool IsActive { get; set; }

        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
