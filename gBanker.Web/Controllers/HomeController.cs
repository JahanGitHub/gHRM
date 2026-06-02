
#region Usings

using AutoMapper;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using gHRM.Web.ViewModels.Dashboard;
using System.Configuration;

#endregion

namespace gHRM.Web.Controllers
{    
    public class HomeController : BaseController
    {
        #region Private Variables

        private readonly IEmployeeOfficeMappingService employeeOfficeService;
        private readonly IOfficeService officeService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly ICompanyService companyService;

        #endregion

        #region Ctor

        public HomeController(IEmployeeOfficeMappingService employeeOfficeService, IOfficeService officeService, IEmployeeSPService employeeSPService, ICompanyService companyService)
        {
            this.employeeOfficeService = employeeOfficeService;
            this.officeService = officeService;
            this.employeeSPService = employeeSPService;
            this.companyService = companyService;
        }

        #endregion

        #region Index

        public ActionResult Index()
        {
            bool DASHBOARD_ENABLED = AppSetting.GetBool(AppSetting.DASHBOARD_ENABLED, HttpContext);
            if (DASHBOARD_ENABLED)
            {
                var _Company = companyService.GetAll().Where(p => p.CompanyId == SessionHelper.CompanyID).FirstOrDefault();
                if (_Company.CompanyCode.Trim() == gHRM.Core.Utilities.Constants.GHRMPlusCompanyConstants.GrameenCommunications)
                {
                    if (LoggedInEmployeeId != 2 && LoggedInEmployeeId != 10 && LoggedInEmployeeId != 3) DASHBOARD_ENABLED = false;
                   // && LoggedInEmployeeId != 3
                }
               
            }
            //DASHBOARD_ENABLED = false;

            if (SessionHelper.CompanyCode == "Prottyashi" || SessionHelper.CompanyCode == "GTT")
            {
                var _role = new gHRMDBContext().AspNetUsers.Where(k => k.EmployeeId == LoggedInEmployeeId).Select(z => z.RoleId).FirstOrDefault();

                var param = new { EmployeeId = LoggedInEmployeeId, RoleId = _role };

                var enableRoleList = employeeSPService.GetDataWithParameter(param, "SP_ENABLEDASHBOARD");
                // Check if the role exists in the comma-separated string from the SP result
                if (enableRoleList.Tables[0].Rows[0][0].ToString().Split(',').Contains(_role.ToString()))
                {
                    DASHBOARD_ENABLED = true;
                }
            }


            ViewBag.ShowPopup = false;
            DashboardHelper _Helper = new DashboardHelper(HttpContext);
            ViewBag.Data = _Helper.LoadData();

            if (SessionHelper.LoginUserOfficeID == default(int?))
                ViewBag.ShowPopup = true;

            if (SessionHelper.LoggedInEmployee != null)
            {
                var officeList = "remove";
                ViewBag.EmployeeOfficeMappings = officeList;
            }

            var returnUrl = SessionHelper.SSOReturnUrl != null ? SessionHelper.SSOReturnUrl : string.Empty;
            var model = new HomePageViewModel
            {
                ReturnUrl = returnUrl
            };

            SessionHelper.SSOReturnUrl = "";

            if (string.IsNullOrWhiteSpace(SessionHelper.SSOEncryptedUserCredential) )
            {
                return DASHBOARD_ENABLED ? View("IndexDashboard", model) : View(model);
            }
            //IndexDashboard

          //track credentials for possible sso instances
          model = TrackCredentialsForSSOInstances(model);

            return DASHBOARD_ENABLED ? View("IndexDashboard", model) : View(model);
        }

        #region For ERP Dashboard
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Welcome()
        {
            return View();
        }
        #endregion For ERP Dashboard
        public string GMailInit()
        {
            EmailHelper _EmailHelper = new EmailHelper(HttpContext);
            return "Success";
        }

