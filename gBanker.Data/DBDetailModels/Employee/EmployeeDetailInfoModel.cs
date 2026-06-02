using gHRM.Core.Utilities;
using System;

namespace gHRM.Data.DBDetailModels.Employee
{
    public class EmployeeDetailInfoModel
    {
        public int TotalCount { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameBng { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public int? OfficeId { get; set; }
        public string OfficeCode { get; set; }

        public string OfficeName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? SectionId { get; set; }
        public int? DesignationId { get; set; }
        public string DesignationName { get; set; }
        public string OfficeDesignationId { get; set; }
        public string OfficeDesignationName { get; set; }
        public string BatchNo { get; set; }

        public DateTime? FirstJoiningDate { get; set; }
        public int? AgreementPeriodInMonth { get; set; }
        public DateTime? AgreementFromDate { get; set; }
        public DateTime? AgreementToDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? PermanentDate { get; set; }
        public int? StatusPeriodInMonth { get; set; }
        public DateTime? StatusFromDate { get; set; }
        public DateTime? StatusToDate { get; set; }
        public DateTime? StatusDate { get; set; }
        public int? SeniorityLoss { get; set; }
        public string StatusChangeComment { get; set; }
        public string TerminationCause { get; set; }
        public string BankAccountNo { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string TinNo { get; set; }
        public string JobExperience { get; set; }
        public string ComputerEfficiency { get; set; }




        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string BirthPlace { get; set; }
        public string Nationality { get; set; }
        public string NationalId { get; set; }
        public string MaritalStatus { get; set; }
        public string Religion { get; set; }
        public string Email { get; set; }
        public string OfficialEmail { get; set; }
        public string ContactNo { get; set; }
        public string PABXExtension { get; set; }
        public string BloodGroup { get; set; }
        public string PassportNo { get; set; }

        public DateTime? PassportIssueDate { get; set; }
        public DateTime? PassportExpireDate { get; set; }
        public string EmployeeImageLink { get; set; }
        public bool IsActive { get; set; }


        public long? preAdrsAddressId { get; set; }
        public int? preAdrsCountryId { get; set; }
        public string PreCountry { get; set; }
        public int? preAdrsStateOrProvinceId { get; set; }
        public string PreDivision { get; set; }
        public int? preAdrsDistrictId { get; set; }
        public string PreDistrict { get; set; }
        public int? preAdrsThanaId { get; set; }
        public string PreThana { get; set; }
        public int? preAdrsUnionId { get; set; }
        public string PreUnion { get; set; }
        public string preAdrsStreetOrHouse { get; set; }
        public string preAdrsZipCode { get; set; }
        public string preAdrsAddressDetail { get; set; }

        public long? perAdrsAddressId { get; set; }
        public int? perAdrsCountryId { get; set; }
        public string PerCountry { get; set; }
        public int? perAdrsStateOrProvinceId { get; set; }
        public string PerDivision { get; set; }
        public int? perAdrsDistrictId { get; set; }
        public string PerDistrict { get; set; }
        public int? perAdrsThanaId { get; set; }
        public string PerThana { get; set; }
        public int? perAdrsUnionId { get; set; }
        public string PerUnion { get; set; }
        public string perAdrsStreetOrHouse { get; set; }
        public string perAdrsZipCode { get; set; }
        public string perAdrsAddressDetail { get; set; }
    }

    public class EmployeeDetailApiModel
    {
        public int TotalCount { get; set; }
        public long EmployeeId { get; set; }
        public String EmployeeCode { get; set; }
        public String EmployeeName { get; set; }
        public String EmployeeNameBng { get; set; }
        public String DesignationName { get; set; }
        public String ContactNo { get; set; }
        public String Email { get; set; }
        public int OfficeId { get; set; }
    }

    public class FixedAssetEmployeeModel
    {       
        public long EmployeeId { get; set; }
        public String EmployeeCode { get; set; }
        public String EmployeeName { get; set; }
    }
}
