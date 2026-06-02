using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace gHRM.Web.Models
{
    public class clsConnection
    {
        public static SqlConnection Con = new SqlConnection(ConfigurationManager.ConnectionStrings["gHRMConnectionString"].ConnectionString);
       // public static string OrganizationName = ConfigurationManager.AppSettings["OrganizationName"];
        public static Stream oStream;
    }
}