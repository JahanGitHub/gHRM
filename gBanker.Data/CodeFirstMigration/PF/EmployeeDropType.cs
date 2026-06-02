using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration.PF
{
    [Table("gcpf.EmployeeDropType")]
  public partial class EmployeeDropType
    {
    
        [Key]
        [Display(Name="Drop Id")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DropId { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name="Drop Type")]
        public string DropType { get; set; }


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
