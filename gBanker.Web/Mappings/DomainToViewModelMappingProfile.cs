using AutoMapper;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Apply;
using gHRM.Data.CodeFirstMigration.Basic;
using gHRM.Data.CodeFirstMigration.Cooperative;
using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.HealthWelfareFund;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.CodeFirstMigration.PerformanceEvaluations;
using gHRM.Data.CodeFirstMigration.TaDa;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using gHRM.Data.DBDetailModels;
using gHRM.Web.ViewModels;
using gHRM.Web.ViewModels.Apply;
using gHRM.Web.ViewModels.Cooperative;
using gHRM.Web.ViewModels.Discipline;
using gHRM.Web.ViewModels.Employee;
using gHRM.Web.ViewModels.Payroll;
using gHRM.Web.ViewModels.PerformanceEvaluations;
using gHRM.Web.ViewModels.TaDa;
using gHRM.Web.ViewModels.WelfareFund.HealthWelfareFundConfiguration;
using gHRM.Web.ViewModels.WelfareFund.HealthWelfareFundSetting;
using gHRM.Web.ViewModels.WelfareFund.StaffWelfareFundConfiguration;
using gHRM.Web.ViewModels.WelfareFund.StaffWelfareFundSettings;
using gHRM.Web.ViewModels.Loan;
using gHRM.Data.CodeFirstMigration.Loan;
using gHRM.Web.ViewModels.Basic;
using gHRM.Web.ViewModels.FeedBack;

