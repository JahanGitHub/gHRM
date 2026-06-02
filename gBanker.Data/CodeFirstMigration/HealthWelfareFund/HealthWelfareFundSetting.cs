
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace gHRM.Data.CodeFirstMigration.HealthWelfareFund
{

    [Table("HealthWelfareFundSetting")]
    public partial class HealthWelfareFundSetting
    {
        public int HealthWelfareFundSettingId { get; set; }

        public decimal? DeductionAmount { get; set; }

        public bool IsPercentage { get; set; }

        public bool IsActive { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }


        [NotMapped]
        public string CreateDateInString => CreateDate != null ? ((DateTime)CreateDate).ToString("dd MMM yyyy") : "";
    }
}
