#region Usings
using AutoMapper;
using gHRM.Data;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Web.CommonDropdown;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
#endregion

namespace gHRM.Web.Infrastucture.Framework
{
    public class WorkContext : IWorkContext
    {
        #region Private Members

        private UserManager<ApplicationUser> userManager;
        private readonly IEmployeeService employeeService;
        private readonly ICompanyService companyService;
        private readonly IOfficeService officeService;
        private readonly ISecurityService securityService;
        public CommonDynamicDropDown commonDynamicDropDown;

        #endregion

        #region Ctor

        public WorkContext(
              UserManager<ApplicationUser> userManager
            , IEmployeeService employeeService
            , ICompanyService companyService
            , IOfficeService officeService
            , ISecurityService securityService
            )
        {
            this.userManager = userManager;
            this.employeeService = employeeService;
            this.securityService = securityService;
            this.companyService = companyService;
            this.officeService = officeService;

            commonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion

        #region Is Authenticated

        public bool IsAuthenticated
        {
            get { return CheckUserIsAuthenticated(); }
            set { }
        }


        #endregion

        #region Is Session Exist By Key

        public bool IsSessionExistByKey(string sessionKey)
        {
            return System.Web.HttpContext.Current.Session[sessionKey] != null ? true : false;
        }


        #endregion

        #region Set Current User Session

        public bool SetCurrentUserSession()
        {
            var isAuthenticated = CheckUserIsAuthenticated();
            if (!isAuthenticated)
                return false;

            var formsIdentity = (FormsIdentity)HttpContext.Current.User.Identity;

            var user = userManager.FindById(formsIdentity.Ticket.Name);
            if (user == null)
                return false;

            var employee = employeeService.GetByEmpId(Convert.ToInt64(user.EmployeeId));
            if (employee == null)
                return false;

            var entity = Mapper.Map<Employee, EmployeeViewModel>(employee);
            var offc = officeService.GetById(Convert.ToInt32(employee.OfficeId));
            var comp = companyService.GetById(Convert.ToInt32(offc.CompanyId));

            // ******** for menu create **************
            var parentModules = securityService.GetAllPrentModule().ToList();
            var allModules = commonDynamicDropDown.getRoleWiseChildMenu(0);
            var userModules = securityService.GeAllRoleModules(user.RoleId).ToList();
            SessionHelper.LogSessionInfo(entity, parentModules, userModules, allModules);
            //***********************************

            if (employee.EmployeeName.Contains("Super Admin"))
                SessionHelper.UserNameType = "SA";
            else
                SessionHelper.UserNameType = "E";

            SessionHelper.OrganizationName = comp.CompanyName;
            SessionHelper.CompanyID = comp.CompanyId;
            SessionHelper.CountryID = comp.CountryId;
            SessionHelper.CompanyCode = comp.CompanyCode;
            SessionHelper.LoginUserOfficeID = offc.OfficeId;
            SessionHelper.LoginUserOfficeType = offc.OfficeTypeId;

            SessionHelper.LoggedInOfficeTypeId = Convert.ToInt32(offc.OfficeTypeId);
            SessionHelper.LoggedInRoleId = Convert.ToInt32(user.RoleId);
            SessionHelper.CompanyName = comp.CompanyName;
            SessionHelper.CompanyAddress = comp.CompanyAddress;
            SessionHelper.EmpSessionInfoAfterLogin(entity);

            return true;
        }

        #endregion

        #region Private Methods

        private bool CheckUserIsAuthenticated()
        {
            return SessionHelper.IsAuthenticated;
        }

        #endregion
    }
}