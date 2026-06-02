using AutoMapper;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.CodeFirstMigration.Payroll;
//using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service;
using gHRM.Service.payroll;
using gHRM.Service.Payroll;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels.payroll;
using gHRM.Web.ViewModels.Payroll;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace gHRM.Web.Controllers
{
    public class OvertimeBillController : Controller
    {
        #region variables

        private readonly IEmployeeService employeeService;
        private readonly IAttAttendanceService att_attendanceService;
        private readonly IOvertimeHourEmployeeService overtimeHourEmployeeService;
        private readonly IOvertimeHourEmployeeApprovedService overtimeHourEmployeeApprovedService;
        private readonly IEmployeeSPService employeeSpService;
        private readonly IEmployeeMonthlySalaryService employeeMonthlySalaryService;
        private readonly IEmployeeMonthlySalaryApprovedService employeeMonthlySalaryApprovedService;
        private readonly IOvertimeConfigurationService overtimeConfigurationService;
        private readonly IEmployeeSalaryIncentiveService employeeSalaryIncentiveService;
        private readonly IPRComponentService prComponentService;
        List<OvertimeConfiguration> OvertimeConfigurations = new List<OvertimeConfiguration>();

        public OvertimeBillController(
            IEmployeeService employeeService,
            IAttAttendanceService att_attendanceService,
            IOvertimeHourEmployeeService overtimeHourEmployeeService,
            IOvertimeHourEmployeeApprovedService overtimeHourEmployeeApprovedService,
            IEmployeeSPService employeeSpService,
            IEmployeeMonthlySalaryService employeeMonthlySalaryService,
            IEmployeeMonthlySalaryApprovedService employeeMonthlySalaryApprovedService,
            IOvertimeConfigurationService overtimeConfigurationService,
            IEmployeeSalaryIncentiveService employeeSalaryIncentiveService,
            IPRComponentService prComponentService)
        {
            this.employeeService = employeeService;
            this.att_attendanceService = att_attendanceService;
            this.overtimeHourEmployeeService = overtimeHourEmployeeService;
            this.overtimeHourEmployeeApprovedService = overtimeHourEmployeeApprovedService;
            this.employeeSpService = employeeSpService;
            this.employeeMonthlySalaryService = employeeMonthlySalaryService;
            this.employeeMonthlySalaryApprovedService = employeeMonthlySalaryApprovedService;
            this.overtimeConfigurationService = overtimeConfigurationService;
            this.employeeSalaryIncentiveService = employeeSalaryIncentiveService;
            this.prComponentService = prComponentService;
            OvertimeConfigurations = overtimeConfigurationService.GetAll().ToList();
        }

        #endregion

        #region HTTPRequests

        [HttpGet]
        public ActionResult Index()
        {
            var model = new OvertimeHourEmployeeViewModel();
            return View(model);
        }

        //[HttpGet]
        public JsonResult getOvertimeNotPaid()
        {
            try
            {
                var OTDetailsMonth = overtimeHourEmployeeApprovedService.GetAll()
                  .Where(p => p.IsActive == true && p.IsPaid != true)
                  .ToList();

                var entity = Mapper.Map<List<OvertimeHourEmployeeApproved>, List<OvertimeHourEmployeeViewModel>>(OTDetailsMonth);

                return Json(new { Result = "OK", data = entity, total = OTDetailsMonth.LongCount() }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult PayOvertime(int id,string Remarks)
        {
            int result = 0;
            string message = "";

            try
            {
                var param = new { RowId = id, Remarks= Remarks };
                var UpdateOTIsPaid = employeeSpService.GetDataWithParameter(param, "OTHourEmployeeApprovedIsPaid");

                result = 1;
                message = "Overtime approved successfully";

            }
            catch (Exception ex)
            {
                result = 0;
                message = "Error occured, Save denied";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RejectOvertime(int id)
        {
            int result = 0;
            string message = "";

            try
            {
                var param = new { RowId = id };
                var OTHourEmployeeApprovedInActive = employeeSpService.GetDataWithParameter(param, "OTHourEmployeeApprovedIsPaid");

                result = 1;
                message = "Overtime Rejected successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = "Error occured, Save denied";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult OvertimeWithSalay()
        {
            var model = new OvertimeHourEmployeeViewModel();
            MapDropdownForYearMonth(model);
            return View(model);
        }

        [HttpPost]
        public JsonResult ApproveOvertimeWithSalay(string salaryYear, string salaryMonth)
        {
            int result = 0;
            string message = "";
            try
            {
                int year = Convert.ToInt32(salaryYear);
                int month = Convert.ToInt32(salaryMonth);

                var firstDayOfMonth = new DateTime(year, month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                var CheckSalaryApproved = employeeMonthlySalaryApprovedService.GetAll().Where(p => p.SalaryYear == year && p.SalaryMonth == month).ToList();
                if (CheckSalaryApproved.Any())
                {
                    return Json(new { result = result, message = "Salary already approveed for this month" }, JsonRequestBehavior.AllowGet);
                }

                var CheckSalarySendApproved = employeeMonthlySalaryService.GetAll()
                .Where(p => p.SalaryYear == year && p.SalaryMonth == month && p.IsActive == true && p.IsSendForApproval == true).ToList();

                if (CheckSalarySendApproved.Any())
                {
                    return Json(new { result = result, message = "Salary already  Send for approval for this month" }, JsonRequestBehavior.AllowGet);
                }

                var EmployeeApprovedOT = overtimeHourEmployeeApprovedService.GetAll()
                 .Where(p => p.IsActive == true && p.IsPaid != true).ToList();

                List<EmployeeSalaryIncentive> EmployeeSalaryIncentives = new List<EmployeeSalaryIncentive>();

                foreach (var otEmployee in EmployeeApprovedOT)
                {
                    var employee = employeeService.GetByCode(otEmployee.EmployeeCode);
                    if (employee != null)
                    {
                        var prComponent = prComponentService.Get(c => c.ComponentName == "Overtime" && c.EmployeeTypeId == employee.EmployeeTypeId && c.EmployeeStatusId == employee.EmployeeStatusId);

                        if (prComponent != null)
                        {
                            EmployeeSalaryIncentive employeeSalaryIncentive = new EmployeeSalaryIncentive
                            {
                                EmployeeId = Convert.ToInt32(employee.EmployeeId),
                                PRComponentId = prComponent.PRComponentID,
                                PRComponentAmount = Convert.ToDecimal(otEmployee.TotalOTAmount),
                                PRComponentHour = otEmployee.TotalOTHour,
                                IsActive = true,
                                IsApproved = true,
                                StartDate = firstDayOfMonth,
                                EndDate = lastDayOfMonth,
                                //CreatedBy = 123,
                                //UpdatedBy = 456,
                                CreateDate = DateTime.Now.Date,
                                UpdateDate = DateTime.Now.Date
                            };
                            EmployeeSalaryIncentives.Add(employeeSalaryIncentive);
                        }
                    }
                }

                if (EmployeeSalaryIncentives.Any())
                {
                    var response = employeeSalaryIncentiveService.AddTADA(EmployeeSalaryIncentives);
                    var param = new { SalaryYear = year, SalaryMonth = month };
                    var OTHourEmployeeApprovedInActive = employeeSpService.GetDataWithParameter(param, "OTHourEmployeeApprovedIsPaidAll");
                }
                result = 1;
                message = "Over time Approved With Salary";

            }
            catch (Exception ex)
            {
                result = 0;
                message = "Error occured, Save denied";
            }

            return Json(new
            {
                result = result,
                message = message
            }, JsonRequestBehavior.AllowGet);

        }


        #endregion

        #region Methods

        public void MapDropdownForYearMonth(OvertimeHourEmployeeViewModel model)
        {
            var yearList = new List<SelectListItem>();
            yearList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 0; i <= 30; i++)
            {
                yearList.Add(new SelectListItem { Text = (DateTime.Today.Year + i).ToString(), Value = (DateTime.Today.Year + i).ToString() });
            }
            model.YearList = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 1; i <= 12; i++)
            {
                monthList.Add(new SelectListItem { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Value = i.ToString() });
            }
            model.MonthList = monthList;
        }

        #endregion

    }
}