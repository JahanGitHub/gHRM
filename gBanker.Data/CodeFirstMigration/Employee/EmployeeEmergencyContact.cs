using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeEmergencyContact")]
    public class EmployeeEmergencyContact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmergencyContactId { get; set; }

        public long EmployeeId { get; set; }
        public string ContactName { get; set; }
        public string Relation { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
        public string OwnEmail { get; set; }
        public string OfficialEmail { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public long? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
