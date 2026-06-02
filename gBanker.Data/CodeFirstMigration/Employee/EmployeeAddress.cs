using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeAddress")]
    public partial class EmployeeAddress
    {
        [Key]
        public long AddressId { get; set; }

        public long EmployeeId { get; set; }

        [Required]
        [StringLength(2)]
        public string AddressType { get; set; }

        public int CountryId { get; set; }

        public int StateOrProvinceId { get; set; }

        public int? DistrictId { get; set; }

        public int? ThanaId { get; set; }

        public int? UnionId { get; set; }
                
        [StringLength(100)]
        public string StreetOrHouse { get; set; }
               
        [StringLength(100)]
        public string ZipCode { get; set; }
        [StringLength(200)]
        public string PostOffice { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        public virtual District District { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual LgThana LgThana { get; set; }

        public virtual LgUnion LgUnion { get; set; }

        [ForeignKey("StateOrProvinceId")]
        public virtual StateOrProvince StateOrProvince { get; set; }

        public string AddressDetail { get; set; }
    }
}
