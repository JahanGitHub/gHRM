using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.Promotions
{
    public class PromotionBackLogImportModel
    {
        public Int64 EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string PayrollDesignation { get; set; }
        public string PromotionType { get; set; }
        public DateTime PromotionDate { get; set; }
        public DateTime NextReviewDate { get; set; }
        public string DurationInMonth { get; set; }
        public decimal? GrossSalary { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? HouseRent { get; set; }
        public decimal? Medical { get; set; }
        public decimal? Conveyance { get; set; }
        public decimal? Others { get; set; }
        public bool IsActive { get; set; }
        public Int64? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
