using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
    [Table("gcpf.EmployeeConfiguration")]
   public class EmployeeConfiguration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EmployeeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string EmployeeCode { get; set; }

        [Required]
        public bool IsActive { get; set; }
        [Required]
        public bool IsPFWithdrawn { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string EmployeeName { get; set; }

        [Required]
        [ForeignKey("OfficeSetup")]
        public int OfficeId { get; set; }
        public OfficeSetup OfficeSetup { get; set; }
        public decimal AdditionalSelfRate { get; set; }

        [Required]
        public long CreateUser { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeletedUser { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? DeleteDate { get; set; }
    }
}
