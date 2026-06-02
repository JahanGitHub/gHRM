using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("InternalOrganization")]
    public class InternalOrganization
    {
        [Key]
        public int OrgId { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationCode { get; set; }
        public bool IsActive { get; set; }
        public long? CreateBy { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
