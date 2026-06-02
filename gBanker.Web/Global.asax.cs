
using gHRM.Web.Helpers;
using gHRM.Web.Scheduler;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace gHRM.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            Bootstrapper.Run();

            if (AppSetting.GetBool(AppSetting.ScheduleEnable, null))
                ScheduleRegister.DailyLateInScheduleStart();
        }

        //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        //public class NoDirectAccessAttribute : ActionFilterAttribute
        //{
        //    public override void OnActionExecuting(ActionExecutingContext filterContext)
        //    {
        //        if (filterContext.HttpContext.Request.UrlReferrer == null ||
        //                    filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
        //        {
        //            filterContext.Result = new RedirectToRouteResult(new
        //                           RouteValueDictionary(new { controller = "Home", action = "Index", area = "" }));
        //        }
        //    }
        //}
    }
}