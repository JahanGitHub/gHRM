using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gHRM.Data.CodeFirstMigration
{
    public partial class View_EmployeeDesignation
    {
        public int? RowSl { get; set; }

        [Key]
        [Column(Order = 0)]
        public int DesignationId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string DesignationCode { get; set; }

        [StringLength(50)]
        public string Rank { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string DesignationName { get; set; }

        [StringLength(100)]
        public string DesignationShortName { get; set; }

        [StringLength(5)]
        public string DesignationType { get; set; }

        public int? SalaryScaleId { get; set; }

        public int? CompanyId { get; set; }

        [StringLength(50)]
        public string insuranceAmount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateTo { get; set; }

        [StringLength(50)]
        public string DateFromMsg { get; set; }

        [StringLength(50)]
        public string DateToMsg { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool IsActive { get; set; }
    }
}
