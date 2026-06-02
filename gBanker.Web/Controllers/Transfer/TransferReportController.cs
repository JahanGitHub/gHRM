using System.Data;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using gHRM.Web.ViewModels;
using gHRM.Data.CodeFirstMigration;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Text;
using gHRM.Web.DropDownService;
using gHRM.Web.CommonDropdown;

namespace gHRM.Web.Controllers
{

    public class TransferReportController : BaseController
    {

        #region Variables
        private readonly IEmployeeSPService employeeSpService;
        private readonly IOfficeService officeService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IEmployeeStatusService employeeStatusService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeDesignationService employeeDesignationService;
        private readonly IEmployementTypeService employementTypeService;
        private readonly ITransferOfficeOrderrService transferOfficeOrderrService;
        private readonly IEmployeeTransferService employeeTransferService;
        private readonly ICompanyService companyService;

        //private readonly IEmployeeReportOptionService employeeReportOptionService;
        private readonly CommonReportOptions commonReportOptions;
        public CommonDynamicDropDown commonDynamicDropDown;


        public TransferReportController(
            IEmployeeSPService employeeSpService,
            IOfficeService officeService,
            IOfficeTypeService officeTypeService,
            IEmployeeStatusService employeeStatusService,
            IEmployeeDepartmentService employeeDepartmentService,
            IEmployeeService employeeService,
            //IEmployeeReportOptionService employeeReportOptionService,
            IEmployeeDesignationService employeeDesignationService,
            IEmployementTypeService employementTypeService,
            ITransferOfficeOrderrService transferOfficeOrderrService,
            IEmployeeTransferService employeeTransferService,
            ICompanyService companyService)
        {
            this.employeeSpService = employeeSpService;
            this.officeService = officeService;
            this.officeTypeService = officeTypeService;
            this.employeeStatusService = employeeStatusService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.employeeService = employeeService;
            this.employeeDesignationService = employeeDesignationService;
            this.employementTypeService = employementTypeService;
            this.transferOfficeOrderrService = transferOfficeOrderrService;
            this.employeeTransferService = employeeTransferService;
            this.companyService = companyService;
            //this.employeeReportOptionService = employeeReportOptionService;
            commonReportOptions = new CommonReportOptions();
            commonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion

        #region Events

        public ActionResult TransferOfficeOrder()
        {
            return View();
        }

        public ActionResult TransferOfficeOrderAddin()
        {
            var model = new TransferOfficeOrderViewModel();

            var employeeProfilelist = new List<SelectListItem>();
            employeeProfilelist.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Header", Value = "Header" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Middle", Value = "Middle" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Footer", Value = "Footer" });
            model.ReportPlacementList = employeeProfilelist;


            return View(model);
        }

        public ActionResult TransferReportDateWiseForStaf( string Date, string DateTo, bool DownloadExcel, string EmployeeCode)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = "1,2,3,4,5,6,7,8,9,10,11" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = Date.ToString() });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "ToDate", Value = DateTo.ToString() });

                //  paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = EmployeeCode.ToString() });

                PrintSSRSReport("/gHRMPlus_Reports/TransferReportForDateToDate", paramValues.ToArray());
                return Content(string.Empty);


            }
            catch (Exception ex)
            {
                return Content("<b>error</b><br />" + ex.Message);
                // return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult TransferReportEmpCodeForStaf( bool DownloadExcel, string EmployeeCode)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = "0"});
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = "1,2,3,4,5,6,7,8,9,10,11" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = "2022-11-22" });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "ToDate", Value = "2022-11-22" });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = EmployeeCode.ToString() });

                PrintSSRSReport("/gHRMPlus_Reports/TransferReportForEmpCode", paramValues.ToArray());
                return Content(string.Empty);


            }
            catch (Exception ex)
            {
                return Content("<b>error</b><br />" + ex.Message);
                // return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        public ActionResult TransferOfficeOrderReport()
        {
            var model = new TransferReportViewModel();
            MapDropdownForReport(model);
            employeeSpService.GetDataWithoutParameter("trns.SetTransferOfficeOrderView");
            ViewBag.CompanyCode = SessionHelper.CompanyCode;
            ViewBag.TRANSFER_OFFICE_ORDER_REPORT_SHOW_BTN_COMPANYREPORT = GetSetting("TRANSFER_OFFICE_ORDER_REPORT_SHOW_BTN_COMPANYREPORT") == "true";
            return View(model);
        }

        public ActionResult TransferOfficeOrderReport_Addin()
        {
            var model = new TransferReportViewModel();
            MapDropdownForReport(model);
            employeeSpService.GetDataWithoutParameter("trns.SetTransferOfficeOrderView");
            ViewBag.CompanyCode = SessionHelper.CompanyCode;
            ViewBag.TRANSFER_OFFICE_ORDER_REPORT_SHOW_BTN_COMPANYREPORT = GetSetting("TRANSFER_OFFICE_ORDER_REPORT_SHOW_BTN_COMPANYREPORT") == "true";
            return View(model);
        }
        public ActionResult ReportForTransfer()
        {
            var model = new TransferReportViewModel();
            MapDropdownForReport(model);
            employeeSpService.GetDataWithoutParameter("trns.SetTransferOfficeOrderView");
            ViewBag.CompanyCode = SessionHelper.CompanyCode;
            ViewBag.TRANSFER_OFFICE_ORDER_REPORT_SHOW_BTN_COMPANYREPORT = GetSetting("TRANSFER_OFFICE_ORDER_REPORT_SHOW_BTN_COMPANYREPORT") == "true";
            return View(model);
        }




        public ActionResult TransferHistoryReport()
        {
            var model = new TransferReportViewModel();
            MapDropdownForReport(model);
            return View(model);
        }

        public ActionResult TransferIndicatorReport()
        {
            var model = new TransferReportViewModel();
            MapDropdownForReport(model);
            ViewBag.CompanyCode = SessionHelper.CompanyCode;
            ViewBag.EMPLOYEE_TRANSFER_INDICATOR_REPORT_SHOW_BTN_COMPANYREPORT = GetSetting("EMPLOYEE_TRANSFER_INDICATOR_REPORT_SHOW_BTN_COMPANYREPORT") == "true";
            return View(model);
        }

        public ActionResult TransferIndicatorReportDemo()
        {
            var model = new TransferReportViewModel();
            MapDropdownForReportDemo(model);
            return View(model);
        }

        public ActionResult TransferEmployeeOfficeOrder(string DateFrom, string DateTo, int OrderNo, string IdLists)
        {
            try
            {

                var param = new { DateFrom = DateFrom, DateTo = DateTo, OrderNo = OrderNo };
                var paramSub = new { IdLists = IdLists };
                var mainReport = employeeSpService.GetDataWithParameter(param, "trns.SP_Rpt_TransferEmployeeOfficeOrder");
                var subReport = employeeSpService.GetDataWithParameter(paramSub, "trns.SP_RPT_TransferEmployeeOfficeOrder_Sub");
              

                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("rpt_TransferEmployeeOfficeOrder_Sub", subReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                if(SessionHelper.CompanyInfo.CompanyShortName == "addin")
                {
                    var mdSignatury = employeeSpService.GetDataWithoutParameter("MdSignatory");
                    var paramSub_addin1 = new { IdLists = IdLists, ReportPlacementType = "Header" };
                    var paramSub_addin2 = new { IdLists = IdLists, ReportPlacementType = "Middle" };
                    var paramSub_addin3 = new { IdLists = IdLists, ReportPlacementType = "Footer" };
                    var subReport_addin_header = employeeSpService.GetDataWithParameter(paramSub_addin1, "trns.SP_RPT_TransferEmployeeOfficeOrder_Sub_Addin");
                    var subReport_addin_middle = employeeSpService.GetDataWithParameter(paramSub_addin2, "trns.SP_RPT_TransferEmployeeOfficeOrder_Sub_Addin");
                    var subReport_addin_footer = employeeSpService.GetDataWithParameter(paramSub_addin3, "trns.SP_RPT_TransferEmployeeOfficeOrder_Sub_Addin");

                    var subReportDbAddin = new Dictionary<string, DataTable>();
                    subReportDbAddin.Add("rpt_TransferEmployeeOfficeOrder_Sub_Middle", subReport_addin_middle.Tables[0]);
                    subReportDbAddin.Add("rpt_TransferEmployeeOfficeOrder_Sub", subReport_addin_footer.Tables[0]);
                    subReportDbAddin.Add("rpt_MdSignatory", mdSignatury.Tables[0]);

                    ReportHelper.PrintWithSubReport("Transfer/rpt_TransferEmployeeOfficeOrder_Adddin.rpt", mainReport.Tables[0], reportParam, subReportDbAddin);
                } 
                else
                ReportHelper.PrintWithSubReport("Transfer/rpt_TransferEmployeeOfficeOrder.rpt", mainReport.Tables[0], reportParam, subReportDb);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult TransferEmployeeOfficeOrderDoc(string DateFrom, string DateTo, int OrderNo, string IdLists)
        {
            try
            {

                var param = new { DateFrom = DateFrom, DateTo = DateTo, OrderNo = OrderNo };
                var paramSub = new { IdLists = IdLists };
                var mainReport = employeeSpService.GetDataWithParameter(param, "trns.SP_Rpt_TransferEmployeeOfficeOrder");
                var subReport = employeeSpService.GetDataWithParameter(paramSub, "trns.SP_RPT_TransferEmployeeOfficeOrder_Sub");

                

                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("rpt_TransferEmployeeOfficeOrder_Sub", subReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                if (SessionHelper.CompanyInfo.CompanyShortName == "addin")
                {
                    var mdSignatury = employeeSpService.GetDataWithoutParameter("MdSignatory");
                    var paramSub_addin1 = new { IdLists = IdLists, ReportPlacementType = "Header" };
                    var paramSub_addin2 = new { IdLists = IdLists, ReportPlacementType = "Middle" };
                    var paramSub_addin3 = new { IdLists = IdLists, ReportPlacementType = "Footer" };
                    var subReport_addin_header = employeeSpService.GetDataWithParameter(paramSub_addin1, "trns.SP_RPT_TransferEmployeeOfficeOrder_Sub_Addin");
                    var subReport_addin_middle = employeeSpService.GetDataWithParameter(paramSub_addin2, "trns.SP_RPT_TransferEmployeeOfficeOrder_Sub_Addin");
                    var subReport_addin_footer = employeeSpService.GetDataWithParameter(paramSub_addin3, "trns.SP_RPT_TransferEmployeeOfficeOrder_Sub_Addin");

                    var subReportDbAddin = new Dictionary<string, DataTable>();
                    subReportDbAddin.Add("rpt_TransferEmployeeOfficeOrder_Sub_Middle", subReport_addin_middle.Tables[0]);
                    subReportDbAddin.Add("rpt_TransferEmployeeOfficeOrder_Sub", subReport_addin_footer.Tables[0]);
                    subReportDbAddin.Add("rpt_MdSignatory", mdSignatury.Tables[0]);

                    ReportHelper.PrintWithSubReportDoc("Transfer/rpt_TransferEmployeeOfficeOrder_Adddin.rpt", mainReport.Tables[0], reportParam, subReportDbAddin);
                }
                else
                    ReportHelper.PrintWithSubReportDoc("Transfer/rpt_TransferEmployeeOfficeOrder.rpt", mainReport.Tables[0], reportParam, subReportDb);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult EmployeeHistory(string EmployeeCode)
        {
            try
            {
                var empInfo = employeeService.GetByCode(EmployeeCode);
                if (empInfo !=null)
                {
                    var EmployeeId = empInfo.EmployeeId;

                    var param = new { EmployeeCode = EmployeeCode };
                    var paramSub = new { EmployeeCode = EmployeeCode };
                    var paramSubTransfer = new { EmployeeId = EmployeeId };
                    var mainReport = employeeSpService.GetDataWithParameter(param, "trns.SP_Rpt_TransferHistory");
                    var subReport = employeeSpService.GetDataWithParameter(paramSub, "trns.SP_RPT_TransferDesignationHistory_Sub");
                    var subReport1 = employeeSpService.GetDataWithParameter(paramSub, "trns.SP_RPT_TransferResponsibilityHistory_Sub");
                    var subReport2 = employeeSpService.GetDataWithParameter(paramSubTransfer, "trns.SP_GetPreviousOfficeDatabyId");
                    var subReportDb = new Dictionary<string, DataTable>();
                    subReportDb.Add("rpt_TransferDesignationHistory_Sub", subReport.Tables[0]);
                    subReportDb.Add("rpt_TransferResponsibilityHistory_Sub", subReport1.Tables[0]);
                    subReportDb.Add("rpt_TransferHistoryRecord_Sub", subReport2.Tables[0]);
                    var reportParam = new Dictionary<string, object>();
                    ReportHelper.PrintWithSubReport("Transfer/rpt_TransferHistory.rpt", mainReport.Tables[0], reportParam, subReportDb);
                    return Content(string.Empty);
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "No employee found"});
                }
               
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult EmployeeHistoryJCF(string EmployeeCode)
        {
            try
            {
                var empInfo = employeeService.GetByCode(EmployeeCode);
                if (empInfo != null)
                {
                    var EmployeeId = empInfo.EmployeeId;

                    var param = new { EmployeeCode = EmployeeCode };
                    var paramSub = new { EmployeeCode = EmployeeCode };
                    var paramSubTransfer = new { EmployeeId = EmployeeId };
                    var mainReport = employeeSpService.GetDataWithParameter(param, "trns.SP_Rpt_TransferHistory");
                    var subReport = employeeSpService.GetDataWithParameter(paramSub, "trns.SP_RPT_TransferDesignationHistory_Sub");
                    var subReport1 = employeeSpService.GetDataWithParameter(paramSub, "trns.SP_RPT_TransferResponsibilityHistoryJCF_Sub");
                    var subReport2 = employeeSpService.GetDataWithParameter(paramSubTransfer, "trns.SP_GetPreviousOfficeDatabyId");
                    var subReportDb = new Dictionary<string, DataTable>();
                    subReportDb.Add("rpt_TransferDesignationHistory_Sub", subReport.Tables[0]);
                    subReportDb.Add("rpt_TransferResponsibilityHistory_Sub", subReport1.Tables[0]);
                    subReportDb.Add("rpt_TransferHistoryRecord_Sub", subReport2.Tables[0]);
                    var reportParam = new Dictionary<string, object>();
                    ReportHelper.PrintWithSubReport("Transfer/rpt_TransferHistoryJCF.rpt", mainReport.Tables[0], reportParam, subReportDb);
                    return Content(string.Empty);
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "No employee found" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        public ActionResult EmployeeTransferIndicatorReport(
            string DateFrom,
            string DateTo,
            string status,
            string empType,
            string officeId,
            string OfficeTypeId,
            string officeLevelId,
            string DepartmentId,
            string DesignationId,
            string SectionId,
            string Age,
            string AgeStatus,
            string ResponsibilityId,
            string AgeOffice,
            string AgeStatusOffice)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sb_duration = new StringBuilder();

                
                if (!String.IsNullOrEmpty(officeId) && officeId != "0")
                {
                    sb.Append(" AND vw.OfficeId=" + Convert.ToInt32(officeId));
                }

                // Office type Selected but no office selected i.e all office of this type
                if (!String.IsNullOrEmpty(OfficeTypeId) && (officeId == "0" || String.IsNullOrEmpty(officeId)))
                {
                    int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                    if ( officeLevelId == "0" || String.IsNullOrEmpty(officeLevelId))
                    {
                        sb.Append(" AND vw.OfficeTypeId=" + _OfficeTypeId);
                    }
                    else
                    {
                        var office = officeService.GetById(Convert.ToInt32(officeLevelId));
                        if (office != null && office.OfficeTypeId == 4) //Zone Office
                        {
                            sb.Append(" AND vwom.SecondLevel='" + office.OfficeCode + "'");
                        }

                        if (office != null && office.OfficeTypeId == 5) //Area Office
                        {
                            sb.Append(" AND vwom.ThirdLevel='" + office.OfficeCode + "'");
                        }
                    }
                }

                if (!String.IsNullOrEmpty(DesignationId))
                {
                    int _DesignationId = Convert.ToInt32(DesignationId);
                    sb.Append(" AND vw.DesignationId=" + _DesignationId);
                }
                if (!String.IsNullOrEmpty(ResponsibilityId))
                {
                    string _ResponsibilityId = ResponsibilityId;
                    sb.Append(" and vw.EmployeeRank='" + _ResponsibilityId + "'");
                }
                if (!String.IsNullOrEmpty(DepartmentId))
                {
                    int _DepartmentId = Convert.ToInt32(DepartmentId);
                    sb.Append(" AND vw.DepartmentId=" + _DepartmentId);
                }
                if (!String.IsNullOrEmpty(SectionId))
                {
                    int _SectionId = Convert.ToInt32(SectionId);
                    sb.Append(" AND vw.SectionId=" + _SectionId);
                }
                if (!String.IsNullOrEmpty(Age))
                {
                    if (AgeStatus == "A")
                    {
                        AgeStatus = ">=";//>
                    }
                    else if (AgeStatus == "B")
                    {
                        AgeStatus = "<";
                    }
                    else if (AgeStatus == "E")
                    {
                        AgeStatus = "=";
                    }
                    sb.Append(" And (SELECT [Year] FROM [dbo].[GetDateDetails](vw.FirstJoiningDate, GETDATE())) " + AgeStatus + Convert.ToInt32(Age));
                }
        
                if (!String.IsNullOrEmpty(DateFrom) && !String.IsNullOrEmpty(DateTo))
                {
                    sb.Append(" AND vw.FirstJoiningDate between'" + DateFrom + "' AND '" + DateTo + "'");
                }
                if (!String.IsNullOrEmpty(status))
                {
                    int _status = Convert.ToInt32(status);
                    sb.Append(" AND vw.EmployeeStatusId=" + _status);
                }
                if (!String.IsNullOrEmpty(empType))
                {
                    int _empType = Convert.ToInt32(empType);
                    sb.Append(" AND vw.EmployeeTypeId=" + _empType);
                }

                if (!String.IsNullOrEmpty(AgeOffice))
                {
                    if (AgeStatusOffice == "A")
                    {
                        AgeStatusOffice = ">=";//>
                    }
                    else if (AgeStatusOffice == "B")
                    {
                        AgeStatusOffice = "<";
                    }
                    else if (AgeStatusOffice == "E")
                    {
                        AgeStatusOffice = "=";
                    }
                    sb_duration.Append(" And (SELECT [Year] FROM [dbo].[GetDateDetails](trns.LastJoiningDate, GETDATE())) " + AgeStatusOffice + Convert.ToInt32(AgeOffice));
                }

                var param = new { AndCondition = sb.ToString(), DurationCondition = sb_duration.ToString() };
                var mainReport = employeeSpService.GetDataWithParameter(param, "trns.SP_RPT_EmployeeTransferIndicatorReportNew");
                var reportParam = new Dictionary<string, object>();
                //reportParam.Add("DateFrom", DateFrom);
                //reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Transfer/rpt_EmployeeTransferIndicatorReport.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult EmployeeTransferIndicatorReportDemo(
            string DateFrom,
            string DateTo,
            string status,
            string empType,
            string officeId,
            string OfficeTypeId,
            string DepartmentId,
            string DesignationId,
            string SectionId,
            string Age,
            string AgeStatus,
            string ResponsibilityId,
            string AgeOffice,
            string AgeStatusOffice)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sb_duration = new StringBuilder();

                if (!String.IsNullOrEmpty(OfficeTypeId))
                {

                    if (officeId == "1000")
                    {
                        sb.Append(" AND vw.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.OfficeTypeId "+"between " +OfficeTypeId + " And " +6+ ")");
                    }
                    else if (officeId == "2000")
                    {
                        sb.Append(" AND vw.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.OfficeTypeId " + "between " + OfficeTypeId + " And " + 6 + ")");
                    }
                    else if (officeId == "3000")
                    {
                        sb.Append(" AND vw.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.OfficeTypeId " + "between " + OfficeTypeId + " And " + 6 + ")");
                    }
                    else
                    {
                        int _OfficeTypeId = Convert.ToInt32(OfficeTypeId);
                        sb.Append(" AND vw.OfficeTypeId=" + _OfficeTypeId);
                    }

                }
                if (!String.IsNullOrEmpty(officeId) && officeId != "0")
                {
                    if(officeId == "1000")
                    {

                    }
                    else if (officeId == "2000")
                    {

                    }
                    else if (officeId == "3000")
                    {

                    }
                    else
                    {
                        int _officeId = Convert.ToInt32(officeId);
                        sb.Append(" AND vw.OfficeId=" + _officeId);
                    }
                }
                if (!String.IsNullOrEmpty(DesignationId))
                {
                    int _DesignationId = Convert.ToInt32(DesignationId);
                    sb.Append(" AND vw.DesignationId=" + _DesignationId);
                }
                if (!String.IsNullOrEmpty(ResponsibilityId))
                {
                    string _ResponsibilityId = ResponsibilityId;
                    sb.Append(" and vw.EmployeeRank='" + _ResponsibilityId + "'");
                }
                if (!String.IsNullOrEmpty(DepartmentId))
                {
                    int _DepartmentId = Convert.ToInt32(DepartmentId);
                    sb.Append(" AND vw.DepartmentId=" + _DepartmentId);
                }
                if (!String.IsNullOrEmpty(SectionId))
                {
                    int _SectionId = Convert.ToInt32(SectionId);
                    sb.Append(" AND vw.SectionId=" + _SectionId);
                }
                if (!String.IsNullOrEmpty(Age))
                {
                    if (AgeStatus == "A")
                    {
                        AgeStatus = ">";
                    }
                    else if (AgeStatus == "B")
                    {
                        AgeStatus = "<";
                    }
                    else if (AgeStatus == "E")
                    {
                        AgeStatus = "=";
                    }
                    sb.Append(" And DATEDIFF(year, vw.FirstJoiningDate, GETDATE())" + AgeStatus + Convert.ToInt32(Age));
                }

                if (!String.IsNullOrEmpty(DateFrom) && !String.IsNullOrEmpty(DateTo))
                {
                    sb.Append(" AND vw.FirstJoiningDate between'" + DateFrom + "' AND '" + DateTo + "'");
                }
                if (!String.IsNullOrEmpty(status))
                {
                    int _status = Convert.ToInt32(status);
                    sb.Append(" AND vw.EmployeeStatusId=" + _status);
                }
                if (!String.IsNullOrEmpty(empType))
                {
                    int _empType = Convert.ToInt32(empType);
                    sb.Append(" AND vw.EmployeeTypeId=" + _empType);
                }

                if (!String.IsNullOrEmpty(AgeOffice))
                {
                    if (AgeStatusOffice == "A")
                    {
                        AgeStatusOffice = ">";
                    }
                    else if (AgeStatusOffice == "B")
                    {
                        AgeStatusOffice = "<";
                    }
                    else if (AgeStatusOffice == "E")
                    {
                        AgeStatusOffice = "=";
                    }
                    sb_duration.Append(" And DATEDIFF(year, trns.LastJoiningDate, GETDATE())" + AgeStatusOffice + Convert.ToInt32(AgeOffice));
                }

                var param = new { AndCondition = sb.ToString(), DurationCondition = sb_duration.ToString() };
                var mainReport = employeeSpService.GetDataWithParameter(param, "trns.SP_RPT_EmployeeTransferIndicatorReportDemo");
                var reportParam = new Dictionary<string, object>();
                //reportParam.Add("DateFrom", DateFrom);
                //reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Transfer/rpt_EmployeeTransferIndicatorReportDemo.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetTransferPlanningProposalReport(int OrderNo)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                string CompanyWebsiteUrl = "";
                string CompanyName = companyService.GetCompanyNameOtherAndWebsite(out CompanyWebsiteUrl);
                if (null == CompanyWebsiteUrl) CompanyWebsiteUrl = ".....";
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyWebsiteUrl", Value = CompanyWebsiteUrl });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyImage", Value = Request.Url.GetLeftPart(UriPartial.Authority) + SessionHelper.CompanyImage });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OrderNo", Value = OrderNo.ToString() });
                if (SessionHelper.CompanyInfo.CompanyShortName == "addin")
                    PrintSSRSReport("/gHRMPlus_Reports/TransferPlanningProposal_Addin", paramValues.ToArray());
                else
                    PrintSSRSReport("/gHRMPlus_Reports/TransferPlanningProposal", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult GetTransferPlanningOfficeOrderReport(int OrderNo)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OrderNo", Value = OrderNo.ToString() });
                if(SessionHelper.CompanyInfo.CompanyShortName == "addin")
                PrintSSRSReport("/gHRMPlus_Reports/AdddinTransferPlanningOfficeOrder", paramValues.ToArray());
                else
                PrintSSRSReport("/gHRMPlus_Reports/TransferPlanningOfficeOrder", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        public ActionResult GetTransferPlanningProposalReportDoc(int OrderNo)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                string CompanyWebsiteUrl = "";
                string CompanyName = companyService.GetCompanyNameOtherAndWebsite(out CompanyWebsiteUrl);
                if (null == CompanyWebsiteUrl) CompanyWebsiteUrl = ".....";
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyWebsiteUrl", Value = CompanyWebsiteUrl });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyImage", Value = Request.Url.GetLeftPart(UriPartial.Authority) + SessionHelper.CompanyImage });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OrderNo", Value = OrderNo.ToString() });
                if (SessionHelper.CompanyInfo.CompanyShortName == "addin")
                    PrintSSRSMultiformat( "word", "/gHRMPlus_Reports/TransferPlanningProposal_Addin", paramValues.ToArray());
                else
                    PrintSSRSMultiformat("word", "/gHRMPlus_Reports/TransferPlanningProposal", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult GetTransferPlanningOfficeOrderReportDoc(int OrderNo)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OrderNo", Value = OrderNo.ToString() });
                if (SessionHelper.CompanyInfo.CompanyShortName == "addin")
                    PrintSSRSMultiformat("word", "/gHRMPlus_Reports/AdddinTransferPlanningOfficeOrder", paramValues.ToArray());
                else
                    PrintSSRSMultiformat("word", "/gHRMPlus_Reports/TransferPlanningOfficeOrder", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult CompanyReport_EmployeeTransferIndicatorReport(
            string DateFrom,
            string DateTo,
            string status,
            string empType,
            string officeId,
            string OfficeTypeId,
            string officeLevelId,
            string DepartmentId,
            string DesignationId,
            string SectionId,
            string Age,
            string AgeStatus,
            string ResponsibilityId,
            string AgeOffice,
            string AgeStatusOffice)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateFrom", Value = DateFrom });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateTo", Value = DateTo });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "status", Value = status });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "empType", Value = empType });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officeId", Value = officeId });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officeLevelId", Value = officeLevelId });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DepartmentId });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Age", Value = Age });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AgeStatus", Value = AgeStatus });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "ResponsibilityId", Value = ResponsibilityId });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AgeOffice", Value = AgeOffice });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AgeStatusOffice", Value = AgeStatusOffice });
                PrintSSRSReport("/gHRMPlus_Reports/EmployeeTransferIndicator", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        #endregion


        #region TransferOfficeOrder


        public ActionResult TransferOfficeOrderList([DataSourceRequest]DataSourceRequest request)
        {

            var EmployeeStatusList = transferOfficeOrderrService.GetMany(p => p.IsActive == true).ToList();
            var viewList = EmployeeStatusList.AsEnumerable().Select((p, sl) => new TransferOfficeOrderViewModel()
            {
                CCForOfficeOrderId = p.CCForOfficeOrderId,
                CCForOfficeOrderName = p.CCForOfficeOrderName,
                CCForOfficeOrderNameView = p.CCForOfficeOrderNameView,
                ViewOrder = p.ViewOrder,
                ReportPlacementType = p.ReportPlacementType,
            }).ToList();

            DataSourceResult result = viewList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveTransferOfficeOrder(TransferOfficeOrderViewModel ccForOfficeOrder)
        {
            int result = 0;
            string message= string.Empty;

            try
            {
                var isDuplicate = transferOfficeOrderrService.GetMany(p => p.IsActive == true &&
                                  p.CCForOfficeOrderName.ToUpper().Trim() == ccForOfficeOrder.CCForOfficeOrderName.ToUpper().Trim()).ToList();

                if (isDuplicate.Any())
                {
                    message = "Duplicate Status Name found, Save denied";
                }
                else
                {
                    var entity = new TransferOfficeOrder();
                    entity.CCForOfficeOrderName = ccForOfficeOrder.CCForOfficeOrderName;
                    entity.CCForOfficeOrderNameView = ccForOfficeOrder.CCForOfficeOrderName;
                    entity.ViewOrder = ccForOfficeOrder.ViewOrder;
                    entity.IsActive = true;
                    entity.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    entity.ReportPlacementType = ccForOfficeOrder.ReportPlacementType;
                    transferOfficeOrderrService.Create(entity);
                    result = 1;
                    message = "Save Successfull";
                }
            }

            catch (Exception ex)
            {
                message = "Save Denied";
            }

            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateTransferOfficeOrder(TransferOfficeOrderViewModel ccForOfficeOrder)
        {
            int result = 0;
            string message = string.Empty;

            try
            {
                var isDuplicateList = transferOfficeOrderrService.GetMany(p => p.IsActive == true && p.CCForOfficeOrderId != ccForOfficeOrder.CCForOfficeOrderId).ToList();
                var isDuplicate = new TransferOfficeOrder();

                if (!string.IsNullOrEmpty(ccForOfficeOrder.CCForOfficeOrderName))
                {
                    isDuplicate = isDuplicateList.Where(p => p.CCForOfficeOrderName == ccForOfficeOrder.CCForOfficeOrderName).FirstOrDefault();
                }
                if (!string.IsNullOrEmpty(ccForOfficeOrder.CCForOfficeOrderNameView))
                {
                    isDuplicate = isDuplicateList.Where(p => p.CCForOfficeOrderNameView == ccForOfficeOrder.CCForOfficeOrderNameView).FirstOrDefault();
                }

                if (isDuplicate!=null)
                {
                    message = "Duplicate Status Name found, Save denied";
                }
                else
                {
                    var entity = transferOfficeOrderrService.GetById(ccForOfficeOrder.CCForOfficeOrderId);
                    entity.CCForOfficeOrderId = ccForOfficeOrder.CCForOfficeOrderId;
                    if (!string.IsNullOrEmpty(ccForOfficeOrder.CCForOfficeOrderName))
                    {
                        entity.CCForOfficeOrderName = ccForOfficeOrder.CCForOfficeOrderName;
                    }
                    if (!string.IsNullOrEmpty(ccForOfficeOrder.CCForOfficeOrderNameView))
                    {
                        entity.CCForOfficeOrderNameView = ccForOfficeOrder.CCForOfficeOrderNameView;
                    }
                    entity.ViewOrder = ccForOfficeOrder.ViewOrder;
                    entity.IsActive = true;
                    entity.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    entity.ReportPlacementType = ccForOfficeOrder.ReportPlacementType;
                    transferOfficeOrderrService.Update(entity);
                    message = "Update Successfully";
                    result = 1;
                }
            }

            catch (Exception ex)
            {
                message = "Update Denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteTranferOfficeOrder(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = transferOfficeOrderrService.GetById(Id);
                model.IsActive = false;
                model.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                transferOfficeOrderrService.Update(model);
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

        public void MapDropdownForReport(TransferReportViewModel model)
        {
            model.ReportList = commonReportOptions.GetTransferReportOptions();
            var Report = new List<SelectListItem>();
            Report.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            Report.Add(new SelectListItem() { Text = "Employee Transfer Indicator", Value = "1" });
            model.ReportListStatic = Report;

            var officeType = officeTypeService.GetMany(w => w.IsActive == true); ;
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
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            model.OfficeList = ofc_items;


            var ZoneList = officeService.GetMany(x => x.OfficeTypeId == 4 && x.IsActive == true);
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

            var empStatus = new List<SelectListItem>();
            empStatus.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            var statusList = employeeStatusService.GetMany(x => x.IsActive == true).OrderBy(p => p.ViewOrder);
            var getEmpStatus = statusList.AsEnumerable().Select(row => new SelectListItem
            {
                Text = row.StatusName,
                Value = row.StatusValue

            }).ToList();
            empStatus.AddRange(getEmpStatus);
            model.EmployeeStatusList = empStatus;

            var dept = employeeDepartmentService.GetMany(p => p.IsActive == true);
            var viewDept = dept.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.DepartmentName,
                Value = p.DepartmentId.ToString()
            }).ToList();
            var deptList = new List<SelectListItem>();
            deptList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            deptList.AddRange(viewDept);
            model.DepartmentList = deptList;

            var departmentList = employeeDepartmentService.GetAll();
            var viewDepartmentList = departmentList.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.DepartmentCode, m.DepartmentName), Value = m.DepartmentId.ToString() });
            var dep_items = new List<SelectListItem>();
            dep_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            dep_items.AddRange(viewDepartmentList);
            model.DepartmentList = dep_items;


            var designationList = employeeDesignationService.GetAll();
            var viewDesignationList = designationList.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.DesignationCode, m.DesignationName), Value = m.DesignationId.ToString() });
            var desig_items = new List<SelectListItem>();
            desig_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            desig_items.AddRange(viewDesignationList);
            model.DesignationList = desig_items;

            var activeInactiveList = new List<SelectListItem>();
            activeInactiveList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            activeInactiveList.Add(new SelectListItem() { Text = "Active", Value = "1" });
            activeInactiveList.Add(new SelectListItem() { Text = "Inactive", Value = "2" });
            model.ActiveInactiveList = activeInactiveList;

            var sectionList = new List<SelectListItem>();
            sectionList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            model.SectionList = sectionList;

            var designationLists = employeeDesignationService.GetAll();
            var viewDesignationLists = designationLists.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.DesignationCode, m.DesignationName), Value = m.DesignationId.ToString() });
            var desig_item = new List<SelectListItem>();
            desig_item.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            desig_item.AddRange(viewDesignationLists);
            model.DesignationList = desig_item;
            model.ResponsibilityList = commonDynamicDropDown.GetAllOfficeDesignationList();

            var empType = employementTypeService.GetMany(p => p.IsActive == true).OrderBy(p => p.ViewOrder).ToList();
            var viewEmpType = empType.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.EmployementTypeName,
                Value = p.EmployementTypeId.ToString()
            }).ToList();
            var typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            typeList.AddRange(viewEmpType);
            model.EmploymentTypeList = typeList;

            var employeeProfilelist = new List<SelectListItem>();
            employeeProfilelist.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Header", Value = "Header" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Middle", Value = "Middle" });
            employeeProfilelist.Add(new SelectListItem() { Text = "Footer", Value = "Footer" });
            model.ReportPlacementList = employeeProfilelist;

        }

        public void MapDropdownForReportDemo(TransferReportViewModel model)
        {
            model.ReportList = commonReportOptions.GetTransferReportOptions();
            var Report = new List<SelectListItem>();
            Report.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            Report.Add(new SelectListItem() { Text = "Employee Transfer Indicator", Value = "1" });
            model.ReportListStatic = Report;

            var officeType = officeTypeService.GetMany(w => w.IsActive == true); ;
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
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            model.OfficeList = ofc_items;


            var ZoneList = officeService.GetMany(x => x.OfficeTypeId == 4 && x.IsActive == true);
            var viewZoneList = ZoneList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = x.OfficeName.ToString()
            });
            var zone_items = new List<SelectListItem>();
            zone_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            zone_items.AddRange(viewZoneList);
            zone_items.Add(new SelectListItem() { Text = "All", Value = "1000", Selected = true });
            model.ZoneList = zone_items;

            var area_items = new List<SelectListItem>();
            area_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            model.AreaList = area_items;

            var unit_items = new List<SelectListItem>();
            unit_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            //zone_items.AddRange(viewZoneList);
            model.UnitList = unit_items;

            var empStatus = new List<SelectListItem>();
            empStatus.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            var statusList = employeeStatusService.GetMany(x => x.IsActive == true).OrderBy(p => p.ViewOrder);
            var getEmpStatus = statusList.AsEnumerable().Select(row => new SelectListItem
            {
                Text = row.StatusName,
                Value = row.StatusValue

            }).ToList();
            empStatus.AddRange(getEmpStatus);
            model.EmployeeStatusList = empStatus;

            var dept = employeeDepartmentService.GetMany(p => p.IsActive == true);
            var viewDept = dept.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.DepartmentName,
                Value = p.DepartmentId.ToString()
            }).ToList();
            var deptList = new List<SelectListItem>();
            deptList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            deptList.AddRange(viewDept);
            model.DepartmentList = deptList;

            var departmentList = employeeDepartmentService.GetAll();
            var viewDepartmentList = departmentList.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.DepartmentCode, m.DepartmentName), Value = m.DepartmentId.ToString() });
            var dep_items = new List<SelectListItem>();
            dep_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            dep_items.AddRange(viewDepartmentList);
            model.DepartmentList = dep_items;


            var designationList = employeeDesignationService.GetAll();
            var viewDesignationList = designationList.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.DesignationCode, m.DesignationName), Value = m.DesignationId.ToString() });
            var desig_items = new List<SelectListItem>();
            desig_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            desig_items.AddRange(viewDesignationList);
            model.DesignationList = desig_items;

            var activeInactiveList = new List<SelectListItem>();
            activeInactiveList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            activeInactiveList.Add(new SelectListItem() { Text = "Active", Value = "1" });
            activeInactiveList.Add(new SelectListItem() { Text = "Inactive", Value = "2" });
            model.ActiveInactiveList = activeInactiveList;

            var sectionList = new List<SelectListItem>();
            sectionList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            model.SectionList = sectionList;

            var designationLists = employeeDesignationService.GetAll();
            var viewDesignationLists = designationLists.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.DesignationCode, m.DesignationName), Value = m.DesignationId.ToString() });
            var desig_item = new List<SelectListItem>();
            desig_item.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            desig_item.AddRange(viewDesignationLists);
            model.DesignationList = desig_item;
            model.ResponsibilityList = commonDynamicDropDown.GetAllOfficeDesignationList();

            var empType = employementTypeService.GetMany(p => p.IsActive == true).OrderBy(p => p.ViewOrder).ToList();
            var viewEmpType = empType.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.EmployementTypeName,
                Value = p.EmployementTypeId.ToString()
            }).ToList();
            var typeList = new List<SelectListItem>();
            typeList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            typeList.AddRange(viewEmpType);
            model.EmploymentTypeList = typeList;
        }

        #endregion
    }
}
