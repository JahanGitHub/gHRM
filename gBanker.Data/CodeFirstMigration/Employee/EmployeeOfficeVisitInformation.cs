using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeOfficeVisitInformation")]
    public class EmployeeOfficeVisitInformation
    {
        [Key]
        public int EmpOfficeVisitId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string VisitType { get; set; }
        public string Location { get; set; }
        public string Reason { get; set; }
        public int CurrentOfficeProvided { get; set; }
        public bool IsActive { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
        public long CreateBy { get; set; }
        public long UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        
    }
}
