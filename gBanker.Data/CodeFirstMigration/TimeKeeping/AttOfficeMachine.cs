
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("att.AttOfficeMachine")]
    public partial class AttOfficeMachine
    {
        [Key]
        public int OfficeMachineId { get; set; }

        [Required]
        [StringLength(50)]
        public string MachineName { get; set; }

        public int OfficeId { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
    }
}
