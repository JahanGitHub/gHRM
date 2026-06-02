using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("CurrentOrganizationRelationship")]
    public class CurrentOrganizationRelationship
    {
        [Key]
        public int SelfOrgRelationId { get; set; }
        public int OfficeId { get; set; }
        public long EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int RelationId { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
        public bool IsActive { get; set; }
        public long CreateBy { get; set; }
        public long UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        
        
    }
}
