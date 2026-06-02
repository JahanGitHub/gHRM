using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Web.ViewModels;
using gHRM.Web.Helpers;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gHRM.Service.TimeKeeping;
using gHRM.Core.Utilities.Constants;

namespace gHRM.Web.Controllers
{
    public class TimeKeepingExceptionController : BaseController
    {
        #region variables

        private readonly IOfficeTypeService officeTypeService;
        private readonly IOfficeService officeService;
        private readonly ITimeKeepingRosterService timeKeepingRosterService;
        private readonly IEmployeeOfficeTimeExceptionService employeeOfficeTimeExceptionService;
        private readonly IView_EmployeeOfficeTimeExceptionService view_EmployeeOfficeTimeExceptionService;
        private readonly IRoasterEmployeeScheduleService roasterEmployeeScheduleService;
        public TimeKeepingExceptionController(
            IOfficeTypeService officeTypeService,
            IOfficeService officeService,
            ITimeKeepingRosterService timeKeepingRosterService,
            IEmployeeOfficeTimeExceptionService employeeOfficeTimeExceptionService,
            IView_EmployeeOfficeTimeExceptionService view_EmployeeOfficeTimeExceptionService,
            IRoasterEmployeeScheduleService roasterEmployeeScheduleService
        )
        {
            this.officeTypeService = officeTypeService;
            this.officeService = officeService;
            this.timeKeepingRosterService = timeKeepingRosterService;
            this.employeeOfficeTimeExceptionService = employeeOfficeTimeExceptionService;
            this.view_EmployeeOfficeTimeExceptionService = view_EmployeeOfficeTimeExceptionService;
            this.roasterEmployeeScheduleService = roasterEmployeeScheduleService;
        }

        #endregion

        #region events

        public ActionResult OfficeTimeException()
        {
            var entity = new EmployeeOfficeTimeExceptionViewModel();
            MapDropDownForTimeException(entity);
            return View(entity);
        }

        #endregion


        #region HttpRequests

