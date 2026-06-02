using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.Payroll;
using gHRM.Web.Reports.Payroll;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using BasicDataAccess;
using gHRM.Web.Reports.MigraDoc;
using Newtonsoft.Json;
using gHRM.Core.Utilities;
using OfficeOpenXml;
using System.IO;
using System.Text;
using System.Collections;

namespace gHRM.Web.Controllers
{
    public class PRSalaryReportController : BaseController
    {

        #region Variables

        private readonly IEmployeeSPService employeeSPService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IOfficeService officeService;


        public PRSalaryReportController(
             IEmployeeSPService employeeSPService
            , IOfficeTypeService officeTypeService
            , IOfficeService officeService

        )
        {
            this.employeeSPService = employeeSPService;
            this.officeTypeService = officeTypeService;
            this.officeService = officeService;

        }

        #endregion

        #region ActionMethods

        //public ActionResult SalaryReports()
        //{
        //    ViewData["Months"] = Months();
        //    ViewData["Years"] = Years();
        //    var model = new PRWorkAreaViewModel();

        //    mapIndexDropdown(model);
        //    var list = new List<SelectListItem>();
        //    var pleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
        //    list.Add(pleaseSelect);
        //    list.Add(new SelectListItem { Text = "Salary Report", Value = "SR" });
        //    list.Add(new SelectListItem { Text = "Salary Bank Statement", Value = "SBS" });
        //    model.ReportList = list;

        //    var listBank = new List<SelectListItem>();
        //    listBank.Add(pleaseSelect);

        //    var bank = bankNameService.GetMany(b => b.IsActive == true).ToList();
        //    var listView = bank.Select(row => new SelectListItem()
        //    {
        //        Text = row.BankFullName,
        //        Value = row.BankCode
        //    }).ToList();

        //    listBank.AddRange(listView);

        //    model.BankList = listBank;

        //    return View(model);
        //}

        public ActionResult EmployeeMonthlySalaryReportConfig()
        {
            EmployeeMonthlySalaryReport Obj = new EmployeeMonthlySalaryReport(HttpContext);
            ViewBag.DefaultSettings = JsonConvert.SerializeObject(Obj.DefaultSettings);
            ViewBag.Settings = JsonConvert.SerializeObject(Obj.Settings);
            return View();
        }

        [HttpPost]
        public JsonResult EmployeeMonthlySalaryReportConfigSave(EmployeeMonthlySalaryReport.EmployeeMonthlySalaryReport_Settings Data)
        {
            try
            {
                EmployeeMonthlySalaryReport Obj = new EmployeeMonthlySalaryReport(HttpContext);
                if (Data.TopMargin <= 0) Data.TopMargin = Obj.Settings.TopMargin;
                if (Data.HeaderFontSize <= 0) Data.HeaderFontSize = Obj.Settings.HeaderFontSize;
                if (Data.TableHeaderHeight <= 0) Data.TableHeaderHeight = Obj.Settings.TableHeaderHeight;
                if (Data.BodyColWidth <= 0) Data.BodyColWidth = Obj.Settings.BodyColWidth;
                if (Data.BodyFontSize <= 0) Data.BodyFontSize = Obj.Settings.BodyFontSize;
                System.IO.File.WriteAllText(Server.MapPath("~") + "App_Data\\EmployeeMonthlySalaryReport_Settings.json", JsonConvert.SerializeObject(Data));
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(Funct.GetError(ex));
            }
        }
        #endregion

        #region Salary Reports

        public ActionResult PrintSalaryBeforeApprovalReportPDF(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID, bool? w_o_HO)
        {
            try
            {
                if (officeTypeId == 0 && SessionHelper.LoggedInOfficeTypeId == 1)
                    officeTypeId = SessionHelper.LoggedInOfficeTypeId;
                string EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE = AppSetting.Get(AppSetting.EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE, HttpContext);

                if ("GT" == EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE)
                {
                    return EmployeeMonthlySalaryReport_GT(Year, Month, officeTypeId, salaryDate, officeID,w_o_HO);
                }
                if ("MigraDoc" == EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE)
                {
                    return EmployeeMonthlySalaryReport_MigraDoc(Year, Month, officeTypeId, salaryDate, officeID);
                }

                if ("GSSB" == EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE)
                {
                    return EmployeeMonthlySalaryReport_GSSB(Year, Month, officeTypeId, salaryDate, officeID);
                }
                if ("GTT" == EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE)
                {
                  return EmployeeMonthlySalaryReport_GTT(Year, Month, officeTypeId, salaryDate, officeID);
                }

                if ("SANGRAM" == EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE)
                {
                    return EmployeeMonthlySalaryReport_SANGRAM(Year, Month, officeTypeId, salaryDate, officeID, w_o_HO);
                }

                //if ("PIDIM" == EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE)
                //{
                //    return EmployeeMonthlySalaryReport_PIDIM(Year, Month, officeTypeId, salaryDate, officeID, w_o_HO);
                //}


                var param = new { SalaryYear = Year, SalaryMonth = Month, salaryDate = salaryDate, OfficeTypeId = officeTypeId, OfficeID = (officeID ?? 0) };

                var firstDate = new DateTime(Year, Month, 1);
                DateTime firstOfNextMonth = new DateTime(Year, Month, 1).AddMonths(1);
                var lastDate = firstOfNextMonth.AddDays(-1);

                var sqlProcedureName = "";
                sqlProcedureName = GetStoredProcedureMonthlySalaryBeforeApproval();

                var salaryData = employeeSPService.GetDataWithParameter(param, sqlProcedureName);
                var reportParam = new Dictionary<string, object>();

                var param2 = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeId = officeTypeId, OfficeID = (officeID ?? 0) };

                var subreportData = new DataSet();

                if (salaryData != null && salaryData.Tables.Count > 0)
                    subreportData = employeeSPService.GetDataWithParameter(param2, "prl.SP_GET_SalaryIncentive_ArrearAllEmployee");

                var subReportDB = new Dictionary<string, DataTable>();
               // string rpt = "PIDIM" == EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE ? "rptMothlySalaryReportPidim.rpt" : "rpt_SalaryReport_GSSB.rpt";
                string rpt = "PIDIM" == EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE ? "rptMothlySalaryReport_Test.rpt" : "rpt_SalaryReport_GSSB.rpt";
                subReportDB.Add("EmployeeArrearReport", subreportData.Tables[0]);
                if ("PIDIM" == EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE)
                {
                    //reportParam.Add("CompanyName22", SessionHelper.CompanyName);
                    //reportParam.Add("CompanyAddress22", SessionHelper.CompanyAddress);

                    ReportHelper.PrintWithSubReport("Payroll/" + rpt + "", salaryData.Tables[0], reportParam, subReportDB, new rptMothlySalaryReportPidim());
                }  
                else if("GC" == EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE)
                {
                    //ReportHelper.PrintWithSubReport("Payroll/" + rpt + "", salaryData.Tables[0], new Dictionary<string, object>(), subReportDB, new rptMothlySalaryReport_Test());
                    ReportHelper.PrintWithSubReport("Payroll/" + rpt + "", salaryData.Tables[0], new Dictionary<string, object>(), subReportDB, new rptMothlySalaryReport_GC());
                }
                else if ("GMPF" == EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE)
                {            
                    ReportHelper.PrintWithSubReport("Payroll/" + rpt + "", salaryData.Tables[0], reportParam, subReportDB, new rptMothlySalaryReportGMPF());
                }
                else if ("GUP" == EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE)
                {
                    rpt = "rptMothlySalaryReport_GUP.rpt";
                    ReportHelper.PrintWithSubReport("Payroll/" + rpt + "", salaryData.Tables[0], new Dictionary<string, object>(), subReportDB, new rptMothlySalaryReport_GUP());
                }
                else
                {
                    ReportHelper.PrintWithSubReport("Payroll/" + rpt + "", salaryData.Tables[0], new Dictionary<string, object>(), subReportDB, new rptMothlySalaryReport());                   

                }
                 

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintSalaryBeforeApprovalReportPDF3_excel(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID, bool? w_o_HO, int reportId )
        {
            try
            {
                if (officeTypeId == 0 && SessionHelper.LoggedInOfficeTypeId == 1)
                    officeTypeId = SessionHelper.LoggedInOfficeTypeId;
                string EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE = AppSetting.Get(AppSetting.EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE, HttpContext);

                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryYear", Value = Year.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryMonth", Value = Month.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryDate", Value = salaryDate });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (officeTypeId ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (officeID ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "w_o_HO", Value = (w_o_HO ?? false) });


                var asigId1 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ASignatureId).FirstOrDefault();
                var Signature1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.DesignationId).FirstOrDefault();
                var Designation1 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId1).Select(z => z.DesignationName).FirstOrDefault();


