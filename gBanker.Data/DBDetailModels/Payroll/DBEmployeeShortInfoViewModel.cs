using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.Payroll
{
   public class DBEmployeeShortInfoViewModel
    {
public long EmployeeId  {get;set;}
public string    EmployeeCode {get;set;}
public string EmployeeName    {get;set;}
public string    EmployeeNameBng {get;set;}
public int? CompanyId   {get;set;}
public int? BranchId    {get;set;}
public int? OfficeId    {get;set;}
public int? DepartmentId    {get;set;}
public int? SectionId   {get;set;}
public int? DesignationId   {get;set;}
public string EmployeeRank {get;set;}
public int? OfficeDesignationId {get;set;}
public int? SignatureDesignationId  {get;set;}
public string    BatchNo {get;set;}
public int? EmployeeTypeId  {get;set;}
public int? EmploymentTypeId    {get;set;}
public  DateTime?    FirstJoiningDate {get;set;}
public int? AgreementPeriodInMonth  {get;set;}
public  DateTime?    ConfirmationDate {get;set;}
public int? EmployeeStatusId { get;set;}
public string    StatusChangeComment {get;set;}
public  DateTime? DateOfEmployeeStatus    {get;set;}
public string    TerminationCause {get;set;}
public  DateTime? StatusDate  {get;set;}
public int? SeniorityLoss   {get;set;}
decimal GrossSalary {get;set;}
public string BankAccountNo {get;set;}
public  DateTime? FirstDateOfScale    {get;set;}
public Decimal? DeductionRate {get;set;}
public int? SalaryMode  {get;set;}
public int? PRSalaryScaleID {get;set;}
public int? PRHouseRentID   {get;set;}
public int? IncrementMonth  {get;set;}
public  DateTime?    EffectiveStartDate {get;set;}
public  DateTime? EffectiveEndDate    {get;set;}
public int? GradeId {get;set;}
public int? Step    {get;set;}
public int? IncrementYearFrom   {get;set;}
public string    BankName {get;set;}
public string BankBranchName  {get;set;}
public bool? IsPFApplicable {get;set;}
public string PFType  {get;set;}
public bool? IsInterest   { get;set;}
public bool? IsPFClossed {get;set;}
public bool? IsOvertime {get;set;}
public decimal? OvertimeRate    {get;set;}
public decimal? MaxOvertimePerDay   {get;set;}
public decimal? MaxOvertimePerMonth {get;set;}
public decimal? TotalEarnings   {get;set;}
public string    TinNo {get;set;}
public DateTime? LoginTime  { get;set;}
public DateTime?    LogoutTime {get;set;}
public DateTime? LastLogpublicontime   {get;set;}
public string    JobExperience {get;set;}
public string ComputerEfficiency  {get;set;}
public bool? IsInvestigation {get;set;}
public string Gender  {get;set;}
public  DateTime?    DateOfBirth {get;set;}
public string BirthPlace  {get;set;}
public string    Nationality {get;set;}
public string NationalId  {get;set;}
public string    MaritalStatus {get;set;}
public string Religion    {get;set;}
public string    PermanentAddress {get;set;}
public string PresentAddress  {get;set;}
public string    Email {get;set;}
public string OfficialEmail   {get;set;}
public string    ContactNo1 {get;set;}
public string ContactNo2  {get;set;}
public string    PABXExtension {get;set;}
public string BloodGroup  {get;set;}
public string PassportNo {get;set;}
public  DateTime? PassportIssueDate   {get;set;}
public  DateTime?    PassportExpireDate {get;set;}
public string EmployeeImageLink   {get;set;}
public string    OfficeName {get;set;}
public int? OfficeTypeId    {get;set;}
public string OfficeCode {get;set;}
public string OfficeNameBn    {get;set;}
public int? OfficeLevel {get;set;}
public int? OfficeLocationId    {get;set;}
public string    OfficeLocationName {get;set;}
public string DepartmentName  {get;set;}
public string    DepartmentCode {get;set;}
public string DepartmentShortName {get;set;}
public string    DesignationCode {get;set;}
public int? NextDesignationId   {get;set;}
public string    DesignationName {get;set;}
public string DesignationType {get;set;}
public string    OffcDesignName {get;set;}
public string OffcDesignNameBn    {get;set;}
public string    SignatureCode {get;set;}
public string SignatureName   {get;set;}
public string    SectionCode {get;set;}
public string EmployeeTypeName    {get;set;}
public string    EmployementTypeName {get;set;}
public string EmployeeStatusName  {get;set;}
public string EmployeeStatusValue {get;set;}
public string GradeName   {get;set;}
public string GradeDescription {get;set;}
decimal InitialAmount   {get;set;}



    }
}
