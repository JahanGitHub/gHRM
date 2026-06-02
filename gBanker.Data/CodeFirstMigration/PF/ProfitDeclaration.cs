using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
    [Table("pf.ProfitDeclaration")]
   public partial class ProfitDeclaration
    {
    
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeclararionId { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime YearStartDate { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime YearEndDate { get; set; }
        [Required]
        public decimal Profit { get; set; }
        public bool CalculationWithProfit { get; set; }
        [Required]
        public decimal ProfitRate { get; set; }
        [Required]
        public decimal InduceRate { get; set; }
        public decimal? DistribursAmount { get; set; }
        [Required]
        public string DeclarationStatus { get; set; }// Delete=D,Close=C,Approved=A,Entry=E

        [Required]
        public long CreateUser { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
        public long? DeletedUser { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? DeleteDate { get; set; }
    }
}