                var asigId2 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.BSignatureId).FirstOrDefault();
                var Signature2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.DesignationId).FirstOrDefault();
                var Designation2 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId2).Select(z => z.DesignationName).FirstOrDefault();

                var asigId3 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.CSignatureId).FirstOrDefault();
                var Signature3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.DesignationId).FirstOrDefault();
                var Designation3 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId3).Select(z => z.DesignationName).FirstOrDefault();


                var asigId4 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.DSignatureId).FirstOrDefault();
                var Signature4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.DesignationId).FirstOrDefault();
                var Designation4 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId4).Select(z => z.DesignationName).FirstOrDefault();


                var asigId5 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ESignatureId).FirstOrDefault();
                var Signature5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.DesignationId).FirstOrDefault();
                var Designation5 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId5).Select(z => z.DesignationName).FirstOrDefault();


                var asigId6 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.FSignatureId).FirstOrDefault();
                var Signature6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.DesignationId).FirstOrDefault();
                var Designation6 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId6).Select(z => z.DesignationName).FirstOrDefault();



                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature1", Value = Signature1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature2", Value = Signature2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature3", Value = Signature3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature4", Value = Signature4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature5", Value = Signature5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature6", Value = Signature6 });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation1", Value = Designation1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation2", Value = Designation2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation3", Value = Designation3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation4", Value = Designation4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation5", Value = Designation5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation6", Value = Designation6 });

                var param = new { CompanyShortName = SessionHelper.CompanyInfo.CompanyShortName, OfficeId = LoginUserOfficeID, UserId = LoggedInEmployeeId, ReportId = reportId };
                var ssrs = employeeSPService.GetDataWithParameter(param, "GetReportPathByCompany");

                var rptName = ssrs.Tables[0].Rows[0]["ReportPath"].ToString();
                PrintSSRSMultiformat("excel", rptName, paramValues.ToArray());

                return Content(string.Empty);


            }
            catch (Exception ex)
            {
                var exceptionDetails = new StringBuilder();
                exceptionDetails.AppendLine($"Message: {ex.Message}");

                if (ex.InnerException != null)
                    exceptionDetails.AppendLine($"Inner Exception: {ex.InnerException.Message}");

                if (!string.IsNullOrEmpty(ex.HelpLink))
                    exceptionDetails.AppendLine($"Help Link: {ex.HelpLink}");

                exceptionDetails.AppendLine($"Source: {ex.Source}");
                exceptionDetails.AppendLine($"Data: {string.Join(", ", ex.Data.Cast<DictionaryEntry>().Select(de => $"{de.Key}: {de.Value}"))}");

                return Json(new
                {
                    Result = "ERROR",
                    Message = exceptionDetails.ToString()
                }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult PrintSalaryBeforeApprovalReportPDF2_excel(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID, bool? w_o_HO)
        {
            try
            {
                if (officeTypeId == 0 && SessionHelper.LoggedInOfficeTypeId == 1)
                    officeTypeId = SessionHelper.LoggedInOfficeTypeId;
                string EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE = AppSetting.Get(AppSetting.EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE, HttpContext);

                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryYear", Value = Year.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryMonth", Value = Month.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryDate", Value = salaryDate });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (officeTypeId ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (officeID ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "w_o_HO", Value = (w_o_HO ?? false) });


                var asigId1 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ASignatureId).FirstOrDefault();
                var Signature1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.DesignationId).FirstOrDefault();
                var Designation1 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId1).Select(z => z.DesignationName).FirstOrDefault();


                var asigId2 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.BSignatureId).FirstOrDefault();
                var Signature2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.DesignationId).FirstOrDefault();
                var Designation2 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId2).Select(z => z.DesignationName).FirstOrDefault();

                var asigId3 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.CSignatureId).FirstOrDefault();
                var Signature3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.DesignationId).FirstOrDefault();
                var Designation3 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId3).Select(z => z.DesignationName).FirstOrDefault();


                var asigId4 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.DSignatureId).FirstOrDefault();
                var Signature4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.DesignationId).FirstOrDefault();
                var Designation4 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId4).Select(z => z.DesignationName).FirstOrDefault();


                var asigId5 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ESignatureId).FirstOrDefault();
                var Signature5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.DesignationId).FirstOrDefault();
                var Designation5 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId5).Select(z => z.DesignationName).FirstOrDefault();


                var asigId6 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.FSignatureId).FirstOrDefault();
                var Signature6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.DesignationId).FirstOrDefault();
                var Designation6 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId6).Select(z => z.DesignationName).FirstOrDefault();



                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature1", Value = Signature1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature2", Value = Signature2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature3", Value = Signature3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature4", Value = Signature4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature5", Value = Signature5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature6", Value = Signature6 });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation1", Value = Designation1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation2", Value = Designation2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation3", Value = Designation3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation4", Value = Designation4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation5", Value = Designation5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation6", Value = Designation6 });

                if (SessionHelper.CompanyInfo.CompanyShortName == "GTT")
                    PrintSSRSMultiformat("excel", "/gHRMPlus_Reports/EmployeeMonthlySalaryReport_GTT_new", paramValues.ToArray());
                else if (SessionHelper.CompanyInfo.CompanyShortName == "GT")
                    PrintSSRSMultiformat("excel", "/gHRMPlus_Reports/EmployeeMonthlySalaryReport_GT", paramValues.ToArray());
                else if (SessionHelper.CompanyInfo.CompanyShortName == "VERC")
                    PrintSSRSMultiformat("excel", "/gHRMPlus_Reports/EmployeeMonthlySalaryReport_VERC", paramValues.ToArray());
                else if (SessionHelper.CompanyInfo.CompanyShortName == "GMPF")
                    PrintSSRSMultiformat("excel", "/gHRMPlus_Reports/EmployeeMonthlySalaryReport_GMPF", paramValues.ToArray());
                else if (SessionHelper.CompanyInfo.CompanyShortName == "Prottyashi")
                    PrintSSRSMultiformat("excel", "/gHRMPlus_Reports/EmployeeMonthlySalaryReport_Prottyashi", paramValues.ToArray());

                //if (SessionHelper.CompanyInfo.CompanyShortName == "GTT")
                //       PrintSSRSReport("/gHRMPlus_Reports/EmployeeMonthlySalaryReport_GTT", paramValues.ToArray());
                return Content(string.Empty);


            }
            catch (Exception ex)
            {
                var exceptionDetails = new StringBuilder();
                exceptionDetails.AppendLine($"Message: {ex.Message}");

                if (ex.InnerException != null)
                    exceptionDetails.AppendLine($"Inner Exception: {ex.InnerException.Message}");

                if (!string.IsNullOrEmpty(ex.HelpLink))
                    exceptionDetails.AppendLine($"Help Link: {ex.HelpLink}");

                exceptionDetails.AppendLine($"Source: {ex.Source}");
                exceptionDetails.AppendLine($"Data: {string.Join(", ", ex.Data.Cast<DictionaryEntry>().Select(de => $"{de.Key}: {de.Value}"))}");

                return Json(new
                {
                    Result = "ERROR",
                    Message = exceptionDetails.ToString()
                }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult PrintSalaryAfterApprovalReportPDF2_excel(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID, bool? w_o_HO)
        {
            try
            {
                if (officeTypeId == 0 && SessionHelper.LoggedInOfficeTypeId == 1)
                    officeTypeId = SessionHelper.LoggedInOfficeTypeId;
                string EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE = AppSetting.Get(AppSetting.EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE, HttpContext);

                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryYear", Value = Year.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryMonth", Value = Month.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryDate", Value = salaryDate });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (officeTypeId ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (officeID ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "w_o_HO", Value = (w_o_HO ?? false) });


                var asigId1 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ASignatureId).FirstOrDefault();
                var Signature1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.DesignationId).FirstOrDefault();
                var Designation1 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId1).Select(z => z.DesignationName).FirstOrDefault();


                var asigId2 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.BSignatureId).FirstOrDefault();
                var Signature2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.DesignationId).FirstOrDefault();
                var Designation2 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId2).Select(z => z.DesignationName).FirstOrDefault();

                var asigId3 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.CSignatureId).FirstOrDefault();
                var Signature3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.DesignationId).FirstOrDefault();
                var Designation3 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId3).Select(z => z.DesignationName).FirstOrDefault();


                var asigId4 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.DSignatureId).FirstOrDefault();
                var Signature4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.DesignationId).FirstOrDefault();
                var Designation4 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId4).Select(z => z.DesignationName).FirstOrDefault();


                var asigId5 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ESignatureId).FirstOrDefault();
                var Signature5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.DesignationId).FirstOrDefault();
                var Designation5 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId5).Select(z => z.DesignationName).FirstOrDefault();


                var asigId6 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.FSignatureId).FirstOrDefault();
                var Signature6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.DesignationId).FirstOrDefault();
                var Designation6 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId6).Select(z => z.DesignationName).FirstOrDefault();



                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature1", Value = Signature1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature2", Value = Signature2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature3", Value = Signature3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature4", Value = Signature4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature5", Value = Signature5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature6", Value = Signature6 });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation1", Value = Designation1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation2", Value = Designation2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation3", Value = Designation3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation4", Value = Designation4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation5", Value = Designation5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation6", Value = Designation6 });

                if (SessionHelper.CompanyInfo.CompanyShortName == "GTT")
                    PrintSSRSMultiformat("excel", "/gHRMPlus_Reports/EmployeeMonthlySalaryReport_GTT_new", paramValues.ToArray());
                else if (SessionHelper.CompanyInfo.CompanyShortName == "GT")
                    PrintSSRSMultiformat("excel", "/gHRMPlus_Reports/EmployeeMonthlySalaryReport_GT_APPROVED", paramValues.ToArray());
                else if (SessionHelper.CompanyInfo.CompanyShortName == "VERC")
                    PrintSSRSMultiformat("excel", "/gHRMPlus_Reports/EmployeeMonthlySalaryReport_VERC", paramValues.ToArray());

                //if (SessionHelper.CompanyInfo.CompanyShortName == "GTT")
                //       PrintSSRSReport("/gHRMPlus_Reports/EmployeeMonthlySalaryReport_GTT", paramValues.ToArray());
                return Content(string.Empty);


            }
            catch (Exception ex)
            {
                var exceptionDetails = new StringBuilder();
                exceptionDetails.AppendLine($"Message: {ex.Message}");

                if (ex.InnerException != null)
                    exceptionDetails.AppendLine($"Inner Exception: {ex.InnerException.Message}");

                if (!string.IsNullOrEmpty(ex.HelpLink))
                    exceptionDetails.AppendLine($"Help Link: {ex.HelpLink}");

                exceptionDetails.AppendLine($"Source: {ex.Source}");
                exceptionDetails.AppendLine($"Data: {string.Join(", ", ex.Data.Cast<DictionaryEntry>().Select(de => $"{de.Key}: {de.Value}"))}");

                return Json(new
                {
                    Result = "ERROR",
                    Message = exceptionDetails.ToString()
                }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult PrintSalaryBeforeApprovalReportPDF3(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID, bool? w_o_HO, string reportId )
        {
            try
            {
                if (officeTypeId == 0 && SessionHelper.LoggedInOfficeTypeId == 1)
                    officeTypeId = SessionHelper.LoggedInOfficeTypeId;
                string EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE = AppSetting.Get(AppSetting.EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE, HttpContext);

                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryYear", Value = Year.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryMonth", Value = Month.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryDate", Value = salaryDate });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (officeTypeId ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (officeID ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "w_o_HO", Value = (w_o_HO ?? false) });


                var asigId1 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ASignatureId).FirstOrDefault();
                var Signature1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.DesignationId).FirstOrDefault();
                var Designation1 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId1).Select(z => z.DesignationName).FirstOrDefault();


                var asigId2 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.BSignatureId).FirstOrDefault();
                var Signature2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.DesignationId).FirstOrDefault();
                var Designation2 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId2).Select(z => z.DesignationName).FirstOrDefault();

                var asigId3 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.CSignatureId).FirstOrDefault();
                var Signature3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.DesignationId).FirstOrDefault();
                var Designation3 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId3).Select(z => z.DesignationName).FirstOrDefault();


                var asigId4 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.DSignatureId).FirstOrDefault();
                var Signature4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.DesignationId).FirstOrDefault();
                var Designation4 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId4).Select(z => z.DesignationName).FirstOrDefault();


                var asigId5 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ESignatureId).FirstOrDefault();
                var Signature5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.DesignationId).FirstOrDefault();
                var Designation5 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId5).Select(z => z.DesignationName).FirstOrDefault();


                var asigId6 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.FSignatureId).FirstOrDefault();
                var Signature6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.DesignationId).FirstOrDefault();
                var Designation6 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId6).Select(z => z.DesignationName).FirstOrDefault();



                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature1", Value = Signature1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature2", Value = Signature2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature3", Value = Signature3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature4", Value = Signature4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature5", Value = Signature5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature6", Value = Signature6 });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation1", Value = Designation1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation2", Value = Designation2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation3", Value = Designation3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation4", Value = Designation4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation5", Value = Designation5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation6", Value = Designation6 });


                var param = new { CompanyShortName = SessionHelper.CompanyInfo.CompanyShortName, OfficeId = LoginUserOfficeID, UserId = LoggedInEmployeeId, ReportId = reportId };
                var ssrs = employeeSPService.GetDataWithParameter(param, "GetReportPathByCompany");

                var rptName = ssrs.Tables[0].Rows[0]["ReportPath"].ToString();
                PrintSSRSReport(rptName, paramValues.ToArray());

                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                var exceptionDetails = new StringBuilder();
                exceptionDetails.AppendLine($"Message: {ex.Message}");

                if (ex.InnerException != null)
                    exceptionDetails.AppendLine($"Inner Exception: {ex.InnerException.Message}");

                if (!string.IsNullOrEmpty(ex.HelpLink))
                    exceptionDetails.AppendLine($"Help Link: {ex.HelpLink}");

                exceptionDetails.AppendLine($"Source: {ex.Source}");
                exceptionDetails.AppendLine($"Data: {string.Join(", ", ex.Data.Cast<DictionaryEntry>().Select(de => $"{de.Key}: {de.Value}"))}");

                return Json(new
                {
                    Result = "ERROR",
                    Message = exceptionDetails.ToString()
                }, JsonRequestBehavior.AllowGet);
            }

        }



        public ActionResult PrintSalaryBeforeApprovalReportPDF_SalaryAdvice(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID, bool? w_o_HO, string reportId)
        {
            try
            {
                if (officeTypeId == 0 && SessionHelper.LoggedInOfficeTypeId == 1)
                    officeTypeId = SessionHelper.LoggedInOfficeTypeId;
                string EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE = AppSetting.Get(AppSetting.EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE, HttpContext);

                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryYear", Value = Year.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryMonth", Value = Month.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryDate", Value = salaryDate });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (officeTypeId ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (officeID ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "w_o_HO", Value = (w_o_HO ?? false) });


                var asigId1 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ASignatureId).FirstOrDefault();
                var Signature1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.DesignationId).FirstOrDefault();
                var Designation1 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId1).Select(z => z.DesignationName).FirstOrDefault();


                var asigId2 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.BSignatureId).FirstOrDefault();
                var Signature2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.DesignationId).FirstOrDefault();
                var Designation2 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId2).Select(z => z.DesignationName).FirstOrDefault();

                var asigId3 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.CSignatureId).FirstOrDefault();
                var Signature3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.DesignationId).FirstOrDefault();
                var Designation3 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId3).Select(z => z.DesignationName).FirstOrDefault();


                var asigId4 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.DSignatureId).FirstOrDefault();
                var Signature4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.DesignationId).FirstOrDefault();
                var Designation4 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId4).Select(z => z.DesignationName).FirstOrDefault();


                var asigId5 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ESignatureId).FirstOrDefault();
                var Signature5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.DesignationId).FirstOrDefault();
                var Designation5 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId5).Select(z => z.DesignationName).FirstOrDefault();


                var asigId6 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.FSignatureId).FirstOrDefault();
                var Signature6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.DesignationId).FirstOrDefault();
                var Designation6 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId6).Select(z => z.DesignationName).FirstOrDefault();



                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature1", Value = Signature1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature2", Value = Signature2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature3", Value = Signature3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature4", Value = Signature4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature5", Value = Signature5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature6", Value = Signature6 });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation1", Value = Designation1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation2", Value = Designation2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation3", Value = Designation3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation4", Value = Designation4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation5", Value = Designation5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation6", Value = Designation6 });


                var param = new { CompanyShortName = SessionHelper.CompanyInfo.CompanyShortName, OfficeId = LoginUserOfficeID, UserId = LoggedInEmployeeId, ReportId = reportId };
                var ssrs = employeeSPService.GetDataWithParameter(param, "GetReportPathByCompany");

                var rptName = ssrs.Tables[0].Rows[0]["ReportPath"].ToString();
                PrintSSRSReport(rptName, paramValues.ToArray());

                return Content(string.Empty);


            }
            catch (Exception ex)
            {
                var exceptionDetails = new StringBuilder();
                exceptionDetails.AppendLine($"Message: {ex.Message}");

                if (ex.InnerException != null)
                    exceptionDetails.AppendLine($"Inner Exception: {ex.InnerException.Message}");

                if (!string.IsNullOrEmpty(ex.HelpLink))
                    exceptionDetails.AppendLine($"Help Link: {ex.HelpLink}");

                exceptionDetails.AppendLine($"Source: {ex.Source}");
                exceptionDetails.AppendLine($"Data: {string.Join(", ", ex.Data.Cast<DictionaryEntry>().Select(de => $"{de.Key}: {de.Value}"))}");

                return Json(new
                {
                    Result = "ERROR",
                    Message = exceptionDetails.ToString()
                }, JsonRequestBehavior.AllowGet);
            }

        }



        public ActionResult EmployeeWiseLeaveReportPrint2(string dateFrom="2025-01-01", string dateTo="2025-01-01", string employeeCode="240020")
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

                PrintSSRSReport("/gHRMPlus_Reports/SSRS_SalaryAdvice", paramValues.ToArray());

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



        public ActionResult PrintSalaryBeforeApprovalReportPDF4(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID, bool? w_o_HO, string reportId )
        {
            try
            {
                if (officeTypeId == 0 && SessionHelper.LoggedInOfficeTypeId == 1)
                    officeTypeId = SessionHelper.LoggedInOfficeTypeId;
                string EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE = AppSetting.Get(AppSetting.EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE, HttpContext);

                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryYear", Value = Year.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryMonth", Value = Month.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryDate", Value = salaryDate });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (officeTypeId ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (officeID ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "w_o_HO", Value = (w_o_HO ?? false) });


                var asigId1 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ASignatureId).FirstOrDefault();
                var Signature1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.DesignationId).FirstOrDefault();
                var Designation1 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId1).Select(z => z.DesignationName).FirstOrDefault();


                var asigId2 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.BSignatureId).FirstOrDefault();
                var Signature2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.DesignationId).FirstOrDefault();
                var Designation2 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId2).Select(z => z.DesignationName).FirstOrDefault();

                var asigId3 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.CSignatureId).FirstOrDefault();
                var Signature3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.DesignationId).FirstOrDefault();
                var Designation3 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId3).Select(z => z.DesignationName).FirstOrDefault();


                var asigId4 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.DSignatureId).FirstOrDefault();
                var Signature4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.DesignationId).FirstOrDefault();
                var Designation4 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId4).Select(z => z.DesignationName).FirstOrDefault();


                var asigId5 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ESignatureId).FirstOrDefault();
                var Signature5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.DesignationId).FirstOrDefault();
                var Designation5 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId5).Select(z => z.DesignationName).FirstOrDefault();


                var asigId6 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.FSignatureId).FirstOrDefault();
                var Signature6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.DesignationId).FirstOrDefault();
                var Designation6 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId6).Select(z => z.DesignationName).FirstOrDefault();



                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature1", Value = Signature1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature2", Value = Signature2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature3", Value = Signature3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature4", Value = Signature4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature5", Value = Signature5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature6", Value = Signature6 });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation1", Value = Designation1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation2", Value = Designation2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation3", Value = Designation3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation4", Value = Designation4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation5", Value = Designation5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation6", Value = Designation6 });

                var param = new { reportId = reportId, OrgId = SessionHelper.CompanyInfo.CompanyShortName };
                var rpt = employeeSPService.GetDataWithParameter(param, "GetReportName");

                var rptName = rpt.Tables[0].Rows[0]["rptName"].ToString();

                PrintSSRSReport(rptName, paramValues.ToArray());

                return Content(string.Empty);


            }
            catch (Exception ex)
            {
                var exceptionDetails = new StringBuilder();
                exceptionDetails.AppendLine($"Message: {ex.Message}");

                if (ex.InnerException != null)
                    exceptionDetails.AppendLine($"Inner Exception: {ex.InnerException.Message}");

                if (!string.IsNullOrEmpty(ex.HelpLink))
                    exceptionDetails.AppendLine($"Help Link: {ex.HelpLink}");

                exceptionDetails.AppendLine($"Source: {ex.Source}");
                exceptionDetails.AppendLine($"Data: {string.Join(", ", ex.Data.Cast<DictionaryEntry>().Select(de => $"{de.Key}: {de.Value}"))}");

                return Json(new
                {
                    Result = "ERROR",
                    Message = exceptionDetails.ToString()
                }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult PrintSalaryBeforeApprovalReportPDF2(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID, bool? w_o_HO)
        {
            try
            {
                if (officeTypeId == 0 && SessionHelper.LoggedInOfficeTypeId == 1)
                    officeTypeId = SessionHelper.LoggedInOfficeTypeId;

                //App_Data\AppSetting.json
                string EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE = AppSetting.Get(AppSetting.EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE, HttpContext);

                gHRMDBContext db = new gHRMDBContext();
                    var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryYear", Value = Year.ToString() });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryMonth", Value = Month.ToString() });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryDate", Value = salaryDate });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (officeTypeId ?? 0).ToString() });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (officeID ?? 0).ToString() });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "w_o_HO", Value = (w_o_HO ?? false) });


                    var asigId1 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ASignatureId).FirstOrDefault();
                    var Signature1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.EmployeeName).FirstOrDefault();

                    var desigId1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.DesignationId).FirstOrDefault();
                    var Designation1 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId1).Select(z => z.DesignationName).FirstOrDefault();


                    var asigId2 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.BSignatureId).FirstOrDefault();
                    var Signature2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.EmployeeName).FirstOrDefault();

                    var desigId2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.DesignationId).FirstOrDefault();
                    var Designation2 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId2).Select(z => z.DesignationName).FirstOrDefault();

                    var asigId3 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.CSignatureId).FirstOrDefault();
                    var Signature3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.EmployeeName).FirstOrDefault();

                    var desigId3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.DesignationId).FirstOrDefault();
                    var Designation3 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId3).Select(z => z.DesignationName).FirstOrDefault();


                    var asigId4 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.DSignatureId).FirstOrDefault();
                    var Signature4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.EmployeeName).FirstOrDefault();

                    var desigId4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.DesignationId).FirstOrDefault();
                    var Designation4 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId4).Select(z => z.DesignationName).FirstOrDefault();


                    var asigId5 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ESignatureId).FirstOrDefault();
                    var Signature5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.EmployeeName).FirstOrDefault();

                    var desigId5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.DesignationId).FirstOrDefault();
                    var Designation5 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId5).Select(z => z.DesignationName).FirstOrDefault();


                    var asigId6 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.FSignatureId).FirstOrDefault();
                    var Signature6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.EmployeeName).FirstOrDefault();

                    var desigId6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.DesignationId).FirstOrDefault();
                    var Designation6 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId6).Select(z => z.DesignationName).FirstOrDefault();



                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature1", Value = Signature1 });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature2", Value = Signature2 });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature3", Value = Signature3 });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature4", Value = Signature4 });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature5", Value = Signature5 });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature6", Value = Signature6 });

                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation1", Value = Designation1 });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation2", Value = Designation2 });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation3", Value = Designation3 });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation4", Value = Designation4 });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation5", Value = Designation5 });
                    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation6", Value = Designation6 });

                   if (SessionHelper.CompanyInfo.CompanyShortName == "GTT")
                      PrintSSRSReport("/gHRMPlus_Reports/EmployeeMonthlySalaryReport_GTT_new", paramValues.ToArray());
                   else if(SessionHelper.CompanyInfo.CompanyShortName == "VERC")
                      PrintSSRSReport("/gHRMPlus_Reports/EmployeeMonthlySalaryReport_VERC", paramValues.ToArray());
                   else if (SessionHelper.CompanyInfo.CompanyShortName == "Prottyashi")
                      PrintSSRSReport("/gHRMPlus_Reports/EmployeeMonthlySalaryReport_Prottyashi", paramValues.ToArray());
                   else if (SessionHelper.CompanyInfo.CompanyShortName == "GMPF")
                      PrintSSRSReport("/gHRMPlus_Reports/EmployeeMonthlySalaryReport_GMPF", paramValues.ToArray());
                //if (SessionHelper.CompanyInfo.CompanyShortName == "GTT")
                //       PrintSSRSReport("/gHRMPlus_Reports/EmployeeMonthlySalaryReport_GTT", paramValues.ToArray());
                return Content(string.Empty);
        

            }
            catch (Exception ex)
            {
                var exceptionDetails = new StringBuilder();
                exceptionDetails.AppendLine($"Message: {ex.Message}");

                if (ex.InnerException != null)
                    exceptionDetails.AppendLine($"Inner Exception: {ex.InnerException.Message}");

                if (!string.IsNullOrEmpty(ex.HelpLink))
                    exceptionDetails.AppendLine($"Help Link: {ex.HelpLink}");

                exceptionDetails.AppendLine($"Source: {ex.Source}");
                exceptionDetails.AppendLine($"Data: {string.Join(", ", ex.Data.Cast<DictionaryEntry>().Select(de => $"{de.Key}: {de.Value}"))}");

                return Json(new
                {
                    Result = "ERROR",
                    Message = exceptionDetails.ToString()
                }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult PrintSalaryBeforeApprovalReportExel(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID)
        {
            try
            {

                if (SessionHelper.CompanyInfo.CompanyShortName == "GTT" || SessionHelper.CompanyInfo.CompanyShortName == "GT" || SessionHelper.CompanyInfo.CompanyShortName == "VERC" || SessionHelper.CompanyInfo.CompanyShortName == "GMPF" || SessionHelper.CompanyInfo.CompanyShortName == "Prottyashi")
                {
                    return PrintSalaryBeforeApprovalReportPDF2_excel(Year, Month, officeTypeId, salaryDate, officeID, false );
                }
                else
                {

                    if (officeTypeId == 0 && SessionHelper.LoggedInOfficeTypeId == 1)
                        officeTypeId = SessionHelper.LoggedInOfficeTypeId;


                    string EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE = AppSetting.Get(AppSetting.EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE, HttpContext);

                    var param = new { SalaryYear = Year, SalaryMonth = Month, salaryDate = salaryDate, OfficeTypeId = officeTypeId, OfficeID = (officeID ?? 0) };



                    var firstDate = new DateTime(Year, Month, 1);
                    DateTime firstOfNextMonth = new DateTime(Year, Month, 1).AddMonths(1);
                    var lastDate = firstOfNextMonth.AddDays(-1);

                    var sqlProcedureName = "";
                    if (officeTypeId == null)
                        sqlProcedureName = GetStoredProcedureMonthlySalaryBeforeApproval_WO_HO();
                    else
                        sqlProcedureName = GetStoredProcedureMonthlySalaryBeforeApproval();

                    var salaryData = employeeSPService.GetDataWithParameter(param, sqlProcedureName);

                    var reportParam = new Dictionary<string, object>();

                    var param2 = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeId = officeTypeId, OfficeID = (officeID ?? 0) };
                    var subreportData = employeeSPService.GetDataWithParameter(param2, "prl.SP_GET_SalaryIncentive_ArrearAllEmployee");

                    var subReportDB = new Dictionary<string, DataTable>();

                    subReportDB.Add("EmployeeArrearReport", subreportData.Tables[0]);
                    ReportHelper.ExportExcelWithSubReport("Payroll/rptMothlySalaryReport.rpt", salaryData.Tables[0], new Dictionary<string, object>(), subReportDB, new rptMothlySalaryReport());


                    return Content(string.Empty);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult PrintSalaryBeforeApprovalReportExel99(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID, bool? w_o_HO)
        {
            try
            {

                if (SessionHelper.CompanyInfo.CompanyShortName == "GTT" || SessionHelper.CompanyInfo.CompanyShortName == "GT")
                {
                    return PrintSalaryBeforeApprovalReportPDF2_excel(Year, Month, officeTypeId, salaryDate, officeID, w_o_HO);
                }
                else
                {

                    if (officeTypeId == 0 && SessionHelper.LoggedInOfficeTypeId == 1)
                        officeTypeId = SessionHelper.LoggedInOfficeTypeId;


                    string EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE = AppSetting.Get(AppSetting.EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE, HttpContext);

                    var param = new { SalaryYear = Year, SalaryMonth = Month, salaryDate = salaryDate, OfficeTypeId = officeTypeId, OfficeID = (officeID ?? 0) };



                    var firstDate = new DateTime(Year, Month, 1);
                    DateTime firstOfNextMonth = new DateTime(Year, Month, 1).AddMonths(1);
                    var lastDate = firstOfNextMonth.AddDays(-1);

                    var sqlProcedureName = "";
                    if (officeTypeId == null)
                        sqlProcedureName = GetStoredProcedureMonthlySalaryBeforeApproval_WO_HO();
                    else
                        sqlProcedureName = GetStoredProcedureMonthlySalaryBeforeApproval();

                    var salaryData = employeeSPService.GetDataWithParameter(param, sqlProcedureName);

                    var reportParam = new Dictionary<string, object>();

                    var param2 = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeId = officeTypeId, OfficeID = (officeID ?? 0) };
                    var subreportData = employeeSPService.GetDataWithParameter(param2, "prl.SP_GET_SalaryIncentive_ArrearAllEmployee");

                    var subReportDB = new Dictionary<string, DataTable>();

                    subReportDB.Add("EmployeeArrearReport", subreportData.Tables[0]);
                    ReportHelper.ExportExcelWithSubReport("Payroll/rptMothlySalaryReport.rpt", salaryData.Tables[0], new Dictionary<string, object>(), subReportDB, new rptMothlySalaryReport());


                    return Content(string.Empty);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintSalaryBeforeApprovalReportExel3(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID, int reportId )
        {
            try
            {

                if (SessionHelper.CompanyInfo.CompanyShortName == "GTT")
                {
                    return PrintSalaryBeforeApprovalReportPDF3_excel(Year, Month, officeTypeId, salaryDate, officeID, false, reportId);
                }
                else
                {

                    if (officeTypeId == 0 && SessionHelper.LoggedInOfficeTypeId == 1)
                        officeTypeId = SessionHelper.LoggedInOfficeTypeId;


                    string EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE = AppSetting.Get(AppSetting.EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE, HttpContext);

                    var param = new { SalaryYear = Year, SalaryMonth = Month, salaryDate = salaryDate, OfficeTypeId = officeTypeId, OfficeID = (officeID ?? 0) };



                    var firstDate = new DateTime(Year, Month, 1);
                    DateTime firstOfNextMonth = new DateTime(Year, Month, 1).AddMonths(1);
                    var lastDate = firstOfNextMonth.AddDays(-1);

                    var sqlProcedureName = "";
                    if (officeTypeId == null)
                        sqlProcedureName = GetStoredProcedureMonthlySalaryBeforeApproval_WO_HO();
                    else
                        sqlProcedureName = GetStoredProcedureMonthlySalaryBeforeApproval();

                    var salaryData = employeeSPService.GetDataWithParameter(param, sqlProcedureName);

                    var reportParam = new Dictionary<string, object>();

                    var param2 = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeId = officeTypeId, OfficeID = (officeID ?? 0) };
                    var subreportData = employeeSPService.GetDataWithParameter(param2, "prl.SP_GET_SalaryIncentive_ArrearAllEmployee");

                    var subReportDB = new Dictionary<string, DataTable>();

                    subReportDB.Add("EmployeeArrearReport", subreportData.Tables[0]);
                    ReportHelper.ExportExcelWithSubReport("Payroll/rptMothlySalaryReport.rpt", salaryData.Tables[0], new Dictionary<string, object>(), subReportDB, new rptMothlySalaryReport());


                    return Content(string.Empty);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintSalaryBeforeApprovalReportExel2(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID)
        {
            try
            {

                if (SessionHelper.CompanyInfo.CompanyShortName == "GTT")
                {
                    return PrintSalaryBeforeApprovalReportPDF2_excel(Year, Month, officeTypeId, salaryDate, officeID, false);
                }
                else
                {

                    if (officeTypeId == 0 && SessionHelper.LoggedInOfficeTypeId == 1)
                        officeTypeId = SessionHelper.LoggedInOfficeTypeId;


                    string EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE = AppSetting.Get(AppSetting.EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE, HttpContext);

                    var param = new { SalaryYear = Year, SalaryMonth = Month, salaryDate = salaryDate, OfficeTypeId = officeTypeId, OfficeID = (officeID ?? 0) };



                    var firstDate = new DateTime(Year, Month, 1);
                    DateTime firstOfNextMonth = new DateTime(Year, Month, 1).AddMonths(1);
                    var lastDate = firstOfNextMonth.AddDays(-1);

                    var sqlProcedureName = "";
                    if (officeTypeId == null)
                        sqlProcedureName = GetStoredProcedureMonthlySalaryBeforeApproval_WO_HO();
                    else
                        sqlProcedureName = GetStoredProcedureMonthlySalaryBeforeApproval();

                    var salaryData = employeeSPService.GetDataWithParameter(param, sqlProcedureName);

                    var reportParam = new Dictionary<string, object>();

                    var param2 = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeId = officeTypeId, OfficeID = (officeID ?? 0) };
                    var subreportData = employeeSPService.GetDataWithParameter(param2, "prl.SP_GET_SalaryIncentive_ArrearAllEmployee");

                    var subReportDB = new Dictionary<string, DataTable>();

                    subReportDB.Add("EmployeeArrearReport", subreportData.Tables[0]);
                    ReportHelper.ExportExcelWithSubReport("Payroll/rptMothlySalaryReport.rpt", salaryData.Tables[0], new Dictionary<string, object>(), subReportDB, new rptMothlySalaryReport());


                    return Content(string.Empty);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult PrintRejectedEmployeesSalaryReportPDF(int Year, int Month)
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month };
                var salaryData = employeeSPService.GetDataWithParameter(param, "prl.SP_rpt_MothlySalaryReport_RejectedEmployee");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Payroll/rptMothlySalaryReport.rpt", salaryData.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintApprovedSalaryReportPDF(int Year, int Month, int OfficeTypeId = 0)
        {
            try
            {
                string EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE = AppSetting.Get(AppSetting.EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE, HttpContext);
                if ("GT" == EMPLOYEE_MONTHLY_SALARY_REPORT_TEMPLATE)
                {
                    var lst_dt=new gHRMDBContext().EmployeeMonthlySalary.Where(x => x.IsActive && x.IsApproved && x.SalaryYear == Year && x.SalaryMonth == Month)
                        .Select(s => s.SalaryDate).Distinct();
                    if (lst_dt.Any())
                    {
                        string salaryDate = lst_dt.First().ToString("dd-MMM-yyyy");
                        return EmployeeMonthlySalaryReport_GT(Year, Month, OfficeTypeId, salaryDate, 0, false);
                    }
                }

                var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId };
                var firstDate = new DateTime(Year, Month, 1);
                DateTime firstOfNextMonth = new DateTime(Year, Month, 1).AddMonths(1);
                var lastDate = firstOfNextMonth.AddDays(-1);

                var salaryData = employeeSPService.GetDataWithParameter(param, "prl.SP_rpt_MothlySalaryReportAfterApproval");
                var reportParam = new Dictionary<string, object>();

                var param2 = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeId = 0, OfficeID = 0 };
                var subreportData = employeeSPService.GetDataWithParameter(param2, "prl.SP_GET_SalaryIncentive_ArrearAllEmployee");

                var subReportDB = new Dictionary<string, DataTable>();

                subReportDB.Add("EmployeeArrearReport", subreportData.Tables[0]);
                ReportHelper.PrintWithSubReport("Payroll/rptMothlySalaryReport.rpt", salaryData.Tables[0], new Dictionary<string, object>(), subReportDB, new rptMothlySalaryReport());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintApprovedSalaryReportExel2(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID, bool? w_o_HO)
        {
            try
            {
                if (SessionHelper.CompanyInfo.CompanyShortName == "GTT" || SessionHelper.CompanyInfo.CompanyShortName == "GT")
                {
                    return PrintSalaryAfterApprovalReportPDF2_excel(Year, Month, officeTypeId, salaryDate, officeID, w_o_HO);
                }
                else
                {
                    var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = officeTypeId };

                    var firstDate = new DateTime(Year, Month, 1);
                    DateTime firstOfNextMonth = new DateTime(Year, Month, 1).AddMonths(1);
                    var lastDate = firstOfNextMonth.AddDays(-1);

                    var salaryData = employeeSPService.GetDataWithParameter(param, "prl.SP_rpt_MothlySalaryReportAfterApproval");
                    var reportParam = new Dictionary<string, object>();

                    var param2 = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeId = 0, OfficeID = 0 };
                    var subreportData = employeeSPService.GetDataWithParameter(param2, "prl.SP_GET_SalaryIncentive_ArrearAllEmployee");

                    var subReportDB = new Dictionary<string, DataTable>();
                    subReportDB.Add("EmployeeArrearReport", subreportData.Tables[0]);
                    ReportHelper.ExportExcelWithSubReport("Payroll/rptMothlySalaryReport_Test.rpt", salaryData.Tables[0], new Dictionary<string, object>(), subReportDB, new rptMothlySalaryReport_Test());

                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult PrintApprovedSalaryReportExel(int Year, int Month, int OfficeTypeId = 0)
        {
            try
            {
                    var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId };

                    var firstDate = new DateTime(Year, Month, 1);
                    DateTime firstOfNextMonth = new DateTime(Year, Month, 1).AddMonths(1);
                    var lastDate = firstOfNextMonth.AddDays(-1);

                    var salaryData = employeeSPService.GetDataWithParameter(param, "prl.SP_rpt_MothlySalaryReportAfterApproval");
                    var reportParam = new Dictionary<string, object>();

                    var param2 = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeId = 0, OfficeID = 0 };
                    var subreportData = employeeSPService.GetDataWithParameter(param2, "prl.SP_GET_SalaryIncentive_ArrearAllEmployee");

                    var subReportDB = new Dictionary<string, DataTable>();
                    subReportDB.Add("EmployeeArrearReport", subreportData.Tables[0]);
                    ReportHelper.ExportExcelWithSubReport("Payroll/rptMothlySalaryReport_Test.rpt", salaryData.Tables[0], new Dictionary<string, object>(), subReportDB, new rptMothlySalaryReport_Test());
                
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult PrintSalaryReportAfterApprovalGroupByOfficePDF(int Year, int Month, int OfficeTypeId = 0)
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId };

                var firstDate = new DateTime(Year, Month, 1);
                DateTime firstOfNextMonth = new DateTime(Year, Month, 1).AddMonths(1);
                var lastDate = firstOfNextMonth.AddDays(-1);

                var salaryData = employeeSPService.GetDataWithParameter(param, "prl.SP_rpt_MothlySalaryReportAfterApproval");
                var reportParam = new Dictionary<string, object>();

                var param2 = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeId = OfficeTypeId, OfficeID = 0 };
                var subreportData = employeeSPService.GetDataWithParameter(param2, "prl.SP_GET_SalaryIncentive_ArrearAllEmployee");

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("EmployeeArrearReport", subreportData.Tables[0]);
                ReportHelper.PrintWithSubReport("Payroll/rptMothlySalaryReportGroupBy.rpt", salaryData.Tables[0], new Dictionary<string, object>(), subReportDB, new rptMothlySalaryReportGroupBy());

                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintSalaryReportAfterApprovalGroupByOffice(int Year, int Month, int OfficeTypeId = 0)
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeTypeId = OfficeTypeId };

                var firstDate = new DateTime(Year, Month, 1);
                DateTime firstOfNextMonth = new DateTime(Year, Month, 1).AddMonths(1);
                var lastDate = firstOfNextMonth.AddDays(-1);

                var salaryData = employeeSPService.GetDataWithParameter(param, "prl.SP_rpt_MothlySalaryReportAfterApproval");
                var reportParam = new Dictionary<string, object>();

                var param2 = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeId = OfficeTypeId, OfficeID = 0 };
                var subreportData = employeeSPService.GetDataWithParameter(param2, "prl.SP_GET_SalaryIncentive_ArrearAllEmployee");

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("EmployeeArrearReport", subreportData.Tables[0]);
                ReportHelper.ExportExcelWithSubReport("Payroll/rptMothlySalaryReportGroupBy.rpt", salaryData.Tables[0], new Dictionary<string, object>(), subReportDB, new rptMothlySalaryReportGroupBy());

                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintSalaryReportAfterApprovalGroupByZoneAreaPDF(int Year, int Month, int OfficeId = 0)
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month, OfficeId = OfficeId };

                var firstDate = new DateTime(Year, Month, 1);
                DateTime firstOfNextMonth = new DateTime(Year, Month, 1).AddMonths(1);
                var lastDate = firstOfNextMonth.AddDays(-1);

                var salaryData = employeeSPService.GetDataWithParameter(param, "prl.SP_rpt_MothlySalaryReportAfterApprovalZoneArea");
                var reportParam = new Dictionary<string, object>();

                var param2 = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeId = OfficeId };
                var subreportData = employeeSPService.GetDataWithParameter(param2, "prl.SP_GET_SalaryIncentive_ArrearAllEmployeeZoneArea");

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("EmployeeArrearReport", subreportData.Tables[0]);
                ReportHelper.PrintWithSubReport("Payroll/rptMothlySalaryReportGroupByZoneArea.rpt", salaryData.Tables[0], new Dictionary<string, object>(), subReportDB, new rptMothlySalaryReportGroupBy());

                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PrintPFReportBeforeApproval(int Year, int Month, int reportType, int? officeTypeId = 0)
        {
            try
            {
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Year", Value = Year.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Month", Value = Month.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officeTypeId", Value = officeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "reportType", Value = reportType.ToString() });
                PrintSSRSReport("/gHRMPlus_Reports/PFReportBeforeApproval", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // Salary Statement Details Report Before Approval PDF
        public ActionResult PrintEmployeeSalaryStatementDetailsBeforeApprovalPDF(int Year, int Month, int? officeTypeId, int? officeID)
            {
            try
            {
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
             //   paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
             //   paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Year", Value = Year.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Month", Value = Month.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = officeTypeId.ToString() });
            //    paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryDate", Value = salaryDate.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeID", Value = officeID.ToString() });
               // paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "reportType", Value = reportType.ToString() });
                PrintSSRSReport("/gHRMPlus_Reports/GC_Salary_Statement_Details", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Unknows/unused Reports

        public ActionResult PrintOfficeTypeWiseSalaryReportAfterApproval(int OfficeTypeId, int Year, int Month, string BankCode)
        {
            try
            {
                var param = new { SalaryYear = Year, SalaryMonth = Month };

                var firstDate = new DateTime(Year, Month, 1);
                DateTime firstOfNextMonth = new DateTime(Year, Month, 1).AddMonths(1);
                var lastDate = firstOfNextMonth.AddDays(-1);

                var salaryData = employeeSPService.GetDataWithParameter(param, "prl.SP_rpt_MothlySalaryReportAfterApproval");
                var reportParam = new Dictionary<string, object>();

                var param2 = new { EffectiveStartDate = firstDate, EffectiveEndDate = lastDate, OfficeTypeId = OfficeTypeId, OfficeID = 0 };
                var subreportData = employeeSPService.GetDataWithParameter(param2, "prl.SP_GET_SalaryIncentive_ArrearAllEmployee");

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("EmployeeArrearReport", subreportData.Tables[0]);
                ReportHelper.PrintWithSubReport("Payroll/rptMothlySalaryReport.rpt", salaryData.Tables[0], new Dictionary<string, object>(), subReportDB, new rptMothlySalaryReport());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Dropdown Methods

        public void mapIndexDropdown(PRWorkAreaViewModel model)
        {
            var pleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var view_list = new List<SelectListItem>();
            view_list.Add(pleaseSelect);

            var officeTypeList = officeTypeService.GetMany(x => x.IsActive == true);
            var list = officeTypeList.AsEnumerable().Select(row => new SelectListItem
            {
                Text = row.OfficeTypeName,
                Value = row.OfficeTypeId.ToString()

            }).ToList();
            view_list.AddRange(list);

            model.OfficeTypeList = view_list;

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
        }

        #endregion

        #region Private Methods

        private string GetStoredProcedureMonthlySalaryBeforeApproval_WO_HO()
        {
            var sqlProcedureName = "prl.EmployeeMonthlySalary_GetMothlySalaryBeforeApproval_WO_HO";

            if (SessionHelper.CompanyInfo.CompanyShortName == GHRMPlusCompanyConstants.GrameenTelecomTrust)
                sqlProcedureName = "prl.EmployeeMonthlySalary_GetGTTMothlySalaryBeforeApproval";

            return sqlProcedureName;
        }

        private string GetStoredProcedureMonthlySalaryBeforeApproval()
        {
            var sqlProcedureName = "prl.EmployeeMonthlySalary_GetMothlySalaryBeforeApproval";

            if (SessionHelper.CompanyInfo.CompanyShortName == GHRMPlusCompanyConstants.GrameenTelecomTrust)
                sqlProcedureName = "prl.EmployeeMonthlySalary_GetGTTMothlySalaryBeforeApproval";

            return sqlProcedureName;
        }

        public ActionResult EmployeeMonthlySalaryReport_GT(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID,bool? w_o_HO)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryYear", Value = Year.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryMonth", Value = Month.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryDate", Value = salaryDate });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (officeTypeId ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (officeID ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "w_o_HO", Value = (w_o_HO ?? false) });


                var asigId1 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ASignatureId).FirstOrDefault();
                var Signature1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.DesignationId).FirstOrDefault();
                var Designation1 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId1).Select(z => z.DesignationName).FirstOrDefault();


                var asigId2 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.BSignatureId).FirstOrDefault();
                var Signature2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.DesignationId).FirstOrDefault();
                var Designation2 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId2).Select(z => z.DesignationName).FirstOrDefault();

                var asigId3 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.CSignatureId).FirstOrDefault();
                var Signature3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.DesignationId).FirstOrDefault();
                var Designation3 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId3).Select(z => z.DesignationName).FirstOrDefault();


                var asigId4 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.DSignatureId).FirstOrDefault();
                var Signature4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.DesignationId).FirstOrDefault();
                var Designation4 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId4).Select(z => z.DesignationName).FirstOrDefault();


                var asigId5 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ESignatureId).FirstOrDefault();
                var Signature5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.DesignationId).FirstOrDefault();
                var Designation5 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId5).Select(z => z.DesignationName).FirstOrDefault();


                var asigId6 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.FSignatureId).FirstOrDefault();
                var Signature6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.DesignationId).FirstOrDefault();
                var Designation6 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId6).Select(z => z.DesignationName).FirstOrDefault();



                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature1", Value = Signature1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature2", Value = Signature2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature3", Value = Signature3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature4", Value = Signature4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature5", Value = Signature5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature6", Value = Signature6 });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation1", Value = Designation1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation2", Value = Designation2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation3", Value = Designation3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation4", Value = Designation4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation5", Value = Designation5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation6", Value = Designation6 });



                PrintSSRSReport("/gHRMPlus_Reports/EmployeeMonthlySalaryReport_GT", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        public ActionResult EmployeeMonthlySalaryReport_PIDIM(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID, bool? w_o_HO)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryYear", Value = Year.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryMonth", Value = Month.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryDate", Value = salaryDate });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (officeTypeId ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (officeID ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "w_o_HO", Value = (w_o_HO ?? false) });


                var asigId1 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ASignatureId).FirstOrDefault();
                var Signature1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.DesignationId).FirstOrDefault();
                var Designation1 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId1).Select(z => z.DesignationName).FirstOrDefault();


                var asigId2 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.BSignatureId).FirstOrDefault();
                var Signature2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.DesignationId).FirstOrDefault();
                var Designation2 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId2).Select(z => z.DesignationName).FirstOrDefault();

                var asigId3 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.CSignatureId).FirstOrDefault();
                var Signature3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.DesignationId).FirstOrDefault();
                var Designation3 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId3).Select(z => z.DesignationName).FirstOrDefault();


                var asigId4 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.DSignatureId).FirstOrDefault();
                var Signature4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.DesignationId).FirstOrDefault();
                var Designation4 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId4).Select(z => z.DesignationName).FirstOrDefault();


                var asigId5 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ESignatureId).FirstOrDefault();
                var Signature5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.DesignationId).FirstOrDefault();
                var Designation5 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId5).Select(z => z.DesignationName).FirstOrDefault();


                var asigId6 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.FSignatureId).FirstOrDefault();
                var Signature6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.DesignationId).FirstOrDefault();
                var Designation6 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId6).Select(z => z.DesignationName).FirstOrDefault();



                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature1", Value = Signature1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature2", Value = Signature2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature3", Value = Signature3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature4", Value = Signature4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature5", Value = Signature5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature6", Value = Signature6 });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation1", Value = Designation1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation2", Value = Designation2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation3", Value = Designation3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation4", Value = Designation4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation5", Value = Designation5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation6", Value = Designation6 });

                PrintSSRSReport("/gHRMPlus_Reports/EmployeeMonthlySalaryReport_PIDIM", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult EmployeeMonthlySalaryReport_SANGRAM(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID, bool? w_o_HO)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryYear", Value = Year.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryMonth", Value = Month.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SalaryDate", Value = salaryDate });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (officeTypeId ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (officeID ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "w_o_HO", Value = (w_o_HO ?? false) });


                var asigId1 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ASignatureId).FirstOrDefault();
                var Signature1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId1 = db.Employees.Where(x => x.EmployeeId == asigId1).Select(z => z.DesignationId).FirstOrDefault();
                var Designation1 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId1).Select(z => z.DesignationName).FirstOrDefault();


                var asigId2 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.BSignatureId).FirstOrDefault();
                var Signature2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId2 = db.Employees.Where(x => x.EmployeeId == asigId2).Select(z => z.DesignationId).FirstOrDefault();
                var Designation2 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId2).Select(z => z.DesignationName).FirstOrDefault();

                var asigId3 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.CSignatureId).FirstOrDefault();
                var Signature3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId3 = db.Employees.Where(x => x.EmployeeId == asigId3).Select(z => z.DesignationId).FirstOrDefault();
                var Designation3 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId3).Select(z => z.DesignationName).FirstOrDefault();


                var asigId4 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.DSignatureId).FirstOrDefault();
                var Signature4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId4 = db.Employees.Where(x => x.EmployeeId == asigId4).Select(z => z.DesignationId).FirstOrDefault();
                var Designation4 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId4).Select(z => z.DesignationName).FirstOrDefault();


                var asigId5 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.ESignatureId).FirstOrDefault();
                var Signature5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId5 = db.Employees.Where(x => x.EmployeeId == asigId5).Select(z => z.DesignationId).FirstOrDefault();
                var Designation5 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId5).Select(z => z.DesignationName).FirstOrDefault();


                var asigId6 = db.ReportSignatures.Where(x => x.Code == "EMSS").Select(z => z.FSignatureId).FirstOrDefault();
                var Signature6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.EmployeeName).FirstOrDefault();

                var desigId6 = db.Employees.Where(x => x.EmployeeId == asigId6).Select(z => z.DesignationId).FirstOrDefault();
                var Designation6 = db.EmployeeDesignations.Where(x => x.DesignationId == desigId6).Select(z => z.DesignationName).FirstOrDefault();



                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature1", Value = Signature1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature2", Value = Signature2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature3", Value = Signature3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature4", Value = Signature4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature5", Value = Signature5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Signature6", Value = Signature6 });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation1", Value = Designation1 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation2", Value = Designation2 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation3", Value = Designation3 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation4", Value = Designation4 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation5", Value = Designation5 });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Designation6", Value = Designation6 });

                PrintSSRSReport("/gHRMPlus_Reports/EmployeeMonthlySalaryReport_SANGRAM", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult EmployeeMonthlySalaryReport_MigraDoc(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID)
        {
            try
            {
                var Params = new
                {
                    SalaryYear = Year,
                    SalaryMonth = Month,
                    SalaryDate = salaryDate,
                    OfficeTypeId = officeTypeId ?? 0,
                    OfficeID = officeID ?? 0
                };
                string ReportName = "EmployeeMonthlySalaryReport";
                using (var gbData = new gHRMDataAccess())
                {
                    DataSet DSet = gbData.GetDataOnDateset("prl.SP_EmployeeMonthlySalaryReport_MigraDoc", Params);
                    EmployeeMonthlySalaryReport Report = new EmployeeMonthlySalaryReport(HttpContext);
                    Report.SalaryYear = Year;
                    Report.SalaryMonth = Month;
                    return ReportMigraDoc(Report.GetPdfDocument(ReportName, DSet));
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        public ActionResult EmployeeMonthlySalaryReport_GTT(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID)
        {
            try
            {
                var Params = new
                {
                    SalaryYear = Year,
                    SalaryMonth = Month,
                    SalaryDate = salaryDate,
                    OfficeTypeId = officeTypeId ?? 0,
                    OfficeID = officeID ?? 0
                };
                string ReportName = "EmployeeMonthlySalaryReport_GTT";



                using (var gbData = new gHRMDataAccess())
                {
                    DataSet DSet = gbData.GetDataOnDateset("prl.SP_EmployeeMonthlySalaryReport_GTT", Params);
                    EmployeeMonthlySalaryReport_GTT Report = new EmployeeMonthlySalaryReport_GTT(HttpContext);
                    Report.SalaryYear = Year;
                    Report.SalaryMonth = Month;
                    return ReportMigraDoc(Report.GetPdfDocument(ReportName, DSet));
                }


            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

       

        public ActionResult EmployeeMonthlySalaryReport_GSSB(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID)
        {
            try
            {
                var Params = new
                {
                    SalaryYear = Year,
                    SalaryMonth = Month,
                    SalaryDate = salaryDate,
                    OfficeTypeId = officeTypeId ?? 0,
                    OfficeID = officeID ?? 0
                };
                string ReportName = "EmployeeMonthlySalaryReport_GSSB";



                using (var gbData = new gHRMDataAccess())
                {
                    DataSet DSet = gbData.GetDataOnDateset("prl.SP_EmployeeMonthlySalaryReport_GSSB", Params);
                    EmployeeMonthlySalaryReport_GSSB Report = new EmployeeMonthlySalaryReport_GSSB(HttpContext);
                    Report.SalaryYear = Year;
                    Report.SalaryMonth = Month;
                    return ReportMigraDoc(Report.GetPdfDocument(ReportName, DSet));
                }


            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        public ActionResult EmployeeMonthlySalaryReport_PIDIM(int Year, int Month, int? officeTypeId, string salaryDate, int? officeID)
        {
            try
            {
                var Params = new
                {
                    SalaryYear = Year,
                    SalaryMonth = Month,
                    SalaryDate = salaryDate,
                    OfficeTypeId = officeTypeId ?? 0,
                    OfficeID = officeID ?? 0
                };
                string ReportName = "EmployeeMonthlySalaryReport_PIDIM";



                using (var gbData = new gHRMDataAccess())
                {
                    DataSet DSet = gbData.GetDataOnDateset("prl.SP_EmployeeMonthlySalaryReport_PIDIM", Params);
                    EmployeeMonthlySalaryReport_PIDIM Report = new EmployeeMonthlySalaryReport_PIDIM(HttpContext);
                    Report.SalaryYear = Year;
                    Report.SalaryMonth = Month;
                    return ReportMigraDoc(Report.GetPdfDocument(ReportName, DSet));
                }


            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        #endregion

    } // End of Class
}