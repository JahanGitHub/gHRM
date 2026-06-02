using System;

using System.ComponentModel.DataAnnotations;


namespace gHRM.Web.ViewModels
{
    public class ApplicantMasterViewModel :BaseModel
    {
        [Display(Name = "ID")]
        [Required(ErrorMessage = "{0} is Required")]
        public Int64 ID { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string LastName { get; set; }

        [Display(Name = "Father Name")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string FatherName { get; set; }

        [Display(Name = "Mother Name")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string MotherName { get; set; }

        [Display(Name = "Guardian Name")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string GuardianName { get; set; }

        [Display(Name = "Date of Birth")]
        public string BirthDateMsg { get; set; }

        [Display(Name = "Gender")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Gender { get; set; }

        [Display(Name = "Religion")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Religion { get; set; }

        [Display(Name = "Marital Status")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string MaritalStatus { get; set; }

        [Display(Name = "Nationality")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Nationality { get; set; }

        [Display(Name = "National Id")]
        public decimal? NationalId { get; set; }

        [Display(Name = "Passport Number")]
        public decimal? PassportNumber { get; set; }

        [Display(Name = "Passport Issue Date")]
        public DateTime? PassportIssueDate { get; set; }

        [Display(Name = "Primary Mobile Number")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string PrimaryMobile { get; set; }

        [Display(Name = "Secondary Mobile Number")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string SecondaryMobile { get; set; }

        [Display(Name = "Primary Email")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string PrimaryEmail { get; set; }

        [Display(Name = "Blood Group")]
        [StringLength(5, ErrorMessage = "Maximum length is {1}")]
        public string BloodGroup { get; set; }

        [Display(Name = "Career Objective")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string CareerObjective { get; set; }

        [Display(Name = "Present Salary")]
        public decimal? PresentSalary { get; set; }

        [Display(Name = "Expected Salary")]
        public decimal? ExpectedSalary { get; set; }

        [Display(Name = "Lookingfor Job_ Level")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string LookingforJob_Level { get; set; }

        [Display(Name = "Availablefor")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string Availablefor { get; set; }

        [Display(Name = "Career Summary")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string CareerSummary { get; set; }

        [Display(Name = "Special Qualification")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string SpecialQualification { get; set; }

        [Display(Name = "Image")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string Image { get; set; }

        [Display(Name = "Qualification Keyword")]
        [StringLength(255, ErrorMessage = "Maximum length is {1}")]
        public string QualificationKeyword { get; set; }

        [Display(Name = "Kinds Of Disability")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string KindsOfDisability { get; set; }

        [Display(Name = "Disability Id")]
        public decimal? DisabilityId { get; set; }
        public Int64? UserId { get; set; }

        public byte[] ImageByte { get; set; }

        [Display(Name = "Present Address")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string PresentAddress { get; set; }

        [Display(Name = "Permanent Address like Vill:, PS:")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string PermanentAddress { get; set; }

        [Display(Name = "Cover Letter:")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string CoverLetter { get; set; }

        public string AttachedCV { get; set; }
    }
}