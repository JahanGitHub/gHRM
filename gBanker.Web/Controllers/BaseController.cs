using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using gHRM.Web.Helpers;
using gHRM.Web.Filters;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using gHRM.Core.Common;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using System.Linq;
using gHRM.Web.Infrastucture.Framework;
//using CrystalDecisions.Shared;
using gHRM.Service.ReportServices;
using gHRM.Service.ReportExecutionService;
using Microsoft.Reporting.WebForms;
using gHRM.Core.Utilities.Constants;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System.Globalization;
using gHRM.Web.Infrastucture;

namespace gHRM.Web.Controllers
{
    [Authorize]
    //[CustomAuthenticationFilter]
    [SessionExpireFilter]
    [DisableCache]
    public class BaseController : Controller
    {
        public string CompanyLogoUrl;
        private string getCompanyLogoUrl()
        {
            CompanyLogoUrl = "CompanyLogo/CompanyLogo.Png";
            return CompanyLogoUrl;
        }

        public string getCompanyName()
        {
            //new
            return "";
        }

        //Asad Added
        //Implement later
        public string getCompanyAddress()
        {
            return "";
        }

        //Asad Added Accordng to NUPMS
        public IEnumerable<SelectListItem> TransactionTypeList()
        {
            var TransactionType = new List<SelectListItem>();
            TransactionType.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            TransactionType.Add(new SelectListItem() { Text = "Debit", Value = "Dr" });
            TransactionType.Add(new SelectListItem() { Text = "Credit", Value = "Cr" });
            return TransactionType;
        }

        public IEnumerable<SelectListItem> VoucherTypeList()
        {
            var VoucherType = new List<SelectListItem>();
            VoucherType.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            VoucherType.Add(new SelectListItem() { Text = "Cash Voucher", Value = "Ca" });
            VoucherType.Add(new SelectListItem() { Text = "Bank Cash", Value = "Bc" });
            VoucherType.Add(new SelectListItem() { Text = "Bank Voucher", Value = "Ba" });
            VoucherType.Add(new SelectListItem() { Text = "Payment Voucher", Value = "Pv" });
            VoucherType.Add(new SelectListItem() { Text = "Receipt Voucher", Value = "Rv" });
            VoucherType.Add(new SelectListItem() { Text = "Journal Voucher", Value = "Jr" });

            return VoucherType;
        }

