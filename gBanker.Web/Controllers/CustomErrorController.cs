using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers
{
    public class CustomErrorController : Controller
    {
        // GET: CustomError
        public ActionResult Index()
        {
            var loggedUser = Session["USER_ID"];
            if (loggedUser != null)
            {
                var validationError = Session["UNAUTHORIZED_ACCES"] as string;
                if (string.IsNullOrEmpty(validationError))
                    validationError = "Exception occured. Please contact with your administrator to fix the error.";
                ViewBag.UnauthorizedAccessError = validationError;
                Session["UNAUTHORIZED_ACCES"] = null;
                return View();
            }
            else
            {
                //"~/Account/Login"
                return RedirectToAction("Login", "Account");
            }


        }
    }
}