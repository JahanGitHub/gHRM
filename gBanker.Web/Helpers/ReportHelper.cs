using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using gHRM.Data.DBDetailModels.OverTimes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace gHRM.Web.Helpers
{
    public class ReportHelper
    {
        //public static void PrintReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters)
        //{
        //    try
        //    {

        //        ReportDocument crDocument = new ReportDocument();

        //        ExportOptions crExportOptions = new ExportOptions();
        //        DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
        //        string strFName;
        //        //All CR file assumed that it resides in the reports folder....
        //        string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
        //        crDocument.Load(strReportPathAbsolute);
        //        crDocument.SetDataSource(dataSource);

        //        foreach (KeyValuePair<string, object> kvp in parameters)
        //        {
        //            crDocument.SetParameterValue(kvp.Key, kvp.Value);

        //        }
        //        strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
        //        crDiskFileDestination.DiskFileName = strFName;
        //        crExportOptions = crDocument.ExportOptions;
        //        crExportOptions.DestinationOptions = crDiskFileDestination;
        //        crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //        crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //        crDocument.Export();
        //        crDocument.Dispose();
        //        crDocument.Close();
        //        //Response.ClearContent();
        //        // Response.ClearHeaders();
        //        // Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", strFName));
        //        HttpContext.Current.Response.ContentType = "application/pdf";
        //       HttpContext.Current.Response.WriteFile(strFName);
        //       HttpContext.Current.Response.End();
        //        // Response.Close();
        //        System.IO.File.Delete(strFName);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        //jahan
        public static void PrintReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters)
        {
            string strReportPathAbsolute = "";
            ReportDocument crDocument = new ReportDocument();
            try
            {
                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);
                crDocument.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);

                }
                ///strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.xls", Guid.NewGuid());
                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;

                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //crExportOptions.ExportFormatType = ExportFormatType.Excel;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDocument.Export();
                crDocument.Dispose();
                crDocument.Close();
                //Response.ClearContent();
                //Response.ClearHeaders();
                //Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", strFName));
                ///HttpContext.Current.Response.ContentType = "application/xls";
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();
                // Response.Close();
                System.IO.File.Delete(strFName);
                crDocument.Close();
                crDocument.Dispose();
                //GC.Collect();

            }
            catch (Exception ex)
            {
                StringBuilder s = new StringBuilder();
                while (ex != null)
                {
                    s.AppendLine("Exception type: " + ex.GetType().FullName);
                    s.AppendLine("Report Path: " + strReportPathAbsolute);
                    s.AppendLine("Message       : " + ex.Message);
                    s.AppendLine("Stacktrace:");
                    s.AppendLine(ex.StackTrace);
                    s.AppendLine();
                    ex = ex.InnerException;
                }
                throw new Exception(s.ToString());
            }
            finally
            {
                crDocument.Close();
                crDocument.Dispose();
            }
        }



        public static void PrintReportWithMultipleDataSource(string reportName, DataTable dataSource, string dataSourceName,
            DataTable dtCompanyInfo, string dtCompanyInfoName, Dictionary<string, object> parameters)
        {
            try
            {
                ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;

                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);               
                crDocument.Database.Tables[dataSourceName].SetDataSource(dataSource);
                crDocument.Database.Tables[dtCompanyInfoName].SetDataSource(dtCompanyInfo);
                
                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);
                }
                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDocument.Export();
                crDocument.Dispose();
                crDocument.Close();                
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();             
                System.IO.File.Delete(strFName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static void PrintReportWithSubReportAndMultipleDataSource(string reportName, DataTable dataSource, string dataSourceName,
            DataTable dtCompanyInfo, string dtCompanyInfoName, Dictionary<string, object> parameters, Dictionary<string, DataTable> subReportDatasources)
        {
            try
            {
                ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;

                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);               
                crDocument.Database.Tables[dataSourceName].SetDataSource(dataSource);
                crDocument.Database.Tables[dtCompanyInfoName].SetDataSource(dtCompanyInfo);
                
                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);
                }

                if (subReportDatasources != null)
                {
                    foreach (KeyValuePair<string, DataTable> kvp in subReportDatasources)
                    {                       
                        crDocument.OpenSubreport(kvp.Key).SetDataSource(kvp.Value);
                    }
                }

                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDocument.Export();
                crDocument.Dispose();
                crDocument.Close();                
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();             
                System.IO.File.Delete(strFName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ExportExcelReport2(string reportName, DataTable dataSource, Dictionary<string, object> parameters)
        {
            try
            {
                ReportDocument crDocument = new ReportDocument();
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);
                crDocument.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);
                }

                // Set up Excel format options to include headers
                ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
                excelFormatOpts.ExcelUseConstantColumnWidth = true;
                excelFormatOpts.ShowGridLines = true;
                excelFormatOpts.ExcelTabHasColumnHeadings = true;
                //excelFormatOpts.ExportPageHeadersAndFooters = true; // Critical for headers
                //excelFormatOpts.ExportPageArea = ExportPageAreaKind.All; // Explicitly include headers/footers
                excelFormatOpts.UsePageRange = true;



                // Configure export options
                ExportOptions exportOpts = crDocument.ExportOptions;
                exportOpts.ExportFormatType = ExportFormatType.ExcelWorkbook;
                exportOpts.FormatOptions = excelFormatOpts;

                // Export directly to the HTTP response
                crDocument.ExportToHttpResponse(
                    exportOpts.ExportFormatType,
                    HttpContext.Current.Response,
                    true,
                    reportName.Replace(".", "").Replace("RPT", "").Replace("rpt", "") + DateTime.Now.ToString("dd_MMM_yyyy_hhmmsszzz")
                );

                crDocument.Dispose();
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
            }
        }

        public static void ExportExcelReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters)
        {
            try
            {

                ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);
                crDocument.SetDataSource(dataSource);
                
                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);
                }
            
                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.xlsx", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;                
                /* new */
                excelFormatOpts.ExcelUseConstantColumnWidth = true;
                excelFormatOpts.ShowGridLines = true;
                excelFormatOpts.ExcelTabHasColumnHeadings = true;
                excelFormatOpts.ExcelAreaGroupNumber = 1;              
                excelFormatOpts.UsePageRange = true;
                excelFormatOpts.ExportPageHeadersAndFooters = ExportPageAreaKind.OncePerReport;
                /*end new */            
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.ExcelWorkbook;
               
                crDocument.Export();               
                crDocument.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, HttpContext.Current.Response, true, reportName.Replace(".", "").Replace("RPT", "").Replace("rpt", "") + DateTime.Now.ToString("dd_MMM_yyyy_hhmmsszzz"));
                crDocument.Dispose();
                crDocument.Close();
                HttpContext.Current.Response.End();
                System.IO.File.Delete(strFName);
            }
            catch (Exception ex)
            {


            }

        }
        public static void MyPrintReport(string reportName, DataTable dataSource, DataTable subDataSource, Dictionary<string, object> parameters)
        {
            try
            {

                ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);
                
                crDocument.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);

                }
                crDocument.OpenSubreport("rpt_acc_cashbook_bank.rpt").SetDataSource(subDataSource);
                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDocument.Export();
                crDocument.Dispose();
                crDocument.Close();
                //Response.ClearContent();
                // Response.ClearHeaders();
                // Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", strFName));
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();
                // Response.Close();
                System.IO.File.Delete(strFName);

            }
            catch (Exception ex)
            {


            }

        }

        public static void PrintWithSubReport_Gbanker(string reportName, DataTable dataSource, Dictionary<string, object> parameters, Dictionary<string, DataTable> subReportDatasources, ReportClass reportClass)
        {
            try
            {


                // ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                reportClass.Load(strReportPathAbsolute);
                reportClass.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    reportClass.SetParameterValue(kvp.Key, kvp.Value);

                }
                if (subReportDatasources != null)
                {
                    foreach (KeyValuePair<string, DataTable> kvp in subReportDatasources)
                    {
                        //string strSReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + kvp.Key);
                        //crDocument.OpenSubreport(strSReportPathAbsolute).SetDataSource(kvp.Value);
                        reportClass.OpenSubreport(kvp.Key).SetDataSource(kvp.Value);
                    }
                }

                ///strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.xls", Guid.NewGuid());
                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = reportClass.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                ///crExportOptions.ExportFormatType = ExportFormatType.Excel;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                reportClass.Export();
                reportClass.Dispose();
                reportClass.Close();
                //Response.ClearContent();
                // Response.ClearHeaders();
                // Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", strFName));
                ///HttpContext.Current.Response.ContentType = "application/xls";
                //mahi
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();
                // Response.Close();
                System.IO.File.Delete(strFName);
                reportClass.Close();
                reportClass.Dispose();

            }
            catch (Exception ex)
            {
                StringBuilder s = new StringBuilder();
                while (ex != null)
                {
                    s.AppendLine("Exception type: " + ex.GetType().FullName);
                    s.AppendLine("Message       : " + ex.Message);
                    s.AppendLine("Stacktrace:");
                    s.AppendLine(ex.StackTrace);
                    s.AppendLine();
                    ex = ex.InnerException;
                }
            }
        }

        public static void PrintWithSubReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters, Dictionary<string, DataTable>  subReportDatasources)
        {
            try
            {

                ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);
                crDocument.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value); 

                }
                if (subReportDatasources != null)
                {
                    foreach (KeyValuePair<string, DataTable> kvp in subReportDatasources)
                    {
                        //string strSReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + kvp.Key);
                        //crDocument.OpenSubreport(strSReportPathAbsolute).SetDataSource(kvp.Value);
                        crDocument.OpenSubreport(kvp.Key).SetDataSource(kvp.Value);
                    }
                }
               
                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDocument.Export();
                crDocument.Dispose();
                crDocument.Close();
                //Response.ClearContent();
                // Response.ClearHeaders();
                // Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", strFName));
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();
                // Response.Close();
                System.IO.File.Delete(strFName);

            }
            catch (Exception ex)
            {


            }

        }

        public static void PrintWithSubReportDoc(string reportName, DataTable dataSource, Dictionary<string, object> parameters, Dictionary<string, DataTable> subReportDatasources)
        {
            try
            {

                ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);
                crDocument.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);

                }
                if (subReportDatasources != null)
                {
                    foreach (KeyValuePair<string, DataTable> kvp in subReportDatasources)
                    {
                        //string strSReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + kvp.Key);
                        //crDocument.OpenSubreport(strSReportPathAbsolute).SetDataSource(kvp.Value);
                        crDocument.OpenSubreport(kvp.Key).SetDataSource(kvp.Value);
                    }
                }

                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.doc", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.WordForWindows;
                crDocument.Export();
                crDocument.ExportToHttpResponse(ExportFormatType.WordForWindows, HttpContext.Current.Response, true, reportName.Replace(".", "").Replace("RPT", "").Replace("rpt", "") + DateTime.Now.ToString("dd_MMM_yyyy_hhmmsszzz"));
                crDocument.Dispose();
                crDocument.Close();


                //Response.ClearContent();
                // Response.ClearHeaders();
                // Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", strFName));
                HttpContext.Current.Response.ContentType = "application/msword";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();
                // Response.Close();
                System.IO.File.Delete(strFName);

            }
            catch (Exception ex)
            {


            }

        }

        public static void PrintTimeKeepingWithSubReport(string reportName, IEnumerable<TimeKeepingReportModel> dataSource,
            Dictionary<string, object> parameters, 
            Dictionary<string, DataTable> subReportDatasources)
        {
            try
            {

                ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);
                crDocument.SetDataSource(dataSource);
                

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);
                }
                if (subReportDatasources != null)
                {
                    foreach (KeyValuePair<string, DataTable> kvp in subReportDatasources)
                    {                      
                        crDocument.OpenSubreport(kvp.Key).SetDataSource(kvp.Value);
                    }
                }

                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDocument.Export();
                crDocument.Dispose();
                crDocument.Close();               
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();               
                System.IO.File.Delete(strFName);

            }
            catch (Exception ex)
            {

            }
        }


        public static void PrintWithSubReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters, Dictionary<string, DataTable> subReportDatasources, ReportClass reportClass)
        {
            try
            {
                

               // ReportDocument crDocument = new ReportDocument();                

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                reportClass.Load(strReportPathAbsolute);
                reportClass.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    reportClass.SetParameterValue(kvp.Key, kvp.Value);

                }
                if (subReportDatasources != null)
                {
                    foreach (KeyValuePair<string, DataTable> kvp in subReportDatasources)
                    {
                        //string strSReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + kvp.Key);
                        //crDocument.OpenSubreport(strSReportPathAbsolute).SetDataSource(kvp.Value);
                        reportClass.OpenSubreport(kvp.Key).SetDataSource(kvp.Value);
                    }
                }

                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = reportClass.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                reportClass.Export();
                //Response.ClearContent();
                // Response.ClearHeaders();
                // Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", strFName));
                reportClass.Close();
                reportClass.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();
                // Response.Close();
      

                System.IO.File.Delete(strFName);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static void ExportExcelWithSubReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters, Dictionary<string, DataTable> subReportDatasources, ReportClass reportClass)
        {
            try
            {
                ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);                
                crDocument.Load(strReportPathAbsolute);
                crDocument.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);
                }
                if (subReportDatasources != null)
                {
                    foreach (KeyValuePair<string, DataTable> kvp in subReportDatasources)
                    {
                        crDocument.OpenSubreport(kvp.Key).SetDataSource(kvp.Value);
                    }
                }                
                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.xlsx", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;
                
                excelFormatOpts.ExcelUseConstantColumnWidth = true;
                excelFormatOpts.ShowGridLines = true;
                excelFormatOpts.ExcelTabHasColumnHeadings = true;
                excelFormatOpts.ExcelAreaGroupNumber = 1;
                excelFormatOpts.UsePageRange = true;
               
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.ExcelWorkbook;                
                
                crDocument.Export();
                crDocument.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, HttpContext.Current.Response, true, reportName.Replace(".", "").Replace("RPT", "").Replace("rpt", "") + DateTime.Now.ToString("dd_MMM_yyyy_hhmmsszzz"));
                crDocument.Dispose();
                crDocument.Close();
                HttpContext.Current.Response.End();
                System.IO.File.Delete(strFName);
            }
            catch (Exception ex)
            {


            }
        }
        
        public static void ExportExcelWithSubReport(string reportName, IEnumerable<TimeKeepingReportModel> dataSource, Dictionary<string, object> parameters, Dictionary<string, DataTable> subReportDatasources, ReportClass reportClass)
        {
            try
            {
                ReportDocument crDocument = new ReportDocument();
                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                //reportClass.Load(strReportPathAbsolute);
                //reportClass.SetDataSource(dataSource);
                crDocument.Load(strReportPathAbsolute);
                crDocument.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);

                }
                if (subReportDatasources != null)
                {
                    foreach (KeyValuePair<string, DataTable> kvp in subReportDatasources)
                    {
                        crDocument.OpenSubreport(kvp.Key).SetDataSource(kvp.Value);
                    }
                }

                //strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                //crDiskFileDestination.DiskFileName = strFName;
                //crExportOptions = reportClass.ExportOptions;
                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.xlsx", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;

                /* new */
                excelFormatOpts.ExcelUseConstantColumnWidth = true;
                excelFormatOpts.ShowGridLines = true;
                excelFormatOpts.ExcelTabHasColumnHeadings = true;
                excelFormatOpts.ExcelAreaGroupNumber = 1;
                excelFormatOpts.UsePageRange = true;
                /*end new */

                //crExportOptions.DestinationOptions = crDiskFileDestination;
                //crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //crExportOptions.ExportFormatType = ExportFormatType.Excel;
                //reportClass.Export();
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.ExcelWorkbook;

                //reportClass.ExportToHttpResponse(ExportFormatType.Excel, HttpContext.Current.Response, true, strFName);
                //reportClass.Dispose();
                //reportClass.Close();
                //HttpContext.Current.Response.End();
                //System.IO.File.Delete(strFName);
                crDocument.Export();
                crDocument.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, HttpContext.Current.Response, true, reportName.Replace(".", "").Replace("RPT", "").Replace("rpt", "") + DateTime.Now.ToString("dd_MMM_yyyy_hhmmsszzz"));
                crDocument.Dispose();
                crDocument.Close();
                HttpContext.Current.Response.End();
                System.IO.File.Delete(strFName);

            }
            catch (Exception ex)
            {


            }

        }

        public static void PrintWithSubReportParameter(string reportName, DataTable dataSource, Dictionary<string, object> parameters, Dictionary<string, DataTable> subReportDatasources, ReportClass reportClass)
        {
            try
            {


                // ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                reportClass.Load(strReportPathAbsolute);
                reportClass.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    //reportClass.SetParameterValue(kvp.Key, kvp.Value);
                    reportClass.SetParameterValue(kvp.Key, kvp.Value);
                    

                }
                if (subReportDatasources != null)
                {
                    foreach (KeyValuePair<string, DataTable> kvp in subReportDatasources)
                    {
                        //string strSReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + kvp.Key);
                        //crDocument.OpenSubreport(strSReportPathAbsolute).SetDataSource(kvp.Value);
                        reportClass.OpenSubreport(kvp.Key).SetDataSource(kvp.Value);
                    }
                }

                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = reportClass.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                reportClass.Export();
                //Response.ClearContent();
                // Response.ClearHeaders();
                // Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", strFName));
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();
                // Response.Close();
                System.IO.File.Delete(strFName);

            }
            catch (Exception ex)
            {


            }

        }


        public static void PrintWithSubReportParameter2(string reportName, DataTable dataSource, Dictionary<string, object> parameters, Dictionary<string, DataTable> subReportDatasources, ReportClass reportClass)
        {
            try
            {
                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;

                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                reportClass.Load(strReportPathAbsolute);
                reportClass.SetDataSource(dataSource);

                // Set parameters
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> kvp in parameters)
                    {
                        reportClass.SetParameterValue(kvp.Key, kvp.Value);
                    }
                }
                else
                {
                    // Log or handle missing parameters
                    System.Diagnostics.Debug.WriteLine("Parameters dictionary is null.");
                }

                // Set sub-report data sources
                //if (subReportDatasources != null)
                //{
                //    foreach (KeyValuePair<string, DataTable> kvp in subReportDatasources)
                //    {
                //        // Ensure the sub-report exists
                //        reportClass.OpenSubreport(kvp.Key)?.SetDataSource(kvp.Value);
                //    }
                //}

                // Export report
                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = reportClass.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                reportClass.Export();

                // Send file to response
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();

                // Clean up
                System.IO.File.Delete(strFName);
            }
            catch (Exception ex)
            {
                // Log exception details
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                // Optionally rethrow or handle the exception
                throw;
            }
        }

    }
}