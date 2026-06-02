using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class EmployeeOtherInformationViewModel
    {
        //visit information starts//
        public int EmpOfficeVisitId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string EmployeeName { get; set; }
        public string VisitType { get; set; }
        public string Location { get; set; }
        public string Reason { get; set; }
        public string CurrentOfficeProvided { get; set; }
        public int CurrentOfficeProvidedVal { get; set; }
        public List<SelectListItem> VisitTypeList { get; set; }
        public List<SelectListItem> OfficeProvidedList { get; set; }


        //visit information ends//


        // Relation starts//
        public int LinkId { get; set; }
        public string OrganizationCode { get; set; }
        public List<SelectListItem> OrganizationList { get; set; }
        public string RelativeEmployeeCode { get; set; }
        public string RelativeEmployeeName { get; set; }
        public string RelativeDepartmentName { get; set; }
        public string RelativeDesignationName { get; set; }
        public string Relation { get; set; }
        public List<SelectListItem> RelationshipList { get; set; }

        // Relation ends//

        // Experience starts//

        public int WorkExpId { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime ReleaseDate { get; set; }

        public string JoiningDateView { get; set; }
        public string ReleaseDateView { get; set; }    

        // Experience ends//


        //Family info starts//

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
        public DateTime FamilyMemberDateOfBirth { get; set; }
        public string FamilyMemberDateOfBirthShow { get; set; }

        [Display(Name = "Occupation (পেশা)")]
        public string FamilyMemberOccupation { get; set; }
        [Display(Name = "Educatiuonal Qualification (শিক্ষাগত যোগ্যতা)")]
        public string EducationalQualification { get; set; }

        public List<SelectListItem> GenderList { get; set; }


        public IEnumerable<SelectListItem> relationWithEmployeeList { get; set; }
        public IEnumerable<SelectListItem> languageList { get; set; }
        public IEnumerable<SelectListItem> efficiencyList { get; set; }
        [Display(Name = "Efficiency (দক্ষতা)")]
        public string Efficiency { get; set; }
        [Display(Name = "Computer Efficiency (কম্পিউটার দক্ষতা)")]
        public string ComputerEfficiency { get; set; }
        public string RelationWithFamilyMemberId { get; set; }
        public string FamilyMemberGenderId { get; set; }
        public int Id { get; set; }



        //Family info ends//

        //model for Employee Training 
        public int EmployeeTrainingId { get; set; }
        public string TrainingTitle { get; set; }
        public string InstituteName { get; set; }
        public int? TrainingCountryId { get; set; }
        public string TrainingTopics { get; set; }
        public string Result { get; set; }

        [Display(Name ="Training From")]
        public DateTime? TrainingDateFrom { get; set; }

        [Display(Name = "Training To")]
        public DateTime? TrainingDateTo { get; set; }

        public string CurrentOfficeTraining { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
        public long? approveby { get; set; }
        public DateTime? ApproveAndRejectionDate { get; set; }
        public DateTime? InActiveDate { get; set; }
        public IEnumerable<SelectListItem> CurrentOfficeTrainingList { get; set; }
        public IEnumerable<SelectListItem> IsApprovedList { get; set; }
        public IEnumerable<SelectListItem> IsRejectedList { get; set; }
        public string Empsearch { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
        public string SupportedBy { get; set; }
        public string OrganisedBy { get; set; }
        public int EmployeeTrainingDropDownId { get; set; }
        public List<SelectListItem> EmployeeTranningDropDownList { get; set; }


        public int InstituteNameDropDownId { get; set; }
        public List<SelectListItem> InstituteNameDropDownList { get; set; }

        // employee training end

        public int MaritalId { get; set; }
        public string MaritalStatus { get; set; }

        public List<SelectListItem> MaritalList { get; set; }

        public string VisitStatus { get; set; }
        public string RelativeStatus { get; set; }
        public string ExperienceStatus { get; set; }
        public string FamilyInfoStatus { get; set; }
        public string StatusApprovalOrRejection { get; set; }

        //previous work experience

        public int OrgId { get; set; }
        public string OrganizationName { get; set; }
        public string PreviousDepartment { get; set; }
        public string PreviousDesignation { get; set; }
        public DateTime PrevJoiningDate { get; set; }
        public string PrevJoiningDateView { get; set; }
        public DateTime PrevReleaseDate { get; set; }
        public string PrevReleaseDateView { get; set; }
        public int ExperienceYear { get; set; }
        public int ExperienceMonth { get; set; }

        public string PreSupervisorName { get; set; }
        public string PreSupervisorMobileNo { get; set; }
        public string PreLeaveReason { get; set; }

        public int PublicationId { get; set; }
        public string PublicationName { get; set; }
        public string PublicationDetail { get; set; }
        public string FirstJoiningDate { get; set; }
        public string JoiningDesignation { get; set; }
        public decimal JoiningSalary { get; set; }
        public decimal CurrentSalary { get; set; }
        public string PreviousExperience { get; set; }
        public string CurrentOfficeExperience { get; set; }
        public string TotalExperience { get; set; }
        public int PreviousDesignationId { get; set; }
        public string PreviousDesignationName { get; set; }
        public int NewDesignationId { get; set; }
        public string NewDesignationName { get; set; }
        public string PromotionDate { get; set; }
        public string DesignationName { get; set; }
        public string EffectiveDateFrom { get; set; }
        public string EffectiveDateTo { get; set; }
        public double GrossSalary { get; set; }
        public string DateOfIncrementSalary { get; set; }
        public double AmountOfIncrementedSalary { get; set; }
        public string PercentageOfIncrementedSalary { get; set; }

        // starts relation with self organization employee

        public int SelfOrgRelationId { get; set; }
        public int COEDesignationId { get; set; }
        public string COEDesignationName { get; set; }
        public int COEDepartmentId { get; set; }
        public string COEDepartmentName { get; set; }
        public string COEmployeeName { get; set; }
        public int COERelationId { get; set; }
        public string COERelationName { get; set; }


        [Display(Name = "Office Type (অফিসের ধরণ)")]
        public int OfficeTypeId { get; set; }
        [Display(Name = "Head Office (প্রধান কার্য্যালয়)")]
        public string PVHeadOfficeId { get; set; }
        [Display(Name = "Project Office (প্রোজেক্ট অফিস)")]
        public string PVProjectId { get; set; }

        public string DepartmentId { get; set; }
        [Display(Name = "Zone Name (যোনের নাম)")]
        public string ZoneId { get; set; }
        [Display(Name = "Area Name (এরিয়ার নাম)")]
        public string AreaId { get; set; }
        [Display(Name = "Unit Name (ইউনিটের নাম)")]//"Branch Name (শাখার নাম)"
        public string UnitId { get; set; }
        public int ParentOfficeId { get; set; }

        public int? HeadOfficeId { get; set; }
        public int? ProjectId { get; set; }
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> DesignationList { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> EmployeeList { get; set; }

        // ends relation with self organization employee
        public string ZoneOfficeName { get; set; }
        public string AreaOfficeName { get; set; }
        public string UnitOfficeName { get; set; }
    }
}