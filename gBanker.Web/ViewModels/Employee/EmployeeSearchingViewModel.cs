using gHRM.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeSearchingViewModel : BaseModel
    {
        public long EmployeeId { get; set; }

        public int? CompanyId { get; set; }

        public int? BranchId { get; set; }

        [Display(Name = "Office Name অফিসের নাম")]
        public int? OfficeId { get; set; }

        public int? NewHoId { get; set; }

        public int? NewZoneId { get; set; }

        public int? NewAreaId { get; set; }

        public int? NewBranchId { get; set; }

        [Display(Name = "Office Type")]
        public int? OfficeTypeId { get; set; }

        public string OfficeTypeName { get; set; }

        //[Required(ErrorMessage = "Batch No is required")]
        [Display(Name = "Batch No (ব্যাচ নং)")]
        public string BatchNo { get; set; }

        [Display(Name = "Employee Code (কর্মকর্তা/কর্মচারী পরিচিতি নং)")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Name (নাম)")]
        public string EmployeeName { get; set; }

        [Display(Name = "Name BN (নাম বাংলায়)")]
        public string EmployeeNameBng { get; set; }

        public string kMessage { get; set; }

        [Display(Name = "Date of Birth (জন্ম তারিখ)")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Birth Place (জন্মস্থান)")]
        public string BirthPlace { get; set; }

        public DateTime FirstDateOfScale { get; set; }

        [Display(Name = "Status Date (স্ট্যাটাসের তারিখ)")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StatusDate { get; set; }

        [Display(Name = "Status From Date ")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StatusFromDate { get; set; }

        [Display(Name = "Status To Date ")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StatusToDate { get; set; }

        public string StatusDateForCertificate { get; set; }

        [Display(Name = "Agreement Period (চুক্তির মেয়াদকাল)")]
        public int? AgreementPeriodInMonth { get; set; }

        [Display(Name = "Status Duration (স্ট্যাটাস মেয়াদকাল)")]
        public int? StatusPeriodInMonth { get; set; }

        [Display(Name = "Status Cause (স্ট্যাটাস পরিবর্তনের কারণ)")]
        public string TerminationCause { get; set; }

        [Display(Name = "Status Cause (স্ট্যাটাস পরিবর্তনের কারণ)")]
        public string DefaultTerminationCause { get; set; }

        [Display(Name = "Status Cause (স্ট্যাটাস পরিবর্তনের কারণ)")]
        public string DefaultRetiredCause { get; set; }

        [Display(Name = "Gender (লিঙ্গ)")]
        public string Gender { get; set; }

        [Display(Name = "Permanent Address (স্থায়ী ঠিকানা)")]
        public string PermanentAddress { get; set; }

        [Display(Name = "Present Address (বর্তমান ঠিকানা)")]
        public string PresentAddress { get; set; }

        [Display(Name = "Marital Status (বৈ্বাহিক অবস্থা)")]
        public string MaritalStatus { get; set; }

        [Display(Name = "Nationality (জাতীয়তা)")]
        public string Nationality { get; set; }

        [StringLength(50)]
        [Display(Name = "National ID (জাতীয় পরিচয়পত্র নং)")]
        public string NationalId { get; set; }

        [Display(Name = "Religion (ধর্ম)")]
        public string Religion { get; set; }

        [Display(Name = "Tin No (টিন নম্বর)")]
        public string TinNo { get; set; }

        [Display(Name = "Employee Image (ছবি)")]
        public byte[] EmployeeImage { get; set; }

        [Display(Name = "Signature (স্বাক্ষর)")]
        public byte[] EmpSignature { get; set; }

        [Display(Name = "Gross Salary (থোক বেতন)")]
        public decimal? GrossSalary { get; set; }

        public decimal? TotalEarnings { get; set; }

        public DateTime DateOfEmployeeStatus { get; set; }

        [Display(Name = "Email (ই-মেইল)")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Official Email (অফিস ই-মেইল)")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string OfficialEmail { get; set; }

        [Display(Name = "Blood Group (রক্তের গ্রুপ)")]
        public string BloodGroup { get; set; }

        [Display(Name = "Passport No. (পাসপোর্ট নাম্বার)")]
        public string PassportNo { get; set; }

        [Display(Name = "Passport Issue Date (ইস্যুর তারিখ)")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PassportIssueDate { get; set; }

        [Display(Name = "Passport Expire Date (মেয়াদ উত্তির্নের তারিখ)")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PassportExpireDate { get; set; }

        [Display(Name = "Contact No 1 (যোগাযোগ)")]
        public string ContactNo1 { get; set; }

        [Display(Name = "Contact No 2 (যোগাযোগ)")]
        public string ContactNo2 { get; set; }

        [Display(Name = "Payroll Position (পদবী)")]
        public int DesignationId { get; set; }

        public int? SignatureDesignationId { get; set; }

        public string SignatureDesignation { get; set; }

        public int? OfficeDesignationId { get; set; }

        public string OfficeDesignationName { get; set; }

        public string DepartmentName { get; set; }

        public int? Adjustment { get; set; }

        public string DesignationName { get; set; }

        public int? GradeId { get; set; }

        public int? Step { get; set; }

        public string FirstJoiningDateMsg { get; set; }

        [Display(Name = "Job Confirmation Date (নিশ্চিতকরন তারিখ)")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ConfirmationDate { get; set; }

        [Display(Name = "Previous Confirmation Date (পূর্বের নিশ্চিতকরন তারিখ)")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PreviousConfirmationDate { get; set; }

        public string EmployementTypeName { get; set; }

        public string ConfirmationDateMsg { get; set; }

        public string StatusDateMsg { get; set; }

        public string LastPostingDtMSg { get; set; }

        public string LastPromotionDtMsg { get; set; }

        public int PostingStatus { get; set; }

        [Display(Name = "Department (বিভাগ)")]
        public int DepartmentId { get; set; }

        [Display(Name = "Joining Date (যোগদান তারিখ)")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FirstJoiningDate { get; set; }

        public int EmployeeStatusId { get; set; }

        [Display(Name = "Employment Status (কর্মসংস্থানের অবস্থা)")]
        public string EmployeeStatus { get; set; }

        public int? EmploymentTypeId { get; set; }

        public string EmploymentType { get; set; }



        public bool IsValidEmployeeStatus { get; set; }

        [Display(Name = "Responsibility (অফিস পদবী)")]
        public string EmployeeRank { get; set; }

        public string OfficeCode { get; set; }

        [Display(Name = "Office Name (কর্মস্থলের নাম)")]
        public string OfficeName { get; set; }

        [Display(Name = "Designation (পদবী)")]
        public string DeptDesigStatus { get; set; }

        [Display(Name = "Address (ঠিকানা)")]
        public string Address { get; set; }
        [Display(Name = "Job Experience (চাকুরীর অভিজ্ঞতা)")]
        public string JobExperience { get; set; }

        [Display(Name = "Phone (ফোন)")]
        public string Phone { get; set; }

        [Display(Name = "Employee Image (ছবি)")]
        public HttpPostedFileBase ImgFile { get; set; }

        public DateTime? ServerCurrentDate { get; set; }
        [Display(Name = "Probation Period (শিক্ষানবিশ কাল)")]

        public int? ProbationaryPeriod { get; set; }

        public string BankCode { get; set; }
        public bool? IsSalaryApplicable { get; set; }
        public int? OfficeLocationId { get; set; }
        public int? PFTypeId { get; set; }

        public IEnumerable<SelectListItem> AgreementPeriodInMonthList { get; set; }
        public IEnumerable<SelectListItem> StatusPeriodInMonthList { get; set; }

        public IEnumerable<SelectListItem> ProbationPeriodList { get; set; }

        public IEnumerable<SelectListItem> DegreeLevelList { get; set; }

        public IEnumerable<SelectListItem> DegreeList { get; set; }

        public IEnumerable<SelectListItem> ConcentrationList { get; set; }

        public IEnumerable<SelectListItem> RetiredCauseList { get; set; }

        public IEnumerable<SelectListItem> TerminationCauseList { get; set; }

        public IEnumerable<SelectListItem> RankList { get; set; }

        public IEnumerable<SelectListItem> EmployeeStatusList { get; set; }

        public IEnumerable<SelectListItem> DepartmentList { get; set; }

        public IEnumerable<SelectListItem> DesignationList { get; set; }

        public IEnumerable<SelectListItem> OfficeDesignationList { get; set; }

        public IEnumerable<SelectListItem> GenderList { get; set; }

        public IEnumerable<SelectListItem> MaritalList { get; set; }

        public IEnumerable<SelectListItem> ReligionList { get; set; }

        public IEnumerable<SelectListItem> HOList { get; set; }

        public IEnumerable<SelectListItem> ZoneList { get; set; }

        public IEnumerable<SelectListItem> AreaList { get; set; }

        public IEnumerable<SelectListItem> UnitList { get; set; }

        public IEnumerable<SelectListItem> BranchList { get; set; }

        public IEnumerable<SelectListItem> OfficeList { get; set; }

        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }

        //Employee Education

        public long EmployeeEducationId { get; set; }

        [StringLength(150)]
        //[Required(ErrorMessage = "Degree Title is required")]

        [Display(Name = "Degree Level (খেতাব সমতা)")]
        public string DegreeLevel { get; set; }

        [Display(Name = "Degree/Title (খেতাব)")]
        public string DegreeTitle { get; set; }

        [StringLength(250)]
        //[Required(ErrorMessage = "Concentration is required")]
        //[Display(Name = "Concentration (Department/Major/Group)(মূল বিষয়)")]
        [Display(Name = "Concentration Name (মূল বিষয়)")]
        public string Concentration { get; set; }


        //[Display(Name = "Concentration (Department/Major/Group)(মূল বিষয়)")]
        [Display(Name = "Concentration Name (মূল বিষয়)")]
        public string ConcentrationName { get; set; }

        [Display(Name = "Concentration Code")]
        public string ConcentrationCode { get; set; }

        public int ConcentrationId { get; set; }

        public string CertificateType { get; set; }
        public IEnumerable<SelectListItem> CertificateTypeList { get; set; }
        [StringLength(500)]
        //[Required(ErrorMessage = "Institution Name is required")]
        [Display(Name = "Institution Name (প্রতিষ্ঠানের নাম)")]
        public string InstitutionName { get; set; }

        //[Required(ErrorMessage = "Passing Year is required")]
        [Display(Name = "Passing Year (পাসের সন)")]
        public string PassingYear { get; set; }

        [StringLength(50)]
        //[Required(ErrorMessage = "Result Type is required")]
        [Display(Name = "Result Type (ফলাফলের ধরন)")]
        public string ResultType { get; set; }

        [StringLength(11)]
        [Display(Name = "Division (বিভাগ)")]
        public string Division { get; set; }

        [StringLength(11)]
        [Display(Name = "Marks/Percentage (নম্বর/শতকরা)")]
        public string MarksPercentage { get; set; }

        [StringLength(10)]
        [Display(Name = "CGPA (সিজিপিএ)")]
        public string CGPA { get; set; }

        [StringLength(10)]
        [Display(Name = "CGPA Scale")]
        public string CGPAScale { get; set; }

        [StringLength(20)]
        [Display(Name = "Duration (স্থিতিকাল)")]
        public string Duration { get; set; }

        [StringLength(500)]
        [Display(Name = "Achievements (অর্জন)")]
        public string Acheivements { get; set; }

        public IEnumerable<SelectListItem> ResultTypeList { get; set; }


        // Employee Address

        public long EmployeeAddressId { get; set; }

        //[Required(ErrorMessage = "Address Type is required")]
        [Display(Name = "Address Type (ঠিকানার ধরন)")]
        public string AddressType { get; set; }

        //[Required(ErrorMessage = "Country Name is required")]
        [Display(Name = "Country Name (দেশের নাম)")]
        public int CountryId { get; set; }

        //[Required(ErrorMessage = "Division Name is required")]
        [Display(Name = "Division Name (বিভাগের নাম)")]
        public int StateOrProvinceId { get; set; }

        [Display(Name = "District Name (জেলার নাম)")]
        public int? DistrictId { get; set; }

        [Display(Name = "Thana Name (থানার নাম)")]
        public int? ThanaId { get; set; }

        [Display(Name = "Union Name (ইউনিয়নের নাম)")]
        public int? UnionId { get; set; }

        [Display(Name = "Street/House (সড়ক/বাড়ি নং)")]
        public string StreetOrHouse { get; set; }

        //[Required(ErrorMessage = "Zip Code is required")]
        [Display(Name = "Zip Code (পোস্ট কোড)")]
        public string ZipCode { get; set; }

        public int? ZoneId { get; set; }
        public int? AreaId { get; set; }
        public int? UnitId { get; set; }
        [Display(Name = "Head Office")]
        public int? HeadOfficeId { get; set; }
        public int? ProjectId { get; set; }
        [Display(Name = "Supervisor Name")]
        [Required(ErrorMessage = "Supervisor Name is required")]
        public long SupervisorId { get; set; }
        public int SupervisorDeptartmentId { get; set; }
        [Display(Name = "Responsibility")]

        public string SupervisorEmployeeRank { get; set; }
        public IEnumerable<SelectListItem> SupervisorList { get; set; }
        public IEnumerable<SelectListItem> SupervisorOrnamentalDesignationList { get; set; }
        public IEnumerable<SelectListItem> SupervisorDeptList { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public IEnumerable<SelectListItem> AddressTypeList { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> StateOrProvinceList { get; set; }
        public IEnumerable<SelectListItem> DistrictList { get; set; }
        public IEnumerable<SelectListItem> ThanaList { get; set; }
        public IEnumerable<SelectListItem> UnionList { get; set; }
        public IEnumerable<SelectListItem> MedicalPersonList { get; set; }
        public IEnumerable<SelectListItem> BloodGroupList { get; set; }
        public IEnumerable<SelectListItem> BloodPressureTypeList { get; set; }

        // Employee Reference

        public long EmployeeReferenceId { get; set; }

        //[Required(ErrorMessage = "Reference name is required")]
        [Display(Name = "Reference Name (রেফারেন্সের নাম)")]
        public string EmployeeReferenceName { get; set; }

        //[Required(ErrorMessage = "Occupation is required")]
        [Display(Name = "Occupation (পেশা)")]
        public string EmployeeReferenceOccupation { get; set; }

        //[Required(ErrorMessage = "Designation is required")]
        [Display(Name = "Designation (পদবী)")]
        public string EmployeeReferenceDesignation { get; set; }

        [Display(Name = "Relation (সম্পর্ক)")]
        public string ConnectionWithEmployee { get; set; }

        [Display(Name = "Address (ঠিকানা)")]
        public string ContactAddress { get; set; }

        [Display(Name = "Mobile (মোবাইল নম্বর)")]
        public string Mobile { get; set; }

        [Display(Name = "Telephone (টেলিফোন নম্বর)")]
        public string Telephone { get; set; }

        [Display(Name = "Fax (ফেক্স)")]
        public string Fax { get; set; }

        [Display(Name = "Email (ই-মেইল)")]
        public string RefEmail { get; set; }

        [Display(Name = "Remarks (মন্তব্য)")]
        public string Remarks { get; set; }
        [Display(Name = "Language (ভাষা)")]
        public string Language { get; set; }

        // Employee Family Info
        public long EmployeeFamilyInfoId { get; set; }

        //[Required(ErrorMessage = "Member name is required")]
        [Display(Name = "Member Name (সদস্যের নাম)")]
        public string FamilyMemberName { get; set; }

        //[Required(ErrorMessage = "Relation is required")]
        [Display(Name = "Relation (সম্পর্ক)")]
        public string RelationWithFamilyMember { get; set; }

        //[Required(ErrorMessage = "Gender is required")]
        [Display(Name = "Gender (লিঙ্গ)")]
        public string FamilyMemberGender { get; set; }

        [Display(Name = "Date of Birth (জন্ম তারিখ)")]
        //[InputMask("99/99/9999")]
        public string FamilyMemberDateOfBirth { get; set; }

        [Display(Name = "Occupation (পেশা)")]
        public string FamilyMemberOccupation { get; set; }
        [Display(Name = "Educatiuonal Qualification (শিক্ষাগত যোগ্যতা)")]
        public string EducationalQualification { get; set; }

        public IEnumerable<SelectListItem> relationWithEmployeeList { get; set; }
        public IEnumerable<SelectListItem> languageList { get; set; }
        public IEnumerable<SelectListItem> efficiencyList { get; set; }

        [Display(Name = "Efficiency (দক্ষতা)")]
        public string Efficiency { get; set; }
        [Display(Name = "Computer Efficiency (কম্পিউটার দক্ষতা)")]
        public string ComputerEfficiency { get; set; }
        ////for posting details
        public long sl { get; set; }
        public string JoiningDate { get; set; }
        public string DepartureDate { get; set; }
        public decimal? Tha_Dist { get; set; }

        ///Employee query
        public int TotEmployee { get; set; }

        public string OffcDesignName { get; set; }

        public string EmployeeStatusDate { get; set; }

        public string WitnessCode { get; set; }

        public string WitnessName { get; set; }

        public string WitnessDesignation { get; set; }

        public string WitnessAddress { get; set; }

        public string WitnessDateMsg { get; set; }

        public int EmbezzleEmpId { get; set; }

        public int NomineeMasterId { get; set; }

        public string SNO { get; set; }

        public long NomineeDetailId { get; set; }

        public string NomineeType { get; set; }

        public string NomineeName { get; set; }

        public string NomineeAddress { get; set; }

        public int? NomineeAge { get; set; }

        public string NomineeRelation { get; set; }

        public string Mode { get; set; }

        public decimal? NomineePercentage { get; set; }

        public string NomineeNationalId { get; set; }

        public string NomineeRemarks { get; set; }

        public string NomineeTypeValue { get; set; }

        public string SlNo { get; set; }

        public string ZoneName { get; set; }

        public string AreaName { get; set; }

        public string BranchName { get; set; }

        public string FirstDateOfScaleMsg { get; set; }

        public string DateOfBirthMsg { get; set; }

        public int EmpCnt { get; set; }

        public string NomineeTypeShort { get; set; }

        // Emergency Contact
        [Display(Name = "Contact Name (যোগাযোগের নাম)")]
        public string EmergencyContactName { get; set; }
        [Display(Name = "Relation (সম্পর্ক)")]
        public string EmergencyRelation { get; set; }
        [Display(Name = "Mobile Number (মোবাইল নাম্বার)")]
        public string EmergencyMobile { get; set; }
        [Display(Name = "Telephone Number (টেলিফোন নাম্বার)")]
        public string EmergencyTelephone { get; set; }
        [Display(Name = "E-mail (ই-মেইল)")]
        public string EmergencyOwnEmail { get; set; }
        [Display(Name = "Official E-mail (অফিস ই-মেইল)")]
        public string EmergencyOfficialEmail { get; set; }
        [Display(Name = "Address (ঠিকান)")]
        public string EmergencyAddress { get; set; }
        // Medical Info
        [Display(Name = "Person (ব্যক্তি)")]
        public string MedicalInfoOf { get; set; }
        [Display(Name = "Blood Group (রক্তের গ্রুপ)")]
        public string PersonBloodGroup { get; set; }
        [Display(Name = "Blood Pressure (রক্ত চাপ)")]
        public bool HasBloodPressure { get; set; }
        [Display(Name = "Pressure Type (ধরণ)")]
        public string BloodPressureType { get; set; }
        [Display(Name = "Diabetics (ডায়েবেটিস)")]
        public bool HasDiabetics { get; set; }
        [Display(Name = "Heart Diseases (হৃদরোগ")]
        public bool HasHeartDisease { get; set; }
        [Display(Name = "Allergies (এলার্জি)")]
        public bool HasAlergy { get; set; }
        [Display(Name = "Other Diseases (অন্যান্য)")]
        public bool HasOtherDisease { get; set; }
        [Display(Name = "X-Ray Chest (বুকের এক্সরে)")]
        public bool? XRayChest { get; set; }
        [Display(Name = "VDRL (ভিডিআরএল)")]
        public bool? VDRL { get; set; }
        [Display(Name = "HBs Ag (E) (হেপাটাইটিস)")]
        public bool? HBsAgE { get; set; }
        [Display(Name = "Vision Test (দৃষ্টি পরীক্ষা)")]
        public bool? VisionTest { get; set; }
        [Display(Name = "Weight (ওজন)")]
        public string Weight { get; set; }
        [Display(Name = "Height (উচ্চতা")]
        public string Height { get; set; }

        [Display(Name = "Remarks (মন্তব্য)")]
        public string MedicalRemarks { get; set; }

        public string ExperienceYear { get; set; }
        public string ExperienceMonth { get; set; }
        public string ExperienceDay { get; set; }
        public List<SelectListItem> EmergencyContactList { get; set; }
        public int DocumentTypeId { get; set; }
        public string TypeName { get; set; }
        public List<SelectListItem> DocumentTypeNameList { get; set; }

        [Display(Name = "Document Attachment")]
        public byte[] EmployeeDocument { get; set; }
        public string OrnamentalDesignationName { get; set; }


        public int GuarantorId { get; set; }
        public string GuarantorName { get; set; }
        public int? GuarantorRelationshipId { get; set; }
        public int? OccupationId { get; set; }
        public string ContactNo { get; set; }
        public string GurantorNationalID { get; set; }
        public HttpPostedFileBase GuarantorImgFile { get; set; }
        public byte[] GuarantorImage { get; set; }//
        public int? PresentCountryId { get; set; }
        public int? PresentDivisionId { get; set; }
        public int? PresentDistrictId { get; set; }
        public int? PresentThanaId { get; set; }
        public int? PresentUnionId { get; set; }
        public string PresentStreetOrHouse { get; set; }
        public string PresentZipCode { get; set; }

        public int? PermanentCountryId { get; set; }

        public int? PermanentDivisionId { get; set; }

        public int? PermanentDistrictId { get; set; }

        public int? PermanentThanaId { get; set; }

        public int? PermanentUnionId { get; set; }
        public string PermanentStreetOrHouse { get; set; }
        public string PermenantZipCode { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? CreateBy { get; set; }

        public long? UpdateBy { get; set; }
        public string GuarantorRelationshipName { get; set; }
        public IEnumerable<SelectListItem> RelationshipNameList { get; set; }
        public string OccupationName { get; set; }
        public IEnumerable<SelectListItem> OccupationNameList { get; set; }
        public string GRType { get; set; }
        public IEnumerable<SelectListItem> GRTypeList { get; set; }

        public int? GuarantorPresentCountryId { get; set; }
        public int? GuarantorPresentDivisionId { get; set; }
        public int? GuarantorPresentDistrictId { get; set; }
        public int? GuarantorPresentThanaId { get; set; }
        public int? GuarantorPresentUnionId { get; set; }
        public string GuarantorPresentStreetOrHouse { get; set; }
        public string GuarantorPresentZipCode { get; set; }

        public int? GuarantorPermanentCountryId { get; set; }
        public int? GuarantorPermanentDivisionId { get; set; }
        public int? GuarantorPermanentDistrictId { get; set; }
        public int? GuarantorPermanentThanaId { get; set; }
        public int? GuarantorPermanentUnionId { get; set; }
        public string GuarantorPermanentStreetOrHouse { get; set; }
        public string GuarantorPermanentZipCode { get; set; }
        public string EmployeePin { get; set; }
        public int CertificateId { get; set; }
        public string Memo { get; set; }
        public int? NoOfCopies { get; set; }
        public string Status { get; set; }
        public string EmployeeCertificateStatus { get; set; }
        public IEnumerable<SelectListItem> EmployeeCertificateStatusList { get; set; }
        public string DegreeName { get; set; }
        public string SD { get; set; }

        [Display(Name = "Date")]
        public string CertificateStatusDate { get; set; }
        public string Comment { get; set; }
        public string DegreeCode { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        //model for Employee Training 
        public int EmployeeTrainingId { get; set; }
        public string TrainingTitle { get; set; }
        public string InstituteName { get; set; }
        public int? TrainingCountryId { get; set; }
        public string TrainingTopics { get; set; }
        public string Result { get; set; }
        public DateTime? TrainingDateFrom { get; set; }
        public DateTime? TrainingDateTo { get; set; }
        public string CurrentOfficeTraining { get; set; }
        public DateTime? ApproveAndRejectionDate { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
        public long? approveby { get; set; }
        public DateTime? InActiveDate { get; set; }
        public IEnumerable<SelectListItem> CurrentOfficeTrainingList { get; set; }
        public IEnumerable<SelectListItem> isapprovedList { get; set; }
        public IEnumerable<SelectListItem> isrejectedList { get; set; }
        public string Empsearch { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }

        // employee training end

        public int EmpOfficeVisitId { get; set; }
        public string VisitType { get; set; }
        public string Location { get; set; }
        public string Reason { get; set; }
        public int CurrentOfficeProvided { get; set; }
        public List<SelectListItem> VisitTypeList { get; set; }
        public List<SelectListItem> OfficeProvidedList { get; set; }
        public List<string> typeFilterColumn { get; set; }

        public int? EmployeeTypeId { get; set; }
        public string EmployeeStatusName { get; set; }
        public string EmployeeStatusValue { get; set; }



        // Employee Qualities

        public int EmployeeQualitiesId { get; set; }
        public int? PromotionEvaluationId { get; set; }
        public int PersonalQualificationPromotionId { get; set; }
        public string Marks { get; set; }
        public List<SelectListItem> MarksList { get; set; }
        //public List<SelectListItem> PersonalQualificationforPromotionList { get; set; }

        //public int EmployeeTypeId { get; set; }
        //public decimal TotalEarnings { get; set; }

        ////Employee promotion evaluation
        [Display(Name = "Next Review Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NextReviewDate { get; set; }

        public DateTime EvaluationDateFrom { get; set; }
        public DateTime EvaluationDateTo { get; set; }
        public string EvaluationCategory { get; set; }
        public List<SelectListItem> CategoryList { get; set; }

        public int WorkExpId { get; set; }

        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        // attendance roster
        public int TimeKeepingRosterId { get; set; }
        public string RosterName { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LastLoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public List<SelectListItem> AttendanceRosterList { get; set; }
        public int EmployeeRosterScheduleId { get; set; }

        [Display(Name = "Salary Type")]
        public List<SelectListItem> EmployeeSalaryType { get; set; }
        public List<SelectListItem> SignatureDesignationList { get; set; }

        public string Section { get; set; }

        public int? SectionId { get; set; }

        public List<SelectListItem> SectionList { get; set; }
        public List<SelectListItem> ResultDivisionList { get; set; }
        public List<SelectListItem> EmploymentTypeList { get; set; }
        [Display(Name = "Same As Present Address")]
        public string IsSameAsPresentAddress { get; set; }
        public List<SelectListItem> SameAddressList { get; set; }

        public int TotalWorkingDays { get; set; }
        public int TotalPresent { get; set; }
        public int LatePresent { get; set; }
        public string AttendanceDateFrom { get; set; }
        public string AttendanceDateTo { get; set; }

        [Display(Name = "PABX Extension")]
        //public int? PABXExtension { get; set; }
        public string PABXExtension { get; set; }


        [Display(Name = "Address Detail")]
        public string AddressDetail { get; set; }
        public string PresentAddressDetailForGuarantor { get; set; }
        public string PermanentAddressDetailForGuarantor { get; set; }
        //  public string Image64 { get { return EmployeeImage != null ? Convert.ToBase64String(EmployeeImage) : null; } }
        public string EmployeeImageBase64 { get; set; }
        public string ImageFilePath { get; set; }
        public string EmployeeImageLink { get; set; }
        public string ReferenceORGuarantorDetail { get; set; }
        public double GuaranteeMoney { get; set; }
        public int Id { get; set; }
        public string BankAccountNo { get; set; }
        public int ValidApproverCount { get; set; }
        public bool IfBasicDataPopulated { get; set; }
        public bool IfEducationDataPopulated { get; set; }
        public bool IfSupervisorDataPopulated { get; set; }
        public bool IfAddressDataPopulated { get; set; }
        public bool IfFamilyDataPopulated { get; set; }
        public bool IfEmergencyContactDataPopulated { get; set; }
        public bool IfMedicalDataPopulated { get; set; }
        public bool IfDocumentDataPopulated { get; set; }
        public bool IfGuarantorDataPopulated { get; set; }
        public bool IfCertificateDataPopulated { get; set; }
        public bool IfTrainingDataPopulated { get; set; }
        public bool IfOfficeVisitDataPopulated { get; set; }
        public bool IfRelationDataPopulated { get; set; }
        public bool IfWorkExperienceDataPopulated { get; set; }
        public bool IfPrevExperienceDataPopulated { get; set; }
        public bool IfAttendanceDataPopulated { get; set; }
        public bool IfPublicationDataPopulated { get; set; }
        public bool IfLanguageDataPopulated { get; set; }
        public bool IfInterOrgRelationDataPopulated { get; set; }
        [Display(Name = "Changing Status Remarks")]
        public string StatusChangeComment { get; set; }
        public DateTime? AgreementFromDate { get; set; }
        [Display(Name = "Agreement To Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? AgreementToDate { get; set; }

        public int rowSl { get; set; }
        public string BirthCertificateNo { get; set; }

    }
}