using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Service.TimeKeeping;
using gHRM.Web.Helpers;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;

namespace gHRM.Web.Controllers
{
    public class TimeKeepingRosterController : BaseController
    {
        #region Variables
        private readonly ITimeKeepingRosterService timeKeepingRosterService;
        private readonly IView_TimeKeepingRosterService view_TimeKeepingRosterService;
        private readonly IEmployeeRosterScheduleService employeeRosterScheduleService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IRoasterEmployeeScheduleService roasterEmployeeScheduleService;

        public TimeKeepingRosterController(
            ITimeKeepingRosterService timeKeepingRosterService,
            IView_TimeKeepingRosterService view_TimeKeepingRosterService,
            IEmployeeRosterScheduleService employeeRosterScheduleService,
            IEmployeeSPService employeeSPService,
            IRoasterEmployeeScheduleService roasterEmployeeScheduleService
        )
        {
            this.timeKeepingRosterService = timeKeepingRosterService;
            this.view_TimeKeepingRosterService = view_TimeKeepingRosterService;
            this.employeeRosterScheduleService = employeeRosterScheduleService;
            this.employeeSPService = employeeSPService;
            this.roasterEmployeeScheduleService = roasterEmployeeScheduleService;
        }

        #endregion

        #region Actions

        public ActionResult BMSRoaster()
        {
            return View();
        }



        public ActionResult Index()
        {
            return View();
        }

        #endregion


        #region HttpRequests

        public JsonResult SaveTimeKeepingRoster(TimeKeepingRoster timeKeepingRoster)
        {
            var result = string.Empty;
            try
            {
                var entity = new TimeKeepingRoster();
                entity.TimeKeepingRosterId = timeKeepingRoster.TimeKeepingRosterId;
                entity.RosterName = timeKeepingRoster.RosterName;
                entity.LoginTime = timeKeepingRoster.LoginTime;
                entity.LastLoginTime = timeKeepingRoster.LastLoginTime;
                entity.LogoutTime = timeKeepingRoster.LogoutTime;
                entity.EffectiveStartDate = timeKeepingRoster.EffectiveStartDate;
                entity.EffectiveEndDate = timeKeepingRoster.EffectiveEndDate;
                entity.IsActive = true;
                entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                //entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                entity.CreateDate = DateTime.UtcNow;
                //entity.UpdateDate = DateTime.UtcNow;
                timeKeepingRosterService.Create(entity);
                result = "Save Successfull";
            }
            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public JsonResult UpdateTimeKeepingRoster(TimeKeepingRoster timeKeepingRoster)
        {
            var result = string.Empty;          

            try
            {
                var employeeRoasterSchedule = roasterEmployeeScheduleService.GetByTimeKeepingRoasterId(timeKeepingRoster.TimeKeepingRosterId);

                if (employeeRoasterSchedule != null && employeeRoasterSchedule.Id > 0)
                    return Json(new { result = "This roaster is currently in used. Please try another!" }, JsonRequestBehavior.AllowGet);

                var isDuplicate =
                       timeKeepingRosterService.GetAll()
                           .Where(
                               p =>
                                   p.IsActive == true && p.TimeKeepingRosterId != timeKeepingRoster.TimeKeepingRosterId &&
                                   p.RosterName.ToUpper().Trim() == timeKeepingRoster.RosterName.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Employee Roster Name, Update denied";
                    return Json(new { result = result }, JsonRequestBehavior.AllowGet);
                }

                var entity = timeKeepingRosterService.GetById(timeKeepingRoster.TimeKeepingRosterId);

                entity.TimeKeepingRosterId = timeKeepingRoster.TimeKeepingRosterId;
                entity.RosterName = timeKeepingRoster.RosterName;
                entity.LoginTime = timeKeepingRoster.LoginTime;
                entity.LastLoginTime = timeKeepingRoster.LastLoginTime;
                entity.LogoutTime = timeKeepingRoster.LogoutTime;
                entity.EffectiveStartDate = timeKeepingRoster.EffectiveStartDate;
                entity.EffectiveEndDate = timeKeepingRoster.EffectiveEndDate;
                entity.IsActive = true;                
                entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                entity.UpdateDate = DateTime.UtcNow;

                //let's add into [TimeKeepingRoster]
                timeKeepingRosterService.Update(entity);
                result = "Update Successfull";
            }
            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ListTimeKeepingRoster([DataSourceRequest]DataSourceRequest request)
        {
            var VMcar = view_TimeKeepingRosterService.GetAll().Where(t => t.IsActive == true).ToList();

            DataSourceResult result = VMcar.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRoasterDetailsById(int id)
        {
            var loginTime = "";
            var lastLoginTime = "";
            var logoutTime = "";

            //get from [timeKeepingRoster] by id
            var timeKeepingRoster = timeKeepingRosterService.GetById(id);

            if (timeKeepingRoster != null)
            {
                loginTime = timeKeepingRoster.LoginTime.ToString("HH:mm",CultureInfo.InvariantCulture);
                lastLoginTime = timeKeepingRoster.LastLoginTime.ToString("HH:mm", CultureInfo.InvariantCulture);
                logoutTime = timeKeepingRoster.LogoutTime.ToString("HH:mm", CultureInfo.InvariantCulture);
            } 

            var newtimeKeepingRoster =new {
                LoginTime= loginTime,
                LastLoginTime= lastLoginTime,
                LogoutTime= logoutTime
            };

            return Json(newtimeKeepingRoster, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InformationDeleteTimeKeepingRoster(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var employeeRoasterSchedule = roasterEmployeeScheduleService.GetByTimeKeepingRoasterId(Id);

                if (employeeRoasterSchedule != null && employeeRoasterSchedule.Id > 0)
                    return Json(new { result=0, message = "This roaster is currently in used. Please try another!" }, JsonRequestBehavior.AllowGet);
                
                var model = timeKeepingRosterService.GetById(Id);
                model.IsActive = false;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                timeKeepingRosterService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}