using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    
    [Table("Country")]
    public partial class Country
    {
        public Country()
        {
            EmployeeAddresses = new HashSet<EmployeeAddress>();
            StateOrProvinces = new HashSet<StateOrProvince>();
        }

        public int CountryId { get; set; }

        [StringLength(50)]
        public string CountryCode { get; set; }

        [Required]
        [StringLength(100)]
        public string CountryName { get; set; }

        [Required]
        [StringLength(10)]
        public string CountryShortCode { get; set; }

        [Required]
        [StringLength(10)]
        public string isoCode3 { get; set; }

        public bool Status { get; set; }

        public virtual ICollection<EmployeeAddress> EmployeeAddresses { get; set; }

        public virtual ICollection<StateOrProvince> StateOrProvinces { get; set; }
    }
}
