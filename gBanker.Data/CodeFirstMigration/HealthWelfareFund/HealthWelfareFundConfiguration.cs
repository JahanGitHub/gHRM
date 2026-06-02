

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace gHRM.Data.CodeFirstMigration.HealthWelfareFund
{

    [Table("HealthWelfareFundConfiguration")]
    public partial class HealthWelfareFundConfiguration
    {
        public int HealthWelfareFundConfigurationId { get; set; }

        public int? EmployeeId { get; set; }

        public int? HealthWelfareFundSettingId { get; set; }

        public decimal? CollectionAmount { get; set; }

        public DateTime? CollectionDate { get; set; }

        public bool IsActive { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
    }
}
