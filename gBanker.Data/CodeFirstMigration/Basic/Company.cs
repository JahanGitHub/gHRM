using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("Company")]
    public partial class Company
    {
        public int CompanyId { get; set; }

        [Required]
        [StringLength(250)]
        public string CompanyName { get; set; }
        public string CompanyNameOther { get; set; }
        public string CompanyShortName { get; set; }
        public string CompanyCode { get; set; }
        public string CompanySignaturePath { get; set; }
        public string ImagePath { get; set; }
        public byte[] CompanyImage { get; set; }

        [Required]
        [StringLength(500)]
        public string CompanyAddress { get; set; }

        [StringLength(100)]
        public string CompanyEmail { get; set; }

        [StringLength(100)]
        public string CompanyMobile { get; set; }

        [StringLength(100)]
        public string CompanyPhone { get; set; }

        [StringLength(100)]
        public string CompanyType { get; set; }
        public int CountryId { get; set; }

        [StringLength(250)]
        public string CompanySlogan { get; set; }

        [StringLength(250)]
        public string WebsiteUrl { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
    }
}
