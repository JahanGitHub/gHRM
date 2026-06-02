using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("Branch")]
    public partial class Branch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BranchId { get; set; }

        [Required]
        [StringLength(250)]
        public string BranchName { get; set; }

        [Required]
        [StringLength(100)]
        public string BranchAddress { get; set; }

        [StringLength(100)]
        public string BranchEmail { get; set; }

        [StringLength(100)]
        public string BranchPhone { get; set; }

        public int CompanyId { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public virtual Company Company { get; set; }
    }
}
