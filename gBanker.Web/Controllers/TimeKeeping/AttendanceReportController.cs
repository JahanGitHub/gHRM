using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using gHRM.Web.Helpers;
using System.Globalization;
using Kendo.Mvc.Extensions;
using gHRM.Web.Reports;
using gHRM.Web.Reports.TimeKeeping;
using gHRM.Core.Filters.TimeKeepings;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service.payroll;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.DBDetailModels.OverTimes;
using gHRM.Core.Utilities.Constants;

namespace gHRM.Web.Controllers
{
    public class AttendanceReportController : BaseController
    {

        #region variables

        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IOfficeService officeService;
        private readonly IOvertimeConfigurationService overtimeConfigurationService;
        private readonly IOvertimeHourEmployeeService overtimeHourEmployeeService;

        private readonly IAttAttendanceService AttAttendanceService;
        private readonly IView_TimeKeepingDetailService view_TimeKeepingDetailService;

        public AttendanceReportController(
              IEmployeeService employeeService
            , IEmployeeSPService employeeSPService
            , IOfficeTypeService officeTypeService
            , IOfficeService officeService
            , IOvertimeConfigurationService overtimeConfigurationService
            , IAttAttendanceService Att_AttendanceService
            , IView_TimeKeepingDetailService view_TimeKeepingDetailService
            , IOvertimeHourEmployeeService overtimeHourEmployeeService

            )
        {
            this.employeeService = employeeService;
            this.employeeSPService = employeeSPService;
            this.officeTypeService = officeTypeService;
            this.officeService = officeService;
            this.overtimeConfigurationService = overtimeConfigurationService;
            this.overtimeHourEmployeeService = overtimeHourEmployeeService;
            this.AttAttendanceService = Att_AttendanceService;
            this.view_TimeKeepingDetailService = view_TimeKeepingDetailService;

        }

        #endregion

        #region events

        public ActionResult AttendanceReport()
        {
            var model = new AttAttendanceViewModel();
            MapDropDown(model);
            return View(model);
        }

        public ActionResult AttendanceStatus()
        {
            var model = new AttAttendanceViewModel();
            MapDropdownForEmployeeStatus(model);
            return View(model);
        }


        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public ActionResult RPTMESSAGE()
        //{
        //    return View();
        //}

        //public ActionResult AttendanceRegister()
        //{
        //    return View();
        //}

        //public ActionResult MonthlyAttenSummary()
        //{
        //    return View();
        //}

        //public ActionResult AttendanceDetail()
        //{
        //    return View();+-66
        //}

        #endregion

        #region HttpRequests

