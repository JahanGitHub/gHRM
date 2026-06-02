using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("att.AttCardIssue")]
    public partial class AttCardIssue
    {
        public long AttCardIssueId { get; set; }
        [Display(Name = "Employee Id")]
        public long EmployeeId { get; set; }

        [Required]
        [StringLength(20)]
        public string CardNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CardIssueDate { get; set; }

        [StringLength(100)]
        public string Remarks { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
    }
}
