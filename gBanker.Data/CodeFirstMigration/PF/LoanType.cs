using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
   [Table("gcpf.LoanType")]
   public partial class LoanType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int LoanTypeId { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name="Loan Type")]
        public string LoanTypeName { get; set; }
        [Required]
        [Display(Name = "Interest Rate")]
        public decimal InterestRate { get; set; }
        public int? LoanPercentage { get; set; }
              
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
        public int InterestRateTypeId { get; set; }
    }
}
