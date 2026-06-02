using gHRM.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.Employee
{
    public class EmployeeBasicServiceBookModel
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Gender { get; set; }
        public string EmployeeGender { get; set; }
        public string DateOfBirth { get; set; }
        public string NationalID { get; set; }
        public string BloodGroup { get; set; }
        public string FirstJoiningDate { get; set; }
        public string MaritalStatus { get; set; }
        public byte[] EmployeeImage { get; set; }
        public string EmployeeImageLink { get; set; }
        public byte[] EmployeeImageLinkImage
        {
            get
            {
                var newEmployeeImage = EmployeeImage != null
                    ? EmployeeImage : EmployeeImageLink.ImagePathToByte();
                return newEmployeeImage;
            }
        }
        public byte[] EmpSignature { get; set; }
        public byte[] MDSignature { get; set; }
        public byte[] MDSignature2 { get; set; }

        public byte[] MDSignature3 { get; set; }
        public byte[] MDSignature4 { get; set; }
        public string SpecialSymbol { get; set; }
        public byte[] SpecialSymbolImage
        {
            get
            {
                var newSpecialSymbolImage = SpecialSymbol.ImagePathToByte();
                return newSpecialSymbolImage;
            }
        }
        public string SpecialRemarks { get; set; }
        public string FingerPrint { get; set; }
        public byte[] FingerPrintImage
        {
            get
            {
                var newFingerPrint = FingerPrint.ImagePathToByte();
                return newFingerPrint;
            }
        }
        public string SpouseName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public int? PermanentAddressId { get; set; }
        public string PermanentAddressPostOffice{ get; set; }
        public string PermanentAddressStreetOrHouse { get; set; }
        public string PermanentAddressUnionName { get; set; }
        public string PermanentAddressThanaName { get; set; }
        public string PermanentAddressUpazillaName { get; set; }
        public string PermanentAddressDistrictName { get; set; }
        public int? PermanentAddressEmployeeId { get; set; }
        public int? PresentAddressId { get; set; }
        public string PresentAddressDetails { get; set; }
        public int? PresentAddressEmployeeId { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }
        public string OrganizationOwnerOrManagerName { get; set; }
        public string OrganizationOwnerOrManagerName2 { get; set; }
        public string OrganizationOwnerOrManagerName3 { get; set; }
        public string OrganizationOwnerOrManagerName4 { get; set; }

        public string OrganizationOwnerOrManagerDesignation { get; set; }
        public string TrainingInfo { get; set; }
        public string EducationInfo { get; set; }
        public string Height { get; set; }

        public string DateOfTermination { get; set; }

        public string ReasonOfTermination { get; set; }
        public string JOININGDATEMGT3 { get; set; }
        public string JOININGDATEMGT4 { get; set; }


        public string TERMINATIONDATEMG3 { get; set; }
        public string TERMINATIONDATEMG4 { get; set; }

    }

    public class MonthlySalaryForServiceBookModel
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public int SalaryYear { get; set; }
        public int SalaryMonth { get; set; }
        public string ComponentName { get; set; }
        public string SalaryDate { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? HouseRent { get; set; }
        public decimal? Medical { get; set; }
        public decimal? Conveyance { get; set; }
        public decimal? BonusAmount { get; set; }

        public string OrganizationOwnerOrManagerName { get; set; }
        public string OrganizationOwnerOrManagerDesignation { get; set; }

        public decimal? PFTotal { get; set; }
        public decimal? PF { get; set; }


        public byte[] EmpSignature { get; set; }
        public byte[] MDSignature { get; set; }


    }

    public class LeaveRecordServiceBookModel
    {
        public long EmployeeId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal TotalDays { get; set; }
        public int RemainingLeaveBalance { get; set; }
        public int TotalLeaveSellTlllNow { get; set; }
        public string LastLeaveSellDate { get; set; }
        public int TotalSold { get; set; }
        public int SellRemainingLeaveBalance { get; set; }
        public byte[] EmpSignature { get; set; }
        public byte[] MDSignature { get; set; }
    }
    public class CaseNoSlNoModel
    {
        public long EmployeeId { get; set; }
        public string CaseNo { get; set; }
        public string SlNo { get; set; }
        public string CaseDateFrom { get; set; }
        public string CaseDateTo { get; set; }
        public string CaseType { get; set; }
        public string CaseDescription { get; set; }
        public string CrimeLocationName { get; set; }
        public string CrimeLocationZoneName { get; set; }
        public string DealerName { get; set; }
        public string EnquiryName { get; set; }
        public string Crimes { get; set; }
        public string CrimeForName { get; set; }
        public string DispatchNo { get; set; }
        public string StatusMsg { get; set; }
        public string StatusDt { get; set; }
        public string PunishmentName { get; set; }
        public string TotAnnexationAmount { get; set; }
        public string TotReturnamount { get; set; }
        public string TotBalancemsg { get; set; }
        public byte[] EmpSignature { get; set; }
        public byte[] MDSignature { get; set; }
    }
}
