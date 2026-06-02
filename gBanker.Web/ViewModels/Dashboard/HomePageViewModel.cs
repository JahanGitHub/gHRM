using gHRM.Web.ViewModels.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Dashboard
{
    public class HomePageViewModel
    {
        public HomePageViewModel()
        {
            this.SSOInstances = new List<SSOInstanceViewModel>();
        }
        public List<SSOInstanceViewModel> SSOInstances { get; set; }
        public string ReturnUrl { get; set; }
    }    
}