using AutoMapper;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service;
using gHRM.Service.payroll;
using gHRM.Service.Payroll;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels.Payroll;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace gHRM.Web.Controllers.Payroll
{
    public class OvertimeProcessController : BaseController
    {
        #region Private Variables

        private readonly IEmployeeService employeeService;
        private readonly IAttAttendanceService att_attendanceService;
        private readonly IOvertimeHourEmployeeService overtimeHourEmployeeService;
        private readonly IOvertimeHourEmployeeApprovedService overtimeHourEmployeeApprovedService;
        private readonly IEmployeeSPService employeeSpService;
        private readonly IEmployeeMonthlySalaryService employeeMonthlySalaryService;
        private readonly IEmployeeMonthlySalaryApprovedService employeeMonthlySalaryApprovedService;
        private readonly IOvertimeConfigurationService overtimeConfigurationService;
        List<OvertimeConfiguration> OvertimeConfigurations = new List<OvertimeConfiguration>();

        public OvertimeProcessController(
            IEmployeeService employeeService,
            IAttAttendanceService att_attendanceService,
            IOvertimeHourEmployeeService overtimeHourEmployeeService,
            IOvertimeHourEmployeeApprovedService overtimeHourEmployeeApprovedService,
            IEmployeeSPService employeeSpService,
            IEmployeeMonthlySalaryService employeeMonthlySalaryService,
            IEmployeeMonthlySalaryApprovedService employeeMonthlySalaryApprovedService,
            IOvertimeConfigurationService overtimeConfigurationService)
        {
            this.employeeService = employeeService;
            this.att_attendanceService = att_attendanceService;
            this.overtimeHourEmployeeService = overtimeHourEmployeeService;
            this.overtimeHourEmployeeApprovedService = overtimeHourEmployeeApprovedService;
            this.employeeSpService = employeeSpService;
            this.employeeMonthlySalaryService = employeeMonthlySalaryService;
            this.employeeMonthlySalaryApprovedService = employeeMonthlySalaryApprovedService;
            this.overtimeConfigurationService = overtimeConfigurationService;

            OvertimeConfigurations = overtimeConfigurationService.GetAll().ToList();
        }

        #endregion

        #region HttpRequests

        [HttpGet]
        public ActionResult Index()
        {
            var model = new OvertimeHourEmployeeViewModel();

            //populate year and month listing
            MapDropdownForYearMonth(model);

            return View(model);
        }

        [HttpPost]
        public JsonResult Process(string salaryYear, string salaryMonth)
        {
            int result = 0;
            string message = "";
            int year = Convert.ToInt32(salaryYear);
            int month = Convert.ToInt32(salaryMonth);

            try
            {
                DateTime firstDayOfMonth = new DateTime(year, month, 1);
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                //check salary approved for this year and month
                var checkSalaryApproved = employeeMonthlySalaryApprovedService
                                                .GetEmployeeMonthlySalaryApprovedByYearAndMonth(year, month);

                if (checkSalaryApproved.Any())                
                    return Json(new { result = result, message = "Salary already approveed for this month" }, JsonRequestBehavior.AllowGet);

                //check salary send approved for this year and month
                var checkSalarySendApproved = employeeMonthlySalaryService.GetEmployeeMonthlySalaryActiveAndIsSendForApprovalByYearAndMonth(year, month);
                    
                if (checkSalarySendApproved.Any())                
                    return Json(new { result = result, message = "Salary already  Send for approval for this month" }, JsonRequestBehavior.AllowGet);
                
                //check overtime hour employee approved for this year and month
                var checkOvertimeApproved = overtimeHourEmployeeApprovedService
                                                .GetOvertimeHourEmployeeApprovedByYearAndMonth(year, month);
                  
                if (checkOvertimeApproved.LongCount() > 0)                
                    return Json(new { result = result, message = "Overtime already approved for this month" }, JsonRequestBehavior.AllowGet);

                //check overtime hour send approved for this year and month
                var checkOvertimeSendForApproval = overtimeHourEmployeeService.GetOvertimeHourEmployeeByYearAndMonth(year, month);
                   
                if (checkOvertimeSendForApproval.LongCount() > 0)                
                    return Json(new { result = result, message = "Overtime already send for approval for this month" }, JsonRequestBehavior.AllowGet);
                
                var param = new { SalaryYear = year, SalaryMonth = month };

                //reset OvertimeHourEmployee if any for this year and month 
                var oTHourEmployeeInActive = employeeSpService.GetDataWithParameter(param, "prl.OTHourEmployeeInActive");

                var employeeOvetimeList = new List<OvertimeHourEmployeeViewModel>();

                //get employee listing
                var employeeList = employeeService.GetMany(x => x.IsOverTime == true).OrderBy(p => p.EmployeeId).ToList();

                foreach (var employee in employeeList)
                {
                    int totalWorkHour = 0;
                    int totalOTHour = 0;

                    //get employee total last working hours
                    int employeeWorkingHourPerDay = employee.LogoutTime.HasValue && employee.LoginTime.HasValue ? (int)(employee.LogoutTime - employee.LoginTime).Value.Hours : 0;

                    //get attendance listing for this employee and around one month range (for the current month)
                    var mothlyAttendanceOfEmp = att_attendanceService.GetMany(a => a.EmployeeId == employee.EmployeeId 
                                                        && (a.AttenDate >= firstDayOfMonth && a.AttenDate <= lastDayOfMonth))
                                                            .ToList();

                    foreach (var dailyAttendanceOfEmp in mothlyAttendanceOfEmp)
                    {
                        int dailyWorkedHour = dailyAttendanceOfEmp.LogoutTime.HasValue && dailyAttendanceOfEmp.LoginTime.HasValue 
                                ? (int)(dailyAttendanceOfEmp.LogoutTime - dailyAttendanceOfEmp.LoginTime).Value.Hours : 0;

                        if (dailyWorkedHour > employeeWorkingHourPerDay)
                        {
                            int overtime = dailyWorkedHour - employeeWorkingHourPerDay;

                            if (overtime > employee.MaxOvertimePerDay)                            
                                overtime = Convert.ToInt32(employee.MaxOvertimePerDay);
                            
                            totalOTHour += overtime;
                        }

                        totalWorkHour += dailyWorkedHour;
                    }

                    if (totalOTHour > employee.MaxOvertimePerMonth)                    
                        totalOTHour = Convert.ToInt32(employee.MaxOvertimePerMonth);
                    
                    if (totalOTHour > 0)
                    {
                        var employeeOvetime = new OvertimeHourEmployeeViewModel
                        {
                            EmployeeCode = employee.EmployeeCode,
                            Month = Convert.ToString(month),
                            Year = year,
                            TotalWorkHour = totalWorkHour,
                            TotalOTHour = totalOTHour,
                            TotalOTAmount = CalculateTotalAmount(totalOTHour, employee.GrossSalary),
                            IsActive = true,
                            IsSendForApproval = false
                        };

                        employeeOvetimeList.Add(employeeOvetime);
                    }
                }

                var entity = Mapper.Map<List<OvertimeHourEmployeeViewModel>, List<OvertimeHourEmployee>>(employeeOvetimeList);
                var respose = overtimeHourEmployeeService.AddEmployeeOvertimeList(entity);
                result = 1;
                message = "Saved successfully";

            }
            catch (Exception ex)
            {
                result = 0;
                message = "Error occured, Save denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonthlyOvertime(string year, string month)
        {
            try
            {
                int salaryYear = Convert.ToInt32(year);
                int salarymonth = Convert.ToInt32(month);

                var oTDetailsMonth = overtimeHourEmployeeService.
                    GetAll()
                    .Where(p => p.Year == salaryYear && p.Month == salarymonth && p.IsActive == true)
                    .ToList();
                
                var entity = Mapper.Map<List<OvertimeHourEmployee>, List<OvertimeHourEmployeeViewModel>>(oTDetailsMonth);

                return Json(new { Result = "OK", data = entity, total = oTDetailsMonth.LongCount() }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SendForApproval(string salaryYear, string salaryMonth)
        {
            int result = 0;
            string message = "";
            int year = Convert.ToInt32(salaryYear);
            int month = Convert.ToInt32(salaryMonth);

            try
            {
                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = new DateTime(year, month, 28);

                var checkOvertimeSendForApproval = overtimeHourEmployeeService.GetOvertimeHourEmployeeByYearAndMonth(year,month);

                if (checkOvertimeSendForApproval.LongCount() > 0)
                {
                    return Json(new { result = result, message = "Overtime already send for approval for this month" }, JsonRequestBehavior.AllowGet);
                }

                var param = new { SalaryYear = year, SalaryMonth = month };

                var oTIsSendForApproval = employeeSpService.GetDataWithParameter(param, "prl.OTHourEmployeeIsSendForApproval");

                result = 1;
                message = "Send for active successfully";

            }
            catch (Exception ex)
            {
                result = 0;
                message = "Error occured, Save denied";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OvertimeApprove(string salaryYear, string salaryMonth)
        {
            int result = 0;
            string message = "";
            int year = Convert.ToInt32(salaryYear);
            int month = Convert.ToInt32(salaryMonth);

            try
            {
                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = new DateTime(year, month, 28);

                var checkOvertimeApproved = overtimeHourEmployeeApprovedService.GetOvertimeHourEmployeeApprovedByYearAndMonth(year,month);

                if (checkOvertimeApproved.LongCount() > 0)
                {
                    return Json(new { result = result, message = "Overtime already approved for this month" }, JsonRequestBehavior.AllowGet);
                }

                var checkOvertimeSendForApproval = overtimeHourEmployeeService.GetOvertimeHourEmployeeByYearAndMonth(year, month);

                if (checkOvertimeSendForApproval.LongCount() > 0)
                {
                    var entity = Mapper.Map<List<OvertimeHourEmployee>, List<OvertimeHourEmployeeApproved>>(checkOvertimeSendForApproval);
                    var respose = overtimeHourEmployeeApprovedService.AddEmployeeOvertimeApprovedList(entity);
                    result = 1;
                    message = "Overtime approved";
                }
                else
                {
                    result = 1;
                    message = "No overtime found for approve";
                }

            }
            catch (Exception ex)
            {
                result = 0;
                message = "Error occured, Save denied";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OvertimeReject(string salaryYear, string salaryMonth)
        {
            int result = 0;
            string message = "";
            int year = Convert.ToInt32(salaryYear);
            int month = Convert.ToInt32(salaryMonth);

            try
            {
                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = new DateTime(year, month, 28);

                var checkOvertimeApproved = overtimeHourEmployeeApprovedService.GetOvertimeHourEmployeeApprovedByYearAndMonth(year, month);

                if (checkOvertimeApproved.LongCount() > 0)
                {
                    return Json(new { result = result, message = "Overtime already approved for this month" }, JsonRequestBehavior.AllowGet);
                }

                var checkOvertimeSendForApproval = overtimeHourEmployeeService.GetOvertimeHourEmployeeByYearAndMonth(year, month);

                if (checkOvertimeSendForApproval.LongCount() > 0)
                {
                    var param = new { SalaryYear = year, SalaryMonth = month };
                    var updateOTIsSendForApproval = employeeSpService.GetDataWithParameter(param, "prl.OTHourEmployeeRejctIsSendForApproval");

                    result = 1;
                    message = "Overtime Rejected";
                }
                else
                {
                    result = 1;
                    message = "No overtime found for Reject";
                }

            }
            catch (Exception ex)
            {
                result = 0;
                message = "Error occured, Save denied";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }
            else
            {
                int overtimeConfigId = id.Value;
                try
                {

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost]
        public JsonResult Update(OvertimeConfigurationViewModel model)
        {
            int result = 0;
            string message = "";
            try
            {
                result = 1;
                message = "Data Updated Successfully";
            }
            catch (Exception ex)
            {
                result = 0;
                message = "Error occured, Save denied";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }
            else
            {
                int overtimeConfigId = id.Value;

                try
                {
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        #endregion

        #region Methods

        public decimal CalculateTotalAmount(int totalOTHour, decimal? gross)
        {
            decimal totaAmount = 0;
            int oTHours = totalOTHour;
            foreach (var overtimeConfiguration in OvertimeConfigurations)
            {
                int workingHour = (int)(overtimeConfiguration.HourTo - overtimeConfiguration.HourFrom) + 1;

                if (Convert.ToInt16(overtimeConfiguration.Rule) == 1)//fixed
                {
                    totaAmount += oTHours * Convert.ToDecimal(overtimeConfiguration.Amount);
                    oTHours -= workingHour;
                }

                if (Convert.ToInt16(overtimeConfiguration.Rule) == 2) //gross
                {
                    decimal grossAmount = Convert.ToDecimal(gross);
                    decimal grossDividedBy = Convert.ToDecimal(overtimeConfiguration.DividedBy);
                    totaAmount += oTHours * (grossAmount / grossDividedBy);
                    oTHours -= workingHour;
                }


                if (Convert.ToInt16(overtimeConfiguration.Rule) == 3) //basic
                {
                    //decimal grossAmount = Convert.ToDecimal(basic);
                    //decimal basicDividedBy = Convert.ToDecimal(overtimeConfiguration.DividedBy);
                    //totaAmount += OTHours * (grossAmount / basicDividedBy);
                    //OTHours -= workingHour;
                }

                if (oTHours <= workingHour)
                {
                    break;
                }
            }
            return totaAmount;
        }

        public void MapDropdownForYearMonth(OvertimeHourEmployeeViewModel model)
        {
            var yearList = new List<SelectListItem>();
            yearList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 0; i <= 2; i++)
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