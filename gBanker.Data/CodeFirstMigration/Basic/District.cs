using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{

    [Table("District")]
    public partial class District
    {
        public District()
        {
            EmployeeAddresses = new HashSet<EmployeeAddress>();
            LgThanas = new HashSet<LgThana>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int district_id { get; set; }

        public int division_Id { get; set; }

        [StringLength(255)]
        public string district_code { get; set; }

        [StringLength(200)]
        public string district_name_bng { get; set; }

        [StringLength(200)]
        public string district_name_eng { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public virtual StateOrProvince StateOrProvince { get; set; }

        public virtual ICollection<EmployeeAddress> EmployeeAddresses { get; set; }

        public virtual ICollection<LgThana> LgThanas { get; set; }
    }
}
