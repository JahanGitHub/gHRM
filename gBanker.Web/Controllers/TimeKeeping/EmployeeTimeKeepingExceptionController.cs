using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Transactions;

using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels;
using gHRM.Web.Helpers;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gHRM.Core.Filters.Payroll;
using gHRM.Core.Filters.TimeKeepings;

namespace gHRM.Web.Controllers
{
    public class EmployeeTimeKeepingExceptionController : BaseController
    {

        #region variables

        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSpService;
        private readonly IOfficeDesignationService officeDesignationService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IEmployeeTimeKeepingExceptionService employeeTimeKeepingExceptionService;
        private readonly IView_EmployeeTimeKeepingExceptionService view_EmployeeTimeKeepingExceptionService;

        public EmployeeTimeKeepingExceptionController(
            IEmployeeService employeeService,
            IEmployeeSPService employeeSpService,
            IOfficeDesignationService officeDesignationService,
            IEmployeeDepartmentService employeeDepartmentService,
            IEmployeeTimeKeepingExceptionService employeeTimeKeepingExceptionService,
            IView_EmployeeTimeKeepingExceptionService view_EmployeeTimeKeepingExceptionService

        )
        {
            this.employeeService = employeeService;
            this.employeeSpService = employeeSpService;
            this.officeDesignationService = officeDesignationService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.employeeTimeKeepingExceptionService = employeeTimeKeepingExceptionService;
            this.view_EmployeeTimeKeepingExceptionService = view_EmployeeTimeKeepingExceptionService;
        }

        #endregion

        #region Index

        public ActionResult index()
        {
            var model = new EmployeeTimeKeepingExceptionViewModel();
            MapDropdownEmployeetoAtt(model);
            return View(model);
        }

        #endregion

        #region Add

