
#region Usings

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
using gHRM.Web.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using gHRM.Data;
using gHRM.Service;
using gHRM.Data.CodeFirstMigration;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using AutoMapper;
using gHRM.Web.Filters;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gHRM.Service.StoreProcedure;
using System.Data;
using gHRM.Web.CommonDropdown;
using gHRM.Core.Utilities.Constants;
using System.Configuration;
using gHRM.Core.Utilities.Encryption;
using gHRM.Web.ViewModels.Dashboard;
using gHRM.Data.DBDetailModels.Security;
using gHRM.Service.Payroll;
using gHRM.Core.Utilities;
using System.Net;
using System.Text;

#endregion

namespace gHRM.Web.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        #region Private Members
        private IAuthenticationManager _authnManager;
        private UserManager<ApplicationUser> UserManager;
        private readonly IEmployeeService employeeService;
        private readonly IAspNetRoleService roleService;
        private readonly ICompanyService companyService;
        private readonly IOfficeService officeService;
        private readonly ISecurityService securityService;
        private readonly IAspNetUserService aspNetUserService;
        private readonly IAspAdminPasswordTableService aspAdminPasswordTableService;
        private readonly ISingleSignOnTrackingService singleSignOnTrackingService;
        private readonly IUserService userService;
        private readonly ICompanyWisePayrollConfigService companyWisePayrollConfigService;
        private readonly ILogger loggger;
        private readonly IKeyCloakService keyCloakService;
        private readonly IEmployeeSPService employeeSPService;
        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;

        #endregion

        #region Ctor

        public AccountController(
              IEmployeeService employeeService
            , IAspNetRoleService roleService
            , ISecurityService securityService
            , IAspNetUserService aspNetUserService
            , ILogger loggger
            , IUserService userService
            , ICompanyService companyService
            , IOfficeService officeService
            , IAspAdminPasswordTableService aspAdminPasswordTableService
            , ISingleSignOnTrackingService singleSignOnTrackingService
            , IEmployeeSPService employeeSPService
            , UserManager<ApplicationUser> userManager
            , IKeyCloakService keyCloakService
            , ICompanyWisePayrollConfigService companyWisePayrollConfigService
            )
        {
            this.UserManager = userManager;
            this.employeeService = employeeService;
            this.roleService = roleService;
            this.securityService = securityService;
            this.aspNetUserService = aspNetUserService;
            this.loggger = loggger;
            this.keyCloakService = keyCloakService;
            this.companyService = companyService;
            this.officeService = officeService;
            this.aspAdminPasswordTableService = aspAdminPasswordTableService;
            this.employeeSPService = employeeSPService;
            this.singleSignOnTrackingService = singleSignOnTrackingService;
            this.userService = userService;
            this.companyWisePayrollConfigService = companyWisePayrollConfigService;

            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion

        #region Security Actions

        public ActionResult Index()
        {
            LogRequest();
            return View();
        }

        [SessionExpireFilter]
        [DisableCache]
        public async Task<ActionResult> DeleteLogin(string Id)
        {
            try
            {
                var aspNetUser = aspNetUserService.Get(x => x.Id == Id);

                if (aspNetUser != null)
                {
                    aspNetUserService.DeleteLogin(Id);

                    if (SessionHelper.EnabledSSOLogin)
                    {
                        //let's inactive auth user
                        await InactiveAuthUser(aspNetUser.UserName);
                    }
                }

                return View("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [SessionExpireFilter]
        [DisableCache]
        public JsonResult EditUserRole(AspNetUser user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Id) || user.RoleId <= 0)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "Form is not valid! " +
                          "Please correct it and try again."
                    });
                }
                var dbUser = aspNetUserService.GetByUserId(user.Id);
                if (dbUser != null)
                {
                    dbUser.RoleId = user.RoleId;
                    aspNetUserService.Update(dbUser);
                }
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [SessionExpireFilter]
        [DisableCache]
        public JsonResult RoleList(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var selectedRles = roleService.GetMany(w => w.Id == id).Select(s => new { DisplayText = s.Name, Value = s.Id }).ToList();
                return new JsonResult() { Data = new { Result = "OK", Options = selectedRles }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var allRoles = roleService.GetAll().Select(s => new { DisplayText = s.Name, Value = s.Id }).ToList();
                return new JsonResult() { Data = new { Result = "OK", Options = allRoles }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public ActionResult getUserDashboard([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var userList = employeeSPService.GetDataWithoutParameter("secu.SP_Get_UsersList");
                var List_ViewModel = userList.Tables[0].AsEnumerable()
                .Select(row => new AspNetUser()
                {
                    Id = row.Field<string>("Id"),
                    EmployeeId = row.Field<long>("AccessFailedCount"),
                    UserName = row.Field<string>("UserName"),
                    FirstName = row.Field<string>("FirstName"),
                    RoleId = row.Field<int>("RoleId"),
                    RoleName = row.Field<string>("Name")

                }).ToList();
                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [SessionExpireFilter]
        [DisableCache]
        public ActionResult GetAllLogins(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                var allentities = aspNetUserService.GetAll().ToList();
                var totalCount = allentities.Count();
                var entities = allentities.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = entities, TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        //
        // GET: /Account/Login

        #endregion


        #region Login
        public async Task<ActionResult> Login(string returnUrl = "", bool logout = false)
        {

            var model = new LoginModel
            {
                ReturnUrl = returnUrl
            };

            //can show login panel
            bool canShowLoginPanel = CanShowLoginPanel();

            model.CanShowLoginPanel = canShowLoginPanel;

            //Get single signon identifier
            var encryptedUserCredential = singleSignOnTrackingService.GetSingleSignOnIdentifier();

            if (string.IsNullOrWhiteSpace(encryptedUserCredential))
                return View(model);

            var splitedBy = "_And_";
            var splitedCredential = encryptedUserCredential.Split(new string[] { splitedBy }, StringSplitOptions.None);

            if (splitedCredential == null || splitedCredential.Length < 2)
                return View(model);

            string cipherPassword = splitedCredential[0].ToString();
            string username = splitedCredential[1].ToString();

            var encryptedPassword = cipherPassword.Replace("slash", "/").Replace("plus", "+");
            var userPassword = CryptoService.Decrypt(encryptedPassword, username);

            model = new LoginModel
            {
                UserName = username,
                Password = userPassword,
                RememberMe = false,
                ReturnUrl = returnUrl
            };

            var user = await UserManager.FindAsync(model.UserName, model.Password);
            if (user == null)
                return View(model);

            await SignInAsync(user, model.RememberMe);

            var employee = employeeService.GetByEmpId(Convert.ToInt64(user.EmployeeId));
            if (employee == null)
                return View(model);

            var entity = Mapper.Map<Employee, EmployeeViewModel>(employee);
            var offc = officeService.GetById(Convert.ToInt32(employee.OfficeId));
            var comp = companyService.GetById(Convert.ToInt32(offc.CompanyId));
            var companyWisePayrollConfig = companyWisePayrollConfigService.GetByCompanyCode(comp.CompanyCode);

            //let's create menu related session
            var parentModules = securityService.GetAllPrentModule().ToList();
            var allModules = commonDynamicDropDown.getRoleWiseChildMenu(0);
            var userModules = securityService.GeAllRoleModules(user.RoleId).ToList();
            SessionHelper.LogSessionInfo(entity, parentModules, userModules, allModules);
            //***********************************

            if (employee.EmployeeName.Trim() == EmployeeUserConstants.SuperAdmin)
                SessionHelper.UserNameType = UserTypeConstants.SuperAdmin;
            else
                SessionHelper.UserNameType = UserTypeConstants.Employee;

            SessionHelper.OrganizationName = comp.CompanyName;
            SessionHelper.CompanyID = comp.CompanyId;
            SessionHelper.CountryID = comp.CountryId;
            SessionHelper.LoginUserOfficeID = offc.OfficeId;
            SessionHelper.LoginUserOfficeType = offc.OfficeTypeId;
            SessionHelper.LoginUserOfficeLevel = offc.OfficeLevel;

            SessionHelper.LoggedInOfficeTypeId = Convert.ToInt32(offc.OfficeTypeId);
            SessionHelper.LoggedInRoleId = Convert.ToInt32(user.RoleId);
            SessionHelper.CompanyName = comp.CompanyName;
            SessionHelper.CompanyCode = comp.CompanyCode;
            SessionHelper.CompanyAddress = comp.CompanyAddress;
            SessionHelper.CompanyImage = comp.ImagePath;
            SessionHelper.PayrollConfigurationType = companyWisePayrollConfig.PayrollConfigurationType;
            SessionHelper.NoOfSalaryDays = companyWisePayrollConfig.NoOfSalaryDays.ToString();
            SessionHelper.PayrollType = companyWisePayrollConfig.PayrollType.ToString();

            //let's create employee session after login successful!
            SessionHelper.EmpSessionInfoAfterLogin(entity);

            //let's track company info session            
            SessionHelper.TrackCompanyInfoSession(comp);

            var redirectUrl = $"/home/index";
            return Redirect(redirectUrl);
        }

        /* Mahfuz close this method
         [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            //can show login panel

            var db = new gHRMDBContext();

            var company = db.Companies.Where(z => z.IsActive).Select(q => q.CompanyShortName).FirstOrDefault();

            string[] arr_id = { "0018", "1895", "5049", "0625", "0111", "0589", "superadmin", "2685", "2760", "0594", "2684", "2836", "0658","2647" };
            if (company == "GC")
            {

                if (!arr_id.Any(id => model.UserName.Contains(id)))
                {
                    ModelState.AddModelError("", "You are not an active employee of this organization");
                    return View(model);
                }
                else
                {
                    if (arr_id.Any(id => model.UserName.Contains("superadmin")))
                    {
                        if (!Request.Url.ToString().ToLower().Contains("localhost"))
                        {
                            ModelState.AddModelError("", "You are not an active employee of this organization");
                            return View(model);
                        }
                    }
                }
            }
            else
            {
                if (company == "GC")
                {
                    if (arr_id.Any(id => model.UserName.Contains("superadmin")))
                    {
                        if (!Request.Url.ToString().ToLower().Contains("localhost"))
                        {
                            ModelState.AddModelError("", "You are not an active employee of this organization");
                            return View(model);
                        }
                    }
                }
            }

            model.CanShowLoginPanel = CanShowLoginPanel();
            LogRequest();
            string SUPER_ADMIN_EMPLOYEEID = AppSetting.Get(AppSetting.SUPER_ADMIN_EMPLOYEEID, HttpContext);

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "You must all the required fields.");
                return View(model);
            }

            var user = await UserManager.FindAsync(model.UserName, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }
            if (SUPER_ADMIN_EMPLOYEEID != user.EmployeeId.ToString() && !employeeService.IsActive(user.EmployeeId))
            {
                ModelState.AddModelError("", "You are not an active employee of this organization");
                return View(model);
            }


            var aspNetRole = roleService.GetByRoleId(user.RoleId.ToString());
            if (aspNetRole == null)
                return Json(new { Result = "ERROR", Message = $"User role not found" }, JsonRequestBehavior.AllowGet);

            var employee = employeeService.GetByEmpId(Convert.ToInt64(user.EmployeeId));
            if (employee == null)
            {
                ModelState.AddModelError("", "Employee information not found for this user.");
                return View(model);
            }

            //try to sign in
            await SignInAsync(user, model.RememberMe);

            var entity = Mapper.Map<Employee, EmployeeViewModel>(employee);
            var offc = officeService.GetById(Convert.ToInt32(employee.OfficeId));
            var comp = companyService.GetById(Convert.ToInt32(offc.CompanyId));
            var companyWisePayrollConfig = companyWisePayrollConfigService.GetByCompanyCode(comp.CompanyCode);

            // ******** for menu create **************
            var parentModules = securityService.GetAllPrentModule().ToList();
            var allModules = commonDynamicDropDown.getRoleWiseChildMenu(0);
            var userModules = securityService.GeAllRoleModules(user.RoleId).ToList();
            SessionHelper.LogSessionInfo(entity, parentModules, userModules, allModules);
            //***********************************

            //if (employee.EmployeeName.Contains("Super Admin"))
            if (aspNetRole.Name.Contains("Super Admin"))
                SessionHelper.UserNameType = "SA";
            else
                SessionHelper.UserNameType = "E";

            SessionHelper.OrganizationName = comp.CompanyName;
            SessionHelper.CompanyID = comp.CompanyId;
            SessionHelper.CountryID = comp.CountryId;
            SessionHelper.CompanyCode = comp.CompanyCode;
            SessionHelper.CompanyImage = comp.ImagePath;
            SessionHelper.LoginUserOfficeID = offc.OfficeId;
            SessionHelper.LoginUserOfficeType = offc.OfficeTypeId;
            SessionHelper.LoginUserOfficeLevel = offc.OfficeLevel;


            SessionHelper.LoggedInOfficeTypeId = Convert.ToInt32(offc.OfficeTypeId);
            SessionHelper.LoggedInRoleId = Convert.ToInt32(user.RoleId);
            SessionHelper.CompanyName = comp.CompanyName;
            SessionHelper.CompanyAddress = comp.CompanyAddress;
            SessionHelper.EmpSessionInfoAfterLogin(entity);
            SessionHelper.PayrollConfigurationType = companyWisePayrollConfig.PayrollConfigurationType;
            SessionHelper.NoOfSalaryDays = companyWisePayrollConfig.NoOfSalaryDays.ToString();
            SessionHelper.PayrollType = companyWisePayrollConfig.PayrollType.ToString();
            SessionHelper.EnabledSSOLogin = false;

            //let's track company info session            
            SessionHelper.TrackCompanyInfoSession(comp);

            var redirectUrl = "";
            if (SessionHelper.OrganizationName == "Grameen Communications")
            {
                if (model.Password.Trim() == "123456" || model.Password.Trim() == "12345678")
                    redirectUrl = $"/account/managepassword";
                else
                    redirectUrl = $"/home/index";
            }
            else
                redirectUrl = $"/home/index";

            return Redirect(redirectUrl);
        }
         */

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            bool erp_structure = bool.Parse(AppSetting.Get(AppSetting.ERP_STRUCTURE, null));

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await UserManager.FindAsync(model.UserName, model.Password);
                    if (user == null)
                    {
                        if (erp_structure) ViewBag.ServiceMessages = "Invalid username or password.";
                        else ModelState.AddModelError("", "Invalid username or password.");
                        return View(model);
                    }
                    else if ((erp_structure && user.RoleId == 1) || !erp_structure)
                    {
                        #region W/O erp structure & Super Admin
                        var employee = employeeService.GetByEmpId(Convert.ToInt64(user.EmployeeId));
                        if (employee == null)
                        {
                            if (erp_structure) ViewBag.ServiceMessages = "Employee information not found for this user.";
                            else ModelState.AddModelError("", "Employee information not found for this user.");
                            return View(model);
                        }
                        bool emp_IsActive = (user.RoleId == 1 ? true : employeeService.IsActive(user.EmployeeId));
                        if (emp_IsActive)
                        {
                            var aspNetRole = roleService.GetByRoleId(user.RoleId.ToString());
                            if (aspNetRole == null)
                            {
                                if (erp_structure) ViewBag.ServiceMessages = "User role not found.";
                                else ModelState.AddModelError("", "User role not found.");
                                return View(model);
                            }
                            
                            await SignInAsync(user, model.RememberMe);

                            var entity = Mapper.Map<Employee, EmployeeViewModel>(employee);
                            var offc = officeService.GetById(Convert.ToInt32(employee.OfficeId));
                            var comp = companyService.GetById(Convert.ToInt32(offc.CompanyId));
                            var companyWisePayrollConfig = companyWisePayrollConfigService.GetByCompanyCode(comp.CompanyCode);

                            // ******** for menu create **************
                            var parentModules = securityService.GetAllPrentModule().ToList();
                            var allModules = commonDynamicDropDown.getRoleWiseChildMenu(0);
                            var userModules = securityService.GeAllRoleModules(user.RoleId).ToList();
                            SessionHelper.LogSessionInfo(entity, parentModules, userModules, allModules);

                            if (aspNetRole.Name.Contains("Super Admin"))
                                SessionHelper.UserNameType = "SA";
                            else
                                SessionHelper.UserNameType = "E";

                            SessionHelper.OrganizationName = comp.CompanyName;
                            SessionHelper.CompanyID = comp.CompanyId;
                            SessionHelper.CountryID = comp.CountryId;
                            SessionHelper.CompanyCode = comp.CompanyCode;
                            SessionHelper.CompanyImage = comp.ImagePath;
                            SessionHelper.LoginUserOfficeID = offc.OfficeId;
                            SessionHelper.LoginUserOfficeType = offc.OfficeTypeId;
                            SessionHelper.LoginUserOfficeLevel = offc.OfficeLevel;


                            SessionHelper.LoggedInOfficeTypeId = Convert.ToInt32(offc.OfficeTypeId);
                            SessionHelper.LoggedInRoleId = Convert.ToInt32(user.RoleId);
                            SessionHelper.CompanyName = comp.CompanyName;
                            SessionHelper.CompanyAddress = comp.CompanyAddress;
                            SessionHelper.EmpSessionInfoAfterLogin(entity);
                            SessionHelper.PayrollConfigurationType = companyWisePayrollConfig.PayrollConfigurationType;
                            SessionHelper.NoOfSalaryDays = companyWisePayrollConfig.NoOfSalaryDays.ToString();
                            SessionHelper.PayrollType = companyWisePayrollConfig.PayrollType.ToString();
                            SessionHelper.EnabledSSOLogin = false;

                            //let's track company info session            
                            SessionHelper.TrackCompanyInfoSession(comp);
                            return Redirect($"/home/index");
                        }
                        else
                        {
                            if (erp_structure) ViewBag.ServiceMessages = "You are not an active employee of this organization.";
                            else ModelState.AddModelError("", "You are not an active employee of this organization.");
                            return View(model);
                        }
                        #endregion
                    }
                    else if (erp_structure)
                    {
                        #region    erp structure
                        var employee = employeeService.GetByEmpId(Convert.ToInt64(user.EmployeeId));
                        if (employee == null)
                        {
                            ViewBag.ServiceMessages = "Employee information not found for this user.";
                            return View(model);
                        }
                        bool emp_IsActive =  employeeService.IsActive(user.EmployeeId);
                        if (emp_IsActive)
                        {
                            var entity = Mapper.Map<Employee, EmployeeViewModel>(employee);
                            SessionHelper.EmpSessionInfoAfterLogin(entity);
                            SessionHelper.UserName = model.UserName;
                            SessionHelper.UserPassword = model.Password;
                            await SignInAsync(user, model.RememberMe);
                            return Redirect($"/home/welcome");
                        }
                        else
                        {
                            ViewBag.ServiceMessages = "You are not an active employee of this organization.";
                            return View(model);
                        }
                      
                        #endregion erp structure
                    }
                }
                catch (Exception ex)
                {

                }

            }
            else
            {
                if (erp_structure) ViewBag.ServiceMessages = "You must all the required fields.";
                else ModelState.AddModelError("", "You must all the required fields.");
                return View(model);
            }
            return View();
        }

        #endregion

        #region Track Single Sign On

        [HttpGet]
        [AllowAnonymous]
        public ActionResult TrackSingleSignOn(string ecu)
        {
            if (string.IsNullOrWhiteSpace(ecu))
                return View();

            //let's track single sign on credential
            singleSignOnTrackingService.TrackSingleSignOn(ecu);

            return View();
        }

        #endregion

        #region Single Sign On Remove

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SingleSignOnRemove()
        {
            LogRequest();
            AuthenticationManager.SignOut();
            Session.Clear();
            Session.Abandon();

            //let's remove single sign on credential
            singleSignOnTrackingService.RemoveSingleSignOnIdentifier();

            return View();
        }

        #endregion

        #region Log Off

        [HttpGet]
        public ActionResult LogOff()
        {
            var enabledSSOLogin = SessionHelper.EnabledSSOLogin;
            LogRequest();
            AuthenticationManager.SignOut();
            Session.Clear();
            Session.Abandon();

            if (enabledSSOLogin)
            {
                var baseUrl = $@"{Request.Url.Scheme}://{Request.Url.Authority}";
                var redirectUrl = $@"{AuthServerConstants.AUTH_PATH.ToAppSettingValue()}/realms/GK_HEALTH/protocol/openid-connect/logout?redirect_uri={baseUrl}/Account/Login";
                return Redirect(redirectUrl);
            }
            return Redirect("/account/login?logout=true");
        }

        public ActionResult LogOff(bool? logOff)
        {
            LogRequest();
            AuthenticationManager.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Register        

        [SessionExpireFilter]
        [DisableCache]
        public ActionResult Register()
        {
            MapDropdownListValues();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        [DisableCache]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            bool isOperationSuccess = true;
            JsonResult responseResult = null;

            if (!ModelState.IsValid)
                return Json(new { Result = "ERROR", Message = "Please correct required fields." }, JsonRequestBehavior.AllowGet);

            try
            {
                var role = roleService.GetMany(x => x.IsActive == true && x.Id == model.RoleId.ToString()).FirstOrDefault();
                if (role == null)
                    return Json(new { Result = "ERROR", Message = "Role not found." }, JsonRequestBehavior.AllowGet);

                var employee = employeeService.GetByEmpId(model.EmployeeId);
                if (employee == null)
                    return Json(new { Result = "ERROR", Message = "Invalid Employee Code." }, JsonRequestBehavior.AllowGet);

                if (SessionHelper.EnabledSSOLogin)
                {
                    var responseAccessToken = await keyCloakService.GetAccessToken();

                    if (responseAccessToken.IsError)
                    {
                        isOperationSuccess = false;
                        responseResult = Json(new { Result = "ERROR", Message = responseAccessToken.Message }, JsonRequestBehavior.AllowGet);
                    }

                    if (isOperationSuccess)
                    {
                        var email = employee.Email ?? "";

                        //auth server user creation
                        var ssoRegister = new SSORegisterModel
                        {
                            firstName = employee.EmployeeName,
                            lastName = employee.EmployeeCode,
                            email = email,
                            username = model.UserName,
                            enabled = true,
                            credentials = new List<CredentialModel> { new CredentialModel { value = model.Password, temporary = false } }
                        };

                        //let create new auth user
                        var response = await keyCloakService.CreateNewUser(ssoRegister, responseAccessToken.access_token);
                        if (response.IsError)
                        {
                            isOperationSuccess = false;
                            responseResult = Json(new { Result = "ERROR", Message = response.Message }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    if (isOperationSuccess)
                    {
                        responseAccessToken = await keyCloakService.GetAccessToken();
                        if (responseAccessToken.IsError)
                        {
                            isOperationSuccess = false;
                            responseResult = Json(new { Result = "ERROR", Message = responseAccessToken.Message }, JsonRequestBehavior.AllowGet);
                        }

                        if (isOperationSuccess)
                        {
                            //get role
                            var authRole = await roleService.GetSSORoleMapping(model.RoleId);

                            //get user info
                            var authUserInfo = await keyCloakService.GetUserByUsername(model.UserName, responseAccessToken.access_token);
                            if (authUserInfo == null || authRole == null)
                            {
                                isOperationSuccess = false;
                                responseResult = Json(new { Result = "ERROR", Message = "Auth role not found" }, JsonRequestBehavior.AllowGet);
                            }

                            if (isOperationSuccess)
                            {
                                responseAccessToken = await keyCloakService.GetAccessToken();
                                if (responseAccessToken.IsError)
                                {
                                    isOperationSuccess = false;
                                    responseResult = Json(new { Result = "ERROR", Message = responseAccessToken.Message }, JsonRequestBehavior.AllowGet);
                                }

                                if (isOperationSuccess)
                                {
                                    var AuthRoleMappingRequest = new AuthRoleMappingRequestModel
                                    {
                                        AuthRoles = new List<AuthRoleMappingModel> { new AuthRoleMappingModel { id = authRole.SSORoleId, name = authRole.SSORoleName } },
                                        IdOfUser = authUserInfo.id,
                                        IdOfClient = authRole.SSOIdofClient,
                                        ClientRole = authRole.ClientRole,
                                        AccessToken = responseAccessToken.access_token
                                    };

                                    //let's map the role with auth user
                                    var responseMapRoleWithAuthUser = await keyCloakService.MapRoleWithAuthUser(AuthRoleMappingRequest);

                                    if (responseMapRoleWithAuthUser.IsError)
                                        isOperationSuccess = false;
                                }
                            }
                        }
                    }
                }

                if (isOperationSuccess)
                {
                    var user = new ApplicationUser() { UserName = model.UserName, EmployeeId = model.EmployeeId, FirstName = employee.EmployeeName, RoleId = model.RoleId };

                    //let's create a new user
                    var result = await UserManager.CreateAsync(user, model.Password);

                    if (!result.Succeeded)
                    {
                        var msg = "";
                        foreach (var r in result.Errors) msg = string.Format("{0} {1}", msg, r);

                        return Json(new { Result = "ERROR", Message = msg }, JsonRequestBehavior.AllowGet);
                    }

                    var aspAdminPasswordTable = new AspAdminPasswordTable();
                    aspAdminPasswordTable.UserType = UserTypeConstants.Employee;
                    aspAdminPasswordTable.EmployeeID = employee.EmployeeId;
                    aspAdminPasswordTable.UserName = model.UserName;
                    aspAdminPasswordTable.UserPwd = model.Password;

                    //let's create admin password
                    aspAdminPasswordTableService.Create(aspAdminPasswordTable);

                    responseResult = Json(new { Result = "OK", Message = "Login Created successfully." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                responseResult = Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return responseResult;
        }

        #endregion

        #region Register Edit

        public ActionResult RegisterEdit(string Id)
        {
            var entity = aspNetUserService.Get(x => x.Id == Id);
            var id = Convert.ToInt64(entity.EmployeeId);
            var employee = employeeService.GetByEmpId(id);
            var codeName = employee.EmployeeCode + " - " + employee.EmployeeName;

            var model = new RegisterModel();
            model.EmployeeId = (long)entity.EmployeeId;
            model.FirstName = codeName;
            model.UserName = entity.UserName;
            model.RoleId = entity.RoleId;
            MapDropdownListValues();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UpdateUserRole(RegisterModel model)
        {
            ModelState.Remove("ConfirmPassword");

            if (!ModelState.IsValid)
                return Json(new { Result = "ERROR", Message = "Please correct required fields." }, JsonRequestBehavior.AllowGet);

            try
            {
                var validateUser = await UserManager.FindAsync(model.UserName, model.Password);
                if (validateUser == null || string.IsNullOrWhiteSpace(validateUser.Id))
                    return Json(new { Result = "ERROR", Message = "Invalid Password." }, JsonRequestBehavior.AllowGet);

                var updateAspNetUser = aspNetUserService.Get(x => x.Id == model.Id);
                if (updateAspNetUser == null || string.IsNullOrWhiteSpace(updateAspNetUser.Id))
                    return Json(new { Result = "ERROR", Message = "User not found." }, JsonRequestBehavior.AllowGet);

                updateAspNetUser.RoleId = model.RoleId;
                aspNetUserService.Update(updateAspNetUser);

                if (SessionHelper.EnabledSSOLogin)
                {
                    //let's update auth user
                    var response = await UpdateAuthUser(model, updateAspNetUser);
                    return response;
                }
            }
            catch (Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = "OK", Message = "Success! User Updated." }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Common Actions        
        public async Task<JsonResult> RegisterInfoResetPassword(string Id)
        {
            var entity = aspNetUserService.GetByUserId(Id);
            string Result = "";
            if (ModelState.IsValid)
            {
                try
                {
                    var myRandomNo = "12345678";
                    UserManager.RemovePassword(entity.Id);
                    UserManager.AddPassword(entity.Id, myRandomNo.ToString());

                    if (SessionHelper.EnabledSSOLogin)
                    {
                        //let change password to auth user
                        await ChangePasswordToAuthUser(entity.UserName, myRandomNo);
                    }

                    Result = "OK";
                }
                catch (Exception ex)
                {
                    Result = "ERROR";
                }
            }
            else
            {
                Result = "ERROR";
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        // Modified this from private to public and add the setter
        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                if (_authnManager == null)
                    _authnManager = HttpContext.GetOwinContext().Authentication;
                return _authnManager;
            }
            set { _authnManager = value; }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }


        public JsonResult GetEmpInfo(string employee_Code)
        {
            var result = 0;
            var data = "";
            long empId = 0;
            try
            {
                var employee = employeeService.GetByCode(employee_Code);
                empId = employee.EmployeeId;
                data = employee.EmployeeCode + " - " + employee.EmployeeName;
                result = 1;
            }
            catch (Exception e)
            {
                result = 0;
                data = "";
            }
            return Json(new { result = result, empId = empId, data = data }, JsonRequestBehavior.AllowGet);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        [SessionExpireFilter]
        [DisableCache]
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            // ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");

            var model = new LocalPasswordModel();
            return View(model);
        }

        [SessionExpireFilter]
        [DisableCache]
        public ActionResult ManagePassword(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            // ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("ManagePassword");

            var model = new LocalPasswordModel();
            return View(model);
        }



        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        [DisableCache]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                        var asptbl = aspAdminPasswordTableService.GetByUserName(User.Identity.Name);
                        asptbl.UserPwd = model.NewPassword;
                        aspAdminPasswordTableService.Update(asptbl);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        [DisableCache]
        public ActionResult ManagePassword(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("ManagePassword");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                        var asptbl = aspAdminPasswordTableService.GetByUserName(User.Identity.Name);
                        asptbl.UserPwd = model.NewPassword;
                        aspAdminPasswordTableService.Update(asptbl);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("ManagePassword", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("ManagePassword", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        [DisableCache]
        public async Task<ActionResult> ChangePassword(LocalPasswordModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { type = "error", message = "Warning, You must fill all the required fields." }, JsonRequestBehavior.AllowGet);

            try
            {
                //company with old and new password
                if (model.OldPassword == model.NewPassword)
                    return Json(new { type = "error", message = "Old password and new password cannot be same" }, JsonRequestBehavior.AllowGet);

                var userId = User.Identity.GetUserId();
                var username = User.Identity.GetUserName();

                //let's try to change password
                var result = UserManager.ChangePassword(userId, model.OldPassword, model.NewPassword);

                if (!result.Succeeded)
                    return Json(new { type = "error", message = "Failed. " + string.Join(",", result.Errors) }, JsonRequestBehavior.AllowGet);

                if (SessionHelper.EnabledSSOLogin)
                {
                    //let change password to auth user
                    await ChangePasswordToAuthUser(username, model.NewPassword);
                }

                //reflect password changes on web security
                //TODO: need to fix
                //WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                return Json(new { type = "success", message = "Password changed successfully." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { type = "error", message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        [DisableCache]
        public async Task<ActionResult> ChangePasswordNew(LocalPasswordModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { type = "error", message = "Warning, You must fill all the required fields." }, JsonRequestBehavior.AllowGet);

            try
            {
                //company with old and new password
                if (model.OldPassword == model.NewPassword)
                    return Json(new { type = "error", message = "Old password and new password cannot be same" }, JsonRequestBehavior.AllowGet);

                var userId = User.Identity.GetUserId();
                var username = User.Identity.GetUserName();

                //let's try to change password
                var result = UserManager.ChangePassword(userId, model.OldPassword, model.NewPassword);

                if (!result.Succeeded)
                    return Json(new { type = "error", message = "Failed. " + string.Join(",", result.Errors) }, JsonRequestBehavior.AllowGet);

                if (SessionHelper.EnabledSSOLogin)
                {
                    //let change password to auth user
                    await ChangePasswordToAuthUserNew(username, model.NewPassword);
                }

                //reflect password changes on web security
                //TODO: need to fix
                //WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                //return Json(new { type = "success", message = "Password changed successfully." }, JsonRequestBehavior.AllowGet);

                var redirectUrl = "";
                //if (SessionHelper.OrganizationName == "Grameen Communications")
                //{
                //    if (model.Password.Trim() == "123456" || model.Password.Trim() == "12345678")
                //        redirectUrl = $"/account/managepassword";
                //    else
                //        redirectUrl = $"/home/index";
                //}
                //else
                redirectUrl = $"/Account/logoff";

                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                return Json(new { type = "error", message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #endregion

        #region Ajax Calls

        [HttpPost]
        public async Task<JsonResult> SSOSignIn(ApplicationUser applicationUser)
        {
            var response = new BaseResponse();
            try
            {
                SessionHelper.EnabledSSOLogin = true;

                var user = await UserManager.FindByNameAsync(applicationUser.UserName);
                if (user == null)
                {
                    response = new BaseResponse { IsSuccess = false, Message = "Warning! User not found!" };
                    return Json(response, JsonRequestBehavior.AllowGet);
                }

                var aspNetRole = roleService.GetByRoleId(user.RoleId.ToString());
                if (aspNetRole == null)
                {
                    response = new BaseResponse { IsSuccess = false, Message = "Warning! Error on loggin user role not found!" };
                    return Json(response, JsonRequestBehavior.AllowGet);
                }

                var employee = employeeService.GetByEmpId(Convert.ToInt64(user.EmployeeId));
                if (employee == null)
                {
                    response = new BaseResponse { IsSuccess = false, Message = "Warning! Employee information not found for this user." };
                    return Json(response, JsonRequestBehavior.AllowGet);
                }

                bool rememberMe = true;
                //try to sign in
                await SignInAsync(user, rememberMe);

                var entity = Mapper.Map<Employee, EmployeeViewModel>(employee);
                var offc = officeService.GetById(Convert.ToInt32(employee.OfficeId));
                var comp = companyService.GetById(Convert.ToInt32(offc.CompanyId));
                var companyWisePayrollConfig = companyWisePayrollConfigService.GetByCompanyCode(comp.CompanyCode);

                var parentModules = securityService.GetAllPrentModule().ToList();
                var allModules = commonDynamicDropDown.getRoleWiseChildMenu(0);
                var userModules = securityService.GeAllRoleModules(user.RoleId).ToList();
                SessionHelper.LogSessionInfo(entity, parentModules, userModules, allModules);

                if (aspNetRole.Name.Contains("Super Admin"))
                    SessionHelper.UserNameType = "SA";
                else
                    SessionHelper.UserNameType = "E";

                SessionHelper.OrganizationName = comp.CompanyName;
                SessionHelper.CompanyID = comp.CompanyId;
                SessionHelper.CountryID = comp.CountryId;
                SessionHelper.CompanyCode = comp.CompanyCode;
                SessionHelper.CompanyImage = comp.ImagePath;
                SessionHelper.LoginUserOfficeID = offc.OfficeId;
                SessionHelper.LoginUserOfficeType = offc.OfficeTypeId;

                SessionHelper.LoggedInOfficeTypeId = Convert.ToInt32(offc.OfficeTypeId);
                SessionHelper.LoggedInRoleId = Convert.ToInt32(user.RoleId);
                SessionHelper.CompanyName = comp.CompanyName;
                SessionHelper.CompanyAddress = comp.CompanyAddress;
                SessionHelper.EmpSessionInfoAfterLogin(entity);
                SessionHelper.PayrollConfigurationType = companyWisePayrollConfig.PayrollConfigurationType;
                SessionHelper.NoOfSalaryDays = companyWisePayrollConfig.NoOfSalaryDays.ToString();
                SessionHelper.PayrollType = companyWisePayrollConfig.PayrollType.ToString();

                //let's track company info session            
                SessionHelper.TrackCompanyInfoSession(comp);

                response = new BaseResponse { IsSuccess = true, Message = "Success! User Logged In" };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                response = new BaseResponse { IsSuccess = false, Message = "Warning! Error on loggin user" };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Private Methods  

        private async Task<JsonResult> UpdateAuthUser(RegisterModel model, AspNetUser updateAspNetUser)
        {
            var responseAccessToken = await keyCloakService.GetAccessToken();

            if (responseAccessToken.IsError)
                return Json(new { Result = "ERROR", Message = responseAccessToken.Message }, JsonRequestBehavior.AllowGet);

            //get role
            var authRole = await roleService.GetSSORoleMapping(model.RoleId);
            if (authRole == null)
                return Json(new { Result = "ERROR", Message = "Warning, Auth role not found" }, JsonRequestBehavior.AllowGet);

            //get user info
            var authUserInfo = await keyCloakService.GetUserByUsername(updateAspNetUser.UserName, responseAccessToken.access_token);

            //if auth user not found then try to enroll this user to auth server
            if (authUserInfo == null || string.IsNullOrWhiteSpace(authUserInfo.username))
            {
                var employee = employeeService.GetByEmpId(model.EmployeeId);
                if (employee == null)
                    return Json(new { Result = "ERROR", Message = "Invalid Employee Code." }, JsonRequestBehavior.AllowGet);

                //auth server user creation
                var ssoRegister = new SSORegisterModel
                {
                    firstName = employee.EmployeeName,
                    lastName = employee.EmployeeCode,
                    email = employee.Email ?? "",
                    username = model.UserName,
                    enabled = true,
                    credentials = new List<CredentialModel> { new CredentialModel { value = model.Password, temporary = false } }
                };

                //let create new auth user
                var response = await keyCloakService.CreateNewUser(ssoRegister, responseAccessToken.access_token);
                if (response.IsError)
                    return Json(new { Result = "ERROR", Message = response.Message }, JsonRequestBehavior.AllowGet);

                //get user info
                authUserInfo = await keyCloakService.GetUserByUsername(updateAspNetUser.UserName, responseAccessToken.access_token);

                //if auth user not found then try to enroll this user to auth server
                if (authUserInfo == null || string.IsNullOrWhiteSpace(authUserInfo.username))
                    return Json(new { Result = "ERROR", Message = "Auth user not found" }, JsonRequestBehavior.AllowGet);
            }

            var authRoleMappingRequest = new AuthRoleMappingRequestModel
            {
                AuthRoles = new List<AuthRoleMappingModel> { new AuthRoleMappingModel { id = authRole.SSORoleId, name = authRole.SSORoleName } },
                IdOfUser = authUserInfo.id,
                IdOfClient = authRole.SSOIdofClient,
                ClientRole = authRole.ClientRole,
                AccessToken = responseAccessToken.access_token
            };

            //let's map the role with auth user
            var responseMapRoleWithAuthUser = await keyCloakService.MapRoleWithAuthUser(authRoleMappingRequest);

            if (responseMapRoleWithAuthUser.IsError)
                return Json(new { Result = "ERROR", Message = "Warning! Error on role mapping" }, JsonRequestBehavior.AllowGet);

            return Json(new { Result = "OK", Message = "Success! User Updated." }, JsonRequestBehavior.AllowGet);
        }

        private async Task<JsonResult> InactiveAuthUser(string username)
        {
            var responseAccessToken = await keyCloakService.GetAccessToken();

            if (responseAccessToken.IsError)
                return Json(new { Result = "ERROR", Message = responseAccessToken.Message }, JsonRequestBehavior.AllowGet);

            //get user info
            var authUserInfo = await keyCloakService.GetUserByUsername(username, responseAccessToken.access_token);

            //if auth user not found return success and do not need to perform operation in auth server
            if (authUserInfo == null || string.IsNullOrWhiteSpace(authUserInfo.username))
                return Json(new { Result = "OK", Message = "Success! User Deleted." }, JsonRequestBehavior.AllowGet);

            var model = new InactiveAuthUserModel
            {
                id = authUserInfo.id,
                username = username,
                enabled = false
            };

            //let inactive auth user
            var response = await keyCloakService.InactiveAuthUser(model, responseAccessToken.access_token);
            if (response.IsError)
                return Json(new { Result = "ERROR", Message = response.Message }, JsonRequestBehavior.AllowGet);

            return Json(new { Result = "OK", Message = "Success! User Deleted." }, JsonRequestBehavior.AllowGet);
        }

        private async Task<JsonResult> ChangePasswordToAuthUser(string username, string password)
        {
            var responseAccessToken = await keyCloakService.GetAccessToken();

            if (responseAccessToken.IsError)
                return Json(new { Result = "ERROR", Message = responseAccessToken.Message }, JsonRequestBehavior.AllowGet);

            //get user info
            var authUserInfo = await keyCloakService.GetUserByUsername(username, responseAccessToken.access_token);

            //if auth user not found return success and do not need to perform operation in auth server
            if (authUserInfo == null || string.IsNullOrWhiteSpace(authUserInfo.username))
                return Json(new { Result = "OK", Message = "Success! Password Changed" }, JsonRequestBehavior.AllowGet);

            var model = new AuthUserChangePasswordModel
            {
                id = authUserInfo.id,
                username = username,
                credentials = new List<CredentialModel> { new CredentialModel { value = password, temporary = false } }
            };

            //let change password to auth user
            var response = await keyCloakService.ChangePasswordToAuthUser(model, responseAccessToken.access_token);
            if (response.IsError)
                return Json(new { Result = "ERROR", Message = response.Message }, JsonRequestBehavior.AllowGet);

            return Json(new { Result = "OK", Message = "Success! Password Changed" }, JsonRequestBehavior.AllowGet);
        }

        private async Task<ActionResult> ChangePasswordToAuthUserNew(string username, string password)
        {
            var responseAccessToken = await keyCloakService.GetAccessToken();

            if (responseAccessToken.IsError)
                return Json(new { Result = "ERROR", Message = responseAccessToken.Message }, JsonRequestBehavior.AllowGet);

            //get user info
            var authUserInfo = await keyCloakService.GetUserByUsername(username, responseAccessToken.access_token);

            //if auth user not found return success and do not need to perform operation in auth server
            if (authUserInfo == null || string.IsNullOrWhiteSpace(authUserInfo.username))
                return Json(new { Result = "OK", Message = "Success! Password Changed" }, JsonRequestBehavior.AllowGet);

            var model = new AuthUserChangePasswordModel
            {
                id = authUserInfo.id,
                username = username,
                credentials = new List<CredentialModel> { new CredentialModel { value = password, temporary = false } }
            };

            //let change password to auth user
            var response = await keyCloakService.ChangePasswordToAuthUser(model, responseAccessToken.access_token);
            if (response.IsError)
                return Json(new { Result = "ERROR", Message = response.Message }, JsonRequestBehavior.AllowGet);

            // return Json(new { Result = "OK", Message = "Success! Password Changed" }, JsonRequestBehavior.AllowGet);

            var redirectUrl = "";
            //if (SessionHelper.OrganizationName == "Grameen Communications")
            //{
            //    if (model.Password.Trim() == "123456" || model.Password.Trim() == "12345678")
            //        redirectUrl = $"/account/managepassword";
            //    else
            //        redirectUrl = $"/home/index";
            //}
            //else
            redirectUrl = $"/Account/logoff";

            return Redirect(redirectUrl);
        }

        private async Task<JsonResult> GetAccessToken()
        {
            JsonResult responseResult = null;

            //get access token
            var responseAccessToken = await keyCloakService.GetAccessToken();

            if (responseAccessToken.IsError)
                responseResult = Json(new { Result = "ERROR", Message = responseAccessToken.Message }, JsonRequestBehavior.AllowGet);

            return responseResult;
        }

        private bool CanShowLoginPanel(string companyCode = "")
        {
            bool canShowLoginPanel = true;
            if (string.IsNullOrWhiteSpace(companyCode))
            {
                var companyInfo = companyService.GetCompanyInfo();
                if (companyInfo == null)
                    canShowLoginPanel = true;

                companyCode = companyInfo.CompanyCode;
            }

            if (companyCode == GHRMPlusCompanyConstants.GrameenKalyan)
                canShowLoginPanel = false;

            return canShowLoginPanel;
        }
        private void ClearLoginSession()
        {
            AuthenticationManager.SignOut();
            Session.Clear();
            Session.Abandon();
        }

        private int GetOrganization()
        {
            int organizationId = 0;

            switch (SessionHelper.CompanyCode)
            {
                case GHRMPlusCompanyConstants.GUK:
                    organizationId = Convert.ToInt32(GBankerCompanyConstants.GUK);
                    break;

                default:
                    organizationId = Convert.ToInt32(GBankerCompanyConstants.GUK);
                    break;
            }

            return organizationId;
        }

        private List<SSOInstanceViewModel> SingleSignOnReset()
        {
            //remove sso identifier
            singleSignOnTrackingService.RemoveSingleSignOnIdentifier();

            var ssoInstances = new List<SSOInstanceViewModel>();

            //let's create trigger to track this credentials for possible sso instances
            var instanceListing = ConfigurationManager.AppSettings["gHRM.Cookie.SingleSignOn.Instances"];
            if (instanceListing == null || string.IsNullOrWhiteSpace(instanceListing.ToString()))
                return ssoInstances;

            string instances = instanceListing.ToString();
            var fragmentedInstances = instances.Split('@');

            //get current instance base url 
            var currentInstanceUrl = $"{HttpContext.Request.Url.Scheme}://{HttpContext.Request.Url.Authority}";

            foreach (var instanceUrl in fragmentedInstances)
            {
                if (currentInstanceUrl.ToLower() == instanceUrl.ToLower())
                    continue;

                var ssoInstance = new SSOInstanceViewModel
                {
                    BaseUrl = instanceUrl
                };
                ssoInstances.Add(ssoInstance);
            }

            return ssoInstances;
        }

        private void MapDropdownListValues()
        {
            var roleList = roleService.GetMany(x => x.IsActive == true).ToList();

            if (roleList.Any())
                roleList = roleList.Where(f => f.Name != UserRoleConstants.Super_Admin).OrderBy(f => f.Name).AsParallel().ToList();

            var viewList = new List<SelectListItem>();
            viewList.Add(new SelectListItem() { Text = "Select Role", Value = "0" });
            var roleListView = roleList.Select(m => new SelectListItem() { Text = m.Name, Value = m.Id.ToString() }).ToList();
            viewList.AddRange(roleListView);
            ViewBag.RoleList = viewList;

        }
        private void LogRequest()
        {
            try
            {
                var logObject = Logger.GetLogObject();
                loggger.LogRequest(logObject);
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
