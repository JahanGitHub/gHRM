using gHRM.Web.Helpers;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web.Security;
using gHRM.Data.CodeFirstMigration;
using System.Collections.Generic;
using System;
using System.Linq;
using gHRM.Core.Utilities;

namespace gHRM.Web.Filters
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {

        private IAuthenticationManager _authnManager;
        private ILogger _logObject;
        // Modified this from private to public and add the setter
        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                if (_authnManager == null)
                    _authnManager = HttpContext.Current.GetOwinContext().Authentication;
                return _authnManager;
            }
            set { _authnManager = value; }
        }
        public ILogger LogObject
        {
            get
            {
                if (_logObject == null)
                    _logObject = DependencyResolver.Current.GetService<ILogger>();
                return _logObject;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            #region Logger
            //try
            //{
            //    var loggingObject = Logger.GetLogObject();
            //    LogObject.LogRequest(loggingObject);
            //}
            //catch (Exception ex)
            //{
            //    //Send email that logger is not working....
            //    throw ex;
            //}

            #endregion

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("/Account/Login");
                return;
            }

            HttpContext ctx = HttpContext.Current;

            // check if session is supported
            if (ctx.Session == null || ctx.Session.Count<=0)
            {
                filterContext.Result = new RedirectResult("/Account/Login");
                return;
            }

            //TODO: need to change 
            //var response = IsRequestAuthorized();
            //if (!response.IsSuccess)
            //{
            //    filterContext.Result = new RedirectResult("/Account/Login");
            //    return;
            //}

            EnsureRequestIsAuthorized();
            
            #region TODO: Why we do need to check IsNewSession
            // check if a new session id was generated
            //if (ctx.Session.IsNewSession)
            //{
            //    // If it says it is a new session, but an existing cookie exists, then it must
            //    // have timed out
            //    //  string sessionCookie = ctx.Request.Headers["Cookie"];
            //    string sessionCookie = ctx.Request.Headers["Cookie"];
            //    if (null == sessionCookie)
            //    {
            //        base.OnActionExecuting(filterContext);
            //        return;
            //    }

            //    FormsAuthentication.SignOut();
            //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            //    ctx.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //    ctx.Response.Cache.SetExpires(DateTime.Now);
            //    //ctx.Response.Redirect("~/Account/Login"); 

            //    filterContext.Result = new RedirectResult("/Account/Login");
            //    return;
            //}
            //else

            //if (!filterContext.HttpContext.Request.IsAjaxRequest())
            //{
            //    EnsureRequestIsAuthorized();            

            //    base.OnActionExecuting(filterContext);
            //}

            //base.OnActionExecuting(filterContext); 
            #endregion
        }

        private BaseResponse IsRequestAuthorized()
        {
            var response = new BaseResponse
            {
                IsSuccess = true
            };

            if (!HttpContext.Current.Request.IsAuthenticated)
            {
                response.IsSuccess = false;
                return response;
            }

            var userModules = SessionHelper.UserSecurityModules;

            if (userModules == null || userModules.Count <= 0)
            {
                response.IsSuccess = false;
                return response;
            }

            var rd = HttpContext.Current.Request.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");

            if (string.IsNullOrEmpty(currentAction) || string.IsNullOrEmpty(currentController))
            {
                currentController = "home";
                currentAction = "index";
            }

            currentController = currentController.ToLower();
            currentAction = currentAction.ToLower();

            if (currentController == "home" && currentAction == "index")
                return response = new BaseResponse {IsSuccess=true,Message="Success" };

            var isRequestModuleExist = userModules.Any(
                                       w => w.ControllerName.ToLower() == currentController.ToLower());

            if (!isRequestModuleExist)
                return response = new BaseResponse { IsSuccess = false, Message = "fail" };

            return response;
        }

        /*New Add by Akbar*/
        private BaseResponse EnsureRequestIsAuthorized()
        {
            var response = new BaseResponse
            {
                IsSuccess = true
            };

            if (!HttpContext.Current.Request.IsAuthenticated)
            {
                response.IsSuccess = false;
                return response;
            }

            var userModules = SessionHelper.UserSecurityModules;

            if (userModules == null || userModules.Count <= 0)
            {
                response.IsSuccess = false;
                return response;
            }

            var rd = HttpContext.Current.Request.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");

            var isAuthorized = userModules.Where(w => w.ControllerName.ToLower() == currentController.ToLower()).FirstOrDefault();

            if (string.IsNullOrEmpty(currentAction))
                currentAction = "Index";
            var currentModule = userModules.FirstOrDefault(w => w.ControllerName.ToLower() == currentController.ToLower()
                                       && (w.ActionName.ToLower() == currentAction.ToLower()
                                       || w.ActionName.ToLower() == "index"));

            if (currentModule == null)
            {
                response.IsSuccess = false;
                return response;
            }

            var id1 = currentModule.AspNetSecurityModuleId.ToString();
            var id2 = "-1";
            var id3 = "-1";
            if (currentModule.ParentModuleId.HasValue)
            {
                var level2Parent = currentModule.ParentModuleId.Value;
                id2 = level2Parent.ToString();
                var level2Module = userModules.Where(u => u.AspNetSecurityModuleId == level2Parent).FirstOrDefault();
                if (level2Module != null && level2Module.ParentModuleId.HasValue)
                    id3 = level2Module.ParentModuleId.Value.ToString();
            }
            var ids = string.Format("{0}_{1}_{2}", id3, id2, id1);
            SessionHelper.CurrentModuleKeys = ids;

            return response;
        }        

        public List<string> GetLeavePage()
        {
            List<string> appPage = new List<string>();
            appPage.Add("GenerateLeaveApplicationReport");
            appPage.Add("GenerateRequestLeaveReport");
            appPage.Add("FinalAdjustmentLeaveReport");
            appPage.Add("GenerateElAvailApproveReport");
            return appPage;
        }

        private bool ExcludeScurity(string controller, string action)
        {
            Dictionary<string, List<string>> controlers = new Dictionary<string, List<string>>()
            {
                 { "LeaveHistoryNew",GetLeavePage()}
            };

            var contExist = controlers.Where(w => w.Key.ToLower() == controller.ToLower()).FirstOrDefault().Value;
            if (contExist != null)
            {
                var actionExistList = contExist.ToList();
                var exist = actionExistList.Count(b => b == action) > 0;
                return exist;
            }
            else
            {
                return false;
            }

        }
    }    
}