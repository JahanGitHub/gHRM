using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("WorkExperienceWithInterOrganization")]
    public class WorkExperienceWithInterOrganization
    {
        [Key]
        public int WorkExpId { get; set; }
        public long EmployeeId { get; set; }
        public string OrgCode { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsActive { get; set; }
        public long CreateBy { get; set; }
        public long UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
    }
}
