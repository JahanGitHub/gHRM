namespace gHRM.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class View_EmployeeOfficeTimeException
    {
        [Key]
        public int RowSl { get; set; }
        public int Id { get; set; }
        public int OfficeTypeId { get; set; }
        public int OfficeId { get; set; }
        public string LIT { get; set; }
        public string LLT { get; set; }
        public string LOT { get; set; }
        public string ESD { get; set; }
        public string EED { get; set; }
        public int TimeKeepingRosterId { get; set; }
        public String TimeExceptionReason { get; set; }
        public bool IsActive { get; set; }
        public string RosterName { get; set; }
        public string OfficeTypeName { get; set; }
        public string OfficeName { get; set; }
    }
}

