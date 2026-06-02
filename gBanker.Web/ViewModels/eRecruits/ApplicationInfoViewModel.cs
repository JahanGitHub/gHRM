using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.eRecruits
{
    public class ApplicationInfoViewModel
    {

        public long ApplicationId { get; set; }

        public string currentapplicationids { get; set; }

        [Required]
        [StringLength(150)]
        public string ApplicantName { get; set; }

        [Required]
        [StringLength(150)]
        public string FatherName { get; set; }

        [Required]
        [StringLength(150)]
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

        [StringLength(50)]
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

        public int? PermanentThanaId { get; set; }

        public int? PermanentUnionId { get; set; }

        //[StringLength(50)]
        public string PermanentStreetOrHouse { get; set; }

        [StringLength(50)]
        public string PermenantZipCode { get; set; }

        [StringLength(20)]
        public string MobileNo { get; set; }

        [StringLength(70)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(100)]
        public string Expreience { get; set; }

        [StringLength(100)]
        public string ExtraCurriculum { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ApplicationDate { get; set; }

        [StringLength(50)]
        public string Height { get; set; }

        [StringLength(250)]
        public string EmployeeImageLink { get; set; }
        public int rowSl { get; set; }
        public string GroupName { get; set; }
        public string BoardName { get; set; }

        public string vBoardName { get; set; }
        public string vPassingYear { get; set; }
        public List<SelectListItem> GroupNameList { get; set; }
        public List<SelectListItem> BoardNameList { get; set; }
        public string GPA { get; set; }
        //public string DivisionOrClass { get; set; }
        public string RollNo { get; set; }
        public long? RegNo { get; set; }
        public int RollNoVerify { get; set; }
        public IEnumerable<SelectListItem> GenderList { get; set; }
        public IEnumerable<SelectListItem> BloodGroupList { get; set; }
        public IEnumerable<SelectListItem> MaritalList { get; set; }

        public IEnumerable<SelectListItem> ReligionList { get; set; }
        public DateTime? ServerCurrentDate { get; set; }

        public int? GuarantorPresentCountryId { get; set; }
        public int? GuarantorPresentDivisionId { get; set; }
        public int? GuarantorPresentDistrictId { get; set; }
        public string GuarantorPresentThanaId { get; set; }
        public string GuarantorPresentUnionId { get; set; }
        public string GuarantorPresentStreetOrHouse { get; set; }
        public string GuarantorPresentZipCode { get; set; }

        public int? GuarantorPermanentCountryId { get; set; }
        public int? GuarantorPermanentDivisionId { get; set; }
        public int? GuarantorPermanentDistrictId { get; set; }
        public string GuarantorPermanentThanaId { get; set; }
        public string GuarantorPermanentUnionId { get; set; }
        public string GuarantorPermanentStreetOrHouse { get; set; }
        public string GuarantorPermanentZipCode { get; set; }
        public string UploadImage { get; set; }
        public string PassingYear { get; set; }
        public string Comment { get; set; }
        public string Comment2 { get; set; }
        public IEnumerable<SelectListItem> DegreeLevelList { get; set; }
        public string DegreeCode { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
        public string Post { get; set; }
        public IEnumerable<SelectListItem> ApplicantProfileSettingList { get; set; }

        public IEnumerable<SelectListItem> StateOrProvinceList { get; set; }
        public IEnumerable<SelectListItem> DistrictList { get; set; }
        public IEnumerable<SelectListItem> ThanaList { get; set; }
        public IEnumerable<SelectListItem> UnionList { get; set; }

        [Display(Name = "Photo")]
        public byte[] ApplicantImage { get; set; }
        public string ApplicationImageBase64 { get; set; }



        [Display(Name = "Signature Photo")]
        public byte[] ApplicantSignature { get; set; }
        public string ApplicationSignatureImageBase64 { get; set; }


        [StringLength(250)]
        public string EmployeeSignatureLink { get; set; }

        public bool? IsActive { get; set; }
        public long? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int ApplicantProfileSettingId { get; set; }
        public string Weight { get; set; }
        public string GBIdNo { get; set; }
        public string ObtainedMarks { get; set; }
        public string CompanyName { get; set; }
        public string SubjectName { get; set; }


        public IEnumerable<SelectListItem> AppliedPostList { get; set; }
        public int AppliedPostId { get; set; }
        public string GuarantorPresentPostOffice { get; set; }
        public string GuarantorPermenantPostOffice { get; set; }
        public string Nationality { get; set; }
        public IEnumerable<SelectListItem> GradeTypeList { get; set; }
        public int? GradeTypeId { get; set; }


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



        public string MastersSubject { get; set; }
        public string MastersSubjectName { get; set; }
        public string UniversityName { get; set; }
        public string HonorsSubject { get; set; }
        public string HonorsSubjectName { get; set; }
        public int ResultId { get; set; }
        public string ResultName { get; set; }
        public string PerHomeDistrict { get; set; }
        public string PerHomeDistrictName { get; set; }
        public long rowSlK { get; set; }
        public string PaentName { get; set; }
        public string DateOfBirthMsg { get; set; }
         
        public string SSC { get; set; }
        public string HSC { get; set; }
        public string PermanentThana { get; set; }
        public string Bachelor { get; set; }
        public string Masters { get; set; }
        public string SUMOfCGPA { get; set; }
        public string SummaryCGPA { get; set; }


    }
}