        public string GMailTest(string Subject, string Body, string To, string Key)
        {
            if (Key == "test123send456") {
                string Message;
                EmailHelper _EmailHelper = new EmailHelper(HttpContext);
                _EmailHelper.SendMail(Subject, Body, To, out Message);
                return "" == Message ? "success" : Message;
            }
            return "Not authorized";
        }
        #endregion

        #region Demo Video

        public ActionResult Demovideo(string filename)
        {
            string videoUrl = $"/mediacontents/app_demo_videos/{filename}.mp4";

            var model = new DemoVideoViewModel
            {
                VideoUrl = videoUrl,
                VideoType = "video/mp4"
            };

            return View(model);
        }

        #endregion

        #region Dashboard

        public ActionResult Dashboard()
        {
            ViewBag.ShowPopup = false;
            if (SessionHelper.LoginUserOfficeID == default(int?))
            {
                ViewBag.ShowPopup = true;
            }
            if (SessionHelper.LoggedInEmployee != null)
            {
                var officeList = "remove";
                ViewBag.EmployeeOfficeMappings = officeList;
            }

            return View();
        }

        #endregion

        #region About
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        #endregion

        #region Contact
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        #endregion

        #region Ajax Http Requests

        [HttpPost]
        public JsonResult SelectOffice(int officeId)
        {
            if (officeId > 0)
            {
                SessionHelper.LoginUserOfficeID = officeId;
                var office = officeService.GetById(SessionHelper.LoginUserOfficeID.Value);
                var entity = Mapper.Map<Office, OfficeViewModel>(office);
                SessionHelper.LoggedInOfficeDetail = entity;
                try
                {
                    var dayInitialStatus = "1";
                    SessionHelper.TransactionDay = dayInitialStatus;
                    SessionHelper.TransactionDate = DateTime.Now;
                    SessionHelper.OrganizationName = "GC";
                    SessionHelper.ProcessType = "P";
                    SessionHelper.LastDayEndDate = DateTime.Now;
                    SessionHelper.IsDayInitiated = !string.IsNullOrEmpty(dayInitialStatus);

                }
                catch (Exception ex)
                {
                    SessionHelper.IsDayInitiated = false;
                }
            }
            var resultObj = new { TransactionDashBoardString = SessionHelper.TransactionDashBoardString };
            return new JsonResult() { Data = resultObj };
        }
        public JsonResult GetDashboardItems()
        {
            var dashboardModel = new DashboardViewModel();
            dashboardModel.TotalOfficeCount = 2538; // officeService.GetAllOfficeCount();
            //dashboardModel.TotalOrganizationMemberCount = memberService.GetTotalOrganizationMember();
            return new JsonResult() { Data = dashboardModel };
        }
        public JsonResult GetZoneDashboard(string DtFrom, string DtTo)
        {
            try
            {
                //int jtStartIndex, int jtPageSize, string jtSorting
                List<DashboardViewModel> List_ZoneViewModel = new List<DashboardViewModel>();
                var param = new { DateFrom = DtFrom, DateTo = DtTo };
                var zoneList = employeeSPService.GetDataWithParameter(param, "emp.SP_GET_Zone_Dashboard");

                List_ZoneViewModel = zoneList.Tables[0].AsEnumerable()
                .Select(row => new DashboardViewModel
                {
                    ZoneName = row.Field<string>("OfficeName"),
                    TotalPO = row.Field<int>("TotPO"),
                    TotalJoin = row.Field<int>("TotJoin"),
                    TotalLeaveSale = row.Field<int>("TotLeaveSale")

                }).ToList();

                //var currentPageRecords = List_ZoneViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = List_ZoneViewModel, TotalRecordCount = List_ZoneViewModel.LongCount(), JsonRequestBehavior.AllowGet });

                //return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetNotifications()
        {
            var result = 0;
            var notification = new List<NotificationModuleViewModel>();
            //var param = new { EmployeeId = LoggedInEmployeeId };
            if ((LoggedInEmployeeId??0) > 0)
            {

                try
                {
                    notification = new gHRMDBContext().Database.SqlQuery<NotificationModuleViewModel>("dbo.SP_GetNotificationCount " + (LoggedInEmployeeId ?? 0) + "").ToList();
                    //var notificationCount = employeeSPService.GetDataWithParameter(param, "leave.SP_GetNotificationCount");
                    //notification = notificationCount.Tables[0].AsEnumerable().Select(row => new NotificationModuleViewModel
                    //{
                    //    NotificationCount = row.Field<int>("NotificationCount"),
                    //    LinkText = row.Field<string>("LinkText") + "(" + row.Field<int>("NotificationCount").ToString() + ")",
                    //    ControllerName = row.Field<string>("ControllerName"),
                    //    ActionName = row.Field<string>("ActionName"),
                    //}).ToList();
                    result = notification.Count();
                }
                catch(Exception ex)
                {

                }
            }
            //var notification = notificationCount.Tables[0].AsEnumerable().Select(row=>new SelectListItem
            //{
            //    Text = row.Field<string>("ModuleName")+"( "+row.Field<int>("NotificationCount").ToString()+" )",
            //    Value = row.Field<string>("ModuleName")
            //}).ToList();

            return Json(new { result = result, data = notification }, JsonRequestBehavior.AllowGet);
        }
        
        //public JsonResult GetNotificationsCar()
        //{
        //    var result = 0;
        //    var notification = new List<NotificationModuleViewModel>();
        //    var param = new { EmployeeId = LoggedInEmployeeId };
        //    if (param.EmployeeId != null && param.EmployeeId > 0)
        //    {
        //        var notificationCount = employeeSPService.GetDataWithParameter(param, "dbo.SP_GetNotificationCountCar");
        //        notification = notificationCount.Tables[0].AsEnumerable().Select(row => new NotificationModuleViewModel
        //        {
        //            NotificationCount = row.Field<int>("NotificationCount"),
        //            LinkText = row.Field<string>("LinkText") + "(" + row.Field<int>("NotificationCount").ToString() + ")",
        //            ControllerName = row.Field<string>("ControllerName"),
        //            ActionName = row.Field<string>("ActionName"),
        //        }).ToList();
        //        result = notification.Count();
        //    }
        //    //var notification = notificationCount.Tables[0].AsEnumerable().Select(row=>new SelectListItem
        //    //{
        //    //    Text = row.Field<string>("ModuleName")+"( "+row.Field<int>("NotificationCount").ToString()+" )",
        //    //    Value = row.Field<string>("ModuleName")
        //    //}).ToList();

        //    return Json(new { result = result, data = notification }, JsonRequestBehavior.AllowGet);
        //}


        #endregion

        #region Private Methods

        private HomePageViewModel TrackCredentialsForSSOInstances(HomePageViewModel model)
        {
            //let's create trigger to track this credentials for possible sso instances
            var instanceListing = ConfigurationManager.AppSettings["gHRM.Cookie.SingleSignOn.Instances"];
            if (instanceListing == null || string.IsNullOrWhiteSpace(instanceListing.ToString()))
                return model;

            string instances = instanceListing.ToString();
            var fragmentedInstances = instances.Split('@');
            var ssoInstances = new List<SSOInstanceViewModel>();

            //get current instance base url 
            var currentInstanceUrl = $"{HttpContext.Request.Url.Scheme}://{HttpContext.Request.Url.Authority}";

            foreach (var instanceUrl in fragmentedInstances)
            {
                if (currentInstanceUrl.ToLower() == instanceUrl.ToLower())
                    continue;
                var ssoInstance = new SSOInstanceViewModel
                {
                    BaseUrl = instanceUrl,                   
                    EncryptedCredential = SessionHelper.SSOEncryptedUserCredential
                };
                ssoInstances.Add(ssoInstance);
            }

            model.SSOInstances = ssoInstances;

            //reset sso session
            SessionHelper.SSOUsername = string.Empty;
            SessionHelper.SSOEncryptedUserCredential = string.Empty;

            return model;
        }

        #endregion
    }
}
