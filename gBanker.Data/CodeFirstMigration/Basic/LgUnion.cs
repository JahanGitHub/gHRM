using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("LgUnion")]
    public partial class LgUnion
    {
        public LgUnion()
        {
            EmployeeAddresses = new HashSet<EmployeeAddress>();
        }

        [Key]        
        public int union_id { get; set; }

        public int? thana_id { get; set; }

        [StringLength(255)]
        public string union_code { get; set; }

        [StringLength(200)]
        public string union_name_eng { get; set; }

        [StringLength(200)]
        public string union_name_bng { get; set; }

        public virtual ICollection<EmployeeAddress> EmployeeAddresses { get; set; }

        public virtual LgThana LgThana { get; set; }
    }
}
