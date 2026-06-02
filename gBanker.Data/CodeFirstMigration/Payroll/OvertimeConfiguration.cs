namespace gHRM.Data.CodeFirstMigration.Payroll
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("prl.OvertimeConfiguration")]
    public partial class OvertimeConfiguration
    {

        [Key]
        public int OvertimeConfigId { get; set; }

        [Required]
        //[StringLength(100)]
        public int HourFrom { get; set; }

        [Required]
        public int HourTo { get; set; }

        public double ? Amount { get; set; }

        public string Rule { get; set; }

        public double? DividedBy { get; set; }

        public int ? Rank { get; set; }

        //[Column(TypeName = "smalldatetime")]
        //public DateTime? InActiveDate { get; set; }

        //public long? CreateUser { get; set; }

        //[Column(TypeName = "smalldatetime")]
        //public DateTime? CreateDate { get; set; }

        //public long? UpdateUser { get; set; }


        //[Column(TypeName = "smalldatetime")]
        //public DateTime? UpdateDate { get; set; }

        //public virtual Employee Employee { get; set; }
    }
}
