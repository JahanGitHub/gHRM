using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using gHRM.Web.Filters;
using gHRM.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using gHRM.Data;
using gHRM.Service;
using gHRM.Data.CodeFirstMigration;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using AutoMapper;
using System.Text;

namespace gHRM.Web.Helpers
{
    public class SessionHelper
    {
        public static bool IsAuthenticated
        {
            get { return HttpContext.Current.Request.IsAuthenticated; }
        }
        public static void LogSessionInfo(EmployeeViewModel employee, List<AspNetSecurityModule> parentModules, List<AspNetSecurityModule> userSecurityModules, List<AspNetSecurityModule> allModules)
        {
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[SessionKeys.LOGGEDIN_EMPLOYEE] = employee;
                HttpContext.Current.Session[SessionKeys.PARENT_MODULES] = parentModules;
                HttpContext.Current.Session[SessionKeys.USER_SECURITY_MODULES] = userSecurityModules;
                HttpContext.Current.Session[SessionKeys.All_Modules] = allModules;
            }
        }

        #region gHRM_Session
        public static void EmpSessionInfoAfterLogin(EmployeeViewModel employee)
        {
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[SessionKeys.LOGGEDIN_EMPLOYEE] = employee;
            }
        }
        public static EmployeeViewModel LoggedInEmployeeSession
        {
            get
            {
                if (IsAuthenticated && HttpContext.Current.Session[SessionKeys.LOGGEDIN_EMPLOYEE] != null)
                    return HttpContext.Current.Session[SessionKeys.LOGGEDIN_EMPLOYEE] as EmployeeViewModel;
                else
                    return null;
            }
        }
        public static string EmployeeFullName
        {
            get { return LoggedInEmployeeSession == null ? "" : LoggedInEmployeeSession.EmployeeName; }
        }
        #endregion

        #region From NUPMS
        public static int LoggedInRoleId
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Session[SessionKeys.ROLE_ID].ToString()) ? 0 : Convert.ToInt32(HttpContext.Current.Session[SessionKeys.ROLE_ID]); }
            set { HttpContext.Current.Session[SessionKeys.ROLE_ID] = value; }
        }
        //Added by Asad
        public static int LoggedInOfficeTypeId
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Session[SessionKeys.OfficeType_Id].ToString()) ? default(System.Int32) : Convert.ToInt32(HttpContext.Current.Session[SessionKeys.OfficeType_Id]); }
            set { HttpContext.Current.Session[SessionKeys.OfficeType_Id] = value; }
        }

        public static string CompanyName
        {
            get { return HttpContext.Current.Session[SessionKeys.COMPANY_NAME].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.COMPANY_NAME] = value; }
        }

        public static string CompanyAddress
        {
            get { return HttpContext.Current.Session[SessionKeys.COMPANY_ADDRESS].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.COMPANY_ADDRESS] = value; }
        }

        public static string CompanyImage
        {
            get { return HttpContext.Current.Session[SessionKeys.COMPANY_IMAGE].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.COMPANY_IMAGE] = value; }
        }

        public static string CompanySignature
        {
            get { return HttpContext.Current.Session[SessionKeys.COMPANY_SIGNATURE].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.COMPANY_SIGNATURE] = value; }
        }

        public static string PayrollConfigurationType
        {
            get { return HttpContext.Current.Session[SessionKeys.PAYROLL_CONFIGURATION_TYPE].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.PAYROLL_CONFIGURATION_TYPE] = value; }
        }
        public static string NoOfSalaryDays
        {
            get { return HttpContext.Current.Session[SessionKeys.NO_OF_SALARY_DAYS].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.NO_OF_SALARY_DAYS] = value; }
        }
        public static string PayrollType
        {
            get { return HttpContext.Current.Session[SessionKeys.PAYROLL_TYPE].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.PAYROLL_TYPE] = value; }
        } 
        public static bool EnabledSSOLogin
        {
            get { return Convert.ToBoolean(HttpContext.Current.Session[SessionKeys.Enabled_SSO_LOING]); }
            set { HttpContext.Current.Session[SessionKeys.Enabled_SSO_LOING] = value; }
        }
      
        #endregion

        #region Company Info

        public static void TrackCompanyInfoSession(Company company)
        {
            if (HttpContext.Current.Session != null)
                HttpContext.Current.Session[SessionKeys.GHRMPLUS_COMPANYINFO] = company;
        }

        public static Company CompanyInfo
        {
            get
            {
                if (IsAuthenticated && HttpContext.Current.Session[SessionKeys.GHRMPLUS_COMPANYINFO] != null)
                    return HttpContext.Current.Session[SessionKeys.GHRMPLUS_COMPANYINFO] as Company;
                else
                    return null;
            }
        }

        #endregion
       
        public static EmployeeViewModel LoggedInEmployee
        {
            get
            {
                if (IsAuthenticated && HttpContext.Current.Session[SessionKeys.LOGGEDIN_EMPLOYEE] != null)
                    return HttpContext.Current.Session[SessionKeys.LOGGEDIN_EMPLOYEE] as EmployeeViewModel;
                else
                    return null;
            }
        }

        public static OfficeViewModel LoggedInOfficeDetail
        {
            get
            {
                return HttpContext.Current.Session[SessionKeys.LOGGED_IN_OFFICE_DETAIL] as OfficeViewModel;

            }
            set { HttpContext.Current.Session[SessionKeys.LOGGED_IN_OFFICE_DETAIL] = value; }
        }
        public static List<AspNetSecurityModule> AllPrentModules
        {
            get
            {
                var allParentModules = HttpContext.Current.Session[SessionKeys.PARENT_MODULES];

                if (IsAuthenticated && HttpContext.Current.Session[SessionKeys.PARENT_MODULES] != null)
                    return HttpContext.Current.Session[SessionKeys.PARENT_MODULES] as List<AspNetSecurityModule>;
                else
                    return null;
            }
        }
        public static List<AspNetSecurityModule> UserSecurityModules
        {
            get
            {
                var allSecurityModules = HttpContext.Current.Session[SessionKeys.USER_SECURITY_MODULES];

                if (IsAuthenticated && HttpContext.Current.Session[SessionKeys.USER_SECURITY_MODULES] != null)
                    return HttpContext.Current.Session[SessionKeys.USER_SECURITY_MODULES] as List<AspNetSecurityModule>;
                else
                    return null;
            }
        }

        public static Int64? LoginUserEmployeeId { get { return LoggedInEmployee == null ? default(System.Nullable<Int64>) : LoggedInEmployee.EmployeeId; } }
        public static string UserFullName
        {
            get { return LoggedInEmployee == null ? "" : LoggedInEmployee.EmployeeName; }
        }
        public static Int64? LoggedInEmployeeID { get { return LoggedInEmployee == null ? default(System.Nullable<Int64>) : LoggedInEmployee.EmployeeId; } }
        public static Int32? LoggedInEmployeeDepartmentId { get { return LoggedInEmployee == null ? default(System.Nullable<Int32>) : LoggedInEmployee.DepartmentId; } }
        public static DateTime TransactionDate
        {
            get { return DateTime.Parse(HttpContext.Current.Session[SessionKeys.TRANSACTION_DATE].ToString()); }
            set { HttpContext.Current.Session[SessionKeys.TRANSACTION_DATE] = value; }
        }
        public static DateTime? LastDayEndDate
        {

            get
            {
                if (HttpContext.Current.Session[SessionKeys.LASTDAYEND_DATE] != null)
                    return DateTime.Parse(HttpContext.Current.Session[SessionKeys.LASTDAYEND_DATE].ToString());
                else
                {
                    return default(DateTime?);
                }
            }
            set { HttpContext.Current.Session[SessionKeys.LASTDAYEND_DATE] = value; }
        }
        public static string OrganizationName
        {
            get { return HttpContext.Current.Session[SessionKeys.ORGANIZATION_NAME].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.ORGANIZATION_NAME] = value; }
        }

        public static string UserName
        {
            get { return HttpContext.Current.Session[SessionKeys.USER_NAME].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.USER_NAME] = value; }
        }
        public static string UserPassword
        {
            get { return HttpContext.Current.Session[SessionKeys.USERPASSWORD].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.USERPASSWORD] = value; }
        }
        public static string CompanyCode
        {
            get { return HttpContext.Current.Session[SessionKeys.COMPANYCODE].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.COMPANYCODE] = value; }
        }
        public static int? CompanyID
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Session[SessionKeys.COMPANY_ID].ToString()) ? default(System.Nullable<Int32>) : Convert.ToInt32(HttpContext.Current.Session[SessionKeys.COMPANY_ID]); }
            set { HttpContext.Current.Session[SessionKeys.COMPANY_ID] = value; }
        }
        public static int? CountryID
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Session[SessionKeys.COUNTRY_ID].ToString()) ? default(System.Nullable<Int32>) : Convert.ToInt32(HttpContext.Current.Session[SessionKeys.COUNTRY_ID]); }
            set { HttpContext.Current.Session[SessionKeys.COUNTRY_ID] = value; }
        }
        public static int? LoginUserOfficeID
        {
            get
            {

                return HttpContext.Current.Session[SessionKeys.LOGGED_IN_OFFICE_ID] == null ? default(System.Nullable<Int32>) : Convert.ToInt32(HttpContext.Current.Session[SessionKeys.LOGGED_IN_OFFICE_ID]);
            }
            set { HttpContext.Current.Session[SessionKeys.LOGGED_IN_OFFICE_ID] = value; }
        }
        public static int? LoginUserOfficeType
        {
            get { return string.IsNullOrEmpty(HttpContext.Current.Session[SessionKeys.LOGGED_IN_OFFICE_TYPE].ToString()) ? default(System.Nullable<Int32>) : Convert.ToInt32(HttpContext.Current.Session[SessionKeys.LOGGED_IN_OFFICE_TYPE]); }
            set { HttpContext.Current.Session[SessionKeys.LOGGED_IN_OFFICE_TYPE] = value; }
        }
        public static int? LoginUserOfficeLevel
        {
            get { return HttpContext.Current.Session[SessionKeys.LOGIN_USER_OFFICE_LEVEL]==null || string.IsNullOrEmpty(HttpContext.Current.Session[SessionKeys.LOGIN_USER_OFFICE_LEVEL].ToString()) ? default(System.Nullable<Int32>) : Convert.ToInt32(HttpContext.Current.Session[SessionKeys.LOGIN_USER_OFFICE_LEVEL]); }
            set { HttpContext.Current.Session[SessionKeys.LOGIN_USER_OFFICE_LEVEL] = value; }
        }        
        public static string ProcessType
        {
            get
            {
                if (HttpContext.Current.Session[SessionKeys.PROCESS_TYPE] != null)
                    return HttpContext.Current.Session[SessionKeys.PROCESS_TYPE].ToString();

                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session[SessionKeys.PROCESS_TYPE] = value;
            }
        }
        public static string UserNameType
        {
            get { return HttpContext.Current.Session[SessionKeys.USER_NAME_TYPE].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.USER_NAME_TYPE] = value; }
        }
        public static string TransactionDay
        {
            get { return HttpContext.Current.Session[SessionKeys.TRANSACTION_DAY].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.TRANSACTION_DAY] = value; }
        }
        public static bool IsDayInitiated
        {
            get
            {
                if (HttpContext.Current.Session[SessionKeys.IS_DAY_INITIATED] != null)
                    return (bool)HttpContext.Current.Session[SessionKeys.IS_DAY_INITIATED];
                return false;

            }
            set { HttpContext.Current.Session[SessionKeys.IS_DAY_INITIATED] = value; }
        }
        public static string TransactionDashBoardString
        {
            get
            {
                var detail = new StringBuilder();
                if (IsDayInitiated && HttpContext.Current.Session[SessionKeys.TRANSACTION_DATE] != null && TransactionDate != default(DateTime))
                    detail.Append(string.Format("Transation Date:{0} | Day:{1} | ", TransactionDate.ToString("dd MMM, yyyy"), TransactionDay));
                else
                    detail.Append(" No Transation Day Initiated | ");
                if (HttpContext.Current.Session[SessionKeys.LASTDAYEND_DATE] != null && LastDayEndDate != default(DateTime?))
                    detail.Append(string.Format(" Last Day End:{0} | ", LastDayEndDate.Value.ToString("dd MMM, yyyy")));
                if (LoggedInOfficeDetail != null)
                    detail.Append(string.Format(" Office: {0} - {1} ", LoggedInOfficeDetail.OfficeCode, LoggedInOfficeDetail.OfficeName));

                // Working Office:<span id="officeName"> @string.Format("{0} - {1}", gHRM.Web.Helpers.SessionHelper.LoggedInOfficeDetail.OfficeCode, gHRM.Web.Helpers.SessionHelper.LoggedInOfficeDetail.OfficeName)</span>
                return detail.ToString();
            }
        }
        public static string CurrentModuleKeys
        {
            get { return HttpContext.Current.Session[SessionKeys.Current_Module_Keys].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.Current_Module_Keys] = value; }
        }
        public static List<AspNetSecurityModule> AllModules
        {
            get
            {
                if (IsAuthenticated && HttpContext.Current.Session[SessionKeys.All_Modules] != null)
                    return HttpContext.Current.Session[SessionKeys.All_Modules] as List<AspNetSecurityModule>;
                else
                    return null;
            }
        }

        #region SSO

        public static string SSOEncryptedUserCredential
        {
            get
            {
                if (HttpContext.Current.Session[SessionKeys.SSO_ENCRYPTED_USERCREDENTIAL] != null)
                    return HttpContext.Current.Session[SessionKeys.SSO_ENCRYPTED_USERCREDENTIAL].ToString();

                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session[SessionKeys.SSO_ENCRYPTED_USERCREDENTIAL] = value;
            }
        }
        public static string SSOUsername
        {
            get
            {
                if (HttpContext.Current.Session[SessionKeys.SSO_USERNAME] != null)
                    return HttpContext.Current.Session[SessionKeys.SSO_USERNAME].ToString();

                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session[SessionKeys.SSO_USERNAME] = value;
            }
        }
        public static string SSOReturnUrl
        {
            get
            {
                if (HttpContext.Current.Session[SessionKeys.SSO_RETURN_URL] != null)
                    return HttpContext.Current.Session[SessionKeys.SSO_RETURN_URL].ToString();

                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session[SessionKeys.SSO_RETURN_URL] = value;
            }
        }
        public static bool SSOLogout
        {
            get
            {
                if (HttpContext.Current.Session[SessionKeys.SSO_LOGOUT] != null)
                    return Convert.ToBoolean(HttpContext.Current.Session[SessionKeys.SSO_LOGOUT]);

                return false;
            }
            set
            {
                HttpContext.Current.Session[SessionKeys.SSO_LOGOUT] = value;
            }
        }

        #endregion
    }
    public class SessionKeys
    {
        public const string ROLE_ID = "ROLE_ID";
        public const string OfficeType_Id = "OfficeType_Id";
        public const string COMPANY_NAME = "COMPANY_NAME";
        public const string COMPANY_ADDRESS = "COMPANY_ADDRESS";
        public const string COMPANY_IMAGE = "COMPANY_IMAGE";
        public const string COMPANY_SIGNATURE = "COMPANY_SIGNATURE";
        public const string LOGGEDIN_EMPLOYEE = "LOGGEDIN_EMPLOYEE";
        public const string USER_CENTER_ID = "USER_CENTER_ID";
        public const string TRANSACTION_DATE = "TRANSACTION_DATE";
        public const string TRANSACTION_DAY = "TRANSACTION_DAY";
        public const string IS_DAY_INITIATED = "IS_DAY_INITIATED";
        public const string ORGANIZATION_NAME = "ORGANIZATION_NAME";
        public const string COMPANYCODE = "COMPANYCODE";
        public const string PROCESS_TYPE = "PROCESS_TYPE";
        public const string PARENT_MODULES = "PARENT_MODULES";
        public const string USER_SECURITY_MODULES = "ROLE_MODULES";
        public const string LOGGED_IN_OFFICE_ID = "LOGGED_IN_OFFICE_ID";        
        public const string LOGGED_IN_OFFICE_TYPE = "LOGGED_IN_OFFICE_TYPE";
        public const string LOGIN_USER_OFFICE_LEVEL = "LOGIN_USER_OFFICE_LEVEL";
        public const string LOGGED_IN_OFFICE_DETAIL = "LOGGED_IN_OFFICE_DETAIL";
        public const string LASTDAYEND_DATE = "LASTDAYEND_DATE";
        public const string COMPANY_ID = "COMPANY_ID";
        public const string COUNTRY_ID = "COUNTRY_ID";
        public const string USER_NAME_TYPE = "USER_NAME_TYPE";
        public const string Current_Module_Keys = "Current_Module_Keys";
        public const string All_Modules = "All_Modules";
        public const string PAYROLL_CONFIGURATION_TYPE = "PayrollConfigurationType";
        public const string NO_OF_SALARY_DAYS = "NoOfSalaryDays";
        public const string PAYROLL_TYPE = "PayrollType";
        public const string USER_NAME = "UserName";
        public const string USERPASSWORD = "UserPassword";

        //company info
        public const string GHRMPLUS_COMPANYINFO = "GHRMPLUS_COMPANYINFO";


        //SSO related
        public const string Enabled_SSO_LOING = "Enabled_SSO_LOING";
        public const string SSO_ENCRYPTED_USERCREDENTIAL = "SSO_ENCRYPTED_USERCREDENTIAL";
        public const string SSO_USERNAME = "SSO_USERNAME";
        public const string SSO_RETURN_URL = "SSO_RETURN_URL";
        public const string SSO_LOGOUT = "SSO_LOGOUT";
    }
}