using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Basic
{
    public class AttHolidayTypeViewModel
    {
        public int AttHolidayTypeId { get; set; }
        public string HolidayTypeShortName { get; set; }
        public string HolidayTypeFullName { get; set; }
        public bool IsActive { get; set; }
        public DateTime InActiveDate { get; set; }
        public long CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public long UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}