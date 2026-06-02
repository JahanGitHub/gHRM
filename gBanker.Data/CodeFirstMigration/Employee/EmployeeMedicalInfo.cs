using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeMedicalInfo")]
    public class EmployeeMedicalInfo
    {
        [Key]
        public int MedicalInfoId { get; set; }
        public long EmployeeId { get; set; }
        public string MedicalInfoOf { get; set; }
        public string PersonBloodGroup { get; set; }
        public bool HasBloodPressure { get; set; }
        public string BloodPressureType { get; set; }
        public bool HasDiabetics { get; set; }
        public bool HasHeartDisease { get; set; }
        public bool HasAlergy { get; set; }
        public bool HasOtherDisease { get; set; }
        public bool? XRayChest { get; set; }
        public bool? VDRL { get; set; }
        public bool? HBsAgE { get; set; }
        public bool? VisionTest { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string MedicalRemarks { get; set; }
        public bool IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public long? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
