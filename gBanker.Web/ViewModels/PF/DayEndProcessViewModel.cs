using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.PF
{
    public class DayEndProcessViewModel
    {
        [Display(Name = "Is Open")]
        public bool IsOpen { get; set; }
        [Display(Name = "Day Status")]
        public string DayStatus { get; set; }
        [Display(Name = "Transaction Date")]
        public string TransactionDate { get; set; }
        [Display(Name = "System Date")]
        public string SystemDate { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}