using AutoMapper;
using gHRM.Web.ViewModels;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Apply;
using gHRM.Data.DBDetailModels;
using gHRM.Web.ViewModels.Payroll;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Web.ViewModels.payroll;
using gHRM.Web.ViewModels.Discipline;
using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Web.ViewModels.WelfareFund.StaffWelfareFundSettings;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using gHRM.Web.ViewModels.WelfareFund.StaffWelfareFundConfiguration;
using gHRM.Web.ViewModels.WelfareFund.HealthWelfareFundSetting;
using gHRM.Data.CodeFirstMigration.HealthWelfareFund;
using gHRM.Web.ViewModels.WelfareFund.HealthWelfareFundConfiguration;
using gHRM.Data.CodeFirstMigration.Cooperative;
using gHRM.Web.ViewModels.Cooperative;
using gHRM.Web.ViewModels.Employee;
using gHRM.Data.DBDetailModels.Security;
using gHRM.Data;
using gHRM.Web.ViewModels.TaDa;
using gHRM.Data.CodeFirstMigration.TaDa;
using gHRM.Web.ViewModels.Apply;
using gHRM.Web.ViewModels.Loan;
using gHRM.Data.CodeFirstMigration.Loan;
using gHRM.Web.ViewModels.FeedBack;
using gHRM.Web.ViewModels.IncomeTax;

