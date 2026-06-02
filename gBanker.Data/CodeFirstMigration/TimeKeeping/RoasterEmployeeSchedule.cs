using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("att.RoasterEmployeeSchedule")]
    public class RoasterEmployeeSchedule
    {
        [Key]
        public int Id { get; set; }        
        public int EmployeeId { get; set; }        
        public int RoasterId { get; set; }        
        public string RoasterName { get; set; }        
        public DateTime LoginTime { get; set; }        
        public DateTime LastLoginTime { get; set; }        
        public DateTime LogoutTime { get; set; }        
        public DateTime EffectiveStartDate { get; set; }        
        public DateTime EffectiveEndDate { get; set; }        
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Int64? CreateBy { get; set; }
        public Int64? UpdateBy { get; set; }
    }
}
