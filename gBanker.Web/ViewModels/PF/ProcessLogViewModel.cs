using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.PF
{
    public class ProcessLogViewModel: PFBaseModel
    {
        public string ProcessLogId { get; set; }

        [Display(Name = "Start Date")]
        public string StartDate { get; set; }

        [Display(Name = "End Date")]
        public string EndDate { get; set; }



        [Display(Name = "System Date At Day Start")]
        public string SystemDateAtDayStart { get; set; }
        [Display(Name = "System Date At Day End")]
        public string SystemDateAtDayEnd { get; set; }


        [Display(Name = "Is Open")]
        public bool IsOpen { get; set; }
        [Display(Name = "Day Status")]
        public string DayStatus { get; set; }
        [Display(Name = "Transaction Date")]
        public string TransactionDate { get; set; }
    }
}