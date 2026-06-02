
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Globalization;
using gHRM.Web.Helpers;

namespace gHRM.Web.Controllers
{
    public class AttendanceController : BaseController
    {
        #region variables

        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IOfficeService officeService;

        private readonly IAttAttendanceService AttAttendanceService;
        private readonly IView_TimeKeepingDetailService view_TimeKeepingDetailService;
        private readonly IAspNetUserService aspNetUserService;

        public AttendanceController(
              IEmployeeService employeeService
            , IEmployeeSPService employeeSPService
            , IOfficeTypeService officeTypeService
            , IOfficeService officeService
            , IAttAttendanceService Att_AttendanceService
            , IView_TimeKeepingDetailService view_TimeKeepingDetailService
            ,IAspNetUserService aspNetUserService

            )
        {
            this.employeeService = employeeService;
            this.employeeSPService = employeeSPService;
            this.officeTypeService = officeTypeService;
            this.officeService = officeService;
            this.AttAttendanceService = Att_AttendanceService;
            this.view_TimeKeepingDetailService = view_TimeKeepingDetailService;
            this.aspNetUserService = aspNetUserService;

        }

        #endregion

        #region events

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index2()
        {
            return View();
        }

        #endregion

        #region Take Manual Attendance

        public ActionResult Create()
        {
            var model = new AttAttendanceViewModel();
            ViewData["currentDate"] = DateTime.Now;
            model.EmployeeId   = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
            model.EmployeeCode   = employeeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployeeID)).EmployeeCode;
            model.CompanyCode = SessionHelper.CompanyCode;


