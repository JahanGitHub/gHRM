
using gHRM.Data.CodeFirstMigration.Basic;
using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.Loan;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Data.CodeFirstMigration.TaDa;
using gHRM.Data.CodeFirstMigration.ViewsEmployee;
using System.Data.Entity;
using gHRM.Data.CodeFirstMigration.Promotion;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using gHRM.Data.CodeFirstMigration.HealthWelfareFund;
using gHRM.Data.CodeFirstMigration.Cooperative;
using gHRM.Data.CodeFirstMigration.PerformanceEvaluations;
using gHRM.Data.CodeFirstMigration.eRecruit;
using gHRM.Data.CodeFirstMigration.Apply;
using System.Data.Entity.Infrastructure;
using gHRM.Data.CodeFirstMigration;


namespace gHRM.Data.CodeFirstMigration
{

    public partial class gHRMDBContext : DbContext
    {
        public gHRMDBContext() : base("name=gHRMDbContext")
        {

        }

        #region BASIC SETTINGS

        public virtual DbSet<BankAccount> BankAccount { get; set; }
        public virtual DbSet<BankName> BankName { get; set; }
        public virtual DbSet<BankBranch> BankBranch { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<LgThana> LgThanas { get; set; }
        public virtual DbSet<LgUnion> LgUnions { get; set; }
        public virtual DbSet<StateOrProvince> StateOrProvinces { get; set; }
        public virtual DbSet<OfficeType> OfficeTypes { get; set; }
        public virtual DbSet<Office> Offices { get; set; }
        public virtual DbSet<OfficeRegion> OfficeRegions { get; set; }
        public virtual DbSet<OfficeRegionMapping> OfficeRegionMappings { get; set; }
        public virtual DbSet<OfficeLocation> OfficeLocation { get; set; }
        public virtual DbSet<OfficeDesignation> OfficeDesignations { get; set; }
        public virtual DbSet<EducationDegree> EducationDegree { get; set; }
        public virtual DbSet<EducationConcentration> EducationConcentration { get; set; }
        public virtual DbSet<AttHolidayType> AttHolidayType { get; set; }
        public virtual DbSet<CarRecognition> CarRecognition { get; set; }
        public virtual DbSet<CarRecognitionApproval> CarRecognitionApproval { get; set; }
        public virtual DbSet<CarRecognitionApprovedHistory> CarRecognitionApprovedHistory { get; set; }
        public virtual DbSet<ReportSignature> ReportSignatures { get; set; }


        public virtual DbSet<EmployeeAllowence> EmployeeAllowence { get; set; }
        public virtual DbSet<EmployeeTypeConfiguration> EmployeeTypeConfigurations { get; set; }

        public virtual DbSet<Notice> Notice { get; set; }

        #endregion

        #region SECURITY

        public virtual DbSet<ApplicationLog> ApplicationLogs { get; set; }
        public virtual DbSet<ApplicationSetting> ApplicationSettings { get; set; }
        public virtual DbSet<AspAdminPasswordTable> AspAdminPasswordTables { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleModule> AspNetRoleModules { get; set; }
        public virtual DbSet<AspNetSecurityLevel> AspNetSecurityLevels { get; set; }
        public virtual DbSet<AspNetSecurityModule> AspNetSecurityModules { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<SSORoleMapping> SSORoleMappings { get; set; }
       

        #endregion

        #region EMPLOYEE

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeShortInfo> EmployeeShortInfo { get; set; }
        public virtual DbSet<EmployeeStatus> EmployeeStatus { get; set; }
        public virtual DbSet<EmployeeStatusHistory> EmployeeStatusHistorys { get; set; }
        public virtual DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }
        public virtual DbSet<EmployeeDesignation> EmployeeDesignations { get; set; }
        public virtual DbSet<EmployeeOfficeDesignation> EmployeeOfficeDesignations { get; set; }
        public virtual DbSet<EmployeeAddress> EmployeeAddresses { get; set; }
        public virtual DbSet<EmployeeEducation> EmployeeEducations { get; set; }
        public virtual DbSet<EmployeeFamilyInfo> EmployeeFamilyInfoes { get; set; }
        public virtual DbSet<EmployeeReference> EmployeeReferences { get; set; }
        public virtual DbSet<EmployeeTraining> EmployeeTrainings { get; set; }
        public virtual DbSet<EmployeeOtherQualification> EmployeeOtherQualification { get; set; }
        public virtual DbSet<EmployeeEmergencyContact> EmployeeEmergencyContact { get; set; }
        public virtual DbSet<EmployeeMedicalInfo> EmployeeMedicalInfo { get; set; }
        public virtual DbSet<EmployeePreviousWorkExperience> EmployeePreviousWorkExperience { get; set; }
        public virtual DbSet<EmployeePublication> EmployeePublication { get; set; }
        public virtual DbSet<EmployeeSignatureDesignation> EmployeeSignatureDesignation { get; set; }
        public virtual DbSet<EmployeeDepartmentSection> EmployeeDepartmentSection { get; set; }
        public virtual DbSet<EmployeeSupervisor> EmployeeSupervisor { get; set; }
        public virtual DbSet<EmployeeOfficeMapping> EmployeeOfficeMappings { get; set; }
        public virtual DbSet<EmployeeFileAttachemnt> EmployeeFileAttachemnt { get; set; }
        public virtual DbSet<EmployeeReportOption> EmployeeReportOption { get; set; }
        public virtual DbSet<EmployeeReportOptionJCF> EmployeeReportOptionJCF { get; set; }
        public virtual DbSet<EmployeeDesignationMapping> EmployeeDesignationMapping { get; set; }
        public virtual DbSet<EmployeeEquivalentDesignation> EmployeeEquivalentDesignation { get; set; }
        public virtual DbSet<EmployeeGuarantorInformation> EmployeeGuarantorInformation { get; set; }
        public virtual DbSet<EmployeeGuarantorTranInformation> EmployeeGuarantorTranInformation { get; set; }
        public virtual DbSet<EmployeeOfficeVisitInformation> EmployeeOfficeVisitInformation { get; set; }
        public virtual DbSet<LinkWithEmployee> LinkWithEmployee { get; set; }
        public virtual DbSet<EmployeeGradeList> EmployeeGradeList { get; set; }
        public virtual DbSet<NomineeType> NomineeType { get; set; }
        public virtual DbSet<EmployeeNominee> EmployeeNominee { get; set; }
        public virtual DbSet<GuarantorRelationship> GuarantorRelationship { get; set; }
        public virtual DbSet<FamilyRelation> FamilyRelation { get; set; }
        public virtual DbSet<NomineeRelation> NomineeRelation { get; set; }
        public virtual DbSet<Occupation> Occupation { get; set; }
        public virtual DbSet<InternalOrganization> InternalOrganization { get; set; }
        public virtual DbSet<ReceivedCertificates> ReceivedCertificates { get; set; }
        public virtual DbSet<EmployementType> EmployementType { get; set; }
        public virtual DbSet<CurrentOrganizationRelationship> CurrentOrganizationRelationship { get; set; }
        public virtual DbSet<DocumentType> DocumentType { get; set; }
        public virtual DbSet<DocumentTypeModule> DocumentTypeModule { get; set; }
        public virtual DbSet<WorkExperienceWithInterOrganization> WorkExperienceWithInterOrganization { get; set; }
        public virtual DbSet<PanelOfficer> PanelOfficer { get; set; }
        public virtual DbSet<PanelOfficerHistory> PanelOfficerHistory { get; set; }
        public virtual DbSet<View_EmployeeTraining> View_EmployeeTraining { get; set; }
        public virtual DbSet<View_EmployeeGuarantorInformation> View_EmployeeGuarantorInformation { get; set; }
        public virtual DbSet<EmployeeTranningDropDown> EmployeeTranningDropDowns { get; set; }
        public virtual DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
        public virtual DbSet<ResignNotice> ResignNotices { get; set; }



        #endregion

        #region Approval

        public virtual DbSet<EmployeeInformationApproval> EmployeeInformationApproval { get; set; }
        public virtual DbSet<EmployeeFamilyInfoApprovalProcess> EmployeeFamilyInfoApprovalProcess { get; set; }
        public virtual DbSet<EmployeeMaritalStatusApproval> EmployeeMaritalStatusApproval { get; set; }

        //public virtual DbSet<NotificationModule> NotificationModule { get; set; }


        #endregion

        #region Transfer

        public virtual DbSet<EmployeeTransfer> EmployeeTransfer { get; set; }
        public virtual DbSet<TransferOfficeOrder> TransferOfficeOrder { get; set; }
        //public virtual DbSet<TransferJoiningApprovalAuthority> TransferJoiningApprovalAuthority { get; set; }

        #endregion

        #region LEAVE

        public virtual DbSet<ApprovalConfigDetail> ApprovalConfigDetail { get; set; }
        public virtual DbSet<ApprovalConfigMaster> ApprovalConfigMaster { get; set; }
        public virtual DbSet<ApprovalNotification> ApprovalNotification { get; set; }
        public virtual DbSet<ELEncashmentAuthority> ELEncashmentAuthority { get; set; }
        public virtual DbSet<ELEncashmentConfiguration> ELEncashmentConfiguration { get; set; }
        public virtual DbSet<LeaveAdjustmentAuthority> LeaveAdjustmentAuthority { get; set; }
        public virtual DbSet<LeaveApprovers> LeaveApprovers { get; set; }
        public virtual DbSet<LeaveApproversAuthority> LeaveApproversAuthority { get; set; }
        public virtual DbSet<LeaveApproversConfiguration> LeaveApproversConfiguration { get; set; }
        public virtual DbSet<LeaveApproversMetadata> LeaveApproversMetadata { get; set; }
        public virtual DbSet<LeaveCategory> LeaveCategory { get; set; }
        public virtual DbSet<LeaveHistory> LeaveHistories { get; set; }
        public virtual DbSet<LeaveType> LeaveTypes { get; set; }
        public virtual DbSet<LeaveSell> LeaveSells { get; set; }
        public virtual DbSet<LeaveELOpening> LeaveELOpenings { get; set; }
        public virtual DbSet<LeaveMaternityOpening> LeaveMaternityOpenings { get; set; }
        public virtual DbSet<LeaveTypeLedger> LeaveTypeLedgers { get; set; }
        public virtual DbSet<LeaveHistoryAttachment> LeaveHistoryAttachments { get; set; }
        public virtual DbSet<OutOfOffice> OutOfOffices { get; set; }


        #endregion

        #region TimeKeeping/Attendance

        public virtual DbSet<TimeKeepingRoster> TimeKeepingRoster { get; set; }
        public virtual DbSet<View_TimeKeepingRoster> View_TimeKeepingRoster { get; set; }
        public virtual DbSet<AttCardIssue> AttCardIssues { get; set; }
        public virtual DbSet<AttHolidayDeclaration> AttHolidayDeclarations { get; set; }
        public virtual DbSet<AttOfficeMachine> Att_OfficeMachines { get; set; }
        public virtual DbSet<AttAttendance> AttAttendances { get; set; }
        public virtual DbSet<AttendancePenaltyConfiguration> AttendancePenaltyConfigurations { get; set; }
        public virtual DbSet<View_TimeKeepingDetail> View_TimeKeepingDetail { get; set; }
        public virtual DbSet<EmployeeOfficeTimeException> EmployeeOfficeTimeException { get; set; }
        public virtual DbSet<EmployeeRosterSchedule> EmployeeRosterSchedule { get; set; }
        public virtual DbSet<View_EmployeeOfficeTimeException> View_EmployeeOfficeTimeException { get; set; }
        public virtual DbSet<RoasterEmployeeSchedule> RoasterEmployeeSchedules { get; set; }
        public virtual DbSet<EmployeeTimeKeepingException> EmployeeTimeKeepingException { get; set; }
        public virtual DbSet<AttAttendanceType> AttAttendanceType { get; set; }
        public virtual DbSet<View_EmployeeTimeKeepingException> View_EmployeeTimeKeepingException { get; set; }

        public virtual DbSet<TimekeepingAttendanceDevice> TimekeepingAttendanceDevices { get; set; }
        public virtual DbSet<ManualOvertimeConfiguration> ManualOvertimeConfigurations { get; set; }

        #endregion

        #region Promotion

        //public virtual DbSet<EmployeePromotion> EmployeePromotions { get; set; }
        //public virtual DbSet<EmployeePromotionOrder> EmployeePromotionOrders { get; set; }
        //public virtual DbSet<EmployeePromotionHistory> EmployeePromotionHistory { get; set; }

        //public virtual DbSet<PromotionConfig> PromotionConfigs { get; set; }
        //public virtual DbSet<PromotionPunishmentConfig> PromotionPunishmentConfigs { get; set; }

        //public virtual DbSet<PersonalQualificationforPromotion> PersonalQualificationforPromotion { get; set; }
        //public virtual DbSet<PromotionEvaluationQualitieMarking> PromotionEvaluationQualitieMarking { get; set; }
        //public virtual DbSet<View_PromotionEvaluationQualitieMarking> View_PromotionEvaluationQualitieMarking { get; set; }

        //public virtual DbSet<PromotionEvaluationCategory> PromotionEvaluationCategory { get; set; }
        public virtual DbSet<EmployeePromotionFail> EmployeePromotionFails { get; set; }

        public virtual DbSet<gHRM.Data.CodeFirstMigration.EmployeePromotion.EmployeePromotion> EmployeePromotion { get; set; }
        public virtual DbSet<PromotionType> PromotionType { get; set; }

        public virtual DbSet<PromotionConfiguredSalary> PromotionConfiguredSalaries { get; set; }

        #endregion

        #region PAYROLL

        public virtual DbSet<PRComponent> PRComponents { get; set; }
        //public virtual DbSet<PRComponent_designation> PRComponent_designations { get; set; }

        public virtual DbSet<GradeXSalaryStep> GradeXSalarySteps { get; set; }
        public virtual DbSet<PRComponentGroup> PRComponentGroup { get; set; }
        public virtual DbSet<View_PRComponentConfiguration> View_PRComponentConfiguration { get; set; }
        public virtual DbSet<ComponentPayroll> ComponentPayroll { get; set; }
        public virtual DbSet<EmployeeMonthlySalaryApproved> EmployeeMonthlySalaryApproved { get; set; }
        public virtual DbSet<EmployeeMonthlySalary> EmployeeMonthlySalary { get; set; }
        public virtual DbSet<EmployeeSalaryConfigurationHistory> EmployeeSalaryConfigurationHistory { get; set; }
        public virtual DbSet<EmployeeSalaryDeduction> EmployeeSalaryDeduction { get; set; }
        public virtual DbSet<PRDeposit> PRDeposit { get; set; }
        public virtual DbSet<EmployeeSalaryDeposit> EmployeeSalaryDeposit { get; set; }
        public virtual DbSet<EmployeeSalaryIncentive> EmployeeSalaryIncentive { get; set; }
        public virtual DbSet<ProductGroup> ProductGroup { get; set; }
        public virtual DbSet<ProductItem> ProductItem { get; set; }
        public virtual DbSet<EmployeeLoanRegister> EmployeeLoanRegister { get; set; }
        public virtual DbSet<EmployeeLoanInstallmentDetail> EmployeeLoanInstallmentDetail { get; set; }
        public virtual DbSet<LoanInstallmentDetail> LoanInstallmentDetail { get; set; }
        public virtual DbSet<PRSalaryConfiguration> PRSalaryConfigurations { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }

        public virtual DbSet<SalaryGenerationLog> SalaryGenerationLog { get; set; }

        public virtual DbSet<PRSalaryRegister> PRSalaryRegister { get; set; }

        public virtual DbSet<EmployeeSalaryBonus> EmployeeSalaryBonus { get; set; }

        public virtual DbSet<View_EmployeeMonthlySalary> View_EmployeeMonthlySalary { get; set; }
        public virtual DbSet<View_EmployeeSalaryConfiguration> View_EmployeeSalaryConfiguration { get; set; }

        public virtual DbSet<EmployeeMonthlySalaryException> EmployeeMonthlySalaryException { get; set; }

        public virtual DbSet<FestivalBonusCalendar> FestivalBonusCalendar { get; set; }
        public virtual DbSet<SalaryDateConfig> SalaryDateConfigs { get; set; }
        public virtual DbSet<CompanyWisePayrollConfig> CompanyWisePayrollConfigs { get; set; }
        public virtual DbSet<OvertimeException> OvertimeExceptions { get; set; }
        public virtual DbSet<NoticePayConfig> NoticePayConfigs { get; set; }
        public virtual DbSet<EmployeeNoticePay> EmployeeNoticePays { get; set; }

        #endregion

        #region  Overtime

        public virtual DbSet<OvertimeHourEmployee> OvertimeHourEmployee { get; set; }
        public virtual DbSet<OvertimeHourEmployeeApproved> OvertimeHourEmployeeApproved { get; set; }
        public virtual DbSet<OvertimeConfiguration> OvertimeConfiguration { get; set; }

        #endregion

        #region TADA

        public virtual DbSet<EmployeeTADABill> EmployeeTADABill { get; set; }

        public virtual DbSet<TADAPurpose> TADAPurposes { get; set; }

        #endregion

        #region PF

        //public virtual DbQuery<CollectionTypeConfiguration> CollectionTypeConfigurations { get; set; }
        public virtual DbSet<CollectionTypeConfiguration> CollectionTypeConfigurations { get; set; }
        public virtual DbSet<AccountType> AccountType { get; set; }
        public virtual DbSet<GLLevel> GLLevel { get; set; }
        public virtual DbSet<AccountChart> AccountChart { get; set; }
        public virtual DbSet<Collection> Collection { get; set; }
       // public virtual DbSet<LoanDisbursement> LoanDisbursement { get; set; }
        public virtual DbSet<LoanType> LoanType { get; set; }
        public virtual DbSet<OfficeSetup> OfficeSetup { get; set; }
        public virtual DbSet<OrganizationSetup> OrganizationSetup { get; set; }
        public virtual DbSet<PFType> PFType { get; set; }
        public virtual DbSet<ProfitDeclaration> ProfitDeclaration { get; set; }
        public virtual DbSet<TransactionRegister> TransactionRegister { get; set; }
        public virtual DbSet<TransactionCategory> TransactionCategory { get; set; }
        public virtual DbSet<PFWithdrawan> PFWithdrawan { get; set; }
        public virtual DbSet<ProcessLog> ProcessLog { get; set; }
        public virtual DbSet<PRInstallmentProcessLog> PRInstallmentProcessLog { get; set; }
        public virtual DbSet<YearEndProcessLog> YearEndProcessLog { get; set; }
        public virtual DbSet<ProfitDistProcessLog> ProfitDistProcessLog { get; set; }
        public virtual DbSet<TempPFCollection> TempPFCollections { get; set; }
        public virtual DbSet<ContributionRegister> ContributionRegisters { get; set; }
        public virtual DbSet<OrganizationPFSetup> OrganizationPFSetups { get; set; }

        #endregion

        #region GAC

        //public virtual DbSet<AccCategory> AccCategories { get; set; }
        //public virtual DbSet<AccChart> AccCharts { get; set; }
        //public virtual DbSet<AccLastVoucher> AccLastVouchers { get; set; }
        //public virtual DbSet<AccMapping> AccMappings { get; set; }
        //public virtual DbSet<AccPaymentCategory> AccPaymentCategories { get; set; }
        //public virtual DbSet<AccProcessInfo> AccProcessInfoes { get; set; }
        //public virtual DbSet<AccTrxDetail> AccTrxDetails { get; set; }
        //public virtual DbSet<AccTrxMaster> AccTrxMasters { get; set; }
        //public virtual DbSet<AccType> AccTypes { get; set; }
        //public virtual DbSet<AccVoucherRollBack> AccVoucherRollBacks { get; set; }
        //public virtual DbSet<ZoneInfo> ZoneInfos { get; set; }
        //public virtual DbSet<AccVoucherType> AccVoucherType { get; set; }
        //public virtual DbSet<AccTrxDetail> AccTrxDetails { get; set; }
        //public virtual DbSet<AccTrxMaster> AccTrxMasters { get; set; }

        #endregion

        #region Inventory
        public virtual DbSet<Inv_CategoryOrSubCategory> InvCategoryOrSubCategory { get; set; }
        public virtual DbSet<Inv_ItemPriceDetails> InvItemPriceDetails { get; set; }
        public virtual DbSet<Inv_Items> InvItems { get; set; }
        public virtual DbSet<InvWarehouse> InvWarehouses { get; set; }
        public virtual DbSet<InvStoreItem> InvStoreItems { get; set; }
        public virtual DbSet<Inv_Store> Inv_Stores { get; set; }
        public virtual DbSet<Inv_Vendor> Inv_Vendors { get; set; }
        public virtual DbSet<Inv_RequsitionMaster> Inv_RequsitionMasters { get; set; }
        public virtual DbSet<Inv_RequsitionDetails> Inv_RequsitionDetail { get; set; }
        public virtual DbSet<Inv_RequisitionConsulateMaster> Inv_RequisitionConsulateMasters { get; set; }
        public virtual DbSet<Inv_RequisitionConsulateDetails> Inv_RequisitionConsulateDetail { get; set; }
        public virtual DbSet<Inv_TempStore> Inv_TempStores { get; set; }
        public virtual DbSet<Inv_TrxDetail> InvTrxDetails { get; set; }
        public virtual DbSet<Inv_TrxMaster> InvTrxMasters { get; set; }
        public virtual DbSet<InventoryDailyVoucher> InventoryDailyVouchers { get; set; }
        public virtual DbSet<InventoryDailyVoucherHistory> InventoryDailyVoucherHistorys { get; set; }
        public virtual DbSet<Inv_Settings> inv_Settings { get; set; }
        public virtual DbSet<Inv_RequsitionDispose> inv_RequsitionDisposes { get; set; }
        public virtual DbSet<Inv_ConsolidateDisposeRequest> Inv_ConsolidateDisposeRequests { get; set; }
        #endregion Inventory

        #region  EASS
        // public virtual DbSet<EASSChildrenProfile> EASSChildrenProfile { get; set; }
        // public virtual DbSet<EASSCompany> EASSCompany { get; set; }
        // public virtual DbSet<EASSDesignation> EASSDesignation { get; set; }
        // public virtual DbSet<EASSExperienceProfile> EASSExperienceProfile { get; set; }
        // public virtual DbSet<EASSNomineeProfile> EASSNomineeProfile { get; set; }
        // public virtual DbSet<EASSProfile> EASSProfile { get; set; }
        // public virtual DbSet<EASSReference> EASSReference { get; set; }
        // public virtual DbSet<EASSRelationship> EASSRelationship { get; set; }

        // public virtual DbSet<View_EASS_Profile> View_EASS_Profile { get; set; }
        // public virtual DbSet<EASSSalaryConfiguration> EASSSalaryConfiguration { get; set; }
        // public virtual DbSet<view_EASSSalaryConfiguration> view_EASSSalaryConfiguration { get; set; }
        //public virtual DbSet<EASSOvertimeHourConfiguration> EASSOvertimeHourConfiguration { get; set; }
        // public virtual DbSet<EASSTimeKeeping> EASSTimeKeeping { get; set; }
        // public virtual DbSet<View_EASS_TimeKeeping> View_EASS_TimeKeeping { get; set; }
        // public virtual DbSet<EASSMonthlySalary> EASSMonthlySalary { get; set; }
        //public virtual DbSet<View_EASSOvertimeHourConfiguration> View_EASSOvertimeHourConfiguration { get; set; }
        // public virtual DbSet<View_EASSMonthlySalary> View_EASSMonthlySalary { get; set; }

        #endregion

        #region  Disc

        public virtual DbSet<DiscDealingOfficer> DiscDealingOfficers { get; set; }
        public virtual DbSet<DiscEnqueryOfficer> DiscEnqueryOfficers { get; set; }
        public virtual DbSet<DiscCrime> DiscCrimes { get; set; }
        public virtual DbSet<DiscCaseAnnexation> DiscCaseAnnexations { get; set; }
        public virtual DbSet<DiscCaseDetail> DiscCaseDetails { get; set; }
        public virtual DbSet<DiscCaseMaster> DiscCaseMasters { get; set; }
        public virtual DbSet<DiscCaseStatu> DiscCaseStatus { get; set; }
        public virtual DbSet<DiscCaseCrimeLocation> DiscCaseCrimeLocations { get; set; }
        public virtual DbSet<DiscCasePunishmentDetail> DiscCasePunishmentDetails { get; set; }
        public virtual DbSet<DiscCasePunishmentMaster> DiscCasePunishmentMasters { get; set; }
        public virtual DbSet<DiscCaseDealingOfficer> DiscCaseDealingOfficers { get; set; }
        public virtual DbSet<DiscCaseEnquiryOfficer> DiscCaseEnquiryOfficers { get; set; }
        public virtual DbSet<DiscEmbezzleInfo> DiscEmbezzleInfoes { get; set; }
        public virtual DbSet<DiscEmbezzleEmpInfo> DiscEmbezzleEmpInfoes { get; set; }
        public virtual DbSet<DiscMemorendumDetail> DiscMemorendumDetails { get; set; }
        public virtual DbSet<DiscMemorendumMaster> DiscMemorendumMasters { get; set; }
        public virtual DbSet<DiscPunishment> DiscPunishments { get; set; }
        public virtual DbSet<DiscStatu> DiscStatus { get; set; }
        public virtual DbSet<DiscCaseDespatchNo> DiscCaseDespatchNoes { get; set; }
        // public virtual DbSet<DisciplinaryBackDateEntry> DisciplinaryBackDateEntry { get; set; }
        // public virtual DbSet<DisciplinaryActionFileAttachemnt> DisciplinaryActionFileAttachemnt { get; set; }
        // public virtual DbSet<DisciplinaryActionHistory> DisciplinaryActionHistory { get; set; }
        // public virtual DbSet<DiscCaseFollowUp> DiscCaseFollowUp { get; set; }
        //// public virtual DbSet<CaseFollowupFileAttachment> CaseFollowupFileAttachment { get; set; }


        #endregion

        #region  IPD

        //public virtual DbSet<IPDCoordinator> IPDCoordinators { get; set; }
        //public virtual DbSet<IPDCountry> IPDCountrys { get; set; }
        //public virtual DbSet<IPDOrganization> IPDOrganizations { get; set; }
        //public virtual DbSet<IPDProfession> IPDProfessions { get; set; }
        //public virtual DbSet<IPDProgram> IPDPrograms { get; set; }
        //public virtual DbSet<IPDProgramCategory> IPDProgramCategorys { get; set; }
        //public virtual DbSet<IPDSponsor> IPDSponsors { get; set; }
        //public virtual DbSet<IPDVisitorInfo> IPDVisitorInfos { get; set; }


        #endregion

        #region  VM
        //public virtual DbSet<VMCostCenter> VMCostCenter { get; set; }
        //public virtual DbSet<VMServiceType> VMServiceType { get; set; }
        //public virtual DbSet<VMServiceProvider> VMServiceProvider { get; set; }
        //public virtual DbSet<VMCarType> VMCarType { get; set; }
        //public virtual DbSet<VMCarOwner> VMCarOwner { get; set; }
        //public virtual DbSet<VMCarUser> VMCarUser { get; set; }
        //public virtual DbSet<VMCosting> VMCosting { get; set; }
        //public virtual DbSet<VMCarConfiguration> VMCarConfiguration { get; set; }
        //public virtual DbSet<VMRoadPlan> VMRoadPlan { get; set; }
        //public virtual DbSet<View_VMCarUser> View_VMCarUser { get; set; }
        //public virtual DbSet<View_VMOutOf> View_VMOutOf { get; set; }
        //public virtual DbSet<VMCarOutOfService> VMCarOutOfService { get; set; }
        //public virtual DbSet<VMCarStatus> VMCarStatus { get; set; }
        //public virtual DbSet<VMServiceProviderConfiguration> VMServiceProviderConfiguration { get; set; }
        //public virtual DbSet<VMCarRequisition> VMCarRequisition { get; set; }
        //public virtual DbSet<VMPerUnitCost> VMPerUnitCost { get; set; }
        //public virtual DbSet<VMUnit> VMUnit { get; set; }

        //public virtual DbSet<View_VMRequisition> View_VMRequisition { get; set; }

        #endregion

        #region  UNKNOWN/OTHERS

        //public virtual DbSet<Anulipi> Anulipis { get; set; }
        //public virtual DbSet<AnulipiDetail> AnulipiDetails { get; set; }

        //public virtual DbSet<MemberCategory> MemberCategories { get; set; }

        //public virtual DbSet<OfficialFileUpload> OfficialFileUploads { get; set; }

        //public virtual DbSet<Scheduler> Schedulers { get; set; }

        //public virtual DbSet<SchedulerDetail> SchedulerDetails { get; set; }
        // public virtual DbSet<Evaluation> Evaluations { get; set; }
        //public virtual DbSet<Welfare> Welfares { get; set; }

        //public virtual DbSet<FeedbackCategory> FeedbackCategories { get; set; }
        //public virtual DbSet<FeedbackRegister> FeedbackRegisters { get; set; }



        // public virtual DbSet<KPIQuestionSetup> KPIQuestionSetup { get; set; }

        #endregion

        #region Loan
        public virtual DbSet<prlLoanCalculation> prlLoanCalculation { get; set; }
        public virtual DbSet<CollectionMethod> CollectionMethods { get; set; }
        public virtual DbSet<LoanEligibility> LoanEligibility { get; set; }
        public virtual DbSet<LoanPurpose> LoanPurposes { get; set; }
        public virtual DbSet<ApprovalMaster> ApprovalMasters { get; set; }
        public virtual DbSet<ApproveDetail> ApproveDetails { get; set; }
        public virtual DbSet<ApplicantInfo> ApplicantInfos { get; set; }

        public virtual DbSet<ApplicantNominee> ApplicantNominees { get; set; }
        public virtual DbSet<LoanDisbursement> LoanDisbursements { get; set; }
        public virtual DbSet<LoanRegister> LoanRegister { get; set; }
        public virtual DbSet<LoanCollection> LoanCollections { get; set; }

        #endregion

        #region Welfare Fund

        public virtual DbSet<FundSetup> FundSetups { get; set; }
        public virtual DbSet<StaffWelfareFundSetting> StaffWelfareFundSettings { get; set; }
        public virtual DbSet<StaffWelfareFundConfiguration> StaffWelfareFundConfigurations { get; set; }

        public virtual DbSet<HealthWelfareFundSetting> HealthWelfareFundSettings { get; set; }
        public virtual DbSet<HealthWelfareFundConfiguration> HealthWelfareFundConfigurations { get; set; }

        public virtual DbSet<HealthFunding> HealthFundings { get; set; }

        #endregion

        #region Cooperative

        public virtual DbSet<CooperativeConfiguration> CooperativeConfigurations { get; set; }
        public virtual DbSet<CooperativeLedger> CooperativeLedgers { get; set; }



        #endregion

        #region Performance Evaluations
        public virtual DbSet<PerformanceEvaluation> PerformanceEvaluations { get; set; }
        public virtual DbSet<PerformanceEvaluationHistory> PerformanceEvaluationHistories { get; set; }
        public object TADAPurpose { get; internal set; }
        public object Employess { get; internal set; }

        #endregion

        #region FixedAsset

        public virtual DbSet<AssetClientInfo> AssetClientInfo { get; set; }
        public virtual DbSet<AssetDepreciationInfo> AssetDepreciationInfo { get; set; }
        public virtual DbSet<AssetGroupInfo> AssetGroupInfo { get; set; }
        public virtual DbSet<AssetInfo> AssetInfo { get; set; }
        public virtual DbSet<AssetOut> AssetOut { get; set; }
        public virtual DbSet<AssetOverhauling> AssetOverhauling { get; set; }
        public virtual DbSet<AssetPartialOut> AssetPartialOut { get; set; }
        public virtual DbSet<AssetProcessInfo> AssetProcessInfo { get; set; }
        public virtual DbSet<AssetRevaluation> AssetRevaluation { get; set; }
        public virtual DbSet<AssetTransfer> AssetTransfer { get; set; }
        public virtual DbSet<AssetUser> AssetUser { get; set; }
        public virtual DbSet<ClientType> ClientType { get; set; }
        public virtual DbSet<DailyTransaction> DailyTransaction { get; set; }
        public virtual DbSet<DepreciationMethod> DepreciationMethod { get; set; }
        public virtual DbSet<FixAssetUpdates> FixAssetUpdates { get; set; }
        public virtual DbSet<TransactionType> TransactionType { get; set; }
        public virtual DbSet<ReportType> ReportType { get; set; }
        public virtual DbSet<ReportTypeMapping> ReportTypeMapping { get; set; }
        public virtual DbSet<AssetRegister> AssetRegister { get; set; }
        public virtual DbSet<LastAssetCodeInfo> LastAssetCodeInfo { get; set; }
        public virtual DbSet<ProjectInfo> ProjectInfo { get; set; }
        public virtual DbSet<DepriciationRateChange> DepriciationRateChange { get; set; }        
        #endregion

        #region eRecruitment

        public virtual DbSet<ApplicationInfo> ApplicationInfo { get; set; }

        public virtual DbSet<eRecruitEmployeeEducation> eRecruitEmployeeEducations { get; set; }
        //public object InventoryDailyVouchers { get; set; }

        #endregion

        #region Gratuity
        public virtual DbSet<GratuityGlobalConfig> GratuityGlobalConfigs { get; set; }
        public virtual DbSet<EmployeeGratuity> EmployeeGratuities { get; set; }
        #endregion

        #region Apply
        public virtual DbSet<ApplicantMaster> ApplicantMaster { get; set; }
        public virtual DbSet<ApplicantAccademic> ApplicantAccademic { get; set; }
        public virtual DbSet<ApplicantJobExperience> ApplicantJobExperience { get; set; }

        public virtual DbSet<ApplicantTrainingInfo> ApplicantTrainingInfo { get; set; }
        public virtual DbSet<ApplicantReferenceInfo> ApplicantReferenceInfo { get; set; }
        public virtual DbSet<ApplicantAddressInfo> ApplicantAddressInfo { get; set; }
        public virtual DbSet<LevelofEducation> LevelofEducation { get; set; }
        public virtual DbSet<ExamTitle> ExamTitle { get; set; }
        public virtual DbSet<AppliedPost> AppliedPost { get; set; }

        public virtual DbSet<JobsCircular> JobsCircular { get; set; }
        public virtual DbSet<QuestionAnsweredByApplicant> QuestionAnsweredByApplicant { get; set; }
        #endregion

        #region Training
        public virtual DbSet<TrainingTitle> TrainingTitles { get; set; }
        public virtual DbSet<TrainingType> TrainingTypes { get; set; }
        public virtual DbSet<TrainingArea> TrainingAreas { get; set; }
        public virtual DbSet<Institute> Institutes { get; set; }
        public virtual DbSet<Venue> Venues { get; set; }
        public virtual DbSet<ResultGrade> ResultGrades { get; set; }
        #endregion

        #region FeedBack
      
        public virtual DbSet<FeedbackCategory> FeedbackCategories{ get; set; }
        public virtual DbSet<FeedbackRegister> FeedbackRegisters{ get; set; }
        public object ComponentPayrolls { get; internal set; }
        public object EmployeeTypeConfiguration { get; internal set; }
        #endregion

        #region IncomeTax
        public virtual DbSet<IncomeTax> IncomeTaxes { get; set; } // Partha added on 13/07/2025
        #endregion


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ApplicationLog>()
                 .Property(e => e.ActionURL)
                 .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.ClientIP)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.RequestUser)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.RequestDetail)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.QueryStringParams)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.ErrorDetail)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.UserAgent)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.ControllerName)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.ActionName)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.HttpMethod)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.SessionId)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.OrganizationId)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationSetting>()
                .Property(e => e.PLAccount)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationSetting>()
                .Property(e => e.BankAccount)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationSetting>()
                .Property(e => e.OrganizationAddress)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationSetting>()
                .Property(e => e.PhoneNo)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationSetting>()
                .Property(e => e.CellNo)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationSetting>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationSetting>()
                .Property(e => e.LicenseNo)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationSetting>()
                .Property(e => e.ProcessType)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationSetting>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetRole>()
                .Property(e => e.DefaultLinkURL)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetRoleModules)
                .WithRequired(e => e.AspNetRole)
                .HasForeignKey(e => e.RoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetSecurityLevel>()
                .Property(e => e.SecurityLevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityLevel>()
                .Property(e => e.SecurityLevelName)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityLevel>()
                .HasMany(e => e.AspNetRoleModules)
                .WithRequired(e => e.AspNetSecurityLevel)
                .HasForeignKey(e => e.SecurityLevelId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetSecurityModule>()
                .Property(e => e.SecurityModuleCode)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityModule>()
                .Property(e => e.LinkText)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityModule>()
                .Property(e => e.ControllerName)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityModule>()
                .Property(e => e.ActionName)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityModule>()
                .HasMany(e => e.AspNetRoleModules)
                .WithRequired(e => e.AspNetSecurityModule)
                .HasForeignKey(e => e.ModuleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Country>()
                .Property(e => e.CountryCode)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .Property(e => e.CountryShortCode)
                .IsFixedLength();

            modelBuilder.Entity<Country>()
                .Property(e => e.isoCode3)
                .IsFixedLength();

            modelBuilder.Entity<Country>()
                .HasMany(e => e.EmployeeAddresses)
                .WithRequired(e => e.Country)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.StateOrProvinces)
                .WithRequired(e => e.Country)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<District>()
                .HasMany(e => e.EmployeeAddresses)
                .WithOptional(e => e.District)
                .HasForeignKey(e => e.DistrictId);

            modelBuilder.Entity<Employee>()
                .Property(e => e.GrossSalary)
                .HasPrecision(10, 2);

            //modelBuilder.Entity<Employee>()
            //    .Property(e => e.EmployeeStatus)
            //    .IsFixedLength()
            //    .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeRank)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.EmployeeAddresses)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.EmployeeEducations)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.EmployeeFamilyInfoes)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Employee>()
            //    .HasMany(e => e.EmployeePostingHistories)
            //    .WithRequired(e => e.Employee)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.EmployeeReferences)
                .WithRequired(e => e.Employee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EmployeeAddress>()
                .Property(e => e.AddressType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EmployeeDepartment>()
                .HasMany(e => e.Employees)
                .WithRequired(e => e.EmployeeDepartment)
                .HasForeignKey(e => e.DepartmentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EmployeeDesignation>()
                .HasMany(e => e.Employees)
                .WithRequired(e => e.EmployeeDesignation)
                .HasForeignKey(e => e.DesignationId)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<EmployeeDesignation>()
            //    .HasMany(e => e.EmployeeTimeScales)
            //    .WithRequired(e => e.EmployeeDesignation)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<EmployeeFamilyInfo>()
                .Property(e => e.Relation)
                .IsFixedLength();

            modelBuilder.Entity<EmployeeFamilyInfo>()
                .Property(e => e.Gender)
                .IsFixedLength();

            //modelBuilder.Entity<EmployeeOfficeMapping>()
            //    .Property(e => e.CreateUser)
            //    .IsUnicode(false);


            modelBuilder.Entity<EmployeeReference>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            //modelBuilder.Entity<EmployeeSalaryScale>()
            //    .Property(e => e.Salary)
            //    .HasPrecision(7, 2);

            //modelBuilder.Entity<EmployeeSalaryScale>()
            //    .Property(e => e.Increment)
            //    .HasPrecision(5, 2);

            //modelBuilder.Entity<EmployeeTimeScale>()
            //    .Property(e => e.FixedPay)
            //    .HasPrecision(6, 2);

            //modelBuilder.Entity<GeoLocation>()
            //    .Property(e => e.LocationName)
            //    .IsUnicode(false);

            //modelBuilder.Entity<GeoLocation>()
            //    .Property(e => e.FirstLevel)
            //    .IsUnicode(false);

            //modelBuilder.Entity<GeoLocation>()
            //    .Property(e => e.SecondLevel)
            //    .IsUnicode(false);

            //modelBuilder.Entity<GeoLocation>()
            //    .Property(e => e.ThirdLevel)
            //    .IsUnicode(false);

            //modelBuilder.Entity<GeoLocation>()
            //    .Property(e => e.FourthLevel)
            //    .IsUnicode(false);

            //modelBuilder.Entity<GeoLocation>()
            //    .Property(e => e.FifthLevel)
            //    .IsUnicode(false);

            //modelBuilder.Entity<GeoLocation>()
            //    .Property(e => e.CreateUser)
            //    .IsUnicode(false);

            modelBuilder.Entity<LgThana>()
                .HasMany(e => e.EmployeeAddresses)
                .WithOptional(e => e.LgThana)
                .HasForeignKey(e => e.ThanaId);

            modelBuilder.Entity<LgUnion>()
                .HasMany(e => e.EmployeeAddresses)
                .WithOptional(e => e.LgUnion)
                .HasForeignKey(e => e.UnionId);

            //modelBuilder.Entity<MemberCategory>()
            //    .Property(e => e.MemberCategoryCode)
            //    .IsUnicode(false);

            //modelBuilder.Entity<MemberCategory>()
            //    .Property(e => e.CategoryName)
            //    .IsUnicode(false);

            //modelBuilder.Entity<MemberCategory>()
            //    .Property(e => e.CategoryShortName)
            //    .IsUnicode(false);

            //modelBuilder.Entity<MemberCategory>()
            //    .Property(e => e.CreateUser)       //comment by momin
            //    .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.OfficeCode)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.FirstLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.SecondLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.ThirdLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.FourthLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.OfficeAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.PostCode)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            //modelBuilder.Entity<Office>()
            //    .Property(e => e.CreateUser)
            //    .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .HasMany(e => e.ApplicationSettings)
                .WithRequired(e => e.Office)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Office>()
                .HasMany(e => e.EmployeeOfficeMappings)
                .WithRequired(e => e.Office)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Office>()
            //    .HasMany(e => e.EmployeePostingHistories)
            //    .WithRequired(e => e.Office)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Scheduler>()
            //    .Property(e => e.SchedulerName)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Scheduler>()
            //    .Property(e => e.Description)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Scheduler>()
            //    .Property(e => e.Frequency)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Scheduler>()
            //    .Property(e => e.CreateUser)  // comment by momin
            //    .IsUnicode(false);

            //modelBuilder.Entity<Scheduler>()
            //    .HasMany(e => e.SchedulerDetails)
            //    .WithRequired(e => e.Scheduler)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<StateOrProvince>()
                .Property(e => e.CountryShortCode)
                .IsFixedLength();

            modelBuilder.Entity<StateOrProvince>()
                .Property(e => e.Code)
                .IsFixedLength();

            modelBuilder.Entity<StateOrProvince>()
                .HasMany(e => e.Districts)
                .WithRequired(e => e.StateOrProvince)
                .HasForeignKey(e => e.division_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StateOrProvince>()
                .HasMany(e => e.EmployeeAddresses)
                .WithRequired(e => e.StateOrProvince)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<EmployeePromotion>()
            //    .Property(e => e.PromotionType)
            //    .IsFixedLength()
            //    .IsUnicode(false);

            //modelBuilder.Entity<EmployeePromotion>()
            //    .Property(e => e.Pay)
            //    .HasPrecision(7, 2);

            //modelBuilder.Entity<SchedulerDetail>()
            //    .Property(e => e.ErrorDescription)
            //    .IsUnicode(false);

            //modelBuilder.Entity<SchedulerDetail>()
            //    .Property(e => e.CreateUser)
            //    .IsUnicode(false);


            //modelBuilder.Entity<Evaluation>()
            //    .Property(e => e.ReliableInWorkingCmt)
            //    .IsFixedLength()
            //    .IsUnicode(false);

            //modelBuilder.Entity<Evaluation>()
            //    .Property(e => e.LoyalAndCredibleCmt)
            //    .IsFixedLength()
            //    .IsUnicode(false);

            //modelBuilder.Entity<Evaluation>()
            //    .Property(e => e.SuspicionInFinancialTranscCmt)
            //    .IsFixedLength()
            //    .IsUnicode(false);

            //modelBuilder.Entity<Evaluation>()
            //    .Property(e => e.RecomForConSalary)
            //    .IsFixedLength()
            //    .IsUnicode(false);

            //modelBuilder.Entity<Evaluation>()
            //    .Property(e => e.RecomForAvailsScale)
            //    .IsFixedLength()
            //    .IsUnicode(false);

            //modelBuilder.Entity<Evaluation>()
            //    .Property(e => e.CountSig)
            //    .IsFixedLength()
            //    .IsUnicode(false);
            modelBuilder.Entity<ContributionRegister>()
                .Property(e => e.SelfContribution)
                .HasPrecision(18, 6);
            modelBuilder.Entity<ContributionRegister>()
                .Property(e => e.OrgContribution)
                .HasPrecision(18, 6);

            modelBuilder.Entity<DiscCaseMaster>()
                .Property(e => e.CaseType)
                .IsUnicode(false);

            modelBuilder.Entity<GratuityGlobalConfig>()
                .Property(b => b.GratuityGlobalConfigId)
                .IsRequired();

            modelBuilder.Entity<LeaveHistoryAttachment>()
                .Property(b => b.Id)
                .IsRequired();

            modelBuilder.Entity<EmployeeAllowence>()
            .Property(e => e.Id)
            .IsRequired();


            modelBuilder.Entity<EmployeeTypeConfiguration>()
                .Property(e => e.Id)
                .IsRequired();

        }
    }
}
