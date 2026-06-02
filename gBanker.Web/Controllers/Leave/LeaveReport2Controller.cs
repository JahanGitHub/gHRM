using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gHRM.Web.ViewModels;
using gHRM.Web.Helpers;
using gHRM.Service.StoreProcedure;
using System.Data;
using gHRM.Service;
using gHRM.Web.Reports;
using gHRM.Web.CommonDropdown;
using System.Text;
using gHRM.Web.Reports.Leave;
using gHRM.Core.Utilities.Constants;
using gHRM.Web.Infrastucture.Utility;
using gHRM.Service.ReportServies;
using Microsoft.Reporting.WebForms;
using gHRM.Data.CodeFirstMigration;

namespace gHRM.Web.Controllers
{
    public class LeaveReport2Controller : BaseController
    {
        private readonly IEmployeeSPService employeeSPService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IOfficeService officeService;
        private readonly IEmployeeService employeeService;
        private readonly ICompanyService companyService;

        public CommonStaticDropDown commonStaticDropDown;
        public CommonDynamicDropDown commonDynamicDropDown;
        public LeaveReport2Controller(
            IEmployeeSPService employeeSPService,
            IOfficeTypeService officeTypeService,
            IOfficeService officeService,
            ICompanyService companyService,
            IEmployeeService employeeService)
        {
            this.employeeSPService = employeeSPService;
            this.officeTypeService = officeTypeService;
            this.officeService = officeService;
            this.employeeService = employeeService;
            this.companyService = companyService;

            commonStaticDropDown = new CommonStaticDropDown();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }
        public void MapDropdownForLeaveReport(LeaveReportViewModel model)
        {
            var reportList = new List<SelectListItem>();
            reportList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            reportList.Add(new SelectListItem() { Text = "ATTENDANCE RECORD FOR SALARY", Value = "LSM" });

            model.ReportTypeList = reportList;

            var yearList = new List<SelectListItem>();
            yearList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            yearList.Add(new SelectListItem() { Text = (DateTime.Today.Year).ToString(), Value = (DateTime.Today.Year).ToString() });
            yearList.Add(new SelectListItem() { Text = ((DateTime.Today.Year) - 1).ToString(), Value = ((DateTime.Today.Year) - 1).ToString() });
            model.YearList = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 1; i <= 12; i++)
            {
                monthList.Add(new SelectListItem { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Value = i.ToString() });
            }
            model.MonthList = monthList;

        }



        public ActionResult Index()
        {
            var model = new LeaveReportViewModel();
            MapDropdownForLeaveReport(model);
            //model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            //model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            //model.AreaList = commonDynamicDropDown.ddlInitial();
            //model.UnitList = commonDynamicDropDown.ddlInitial();


            var officeTypeList = new List<SelectListItem>();
            //  officeTypeList.Add(PleaseSelect);
            officeTypeList.Add(new SelectListItem() { Text = "Head Office", Value = "1" });
            officeTypeList.Add(new SelectListItem() { Text = "Project", Value = "2" });
            officeTypeList.Add(new SelectListItem() { Text = "Field Office", Value = "3" });
            model.OfficeTypeList = officeTypeList;

            var employeeStatusList = commonDynamicDropDown.ddlEmployeeStatusList();
            employeeStatusList.RemoveAll(x => x.Value == "");
            model.EmployeeStatusList = employeeStatusList;

            return View(model);
        }

