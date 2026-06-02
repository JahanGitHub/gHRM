
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("att.View_EmployeeTimeKeepingException")]
    public partial class View_EmployeeTimeKeepingException
    {
        public int? RowSl { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string ED { get; set; }
        public string LT { get; set; }
        public string LOutT { get; set; }
        public string LastLoginTime { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        //public int AttAttendanceTypeId { get; set; }
        public int AttendenceTypeId { get; set; }
        public string AttenTypeFullName { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public int EmployeeRank { get; set; }
        public string OffcDesignName { get; set; }
        public string Justification { get; set; }
    }
}