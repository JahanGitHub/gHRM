using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeDesignationMapping")]
    public class EmployeeDesignationMapping
    {
        [Key]
        public int DesignationMapId { get; set; }
        public int EquivalentDesignationId { get; set; }
        public int OrnamentalDesginationid { get; set; }
        public int OfficeDesignationId { get; set; }
        public int IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public long CreateBy { get; set; }
        public long UpdateBy { get; set; }
    }
}
