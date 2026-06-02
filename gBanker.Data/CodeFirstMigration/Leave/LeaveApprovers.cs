using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("leave.LeaveApprovers")]
    public class LeaveApprovers
    {
        [Key]
        public int ID { get; set; }
        public long EmployeeId { get; set; }
        public int? EmployeeOfficeId { get; set; }
        public int? EmployeeDepartmentId { get; set; }
        public int? EmployeeDesignationId { get; set; }

        public int ApprovalLevel { get; set; }
        public long ApproverEmpId { get; set; }
        public int ApproveOfficeId { get; set; }
        public int ApproveDepartmentId { get; set; }
        public int ApproveDesignationId { get; set; }
        public bool ManualUpdated { get; set; }

        public bool IsActive { get; set; }
        public long CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

        public int? FromDay { get; set; }
        public int? ToDay { get; set; }
        
    }
}
