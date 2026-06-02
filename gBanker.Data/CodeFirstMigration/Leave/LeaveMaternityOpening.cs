using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("leave.LeaveMaternityOpening")]
    public partial class LeaveMaternityOpening
    {
        [Key]
        public int MatLeaveId { get; set; }

        public long? EmployeeId { get; set; }

        public int? MaternityNo { get; set; }

        public int? MaternityDays { get; set; }
    }
}
