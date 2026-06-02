using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeePreviousWorkExperience")]
    public class EmployeePreviousWorkExperience
    {
        [Key]
        public int OrgId { get; set; }
        public string EmployeeCode { get; set; }
        public string OrganizationName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ExperienceYear { get; set; }
        public int ExperienceMonth { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public string SupervisorName { get; set; }
        public string SupervisorMobileNo { get; set; }
        public string LeaveReason { get; set; }
    }
}
