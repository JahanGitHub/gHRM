using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace gHRM.Data.CodeFirstMigration
{
    [Table("[promo].[PromotionConfiguredSalary]")]
    public partial class PromotionConfiguredSalary
    {
        [Key]
        public int Id { get; set; }       
        public long PromotionId { get; set; }       
        public long EmployeeId { get; set; }
        public decimal? GrossSalary { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? HouseRent { get; set; }
        public decimal? Medical { get; set; }
        public decimal? Conveyance { get; set; }
        public decimal? Others { get; set; }
        public bool IsActive { get; set; }
        public Int64? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public Int64? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