namespace gHRM.Web.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }
        protected override void Configure()
        {

            Mapper.CreateMap<SSOUserRegistrationModel, ApplicationUser>();
            Mapper.CreateMap<ApplicationUser, SSOUserRegistrationModel>();

            Mapper.CreateMap<SSOUserRegistrationModel, AspDotNetUserModel>();
            Mapper.CreateMap<AspDotNetUserModel, SSOUserRegistrationModel>();

            Mapper.CreateMap<ThanaViewModel, LgThana>();
            Mapper.CreateMap<EmployeeViewModel, Employee>();
            Mapper.CreateMap<EmployeeDepartmentViewModel, EmployeeDepartment>();
            Mapper.CreateMap<gHRM.Web.ViewModels.EmployeeDesignationViewModel, EmployeeDesignation>();
            Mapper.CreateMap<CountryViewModel, Country>();
            Mapper.CreateMap<StateOrProvinceViewModel, StateOrProvince>();
            Mapper.CreateMap<DistrictViewModel, District>();
           
            Mapper.CreateMap<BranchViewModel, Branch>();
            Mapper.CreateMap<ThanaViewModel, LgThana>();
            Mapper.CreateMap<CompanyViewModel, Company>();
            Mapper.CreateMap<LeaveTypeViewModel, LeaveType>();
            Mapper.CreateMap<LeaveSellViewModel, LeaveSell>();
            Mapper.CreateMap<LeaveHistoryViewModel, LeaveHistory>();
            Mapper.CreateMap<DiscCrimeViewModel, DiscCrime>();
            Mapper.CreateMap<DiscPunishmentViewModel, DiscPunishment>();
            Mapper.CreateMap<OfficeViewModel, Office>();
            Mapper.CreateMap<AspNetRoleModuleViewModel, AspNetRoleModule>();
            Mapper.CreateMap<AspNetSecurityModuleViewModel, AspNetSecurityModule>();
            
            Mapper.CreateMap<ApplicationSettingViewModel, ApplicationSetting>();
            Mapper.CreateMap<ApplicationSettingViewModel, DBApplicationSettingsDetail>();
           
            Mapper.CreateMap<EmployeeOfficeDesignationViewModel, EmployeeOfficeDesignation>();
          
            Mapper.CreateMap<CaseEntryViewModel, DiscCaseMaster>();
            Mapper.CreateMap<DiscCaseStatusViewModel, DiscCaseStatu>();
                       
            Mapper.CreateMap<NomineeViewModel, EmployeeNominee>();
            
            Mapper.CreateMap<DiscEmbezzleInfoViewModel, DiscEmbezzleInfo>();
            Mapper.CreateMap<DiscCaseEnquiryOfficerViewModel, DiscCaseEnquiryOfficer>();
                        
            Mapper.CreateMap<Att_OfficeMachineViewModel, AttOfficeMachine>();
            Mapper.CreateMap<SalaryDateConfigViewModel, SalaryDateConfig>();
            
            Mapper.CreateMap<PRComponentViewModel, PRComponent>();           
            
            Mapper.CreateMap<AttCardIssueViewModel, AttCardIssue>();   

            //AttHolidayDeclaration
            Mapper.CreateMap<AttHolidayDeclarationViewModel, AttHolidayDeclaration>();
           
            Mapper.CreateMap<DiscDealingOfficerViewModel, DiscDealingOfficer>();
            
            Mapper.CreateMap<OvertimeConfigurationViewModel, OvertimeConfiguration>();
            Mapper.CreateMap<OvertimeHourEmployeeViewModel, OvertimeHourEmployee>();
            Mapper.CreateMap<OvertimeHourEmployeeViewModel, OvertimeHourEmployeeApproved>();
            Mapper.CreateMap<OvertimeHourEmployee, OvertimeHourEmployeeApproved>();
            Mapper.CreateMap<EmployeeStatusViewModel, EmployeeStatus>();
            Mapper.CreateMap<EducationDegreeViewModel, EducationDegree>();
            Mapper.CreateMap<EducationConcentrationViewModel, EducationConcentration>();
            Mapper.CreateMap<InternalOrganizationViewModel, InternalOrganization>();

            //fund
            Mapper.CreateMap<FundSetupViewModel, FundSetup>();
            Mapper.CreateMap<HealthFundingViewModel, HealthFunding>();

            //Welfare Fund
            Mapper.CreateMap<StaffWelfareFundSettingViewModel, StaffWelfareFundSetting>();
            Mapper.CreateMap<StaffWelfareFundConfigurationViewModel, StaffWelfareFundConfiguration>();

            //Starff Welfare Fund
            Mapper.CreateMap<HealthWelfareFundSettingViewModel, HealthWelfareFundSetting>();
            Mapper.CreateMap<HealthWelfareFundConfigurationViewModel, HealthWelfareFundConfiguration>();

            //Cooperative Configuration
            Mapper.CreateMap<CooperativeConfigurationViewModel, CooperativeConfiguration>();
            Mapper.CreateMap<EmployeeAddressViewModel, EmployeeAddress>();
            Mapper.CreateMap<EmployeeFamilyInfoViewModel,EmployeeFamilyInfo>();
            Mapper.CreateMap<EmployeeTranningDropDownViewModel, EmployeeTranningDropDown>();

            Mapper.CreateMap<OvertimeExceptionViewModel, OvertimeException>();
            Mapper.CreateMap<TADAPurposeViewModel, TADAPurpose>();
            Mapper.CreateMap<UnionViewModel, LgUnion>();

            // Applicant 
            Mapper.CreateMap<ApplicantMasterViewModel, ApplicantMaster>();
            Mapper.CreateMap<ApplicantJobExperienceViewModel, ApplicantJobExperience>();
            Mapper.CreateMap<ApplicantAccademicViewModel, ApplicantAccademic>();

            Mapper.CreateMap<ApplicantTrainingInfoViewModel, ApplicantTrainingInfo>();
            Mapper.CreateMap<ApplicantReferenceInfoViewModel, ApplicantReferenceInfo>();

            Mapper.CreateMap<JobsCircularViewModel, JobsCircular>();

            // Loan
            Mapper.CreateMap<CollectionMethodViewModel, CollectionMethod>();
            Mapper.CreateMap<LoanEligibilityViewModel, LoanEligibility>();
            Mapper.CreateMap<LoanPurposeViewModel, LoanPurpose>();
            Mapper.CreateMap<ApplicantInfoViewModel, ApplicantInfo>();
            Mapper.CreateMap<ApplicantInfoViewModel3, ApplicantInfo2>();

            Mapper.CreateMap<FeedbackRegisterViewModel, FeedbackRegister>();
            Mapper.CreateMap<PRComponentViewModel_designation, PRComponent_designation>();
            Mapper.CreateMap<IncomeTaxViewModel, IncomeTax>().ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}