        public JsonResult GetSuccessMessageResult(string message = "")
        {
            return Json(new { Result = "OK", Message = message.Length == 0 ? "Data saved successfully." : message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetErrorMessageResult(string message = "")
        {
            return Json(new { Result = "Error", Message = message.Length == 0 ? "Failed to save data. Please verify all required fields" : message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetErrorMessageResult(Exception ex)
        {
            var msg = ex.Message;
            if (ex.InnerException != null)
                msg = string.Format("Exception: {0}. \n Exception Detail: {1}. \n Source: {2}", msg, ex.InnerException.Message, ex.Source);
            return Json(new { Result = "Error", Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetErrorMessageResult(IEnumerable<ValidationResult> validationResults)
        {
            var msg = "";
            foreach (var validationResult in validationResults)
            {
                string key = validationResult.MemberName ?? string.Empty;
                msg = string.Format("{0}</br>", validationResult.Message);
            }
            return Json(new { Result = "Error", Message = "Please correct the following data. </br>" + msg }, JsonRequestBehavior.AllowGet);
        }

        protected bool IsAuthenticated
        {
            get { return SessionHelper.IsAuthenticated; }
        }

        protected EmployeeViewModel LoggedInEmployee
        {
            get { return SessionHelper.LoggedInEmployee; }
        }

        protected Int64? LoggedInEmployeeId
        {
            get
            {
                return SessionHelper.LoggedInEmployeeID;
            }
        }

        protected Int32? LoggedInEmployeeDepartmentId { get { return SessionHelper.LoggedInEmployeeDepartmentId; } }//jobayer
        protected string UserFullName
        {
            get { return SessionHelper.UserFullName; }
        }
        protected string LoginEmployeeName
        {
            get { return SessionHelper.EmployeeFullName; }
        }
        protected int? LoginUserOfficeID { get { return SessionHelper.LoginUserOfficeID; } }
        protected DateTime TransactionDate
        {
            get { return SessionHelper.TransactionDate; }
        }
        protected string TransactionDay
        {
            get { return SessionHelper.TransactionDay; }
        }
        protected string OrganizationName
        {
            get { return SessionHelper.OrganizationName; }
        }
        protected int? CompanyID
        {
            get { return SessionHelper.CompanyID; }
        }
        protected int? CountryID
        {
            get { return SessionHelper.CountryID; }
        }
        protected int? LoggedInOfficeID
        {
            get { return SessionHelper.LoginUserOfficeID; }
        }
        protected int? LoggedInOfficeType
        {
            get { return SessionHelper.LoginUserOfficeType; }
        }
        protected string ProcessType
        {
            get { return SessionHelper.ProcessType; }
        }
        protected bool IsDayInitiated
        {
            get { return SessionHelper.IsDayInitiated; }
        }

        // Here I have created this for execute each time any controller (inherit this) load 
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string lang = null;
            HttpCookie langCookie = Request.Cookies["culture"];
            if (langCookie != null)
                lang = langCookie.Value;
            else
            {
                var userLanguage = Request.UserLanguages;
                var userLang = userLanguage != null ? userLanguage[0] : "";
                if (userLang != "")
                    lang = userLang;
                else
                    lang = SiteLanguages.GetDefaultLanguage();
            }

            new SiteLanguages().SetLanguage(lang);

            return base.BeginExecuteCore(callback, state);
        }

        protected string GetSetting(string Name)
        {
            string Result = "";
            try
            {
                Result = System.Configuration.ConfigurationManager.AppSettings["SETTING." + Name].ToString();
            }
            catch { }
            return Result;
        }


        #region App Dropdown List
        #region Designation List
        public IEnumerable<SelectListItem> getDesignationTypeList()
        {
            var designationTypeList = new List<SelectListItem>();
            //  designationTypeList.Add(new SelectListItem() { Text = "Regular Designation", Value = "RD", Selected = true });
            // designationTypeList.Add(new SelectListItem() { Text = "Equivalent Designation", Value = "ED" });
            designationTypeList.Add(new SelectListItem() { Text = "Payroll Designation", Value = "OD" });
            return designationTypeList;
        }
        #endregion

        #region Module List
        public IEnumerable<SelectListItem> getModuleNameList()
        {
            var designationTypeList = new List<SelectListItem>();
            designationTypeList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            designationTypeList.Add(new SelectListItem() { Text = "Leave", Value = "LM" });
            designationTypeList.Add(new SelectListItem() { Text = "Transfer Approval", Value = "TA" });
            designationTypeList.Add(new SelectListItem() { Text = "Local Conveyance", Value = "LC" });
            return designationTypeList;
        }
        #endregion

        #endregion

        #region SSRS Report
        protected void PrintSSRSReport(string reportName, ParameterValue[] parameters)
        {
            var result = "";
            string connectionStringName = "gHRMReportDbContext";
            var binaryObj = SSRSReportProcessHelper.RenderReport("pdf", reportName, parameters, ref result, connectionStringName);
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(binaryObj);
            Response.End();
        }


        protected void PrintSSRSMultiformat(string Format, string reportName, ParameterValue[] parameters)
        {
            try
            {
                string connectionStringName = "gHRMReportDbContext";
                CultureInfo culture = CultureInfo.CurrentCulture;
                var result = "";
                var binaryObj = SSRSReportProcessHelper.RenderReport(Format.ToLower(), reportName, parameters, ref result, connectionStringName);
                if (Format.ToLower() == "excel")
                {
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AppendHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("ddMMyyyyHHmmss").ToLower(culture) + ".xls");
                }
                else if(Format.ToLower() == "word")
                {
                    Response.ContentType = "application/vnd.ms-word";
                    Response.AppendHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("ddMMyyyyHHmmss").ToLower(culture) + ".doc");
                }
                else if (Format.ToLower() == "pdf")
                    Response.ContentType = "application/pdf";

                Response.BinaryWrite(binaryObj);
                Response.End();
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
        }
        #endregion

        #region RDLC Report

        public ActionResult Report(DataTable dataSource, string reportDataSourceName, Dictionary<string, object> parameters, string reportTitle, string reportPath, string format, string type = "view", string reportViewMode = ReportViewModeConstants.Potrait)
        {
            var reportDataSources = new List<ReportDataSource>
            {
                new ReportDataSource{ Name = reportDataSourceName.ToString(), Value = dataSource }
            };

            string reportName = $@"{reportTitle.Trim().Replace(" ", "").Replace("(", "").Replace(")", "")}_{DateTime.UtcNow.AddHours(6).ToShortDateString()}_{DateTime.UtcNow.AddHours(6).ToShortTimeString()}";

            return ReportGenerator(reportDataSources, parameters, format, reportPath, reportTitle, reportName, type, reportViewMode);
        }

        public ActionResult Report(List<ReportDataSource> reportDataSources, Dictionary<string, object> parameters, string reportTitle, string reportPath, string format, string type = "view", string reportViewMode = ReportViewModeConstants.Potrait)
        {
            string reportName = $@"{reportTitle.Trim().Replace(" ", "").Replace("(", "").Replace(")", "")}_{DateTime.UtcNow.AddHours(6).ToShortDateString()}_{DateTime.UtcNow.AddHours(6).ToShortTimeString()}";
            return ReportGenerator(reportDataSources, parameters, format, reportPath, reportTitle, reportName, type, reportViewMode);
        }

        public ActionResult ReportGenerator(List<ReportDataSource> reportDataSources, Dictionary<string, object> parameters, string format, string reportPath, string reportTitle, string reportName, string type = "view", string reportViewMode = ReportViewModeConstants.Potrait)
        {
            format = format.Trim().ToLower();
            if (string.IsNullOrEmpty(format))
            {
                format = "pdf";
            }
            else if (format != "pdf" && format != "word" && format != "excel")
            {
                format = "pdf";
            }
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath(reportPath);
            foreach (var item in reportDataSources)
            {
                localReport.DataSources.Add(item);
            }

            localReport.EnableExternalImages = true;
            string imagePath = @"file:\" + AppDomain.CurrentDomain.BaseDirectory;

            var reportParameters = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            if (parameters.Any())
            {
                foreach (var item in parameters)
                {
                    var reportParameter = new Microsoft.Reporting.WebForms.ReportParameter(item.Key, item.Value.ToString());
                    reportParameters.Add(reportParameter);
                }
            }
            localReport.SetParameters(reportParameters);

            string reportType = format; //"Image";            
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx   

            string pageWidth = "8.27in"; string pageHeight = "11.69in";

            if (reportViewMode == ReportViewModeConstants.Landscape)
            {
                pageWidth = "11.69in";
                pageHeight = "8.27in";
            }

            string deviceInfo = $@"<DeviceInfo>
                <OutputFormat>PDF</OutputFormat>
                <PageWidth>{pageWidth}</PageWidth>
                <PageHeight>{pageHeight}</PageHeight>
                <MarginTop>0.25in</MarginTop>
                <MarginLeft>0.75in</MarginLeft>
                <MarginRight>0.25in</MarginRight>
                <MarginBottom>0.25in</MarginBottom>
                </DeviceInfo>";

            Microsoft.Reporting.WebForms.Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Render the report            
            renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            if (format == "pdf")
            {
                if (type == "view")
                {
                    return File(renderedBytes, mimeType);
                }
                else
                {
                    reportName = reportName + ".pdf";
                    return File(renderedBytes, mimeType, reportName);
                }
            }
            else if (format == "word")
            {
                if (type == "view")
                {
                    return File(renderedBytes, mimeType);
                }
                else
                {
                    reportName = reportName + ".doc";
                    return File(renderedBytes, mimeType, reportName);
                }
            }
            else
            {
                reportName = reportName + ".xls";
                return File(renderedBytes, mimeType, reportName);
            }
        }

        #endregion

        #region MigraDoc Report
        public FileContentResult ReportMigraDoc(Document document)
        {
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(false);
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();
            using (MemoryStream stream = new MemoryStream())
            {
                pdfRenderer.PdfDocument.Save(stream, false);
                return File(stream.ToArray(), "application/pdf");
            }
        }    

        #endregion
    }
}