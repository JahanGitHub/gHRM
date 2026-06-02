using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("StateOrProvince")]
    public partial class StateOrProvince
    {
        public StateOrProvince()
        {
            Districts = new HashSet<District>();
            EmployeeAddresses = new HashSet<EmployeeAddress>();
        }

        public int StateOrProvinceId { get; set; }

        public int CountryId { get; set; }

        [StringLength(10)]
        public string CountryShortCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public bool Status { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<District> Districts { get; set; }

        public virtual ICollection<EmployeeAddress> EmployeeAddresses { get; set; }
    }
}
