using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.PF
{
    public class ProfitDistributionProcessViewModel
    {
        public int TransCategoryId { get; set; }
        public int DeclararionId { get; set; }
        [Display(Name="Profit Rate")]
        public string ProfitRate { get; set; }
        public string YearStartDateStr { get; set; }
        public string YearEndDateStr { get; set; }
        [Display(Name = "Financial Period")]
        public string FinancialPeriod { get; set; }
        
        [Display(Name = "Distribution Amt.")]
        public decimal Distribution { get; set; }
        [Display(Name = "Distribution Year")]
        public bool IsValidDistribution { get; set; }
        [Display(Name = "Status")]
        public string Message { get; set; }
        [Display(Name = "Distribution Year")]
        public string DistributionYear { get; set; }
        [Display(Name = "Transaction Date")]
        public string TransactionDate { get; set; }
        public long CreateUser { get; set; }
        [DataType("smalldatetime")]
        public DateTime? CreateDate { get; set; }
    }
}