
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("LgThana")]
    public partial class LgThana
    {
        public LgThana()
        {
            EmployeeAddresses = new HashSet<EmployeeAddress>();
            LgUnions = new HashSet<LgUnion>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int thana_id { get; set; }

        public int? district_id { get; set; }

        [StringLength(255)]
        public string thana_code { get; set; }

        [StringLength(200)]
        public string thana_name_eng { get; set; }

        [StringLength(200)]
        public string thana_name_bng { get; set; }

        public virtual District District { get; set; }

        public virtual ICollection<EmployeeAddress> EmployeeAddresses { get; set; }

        public virtual ICollection<LgUnion> LgUnions { get; set; }

        
    }
}
