using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
    [Table("ContributionRegister",Schema ="pf")]
    public partial class ContributionRegister
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ContributionRegisterId { get; set; }

        public long EmployeeId { get; set; }

        public decimal SelfContribution { get; set; }

        public decimal OrgContribution { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TransactionDate { get; set; }

        [Required]
        [StringLength(1)]
        public string TransactionType { get; set; }// I Interest,C=Contribution,D=Delete

        public string Comments { get; set; }
        public long CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public bool? IsDeleted { get; set; }

        public long? DeletedUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DeleteDate { get; set; }

        
    }
}
