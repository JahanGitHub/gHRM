using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("[att].[TimekeepingAttendanceDevice]")]
    public partial class TimekeepingAttendanceDevice
    {
        [Key]
        public int Id { get; set; }
        public string DeviceCode { get; set; }
        public string DeviceName { get; set; }
        public string CompanyCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