        public ActionResult EmployeeCodeReport(string EmpCode, string DateFrom, string DateTo, int OfficeId)
        {
            try
            {
                PrintTimeKeepingReport(EmpCode, Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo));
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ZKBioRawData(string EmpCode, string DateFrom, string DateTo, int OfficeId)
        {
            try
            {
                PrintTimeKeepingReport2(EmpCode, Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo));
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult EmployeeCodeReportBySingleCode(string EmployeeCode, int Month, int Year)
        {
            try
            {
                var startDate = new DateTime(Year, Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                PrintTimeKeepingReport(EmployeeCode, startDate, endDate);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public void PrintTimeKeepingReport(string EmpCode, DateTime DateFrom, DateTime DateTo)
        {
            try
            {                
                var filter = new TimeKeepingReportSearchFilter
                {
                    EmployeeCode = EmpCode,
                    StartDate = DateFrom,
                    EndDate = DateTo,
                    PreparedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID),
                    GHRMPlusCompany = SessionHelper.CompanyCode
                };

                var listings = employeeSPService.GetTimeKeepingReportDataByFilter(filter);
                var employee = employeeService.GetByCode(EmpCode);

                //get total overtime amount
                var totalOvertimeAmount = GetTotalOvertimeAmount(employee, DateFrom, DateTo, listings);
                              
                foreach (var item in listings)
                {
                    item.WorkingHourSUM = GetItem(item).WorkingHourSUM.Replace(':', '.');
                    item.OverTime = employee.IsOverTime == false ? "00:00:00" : item.OverTime;
                    item.OverTimeHourSUM = employee.IsOverTime == false ? "00.00" : item.OverTimeHourSUM;
                    item.TotalOvertimeAmount = employee.IsOverTime == false ? "" : totalOvertimeAmount;
                }

                var param2 = new { EmployeeCode = EmpCode, AttendaceDateFrom = DateFrom, AttendanceDateTo = DateTo };
                var CodeWiseAttendanceSummary = employeeSPService.GetDataWithParameter(param2, "att.SP_RPT_TypeWiseAttendanceSummaryByEmpCode");

                var param3 = new { EmployeeCode = EmpCode, DateFrom = DateFrom };
                var leaveSummary = employeeSPService.GetDataWithParameter(param3, "leave.LeaveHistory_GetLeaveSummary");//"leave.SP_RPT_GetLeaveBalance_ALCL");

                var reportParam = new Dictionary<string, object>();
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("AttendanceSummery", CodeWiseAttendanceSummary.Tables[0]);
                subReportDB.Add("AttendanceALCL", leaveSummary.Tables[0]);

                if (SessionHelper.CompanyCode == "GT")
                {
                    ReportHelper.PrintTimeKeepingWithSubReport("TimeKeeping/Timekeeping_ByEmployeeCodeReport_GT.rpt", listings, reportParam, subReportDB);
                }
                else
                {
                    ReportHelper.PrintTimeKeepingWithSubReport("TimeKeeping/Timekeeping_ByEmployeeCodeReport.rpt", listings, reportParam, subReportDB);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void PrintTimeKeepingReport2(string EmpCode, DateTime DateFrom, DateTime DateTo)
        {
            try
            {
                var filter = new TimeKeepingReportSearchFilter
                {
                    EmployeeCode = EmpCode,
                    StartDate = DateFrom,
                    EndDate = DateTo,
                    PreparedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID),
                    GHRMPlusCompany = SessionHelper.CompanyCode
                };


                // Convert input strings to DateTime
                var fromDate = Convert.ToDateTime(DateFrom);
                var toDate = Convert.ToDateTime(DateTo);

                var paramValues = new List<Service.ReportExecutionService.ParameterValue>
        {
            new Service.ReportExecutionService.ParameterValue { Name = "CompanyName", Value = SessionHelper.CompanyName },
            new Service.ReportExecutionService.ParameterValue { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress },
            new Service.ReportExecutionService.ParameterValue { Name = "DateFrom", Value = DateFrom.ToString("yyyy-MM-dd") },
            new Service.ReportExecutionService.ParameterValue { Name = "DateTo", Value = DateTo.ToString("yyyy-MM-dd") },
            new Service.ReportExecutionService.ParameterValue { Name = "EmpCode", Value = EmpCode == "" ? "0" : EmpCode },
            new Service.ReportExecutionService.ParameterValue { Name = "OfficeId", Value = "0" }
        };

                // Call the SSRS report printer
                PrintSSRSReport("/gHRMPlus_Reports/ZKBioAttendance", paramValues.ToArray());

               // return Content(string.Empty);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        private TimeKeepingReportModel GetItem(TimeKeepingReportModel item)
        {                        
            if(string.IsNullOrWhiteSpace(item.WorkingHour) || item.WorkingHour.Trim()=="0")
                item.WorkingHour = "0";

            var fragmentedWorkingHour = item.WorkingHour.Split(':');

            if (fragmentedWorkingHour.Length == 3)  //example=> 12:45:36
            {
                var workingHours = Convert.ToDecimal(fragmentedWorkingHour[0]);
                var workingMinutes = Convert.ToDecimal(fragmentedWorkingHour[1]);
                var workingSeconds = Convert.ToDecimal(fragmentedWorkingHour[2]);
                
                if (workingMinutes > 0)
                    workingHours = workingHours + (workingMinutes / 60);

                if(workingSeconds>0)
                    workingHours = workingHours + (workingSeconds / 3600);

                item.WorkingHourSUM = workingHours.ToString();
            }


            return item;
        }


        //public ActionResult ComponentPayrollReport(string DateFrom, string DateTo, string ComponentCategory, string ComponentName, int IsApproved, string EmployeeCode)
        //{
        //    try
        //    {
        //        int _companyId = CompanyID.Value;
        //        var param = new { DateFrom = DateFrom, DateTo = DateTo, ComponentCategory = ComponentCategory, ComponentName = ComponentName, IsApproved = IsApproved, EmployeeCode = EmployeeCode };
        //        var MainReport = employeeSpService.GetDataWithParameter(param, "prl.SP_RPT_ComponentPayroll");
        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("DateFrom", DateFrom);
        //        reportParam.Add("DateTo", DateTo);
        //        reportParam.Add("ComponentCategory", ComponentCategory);
        //        reportParam.Add("ComponentName", ComponentName);


        //        ReportHelper.PrintReport("Payroll/rpt_ComponentPayroll.rpt", MainReport.Tables[0], reportParam);
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
  
        public JsonResult OverTimeProcessEntry(string year, string month)
        {
            int result = 0;
            string message = string.Empty;
            try
            {
                int Yearss = Convert.ToInt32(year);
                int Monthhs = Convert.ToInt32(month);

                DateTime DateFrom = new DateTime(Yearss, Monthhs, 1);
                DateTime DateTo = DateFrom.AddMonths(1).AddDays(-1);

                var employeecodes = employeeService.GetAll().Where(x => x.IsActive == true && x.IsOverTime == true).Select(x => x.EmployeeCode).ToList();

                foreach (var EmpCode in employeecodes)
                {
                    var filter = new TimeKeepingReportSearchFilter
                    {
                        EmployeeCode = EmpCode,
                        StartDate = DateFrom,
                        EndDate = DateTo,
                        PreparedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID)
                    };
                    var mainReport = employeeSPService.GetTimeKeepingReportDataByFilter(filter);
                    var timeKeepingListing = mainReport.ToList();
                    var EmployeeCode = timeKeepingListing[0].EmployeeCode;
                    var AttendaceDateFrom = timeKeepingListing[0].AttendaceDateFrom;
                    var Year = Convert.ToDateTime(AttendaceDateFrom).Year;
                    var Month = Convert.ToDateTime(AttendaceDateFrom).Month;
                    var WorkingHourSUM = timeKeepingListing[0].WorkingHourSUM;
                    var OverTimeHourSUM = timeKeepingListing[0].OverTimeHourSUM;
                    var dividedby = overtimeConfigurationService.GetAll().Where(x => x.OvertimeConfigId == 1).Select(x => x.DividedBy).SingleOrDefault();
                    var grosssalary = employeeService.GetAll().Where(x => x.EmployeeCode == EmployeeCode && x.IsActive == true).Select(x => x.GrossSalary).SingleOrDefault();
                    var TotalOTAmount = (Convert.ToDecimal(grosssalary) / Convert.ToInt16(dividedby)) * Convert.ToDecimal(OverTimeHourSUM);
                    var isOvertimeapplicable = employeeService.GetAll().Where(x => x.EmployeeCode == EmployeeCode && x.IsActive == true).Select(x => x.IsOverTime == true).SingleOrDefault();

                    var entity = new OvertimeHourEmployee();
                    entity.EmployeeCode = EmployeeCode;
                    entity.Year = Year;
                    entity.Month = Month;
                    entity.TotalWorkHour = Convert.ToDecimal(WorkingHourSUM);
                    entity.TotalOTHour = Convert.ToDecimal(OverTimeHourSUM);
                    entity.TotalOTAmount = Convert.ToDecimal(TotalOTAmount);
                    entity.IsActive = true;
                    entity.IsSendForApproval = false;
                    entity.CreateBy = null;
                    entity.UpdateBy = null;
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    overtimeHourEmployeeService.Create(entity);
                }
                message = "OverTime Process Successfully";
                result = 1;
                return Json(new { result, message }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                throw;
                message = "Update Failed";
            }
            return Json(new { result, message }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult DailyAttendanceReportPrint(string Date, int OfficeId)
        {
            try
            {
                var param = new { AttendaceDate = Date, FilterOfficeId = OfficeId, PreparedByEmpId = LoggedInEmployee.EmployeeId };
                var MainReport = employeeSPService.GetDataWithParameter(param, "att.SP_RPT_TimeKeeping_DailyAttendanceReport");

                var param2 = new { AttendaceDate = Date, FilterOfficeId = OfficeId };
                var TypeWiseAttendanceSummary = employeeSPService.GetDataWithParameter(param2, "att.SP_RPT_TypeWiseAttendanceSummary");

                var subReportDB = new Dictionary<string, DataTable>();

                subReportDB.Add("TypeWiseAttendanceSummary", TypeWiseAttendanceSummary.Tables[0]);

                var reportParam = new Dictionary<string, object>();

                ReportHelper.PrintWithSubReport("TimeKeeping/rpt_TimeKeeping_DailyAttendanceReport.rpt", MainReport.Tables[0], reportParam, subReportDB, new rpt_TimeKeeping_DailyAttendanceReport());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult DailyAttendanceReportPrintWithoutGroup(string Date, int OfficeId)
        {
            try
            {
                var param = new { AttendaceDate = Date, FilterOfficeId = OfficeId, PreparedByEmpId = LoggedInEmployee.EmployeeId };
                var MainReport = employeeSPService.GetDataWithParameter(param, "att.SP_RPT_TimeKeeping_DailyAttendanceReport");
                var param2 = new { AttendaceDate = Date, FilterOfficeId = OfficeId };
                var TypeWiseAttendanceSummary = employeeSPService.GetDataWithParameter(param2, "att.SP_RPT_TypeWiseAttendanceSummary");

                var subReportDB = new Dictionary<string, DataTable>();

                subReportDB.Add("AttendanceSummary", TypeWiseAttendanceSummary.Tables[0]);

                var reportParam = new Dictionary<string, object>();

                ReportHelper.PrintWithSubReport("TimeKeeping/rpt_TimeKeeping_DailyAttendanceReport_WithoutGroup.rpt", MainReport.Tables[0], reportParam, subReportDB, new rpt_TimeKeeping_DailyAttendanceReport_WithoutGroup());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        public ActionResult MonthlyAttendanceReportPrint(string DateFrom, string DateTo, int OfficeId
            ,int? departmentId,int? sectionId)
        {
            try
            {
                var param = new { AttendaceDateFrom = DateFrom, AttendanceDateTo = DateTo, FilterOfficeId = OfficeId
                    , PreparedByEmpId = LoggedInEmployeeId,FilterDepartmentId=departmentId,SectionId= sectionId
                };

                var Data = employeeSPService.GetDataWithParameter(param, "att.SP_RPT_TimeKeeping_ByDatePeriod_V2");//SP_RPT_LeaveApprovalNoteSheet
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);

                ReportHelper.PrintReport("TimeKeeping/RPT_TimeKeeping_ByDatePeriod.rpt", Data.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult DailyPresentReportPrint(string DateFrom, string DateTo, string ReportType, int OfficeId)
        {
            try
            {

                var param = new { AttendanceTypeParam = ReportType, FilterOfficeId = OfficeId, PreparedByEmpId = LoggedInEmployee.EmployeeId, AttendaceDateFrom = DateFrom, AttendanceDateTo = DateTo };
                var Data = employeeSPService.GetDataWithParameter(param, "att.SP_RPT_TimeKeeping_DailyPresentReport");//SP_RPT_LeaveApprovalNoteSheet
                var reportParam = new Dictionary<string, object>();
                if (ReportType == "A")
                {
                    ReportHelper.PrintReport("TimeKeeping/RPT_TimeKeeping_DailyAbsentReport.rpt", Data.Tables[0], reportParam);
                }
                else
                {
                    ReportHelper.PrintReport("TimeKeeping/rpt_TimeKeeping_DailyAttendanceByStatusType.rpt", Data.Tables[0], reportParam);
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult ManuallyDailyUpdatedReportPrint(string Date, int OfficeId)
        {
            try
            {
                var param = new { AttenDate = Date, FilterOfficeId = OfficeId, PreparedByEmpId = LoggedInEmployee.EmployeeId };
                var Data = employeeSPService.GetDataWithParameter(param, "att.SP_RPT_TimeKeeping_ManuallyUpdatedReport");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("TimeKeeping/rpt_TimeKeeping_ManuallyUpdatedReport.rpt", Data.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult ManuallyMonthlyUpdatedReportPrint(string Month, int OfficeId)
        {
            try
            {
                var firstDayOfMonth = new DateTime(DateTime.Today.Year, Convert.ToInt32(Month), 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                var param = new { AttendaceDateFrom = firstDayOfMonth, AttendanceDateTo = lastDayOfMonth, FilterOfficeId = OfficeId, PreparedByEmpId = LoggedInEmployee.EmployeeId };
                var Data = employeeSPService.GetDataWithParameter(param, "att.SP_RPT_TimeKeeping_ManuallyMonthlyUpdatedReport");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("TimeKeeping/rpt_TimeKeeping_ManuallyMonthlyUpdatedReport.rpt", Data.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult DailyTourReportPrint(string Date, int OfficeId)
        {
            try
            {
                //var c=CompanyLogoUrl
                var param = new { EventDate = Date, FilterOfficeId = OfficeId, PreparedByEmpId = LoggedInEmployee.EmployeeId, logoUrl = CompanyLogoUrl };
                var Data = employeeSPService.GetDataWithParameter(param, "att.SP_GetTourReportData");//SP_RPT_LeaveApprovalNoteSheet
                var reportParam = new Dictionary<string, object>();
                //reportParam.Add("logoUrl", CompanyLogoUrl);
                ReportHelper.PrintReport("TimeKeeping/rpt_TourReport.rpt", Data.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult DailyAttendanceReportExcelPrint(string Date, int OfficeId)
        {
            try
            {
                var param = new { AttendaceDate = Date, FilterOfficeId = OfficeId, PreparedByEmpId = LoggedInEmployee.EmployeeId };
                var MainReport = employeeSPService.GetDataWithParameter(param, "att.SP_RPT_TimeKeeping_DailyAttendanceReport");

                var param2 = new { AttendaceDate = Date, FilterOfficeId = OfficeId };
                var TypeWiseAttendanceSummary = employeeSPService.GetDataWithParameter(param2, "att.SP_RPT_TypeWiseAttendanceSummary");

                var subReportDB = new Dictionary<string, DataTable>();

                subReportDB.Add("TypeWiseAttendanceSummary", TypeWiseAttendanceSummary.Tables[0]);

                var reportParam = new Dictionary<string, object>();

                ReportHelper.ExportExcelWithSubReport("TimeKeeping/rpt_TimeKeeping_DailyAttendanceReport.rpt", MainReport.Tables[0], reportParam, subReportDB, new rpt_TimeKeeping_DailyAttendanceReport());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult DailyAttendanceReportExcelPrintWithoutGroup(string Date, int OfficeId)
        {
            try
            {
                var param = new { AttendaceDate = Date, FilterOfficeId = OfficeId, PreparedByEmpId = LoggedInEmployee.EmployeeId };
                var MainReport = employeeSPService.GetDataWithParameter(param, "att.SP_RPT_TimeKeeping_DailyAttendanceReport");
                var param2 = new { AttendaceDate = Date, FilterOfficeId = OfficeId };
                var TypeWiseAttendanceSummary = employeeSPService.GetDataWithParameter(param2, "att.SP_RPT_TypeWiseAttendanceSummary");

                var subReportDB = new Dictionary<string, DataTable>();

                subReportDB.Add("AttendanceSummary", TypeWiseAttendanceSummary.Tables[0]);

                var reportParam = new Dictionary<string, object>();

                ReportHelper.ExportExcelWithSubReport("TimeKeeping/rpt_TimeKeeping_DailyAttendanceReport_WithoutGroup.rpt", MainReport.Tables[0], reportParam, subReportDB, new rpt_TimeKeeping_DailyAttendanceReport_WithoutGroup());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult MonthlyAttendanceReportExcelPrint(string DateFrom, string DateTo, int OfficeId, int? departmentId, int? sectionId)
        {
            try
            {                
                var param = new {
                    AttendaceDateFrom = DateFrom,
                    AttendanceDateTo = DateTo,
                    FilterOfficeId = OfficeId, PreparedByEmpId = LoggedInEmployeeId,
                    FilterDepartmentId = departmentId,
                    SectionId = sectionId
                };
                var Data = employeeSPService.GetDataWithParameter(param, "att.SP_RPT_TimeKeeping_ByDatePeriod_V2");//SP_RPT_LeaveApprovalNoteSheet
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.ExportExcelReport("TimeKeeping/RPT_TimeKeeping_ByDatePeriod.rpt", Data.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult EmployeeCodeReportExcel(string EmpCode, string DateFrom, string DateTo, int OfficeId)
        {
            try
            {
                var filter = new TimeKeepingReportSearchFilter
                {
                    EmployeeCode = EmpCode,
                    StartDate = Convert.ToDateTime(DateFrom),                   
                    EndDate = Convert.ToDateTime(DateTo),
                    PreparedBy = Convert.ToInt32(SessionHelper.LoggedInEmployeeID),
                    GHRMPlusCompany=SessionHelper.CompanyCode
                };

                var listings = employeeSPService.GetTimeKeepingReportDataByFilter(filter);
                var employee = employeeService.GetByCode(EmpCode);

                //get total overtime amount
                var totalOvertimeAmount = GetTotalOvertimeAmount(employee, Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo), listings);

                foreach (var item in listings)
                {
                    item.WorkingHourSUM = GetItem(item).WorkingHourSUM.Replace(':', '.');
                    item.OverTime = employee.IsOverTime == false ? "00:00:00" : item.OverTime;
                    item.OverTimeHourSUM = employee.IsOverTime == false ? "00.00" : item.OverTimeHourSUM;
                    item.TotalOvertimeAmount = employee.IsOverTime == false ? "" : totalOvertimeAmount;
                }

                //var param = new { EmployeeCode = EmpCode, AttendaceDateFrom = DateFrom, AttendanceDateTo = DateTo, PreparedByEmpId = LoggedInEmployee.EmployeeId };//, FilterOfficeId = OfficeId
                //var MainReport = employeeSPService.GetDataWithParameter(param, "att.SP_RPT_Timekeeping_ByEmployeeCode");


                var param2 = new { EmployeeCode = EmpCode, AttendaceDateFrom = DateFrom, AttendanceDateTo = DateTo };
                var CodeWiseAttendanceSummary = employeeSPService.GetDataWithParameter(param2, "att.SP_RPT_TypeWiseAttendanceSummaryByEmpCode");

                var param3 = new { EmployeeCode = EmpCode, DateFrom = DateFrom };
                var CodeWiseAttendanceALCL = employeeSPService.GetDataWithParameter(param3, "leave.LeaveHistory_GetLeaveSummary");//leave.SP_RPT_GetLeaveBalance_ALCL

                var reportParam = new Dictionary<string, object>();
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("AttendanceSummery", CodeWiseAttendanceSummary.Tables[0]);
                subReportDB.Add("AttendanceALCL", CodeWiseAttendanceALCL.Tables[0]);


                //var param2 = new { EmployeeCode = EmpCode, AttendaceDateFrom = DateFrom, AttendanceDateTo = DateTo };
                //var CodeWiseAttendanceSummary = employeeSPService.GetDataWithParameter(param2, "att.SP_RPT_TypeWiseAttendanceSummaryByEmpCode ");

                //var subReportDB = new Dictionary<string, DataTable>();
                //subReportDB.Add("AttendanceSummery", CodeWiseAttendanceSummary.Tables[0]);

                //var reportParam = new Dictionary<string, object>();

                ReportHelper.ExportExcelWithSubReport("TimeKeeping/Timekeeping_ByEmployeeCodeReport.rpt", listings, reportParam, subReportDB, new Timekeeping_ByEmployeeCodeReport());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult DailyPresentReportExcelPrint(string DateFrom, string DateTo, string ReportType, int OfficeId)
        {
            try
            {

                var param = new { AttendanceTypeParam = ReportType, FilterOfficeId = OfficeId, PreparedByEmpId = LoggedInEmployee.EmployeeId, AttendaceDateFrom = DateFrom, AttendanceDateTo = DateTo };
                var Data = employeeSPService.GetDataWithParameter(param, "att.SP_RPT_TimeKeeping_DailyPresentReport");//SP_RPT_LeaveApprovalNoteSheet
                var reportParam = new Dictionary<string, object>();
                if (ReportType == "A")
                {
                    ReportHelper.ExportExcelReport("TimeKeeping/RPT_TimeKeeping_DailyAbsentReport.rpt", Data.Tables[0], reportParam);
                }
                else
                {
                    ReportHelper.ExportExcelReport("TimeKeeping/rpt_TimeKeeping_DailyAttendanceByStatusType.rpt", Data.Tables[0], reportParam);
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult ManuallyDailyUpdatedReportExeclPrint(string Date, int OfficeId)
        {
            try
            {
                var param = new { AttenDate = Date, FilterOfficeId = OfficeId, PreparedByEmpId = LoggedInEmployee.EmployeeId };
                var Data = employeeSPService.GetDataWithParameter(param, "att.SP_RPT_TimeKeeping_ManuallyUpdatedReport");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.ExportExcelReport("TimeKeeping/rpt_TimeKeeping_ManuallyUpdatedReport.rpt", Data.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult ManuallyMonthlyUpdatedReportExcelPrint(string Month, int OfficeId)
        {
            try
            {
                var firstDayOfMonth = new DateTime(DateTime.Today.Year, Convert.ToInt32(Month), 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                var param = new { AttendaceDateFrom = firstDayOfMonth, AttendanceDateTo = lastDayOfMonth, FilterOfficeId = OfficeId, PreparedByEmpId = LoggedInEmployee.EmployeeId };
                var Data = employeeSPService.GetDataWithParameter(param, "att.SP_RPT_TimeKeeping_ManuallyMonthlyUpdatedReport");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.ExportExcelReport("TimeKeeping/rpt_TimeKeeping_ManuallyMonthlyUpdatedReport.rpt", Data.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region Methods

        private string GetTotalOvertimeAmount(Employee employee, DateTime DateFrom, DateTime DateTo,
            IEnumerable<Data.DBDetailModels.OverTimes.TimeKeepingReportModel> mainReport)
        {
            string totalOvertimeAmountInText = "N/A";
            try
            {
                double totalOvertimeAmount = 0;

                if (mainReport.Any() && employee != null && employee.IsOverTime == true
                                    && employee.LogoutTime != null && employee.LoginTime != null)
                {
                    var singleReportItem = mainReport.FirstOrDefault();
                    var startDate = Convert.ToDateTime(DateFrom);
                    var endtDate = Convert.ToDateTime(DateTo);
                    double totalWorkingDays = (endtDate - startDate).TotalDays + 1;

                    if (SessionHelper.PayrollType == PayrollTypeConstants.FixedDays)
                    {
                        var lastDateOfMonth = DateTime.DaysInMonth(endtDate.Year, endtDate.Month);
                        if (lastDateOfMonth == endtDate.Day)
                            totalWorkingDays = Convert.ToInt32(SessionHelper.NoOfSalaryDays);
                    }
                   
                    double employeeWorkingHour = (Convert.ToDateTime(employee.LogoutTime) - Convert.ToDateTime(employee.LoginTime)).TotalHours;
                    double employeeGrossSalary = employee.GrossSalary > 0 ? Convert.ToDouble(employee.GrossSalary) : 0;
                    double totalOverTimeHour = Convert.ToDouble(singleReportItem.OverTimeHourSUM);
                    double maxOverTime = Convert.ToDouble(employee.MaxOvertimePerMonth ?? 0);

                    if ("true" == GetSetting("EMPLOYEE_GROSS_SALARY_IS_TWICE_OF_BASIC_SALARY"))
                    {
                        employeeGrossSalary = Convert.ToDouble(employeeService.GetEmployeeBasicSalary(employee.EmployeeId)) * 2;
                    }
                    totalOverTimeHour = totalOverTimeHour > maxOverTime ? maxOverTime : totalOverTimeHour;

                    //double perHourAmount = employeeGrossSalary / (totalWorkingDays * employeeWorkingHour);
                    double perHourAmount = Math.Round(employeeGrossSalary / 208, 2);

                    totalOvertimeAmount = perHourAmount * totalOverTimeHour;

                    //totalOvertimeAmountInText = $"Overtime Details: {String.Format("{0:0.##}", employeeGrossSalary)}(Basic x 2) / ({totalWorkingDays}(Working Days) / {String.Format("{0:0.##}", employeeWorkingHour)}(Working Hours))= {String.Format("{0:0.##}", perHourAmount) }(Hourly Amount) x { String.Format("{0:0.##}", totalOverTimeHour) }(Overtimes)= Tk {String.Format("{0:0.##}", totalOvertimeAmount)}";
                    totalOvertimeAmountInText = $"Overtime Details: {String.Format("{0:0.##}", employeeGrossSalary)}(Basic x 2) / 208 = {String.Format("{0:0.##}", perHourAmount) }(Hourly Amount) x { String.Format("{0:0.##}", totalOverTimeHour) }(Overtimes)= Tk {String.Format("{0:0.##}", totalOvertimeAmount)}";
                }
            }
            catch
            {
                totalOvertimeAmountInText = "N/A";
            }

            return totalOvertimeAmountInText;
        }

        public void MapDropDown(AttAttendanceViewModel model)
        {
            var reportList = new List<SelectListItem>();
            reportList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            reportList.Add(new SelectListItem() { Text = "Daily Attendance By Status Group", Value = "DATT" });
            reportList.Add(new SelectListItem() { Text = "Daily Attendance Without Group", Value = "DATTWG" });
            reportList.Add(new SelectListItem() { Text = "Monthly Attendance", Value = "MATT" });
            reportList.Add(new SelectListItem() { Text = "Employee Code", Value = "EC" });
            reportList.Add(new SelectListItem() { Text = "Daily Present", Value = "PR" });
            reportList.Add(new SelectListItem() { Text = "Daily Absent", Value = "A" });
            reportList.Add(new SelectListItem() { Text = "Daily Late", Value = "LT" });
            reportList.Add(new SelectListItem() { Text = "Manually Daily Updated", Value = "MDU" });
            reportList.Add(new SelectListItem() { Text = "Manually Monthly Updated", Value = "MMU" });
            reportList.Add(new SelectListItem() { Text = "Tour Report", Value = "TR" });
            //reportList.Add(new SelectListItem(){Text = "Manually Updated", Value = "MU"});

            if(SessionHelper.CompanyInfo.CompanyCode == "GTT")
                reportList.Add(new SelectListItem() { Text = "Punch/ZKBioTime Report", Value = "ZKBio" });

            model.ReportTypeList = reportList;


            var MonthList = new List<SelectListItem>();
            MonthList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 1; i <= 12; i++)
            {
                MonthList.Add(new SelectListItem { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Value = i.ToString() });
            }
            model.MonthList = MonthList;

            var officeType = officeTypeService.GetAll().Where(w => w.IsActive == true);
            var viewofficeType = officeType.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeTypeId.ToString(),
                Text = string.Format("{0}", x.OfficeTypeName)
            });
            var officeType_items = new List<SelectListItem>();
            officeType_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            officeType_items.AddRange(viewofficeType);
            model.OfficeTypeList = officeType_items;

            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            model.OfficeList = ofc_items;

            var ZoneList = officeService.GetAll().Where(x => x.OfficeTypeId == 4 && x.IsActive == true);
            var viewZoneList = ZoneList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = x.OfficeName.ToString()
            });
            var zone_items = new List<SelectListItem>();
            zone_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            zone_items.AddRange(viewZoneList);
            model.ZoneList = zone_items;

            var area_items = new List<SelectListItem>();
            area_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            model.AreaList = area_items;

            var unit_items = new List<SelectListItem>();
            unit_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            model.UnitList = unit_items;

            //department           
            var departmentItems = new List<SelectListItem>();
            departmentItems.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });           
            model.DepartmentList = departmentItems;

            //section           
            var sectionItems = new List<SelectListItem>();
            sectionItems.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            model.SectionList = sectionItems;
        }

        public void MapDropdownForEmployeeStatus(AttAttendanceViewModel model)
        {
            long employeeId = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
            var employee = employeeService.GetByEmpId(employeeId);
            var employeeCode = employee.EmployeeCode;
            var officeId = employee.OfficeId;
            model.EmployeeCode = employeeCode;
            model.OfficeId = Convert.ToInt32(officeId);
        }

        #endregion


    }//End of Class
}//End of Namespace