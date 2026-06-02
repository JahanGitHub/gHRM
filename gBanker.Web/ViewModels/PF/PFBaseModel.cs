//using gHRM.Service.PF;
using gHRM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels.PF
{
    public class PFBaseModel
    {
        
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

        public Nullable<Int64> UpdateUser
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                    return Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                else
                    return 0;
            }
        }
        public Nullable<DateTime> UpdateDate { get { return DateTime.Now; } }

        public bool IsDeleted { get { return false; } }
        public Nullable<Int64> DeletedUser
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                    return Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                else
                    return 0;
            }
        }
        public Nullable<DateTime> DeleteDate { get { return DateTime.Now; } }

        //Newly Added
        public string PFType { get; set; }

        //Day Status
        public string TransactionDate {get;set;}
        public bool IsOpen {get;set;}
        public string DayStatus {get;set;}
        public string SystemDate {get;set;}

    }
}