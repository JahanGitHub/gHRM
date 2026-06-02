using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class AspNetRoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int rowSl { get; set; }

        public List<string> AppClientIDs { get; set; }
    }
}