        public JsonResult SaveEmployeeTimeKeepingException(EmployeeTimeKeepingExceptionViewModel employeeTimeKeepingException)
        {
            var result = 0;
            var message = "";
            var isOperationSuccess = true;

            if (employeeTimeKeepingException.EmployeeId <= 0)
            {
                message = "Employee Name Required";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }            

            if (string.IsNullOrWhiteSpace(employeeTimeKeepingException.Justification))
            {
                message = "Please Must Add Justification";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }

            using (var ts = new TransactionScope())
            {
                try
                {
                    var entity = new EmployeeTimeKeepingException();

                    var EventStartDate = employeeTimeKeepingException.EventStartDate;
                    var EventEndDate = employeeTimeKeepingException.EventEndDate;
                    var LoggedInEmployeeID = (SessionHelper.LoggedInEmployeeID);

                    for (var EventDate = EventStartDate; EventDate <= EventEndDate; EventDate = EventDate.AddDays(1))
                    {
                        var employeeTimekeeping = employeeTimeKeepingExceptionService
                                                .GetMany(p => p.IsActive == true
                                                            && p.EmployeeId == employeeTimeKeepingException.EmployeeId
                                                            && p.EventDate == employeeTimeKeepingException.EventDate)
                                                .FirstOrDefault();
                        if (employeeTimekeeping != null && employeeTimekeeping.Id > 0)
                        {
                            employeeTimekeeping.EmployeeId = employeeTimeKeepingException.EmployeeId;
                            employeeTimekeeping.AttendenceTypeId = employeeTimeKeepingException.AttendenceTypeId;
                            employeeTimekeeping.EventDate = employeeTimeKeepingException.EventDate;
                            employeeTimekeeping.LoginTime = employeeTimeKeepingException.LoginTime;
                            employeeTimekeeping.LogoutTime = employeeTimeKeepingException.LogoutTime;
                            employeeTimekeeping.LastLoginTime = employeeTimeKeepingException.LastLoginTime;
                            employeeTimekeeping.Justification = employeeTimeKeepingException.Justification;
                            employeeTimekeeping.IsActive = true;
                            employeeTimekeeping.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                            employeeTimekeeping.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                            employeeTimekeeping.CreateDate = DateTime.UtcNow;
                            employeeTimekeeping.UpdateDate = DateTime.UtcNow;

                            //let's update in employee timekeeping exception
                            employeeTimeKeepingExceptionService.Update(employeeTimekeeping);
                        }
                        else
                        {
                            entity.EmployeeId = employeeTimeKeepingException.EmployeeId;
                            entity.AttendenceTypeId = employeeTimeKeepingException.AttendenceTypeId;
                            entity.EventDate = EventDate;
                            entity.LoginTime = employeeTimeKeepingException.LoginTime;
                            entity.LogoutTime = employeeTimeKeepingException.LogoutTime;
                            entity.LastLoginTime = employeeTimeKeepingException.LastLoginTime;
                            entity.Justification = employeeTimeKeepingException.Justification;
                            entity.IsActive = true;
                            entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                            entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                            entity.CreateDate = DateTime.UtcNow;
                            entity.UpdateDate = DateTime.UtcNow;
                            
                            //let's create employeetime keeping exception
                            employeeTimeKeepingExceptionService.Create(entity);
                        }

                        var searchFilter = new TimeKeepingExceptionSearchFilter
                        {
                            EmployeeId = Convert.ToInt32(entity.EmployeeId),
                            AttendenceTypeId = Convert.ToInt32(entity.AttendenceTypeId),
                            AttenDanceDate = entity.EventDate,
                            LoginTime = entity.LoginTime,
                            LogoutTime = entity.LogoutTime,
                            LastLoginTime = entity.LastLoginTime,
                            Justification = entity.Justification,
                            CreateUser = Convert.ToInt32(LoggedInEmployeeID)
                        };

                        //let's update in attendance table for this timekeeping exception
                        bool LEAVE_AUTO_ADJUSTMENT_DISABLED = AppSetting.GetBool(AppSetting.LEAVE_AUTO_ADJUSTMENT_DISABLED, HttpContext);
                        var response = view_EmployeeTimeKeepingExceptionService
                                                            .UpdateAttendanceForTimekeepingException(searchFilter, LEAVE_AUTO_ADJUSTMENT_DISABLED);

                        if (!response.IsSuccess)
                        {
                            isOperationSuccess = false;
                            message = response.Message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = 0;
                    message = "Save denied";
                    isOperationSuccess = false;
                }

                if (isOperationSuccess)
                {
                    result = 1;
                    message = string.IsNullOrWhiteSpace(message) ? "Saved successfully" : message;
                    ts.Complete();
                }

                ts.Dispose();
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit

        public JsonResult UpdateEmployeeTimeKeepingException(EmployeeTimeKeepingException employeeTimeKeepingException)
        {
            var result = 0;
            var message = "";
            var isOperationSuccess = true;

            var isDuplicate = employeeTimeKeepingExceptionService.GetMany(p => 
                                                            p.IsActive == true 
                                                        && p.Id != employeeTimeKeepingException.Id 
                                                        && p.EmployeeId == employeeTimeKeepingException.EmployeeId 
                                                        && p.EventDate == employeeTimeKeepingException.EventDate)
                                                    .Any();
            if (isDuplicate)
            {
                message = "Employee TimeKeeping Already exists in same date";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(employeeTimeKeepingException.Justification))
            {
                message = "Please Must Add Justification";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }

            using (var ts = new TransactionScope())
            {
                try
                {
                    var model = employeeTimeKeepingExceptionService.GetById(employeeTimeKeepingException.Id);

                    model.EmployeeId = employeeTimeKeepingException.EmployeeId;
                    model.AttendenceTypeId = employeeTimeKeepingException.AttendenceTypeId;
                    model.EventDate = employeeTimeKeepingException.EventDate;
                    model.LoginTime = employeeTimeKeepingException.LoginTime;
                    model.LogoutTime = employeeTimeKeepingException.LogoutTime;
                    model.LastLoginTime = employeeTimeKeepingException.LastLoginTime;
                    model.Justification = employeeTimeKeepingException.Justification;
                    model.IsActive = true;
                    model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.UtcNow;
                    model.UpdateDate = DateTime.UtcNow;

                    //let's update in employee timekeeping exception
                    employeeTimeKeepingExceptionService.Update(model);

                    var searchFilter = new TimeKeepingExceptionSearchFilter
                    {
                        EmployeeId = Convert.ToInt32(model.EmployeeId),
                        AttendenceTypeId = Convert.ToInt32(model.AttendenceTypeId),
                        AttenDanceDate = model.EventDate,
                        LoginTime = model.LoginTime,
                        LogoutTime = model.LogoutTime,
                        LastLoginTime = model.LastLoginTime,
                        Justification = model.Justification,
                        CreateUser = Convert.ToInt32((SessionHelper.LoggedInEmployeeID))
                    };

                    //let's update in attendance table for this timekeeping exception
                    bool LEAVE_AUTO_ADJUSTMENT_DISABLED = AppSetting.GetBool(AppSetting.LEAVE_AUTO_ADJUSTMENT_DISABLED, HttpContext);
                    var response = view_EmployeeTimeKeepingExceptionService
                                                        .UpdateAttendanceForTimekeepingException(searchFilter, LEAVE_AUTO_ADJUSTMENT_DISABLED);

                    if (!response.IsSuccess)
                    {
                        isOperationSuccess = false;
                        message = response.Message;
                    }
                }
                catch (Exception)
                {
                    result = 0;
                    message = "Update denied";
                    isOperationSuccess = false;
                }

                if (isOperationSuccess)
                {
                    result = 1;
                    message = string.IsNullOrWhiteSpace(message) ? "Saved successfully" : message;
                    ts.Complete();
                }

                ts.Dispose();
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion
        
        #region HttpRequests

        public JsonResult ListEmployeeTimeKeepingException([DataSourceRequest]DataSourceRequest request)
        {
            var listings = view_EmployeeTimeKeepingExceptionService.GetEmployeeTimeKeepingExceptions();

            DataSourceResult result = listings.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }       

        public JsonResult InformationDeleteEmployeeTimeKeepingException(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = employeeTimeKeepingExceptionService.GetById(Id);
                model.IsActive = false;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                employeeTimeKeepingExceptionService.Update(model);
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
        
        #region methods

        private void MapDropdownEmployeetoAtt(EmployeeTimeKeepingExceptionViewModel model)
        {
            var attentypellists = new List<SelectListItem>();
            attentypellists.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            attentypellists.Add(new SelectListItem() { Text = "Out Side Office Duty", Value = "11" });
            attentypellists.Add(new SelectListItem() { Text = "Regular Present", Value = "2" });
            attentypellists.Add(new SelectListItem() { Text = "On Office Tour", Value = "5" });
            attentypellists.Add(new SelectListItem() { Text = "Present Without Card", Value = "10" });
            attentypellists.Add(new SelectListItem() { Text = "Field Visit", Value = "12" });
            attentypellists.Add(new SelectListItem() { Text = "Today is in leave due to NOC Duty on Weekly Holiday", Value = "13" });
            model.AttendenceTypeNameList = attentypellists;
        }

        #endregion
    }
}