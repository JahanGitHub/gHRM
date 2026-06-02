
#region Usings

using AutoMapper;
using gHRM.Core.Utilities;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.DBDetailModels.Security;
using gHRM.Service;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;

#endregion

namespace gHRM.Web.Controllers
{
    public class SecurityController : BaseController
    {
        #region Private Methods

        private readonly IAspNetRoleService roleService;
        private readonly ISecurityService securityService;
        private readonly IAspNetUserService aspNetUserService;
        private readonly IKeyCloakService keyCloakService;

        #endregion

        #region Ctor
        public SecurityController(IAspNetRoleService roleService,
           ISecurityService securityService,
           IKeyCloakService keyCloakService,
           IAspNetUserService aspNetUserService)
        {
            this.roleService = roleService;
            this.securityService = securityService;
            this.aspNetUserService = aspNetUserService;
            this.keyCloakService = keyCloakService;
        }
        #endregion

        #region User Roles

        public async Task<ActionResult> UserRole()
        {
            //get access token 
            /*
            var responseAccessToken = await keyCloakService.GetAccessToken();

            var roleDetails = await keyCloakService.GetSSORole(responseAccessToken.access_token, "SandBox Client Role For Health APp");

            */


            var model = new AspNetRoleViewModel();
            return View(model);
        }

        #endregion

