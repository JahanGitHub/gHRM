
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace gHRM.Data.CodeFirstMigration.WelfareFund
{
    [Table("StaffWelfareFundConfiguration")]
    public partial class StaffWelfareFundConfiguration
    {
        public int StaffWelfareFundConfigurationId { get; set; }

        public int EmployeeId { get; set; }

        public int PurposeId { get; set; }

        public decimal FundAmount { get; set; }

        public string remarks { get; set; }


        public bool IsActive { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

    }
}
