using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels.Payroll
{
    public class FestivalBonusCalendarViewModel
    {
        public int Id { get; set; }
        public string ComponentId { get; set; }
        public int Year { get; set; }
        public string Month { get; set; }
        public int MonthNo { get; set; }
        public List<SelectListItem> ComponentList { get; set; }
        public List<SelectListItem> YearList { get; set; }
        public List<SelectListItem> MonthList { get; set; }
        
    }
}