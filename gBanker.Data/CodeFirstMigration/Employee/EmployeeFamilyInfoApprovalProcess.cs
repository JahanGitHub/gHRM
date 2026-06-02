using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeFamilyInfoApprovalProcess")]
    public class EmployeeFamilyInfoApprovalProcess
    {
        [Key]
        public int Id { get; set; }
        public long EmployeeId { get; set; }
        public string Name { get; set; }
        public string Relation { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EducationalQualification { get; set; }
        public string Occupation { get; set; }
        public bool IsActive { get; set; }
        public DateTime InActiveDate { get; set; }
        public long CreateUser { get; set; }
        public long UpdateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public int ApprovedOrRejectedBy { get; set; }
        public DateTime ApprovalOrRejectDate { get; set; }
    }
}
