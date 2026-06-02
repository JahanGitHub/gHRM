namespace gHRM.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class View_TimeKeepingRoster
    {
        [Key]
        public int RowSl { get; set; }
        public string RosterName { get; set; }
        public int TimeKeepingRosterId { get; set; }
        public string LIT { get; set; }
        public string LLT { get; set; }
        public string LOT { get; set; }
        public string ESD { get; set; }
        public string EED { get; set; }
        public bool IsActive { get; set; }
    }

    public partial class EmployeeRoasterScheduleModel
    {
        public int Id { get; set; }
        public int RowSl { get; set; }
        public string RosterName { get; set; }
        public int TimeKeepingRosterId { get; set; }
        public string LIT { get; set; }
        public string LLT { get; set; }
        public string LOT { get; set; }
        public string ESD { get; set; }
        public string EED { get; set; }
        public bool IsActive { get; set; }
    }
}
