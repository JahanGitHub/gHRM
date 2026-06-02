using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
   [Table("gcpf.OrganizationSetup")]
   public partial class OrganizationSetup
    {    
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int OrgId { get; set; }
        [Required]
        [MaxLength(100)]
        public string OrgName { get; set; }

        [Required]
        [DataType("smalldatetime")]
        public DateTime YearStartDate { get; set; }

        [Required]
        [DataType("smalldatetime")]
        public DateTime YearEndDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public long VoucherNo { get; set; }

        [Required]
        [ForeignKey("PFType")]
        public int PFTypeId { get; set; }
        public PFType PFType { get; set; }

        [NotMapped]
        public string PFTypeName { get; set; }


        [Required]
        public long CreateUser { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public long? DeletedUser { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? DeleteDate { get; set; }
    }
}
