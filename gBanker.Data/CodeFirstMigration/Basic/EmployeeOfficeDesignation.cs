using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeOfficeDesignation")]
    public partial class EmployeeOfficeDesignation
    {
        [Key]
        public long EmpOfficeDesigId { get; set; }

        public long EmployeeId { get; set; }

        public int OfficeDesignationId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public int? Duration { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public virtual OfficeDesignation officeDesignation { get; set; }
        public virtual Employee employee { get; set; }






        //public long OrnamentalDesigId { get; set; }
        //public string OrnamentalDesignationName { get; set; }
        //public int? DesignationOrder { get; set; }
        //public bool IsActive { get; set; }
        //public DateTime? InActiveDate { get; set; }
        //public long CreateUser { get; set; }
        //public DateTime? CreateDate { get; set; }
        //public long UpdateUser { get; set; }
        //public DateTime? UpdateDate { get; set; }
        //public int MyProperty { get; set; }
        //public int MyProperty { get; set; }
        //public int MyProperty { get; set; }
        //public int MyProperty { get; set; }
        //public int MyProperty { get; set; }
        //public int MyProperty { get; set; }
        //public int MyProperty { get; set; }
        //public int MyProperty { get; set; }
        //public int MyProperty { get; set; }
    }
}