        #region Ad New Role

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UserRoleCreate(AspNetRoleViewModel model)
        {
            bool isOperationSuccess = true;
            JsonResult responseResult = null;

            if (string.IsNullOrWhiteSpace(model.Name))
                return Json(new { result = 0, message = "Provide Role Name" }, JsonRequestBehavior.AllowGet);

            var roleList = roleService.GetAll();
            if (roleList.Any(x => x.IsActive == true && x.Name.Trim() == model.Name.Trim()))
                return Json(new { result = 0, message = "This Role Already Exists" }, JsonRequestBehavior.AllowGet);

            using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    //Populate new asp net role
                    AspNetRole aspNetRole = PopulateNewAspNetRole(model.Name, roleList);

                    //create role
                    var newAspNetRole = await roleService.AddNewRole(aspNetRole);
                    if (newAspNetRole == null)
                    {
                        responseResult = Json(new { result = 0, message = "Error on role creation" }, JsonRequestBehavior.AllowGet);
                        isOperationSuccess = false;
                    }

                    if (isOperationSuccess && SessionHelper.EnabledSSOLogin)
                    {
                        //get access token
                        var responseAccessToken = await keyCloakService.GetAccessToken();
                        if (responseAccessToken.IsError)
                        {
                            responseResult = Json(new { Result = 0, Message = responseAccessToken.Message }, JsonRequestBehavior.AllowGet);
                            isOperationSuccess = false;
                        }

                        if (isOperationSuccess)
                        {
                            var appClientAppCount = AuthServerClientConstants.Items.Count();

                            //is client role or realm/super admin role
                            var isClientRole = appClientAppCount != model.AppClientIDs.Count;

                            //auth server user creation
                            var ssoRoleModel = new SSORoleModel
                            {
                                clientRole = isClientRole,
                                name = model.Name,
                                description = $"Role {model.Name}, Created From HR on {DateTime.Now.ToString("dd-MMM-yyyy")}"
                            };

                            var request = new SSORoleRequestModel
                            {
                                SSORole = ssoRoleModel,
                                AppClientIDs = model.AppClientIDs,
                                AccessToken= responseAccessToken.access_token
                            };

                            //let create new auth role
                            var response = await keyCloakService.CreateNewRole(request);
                            if (response.IsError)
                            {
                                responseResult = Json(new { result = 0, message = response.Message }, JsonRequestBehavior.AllowGet);
                                isOperationSuccess = false;
                            }

                            //get sso roles
                            var ssoNewRoles = await GetRoleList(model, isClientRole, responseAccessToken.access_token);
                            if (ssoNewRoles.Any())
                            {
                                foreach (var item in ssoNewRoles)
                                {
                                    var newSSORoleMapping = new SSORoleMapping
                                    {
                                        RoleId = Convert.ToInt32(newAspNetRole.Id),
                                        SSORoleId = item.id,
                                        SSOIdofClient = item.AuthIdOfClient,
                                        SSOClientName = item.AuthClientId,
                                        SSORoleName = model.Name,
                                        ClientRole = isClientRole,
                                        CreatedBy = (int)LoggedInEmployeeId,
                                        CreatedDate = DateTime.Now
                                    };

                                    //let's add New sso role
                                    await roleService.AddNewSSORole(newSSORoleMapping);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    responseResult = Json(new { result = 0, message = e.Message }, JsonRequestBehavior.AllowGet);
                    isOperationSuccess = false;
                }

                if (isOperationSuccess)
                {
                    responseResult = Json(new { result = 1, message = "Success! User Role Created" }, JsonRequestBehavior.AllowGet);
                    ts.Complete();
                }

                ts.Dispose();
            }

            return responseResult;
        }

        #endregion

        #region Role Security

        public ActionResult RoleSecurity()
        {
            MapDropdownListValues();
            return View();
        }

        #endregion

        #region Ajax Calls

        public JsonResult RoleSecurityGrid(int? parentMenuId, int? roleId)
        {
            try
            {
                IEnumerable<AspNetSecurityModule> modules;
                if (parentMenuId.HasValue && roleId.HasValue)
                    modules = securityService.GetAllModulesForParent(parentMenuId.Value, roleId.Value);
                else
                    modules = securityService.GetAllPrentModule().Where(m => m.IsActive == true).OrderBy(p => p.DisplayOrder);
                var entites = Mapper.Map<IEnumerable<AspNetSecurityModule>, IEnumerable<AspNetSecurityModuleViewModel>>(modules);
                return Json(new { Result = "OK", Records = entites });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult RoleSecurityCreate(Dictionary<string, bool> SelectedList, Dictionary<string, string> SecurityList, int roleId)
        {
            try
            {
                var roleModules = new List<AspNetRoleModule>();
                foreach (var module in SelectedList)
                {
                    var id = module.Key.Split("_".ToCharArray());
                    var selected = module.Value;
                    if (id.Length == 2 && id[0] == "chk")
                    {
                        var securityLevel = SecurityList.Where(w => w.Key.Split("_".ToCharArray()).Length == 2 && w.Key.Split("_".ToCharArray())[1] == id[1]).FirstOrDefault();
                        var lvlValue = int.Parse(securityLevel.Value);
                        var roleModule = new AspNetRoleModule() { RoleId = roleId.ToString(), ModuleId = int.Parse(id[1]), SecurityLevelId = lvlValue, IsActive = true, IsSelectedForRole = selected, CreatedBy = User.Identity.Name };
                        roleModules.Add(roleModule);
                    }

                }
                securityService.CreateSecurityRole(roleModules);

                return Json(new { Result = "OK", SelectedList= SelectedList.Count() });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public JsonResult UserRoleDelete(string Id)
        {
            var result = 0;
            var message = "";
            if (Id != "")
            {
                var roleId = Convert.ToInt32(Id);
                var roleEntity = roleService.GetAll().Where(x => x.IsActive == true && x.Id.Trim() == Id.Trim()).FirstOrDefault();
                if (roleEntity != null)
                {
                    var isUserExists = aspNetUserService.GetAll().Where(u => u.RoleId == Convert.ToInt32(Id.Trim())).ToList();
                    if (isUserExists.Any())
                    {
                        result = 0;
                        message = "Users Exist With This Role. Delete Denied";
                    }
                    else
                    {
                        roleEntity.IsActive = false;
                        roleEntity.UpdateBy = LoggedInEmployeeId;
                        roleEntity.UpdateDate = DateTime.UtcNow;
                        roleService.Update(roleEntity);
                        result = 1;
                        message = "Role Deleted Successfully";
                    }

                }
                else
                {
                    result = 0;
                    message = "Role Not Found. Delete Denied";

                }
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUserRoles([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<AspNetRoleViewModel> List_ViewModel = new List<AspNetRoleViewModel>();
                var sl = 1;
                var roleList = roleService.GetAll().Where(p => p.IsActive == true);
                List_ViewModel = roleList.AsEnumerable().Select(row => new AspNetRoleViewModel
                {
                    Id = row.Id,
                    Name = row.Name,
                    rowSl = sl++
                }).ToList();

                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region Private Methods

        private async Task<IEnumerable<SSORoleDetailModel>> GetRoleList(AspNetRoleViewModel model,bool clientRole, string accessToken) 
        {
            //get inserted role from keycloak
            var fetchRoleRequest = new SSOFetchRoleRequestModel
            {
                IsClientRole = clientRole,
                RoleName = model.Name,
                AccessToken = accessToken
            };

            var newRoleList = new List<SSORoleDetailModel>();

            if (!clientRole) //realm role
            {
                //get sso role
                var roleDetail = await keyCloakService.GetSSORole(fetchRoleRequest);
                if (roleDetail != null)
                {
                    roleDetail.AuthClientId = "Realm";
                    roleDetail.AuthIdOfClient = "Realm";
                    newRoleList.Add(roleDetail);
                }

                return newRoleList;
            }


            foreach (var clientId in model.AppClientIDs)
            {
                fetchRoleRequest.AppClientId = clientId;

                var roleDetail = await keyCloakService.GetSSORole(fetchRoleRequest);
                if (roleDetail != null)
                {
                    roleDetail.AuthClientId = clientId;
                    roleDetail.AuthIdOfClient = clientId.ToAppSettingValue();
                    newRoleList.Add(roleDetail);
                }
            }

            return newRoleList;
        }
        private void MapDropdownListValues()
        {
            var roleList = roleService.GetAll().Where(x => x.IsActive == true).ToList();
            var currentRole = roleList.FirstOrDefault(f => f.Id == SessionHelper.LoggedInRoleId.ToString());

            if (roleList.Any() && currentRole.Name != UserRoleConstants.Super_Admin)
                roleList = roleList.Where(f => f.Name != UserRoleConstants.Super_Admin).OrderBy(f => f.Name).AsParallel().ToList();

            var viewList = new List<SelectListItem>();
            viewList.Add(new SelectListItem() { Text = "Select Role", Value = "" });

            var roleListView = roleList.Select(m => new SelectListItem() { Text = m.Name, Value = m.Id.ToString() }).ToList();
            viewList.AddRange(roleListView);
            ViewBag.RoleList = viewList;
        }

        private AspNetRole PopulateNewAspNetRole(string Name, IEnumerable<AspNetRole> roleList)
        {
            List<int> roleListInt = new List<int>();
            foreach (var item in roleList)
                roleListInt.Add(Convert.ToInt32(item.Id));

            var maxRoleId = roleListInt.Max();

            var roleId = maxRoleId > 0 ? (maxRoleId + 1) : 1;
            var aspNetRole = new AspNetRole();
            aspNetRole.Id = roleId.ToString();
            aspNetRole.Name = Name.Trim();
            aspNetRole.IsActive = true;
            aspNetRole.DefaultLinkURL = "~/Home";
            aspNetRole.CreateDate = DateTime.UtcNow;
            aspNetRole.CreatedBy = LoggedInEmployeeId;
            return aspNetRole;
        }

        #endregion
    }
}
