using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace gHRM.Web.Helpers
{
    public class ApplicationSettings
    {
        public static string OrganiztionName { get { return string.IsNullOrEmpty(ConfigurationManager.AppSettings["OrgName"]) ? "Grameen Communcations" : ConfigurationManager.AppSettings["OrgName"]; } }
        public static string ColDay { get { return ConfigurationManager.AppSettings["ColDay"]; } }

    }
}