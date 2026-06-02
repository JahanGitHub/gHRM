using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Core.Utilities
{
    public static class CommonHelper
    { 
        public static string GetFormattedEmployeeCodeWithSixDigit(string employeeCode)
        {
            int realEmployeeCode;
            int.TryParse(employeeCode, out realEmployeeCode);

            if (realEmployeeCode > 0)
            {
                if (realEmployeeCode.ToString().Length == 1)
                    return $"00000{realEmployeeCode}";
                else if (realEmployeeCode.ToString().Length == 2)
                    return $"0000{realEmployeeCode}";
                else if (realEmployeeCode.ToString().Length == 3)
                    return $"000{realEmployeeCode}";
                else if (realEmployeeCode.ToString().Length == 4)
                    return $"00{realEmployeeCode}";
                else if (realEmployeeCode.ToString().Length == 5)
                    return $"0{realEmployeeCode}";
                else
                    return $"{realEmployeeCode}";
            }

            return employeeCode;
        }

        public static string GetFormattedEmployeeCodeWithFourDigit(string employeeCode)
        {
            int realEmployeeCode;
            int.TryParse(employeeCode, out realEmployeeCode);

            if (realEmployeeCode > 0)
            {
                if (realEmployeeCode.ToString().Length == 1)
                    return $"000{realEmployeeCode}";
                else if (realEmployeeCode.ToString().Length == 2)
                    return $"00{realEmployeeCode}";
                else if (realEmployeeCode.ToString().Length == 3)
                    return $"0{realEmployeeCode}";
                else
                    return $"{realEmployeeCode}";
            }

            return employeeCode;
        }

        public static string GetFormattedEmployeeCodeWithFiveDigit(string employeeCode)
        {
            int realEmployeeCode;
            int.TryParse(employeeCode, out realEmployeeCode);

            if (realEmployeeCode > 0)
            {
                if (realEmployeeCode.ToString().Length == 1)
                    return $"00{realEmployeeCode}";
                else if (realEmployeeCode.ToString().Length == 2)
                    return $"0{realEmployeeCode}";              
                else
                    return $"{realEmployeeCode}";
            }

            return employeeCode;
        }

        public static string DEV_VERSION()
        {
            string Result = "1.0.0";
            try
            {
                Result = System.Configuration.ConfigurationManager.AppSettings["DEV_VERSION"].ToString();
            }
            catch { }
            return Result;
        }

        public static string RandomString(Random random, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
