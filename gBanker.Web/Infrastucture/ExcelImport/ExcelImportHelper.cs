using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace gHRM.Web.Infrastucture.ExcelImport
{
    public static class ExcelImportHelper
    {
        public static string GetErrorWithRowNo(string CurrentSheetName, int CurrentRowNo, string Message)
        {
            return "Sheet: <b>" + CurrentSheetName + "</b>, Row No: <b>" + CurrentRowNo + "</b>, " + Message;
        }

        public static string GetAllErrorMsg(List<string> ErrorMsgList)
        {
            return ErrorMsgList.Count() > 0 ? "<ul><li>" + string.Join("</li><li>", ErrorMsgList) + "</li></ul>" : "Success";
        }


        public static DateTime? GetDateFromExcelCell2(ExcelWorksheet Sheet, int RowIndex, int ColIndex, string Label, out string Message)
        {
            DateTime? DateData = null;
            Message = "";
            object DateObj = Sheet.Cells[RowIndex, ColIndex].Value;

            try
            {
                if (DateObj != null)
                {
                    if (DateObj is double)
                    {
                        // If the cell contains an OADate (Excel date stored as a number)
                        DateData = DateTime.FromOADate((double)DateObj);
                    }
                    else if (DateObj is DateTime)
                    {
                        // If Excel already treats it as a DateTime object
                        DateData = (DateTime)DateObj;
                    }
                    else
                    {
                        // If stored as text, try parsing with MM/dd/yyyy format
                        string DateStr = DateObj.ToString().Trim();
                        DateData = DateTime.ParseExact(DateStr, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    }
                }
            }
            catch
            {
                Message = $"{Label} must be a valid date with format MM/dd/yyyy on an Excel cell formatted as Text";
            }

            return DateData;
        }


        public static DateTime? GetDateFromExcelCell(ExcelWorksheet Sheet, int RowIndex, int ColIndex, string Label, out string Message)
        {
            DateTime? DateData = null;
            Message = "";
            object DateObj = null;
            try
            {
                DateObj = Sheet.Cells[RowIndex, ColIndex].Value;
                string DateStr = DateObj.ToString().Trim();
                DateData = DateTime.ParseExact(DateStr, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            catch
            {
                /*try
                {
                    if (DateObj is DateTime) DateData = Convert.ToDateTime(DateObj);
                    else DateData = DateTime.FromOADate(double.Parse(DateObj.ToString()));
                }
                catch { }*/
            }
            if (null == DateData)
            {
                Message = Label + " must be a valid data with date format d/M/yyyy on excel cell formatted as Text";
            }
            return DateData;
        }

        public static int? GetIntFromExcelCell(ExcelWorksheet Sheet, int RowIndex, int ColIndex, string Label, out string Message)
        {
            int? NumData = null;
            Message = "";
            object DateObj = null;
            try
            {
                DateObj = Sheet.Cells[RowIndex, ColIndex].Value;
                if (null == DateObj || string.IsNullOrWhiteSpace(DateObj.ToString())) NumData = 0;
                else NumData = Convert.ToInt32(DateObj.ToString());
            }
            catch { }
            if (null == NumData)
            {
                Message = Label + " must be a valid integer number";
            }
            return NumData;
        }

        public static long? GetLongFromExcelCell(ExcelWorksheet Sheet, int RowIndex, int ColIndex, string Label, out string Message)
        {
            long? NumData = null;
            Message = "";
            object DateObj = null;
            try
            {
                DateObj = Sheet.Cells[RowIndex, ColIndex].Value;
                if (null == DateObj || string.IsNullOrWhiteSpace(DateObj.ToString())) NumData = 0;
                else NumData = Convert.ToInt64(DateObj.ToString());
            }
            catch { }
            if (null == NumData)
            {
                Message = Label + " must be a valid long number";
            }
            return NumData;
        }

        public static double? GetDoubleFromExcelCell(ExcelWorksheet Sheet, int RowIndex, int ColIndex, string Label, out string Message)
        {
            double? NumData = null;
            Message = "";
            object DateObj = null;
            try
            {
                DateObj = Sheet.Cells[RowIndex, ColIndex].Value;
                if (null == DateObj || string.IsNullOrWhiteSpace(DateObj.ToString())) NumData = 0;
                else NumData = Convert.ToDouble(DateObj.ToString());
            }
            catch { }
            if (null == NumData)
            {
                Message = Label + " must be a valid double number";
            }
            return NumData;
        }

        public static string GetStringFromExcelCell(ExcelWorksheet Sheet, int RowIndex, int ColIndex, string Label, out string Message)
        {
            string ReturnData = "";
            Message = "";
            object StrObj = null;
            try
            {
                StrObj = Sheet.Cells[RowIndex, ColIndex].Value;
                if (null == StrObj) ReturnData = "";
                else ReturnData = StrObj.ToString();
            }
            catch { }
            return ReturnData;
        }
    }
}