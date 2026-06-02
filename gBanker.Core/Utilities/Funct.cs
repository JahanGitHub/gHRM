using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Core.Utilities
{
    public static class Funct
    {
        public static int GetEncashedAmount(decimal BasicSalary, int Days)
        {
            return Convert.ToInt32(Math.Round((BasicSalary / 30) * Days, 0));
        }

        public static string GetError(Exception ex)
        {
            string InnerExText = "An error occurred while updating the entries. See the inner exception for details.";

            if (ex.Message == InnerExText && ex.InnerException != null)
            {
                return GetError(ex.InnerException);
            }
            return ex.Message;
        }
    }
}
