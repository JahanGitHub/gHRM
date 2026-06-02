using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.PF
{
    public class YearEndProcessViewModel
    {
        [Display(Name = "Transaction Date")]
        public string TransactionDate { get; set; }

        [Display(Name = "Year Start Date")]
        public string YearStartDate { get; set; }
        
        [Display(Name = "Year End Date")]
        public string YearEndDate { get; set; }
        [Display(Name = "Day Status")]
        public string DayStatus { get; set; }
        public bool IsOpen { get; set; }

        public bool IsValidYearEnd { get; set; }
        [Display(Name = "Year End Status")]
        public string YearEndStatus { get; set; }
        
        public long CreateUser { get; set; }
        [DataType("smalldatetime")]
        public DateTime? CreateDate { get; set; }
    }
}