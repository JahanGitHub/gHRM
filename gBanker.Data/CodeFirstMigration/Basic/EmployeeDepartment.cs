using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeDepartment")]
    public partial class EmployeeDepartment
    {
        public EmployeeDepartment()
        {
            Employees = new HashSet<Employee>();
        }

        [Key]
        public int DepartmentId { get; set; }

        public int OfficeTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string DepartmentCode { get; set; }

        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; }

        [StringLength(50)]
        public string DepartmentShortName { get; set; }
        public int? CompanyId { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public long? CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }

        public long? UpdateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual OfficeType OfficeType { get; set; }
    }
}
