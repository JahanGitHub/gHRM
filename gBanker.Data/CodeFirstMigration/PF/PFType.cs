using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration.PF
{
    [Table("gcpf.PFType")]
    public partial class PFType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PFTypeId { get; set; }
        [MaxLength(3)]
        [Required]
        [Display(Name = "Short Name")]
        public string ShortName { get; set; }
        [MaxLength(50)]
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        public bool HasSelfContribution { get; set; }
        [Required]
        public bool HasOrgContribution { get; set; }
        [Required]
        public bool HasAddSelfContribution { get; set; }
        [Required]
        public decimal SelfContributionRate { get; set; }
        [Required]
        public decimal OrgContributionRate { get; set; }

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
