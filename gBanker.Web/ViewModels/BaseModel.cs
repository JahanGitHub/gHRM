using gHRM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class BaseModel
    {
        //public string CreateUser
        //{
        //    get
        //    {
        //        if (HttpContext.Current.User.Identity.IsAuthenticated)
        //            return HttpContext.Current.User.Identity.Name;
        //        else
        //            return "SYSTEM";
        //    }
        //}
        public Int64 CreateUser
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                    return Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                else
                    return 0;
            }
        }
        public DateTime CreateDate { get { return DateTime.Now; } }

        //public Int64 UpdateUser
        //{
        //    get
        //    {
        //        if (HttpContext.Current.User.Identity.IsAuthenticated)
        //            return Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
        //        else
        //            return 0;
        //    }
        //}
        //public DateTime UpdateDate { get { return DateTime.Now; } }
        public Nullable<Int64> UpdateUser { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        
        public Nullable<DateTime> InActiveDate { get; set; }
        public int? boolToInt(bool? value)
        {
            int? returnValue = null; 
            if (value != null && value==true)
            {
                returnValue = 1;
            }else if (value == false)
            {
                returnValue = 0;
            }
            return returnValue;

        }

       // public Nullable<bool> IsActive { get { return IsActive.HasValue ? IsActive : true; } set; }
        //public Nullable<System.DateTime> InActiveDate { get; set { InActiveDate = IsActive.HasValue ? (IsActive.Value == false ? DateTime.Now : default(DateTime?)) : default(DateTime?); } }
    }
}