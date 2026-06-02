using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.Dashboard
{
    public class SSOInstanceViewModel
    {
        public string BaseUrl { get; set; }
        public string EncryptedCredential { get; set; }
        public string Username { get; set; }
    }    
}