            model.InOutTime = DateTime.Now;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["OfficeDayTypeList"] = items;
            return View(model);
        }

        public ActionResult Create2()
        {
            var model = new AttAttendanceViewModel();
            ViewData["currentDate"] = DateTime.Now;
            model.EmployeeId = Convert.ToInt32(SessionHelper.LoggedInEmployeeID);
            model.EmployeeCode = employeeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployeeID)).EmployeeCode;
            model.CompanyCode = SessionHelper.CompanyCode;


            model.InOutTime = DateTime.Now;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["OfficeDayTypeList"] = items;
            return View(model);
        }

        public JsonResult CreateTime(string EmployeeId, string remark, string Clock, string AttOfficeDayTypeId)
        {
            try
            {
                var employeeId = Convert.ToInt64(EmployeeId);
                var attendanceDate = DateTime.Today.Date;
                var empDetail = employeeService.Get(p => p.EmployeeId == employeeId);

                var paramAttendance = new
                {
                    EmployeeCode = empDetail.EmployeeCode,
                    EmployeeId = employeeId,
                    AttenDate = attendanceDate,
                    LogInType = "M",
                    Remark= remark,
                    LogInTime = DateTime.Parse(Clock),
                    logOutTime = DateTime.Parse(Clock),
                    LEAVE_AUTO_ADJUSTMENT_DISABLED = AppSetting.GetBool(AppSetting.LEAVE_AUTO_ADJUSTMENT_DISABLED, HttpContext)
                };

                var val = employeeSPService.GetDataWithParameter(paramAttendance, "att.SP_InsertManualAttendance");

                return GetSuccessMessageResult("Time Entry Successfully.");
            }
            catch (Exception ex)
            {
                return GetSuccessMessageResult(ex.InnerException.ToString());
            }
        }


        #endregion


        #region HttpRequests

        public JsonResult GetEmpInfoByCode(string employeeID)
        {
            try
            {
                List<AttAttendanceViewModel> List_EmployeeViewModel = new List<AttAttendanceViewModel>();
                //var Emp = EmployeeService.GetByCode(employee_code);
                var param = new { EmployeeID = employeeID };
                var empList = employeeSPService.GetDataWithParameter(param, "emp.SP_GetEmployeeDetails");

                List_EmployeeViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new AttAttendanceViewModel
                {
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeId = row.Field<long>("EmployeeID"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    OfficeId = row.Field<int>("OfficeId")
                }).ToList();

                if (List_EmployeeViewModel.Any())
                {
                    var loggedInEmployeeId = LoggedInOfficeID;
                    if (loggedInEmployeeId == List_EmployeeViewModel[0].OfficeId)
                    {
                        List_EmployeeViewModel[0].ValidOfficeEmployee = "Yes";
                    }
                    else
                    {
                        List_EmployeeViewModel[0].ValidOfficeEmployee = "No";
                    }
                }
                else
                {
                    var entity = new AttAttendanceViewModel();
                    entity.ValidOfficeEmployee = "No";
                    List_EmployeeViewModel.Add(entity);
                }

                return Json(List_EmployeeViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetEmpLeaveInfoByCode(string employeeID)
        {
            try
            {
                List<AttAttendanceViewModel> List_EmployeeViewModel = new List<AttAttendanceViewModel>();
                //var Emp = EmployeeService.GetByCode(employee_code);
                DateTime dt = DateTime.Now;
                var param = new { EmployeeId = employeeID, Attndate = dt.Date };//](@ varchar(100), @Attndate  Date)
                var empList = employeeSPService.GetDataWithParameter(param, "leave.SP_GetEmployeeLeaveDetails");

                List_EmployeeViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new AttAttendanceViewModel
                {
                    EmployeeId = row.Field<long>("EmployeeID")

                }).ToList();
                //If Data found Employee On Leave
                //
                if (List_EmployeeViewModel.Count == 0)
                {
                    Response.StatusCode = 404;
                }

                return Json(List_EmployeeViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetOfficeDayTypeList()
        {
            List<AttAttendanceViewModel> List_ViewModel = new List<AttAttendanceViewModel>();

            var List = employeeSPService.GetDataWithoutParameter("att.SP_Get_AttOfficeDayType");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new AttAttendanceViewModel
            {
                AttOfficeDayTypeId = row.Field<int>("AttOfficeDayTypeId"),
                OfficeDayTypeShortName = row.Field<string>("OfficeDayTypeShortName"),
                OfficeDayTypeFullName = row.Field<string>("OfficeDayTypeFullName"),


            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.AttOfficeDayTypeId.ToString(),
                Text = string.Format("{0} - {1}", x.OfficeDayTypeFullName, x.AttOfficeDayTypeId)
            });

            var Component_items = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items.AddRange(Components);
            return Json(Component_items, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetClock()
        {
            try
            {
                List<AttAttendanceViewModel> List_EmployeeViewModel = new List<AttAttendanceViewModel>();

                AttAttendanceViewModel List_EmployeeViewModel2 = new AttAttendanceViewModel();
                //var Emp = EmployeeService.GetByCode(employee_code);
                DateTime d = DateTime.Now;
                List_EmployeeViewModel2.Clock = d.Hour.ToString() + ":" + d.Minute.ToString() + ":" + d.Second.ToString();
                List_EmployeeViewModel2.CurrentDate = Convert.ToDateTime(d.Date).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture); //d.Date.ToString("dd/mm/yyyy");//DateTime.ParseExact(d.Date.ToString,"ddMMyyyy"); //d.Date.ParseExact(dateString, "ddMMyyyy".ToString();//DateTime.ParseExact(dateString, "ddMMyyyy", 

                List_EmployeeViewModel.Add(List_EmployeeViewModel2);
                return Json(List_EmployeeViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAtt_AttendanceList([DataSourceRequest]DataSourceRequest request, string EmployeeId, string AttenDate)
        {
            try
            {
                var attendanceList = new List<View_TimeKeepingDetail>();

                if (Convert.ToInt32(EmployeeId) > 0 && AttenDate != "0")
                {
                    var employeeCode = employeeService.GetById(Convert.ToInt32(EmployeeId)).EmployeeCode;
                    var attendateDate = Convert.ToDateTime(AttenDate);
                    attendanceList = view_TimeKeepingDetailService.GetMany(l => l.EmployeeCode == employeeCode && l.AttenDate == attendateDate).ToList();
                }
                else if (Convert.ToInt32(EmployeeId) > 0 && AttenDate == "0")
                {
                    var employeeCode = employeeService.GetById(Convert.ToInt32(EmployeeId)).EmployeeCode;
                    attendanceList = view_TimeKeepingDetailService.GetMany(l => l.EmployeeCode == employeeCode).ToList();
                }
                else if (EmployeeId == "0" && AttenDate != "0")
                {
                    attendanceList = view_TimeKeepingDetailService.GetMany(l => l.AttenDate == Convert.ToDateTime(AttenDate)).ToList();
                }
                //else if (EmployeeId == "0" && AttenDate == "0")
                //{
                //    attendanceList = view_TimeKeepingDetailService.GetAll().OrderByDescending(o => o.AttenDate).ToList();
                //}
                //var empcode = employeeService.GetById(Convert.ToInt16(LoggedInEmployeeId)).EmployeeCode;
                else if (EmployeeId == "0" && AttenDate == "0")
                {
                    long? empid = LoggedInEmployeeId;
                    int RoleId = aspNetUserService.GetAll().Where(w => w.EmployeeId == empid).Select(w=>w.RoleId).FirstOrDefault();
                    if (RoleId == 1)
                    {
                        attendanceList = view_TimeKeepingDetailService.GetAll().OrderByDescending(o => o.AttenDate).ToList();
                    }
                    else
                    {
                        var loginOfficeId = LoggedInOfficeID;
                        attendanceList = view_TimeKeepingDetailService.GetAll().Where(p => p.OfficeId == loginOfficeId).OrderByDescending(o => o.AttenDate).ToList();
                    }
                }

                DataSourceResult result = attendanceList.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        #endregion

        #region methods

        //private void BackTimeEntry(string EmployeeId, string remark, string Clock, string AttenDate, string AttOfficeDayTypeId)
        //{
        //    var model = new AttAttendanceViewModel();
        //    int employeeID = Convert.ToInt32(EmployeeId);
        //    int AttOfficeDayType = Convert.ToInt32(AttOfficeDayTypeId);
        //    model.AttOfficeDayTypeId = AttOfficeDayType;
        //    model.EmployeeId = employeeID;
        //    //model.Remark = remark;
        //    model.Clock = Clock;


        //    var entity = Mapper.Map<AttAttendanceViewModel, AttAttendance>(model);
        //    try
        //    {
        //        if (entity.EmployeeId == 0)
        //        {
        //            Response.StatusCode = 403;
        //            throw new Exception("Employee Not Found.");
        //        }

        //        DateTime d = Convert.ToDateTime(AttenDate);

        //        string AtteDate = d.Month.ToString("00") + "/" + d.Day.ToString("00") + "/" + d.Year.ToString();


        //        var param = new { EmployeeId = entity.EmployeeId, Date = d.Date };
        //        var empList = employeeSPService.GetDataWithParameter(param, "att.SP_GetLastAttenStatus");

        //        if (empList.Tables[0].Rows.Count > 0)
        //        {
        //            string io = empList.Tables[0].Rows[0]["InOutType"].ToString();
        //            if (io == "I")
        //            {
        //                entity.InOutType = "O";
        //            }
        //            else
        //            {
        //                entity.InOutType = "I";
        //            }
        //        }
        //        else
        //        {
        //            entity.InOutType = "I";
        //        }


        //        entity.EmployeeId = Convert.ToInt32(entity.EmployeeId);
        //        entity.AttenDate = Convert.ToDateTime(AttenDate); //DateTime.Now;  //model.AttenDate;

        //        string AttendanceDate = entity.AttenDate.Month.ToString("00") + "/" + entity.AttenDate.Day.ToString("00") + "/" + entity.AttenDate.Year.ToString();

        //        // entity.Remark = model.Remark;
        //        entity.AttOfficeMachineId = 1; // Temporary 1 Will be from session
        //        entity.LogInType = "M";  // Manual Entry

        //        //Need to add Date and Time Together..
        //        DateTime date = Convert.ToDateTime(AtteDate);
        //        DateTime time = Convert.ToDateTime(model.Clock);
        //        DateTime dtCOMPLTDTTM = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);


        //        entity.InOutTime = dtCOMPLTDTTM;  //DateTime.Parse(model.Clock);

        //        entity.AttOfficeDayTypeId = model.AttOfficeDayTypeId;
        //        entity.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
        //        entity.CreateDate = DateTime.Now;

        //        var param2 = new
        //        {

        //            EmployeeId = entity.EmployeeId,
        //            AttenDate = AttendanceDate,
        //            LogInType = entity.LogInType,
        //            InOutType = entity.InOutType,
        //            InOutTime = entity.InOutTime,
        //            AttOfficeMachineId = entity.AttOfficeMachineId,
        //            AttOfficeDayTypeId = entity.AttOfficeDayTypeId,
        //            CreateUser = entity.CreateUser,
        //            CreateDate = entity.CreateDate

        //        };
        //        var val = employeeSPService.GetDataWithParameter(param2, "att.SP_Attendance_ManualEntry");

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        #endregion

    }
}