namespace gHRM.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TimeKeepingRoster")]
    public class TimeKeepingRoster
    {
        [Key]
        public int TimeKeepingRosterId { get; set; }
        public string RosterName { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LastLoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public bool IsActive { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? CreateBy { get; set; }

        public long? UpdateBy { get; set; }

        //public DateTime? strLoginTime { get; set; }
    }
}