        public JsonResult SaveOfficeTimeException(EmployeeOfficeTimeException employeeOfficeTimeException)
        {
            var result = string.Empty;
            try
            {
                var entity = new EmployeeOfficeTimeException();
                entity.Id = employeeOfficeTimeException.Id;
                entity.OfficeTypeId = employeeOfficeTimeException.OfficeTypeId;
                entity.OfficeId = employeeOfficeTimeException.OfficeId;
                entity.LogInTime = employeeOfficeTimeException.LogInTime;
                entity.LastLogInTime = employeeOfficeTimeException.LastLogInTime;
                entity.LogOutTime = employeeOfficeTimeException.LogOutTime;
                entity.EffectiveDateFrom = employeeOfficeTimeException.EffectiveDateFrom;
                entity.EffectiveDateTo = employeeOfficeTimeException.EffectiveDateTo;
                entity.TimeExceptionReason = employeeOfficeTimeException.TimeExceptionReason;
                entity.TimeKeepingRosterId = employeeOfficeTimeException.TimeKeepingRosterId;
                entity.IsActive = true;
                entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                entity.CreateDate = DateTime.UtcNow;
                entity.UpdateDate = DateTime.UtcNow;
                employeeOfficeTimeExceptionService.Create(entity);
                result = "Save Successfull";
            }
            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UpdateOfficeTimeException(EmployeeOfficeTimeException employeeOfficeTimeException)
        {
            var result = string.Empty;
            var message = "";
            try
            {
                var entity = employeeOfficeTimeExceptionService.GetById(employeeOfficeTimeException.Id);
                entity.Id = employeeOfficeTimeException.Id;
                entity.OfficeTypeId = employeeOfficeTimeException.OfficeTypeId;
                entity.OfficeId = employeeOfficeTimeException.OfficeId;
                entity.LogInTime = employeeOfficeTimeException.LogInTime;
                entity.LastLogInTime = employeeOfficeTimeException.LastLogInTime;
                entity.LogOutTime = employeeOfficeTimeException.LogOutTime;
                entity.EffectiveDateFrom = employeeOfficeTimeException.EffectiveDateFrom;
                entity.EffectiveDateTo = employeeOfficeTimeException.EffectiveDateTo;
                entity.TimeExceptionReason = employeeOfficeTimeException.TimeExceptionReason;
                entity.TimeKeepingRosterId = employeeOfficeTimeException.TimeKeepingRosterId;
                entity.IsActive = true;
                entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                entity.CreateDate = DateTime.UtcNow;
                entity.UpdateDate = DateTime.UtcNow;
                employeeOfficeTimeExceptionService.Update(entity);
                result = "Update Successfull";
            }

            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ListOfficeTimeException([DataSourceRequest]DataSourceRequest request)
        {
            var VMcar = view_EmployeeOfficeTimeExceptionService.GetAll().Where(t => t.IsActive == true).ToList();

            DataSourceResult result = VMcar.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult InformationDeleteOfficeTimeException(int Id)
        {
            var result = 0;
            var message = "";
            try
            {

                var updateEmployeeOfficeTimeException = employeeOfficeTimeExceptionService.GetById(Id);

                if (updateEmployeeOfficeTimeException == null)
                    return Json(new { result = result, message = "Warning, Timekeeping Exception not found!" }, JsonRequestBehavior.AllowGet);
                
                var responseValidity = roasterEmployeeScheduleService.IsCurrentlyUsedInAttendance(null,
                                                (DateTime)updateEmployeeOfficeTimeException.EffectiveDateFrom,
                                                (DateTime)updateEmployeeOfficeTimeException.EffectiveDateTo,
                                                TimeKeepingTypeConstants.OfficeTimeException);

                if (!responseValidity.IsSuccess)                
                    return Json(new { result = result, message = responseValidity.Message }, JsonRequestBehavior.AllowGet); 

                updateEmployeeOfficeTimeException.IsActive = false;
                updateEmployeeOfficeTimeException.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                updateEmployeeOfficeTimeException.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                updateEmployeeOfficeTimeException.CreateDate = DateTime.UtcNow;
                updateEmployeeOfficeTimeException.UpdateDate = DateTime.UtcNow;

                //let's udpate for [EmployeeOfficeTimeException ]
                employeeOfficeTimeExceptionService.Update(updateEmployeeOfficeTimeException);
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


        #region Methods

        public void MapDropDownForTimeException(EmployeeOfficeTimeExceptionViewModel entity)
        {
            var officeType = officeTypeService.GetAll().Where(w => w.IsActive == true);
            var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeTypeId.ToString(),
                Text = string.Format("{0}", x.OfficeTypeName)
            });
            var officeType_items = new List<SelectListItem>();
            officeType_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            officeType_items.AddRange(viewofficeType);
            entity.OfficeTypeList = officeType_items;

            var ZoneList = officeService.GetAll().Where(x => x.OfficeTypeId == 4 && x.IsActive == true);
            var viewZoneList = ZoneList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = x.OfficeName.ToString()
            });
            var zone_items = new List<SelectListItem>();
            zone_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            zone_items.AddRange(viewZoneList);
            entity.ZoneList = zone_items;

            var area_items = new List<SelectListItem>();
            area_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            entity.AreaList = area_items;

            var unit_items = new List<SelectListItem>();
            unit_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            entity.UnitList = unit_items;

            var exception_items = new List<SelectListItem>();
            exception_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            exception_items.Add(new SelectListItem() { Text = "Ramadan", Value = "Ramadan" });
            exception_items.Add(new SelectListItem() { Text = "Winter Season", Value = "Winter Season" });
            exception_items.Add(new SelectListItem() { Text = "Other", Value = "Other" });
            entity.TimeExceptionReasonList = exception_items;

            var timekeepingroster = timeKeepingRosterService.GetAll().Where(p => p.IsActive == true);
            var viewtimekeepingroster = timekeepingroster.Select(a => new SelectListItem()
            {
                Value = a.TimeKeepingRosterId.ToString(),
                Text = a.RosterName
            });
            var listoftimekeepingroster = new List<SelectListItem>();
            listoftimekeepingroster.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            listoftimekeepingroster.AddRange(viewtimekeepingroster);
            entity.RosterNameList = listoftimekeepingroster;
        }

        #endregion

    }
}