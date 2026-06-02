
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class DbDetailBaseModel
    { 
        public DateTime CreateDate { get { return DateTime.Now; } }
       
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
    }
}