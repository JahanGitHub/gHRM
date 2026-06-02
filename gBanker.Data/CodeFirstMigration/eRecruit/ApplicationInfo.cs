using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace gHRM.Data.CodeFirstMigration.eRecruit
{
    [Table("eRecruit.ApplicationInfo")]

    public partial class ApplicationInfo
    {

        [Key]
        public long ApplicationId { get; set; }

        [Required]
        [StringLength(500)]
        public string ApplicantName { get; set; }

        [Required]
        [StringLength(500)]
        public string FatherName { get; set; }

        [Required]
        [StringLength(500)]
        public string MotherName { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [StringLength(50)]
        public string BloodGroup { get; set; }

        [Required]
        [StringLength(50)]
        public string MaritalStatus { get; set; }

        public string NationalId { get; set; }

        [StringLength(100)]
        public string BirthRegistrationNo { get; set; }

        [Required]
        [StringLength(50)]
        public string Religion { get; set; }

        public int? PresentCountryId { get; set; }

        public int? PresentDivisionId { get; set; }

        public int? PresentDistrictId { get; set; }

        public string PresentThanaId { get; set; }

        public string PresentUnionId { get; set; }

        //[StringLength(50)]
        public string PresentStreetOrHouse { get; set; }

        //[StringLength(50)]
        public string PresentZipCode { get; set; }

        public int? PermanentCountryId { get; set; }

        public int? PermanentDivisionId { get; set; }

        public int? PermanentDistrictId { get; set; }

        public string PermanentThanaId { get; set; }

        public string PermanentUnionId { get; set; }

        [StringLength(1000)]
        public string PermanentStreetOrHouse { get; set; }

        [StringLength(50)]
        public string PermenantZipCode { get; set; }

        [StringLength(20)]
        public string MobileNo { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(1000)]
        public string Expreience { get; set; }

        [StringLength(1000)]
        public string ExtraCurriculum { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ApplicationDate { get; set; }

        [StringLength(50)]
        public string Height { get; set; }

        [StringLength(50)]
        public string Weight { get; set; }

        [StringLength(100)]
        public string GBIdNo { get; set; }

        [StringLength(1000)]
        public string EmployeeImageLink { get; set; }
        public byte[] ApplicantImage { get; set; }

        [StringLength(250)]
        public string EmployeeSignatureLink { get; set; }
        public byte[] ApplicantSignature { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsFinalSubmit { get; set; }
        public long? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

        public int ApplicantProfileSettingId { get; set; }

        public int AppliedPostId { get; set; }
        public string PresentPostOffice { get; set; }
        public string PermenantPostOffice { get; set; }
        public string Nationality { get; set; }

        public string ReferenceName { get; set; }
        public string ReferenceFatherName { get; set; }
        public string ReferenceMotherName { get; set; }
        public string ReferenceRelation { get; set; }
        public string ReferenceAddress { get; set; }
        public string ReferenceContactNo { get; set; }


        public string SecondReferenceName { get; set; }
        public string SecondReferenceFatherName { get; set; }
        public string SecondReferenceMotherName { get; set; }
        public string SecondReferenceRelation { get; set; }
        public string SecondReferenceAddress { get; set; }
        public string SecondReferenceContactNo { get; set; }

        public string Age { get; set; }


    }
}
