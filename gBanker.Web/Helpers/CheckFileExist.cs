using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.Helpers
{
    public class CheckFileExist
    {
        public string IsFileExist(string fileLocation)
        {
            //var url = HttpContext.Current.Server.MapPath("~/") + fileLocation;
            //bool exists = System.IO.File.Exists(url);
            //if (exists == true)
            //{
            //    return true;
            //}else{
            //   return false;
            //}

            var url = HttpContext.Current.Server.MapPath("~/") + fileLocation;
            //url = url.Replace("\\", @"\");
            bool exists = System.IO.File.Exists(url);
            if (exists == true)
            {
                return url;
            }
            else
            {
                return "";
            }

        }
    }
}