using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("leave.ELEncashmentConfiguration")]
    public class ELEncashmentConfiguration
    {
        [Key]
        public int ConfigurationId { get; set; }
        public string EligibleFrom { get; set; }
        public string EncashmentStage { get; set; }
        public int EligibilityDuration { get; set; }
        public int MinimumBalance { get; set; }
        public int EncashmentEligibleQuantity { get; set; }
        public bool IsActive { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Formula { get; set; }
    }
}
