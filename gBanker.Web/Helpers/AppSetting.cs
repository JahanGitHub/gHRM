using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.Helpers
{
    public static class AppSetting
    {
        public const string LEAVE_AUTO_ADJUSTMENT_DISABLED = "LEAVE_AUTO_ADJUSTMENT_DISABLED";
        public const string LEAVE_ENTRY_PAGE_SHOW_LEAVE_DAY_DURATION = "LEAVE_ENTRY_PAGE_SHOW_LEAVE_DAY_DURATION";
        public const string OFFICE_EDIT_PAGE_OFFICE_MAPPING_ENABLED = "OFFICE_EDIT_PAGE_OFFICE_MAPPING_ENABLED";
        public const string EMPLOYEE_PERSONAL_REPORT_PAGE_EMPLOYEE_CODE_MODIFY_ALLOW_FOR_USER_ROLE = "EMPLOYEE_PERSONAL_REPORT_PAGE_EMPLOYEE_CODE_MODIFY_ALLOW_FOR_USER_ROLE";
        public const string LEAVE_TYPE_TOTAL_MAX_LEAVE_DAYS_ENABLED = "LEAVE_TYPE_TOTAL_MAX_LEAVE_DAYS_ENABLED";
        public const string SERVICE_BOOK_REPORT_HEIGHT_CONVERT_INCHES_TO_CM = "SERVICE_BOOK_REPORT_HEIGHT_CONVERT_INCHES_TO_CM";
        public const string EMPLOYEE_EDIT_PAGE_CODE_EDIT_ENABLED = "EMPLOYEE_EDIT_PAGE_CODE_EDIT_ENABLED";
        public const string EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE = "EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE";
        public const string DASHBOARD_ENABLED = "DASHBOARD_ENABLED";
        public const string SUPER_ADMIN_EMPLOYEEID = "SUPER_ADMIN_EMPLOYEEID";
        public const string FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE = "FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE";
        public const string ERP_STRUCTURE = "ERP_STRUCTURE";

        // Schedule 
        public const string ScheduleEnable = "ScheduleEnable";
        public const string DailyLateInScheduleEnable = "DailyLateInScheduleEnable";
        public const string AbsentScheduleEnable = "AbsentScheduleEnable";
        public const string PunchNotFoundScheduleEnable = "PunchNotFoundScheduleEnable";
        public const string MonthlyCongratulationScheduleEnable = "MonthlyCongratulationScheduleEnable";
        public const string LogoutScheduleEnable = "LogoutScheduleEnable";

        public static string Get(string Name, HttpContextBase Context)
        {
            return GetData(Name, Context);
        }

        public static bool GetBool(string Name, HttpContextBase Context)
        {
            return "true" == GetData(Name, Context).ToLower();
        }

        private static string GetData(string Name, HttpContextBase Context)
        {
            Dictionary<string, object> ItemList = GetItemDefaultList();

            if (ItemList.ContainsKey(Name))
            {
                string DefaultValue = ItemList[Name].ToString();
                string Value = GetSetting(Name, Context);
                if ("" == Value) return DefaultValue;
                return Value;
            }
            return "";
        }

        public static Dictionary<string, object> GetItemDefaultList()
        {
            Dictionary<string, object> ItemList = new Dictionary<string, object>();
            ItemList[LEAVE_AUTO_ADJUSTMENT_DISABLED] = false;
            ItemList[LEAVE_ENTRY_PAGE_SHOW_LEAVE_DAY_DURATION] = false;
            ItemList[OFFICE_EDIT_PAGE_OFFICE_MAPPING_ENABLED] = false;
            ItemList[EMPLOYEE_PERSONAL_REPORT_PAGE_EMPLOYEE_CODE_MODIFY_ALLOW_FOR_USER_ROLE] = "";
            ItemList[LEAVE_TYPE_TOTAL_MAX_LEAVE_DAYS_ENABLED] = false;
            ItemList[SERVICE_BOOK_REPORT_HEIGHT_CONVERT_INCHES_TO_CM] = false;
            ItemList[EMPLOYEE_EDIT_PAGE_CODE_EDIT_ENABLED] = false;
            ItemList[EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE] = "";
            ItemList[DASHBOARD_ENABLED] = false;
            ItemList[SUPER_ADMIN_EMPLOYEEID] = "";
            ItemList[FUND_TRANSFER_APPLICATION_AND_ADVICE_TO_BANK_REPORT_TEMPLATE] = "";
            ItemList[ERP_STRUCTURE] = false;

            ItemList[ScheduleEnable] = false;
            ItemList[DailyLateInScheduleEnable] = false;
            ItemList[AbsentScheduleEnable] = false;
            ItemList[PunchNotFoundScheduleEnable] = false;
            ItemList[MonthlyCongratulationScheduleEnable] = false;
            ItemList[LogoutScheduleEnable] = false;
            return ItemList;
        }

        private static string GetSetting(string Name, HttpContextBase Context)
        {
            try
            {
                string AppSettingStr = "";
                if (Context == null)
                    AppSettingStr = System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~") + "/App_Data/AppSetting.json");
                else
                    AppSettingStr = System.IO.File.ReadAllText(Context.Server.MapPath("~") + "/App_Data/AppSetting.json");

                if ("" == AppSettingStr) return AppSettingStr;
                Dictionary<string, string> DataList = JsonConvert.DeserializeObject<Dictionary<string, string>>(AppSettingStr);
                return DataList[Name];
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}