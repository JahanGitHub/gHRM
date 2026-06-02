using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.CodeFirstMigration
{
    [Table("fix.AssetDepreciationInfo")]
    public class AssetDepreciationInfo
    {
        [Key]
        public Int64 AssetDepreciationID { get; set; }
        public DateTime? DeprDate { get; set; }
        public Int64? AssetID { get; set; }
        public string AssetSerial { get; set; }
        public decimal? DepreciatedValue { get; set; }
        public int? OrgID { get; set; }
        public int? OfficeID { get; set; }
        public Int64? DailyTransactionId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        [Required(ErrorMessage = "Required")]
        public string CreateUser { get; set; }
        [Required(ErrorMessage = "Required")]
        public DateTime CreateDate { get; set; }
        public decimal? CurrentBookValue { get; set; }
        public decimal? Cost_OpeningBalance { get; set; }
        public decimal? Cost_AdditionByPurchase { get; set; }
        public decimal? Cost_AdditionForPartialDisposal { get; set; }
        public decimal? Cost_AdditionForOverhauling { get; set; }
        public decimal? Cost_AdditionForRevaluation { get; set; }
        public decimal? Cost_DeductionForDisposal { get; set; }
        public decimal? Cost_ClosingBalance { get; set; }
        public decimal? Dep_OpeningBalance { get; set; }
        public decimal? Dep_PeriodCharge { get; set; }
        public decimal? Dep_AdjustmentForDisposal { get; set; }
        public decimal? Dep_ClosingBalance { get; set; }
        public decimal? RateOfDepreciation { get; set; }
        public string ZoneCode { get; set; }

    }
}
