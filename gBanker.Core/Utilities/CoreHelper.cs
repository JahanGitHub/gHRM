using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace gHRM.Core.Utilities
{
    public static class CoreHelper
    {
        public static byte[] ImagePathToByte<T>(this T partialPath)
        {
            if (partialPath==null)
               return null;

            string path = HttpContext.Current.Server.MapPath($"~/{partialPath}");

            if (!File.Exists(path))
                return null;

            var imageInByte = File.ReadAllBytes(path);
            return imageInByte;
        }      


        public static DataTable ToDataTable<T>(this IList<T> list)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in list)
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                table.Rows.Add(values);
            }
            return table;
        }

        public static string ToAppSettingValue<T>(this T key)
        {
            try
            {
                if(key==null)
                    return string.Empty;

                var appSettingValue = string.Empty;

                appSettingValue = ConfigurationManager.AppSettings[key.ToString()].ToString();

                return appSettingValue;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
