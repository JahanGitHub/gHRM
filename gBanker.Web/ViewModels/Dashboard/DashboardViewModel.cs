using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalOfficeCount { get; set; }
        public int TotalOrganizationMemberCount { get; set; }
        public string ZoneName { get; set; }
        public int TotalPO { get; set;}
        public int TotalJoin { get; set; }
        public int TotalLeaveSale { get; set; }
    }    
}