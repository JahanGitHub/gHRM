using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("EmployeeDesignation")]
    public partial class EmployeeDesignation
    {
        public EmployeeDesignation()
        {
            Employees = new HashSet<Employee>();
            //EmployeePromotions = new HashSet<EmployeePromotion>();
            //EmployeeTimeScales = new HashSet<EmployeeTimeScale>();
        }
        [Key]
        public int DesignationId { get; set; }

        [Required]
        [StringLength(50)]
        public string DesignationCode { get; set; }

        [Required]
        [StringLength(100)]
        public string DesignationName { get; set; }

        [StringLength(100)]
        public string DesignationShortName { get; set; }

        [StringLength(5)]
        public string DesignationType { get; set; }
        
        public int SalaryScaleId { get; set; }
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

        public string Rank { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        // public virtual ICollection<EmployeePromotion> EmployeePromotions { get; set; }
        // public virtual ICollection<EmployeeTimeScale> EmployeeTimeScales { get; set; }

        public int? InsuranceAmount { get; set; }
    }
}
