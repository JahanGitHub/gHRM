using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class GratuityApproveViewModel
    {
        public int FromYear { get; set; }
        public int FromMonth { get; set; }
        public string ApproveDate { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
    }
}