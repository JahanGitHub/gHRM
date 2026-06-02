using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("att.AttCSVData")]
    public class AttCSVData
    {
        [Key]
        public int SNo { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string AttendanceTime { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
