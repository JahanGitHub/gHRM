using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeStatusHistory")]
    public partial class EmployeeStatusHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HistoryId { get; set; }

        public long EmployeeId { get; set; }
        
        [StringLength(250)]
        public string Status { get; set; }

        public int? StatusId { get; set; }

        public DateTime? StartDate { get; set; } 
        public DateTime? ConfirmationDate { get; set; }
        public bool IsActive { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }        
    }
}