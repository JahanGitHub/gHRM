using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.ViewModels
{
    public class NoticePayProcessViewModel
    {
        public int OfficeTypeId { get; set; }
        public int OfficeId { get; set; }
        public int FromYear { get; set; }
        public int FromMonth { get; set; }
        public string EmployeeName { get; set; }
        public string ProcessDate { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        public IEnumerable<SelectListItem> AreaList { get; set; }
        public IEnumerable<SelectListItem> UnitList { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> OfficeTypeList { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
    }
}