using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
    [Table("gcpf.PFWithdrawan")]
    public partial class PFWithdrawan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long WithdrawanId { get; set; }
        
        [Required]
        public long EmployeeId { get; set; }
        
        [Required]
        public decimal SelfContribution { get; set; }

        [Required]
        public decimal OrgContribution { get; set; }
        
        [Required]
        public decimal SelfInterestAmount { get; set; }

        [Required]
        public decimal OrgInterestAmount { get; set; }

        [Required]
        public DateTime WithdrawnDate { get; set; }
        
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