namespace gHRM.Web.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Employee, EmployeeViewModel>();
            Mapper.CreateMap<EmployeeDepartment, EmployeeDepartmentViewModel>();
            Mapper.CreateMap<Country, CountryViewModel>();
            Mapper.CreateMap<EmployeeDesignation, gHRM.Web.ViewModels.EmployeeDesignationViewModel>();
            Mapper.CreateMap<StateOrProvince, StateOrProvinceViewModel>();
            Mapper.CreateMap<District, DistrictViewModel>();
            Mapper.CreateMap<Branch, BranchViewModel>();
            Mapper.CreateMap<LgThana, ThanaViewModel>();
            Mapper.CreateMap<Company, CompanyViewModel>();
            Mapper.CreateMap<Office, OfficeViewModel>();
            Mapper.CreateMap<LeaveType, LeaveTypeViewModel>();
            Mapper.CreateMap<LeaveSell, LeaveSellViewModel>();
            Mapper.CreateMap<LeaveHistory, LeaveHistoryViewModel>();
            Mapper.CreateMap<DiscCrime, DiscCrimeViewModel>();
            Mapper.CreateMap<DiscPunishment, DiscPunishmentViewModel>();

            Mapper.CreateMap<AspNetRoleModule, AspNetRoleModuleViewModel>();
            Mapper.CreateMap<AspNetSecurityModule, AspNetSecurityModuleViewModel>();
            Mapper.CreateMap<ApplicationSetting, ApplicationSettingViewModel>();
            Mapper.CreateMap<DBApplicationSettingsDetail, ApplicationSettingViewModel>();

            Mapper.CreateMap<EmployeeOfficeDesignation, EmployeeOfficeDesignationViewModel>();

            Mapper.CreateMap<DiscCaseMaster, CaseEntryViewModel>();
            Mapper.CreateMap<DiscCaseStatu, DiscCaseStatusViewModel>();

            Mapper.CreateMap<NomineeViewModel, NomineeDetailViewModel>();

            Mapper.CreateMap<DiscEmbezzleInfo, DiscEmbezzleInfoViewModel>();
            Mapper.CreateMap<DiscCaseEnquiryOfficer, DiscCaseEnquiryOfficerViewModel>();

            Mapper.CreateMap<AttOfficeMachine, Att_OfficeMachineViewModel>();

            Mapper.CreateMap<PRComponent, PRComponentViewModel>();

            Mapper.CreateMap<AttCardIssue, AttCardIssueViewModel>();
            Mapper.CreateMap<SalaryDateConfig, SalaryDateConfigViewModel>();

            Mapper.CreateMap<NoticeViewModel, Notice>();
            Mapper.CreateMap<Notice, NoticeViewModel>();


            //AttHolidayDeclaration
            Mapper.CreateMap<AttHolidayDeclaration, AttHolidayDeclarationViewModel>();

            Mapper.CreateMap<DiscDealingOfficer, DiscDealingOfficerViewModel>();

            Mapper.CreateMap<OvertimeConfiguration, OvertimeConfigurationViewModel>();
            Mapper.CreateMap<OvertimeHourEmployee, OvertimeHourEmployeeViewModel>();
            Mapper.CreateMap<OvertimeHourEmployeeApproved, OvertimeHourEmployeeViewModel>();
            Mapper.CreateMap<OvertimeHourEmployeeApproved, OvertimeHourEmployee>();
            Mapper.CreateMap<EmployeeStatus, EmployeeStatusViewModel>();
            Mapper.CreateMap<EducationDegree, EducationDegreeViewModel>();
            Mapper.CreateMap<EducationConcentration, EducationConcentrationViewModel>();
            Mapper.CreateMap<InternalOrganization, InternalOrganizationViewModel>();

            //Welfare Fund
            Mapper.CreateMap<StaffWelfareFundSetting, StaffWelfareFundSettingViewModel>();
            Mapper.CreateMap<StaffWelfareFundConfiguration, StaffWelfareFundConfigurationViewModel>();
            //Health Welfare Fund
            Mapper.CreateMap<HealthWelfareFundSetting, HealthWelfareFundSettingViewModel>();
            Mapper.CreateMap<HealthWelfareFundConfiguration, HealthWelfareFundConfigurationViewModel>();

            //Cooperative Configuration
            Mapper.CreateMap<CooperativeConfiguration, CooperativeConfigurationViewModel>();
            Mapper.CreateMap<EmployeeAddress, EmployeeAddressViewModel>();

            Mapper.CreateMap<EmployeeFamilyInfo, EmployeeFamilyInfoViewModel>();
            Mapper.CreateMap<EmployeeTranningDropDown, EmployeeTranningDropDownViewModel>();

            //Performance Evaluation
            Mapper.CreateMap<PerformanceEvaluation, AddOrEditPerformanceEvaluationViewModel>();
            Mapper.CreateMap<AddOrEditPerformanceEvaluationViewModel, PerformanceEvaluation>();

            //Company Wise Payroll Config
            Mapper.CreateMap<CompanyWisePayrollConfig, AddOrEditCompanyWisePayrollConfigViewModel>();
            Mapper.CreateMap<AddOrEditCompanyWisePayrollConfigViewModel, CompanyWisePayrollConfig>();

            //PR Deposit
            Mapper.CreateMap<PRDeposit, PRDepositViewModel>();
            Mapper.CreateMap<PRDepositViewModel, PRDeposit>();

            Mapper.CreateMap<OvertimeException, OvertimeExceptionViewModel>();
            Mapper.CreateMap<TADAPurpose, TADAPurposeViewModel>();
            Mapper.CreateMap<LgUnion, UnionViewModel>();

            // Applicant   ApplicantMaster

            Mapper.CreateMap<ApplicantMaster, ApplicantMasterViewModel>();
            Mapper.CreateMap<ApplicantJobExperience, ApplicantJobExperienceViewModel>();
            Mapper.CreateMap<ApplicantAccademic, ApplicantAccademicViewModel>();

            Mapper.CreateMap<ApplicantTrainingInfo, ApplicantTrainingInfoViewModel>();
            Mapper.CreateMap<ApplicantReferenceInfo, ApplicantReferenceInfoViewModel>();

            Mapper.CreateMap<JobsCircular, JobsCircularViewModel>();

            // Loan
            Mapper.CreateMap<CollectionMethod, CollectionMethodViewModel>();
            Mapper.CreateMap<LoanEligibility, LoanEligibilityViewModel>();
            Mapper.CreateMap<LoanPurpose, LoanPurposeViewModel>();
            Mapper.CreateMap<ApplicantInfo, ApplicantInfoViewModel>();
            Mapper.CreateMap<FeedbackRegister, FeedbackRegisterViewModel>();

        }
    }
}