        public ActionResult LeaveStatementYearlyReportPrint(int Year, int? officeId, int? officeTypeId, string status)
        {
            try
            {
                var DateFrom = new DateTime(Year, 1, 1);
                var DateTo = DateFrom.AddYears(1).AddDays(-1);               

                var param = new { DateFrom = DateFrom, DateTo = DateTo, OfficeId= officeId, OfficeTypeId= officeTypeId, EmployeeStatusArr = status  };
                //var Data = employeeSPService.GetDataWithParameter(param, "leave.SP_LeaveStatementDetail_Test");
                var Data = employeeSPService.GetDataWithParameter(param, "leave.SP_LeaveStatementDetail_Test");
                var reportParam = new Dictionary<string, object>();
                var companyId = SessionHelper.CompanyID;
                var company = companyService.GetAll().Where(p => p.CompanyId == companyId).FirstOrDefault();

                if (company.CompanyCode.Trim() == GHRMPlusCompanyConstants.PidimFoundation)
                {
                    ReportHelper.PrintReport("Leave/rpt_LeaveStatement_For_Pidim.rpt", Data.Tables[0], reportParam);
                    return Content(string.Empty);
                }

                ReportHelper.PrintReport("Leave/rpt_LeaveStatement.rpt", Data.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult LeaveStatementMonthlyReportPrint(string DateFrom, string DateTo, int? officeTypeId)
        {
            try
            {
                // Convert input strings to DateTime
                var fromDate = Convert.ToDateTime(DateFrom);
                var toDate = Convert.ToDateTime(DateTo);

                var paramValues = new List<Service.ReportExecutionService.ParameterValue>
        {
            new Service.ReportExecutionService.ParameterValue { Name = "CompanyName", Value = SessionHelper.CompanyName },
            new Service.ReportExecutionService.ParameterValue { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress },
            new Service.ReportExecutionService.ParameterValue { Name = "DateFrom", Value = fromDate.ToString("yyyy-MM-dd") },
            new Service.ReportExecutionService.ParameterValue { Name = "DateTo", Value = toDate.ToString("yyyy-MM-dd") },
            new Service.ReportExecutionService.ParameterValue { Name = "OfficeTypeId", Value = officeTypeId?.ToString() ?? "0" },
            new Service.ReportExecutionService.ParameterValue { Name = "OfficeId", Value = "0" }
        };

                // Call the SSRS report printer
                PrintSSRSReport("/gHRMPlus_Reports/SalaryAttendanceRecord", paramValues.ToArray());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult LeaveStatementMonthlyReportPrint_excel(string DateFrom, string DateTo, int? officeTypeId)
        {
            try
            {
                // Convert input strings to DateTime
                var fromDate = Convert.ToDateTime(DateFrom);
                var toDate = Convert.ToDateTime(DateTo);

                var paramValues = new List<Service.ReportExecutionService.ParameterValue>
        {
            new Service.ReportExecutionService.ParameterValue { Name = "CompanyName", Value = SessionHelper.CompanyName },
            new Service.ReportExecutionService.ParameterValue { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress },
            new Service.ReportExecutionService.ParameterValue { Name = "DateFrom", Value = fromDate.ToString("yyyy-MM-dd") },
            new Service.ReportExecutionService.ParameterValue { Name = "DateTo", Value = toDate.ToString("yyyy-MM-dd") },
            new Service.ReportExecutionService.ParameterValue { Name = "OfficeTypeId", Value = officeTypeId?.ToString() ?? "0" },
            new Service.ReportExecutionService.ParameterValue { Name = "OfficeId", Value = "0" }
        };

                // Call the SSRS report printer
                PrintSSRSMultiformat("excel", "/gHRMPlus_Reports/SalaryAttendanceRecord", paramValues.ToArray());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        public ActionResult LeaveStatementReportPrintForPidim(int Year, int Month, int? officeId, int? officeTypeId, string leavetype)
        {
            try
            {
                var upto_dt =new DateTime(Year,Month,  DateTime.DaysInMonth(Year, Month));
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "upto_dt", Value = upto_dt });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officeTypeId", Value = officeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officeId", Value = officeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "leavetype", Value = leavetype });
                PrintSSRSReport("/gHRMPlus_Reports/EmployeetypeWiseLeaveStatement", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult EmployeeELEncashmentListReportPrint()
        {
            try
            {

                var reportParam = new Dictionary<string, object>();
                var organizationName = SessionHelper.CompanyCode;

                var canShowMDSignature = (organizationName == GHRMPlusCompanyConstants.GrameenCommunications);
                reportParam.Add("CanShowMDSignature", canShowMDSignature);

                //if grameen communications will be used for 120 custom/forebly used by biplop vai...
                if (SessionHelper.CompanyCode == GHRMPlusCompanyConstants.GrameenCommunications)
                {
                    var Data = employeeSPService.GetDataWithoutParameter("[leave].[SP_rpt_GetELEncashmentListForGC120]");

                    ReportHelper.PrintReport("leave/rpt_EarnLeave_EncashmentListFor120.rpt", Data.Tables[0], reportParam);
                }
                else
                {
                    var Data = employeeSPService.GetDataWithoutParameter("leave.SP_rpt_GetELEncashmentList");

                    ReportHelper.PrintReport("leave/rpt_EarnLeave_EncashmentList.rpt", Data.Tables[0], reportParam);
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult EmployeeWiseLeaveStatement(int Year, string EmployeeCode)
        {
            try
            {
                var DateFrom = new DateTime(Year, 1, 1);
                var DateTo = DateFrom.AddYears(1).AddDays(-1);
                var employeeId = employeeService.GetByCode(EmployeeCode.Trim()).EmployeeId;
                var param = new { EmployeeId = employeeId, DateFrom = DateFrom, DateTo = DateTo };

                var MainReport = employeeSPService.GetDataWithParameter(param, "leave.SP_RPT_EmployeeWiseLeaveStatement");
                var LeaveRecordSummery = employeeSPService.GetDataWithParameter(param, "leave.SP_RPT_GetLeaveStatementSummeryRecord");

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("LeaveRecordSummery", LeaveRecordSummery.Tables[0]);

                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Leave/rpt_EmployeeWiseLeaveStatement.rpt", MainReport.Tables[0], reportParam, subReportDB, new rpt_EmployeeWiseLeaveStatement());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult EmployeeWiseLeave()
        {
            return View();
        }

        public ActionResult EmployeeWiseLeave2()
        {
            return View();
        }

        public ActionResult EmployeeWiseLeave3()
        {
            return View();
        }


        public ActionResult EmployeeWiseLeaveList()
        {
            return View();
        }

        public ActionResult EmployeeWiseLeaveReportPrintALL(string dateFrom, string dateTo, int empid)
        {
            try
            {
                string format = "pdf";
                string type = "view";

                gHRMDBContext db = new gHRMDBContext();
                var employeeCode = db.Employees.Where(z => z.EmployeeId == empid).Select(k => k.EmployeeCode).FirstOrDefault();

                var joinDate = db.Employees.Where(z => z.EmployeeId == empid).Select(k => k.FirstJoiningDate).FirstOrDefault();

                var param = new { @EmployeeCode = employeeCode };
                var employeeInfo = employeeSPService.GetDataWithParameter(param, "rpt_GetEmployeeBasicData");
                var getEmpData = employeeInfo.Tables[0].AsEnumerable().Select(p => new EmployeeViewModel
                {
                    EmployeeName = p.Field<string>("EmployeeName"),
                    DesignationName = p.Field<string>("DesignationName"),
                    DepartmentName = p.Field<string>("DepartmentName"),
                    FirstJoiningDate = p.Field<DateTime>("FirstJoiningDate"),
                    EmployeeCode = p.Field<string>("EmployeeCode"),
                    ConfirmationDate = p.Field<DateTime?>("ConfirmationDate") ?? DateTime.Now,
                });


                var companyInfo = WebHelper.GetCompanyDetails();

                var parameters = new Dictionary<string, object>();
                parameters.Add("DateFrom", joinDate);
                parameters.Add("DateTo", dateTo);
                parameters.Add("EmployeeCode", employeeCode);
                parameters.Add("CompanyName", companyInfo.CompanyName);
                parameters.Add("CompanyAddress", companyInfo.CompanyAddress);
                parameters.Add("CompanyLogo", companyInfo.CompanyLogoURI);

                parameters.Add("EmployeeName", getEmpData.First().EmployeeName);
                parameters.Add("DesignationName", getEmpData.First().DesignationName);
                parameters.Add("DepartmentName", getEmpData.First().DepartmentName);
                parameters.Add("FirstJoiningDate", getEmpData.First().FirstJoiningDate);
                parameters.Add("ConfirmationDate", getEmpData.First().ConfirmationDate);



                string reportTitle = "";
                string reportPath = "";
                string reportViewMode = ReportViewModeConstants.Potrait;

                var param1 = new { @EmployeeCode = employeeCode, @FromDate = dateFrom, @ToDate = dateTo };

                var param2 = new { @EmployeeCode = employeeCode, @FromDate = dateFrom, @ToDate = "14-Jan-2024" };

                var mainDataSource = employeeSPService.GetDataWithParameter(param1, "[leave].[LeaveHistory_GetEmployeeLeaveSummay]");
                var leaveEncashments = employeeSPService.GetDataWithParameter(param2, "[leave].[LeaveHistory_GetEmployeeLeaveEncashment]");

                reportTitle = "Employee Wise Leave Report";
                reportPath = "~/Reports/RDLC/Employee/EmployeeWiseLeave.rdlc";

                var reportDataSources = new List<ReportDataSource>
                {
                    new ReportDataSource{ Name = "EmployeeWiseLeaveDataSet",Value = mainDataSource.Tables[0] },
                    new ReportDataSource{ Name = "EmployeeWiseLeaveEncashmentDataSet",Value = leaveEncashments.Tables[0] }
                };

                parameters.Add("TotalRemainingBalance", mainDataSource.Tables[0].Rows[0]["TotalRemainingBalance"]);
                return Report(reportDataSources, parameters, reportTitle, reportPath, format = "pdf", type = "view", reportViewMode);
            }
            catch (Exception ex)
            {
                // return RedirectToAction("CommonReportGenerationError");
                if (SessionHelper.UserFullName.ToLower().Contains("super"))
                    return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Result = "ERROR", Message = "No Data Found" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult EmployeeWiseLeaveReportPrint(string dateFrom, string dateTo, string employeeCode)
        {
            try
            {
                string format = "pdf";
                string type = "view";

                var param = new { @EmployeeCode = employeeCode };
                var employeeInfo = employeeSPService.GetDataWithParameter(param, "rpt_GetEmployeeBasicData");
                var getEmpData = employeeInfo.Tables[0].AsEnumerable().Select(p => new EmployeeViewModel
                {
                    EmployeeName = p.Field<string>("EmployeeName"),
                    DesignationName = p.Field<string>("DesignationName"),
                    DepartmentName = p.Field<string>("DepartmentName"),
                    FirstJoiningDate = p.Field<DateTime>("FirstJoiningDate"),
                    EmployeeCode = p.Field<string>("EmployeeCode"),
                    ConfirmationDate = p.Field<DateTime?>("ConfirmationDate")?? DateTime.Now,
                });


                var companyInfo = WebHelper.GetCompanyDetails();

                var parameters = new Dictionary<string, object>();
                parameters.Add("DateFrom", dateFrom);
                parameters.Add("DateTo", dateTo);
                parameters.Add("EmployeeCode", employeeCode);
                parameters.Add("CompanyName", companyInfo.CompanyName);
                parameters.Add("CompanyAddress", companyInfo.CompanyAddress);
                parameters.Add("CompanyLogo", companyInfo.CompanyLogoURI);

                parameters.Add("EmployeeName", getEmpData.First().EmployeeName);
                parameters.Add("DesignationName", getEmpData.First().DesignationName);
                parameters.Add("DepartmentName", getEmpData.First().DepartmentName);
                parameters.Add("FirstJoiningDate", getEmpData.First().FirstJoiningDate);
                parameters.Add("ConfirmationDate", getEmpData.First().ConfirmationDate);



                string reportTitle = "";
                string reportPath = "";
                string reportViewMode = ReportViewModeConstants.Potrait;

                var param1 = new { @EmployeeCode = employeeCode, @FromDate = dateFrom, @ToDate = dateTo };

                var mainDataSource = employeeSPService.GetDataWithParameter(param1, "[leave].[LeaveHistory_GetEmployeeLeaveSummay]");
                var leaveEncashments = employeeSPService.GetDataWithParameter(param1, "[leave].[LeaveHistory_GetEmployeeLeaveEncashment]");

                reportTitle = "Employee Wise Leave Report";
                reportPath = "~/Reports/RDLC/Employee/EmployeeWiseLeave.rdlc";

                var reportDataSources = new List<ReportDataSource>
                {
                    new ReportDataSource{ Name = "EmployeeWiseLeaveDataSet",Value = mainDataSource.Tables[0] },
                    new ReportDataSource{ Name = "EmployeeWiseLeaveEncashmentDataSet",Value = leaveEncashments.Tables[0] }
                };

                parameters.Add("TotalRemainingBalance", mainDataSource.Tables[0].Rows[0]["TotalRemainingBalance"]);
                return Report(reportDataSources, parameters, reportTitle, reportPath, format = "pdf", type = "view", reportViewMode);
            }
            catch (Exception ex)
            {
                // return RedirectToAction("CommonReportGenerationError");
                if (SessionHelper.UserFullName.ToLower().Contains("super"))
                    return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Result = "ERROR", Message = "No Data Found" }, JsonRequestBehavior.AllowGet);
            }
        }


          //public ActionResult EmployeeWiseLeaveReportPrint2(string dateFrom, string dateTo, string employeeCode)
        //{
        //    try
        //    {
        //        gHRMDBContext db = new gHRMDBContext();
        //        var empCode = "";
        //        if (employeeCode == "")
        //        {
        //            var empp = db.Employees.Where(z => z.EmployeeId == LoggedInEmployeeId).Select(k => k.EmployeeCode).FirstOrDefault();
        //            empCode = empp;
        //        }
        //        else
        //        {
        //            empCode = employeeCode;
        //        }

        //        var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
        //        paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
        //        paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
        //        paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = (string.IsNullOrEmpty(empCode) ? "0" : empCode) });

        //        paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Employee_Code", Value = (string.IsNullOrEmpty(empCode) ? "0" : empCode) });

        //        paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateFrom", Value = dateFrom });

        //        paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateTo", Value = dateTo });


        //        paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Date_From", Value = dateFrom });

        //        paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Date_To", Value = dateTo });

        //        PrintSSRSReport("/gHRMPlus_Reports/LeaveSummeryAndDetails", paramValues.ToArray());
        //        return Content(string.Empty);


        //    }
        //    catch (Exception ex)
        //    {
        //        // return RedirectToAction("CommonReportGenerationError");
        //        if (SessionHelper.UserFullName.ToLower().Contains("super"))
        //            return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //        else
        //            return Json(new { Result = "ERROR", Message = "No Data Found" }, JsonRequestBehavior.AllowGet);
        //    }
        //}


        public ActionResult EmployeeWiseLeaveReportPrint2(string dateFrom, string dateTo, string employeeCode)
        {
            try
            {
                string empCode;

                using (var db = new gHRMDBContext())
                {
                    if (string.IsNullOrWhiteSpace(employeeCode))
                    {
                        empCode = db.Employees
                                    .Where(z => z.EmployeeId == LoggedInEmployeeId)
                                    .Select(k => k.EmployeeCode)
                                    .FirstOrDefault();
                    }
                    else
                    {
                        empCode = employeeCode;
                    }
                }

                empCode = string.IsNullOrWhiteSpace(empCode) ? "0" : empCode;

                var paramValues = new List<Service.ReportExecutionService.ParameterValue>
        {
            new Service.ReportExecutionService.ParameterValue { Name = "CompanyName", Value = SessionHelper.CompanyName },
            new Service.ReportExecutionService.ParameterValue { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress },
            new Service.ReportExecutionService.ParameterValue { Name = "EmployeeCode", Value = empCode },
            new Service.ReportExecutionService.ParameterValue { Name = "Employee_Code", Value = empCode },
            new Service.ReportExecutionService.ParameterValue { Name = "DateFrom", Value = dateFrom },
            new Service.ReportExecutionService.ParameterValue { Name = "DateTo", Value = dateTo },
            new Service.ReportExecutionService.ParameterValue { Name = "Date_From", Value = dateFrom },
            new Service.ReportExecutionService.ParameterValue { Name = "Date_To", Value = dateTo }
        };

                PrintSSRSReport("/gHRMPlus_Reports/LeaveSummeryAndDetails", paramValues.ToArray());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                var errorMessage = SessionHelper.UserFullName.ToLower().Contains("super")
                    ? ex.Message
                    : "No Data Found";

                return Json(new { Result = "ERROR", Message = errorMessage }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult RegisterOfLeaveAndLeaveBookPrint(string EmployeeCode)
        {
            try
            {
                int TotalEncashment = 0;
                var paramEncashment = new
                {
                    EmployeeCode = EmployeeCode,
                    FromDate = DateTime.Now.AddYears(-200).Date,
                    ToDate = DateTime.Now.Date
                };
                var subReportEncashment = employeeSPService.GetDataWithParameter(paramEncashment, "leave.LeaveHistory_GetEmployeeLeaveEncashment");
                try
                {
                    TotalEncashment = subReportEncashment.Tables[0].AsEnumerable().Sum(x => Convert.ToInt32(x["TotalDays"]));
                }
                catch { }
                var param = new { EmployeeCode = EmployeeCode, TotalEncashment = TotalEncashment };
                var mainReport = employeeSPService.GetDataWithParameter(param, "leave.SP_RegisterOfLeaveAndLeaveBookPrint");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("Encashment", subReportEncashment.Tables[0]);

                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Leave/Rpt_GetLeaveDetails.rpt", mainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        // Half Day Leave Report

        public ActionResult HalfDayLeaveEmployeeWise(int Year,
            string OfficeTypeId, string OfficeId, string DesignationId, string ResponsibilityId, string DeptId, string SectionId, string Status, string EmployeeCode)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = (string.IsNullOrEmpty(EmployeeCode) ? "0" : EmployeeCode) });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "LeaveYear", Value = Year.ToString() });

                
                PrintSSRSReport("/gHRMPlus_Reports/EmployeeHalfDayLeaveReport", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

    }
}