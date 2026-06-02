#region Usings

using System.Data;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using gHRM.Web.ViewModels;
using gHRM.Core.Filters.Employee;
using gHRM.Core.Utilities;
using gHRM.Web.Infrastucture.Utility;
using Microsoft.Reporting.WebForms;
using System.Threading.Tasks;
using System.IO;
using ZXing;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Ajax.Utilities;
using System.Globalization;
using gHRM.Web.CommonDropdown;
using System.Text;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using Kendo.Mvc.UI;
using gHRM.Web.ViewModels.Loan;
using Kendo.Mvc.Extensions;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service.Payroll;
using System.Web;
using gHRM.Web.ViewModels.FeedBack;
using gHRM.Web.Reports.Payroll;

#endregion

namespace gHRM.Web.Controllers
{
    public class EmployeeReportController : BaseController
    {
        #region Private Methods

        private readonly IEmployeeDocumentService employeeDocumentService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSpService;
        private readonly IOfficeService officeService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IEmployeeStatusService employeeStatusService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IEmployeeReportOptionService employeeReportOptionService;
        private readonly IEmployeeDesignationService employeeDesignationService;
        private readonly IEmployementTypeService employementTypeService;
        private readonly IEmployeeTrainingService employeeTrainingService;
        private readonly IEmployeeTranningDropDownService employeeTranningDropDownService;
        private readonly IEducationDegreeService educationDegreeService;        
        private readonly IView_EmployeeSalaryConfigurationService viewSalaryConfigurationService;
        private readonly IEmployeePromotionService employeePromotionService;


        public CommonDynamicDropDown commonDynamicDropDown;

        #endregion

        #region Ctor

        public EmployeeReportController(
            IEmployeeService employeeService,
            IEmployeeSPService employeeSpService,
            IOfficeService officeService,
            IOfficeTypeService officeTypeService,
            IEmployeeStatusService employeeStatusService,
            IEmployeeDepartmentService employeeDepartmentService,
            IEmployeeReportOptionService employeeReportOptionService,
            IEmployeeDesignationService employeeDesignationService,
            IEmployementTypeService employementTypeService,
            IEmployeeTrainingService employeeTrainingService,
            IEmployeeDocumentService employeeDocumentService,
            IEmployeeTranningDropDownService employeeTranningDropDownService,
            IEducationDegreeService educationDegreeService,
            IView_EmployeeSalaryConfigurationService viewSalaryConfigurationService,
            IEmployeePromotionService employeePromotionService

            )
        {
            this.employeeDocumentService = employeeDocumentService;
            this.employeeService = employeeService;
            this.employeeSpService = employeeSpService;
            this.officeService = officeService;
            this.officeTypeService = officeTypeService;
            this.employeeStatusService = employeeStatusService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.employeeReportOptionService = employeeReportOptionService;
            this.employeeDesignationService = employeeDesignationService;
            this.employementTypeService = employementTypeService;
            this.employeeTrainingService = employeeTrainingService;
            this.employeeTranningDropDownService = employeeTranningDropDownService;
            this.educationDegreeService = educationDegreeService;
            this.viewSalaryConfigurationService = viewSalaryConfigurationService;
            this.employeePromotionService = employeePromotionService;
            commonDynamicDropDown = new CommonDynamicDropDown();


        }
        #endregion

        #region Events

        public ActionResult SalaryDetails()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SaveIncrementAmount(int? Id, string EmployeeCode, decimal IncrementAmount, string IncrementAmountDate, decimal CurrentSalary, decimal TotalIncrement)
        {
            var result = 0;
            var data = "";
            try
            {
                if (SessionHelper.LoggedInEmployeeID == null)
                {
                    return Json(new { result = 0, Message = "Session expired. Please log in again.", data = "" }, JsonRequestBehavior.AllowGet);
                }

                DateTime parsedDate = DateTime.ParseExact(IncrementAmountDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                var param = new
                {
                    Id = Id ?? 0,
                    EmployeeCode = EmployeeCode,
                    IncrementAmount = IncrementAmount,
                    IncrementAmountDate = parsedDate,
                    CurrentSalary = CurrentSalary,
                    TotalIncrement = TotalIncrement,
                    CreateBy = SessionHelper.LoggedInEmployeeID
                };

                var empList = employeeSpService.GetDataWithParameter(param, "sp_SaveOrUpdateEmployeeSalaryIncrement");

                result = 1;
                data = "1";
            }
            catch (Exception ex)
            {
                return Json(new { result = 0, Message = ex.Message, data = "" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetIncrementList()
        {
            try
            {
                var list = new List<EmployeeSalaryIncrementViewModel>();

                var result = employeeSpService.GetDataWithoutParameter("sp_GetEmployeeSalaryIncrementList");

                list = result.Tables[0].AsEnumerable().Select(row => new EmployeeSalaryIncrementViewModel
                {
                    Id = Convert.ToInt64(row["Id"]),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    IncrementAmount = row.IsNull("IncrementAmount") ? 0 : row.Field<decimal>("IncrementAmount"),
                    IncrementAmountDate = row.Field<string>("IncrementAmountDate"),
                    CurrentSalary = row.IsNull("CurrentSalary") ? 0 : row.Field<decimal>("CurrentSalary"),
                    TotalIncrement = row.IsNull("TotalIncrement") ? 0 : row.Field<decimal>("TotalIncrement"),
                    OfficeName = row.Field<string>("OfficeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("DesignationName"),
                    GrossSalary = row.Field<decimal>("GrossSalary")
                }).ToList();

                return Json(new { data = list, total = list.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<EmployeeSalaryIncrementViewModel>(), total = 0, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult UpdateIncrementAmount(int Id, string EmployeeCode, decimal IncrementAmount, string IncrementAmountDate, decimal CurrentSalary, decimal TotalIncrement)
        {
            try
            {
                if (SessionHelper.LoggedInEmployeeID == null)
                {
                    return Json(new { result = 0, Message = "Session expired. Please log in again." }, JsonRequestBehavior.AllowGet);
                }

                DateTime parsedDate = DateTime.ParseExact(IncrementAmountDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                var param = new
                {
                    Id = Id,
                    EmployeeCode = EmployeeCode,
                    IncrementAmount = IncrementAmount,
                    IncrementAmountDate = parsedDate,
                    CurrentSalary = CurrentSalary,
                    TotalIncrement = TotalIncrement,
                    ModifiedBy = SessionHelper.LoggedInEmployeeID
                };

                var result = employeeSpService.GetDataWithParameter(param, "sp_UpdateEmployeeSalaryIncrement");

                return Json(new { result = 1, Message = "Updated successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = 0, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult DeleteIncrement(long Id)
        {
            var param = new { Id = Id };
            var result = 0;
            var data = "";
            try
            {
                var empList = employeeSpService.GetDataWithParameter(param, "sp_DeleteEmployeeSalaryIncrement");

                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }




        public ActionResult FinalPaymentDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SavePFGratuit(string PFAmount, string EmployeeCode, string PFPaymentDate, string GratuityAmount, string GratuityPaymentDate, string ResignationEffectiveDate)

        {
            var result = 0;
            var data = "";
            try
            {
                // Get form values 
                EmployeeCode = EmployeeCode;
                PFAmount = PFAmount;
                PFPaymentDate = PFPaymentDate;
                GratuityAmount = GratuityAmount;
                GratuityPaymentDate = GratuityPaymentDate;
                ResignationEffectiveDate = ResignationEffectiveDate;


                // Validate required fields
                if (string.IsNullOrWhiteSpace(EmployeeCode))
                    return Json(new { result = 0, message = "Employee ID is required", data = "" }, JsonRequestBehavior.AllowGet);

                if (string.IsNullOrWhiteSpace(PFAmount) && string.IsNullOrWhiteSpace(GratuityAmount))
                    return Json(new { result = 0, message = "At least one payment amount is required", data = "" }, JsonRequestBehavior.AllowGet);

                // Parse dates
                DateTime? parsedPfDate = null;
                DateTime? parsedGratuityDate = null;
                DateTime? parsedResignationDate = null;
                if (!string.IsNullOrWhiteSpace(PFPaymentDate))
                {
                    if (DateTime.TryParseExact(PFPaymentDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime pfDate))
                        parsedPfDate = pfDate;
                    else
                        return Json(new { result = 0, message = "Invalid PF Payment Date format", data = "" }, JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrWhiteSpace(GratuityPaymentDate))
                {
                    if (DateTime.TryParseExact(GratuityPaymentDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime gratuityDate))
                        parsedGratuityDate = gratuityDate;
                    else
                        return Json(new { result = 0, message = "Invalid Gratuity Payment Date format", data = "" }, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrWhiteSpace(ResignationEffectiveDate))
                {
                    if (DateTime.TryParseExact(ResignationEffectiveDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resignationDate))
                        parsedResignationDate = resignationDate;
                    else
                        return Json(new { result = 0, message = "Invalid Gratuity Payment Date format", data = "" }, JsonRequestBehavior.AllowGet);
                }
                var param = new
                {
                    EmployeeCode = EmployeeCode,
                    PFAmount = string.IsNullOrEmpty(PFAmount) ? (decimal?)null : decimal.Parse(PFAmount),
                    PFPaymentDate = parsedPfDate,
                    GratuityAmount = string.IsNullOrEmpty(GratuityAmount) ? (decimal?)null : decimal.Parse(GratuityAmount),
                    GratuityPaymentDate = parsedGratuityDate,
                    ResignationEffectiveDate = parsedResignationDate,
                    UpdateUser = LoggedInEmployeeId.ToString(),
                    UpdateDate = DateTime.Now
                };

                // Call stored procedure
                var saveResult = employeeSpService.GetDataWithParameter(param, "SP_SavePFGratuity");
                result = 1;
                data = "1"; // or assign ID/confirmation from saveResult if needed
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, message = "Error saving payment: " + ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, message = "Payment saved successfully!", data = data }, JsonRequestBehavior.AllowGet);
        }

      
        public ActionResult GetPFGratuityList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<dynamic> gratuityList = new List<dynamic>();

                var param = new { EmployeeId = LoggedInEmployeeId }; // Adjust if needed
                var result = employeeSpService.GetDataWithParameter(param, "SP_GetPFGratuityList");

                if (result != null && result.Tables.Count > 0)
                {
                    gratuityList = result.Tables[0].AsEnumerable()
                        .Select(row => new
                        {
                            SL = row.Field<long>("SL"),
                            ID = row.Field<int>("Id"),
                            EmployeeCode = row.Field<string>("EmployeeCode"),
                            EmployeeName = row.Field<string>("EmployeeName"),
                            PFAmount = row.Field<decimal?>("PFAmount"),
                            PFPaymentDate = row.Field<string>("PFPaymentDate"),
                            GratuityAmount = row.Field<decimal?>("GratuityAmount"),
                            GratuityPaymentDate = row.Field<string>("GratuityPaymentDate"),
                            ResignationEffectiveDate = row.Field<string>("ResignationEffectiveDate"),
                            FirstJoinDate = row.Field<string>("FirstJoinDate"),
                            DesignationName = row.Field<string>("DesignationName")
                        }).ToList<dynamic>();
                }

                DataSourceResult dsResult = gratuityList.ToDataSourceResult(request);
                return Json(new { data = dsResult.Data, total = dsResult.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = 0, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // YPSA Report 
        public ActionResult FinalPaymentReport_YPSA( string DateFrom, string DateTo)
        {
            try
            {              
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateFrom", Value = DateFrom });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateTo", Value = DateTo });

             
                PrintSSRSReport("/gHRMPlus_Reports/FinalpaymentDetailsYPSA", paramValues.ToArray());  /// 31
            

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult SalaryIncrementReport_YPSA(string DateFrom, string DateTo)
        {
            try
            {
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateFrom", Value = DateFrom });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateTo", Value = DateTo });


                PrintSSRSReport("/gHRMPlus_Reports/SalaryIncrementReport_YPSA", paramValues.ToArray());  /// 31


                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }



        public ActionResult CooperativeLedgerReport()
        {
            return View();
        }

        public ActionResult CooperativeLedgerReportView(string FromDate, string ToDate, string EmployeeCode)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = FromDate });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "ToDate", Value = ToDate });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = EmployeeCode });

               // PrintSSRSMultiformat("excel", "/gHRMPlus_Reports/CooperativeLedgerReport", paramValues.ToArray());
                PrintSSRSReport("/gHRMPlus_Reports/CooperativeLedgerReport", paramValues.ToArray());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        [HttpPost]
        public JsonResult CVFileUpload()
        {
            string result = "OK";
            try
            {

                string EmployeeCode = Request.Form["EmployeeCode"].ToString();

                DateTime dt = DateTime.Now;
                string uploadDay = dt.Day + "-" + dt.Month + "-" + dt.Year;
                uploadDay = "CV_" + uploadDay + "_";
                string fname;
                var path = "";
                var fileType = "";
                // Checking no of files injected in Request object  
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        //  Get all files from Request object  
                        HttpFileCollectionBase files = Request.Files;
                        for (int i = 0; i < files.Count; i++)
                        {
                            //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                            //string filename = Path.GetFileName(Request.Files[i].FileName);  

                            HttpPostedFileBase file = files[i];


                            // Checking for Internet Explorer  
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fname = file.FileName;
                            }

                            // Get the complete folder path and store the file inside it.
                            // 
                            var fileName = Path.GetFileName(fname);
                             fileType = Path.GetFileName(file.ContentType);

                            //var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);//E:\Project\UploadedFile
                            path = Path.Combine(@"E:\IIS\ghrm\GC\UPLOADCV\", uploadDay + fileName);

                            //fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                            file.SaveAs(path);
                        }
                        // Returns message that successfully uploaded  
                        // return Json("File Uploaded Successfully!");
                    }
                    catch (Exception ex)
                    {
                        return Json("Error occurred. Error details: " + ex.Message);
                    }
                }
                else
                {
                    // return Json("No files selected.");
                }


                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                DateTime UpdateDate = DateTime.Now;
                //[SP_FeedBackRegUpdate](@FeedbackCategoryID int,@IsChecked bit ,@IsSolved bit, @SolvedBy varchar(100), @SolvedDate datetime, @UpdateUser varchar(100), @UpdateDate datetime)
                var param = new {  UpdateDate = UpdateDate, FilePath = path, EmployeeCode = EmployeeCode, fileType= fileType };
                var val = employeeSpService.GetDataWithParameter(param, "SP_CVUPLOAD");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExperienceFileUpload()
        {
            string result = "OK";
            try
            {

                string EmployeeCode = Request.Form["EmployeeCode"].ToString();

                DateTime dt = DateTime.Now;
                string uploadDay = dt.Day + "-" + dt.Month + "-" + dt.Year;
                uploadDay = "EXP_" + uploadDay + "_";
                string fname;
                var path = "";
                var fileType = "";
                // Checking no of files injected in Request object  
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        //  Get all files from Request object  
                        HttpFileCollectionBase files = Request.Files;
                        for (int i = 0; i < files.Count; i++)
                        {
                            //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                            //string filename = Path.GetFileName(Request.Files[i].FileName);  

                            HttpPostedFileBase file = files[i];


                            // Checking for Internet Explorer  
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fname = file.FileName;
                            }

                            // Get the complete folder path and store the file inside it.
                            // 
                            var fileName = Path.GetFileName(fname);
                            fileType = Path.GetFileName(file.ContentType);

                            //var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);//E:\Project\UploadedFile
                            path = Path.Combine(@"E:\IIS\ghrm\GC\UPLOADEXP\", uploadDay + fileName);

                            //fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                            file.SaveAs(path);
                        }
                        // Returns message that successfully uploaded  
                        // return Json("File Uploaded Successfully!");
                    }
                    catch (Exception ex)
                    {
                        return Json("Error occurred. Error details: " + ex.Message);
                    }
                }
                else
                {
                    // return Json("No files selected.");
                }


                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString());
                DateTime UpdateDate = DateTime.Now;
                //[SP_FeedBackRegUpdate](@FeedbackCategoryID int,@IsChecked bit ,@IsSolved bit, @SolvedBy varchar(100), @SolvedDate datetime, @UpdateUser varchar(100), @UpdateDate datetime)
                var param = new { UpdateDate = UpdateDate, FilePath = path, EmployeeCode = EmployeeCode, fileType = fileType };
                var val = employeeSpService.GetDataWithParameter(param, "SP_EXPUPLOAD");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult IncrementLetterReport(int id)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "id", Value = id });

                PrintSSRSReport("/gHRMPlus_Reports/IncrementLetterReport_GC", paramValues.ToArray());
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Content("<b>error</b><br />" + ex.Message);
                // return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult DeleteIncrementLetter(int Id)
        {
            var param = new { Id = Id };
            var result = 0;
            var data = "";
            try
            {
                var empList = employeeSpService.GetDataWithParameter(param, "SP_Delete_IncrementLetter");

                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPurposeNameList()
        {
            List<PurposeNameForPayment> List_Employee = new List<PurposeNameForPayment>();

            var param = new { ID = LoggedInEmployeeId }; 
            var empList = employeeSpService.GetDataWithParameter(param,  "prl.SP_GET_PURPOSENAMELIST");

            if (empList.Tables[0].Rows.Count > 0)
            {
                List_Employee = empList.Tables[0].AsEnumerable()
                    .Select(row => new PurposeNameForPayment
                    {
                        Id = row.Field<int>("Id"),
                        Name = row.Field<string>("Name"),
                    }).ToList();
            }
            else
            {
                // Consider handling the case where there is no data differently.
                Response.StatusCode = 403;
                return Json(new { Message = "No data found" });
            }

            var secList = new List<SelectListItem>();
            secList.Add(new SelectListItem() { Text = "Please Select", Value = "" });

            foreach (var item in List_Employee)
            {
                secList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            return Json(secList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetBankList()
        {
            List<PurposeNameForPayment> List_Employee = new List<PurposeNameForPayment>();

            var empList = employeeSpService.GetDataWithoutParameter("prl.SP_GET_BANKLIST");

            if (empList.Tables[0].Rows.Count > 0)
            {
                List_Employee = empList.Tables[0].AsEnumerable()
                    .Select(row => new PurposeNameForPayment
                    {
                        Id = row.Field<int>("Id"),
                        Name = row.Field<string>("Name"),
                    }).ToList();
            }
            else
            {
                // Consider handling the case where there is no data differently.
                Response.StatusCode = 403;
                return Json(new { Message = "No data found" });
            }

            var secList = new List<SelectListItem>();
            secList.Add(new SelectListItem() { Text = "Please Select", Value = "" });

            foreach (var item in List_Employee)
            {
                secList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            return Json(secList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeIncrementLetter(int Id)
        {
            List<FinalSattlementViewModel> List_Employee = new List<FinalSattlementViewModel>();
            var param = new { Id = Id };
            var empList = employeeSpService.GetDataWithParameter(param, "SP_Get_IncrementLetter");


            if (empList.Tables[0].Rows.Count > 0)
            {
                List_Employee = empList.Tables[0].AsEnumerable()
               .Select(row => new FinalSattlementViewModel
               {
                   EmployeeCode = row.Field<string>("EmployeeCode"),
                   DateOfBirthMsg = row.Field<string>("ReportDate"),

               }).ToList();
            }
            else
            {
                Response.StatusCode = 403;
            }

            return Json(List_Employee.ToList(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult getIncrementLetterList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<EmployeeLoanInstallmentDetailViewModel> List_ViewModel = new List<EmployeeLoanInstallmentDetailViewModel>();


                int Result = 0;


                var loanList = employeeSpService.GetDataWithoutParameter("prl.SP_EmployeeIncrementLetterDetailList");
                List_ViewModel = loanList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeLoanInstallmentDetailViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    LoanId = row.Field<int>("Id"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("OffcDesignName"),
                    LoanStartDateMsg = row.Field<string>("ReportDate"),
                    InstallmentDateMsg = row.Field<string>("CreateDate"),
                    OfficeName = row.Field<string>("OfficeName"),
                }).ToList();


                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult WhatsAppNoSave(string WhatsAppNo, string EmployeeCode, string Email)
        {

            var result = 0;
            var data = "";
            try
            {
                var param = new
                {
                    WhatsAppNo = WhatsAppNo,
                    EmployeeCode = EmployeeCode,
                    Email = Email
                };
                var empList = employeeSpService.GetDataWithParameter(param, "WhatsAppNoSave");
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult IncrementLetterSave(string Date, string EmployeeCode, decimal IncrementAmount, int hiddenStep, decimal GrossSalary, string EffectiveDate)
        {

            var result = 0;
            var data = "";
            try
            {
                var param = new
                {
                    Date = Date,
                    EmployeeCode = EmployeeCode,
                    IncrementAmount = IncrementAmount,
                    hiddenStep = hiddenStep,
                    GrossSalary = GrossSalary,
                    EffectiveDate = EffectiveDate
                };
                var empList = employeeSpService.GetDataWithParameter(param, "IncrementLetterSave");
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IncrementLetter()
        {
            return View();
        }

        /// <incrment>
  


        public ActionResult PromotionLetterReport(int id)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "id", Value = id });

                PrintSSRSReport("/gHRMPlus_Reports/PromotionLetterReport_GC", paramValues.ToArray());
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Content("<b>error</b><br />" + ex.Message);
                // return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult FinalPaymentToBank()
        {            
            return View();
        }

        public ActionResult FinalPaymentToBankAck()
        {
            return View();
        }
        public ActionResult FinalPaymentToBankAckPrinted()
        {
            return View();
        }

        public ActionResult FinalPaymentToBankPrinted()
        {
            return View();
        }

        public ActionResult FinalPaymentToBankReport()
        {
            return View();
        }
        public ActionResult FinalPaymentToBankApprovePrint()
        {
            return View();
        }
        public ActionResult FinalPaymentToBankApprove()
        {
            return View();
        }
        public JsonResult DeletePromotionLetter(int Id)
        {
            var param = new { Id = Id };
            var result = 0;
            var data = "";
            try
            {
                var empList = employeeSpService.GetDataWithParameter(param, "SP_Delete_PromotionLetter");

                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeePromotionLetter(int Id)
        {
            List<FinalSattlementViewModel> List_Employee = new List<FinalSattlementViewModel>();
            var param = new { Id = Id };
            var empList = employeeSpService.GetDataWithParameter(param, "SP_Get_PromotionLetter");


            if (empList.Tables[0].Rows.Count > 0)
            {
                List_Employee = empList.Tables[0].AsEnumerable()
               .Select(row => new FinalSattlementViewModel
               {
                   EmployeeCode = row.Field<string>("EmployeeCode"),
                   DateOfBirthMsg = row.Field<string>("ReportDate"),

               }).ToList();
            }
            else
            {
                Response.StatusCode = 403;
            }

            return Json(List_Employee.ToList(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult geLeaveSummaryList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<EmployeeLoanInstallmentDetailViewModel> List_ViewModel = new List<EmployeeLoanInstallmentDetailViewModel>();


                int Result = 0;


                var loanList = employeeSpService.GetDataWithoutParameter("prl.SP_LeaveDetailList");
                List_ViewModel = loanList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeLoanInstallmentDetailViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    LoanId = row.Field<int>("Id"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("OffcDesignName"),
                    LoanStartDateMsg = row.Field<string>("ReportDate"),
                    InstallmentDateMsg = row.Field<string>("CreateDate"),
                    OfficeName = row.Field<string>("OfficeName"),
                }).ToList();


                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult getDispatchNumberList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<EmployeeLoanInstallmentDetailViewModel> List_ViewModel = new List<EmployeeLoanInstallmentDetailViewModel>();


                int Result = 0;


                var loanList = employeeSpService.GetDataWithoutParameter("prl.SP_DispatchNumberDetailList");
                List_ViewModel = loanList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeLoanInstallmentDetailViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    LoanId = row.Field<int>("Id"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("OffcDesignName"),
                    LoanStartDateMsg = row.Field<string>("ReportDate"),
                    InstallmentDateMsg = row.Field<string>("CreateDate"),
                    OfficeName = row.Field<string>("OfficeName"),
                }).ToList();


                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult getReleaseNumberList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<EmployeeLoanInstallmentDetailViewModel> List_ViewModel = new List<EmployeeLoanInstallmentDetailViewModel>();


                int Result = 0;


                var loanList = employeeSpService.GetDataWithoutParameter("prl.SP_ReleaseNumberDetailList");
                List_ViewModel = loanList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeLoanInstallmentDetailViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    LoanId = row.Field<int>("Id"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("OffcDesignName"),
                    LoanStartDateMsg = row.Field<string>("ReportDate"),
                    InstallmentDateMsg = row.Field<string>("CreateDate"),
                    OfficeName = row.Field<string>("OfficeName"),
                }).ToList();


                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        public ActionResult getPromotionLetterList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<EmployeeLoanInstallmentDetailViewModel> List_ViewModel = new List<EmployeeLoanInstallmentDetailViewModel>();


                int Result = 0;


                var loanList = employeeSpService.GetDataWithoutParameter("prl.SP_EmployeePromotionLetterDetailList");
                List_ViewModel = loanList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeLoanInstallmentDetailViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    LoanId = row.Field<int>("Id"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("OffcDesignName"),
                    LoanStartDateMsg = row.Field<string>("ReportDate"),
                    InstallmentDateMsg = row.Field<string>("CreateDate"),
                    OfficeName = row.Field<string>("OfficeName"),                   
                }).ToList();


                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        public ActionResult PromotionLetterSave(string Date, string EmployeeCode)
        {

            var result = 0;
            var data = "";
            try
            {
                var param = new
                {
                    Date = Date,
                    EmployeeCode = EmployeeCode
                };
                var empList = employeeSpService.GetDataWithParameter(param, "PromotionLetterSave");
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PromotionLetter()
        {
            return View();
        }
        public ActionResult DespatchNumber()
        {
            return View();
        }
        public ActionResult ReleaseOrderNumber()
        {
            return View();
        }

        public ActionResult ServiceBookReport()
        {
            return View();
        }

        public ActionResult DespatchNumberSave(string Date, string EmployeeCode)
        {

            var result = 0;
            var data = "";
            try
            {
                var param = new
                {
                    Date = Date,
                    EmployeeCode = EmployeeCode
                };
                var empList = employeeSpService.GetDataWithParameter(param, "DespatchNumberSave");
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReleaseNumberSave(string Date, string EmployeeCode)
        {

            var result = 0;
            var data = "";
            try
            {
                var param = new
                {
                    Date = Date,
                    EmployeeCode = EmployeeCode
                };
                var empList = employeeSpService.GetDataWithParameter(param, "ReleaseNumberSave");
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }

        // Experience Letter

        public ActionResult WhatsAppNoEmail_Get(string EmployeeCode)
        {
            try
            {
                int roleid = SessionHelper.LoggedInRoleId;
                string current_employeeid = SessionHelper.LoginUserEmployeeId.ToString();


                var param = new
                {
                    EmployeeCode = EmployeeCode
                   
                };
                var empList = employeeSpService.GetDataWithParameter(param, "GetWhatsAppEmail");
                if (empList.Tables[0].Rows.Count > 0)
                {
                    return Json(new { Result = empList.Tables[0].Rows[0]["email"].ToString(), Message = empList.Tables[0].Rows[0]["whatsappno"].ToString() });
                }

                return Json(new { Result = "0", Message = "0" });

            }
            catch (Exception ex)
            {
                //return Content("<b>error</b><br />" + ex.Message);
                return Json(new { Result = "0", Message = "Invalid EmployeeCode!" });
            }
        }

        public ActionResult CVAndExperienceLetter_Get(string EmployeeCode)
        {
            try
            {
                int roleid = SessionHelper.LoggedInRoleId;
                string current_employeeid = SessionHelper.LoginUserEmployeeId.ToString();
                

                //if (roleid == 7)
                //{

                    var param = new
                    {
                        EmployeeCode = EmployeeCode
                        ,current_employeeid = current_employeeid
                    };
                    var empList = employeeSpService.GetDataWithParameter(param, "ZoneCVExp");
                    if(empList.Tables[0].Rows.Count>0)
                    {
                        return Json(new { Result = "1", Message = empList.Tables[0].Rows[0]["roleid"].ToString() });
                    }
                    else
                    {
                       // return Content("Sorry "+EmployeeCode+" is not authorized for this zone!");
                        return Json(new { Result = "0", Message = "Sorry " + EmployeeCode + " is not authorized for this zone!" });
                    }
                    
                //}
                //else
                //{
                //    // return Content("Sorry you are not a zone superviser!");
                //    return Json(new { Result = "0", Message = "Sorry you are not a zone superviser!" });
                //}


                return Json(new { Result = "1", Message = "1" });

            }
            catch (Exception ex)
            {
                //return Content("<b>error</b><br />" + ex.Message);
                return Json(new { Result = "0", Message = "Invalid EmployeeCode!" });
            }
        }

        public ActionResult CVAndExperienceLetter()
        {

            return View();
        }

        public ActionResult CVDownload()
        {

            return View();
        }



        public ActionResult ExperienceLetter()
        {
            return View();
        }


        public ActionResult ExperienceLetterReport2(int id)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "id", Value = id });

                //var param = new
                //{
                //    id = id
                //};
                //var empList = employeeSpService.GetDataWithParameter(param, "ExperienceReport");
                //var zonal = empList.Tables[0].Rows[0]["AreaName"].ToString();
                //if (zonal.ToLower().Contains("zonal"))
                //{
                //    PrintSSRSReport("/gHRMPlus_Reports/ExperienceReport_GC_ZONE", paramValues.ToArray());
                //}
                //else
                //{
                PrintSSRSReport("/gHRMPlus_Reports/EmployeeBioDataTemplate", paramValues.ToArray());
                //}


                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Content("<b>error</b><br />" + ex.Message);
                // return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult ExperienceLetterReport3(int id)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "id", Value = id });

                //var param = new
                //{
                //    id = id
                //};
                //var empList = employeeSpService.GetDataWithParameter(param, "ExperienceReport");
                //var zonal = empList.Tables[0].Rows[0]["AreaName"].ToString();
                //if (zonal.ToLower().Contains("zonal"))
                //{
                //    PrintSSRSReport("/gHRMPlus_Reports/ExperienceReport_GC_ZONE", paramValues.ToArray());
                //}
                //else
                //{
                PrintSSRSReport("/gHRMPlus_Reports/EmployeeBioDataTemplate", paramValues.ToArray());
                //}


                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Content("<b>error</b><br />" + ex.Message);
                // return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult FinalPaymentVoucherReport(int id)
        {
            try
            {

                var reportParam = new Dictionary<string, object>();
                var organizationName = SessionHelper.CompanyCode;

                var canShowMDSignature = (organizationName == GHRMPlusCompanyConstants.GrameenCommunications);
                reportParam.Add("CanShowMDSignature", canShowMDSignature);
                var param = new
                {
                    id = id,
                    loginID = SessionHelper.LoginUserEmployeeId
                };
              
                if (SessionHelper.CompanyCode == GHRMPlusCompanyConstants.GrameenCommunications)
                {
                    var Data = employeeSpService.GetDataWithParameter( param , "[prl].[FINALPAYMENTVOUCHER]");

                    ReportHelper.PrintReport("payroll/rpt_FinalPaymentAcknowledgement.rpt", Data.Tables[0], reportParam);
                }        

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult FinalPaymentChequeReport(int id)
        {
            try
            {

                var reportParam = new Dictionary<string, object>();
          
                var param = new
                {
                    id = id,
                    loginID = SessionHelper.LoginUserEmployeeId
                };
               
                if (SessionHelper.CompanyCode == GHRMPlusCompanyConstants.GrameenCommunications)
                {                

                    var Data2 = employeeSpService.GetDataWithParameter(param, "[prl].[SP_ChequePrint]");

                    if (SessionHelper.LoginUserEmployeeId == 75)
                    {
                        ReportHelper.PrintReport("payroll/rpt_ChequePrint.rpt", Data2.Tables[0], reportParam);
                    }
                    else
                    {
                        if (Data2.Tables[0].Rows[0]["BankName"].ToString().Trim() == "Dutch Bangla Bank Ltd.")
                            ReportHelper.PrintReport("payroll/rpt_ChequePrint_DBBL.rpt", Data2.Tables[0], reportParam);
                        else if (Data2.Tables[0].Rows[0]["BankName"].ToString().Trim() == "Standard Bank PLC")
                            ReportHelper.PrintReport("payroll/rpt_ChequePrint_SBL.rpt", Data2.Tables[0], reportParam);
                        else
                            ReportHelper.PrintReport("payroll/rpt_ChequePrint.rpt", Data2.Tables[0], reportParam);
                    }
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        public ActionResult ExperienceLetterReport(int id)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "id", Value = id });

                var param = new
                {
                    id = id
                };
                var empList = employeeSpService.GetDataWithParameter(param, "ExperienceReport");
                var zonal = empList.Tables[0].Rows[0]["AreaName"].ToString();
                var supervisor = empList.Tables[0].Rows[0]["Responsibility"].ToString();

                if (id == 601)
                {
                    PrintSSRSReport("/gHRMPlus_Reports/ExperienceCertificateReport_GC_Zone2", paramValues.ToArray());
                }
                else
                {

                    if (supervisor.ToLower().Contains("supervisor"))
                        //if (zonal.ToLower().Contains("zonal"))
                        PrintSSRSReport("/gHRMPlus_Reports/ExperienceCertificateReport_GC_Zone", paramValues.ToArray());
                    else
                        PrintSSRSReport("/gHRMPlus_Reports/ExperienceCertificateReport_GC", paramValues.ToArray());

                }
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Content("<b>error</b><br />" + ex.Message);
                // return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetFinalPaymentDataEdit(int Id)
        {
            List<FinalSattlementViewModel> List_Employee = new List<FinalSattlementViewModel>();
            var param = new { Id = Id };
            var empList = employeeSpService.GetDataWithParameter(param, "SP_Get_FinalPaymentEdit");


            if (empList.Tables[0].Rows.Count > 0)
            {
                List_Employee = empList.Tables[0].AsEnumerable()
               .Select(row => new FinalSattlementViewModel
               {
                   EmployeeCode = row.Field<string>("EmployeeCode"),
                   DateOfBirthMsg = row.Field<string>("ChequeNo"),
                   GrossSalary = row.Field<decimal>("Amount"),
                   DesignationId = row.Field<int>("BankId"),
                   OfficeDesignationName = row.Field<string>("PaymentDate"),
                   DepartmentName = row.Field<string>("IssueDate"),
                   SignatureDesignationId = row.Field<int>("PurposeId"),
                   EmployeeName = row.Field<string>("EmployeeName"),
               }).ToList();
            }
            else
            {
                Response.StatusCode = 403;
            }

            return Json(List_Employee.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFinalPaymentDataEdit_YPSA(int Id)
        {
            List<FinalSattlementViewModel> List_Employee = new List<FinalSattlementViewModel>();
            var param = new { Id = Id };
            var empList = employeeSpService.GetDataWithParameter(param, "SP_Get_FinalPaymentEdit_YPSA");


            if (empList.Tables[0].Rows.Count > 0)
            {
                List_Employee = empList.Tables[0].AsEnumerable()
               .Select(row => new FinalSattlementViewModel
               {
                   EmployeeCode = row.Field<string>("EmployeeCode"),
                   BasicSalary = row.Field<decimal>("GratuityAmount"),
                   GrossSalary = row.Field<decimal>("PFAmount"),
                   
                   OfficeDesignationName = row.Field<string>("GratuityPaymentDate"),
                   Date = row.Field<string>("PFPaymentDate"),
                   DepartmentName = row.Field<string>("ResignationEffectiveDate"), 
                   
                   EmployeeName = row.Field<string>("EmployeeName"),
               }).ToList();
            }
            else
            {
                Response.StatusCode = 403;
            }

            return Json(List_Employee.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmployeeExperienceLetter(int Id)
        {
            List<FinalSattlementViewModel> List_Employee = new List<FinalSattlementViewModel>();
            var param = new { Id = Id };
            var empList = employeeSpService.GetDataWithParameter(param, "SP_Get_Experience_Letter");


            if (empList.Tables[0].Rows.Count > 0)
            {
                List_Employee = empList.Tables[0].AsEnumerable()
               .Select(row => new FinalSattlementViewModel
               {
                   EmployeeCode = row.Field<string>("EmployeeCode"),
                   DateOfBirthMsg = row.Field<string>("ReportDate"),

               }).ToList();
            }
            else
            {
                Response.StatusCode = 403;
            }

            return Json(List_Employee.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteFinalPaymentVoucherPrint(int Id)
        {
            var param = new { Id = Id, LoginID = SessionHelper.LoginUserEmployeeId };
            var result = 0;
            var data = "";
            try
            {
                var empList = employeeSpService.GetDataWithParameter(param, "prl.SP_Delete_FinalPaymentVoucherPrint");

                var message = empList.Tables[0].Rows[0]["RESULT"].ToString();

                if (message == "allreadyapproved")
                    result = 0;
                else
                    result = 1;

            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteFinalPaymentVoucher(int Id)
        {
            var param = new { Id = Id, LoginID = SessionHelper.LoginUserEmployeeId };
            var result = 0;
            var data = "";
            try
            {
                var empList = employeeSpService.GetDataWithParameter(param, "prl.SP_Delete_FinalPaymentVoucher");

                var message = empList.Tables[0].Rows[0]["RESULT"].ToString();

                if(message == "allreadyapproved")
                    result = 0;
                else
                    result = 1;

            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteFinalPaymentVoucher_YPSA(int Id)
        {
            var param = new { Id = Id, LoginID = SessionHelper.LoginUserEmployeeId };
            var result = 0;
            var data = "";
            try
            {
                var empList = employeeSpService.GetDataWithParameter(param, "prl.SP_Delete_FinalPaymentVoucher_YPSA");

                var message = empList.Tables[0].Rows[0]["RESULT"].ToString();

                if (message == "allreadyapproved")
                    result = 0;
                else
                    result = 1;

            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteEmployeeExperienceLetter(int Id)
        {
            var param = new { Id = Id };
            var result = 0;
            var data = "";
            try
            {
                var empList = employeeSpService.GetDataWithParameter(param, "SP_Delete_Experience_Letter");

                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getFinalPaymentToBankList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<EmployeeLoanInstallmentDetailViewModel> List_ViewModel = new List<EmployeeLoanInstallmentDetailViewModel>();


                int Result = 0;

                var param = new { ID = LoggedInEmployeeId };
                var loanList = employeeSpService.GetDataWithParameter(param, "prl.SP_FINAL_PAYMENT_TO_BANK_DetailList");
                List_ViewModel = loanList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeLoanInstallmentDetailViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    LoanId = row.Field<int>("Id"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("OffcDesignName"),
                    LoanStartDateMsg = row.Field<string>("PurposeName"),
                    InstallmentDateMsg = row.Field<string>("CreateDate"),
                    OfficeName = row.Field<string>("OfficeName"),
                    InstallmentAmount = row.Field<decimal>("Amount"),
                    LoanEndDateMsg = row.Field<string>("ChequeNo"),
                    LoanStatus = row.Field<string>("BANKFULLNAME"),
                    LoanType = row.Field<string>("IssueDate"),
                    LoanScheme = row.Field<string>("PaymentDate"),
                    //OfficeName = row.Field<string>("OfficeName"),
                    //OfficeName = row.Field<string>("OfficeName"),

                }).ToList();


                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult getFinalPaymentToBankListPrinted([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<EmployeeLoanInstallmentDetailViewModel> List_ViewModel = new List<EmployeeLoanInstallmentDetailViewModel>();


                int Result = 0;

                var param = new { ID = LoggedInEmployeeId };
                var loanList = employeeSpService.GetDataWithParameter(param, "prl.SP_FINAL_PAYMENT_TO_BANK_DetailListPrinted");
                List_ViewModel = loanList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeLoanInstallmentDetailViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    LoanId = row.Field<int>("Id"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("OffcDesignName"),
                    LoanStartDateMsg = row.Field<string>("PurposeName"),
                    InstallmentDateMsg = row.Field<string>("CreateDate"),
                    OfficeName = row.Field<string>("OfficeName"),
                    InstallmentAmount = row.Field<decimal>("Amount"),
                    LoanEndDateMsg = row.Field<string>("ChequeNo"),
                    LoanStatus = row.Field<string>("BANKFULLNAME"),
                    LoanType = row.Field<string>("IssueDate"),
                    LoanScheme = row.Field<string>("PaymentDate"),
                    //OfficeName = row.Field<string>("OfficeName"),
                    //OfficeName = row.Field<string>("OfficeName"),

                }).ToList();


                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult getFinalPaymentToBankAckList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<EmployeeLoanInstallmentDetailViewModel> List_ViewModel = new List<EmployeeLoanInstallmentDetailViewModel>();


                int Result = 0;

                var param = new { ID = LoggedInEmployeeId };
                var loanList = employeeSpService.GetDataWithParameter(param, "prl.SP_FINAL_PAYMENT_TO_BANK_DetailListAck");
                List_ViewModel = loanList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeLoanInstallmentDetailViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    LoanId = row.Field<int>("Id"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("OffcDesignName"),
                    LoanStartDateMsg = row.Field<string>("PurposeName"),
                    InstallmentDateMsg = row.Field<string>("CreateDate"),
                    OfficeName = row.Field<string>("OfficeName"),
                    InstallmentAmount = row.Field<decimal>("Amount"),
                    LoanEndDateMsg = row.Field<string>("ChequeNo"),
                    LoanStatus = row.Field<string>("BANKFULLNAME"),
                    LoanType = row.Field<string>("IssueDate"),
                    LoanScheme = row.Field<string>("PaymentDate"),
                    //OfficeName = row.Field<string>("OfficeName"),
                    //OfficeName = row.Field<string>("OfficeName"),

                }).ToList();


                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult getFinalPaymentToBankAckListPrinted([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<EmployeeLoanInstallmentDetailViewModel> List_ViewModel = new List<EmployeeLoanInstallmentDetailViewModel>();


                int Result = 0;

                var param = new { ID = LoggedInEmployeeId };
                var loanList = employeeSpService.GetDataWithParameter(param, "prl.SP_FINAL_PAYMENT_TO_BANK_DetailListPrintedAck");
                List_ViewModel = loanList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeLoanInstallmentDetailViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    LoanId = row.Field<int>("Id"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("OffcDesignName"),
                    LoanStartDateMsg = row.Field<string>("PurposeName"),
                    InstallmentDateMsg = row.Field<string>("CreateDate"),
                    OfficeName = row.Field<string>("OfficeName"),
                    InstallmentAmount = row.Field<decimal>("Amount"),
                    LoanEndDateMsg = row.Field<string>("ChequeNo"),
                    LoanStatus = row.Field<string>("BANKFULLNAME"),
                    LoanType = row.Field<string>("IssueDate"),
                    LoanScheme = row.Field<string>("PaymentDate"),
                    //OfficeName = row.Field<string>("OfficeName"),
                    //OfficeName = row.Field<string>("OfficeName"),

                }).ToList();


                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult getFinalPaymentToBankListNotApprove([DataSourceRequest] DataSourceRequest request, int Purpose, string fromDate, string toDate)
        {
            try
            {
                List<EmployeeLoanInstallmentDetailViewModel> List_ViewModel = new List<EmployeeLoanInstallmentDetailViewModel>();


                int Result = 0;

                var param = new { purposeid  = Purpose, fromDate = fromDate, toDate = toDate };
                var loanList = employeeSpService.GetDataWithParameter(param, "prl.SP_FINAL_PAYMENT_TO_BANK_DetailList_NotApprove");
                List_ViewModel = loanList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeLoanInstallmentDetailViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    LoanId = row.Field<int>("Id"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("OffcDesignName"),
                    LoanStartDateMsg = row.Field<string>("PurposeName"),
                    InstallmentDateMsg = row.Field<string>("CreateDate"),
                    OfficeName = row.Field<string>("OfficeName"),
                    InstallmentAmount = row.Field<decimal>("Amount"),
                    LoanEndDateMsg = row.Field<string>("ChequeNo"),
                    LoanStatus = row.Field<string>("BANKFULLNAME"),
                    LoanType = row.Field<string>("IssueDate"),
                    LoanScheme = row.Field<string>("PaymentDate"),
                    ApprovedStatus = row.Field<string>("ApprovedStatus"),
                    //OfficeName = row.Field<string>("OfficeName"),
                    //OfficeName = row.Field<string>("OfficeName"),

                }).ToList();


                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult getFinalPaymentToBankListApprovedPrint([DataSourceRequest] DataSourceRequest request, int Purpose, string fromDate, string toDate)
        {
            try
            {
                List<EmployeeLoanInstallmentDetailViewModel> List_ViewModel = new List<EmployeeLoanInstallmentDetailViewModel>();


                int Result = 0;

                var param = new { purposeid = Purpose, fromDate = fromDate, toDate = toDate };
                var loanList = employeeSpService.GetDataWithParameter(param, "prl.SP_FINAL_PAYMENT_TO_BANK_DetailList_ApprovedPrint");
                List_ViewModel = loanList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeLoanInstallmentDetailViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    LoanId = row.Field<int>("Id"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("OffcDesignName"),
                    LoanStartDateMsg = row.Field<string>("PurposeName"),
                    InstallmentDateMsg = row.Field<string>("CreateDate"),
                    OfficeName = row.Field<string>("OfficeName"),
                    InstallmentAmount = row.Field<decimal>("Amount"),
                    LoanEndDateMsg = row.Field<string>("ChequeNo"),
                    LoanStatus = row.Field<string>("BANKFULLNAME"),
                    LoanType = row.Field<string>("IssueDate"),
                    LoanScheme = row.Field<string>("PaymentDate"),
                    ApprovedStatus = row.Field<string>("ApprovedStatus"),
                    //OfficeName = row.Field<string>("OfficeName"),
                    //OfficeName = row.Field<string>("OfficeName"),

                }).ToList();


                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        public ActionResult getExperienceLetterList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<EmployeeLoanInstallmentDetailViewModel> List_ViewModel = new List<EmployeeLoanInstallmentDetailViewModel>();


                int Result = 0;


                var loanList = employeeSpService.GetDataWithoutParameter("prl.SP_EmployeeExperienceLetterDetailList");
                List_ViewModel = loanList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeLoanInstallmentDetailViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    LoanId = row.Field<int>("Id"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("OffcDesignName"),
                    LoanStartDateMsg = row.Field<string>("ReportDate"),
                    InstallmentDateMsg = row.Field<string>("CreateDate"),
                    OfficeName = row.Field<string>("OfficeName"),
                }).ToList();


                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult ExperienceLetterSave3(string Date, string EmployeeCode)
        {

            var result = 0;
            var data = "";
            try
            {
                var param = new
                {
                    Date = Date == "" ? System.DateTime.Today.ToShortDateString() : Date,
                    EmployeeCode = EmployeeCode,

                };
                var empList = employeeSpService.GetDataWithParameter(param, "ExperienceLetterSave");
                data = empList.Tables[0].Rows[0]["ID"].ToString();
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExperienceLetterSave2(string Date, string EmployeeCode)
        {

            var result = 0;
            var data = "";
            try
            {
                var param = new
                {
                    Date = Date == "" ? System.DateTime.Today.ToShortDateString() : Date,
                    EmployeeCode = EmployeeCode,

                };
                var empList = employeeSpService.GetDataWithParameter(param, "ExperienceLetterSave");
                data = empList.Tables[0].Rows[0]["ID"].ToString();
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExperienceLetterSave(string Date, string EmployeeCode)
        {

            var result = 0;
            var data = "";
            try
            {
                var param = new
                {
                    Date = Date==""?System.DateTime.Today.ToShortDateString():Date,
                    EmployeeCode = EmployeeCode,

                };
                var empList = employeeSpService.GetDataWithParameter(param, "ExperienceLetterSave");
                data = empList.Tables[0].Rows[0]["ID"].ToString();
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DuplicateChequeNo(string ChequeNo)
        {

            var result = 0;
            var data = "";
            try
            {
                var param = new
                {
                    ChequeNo = ChequeNo
                };
                var empList = employeeSpService.GetDataWithParameter(param, "prl.DuplicateChequeNo");
                data = empList.Tables[0].Rows[0]["Result"].ToString();
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FinalPaymentVoucherSave(string PaymentDate, string EmployeeCode, string IssueDate, string BankBranch, int Bank, string ChequeNo, decimal Amount, int Purpose)
        {

            var result = 0;
            var data = "";
            try
            {
                var param = new
                {
                    PaymentDate = PaymentDate,
                    EmployeeCode = EmployeeCode,
                    IssueDate = IssueDate,                   
                    Bank = Bank,
                    ChequeNo = ChequeNo,
                    Amount = Amount,
                    Purpose = Purpose,
                    CreateBy = SessionHelper.LoggedInEmployeeID
                };
                var empList = employeeSpService.GetDataWithParameter(param, "prl.FinalPaymentVoucherSave");
                data = "1";  //empList.Tables[0].Rows[0]["ID"].ToString();
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult FinalPaymentVoucherApprove(string PaymentDate, string IssueDate, int Purpose)
        {

            var result = 0;
            var data = "";
            try
            {
                var param = new
                {
                    PaymentDate = PaymentDate,
                    IssueDate = IssueDate,
                    Purpose = Purpose,
                    CreateBy = SessionHelper.LoggedInEmployeeID
                };
                var empList = employeeSpService.GetDataWithParameter(param, "prl.FinalPaymentVoucherApprove");
                data = "1";  //empList.Tables[0].Rows[0]["ID"].ToString();
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }




        public ActionResult TerminationLetter()
        {
            return View();
        }


        public ActionResult TerminationReportALL(int OfficeTypeId)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId });

                //var param = new
                //{
                //    id = id
                //};
                //var empList = employeeSpService.GetDataWithoutParameter("TerminationReportALL");
               // var zonal = empList.Tables[0].Rows[0]["AreaName"].ToString();
                //if (zonal.ToLower().Contains("zonal"))
                //{
                //    PrintSSRSReport("/gHRMPlus_Reports/TerminationReport_GC_ZZ", paramValues.ToArray());
                //}
                //else
                //{
                //    PrintSSRSReport("/gHRMPlus_Reports/TerminationReport_GC", paramValues.ToArray());
                //}

                PrintSSRSReport("/gHRMPlus_Reports/TerminationReport_GC_ALL", paramValues.ToArray());
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Content("<b>error</b><br />" + ex.Message);
                // return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult TerminationReportSign(int id)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "id", Value = id });

                var param = new
                {
                    id = id
                };
                var empList = employeeSpService.GetDataWithParameter(param, "TerminationReport");
                var zonal = empList.Tables[0].Rows[0]["AreaName"].ToString();
                if (zonal.ToLower().Contains("zonal"))
                {
                    PrintSSRSReport("/gHRMPlus_Reports/TerminationReport_GC_ZZ_Sign", paramValues.ToArray());
                }
                else if (zonal.ToLower().Contains("gc-ho"))
                {
                    PrintSSRSReport("/gHRMPlus_Reports/TerminationReport_GC_HO_Sign", paramValues.ToArray());
                }
                else
                {
                    PrintSSRSReport("/gHRMPlus_Reports/TerminationReport_GC_Sign", paramValues.ToArray());
                }


                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Content("<b>error</b><br />" + ex.Message);
                // return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult TerminationReport(int id)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "id", Value = id });

                var param = new
                {
                    id = id                   
                };
                var empList = employeeSpService.GetDataWithParameter(param, "TerminationReport");
                var zonal = empList.Tables[0].Rows[0]["AreaName"].ToString();
                if(zonal.ToLower().Contains("zonal"))
                {
                    PrintSSRSReport("/gHRMPlus_Reports/TerminationReport_GC_ZZ", paramValues.ToArray());
                }
                else if(zonal.ToLower().Contains("gc-ho"))
                {
                    PrintSSRSReport("/gHRMPlus_Reports/TerminationReport_GC_HO", paramValues.ToArray());
                }
                else
                {
                    PrintSSRSReport("/gHRMPlus_Reports/TerminationReport_GC", paramValues.ToArray());
                }

               
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Content("<b>error</b><br />" + ex.Message);
                // return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetEmployeeTermination(int Id)
        {
            List<FinalSattlementViewModel> List_Employee = new List<FinalSattlementViewModel>();
            var param = new { Id = Id };
            var empList = employeeSpService.GetDataWithParameter(param, "SP_Get_Termination");


            if (empList.Tables[0].Rows.Count > 0)
            {
                List_Employee = empList.Tables[0].AsEnumerable()
               .Select(row => new FinalSattlementViewModel
               {
                   EmployeeCode = row.Field<string>("EmployeeCode"),
                   DateOfBirthMsg = row.Field<string>("ReportDate"),  

               }).ToList();
            }
            else
            {
                Response.StatusCode = 403;
            }

            return Json(List_Employee.ToList(), JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteEmployeeTermination(int Id)
        {
            var param = new { Id = Id };
            var result = 0;
            var data = "";
            try
            {
                var empList = employeeSpService.GetDataWithParameter(param, "SP_Delete_Termination");

                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }
    

        public ActionResult getTerminationList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<EmployeeLoanInstallmentDetailViewModel> List_ViewModel = new List<EmployeeLoanInstallmentDetailViewModel>();


                int Result = 0;


                var loanList = employeeSpService.GetDataWithoutParameter("prl.SP_EmployeeTerminationDetailList");
                List_ViewModel = loanList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeLoanInstallmentDetailViewModel()
                {
                    rowSl = row.Field<string>("rowSl"),
                    LoanId = row.Field<int>("Id"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationName = row.Field<string>("OffcDesignName"),                    
                    LoanStartDateMsg = row.Field<string>("ReportDate"),                  
                    InstallmentDateMsg = row.Field<string>("CreateDate"),
                    OfficeName = row.Field<string>("OfficeName"),
                }).ToList();


                DataSourceResult result = List_ViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult TerminationSave(string Date, string EmployeeCode)
        {

            var result = 0;
            var data = "";
            try
            {
                var param = new
                {
                    Date = Date,
                    EmployeeCode = EmployeeCode,
                
                };
                var empList = employeeSpService.GetDataWithParameter(param, "TerminationSave");
                data = empList.Tables[0].Rows[0]["ID"].ToString();
                result = 1; 
            }
            catch (Exception ex)
            {
                 result = 0;
                 data = "";              
                return Json(new { result = result, Message = ex.Message , data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult TerminationSaveSign(string Date, string EmployeeCode)
        {

            var result = 0;
            var data = "";
            try
            {
                var param = new
                {
                    Date = Date,
                    EmployeeCode = EmployeeCode,

                };
                var empList = employeeSpService.GetDataWithParameter(param, "TerminationSaveSign");
                data = empList.Tables[0].Rows[0]["ID"].ToString();
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult TerminationSaveALL()
        {

            var result = 0;
            var data = "";
            try
            {
                var param = new
                {                   
                    EmployeeCode = "0",
                };
                var empList = employeeSpService.GetDataWithParameter(param, "TerminationSaveALL");
                data = empList.Tables[0].Rows[0]["ID"].ToString();
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                data = "";
                return Json(new { result = result, Message = ex.Message, data = data }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = result, Message = "", data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FinalSattlement(string EmpCode)
        {
            ViewBag.EmpCode = EmpCode;
            if (String.IsNullOrEmpty(Request.QueryString["Edit"]))
            {
                ViewBag.EditSattlement = "False";
            }
            else
            {
                ViewBag.EditSattlement = "True";
            }

            return View();
        }

        public ActionResult EmployeeReport()
        {
            var model = new EmployeeReportViewModel();
            MapDropdownForDropoutReasonList(model);
            MapDropdownForOfficeTypeList(model);
            MapDropdownForReport(model);

            // For New Searcch Options KHALID major khalid  retd. hurt bir bikkorm

            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["OfficeList"] = items;
            ViewData["HOOfficeList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["OfficeListByType"] = items;
            ViewData["OfficeDeptByType"] = items;
            ViewData["OfficeType"] = LoggedInOfficeType;
            ViewData["LoggedInOfficeId"] = LoggedInOfficeID;
            var offc = officeService.GetById(Convert.ToInt32(LoggedInOfficeID));
            ViewData["SecondLevel"] = offc.SecondLevel;
            ViewData["SecondLevelId"] = officeService.GetByOfficeCode(offc.SecondLevel).OfficeId;
            ViewData["ThirdLevel"] = offc.ThirdLevel;
            ViewData["ThirdLevelId"] = officeService.GetByOfficeCode(offc.ThirdLevel).OfficeId;
            ViewData["FourthLevel"] = offc.FourthLevel;
            ViewData["FourthLevelId"] = officeService.GetByOfficeCode(offc.FourthLevel).OfficeId;
            ViewData["CompanyCode"] = SessionHelper.CompanyCode;
            ViewBag.PERSONAL_INFO_EMPLOYEE_REPORT_HIDE_TAB_COMPANYREPORT = GetSetting("PERSONAL_INFO_EMPLOYEE_REPORT_HIDE_TAB_COMPANYREPORT") == "true";


            var sectionList = new List<SelectListItem>();
            sectionList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            model.SectionList = sectionList;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.DepartmentList = commonDynamicDropDown.GetAllActiveDepartmentList();
            model.DesignationList = commonDynamicDropDown.GetAllPayrollDesignationList();
            model.OfficeDesignationList = commonDynamicDropDown.GetAllOfficeDesignationList();
            var employeeStatusList = commonDynamicDropDown.ddlEmployeeStatusList();
            employeeStatusList.RemoveAll(x => x.Value == "");
            model.EmployeeStatusList = employeeStatusList;

            model.OfficeTypeList = commonDynamicDropDown.GetOfficeTypeList();
            model.ZoneList = commonDynamicDropDown.GetZoneOfficeList();
            model.AreaList = commonDynamicDropDown.ddlInitial();
            model.UnitList = commonDynamicDropDown.ddlInitial();

            ViewBag.DegreeList = educationDegreeService.GetDropdownList();

            // END  New Searcch Options KHALID

            return View(model);
        }

        public ActionResult EmployeeJoiningAndResigningReport(
      int OfficeTypeId, int OfficeId, int DesignationId, int DeptId, int SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = "0" });

                // Call the correct SSRS report
                PrintSSRSReport("/gHRMPlus_Reports/EmployeeDistrictInfo_Test", paramValues.ToArray());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult EmployeeJoiningAndResigningReport_DateRange(
 int OfficeTypeId, int OfficeId, int DesignationId, int DeptId, int SectionId, string Status, string DateFrom, string DateTo )
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateFrom", Value = DateFrom  });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateTo", Value = DateTo });

                // Call the correct SSRS report
                PrintSSRSReport("/gHRMPlus_Reports/EmployeeDistrictInfo_Test2", paramValues.ToArray());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult EmployeeTrainingReport()
        {
            var model = new EmployeeReportViewModel();
            MapDropdownForReport(model);
            return View(model);
        }

        //EmpReportTypeId 1
        public ActionResult EmployeeWiseReportPrint(string empCode)
        {
            long employeeId = 0;
            try
            {
                var employee = employeeService.GetMany(p => p.EmployeeCode == empCode).FirstOrDefault();
                if (employee != null)
                {
                    employeeId = employee.EmployeeId;
                }

                var param = new { EmpID = employeeId };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.rpt_CurrentProductEmplyee");
                var subReport = employeeSpService.GetDataWithParameter(param, "emp.rpt_RetProductEmpHistory");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("rpt_EmployeeWiseProduct_Sub", subReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("rpt_EmployeeWiseProduct.rpt", mainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //EmpReportTypeId 2
        public ActionResult BloodGroupWiseAllEmployeeReportPrint(string bloodGroup, int qType, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, List<string> status, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (bloodGroup == "U")
                {
                    sb.Append(" AND (e.BloodGroup IS NULL OR e.BloodGroup = '" + bloodGroup + "')");
                }
                else if ((bloodGroup != "AG") && (bloodGroup != null || bloodGroup != "0" || bloodGroup != ""))
                {
                    sb.Append(" AND e.BloodGroup = '" + bloodGroup + "'");
                }

                if (status != null && status.Count == 1)
                {
                    if (status[0] != "")
                        sb.Append(" AND es.StatusId IN (" + status[0] + ")");
                }
                else if (status != null && status.Count > 1)
                {
                    string statusList = "";
                    var count = 1;
                    foreach (var Status in status)
                    {
                        if (count < status.Count)
                        {
                            statusList = statusList + "'" + status + "', ";
                        }
                        else
                        {
                            statusList = statusList + "'" + status + "'";
                        }
                        count++;
                    }
                    sb.Append(" AND es.StatusId In(" + statusList + ")");
                }

                if (payRollDesignation != "")
                {
                    sb.Append(" AND E.DesignationId =" + payRollDesignation);
                }
                if (DeptId != "")
                {
                    sb.Append(" AND E.DepartmentId =" + DeptId);
                }
                if (responsibility != "")
                {
                    sb.Append(" AND E.EmployeeRank =" + responsibility);
                }

                if (Section != "")
                {
                    sb.Append(" AND eed.SectionId =" + Section);
                }

                if (officeTypeId != "" && OfficeId == "")
                {
                    sb.Append(" AND E.OfficeId IN (SELECT o.OfficeId FROM Office o WHERE o.OfficeTypeId=" + officeTypeId + ")");
                }
                if (OfficeId != "")
                {
                    sb.Append(" AND E.OfficeId =" + OfficeId);
                }

                //var param = new { BloodGroup = bloodGroup, QType = qType };

                var param = new { AndCondition = sb.ToString() };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_EmpBloodGroup");
                var subReport = employeeSpService.GetDataWithoutParameter("emp.SP_RPT_CountBloodGroup");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("rpt_BloodGroupWiseAllEmployee_Sub", subReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/rpt_BloodGroupWiseAllEmployee.rpt", mainReport.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //EmpReportTypeId 3
        public ActionResult ChartOfBloodSummaryReportPrint(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_CountBloodGroup");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Employee/rpt_ChartOfBloodSummary.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMessage = ex.Message;
                return View("~/Views/Shared/ShowError.cshtml");
            }
        }

        //EmpReportTypeId 4
        public ActionResult OfficeNameWiseEmployeeCount(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_EmpCount_OfficeNameWise");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Employee/rpt_OfficeNameWiseEmployeeCountSummary.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //EmpReportTypeId 5
        public ActionResult OfficeTypeWiseEmployeeCount(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_EmpCount_OfficeTypeWise");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Employee/rpt_OfficeTypeWiseEmployeeCountSummary.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //EmpReportTypeId 6
        public ActionResult GenderWiseEmployeeCount(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_GenderCountSummary");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Employee/rpt_GenderWiseEmpCount.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //EmpReportTypeId 7
        public ActionResult AllDepartmentWiseEmployeeCount(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };

                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_AllDepWiseEmployee");
                // var mainReportForMousumi = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_AllDepWiseEmployeeTest");

                var subReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_DepWiseEmpCount");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("DepWiseEmpSum.rpt", subReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();

                ReportHelper.PrintWithSubReport("Employee/rpt_AllDepWiseEmployee.rpt", mainReport.Tables[0], reportParam, subReportDb);

                //    ReportHelper.PrintWithSubReport("Employee/rpt_AllDepWiseEmployeeTestForMousumi.rpt", mainReportForMousumi.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //EmpReportTypeId 8
        public ActionResult DepartmentWiseTotalEmployeeCount(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_DepWiseEmpCountWithSectionGender");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Employee/rpt_DepWiseEmpSum.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //EmpReportTypeId 9
        public ActionResult DepartmentWiseTotalEmployeeGraphicalView(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_DepWiseEmpCount");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Employee/rpt_GraphicalReportDepWiseEmpSum.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //EmpReportTypeId 10

        public ActionResult PayrollDesignationWiseEmployee(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {                

                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };


                //var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_GetPayrollEmployeeCount");
                //var reportParam = new Dictionary<string, object>();


                // All Status Report
                if (param.EmployeeStatusArr == "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,22") { 

                    var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_GetPayrollEmployeeCount");
                    var reportParam = new Dictionary<string, object>();
                    ReportHelper.PrintReport("Employee/rpt_PayrollDesignationWiseEmpSummary.rpt", mainReport.Tables[0], reportParam);
                }

                // All Active Status Report
                else if (param.EmployeeStatusArr == "1,2,3,4,5,6,7,8,9,10,11")
                {
                    var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_GetPayrollEmployeeCount_ActiveInactive");
                    var reportParam = new Dictionary<string, object>();

                    ReportHelper.PrintReport("Employee/rpt_PayrollDesignationWiseEmpSummary_Active_Inactive.rpt", mainReport.Tables[0], reportParam);
                }

                // All In-Active Status Report
                else if (param.EmployeeStatusArr == "12,13,14,15,16,17,18,19,22")
                {
                    var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_GetPayrollEmployeeCount_ActiveInactive");
                    var reportParam = new Dictionary<string, object>();

                    ReportHelper.PrintReport("Employee/rpt_PayrollDesignationWiseEmpSummary_Active_Inactive.rpt", mainReport.Tables[0], reportParam);
                }

             
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        //EmpReportTypeId 42

        public ActionResult PayrollDesignationWiseEmployee_Payroll(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {

                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };


                //var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_GetPayrollEmployeeCount");
                //var reportParam = new Dictionary<string, object>();


                // All Status Report
                if (param.EmployeeStatusArr == "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,22")
                {

                    var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_GetPayrollEmployeeCount");
                    var reportParam = new Dictionary<string, object>();
                    ReportHelper.PrintReport("Employee/rpt_PayrollDesignationWiseEmpSummary.rpt", mainReport.Tables[0], reportParam);
                }

                // All Active Status Report
                else if (param.EmployeeStatusArr == "1,2,3,4,5,6,7,8,9,10,11")
                {
                    var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_GetPayrollEmployeeCount_ActiveInactive");
                    var reportParam = new Dictionary<string, object>();

                    ReportHelper.PrintReport("Employee/rpt_PayrollDesignationWiseEmpSummary_Active_Inactive.rpt", mainReport.Tables[0], reportParam);
                }

                // All In-Active Status Report
                else if (param.EmployeeStatusArr == "12,13,14,15,16,17,18,19,22")
                {
                    var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_GetPayrollEmployeeCount_ActiveInactive");
                    var reportParam = new Dictionary<string, object>();

                    ReportHelper.PrintReport("Employee/rpt_PayrollDesignationWiseEmpSummary_Active_Inactive.rpt", mainReport.Tables[0], reportParam);
                }


                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }






        //EmpReportTypeId 11
        public ActionResult EmployementTypeWiseEmployeeCount(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_GetEmploymentTypeNameWiseCount");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Employee/rpt_EmploymentCountSummary_graph.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //EmpReportTypeId 12
        public ActionResult PayrollDesignationWiseInsuranceReport(string DateFrom, string DateTo, string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                var param = new
                {
                    DateFrom = DateFrom,
                    DateTo = DateTo,
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PayrollDesignationWishInsurance");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Employee/rpt_PayrollDesignationWishInsurance.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //EmpReportTypeId 7
        public ActionResult PayrollDesignationWiseEmployeeDetails(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };

                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PayrollDesignationWiseEmployeeDetails");
      
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Employee/rpt_PayrollDesignationWiseEmployeeDetails.rpt", mainReport.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult PayrollDesignationWiseInsuranceReportExcel(string DateFrom, string DateTo, string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                var param = new
                {
                    DateFrom = DateFrom,
                    DateTo = DateTo,
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PayrollDesignationWishInsurance");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.ExportExcelReport("Employee/rpt_PayrollDesignationWishInsurance.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult EmployeeBloodGroupReportExcel(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { DateFrom = DateFrom, DateTo = DateTo };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PayrollDesignationWishInsurance");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.ExportExcelReport("Employee/rpt_PayrollDesignationWishInsurance.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult EmployeeTrainingInfoReport(string DateFrom, string DateTo, string InstituteName, string TrainingTitle)
        {
            try
            {
                var param = new { DateFrom = DateFrom, DateTo = DateTo, InstituteName = InstituteName, TrainingTitle = TrainingTitle };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.RPT_EmployeeTrainingReport");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Employee/rpt_EmployeeTrainingReport.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //EmpReportTypeId 13
        public ActionResult EmployeeExperienceReport(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_Employee_Experience_Info");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Employee/rpt_EmployeeExperienceInfo.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //EmpReportTypeId 14
        public ActionResult EmployeeDemographicReport(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_Employee_Demographic_Info");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Employee/EmployeeDemographicReport.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //EmpReportTypeId 16
        public ActionResult EmployeeServiceBookReport(string employeeCode, string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                int OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId);
                int nOfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId);
                int DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation);

                int DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId);
                int SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section);
                string BloodGroup = "0" == bloodGroup ? "" : bloodGroup;
                bool SERVICE_BOOK_REPORT_HEIGHT_CONVERT_INCHES_TO_CM = AppSetting.GetBool(AppSetting.SERVICE_BOOK_REPORT_HEIGHT_CONVERT_INCHES_TO_CM, HttpContext);


                var listings = employeeSpService.GetEmployeeServiceBookInfoListingByFilter(employeeCode, BloodGroup, OfficeTypeId, nOfficeId, DepartmentId, DesignationId, responsibility, SectionId, status, SERVICE_BOOK_REPORT_HEIGHT_CONVERT_INCHES_TO_CM);

                //section 01 & 02: basic Information
                var mainReport = listings.ToDataTable();

                //section 03: servicce record and salary info
                var monthlySalaryForServiceBookListing = employeeSpService.GetMonthlySalaryForServiceBookListingByFilter(employeeCode, BloodGroup, OfficeTypeId, nOfficeId, DepartmentId, DesignationId, responsibility, SectionId, status);
                var monthlySalaryForServiceBookReport = monthlySalaryForServiceBookListing.ToDataTable();


                //section 04: leave records
                int? LeaveTypeId = 5; //==> Earn Leave=5
                if (SessionHelper.CompanyCode == GHRMPlusCompanyConstants.GT)
                    LeaveTypeId = 22;
                if (SessionHelper.CompanyCode == GHRMPlusCompanyConstants.GrameenTelecomTrust)
                    LeaveTypeId = 2;

                var leaveRecordServiceBookListing = employeeSpService.GetLeaveRecordServiceBookListingByFilter(employeeCode, LeaveTypeId, BloodGroup, OfficeTypeId, nOfficeId, DepartmentId, DesignationId, responsibility, SectionId, status);
                var leaveRecordServiceBookReport = leaveRecordServiceBookListing.ToDataTable();

                //section 05: case history records
                var employeeWiseCaseHistoryListing = employeeSpService.GetEmployeeWiseCaseHistoryServiceBookListingByFilter(employeeCode, BloodGroup, OfficeTypeId, nOfficeId, DepartmentId, DesignationId, responsibility, SectionId, status);
                var employeeWiseCaseHistoryReport = employeeWiseCaseHistoryListing.ToDataTable();

                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("EmployeeServiceRecordReport", monthlySalaryForServiceBookReport);
                subReportDb.Add("LeaveRecordServiceBookReport", leaveRecordServiceBookReport);
                subReportDb.Add("EmployeeWiseCaseHistoryReport", employeeWiseCaseHistoryReport);




                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintWithSubReport("Employee/EmployeeServiceBookReport.rpt", mainReport, reportParam, subReportDb);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        // type : 18 Leave Details Report
        public ActionResult EmployeeWiseLeaveDetailsPrint(string empCode)
        {
            long employeeId = 0;
            try
            {
                var employee = employeeService.GetMany(p => p.EmployeeCode == empCode).FirstOrDefault();
                if (employee != null)
                {
                    employeeId = employee.EmployeeId;
                }
                var param = new { EmpID = employeeId, @orgId = SessionHelper.LoginUserOfficeID };

                var mainReport = employeeSpService.GetDataWithParameter(param, "[emp].[rpt_EmpLeaveDetails]");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Leave/Rpt_GetLeaveDetails.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        //EmpReportTypeId 23
        public ActionResult AllDepartmentWiseEmployeeCountForMousumi(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };

                // var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_AllDepWiseEmployee");
                var mainReportForMousumi = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_AllDepWiseEmployeeTest");

                var subReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_DepWiseEmpCount");
                var subReportDb = new Dictionary<string, DataTable>();
                subReportDb.Add("DepWiseEmpSum.rpt", subReport.Tables[0]);
                var reportParam = new Dictionary<string, object>();

                // ReportHelper.PrintWithSubReport("Employee/rpt_AllDepWiseEmployee.rpt", mainReport.Tables[0], reportParam, subReportDb);

                ReportHelper.PrintWithSubReport("Employee/rpt_AllDepWiseEmployeeTestForMousumi.rpt", mainReportForMousumi.Tables[0], reportParam, subReportDb);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        //EmpReportTypeId 24
        public ActionResult PayrollDesignationWiseEmployeeForMousumi(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_Payroll_Designation_For_Mousumi_Info");

                var reportParam = new Dictionary<string, object>();

                ReportHelper.PrintReport("Employee/PayrollDesignationForTestMousumi.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        //EmpReportTypeId 25


        public ActionResult EmployeeExperienceReportForMousumi(
            string OfficeTypeId, string OfficeId, string DesignationId, string ResponsibilityId, string DeptId, string SectionId, string Status, string EmployeeCode)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = (string.IsNullOrEmpty(EmployeeCode) ? "0" : EmployeeCode) });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (string.IsNullOrEmpty(OfficeTypeId) ? "0" : OfficeTypeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (string.IsNullOrEmpty(OfficeId) ? "0" : OfficeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = (string.IsNullOrEmpty(DesignationId) ? "0" : DesignationId) });

                //   paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = "1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = (string.IsNullOrEmpty(DeptId) ? "0" : DeptId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = (string.IsNullOrEmpty(SectionId) ? "0" : SectionId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = (string.IsNullOrEmpty(ResponsibilityId) ? "0" : ResponsibilityId) });
                PrintSSRSReport("/gHRMPlus_Reports/EmployeePrvWorExperienceForMousumi", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }



        //EmpReportTypeId 27

        public ActionResult EmployeeStatusBranchWiseForMousumi(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status, string EmployeeCode)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = (string.IsNullOrEmpty(EmployeeCode) ? "0" : EmployeeCode) });
                //  paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusId", Value = (string.IsNullOrEmpty(EmployeeStatusId) ? "0" : EmployeeStatusId) });


                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (string.IsNullOrEmpty(officeTypeId) ? "0" : officeTypeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (string.IsNullOrEmpty(OfficeId) ? "0" : OfficeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = (string.IsNullOrEmpty(payRollDesignation) ? "0" : payRollDesignation) });
                //   paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = "1, 2, 3, 4, 5, 6, 7, 8, 9, 10" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = (string.IsNullOrEmpty(status) ? "0" : status) });


                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = (string.IsNullOrEmpty(DeptId) ? "0" : DeptId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = (string.IsNullOrEmpty(Section) ? "0" : Section) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = (string.IsNullOrEmpty(responsibility) ? "0" : responsibility) });
                PrintSSRSReport("/gHRMPlus_Reports/EmployeeStatusWiseReportForMousumi", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        //EmpReportTypeId 29

        public ActionResult ReportOfJoiningLetterAfterTransfer(
              string OfficeTypeId, string OfficeId, string DesignationId, string ResponsibilityId, string DeptId, string SectionId, string Status, string EmployeeCode)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = (string.IsNullOrEmpty(EmployeeCode) ? "0" : EmployeeCode) });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (string.IsNullOrEmpty(OfficeTypeId) ? "0" : OfficeTypeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (string.IsNullOrEmpty(OfficeId) ? "0" : OfficeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = (string.IsNullOrEmpty(DesignationId) ? "0" : DesignationId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = "1, 2, 3, 4, 5, 6, 7, 8, 9, 10" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = (string.IsNullOrEmpty(DeptId) ? "0" : DeptId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = (string.IsNullOrEmpty(SectionId) ? "0" : SectionId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = (string.IsNullOrEmpty(ResponsibilityId) ? "0" : ResponsibilityId) });
                PrintSSRSReport("/gHRMPlus_Reports/EmployeeJoiningLetterOfTransferReportForMousumi", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }





        public ActionResult EmployeeCertificatesInformationReport(int EmployeeId, string copyname)
        {
            try
            {
                var sessionUserId = Convert.ToInt64(LoggedInEmployeeId);
                var param = new { EmployeeId = EmployeeId, copyname = copyname, LoggedInEmployeeId = sessionUserId };
                var dataSource = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_EmployeeCertificatesInformation");
                var dtCompanyInfo = WebHelper.GetCompanyInfo();

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("copyname", copyname);

                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";
                var reportPartialPath = "Employee/rpt_EmployeeCertificatesInformation.rpt";

                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, dataSource.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EmployeeSignatureReportList(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };
                var MainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_PROFILE_EmployeeSignature");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Employee/EmployeeSignatureReport.rpt", MainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /*
        public ActionResult EmployeeTransferHistory(string Code)
        {
            var model = new EmployeeOtherInformationViewModel();
            model.EmployeeCode = Code;

           
            return View(model);
        }

        public ActionResult IndividualEmployeePostingHistory(string EmployeeCode)
        {
            try
            {
                var param = new { EmployeeCode = EmployeeCode };

                var mainReport = employeeSpService.GetDataWithParameter(param, "[Emp].Nishan1EmployeePostingHistory");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Employee/IndividualEmployeePostHist.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
    */
        private List<SelectListItem> Months()
        {
            List<SelectListItem> items3 = new List<SelectListItem>();
            items3.Add(new SelectListItem
            {
                Text = "Please Select",
                Value = "0"
            });
            items3.Add(new SelectListItem
            {
                Text = "January",
                Value = "January"
            });
            items3.Add(new SelectListItem
            {
                Text = "February",
                Value = "February"
            });
            items3.Add(new SelectListItem
            {
                Text = "March",
                Value = "March"
            });
            items3.Add(new SelectListItem
            {
                Text = "April",
                Value = "April"
            });
            items3.Add(new SelectListItem
            {
                Text = "May",
                Value = "May"
            });
            items3.Add(new SelectListItem
            {
                Text = "June",
                Value = "June"
            });
            items3.Add(new SelectListItem
            {
                Text = "July",
                Value = "July"
            });
            items3.Add(new SelectListItem
            {
                Text = "August",
                Value = "August"
            });
            items3.Add(new SelectListItem
            {
                Text = "September",
                Value = "September"
            });
            items3.Add(new SelectListItem
            {
                Text = "October",
                Value = "October"
            });
            items3.Add(new SelectListItem
            {
                Text = "November",
                Value = "November"
            });
            items3.Add(new SelectListItem
            {
                Text = "December",
                Value = "December"
            });

            return items3;
        }// End of Month
        private List<SelectListItem> Years()
        {
            List<SelectListItem> items2 = new List<SelectListItem>();
            items2.Add(new SelectListItem
            {
                Text = "Please Select",
                Value = "0"
            });

            int year = DateTime.Now.Year; //Current Year.
            int lowYear = year - 5;


            for (; year >= lowYear; year--)
            {
                items2.Add(new SelectListItem
                {
                    Text = Convert.ToString(year),
                    Value = Convert.ToString(year)
                });
            }

            return items2;
        }// End of Years

        /*
        private string GetNextPromotionDate(string Year, string MonthName)
        {
            var currentDateOfMonth = DateTime.Now.Day;
            var nextPromotionMaxDate = DateTime.Parse($"01-{MonthName}-{Year}").AddMonths(1).AddDays(-1);

            return nextPromotionMaxDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
        }

            */

        public ActionResult GeneralReports()
        {
            ViewData["Months"] = Months();
            ViewData["Years"] = Years();

            var model = new EmployeeOtherInformationViewModel();
            return View(model);
        }

        public ActionResult ProbationDuration()
        {
            try
            {
                var mainReport = employeeSpService.GetDataWithoutParameter("[EMP].Nishan4ProbesionPerionSix");
                var reportParam = new Dictionary<string, object>();

                var dtCompanyInfo = WebHelper.GetCompanyInfo();
                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";

                var reportPartialPath = "Employee/provisionDuration.rpt";

                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, mainReport.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult CuktiVittikEmployees()
        {
            try
            {
                var mainReport = employeeSpService.GetDataWithoutParameter("[EMP].CuktiVittikEmployee");
                var reportParam = new Dictionary<string, object>();

                var dtCompanyInfo = WebHelper.GetCompanyInfo();
                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";

                var reportPartialPath = "Employee/Cuktivittikemployee.rpt";

                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, mainReport.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult DesiWiseEmpList()
        {
            try
            {
                var mainReport = employeeSpService.GetDataWithoutParameter("[EMP].DesigEmployeeList");
                var reportParam = new Dictionary<string, object>();

                var dtCompanyInfo = WebHelper.GetCompanyInfo();
                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";

                var reportPartialPath = "Employee/DesigWiseEmpList.rpt";

                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, mainReport.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult StaffPositionEmpList()
        {
            try
            {
                var mainReport = employeeSpService.GetDataWithoutParameter("[EMP].StaffPositionEmployeeList");
                var reportParam = new Dictionary<string, object>();

                var dtCompanyInfo = WebHelper.GetCompanyInfo();
                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";

                var reportPartialPath = "Employee/StaffPositionEmpList.rpt";

                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, mainReport.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /*
        public ActionResult IncrementElegibleEmpList(string MonthName, string Year)
        {
            try
            {
                // var currentDateOfMonth = DateTime.Now; //.Day;
                // var nextPromotionMaxDate = DateTime.Parse($"01-{currentDateOfMonth.Month}-{currentDateOfMonth.Year}").AddMonths(1).AddDays(-1);

                //var Dates =  nextPromotionMaxDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

                string selectedDate = GetNextPromotionDate(Year, MonthName);
                var param = new{ selectedDate = selectedDate };
                var mainReport = employeeSpService.GetDataWithParameter(param, "[EMP].IncrementElegibleEmpList");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("MonthName", MonthName);
                reportParam.Add("Year", Year);

                ReportHelper.PrintReport("Employee/IncrementElegibleEmpList.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
    */

        public ActionResult OfficeWiseEmpList()
        {
            try
            {
                var mainReport = employeeSpService.GetDataWithoutParameter("[EMP].OfficeWiseEmpNumber");
                var reportParam = new Dictionary<string, object>();

                var dtCompanyInfo = WebHelper.GetCompanyInfo();
                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";

                var reportPartialPath = "Employee/OfficeWiseEmpNumber.rpt";

                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, mainReport.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);


                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult PresentOfficeMoreThanTHreeYears()
        {
            try
            {
                var mainReport = employeeSpService.GetDataWithoutParameter("[Emp].SameOfficeMoreThanThreeYear");
                var reportParam = new Dictionary<string, object>();

                var dtCompanyInfo = WebHelper.GetCompanyInfo();
                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";

                var reportPartialPath = "Employee/PresentOfficeMoreThanThreeYear.rpt";

                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, mainReport.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult LeaveEncashment()
        {
            try
            {
                var mainReport = employeeSpService.GetDataWithoutParameter("[EMP].LeaveEncashment");
                var reportParam = new Dictionary<string, object>();

                var dtCompanyInfo = WebHelper.GetCompanyInfo();
                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";

                var reportPartialPath = "Employee/LeaveEncashment.rpt";

                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, mainReport.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult EmployeesWillBeRetired(string SelectedDate)
        {
            try
            {
                var param = new { @SelectedDate = SelectedDate };
                var mainReport = employeeSpService.GetDataWithParameter(param, "[EMP].WHOWillBeRetired");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("SelectedDate", SelectedDate);

                var dtCompanyInfo = WebHelper.GetCompanyInfo();
                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";

                var reportPartialPath = "Employee/WhoWillBeRetiredt.rpt";

                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, mainReport.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult EmployeeListWhoHasInsurance()
        {
            try
            {
                var mainReport = employeeSpService.GetDataWithoutParameter("[EMP].EmpListWhoHasInsurance");
                var reportParam = new Dictionary<string, object>();

                var dtCompanyInfo = WebHelper.GetCompanyInfo();
                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";

                var reportPartialPath = "Employee/EmpListWithInsurance.rpt";

                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, mainReport.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult EmployeeListAbsentReleased(string FromDate)
        {
            try
            {
                var param = new { FromDate = FromDate };
                var mainReport = employeeSpService.GetDataWithParameter(param, "[EMP].EmpListAbsentReleased");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("FromDate", FromDate);

                var dtCompanyInfo = WebHelper.GetCompanyInfo();
                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";

                var reportPartialPath = "Employee/EmpListAbsentRelease.rpt";

                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, mainReport.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);


                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult EmployeeListOfficeDesig()
        {
            try
            {

                var mainReport = employeeSpService.GetDataWithoutParameter("[Emp].EmpNameList");
                var reportParam = new Dictionary<string, object>();

                var dtCompanyInfo = WebHelper.GetCompanyInfo();
                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";

                var reportPartialPath = "Employee/EmpListWithOfficeInfo.rpt";

                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, mainReport.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);


                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult AgreementToEndEmpNameList()
        {
            try
            {

                var mainReport = employeeSpService.GetDataWithoutParameter("[Emp].AgrementToEndEmpNameList");
                var reportParam = new Dictionary<string, object>();

                var dtCompanyInfo = WebHelper.GetCompanyInfo();
                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";

                var reportPartialPath = "Employee/AgrementToEndEmpList.rpt";

                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, mainReport.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);

                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult LeaveEncahsmentEmpList()
        {
            try
            {

                var mainReport = employeeSpService.GetDataWithoutParameter("[EMP].LeaveEncahsmentEmpList");
                var reportParam = new Dictionary<string, object>();

                var dtCompanyInfo = WebHelper.GetCompanyInfo();
                var dataSourceName = "Command";
                var dtCompanyInfoName = "CompanyInfo";

                var reportPartialPath = "Employee/LeaveEncashmentEmpList.rpt";

                ReportHelper.PrintReportWithMultipleDataSource(reportPartialPath, mainReport.Tables[0], dataSourceName, dtCompanyInfo, dtCompanyInfoName, reportParam);


                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        #endregion

        #region Digital ID Card


        public async Task<ActionResult> DigitalIDCardMousumi(int? officeID, int? departmentId, string employeeCode = "")
        {
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";

            var filter = new EmployeeSearchFilter { OfficeTypeId = officeID, DepartmentId = departmentId, EmployeeCode = employeeCode };
            var filteredList = await employeeDocumentService.GetEmployeeDigitalIDInfo(filter);

            //generate qr code
            filteredList.ForEach(f => f.QRCode = GenerateQRCode(f.EmployeeCode));

            var model = new DigitalIDCardListViewModel
            {
                DigitalIDCardInfos = filteredList,
                BaseUrl = baseUrl
            };

            return View(model);
        }



        public async Task<ActionResult> DigitalIDCard(int? officeID, int? departmentId, string employeeCode = "")
        {
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";

            var filter = new EmployeeSearchFilter { OfficeTypeId = officeID, DepartmentId = departmentId, EmployeeCode = employeeCode };
            var filteredList = await employeeDocumentService.GetEmployeeDigitalIDInfo(filter);

            //generate qr code
            filteredList.ForEach(f => f.QRCode = GenerateQRCode(f.EmployeeCode));

            var model = new DigitalIDCardListViewModel
            {
                DigitalIDCardInfos = filteredList,
                BaseUrl = baseUrl
            };

            return View(model);
        }

        #endregion ProvisionDuration

        #region Private Methods
        private void MapDropdownForDropoutReasonList(EmployeeReportViewModel model)
        {
            var dropOutReasonList = employeeStatusService.GetAll().Where(p => p.IsActive == true && p.IsValid == false);
            var viewDropOutReasonList = dropOutReasonList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.StatusName,
                Value = p.StatusId.ToString()
            });
            var dropOutReason = new List<SelectListItem>();
            dropOutReason.Add(new SelectListItem { Text = "Please Select", Value = "" });
            dropOutReason.AddRange(viewDropOutReasonList);
            model.ReasonList = dropOutReason;
        }
        private void MapDropdownForOfficeTypeList(EmployeeReportViewModel model)
        {
            var officeTypeList = officeTypeService.GetAll().Where(p => p.IsActive == true);
            var viewOfficeTypeList = officeTypeList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.OfficeTypeName,
                Value = p.OfficeTypeId.ToString()
            });
            var officeType = new List<SelectListItem>();
            officeType.Add(new SelectListItem { Text = "Please Select", Value = "" });
            officeType.AddRange(viewOfficeTypeList);
            model.OfficeTypeList = officeType;
        }

        private string GenerateQRCode(string qrcodeText)
        {
            string folderPath = "~/Webshared/Uploads/";
            string imagePath = $"~/Webshared/Uploads/DigitalIDQRCode/{qrcodeText}_qr_code.jpg";

            var absolutePath = HttpContext.Server.MapPath(imagePath.Replace("~/", ""));
            if (System.IO.File.Exists(absolutePath))
                return imagePath.Replace("~/", "");

            // If the directory doesn't exist then create it.
            if (!Directory.Exists(Server.MapPath(folderPath)))
                Directory.CreateDirectory(Server.MapPath(folderPath));

            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            var result = barcodeWriter.Write(qrcodeText);

            string barcodePath = Server.MapPath(imagePath);
            var barcodeBitmap = new Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

            return imagePath.Replace("~/", "");
        }

        public void MapDropdownForReport(EmployeeReportViewModel model)
        {
            var empReportList = employeeReportOptionService.GetMany(p => p.IsActive == true).OrderBy(p => p.EmpReportTypeId);

            var viewList = empReportList.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.EmpReportTypeName,
                Value = p.EmpReportTypeId.ToString()
            }).ToList();

            var list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            list.AddRange(viewList);
            model.ReportList = list;

            var bloodGroupList = new List<SelectListItem>();
            bloodGroupList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            bloodGroupList.Add(new SelectListItem() { Text = "A+", Value = "A+" });
            bloodGroupList.Add(new SelectListItem() { Text = "A-", Value = "A-" });
            bloodGroupList.Add(new SelectListItem() { Text = "B+", Value = "B+" });
            bloodGroupList.Add(new SelectListItem() { Text = "B-", Value = "B-" });
            bloodGroupList.Add(new SelectListItem() { Text = "AB+", Value = "AB+" });
            bloodGroupList.Add(new SelectListItem() { Text = "AB-", Value = "AB-" });
            bloodGroupList.Add(new SelectListItem() { Text = "O+", Value = "O+" });
            bloodGroupList.Add(new SelectListItem() { Text = "O-", Value = "O-" });
            bloodGroupList.Add(new SelectListItem() { Text = "Unknown", Value = "U" });
            bloodGroupList.Add(new SelectListItem() { Text = "All Group", Value = "AG" });
            model.BloodGroupList = bloodGroupList;

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
            model.AreaList = area_items;

            var unit_items = new List<SelectListItem>();
            unit_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
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

            var activeInactiveList = new List<SelectListItem>();
            activeInactiveList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            activeInactiveList.Add(new SelectListItem() { Text = "Active", Value = "1" });
            activeInactiveList.Add(new SelectListItem() { Text = "Inactive", Value = "2" });
            model.ActiveInactiveList = activeInactiveList;

            var instituteNameList = employeeTrainingService.GetMany(x => x.IsActive == true && x.IsApproved == true).Select(x => x.InstituteName).Distinct();
            var viewinstituteNameList = instituteNameList.Select(x => x).ToList().Select(y => new SelectListItem
            {
                Value = y,
                Text = y
            });

            var instituteNameList_items = new List<SelectListItem>();
            instituteNameList_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            instituteNameList_items.AddRange(viewinstituteNameList);
            model.InstituteNameList = instituteNameList_items;

            var trainingTitle = employeeTranningDropDownService.GetAll().Where(p => p.IsActive == true);
            var viewtrainingTitlelist = trainingTitle.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.EmployeeTrainingDropDownName,
                Value = p.EmployeeTrainingDropDownId.ToString()
            }).ToList();

            var trainingTitlelist = new List<SelectListItem>();
            trainingTitlelist.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            trainingTitlelist.AddRange(viewtrainingTitlelist);
            model.TrainingTitleList = trainingTitlelist;
        }

        #endregion

        # region JCF Reports
        public ActionResult EmployeeProfileAtAGlance()
        {
            var model = new EmployeeOtherInformationViewModel();
            return View(model);
        }

        public ActionResult StaffProfileAtaGlance(string format, string EmployeeCode = "")
        {
            try
            {       // Stored Procedure: SP_GetEmployeeProfile_AtGlance
                string type = "view";
                DataSet mainDataSource = GetEmployeePersonalInfoData(EmployeeCode);
                string reportTitle = "Staff Profile At a Glance";
                var parameters = new Dictionary<string, object>();
                parameters.Add("ReportTitle", reportTitle);
                parameters.Add("OfficeName", SessionHelper.OrganizationName);
                parameters.Add("OfficeAddress", SessionHelper.CompanyAddress);

                var reportDataSourceName = "EmpProfileAtaGlance";
                string reportPath = "~/Reports/RDLC/Employee/StaffProfileAtAGalance.rdlc";
                string reportViewMode = ReportViewModeConstants.Potrait;

                return Report(mainDataSource.Tables[0], reportDataSourceName, parameters, reportTitle, reportPath, format, type = "view", reportViewMode);
            }
            catch (Exception ex)
            {
                return RedirectToAction("CommonReportGenerationError");
            }
        }

        private DataSet GetEmployeePersonalInfoData(string EmployeeCode)
        {
            var param = new { EmployeeCode = EmployeeCode };
            var mainDataSource = employeeSpService.GetDataWithParameter(param, "dbo.SP_GetEmployeeProfile_AtGlance");
            return mainDataSource;
        }

        public ActionResult StaffIndividualProfileAtAGlance()
        {
            return View();
        }

        public ActionResult EmployeeAppointmentLetter()
        {
            return View();
        }
        public ActionResult EmployeeJobConfirmationLetter()
        {
            return View();
        }

        public ActionResult EmployeeIncrementLetter()
        {
            return View();
        }

        public ActionResult WhatsAppNo()
        {
            return View();
        }
        public ActionResult EmployeePromotionLetter()
        {
            return View();
        }
        public ActionResult EmployeeJobSeparationLetter()
        {
            return View();
        }


        #endregion JCF Reports



        public ActionResult DownloadLetters(string ddlList, string EmployeeCode)
        {
            //var dData = officialFileUploadService.GetById(Convert.ToInt32(FileUploadId));
            //return File("~/Content/gBanker6.0_User_Manual.pdf", "application/pdf", "gBanker6.0_User_Manual.pdf");

            var param = new { ddlList = ddlList, EmployeeCode = EmployeeCode };

            var getFeedbackRegisterDetails = employeeSpService.GetDataWithParameter(param, "SP_Download_Letters");  //     .GetById(Convert.ToInt32(FileUploadId));



            var location = getFeedbackRegisterDetails.Tables[0].Rows[0]["FileLocation"].ToString();// dData.FileLocation + "/" + dData.FileName;
            if (location == null || location == "")// If File Not Exist.
            {
                return GetErrorMessageResult(); ;
            }
            var fileName = Path.GetFileName(location);


            return File(location, "application/pdf", fileName); //dData.FileName
                                                                //return View();
        }

        public ActionResult DownloadFile(string types,  string FileUploadId)
        {
            //var dData = officialFileUploadService.GetById(Convert.ToInt32(FileUploadId));
            //return File("~/Content/gBanker6.0_User_Manual.pdf", "application/pdf", "gBanker6.0_User_Manual.pdf");

            var param = new { types = types , EmployeeId = FileUploadId };

            var getFeedbackRegisterDetails = employeeSpService.GetDataWithParameter(param, "SP_Download_File");  //     .GetById(Convert.ToInt32(FileUploadId));



            var location = getFeedbackRegisterDetails.Tables[0].Rows[0]["FileLocation"].ToString();// dData.FileLocation + "/" + dData.FileName;
            if (location == null || location == "")// If File Not Exist.
            {
                return GetErrorMessageResult(); ;
            }
            var fileName = Path.GetFileName(location);

            //Save/Update downloadTimes table
            var OfficeId = SessionHelper.LoginUserOfficeID;
          
            if(getFeedbackRegisterDetails.Tables[0].Rows[0]["CVFileType"].ToString() == "msword")
            {
                return File(location, "application/doc", fileName);
            }

            if (getFeedbackRegisterDetails.Tables[0].Rows[0]["CVFileType"].ToString() == "vnd.openxm")
            {
                return File(location, "application/docx", fileName);
            }

            if (getFeedbackRegisterDetails.Tables[0].Rows[0]["CVFileType"].ToString() == "pdf")
            {
                return File(location, "application/pdf", fileName);
            }

            return File(location, "application/pdf", fileName); //dData.FileName
                                                                //return View();
        }

        public ActionResult DownloadFile2(string types,  string FileUploadId)
        { 
            //var dData = officialFileUploadService.GetById(Convert.ToInt32(FileUploadId));
            //return File("~/Content/gBanker6.0_User_Manual.pdf", "application/pdf", "gBanker6.0_User_Manual.pdf");



            var param = new { types = types,  EmployeeId = FileUploadId };

            var getFeedbackRegisterDetails = employeeSpService.GetDataWithParameter(param, "SP_Download_File");  //  



            var location = getFeedbackRegisterDetails.Tables[0].Rows[0]["FileLocation"].ToString();// dData.FileLocation + "/" + dData.FileName;
            if (location == null || location == "")// If File Not Exist.
            {
                return GetErrorMessageResult(); ;
            }
            var fileName = Path.GetFileName(location);

            //Save/Update downloadTimes table
            var OfficeId = SessionHelper.LoginUserOfficeID;
            if (getFeedbackRegisterDetails.Tables[0].Rows[0]["CVFileType"].ToString() == "msword")
            {
                return File(location, "application/doc", fileName);
            }

            if (getFeedbackRegisterDetails.Tables[0].Rows[0]["CVFileType"].ToString() == "vnd.openxm")
            {
                return File(location, "application/docx", fileName);
            }

            if (getFeedbackRegisterDetails.Tables[0].Rows[0]["CVFileType"].ToString() == "pdf")
            {
                return File(location, "application/pdf", fileName);
            }


            return File(location, "application/pdf", fileName); //dData.FileName
                                                                //return View();
        }


        public ActionResult DownloadFileAdmin(string types, string FileUploadId)
        {
            //var dData = officialFileUploadService.GetById(Convert.ToInt32(FileUploadId));
            //return File("~/Content/gBanker6.0_User_Manual.pdf", "application/pdf", "gBanker6.0_User_Manual.pdf");

            var param = new { types = types, EmployeeId = FileUploadId };

            var getFeedbackRegisterDetails = employeeSpService.GetDataWithParameter(param, "SP_Download_File_Admin");  //     .GetById(Convert.ToInt32(FileUploadId));



            var location = getFeedbackRegisterDetails.Tables[0].Rows[0]["FileLocation"].ToString();// dData.FileLocation + "/" + dData.FileName;
            if (location == null || location == "")// If File Not Exist.
            {
                return GetErrorMessageResult(); ;
            }
            var fileName = Path.GetFileName(location);

            //Save/Update downloadTimes table
            var OfficeId = SessionHelper.LoginUserOfficeID;

            if (getFeedbackRegisterDetails.Tables[0].Rows[0]["CVFileType"].ToString() == "msword")
            {
                return File(location, "application/doc", fileName);
            }

            if (getFeedbackRegisterDetails.Tables[0].Rows[0]["CVFileType"].ToString() == "vnd.openxm")
            {
                return File(location, "application/docx", fileName);
            }

            if (getFeedbackRegisterDetails.Tables[0].Rows[0]["CVFileType"].ToString() == "pdf")
            {
                return File(location, "application/pdf", fileName);
            }

            return File(location, "application/pdf", fileName); //dData.FileName
                                                                //return View();
        }

        public ActionResult DownloadFile2Admin(string types, string FileUploadId)
        {
            //var dData = officialFileUploadService.GetById(Convert.ToInt32(FileUploadId));
            //return File("~/Content/gBanker6.0_User_Manual.pdf", "application/pdf", "gBanker6.0_User_Manual.pdf");



            var param = new { types = types, EmployeeId = FileUploadId };

            var getFeedbackRegisterDetails = employeeSpService.GetDataWithParameter(param, "SP_Download_File_Admin");  //  



            var location = getFeedbackRegisterDetails.Tables[0].Rows[0]["FileLocation"].ToString();// dData.FileLocation + "/" + dData.FileName;
            if (location == null || location == "")// If File Not Exist.
            {
                return GetErrorMessageResult(); ;
            }
            var fileName = Path.GetFileName(location);

            //Save/Update downloadTimes table
            var OfficeId = SessionHelper.LoginUserOfficeID;
            if (getFeedbackRegisterDetails.Tables[0].Rows[0]["CVFileType"].ToString() == "msword")
            {
                return File(location, "application/doc", fileName);
            }

            if (getFeedbackRegisterDetails.Tables[0].Rows[0]["CVFileType"].ToString() == "vnd.openxm")
            {
                return File(location, "application/docx", fileName);
            }

            if (getFeedbackRegisterDetails.Tables[0].Rows[0]["CVFileType"].ToString() == "pdf")
            {
                return File(location, "application/pdf", fileName);
            }


            return File(location, "application/pdf", fileName); //dData.FileName
                                                                //return View();
        }



        public JsonResult GetFeedBackRegisterList(string EmployeeCode,  string Id, string IsChecked, string IsSolved, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                // string Ids = Convert.ToString(Id);

         


                List<FeedbackRegisterViewModel> List_ViewModel = new List<FeedbackRegisterViewModel>();
                var param = new { EmployeeCode = EmployeeCode };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_Get_CVDOWNLOAD_List");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new FeedbackRegisterViewModel
                {
                    FeedbackRegisterID = row.Field<long>("FeedbackRegisterID"),
                    EmployeeId = row.Field<long?>("EmployeeId"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    FeedbackDateSTR = row.Field<string>("FeedbackDate"),
                    FeedbackCategoryName = row.Field<string>("FeedbackCategoryName"),
                    FeedbackDescription = row.Field<string>("FeedbackDescription"),
                    ChkStatus = row.Field<string>("IsChecked"),
                    SolvedStatus = row.Field<string>("IsSolved"),
                    SolvedBy = row.Field<string>("SolvedBy"),
                    Remarks = row.Field<string>("Remarks"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),

                }).ToList();

                if (Id != null)
                {
                    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                }

                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function

        private List<View_EmployeeSalaryConfiguration> GenerateDataList(string employeeCode)
        {
            var empList = new List<View_EmployeeSalaryConfiguration>();

            var param = new { EmployeeCode = Convert.ToString(employeeCode) };
            var employeeData = employeeSpService.GetDataWithParameter(param, "prl.SP_GetPayroll_INCREMENT_EmployeebyEmployeeCode");

            var empdataList = employeeData.Tables[0].AsEnumerable()
            .Select(row => new EmployeeViewModel
            {
                OfficeId = row.Field<int>("OfficeId"),
                EmployeeId = row.Field<long>("EmployeeId"),
                EmployeeCode = row.Field<string>("EmployeeCode"),
                EmployeeName = row.Field<string>("EmployeeName"),
                EmployeeNameBng = row.Field<string>("EmployeeNameBng"),
                EmployeeTypeId = row.Field<int?>("EmployeeTypeId"),
                EmployeeStatusId = Convert.ToInt32(row.Field<int?>("EmployeeStatusId")),
                EmployeeStatusName = row.Field<string>("EmployeeStatusName"),
                EmployeeStatusValue = row.Field<string>("EmployeeStatusValue"),
                IsSalaryApplicable = row.Field<bool?>("IsSalaryApplicable"),
                DepartmentName = row.Field<string>("DepartmentName"),
                DesignationName = row.Field<string>("OffcDesignName"),
                FirstJoiningDateMsg = row.Field<string>("FirstJoiningDate"),
                ConfirmationDateMsg = row.Field<string>("ConfirmationDate"),
                BankAccountNo = row.Field<string>("BankAccountNo"),
                OfficeLocationId = row.Field<int>("OfficeLocationId"),
                PFTypeId = row.Field<int>("PFTypeId"),
                TotalEarnings = row.Field<decimal>("INCREMENTAMOUNT"),
                GrossSalary = row.Field<decimal>("GrossSalary"),
                Step = row.Field<int>("Step"),
                LastPromotionDtMsg = row.Field<string>("PromotionDateMsg"),
                GradeId = row.Field<int>("gradeid")
            }).ToList();

            foreach (var item in empdataList)
            {
                var data = new View_EmployeeSalaryConfiguration();
                data.OfficeID = Convert.ToInt32(item.OfficeId);
                data.EmployeeID = item.EmployeeId;
                data.PRComponentId = 0;
                data.EmployeeTypeName = "";
                data.ComponentGroupName = "";
                data.ComponentName = "";
                data.IsActive = true;
                data.CalculatedAmount = 0;
                data.ComponentType = "";
                data.RatioBasedOn = "";
                data.EmployeeTypeId = item.EmployeeTypeId == null ? 0 : item.EmployeeTypeId.Value;
                data.EffectiveStartDate = DateTime.Now.ToString("dd-MMM-yyyy");//DateTime.Today.ToString();
                var dateAdv = DateTime.Now.AddYears(3);
                data.EffectiveEndDate = dateAdv.ToString("dd-MMM-yyyy");
                data.GrossSalary = item.GrossSalary;
                data.BasicSalary = 0;
                data.BankAccountNo = "";
                data.Step = item.Step;
                data.GradeId = item.GradeId;
                data.LogInTime = "10:00:00";
                data.LogOutTime = "18:00:00";
                data.LastLoginTime = "10:00:00";
                data.IsOverTime = false;
                //data.OvertimeHour = 0;
                //data.IncrementMonth = 0;
                data.EmployeeCode = item.EmployeeCode;
                data.EmployeeName = item.EmployeeName;
                data.EmployeeNameBng = item.EmployeeNameBng;
                data.EmployeeStatusId = item.EmployeeStatusId;
                data.EmployeeStatusName = item.EmployeeStatusName;
                data.OfficeLocationId = item.OfficeLocationId;
                // data.EmployeeStatusName = ReturnEmployeeStatusReverse(item.EmployeeStatus.Trim());
                //data.DepartmentName = item.DepartmentName;
                //data.DesignationName = item.DesignationName;
                data.CalculatedAmount = (decimal)item.TotalEarnings;
                data.EmployeeNameBng = item.LastPromotionDtMsg;
                empList.Add(data);
            }
            return empList;
        }

        public JsonResult GetExistingSalaryConfigurationListbyEmployeeCode(string employeeCode)
        {
            try
            {
                var withResignEmployee = false;
                //get employee information
                var employeeInfo = employeeService.GetByCode(employeeCode.Trim(), withResignEmployee);

                if (employeeInfo == null)
                    return Json(new { Result = "ERROR", Message = "Employee not exist. Please try again!" }, JsonRequestBehavior.AllowGet);

                var officeInfo = officeService.Get(b => b.OfficeId == employeeInfo.OfficeId);
                var joiningDate = Convert.ToDateTime(employeeInfo.FirstJoiningDate).ToString("dd-MMM-yyyy");
                var confirmationDate = Convert.ToDateTime(employeeInfo.ConfirmationDate).ToString("dd-MMM-yyyy");

                var departmentName = employeeDepartmentService.GetById(Convert.ToInt32(employeeInfo.DepartmentId)).DepartmentName;
                var designationName = employeeDesignationService.GetById(Convert.ToInt32(employeeInfo.DesignationId)).DesignationName;

                //get employee promotion from [promo].[EmployeePromotion]
                var promotionInfo = employeePromotionService.GetPromotionInfo(employeeInfo.EmployeeId);

                var promotionDate = string.Empty;
                var nextReviewDate = string.Empty;
                if (promotionInfo != null)
                {
                    promotionDate = Convert.ToDateTime(promotionInfo.PromotionDate).ToString("dd-MMM-yyyy");
                    nextReviewDate = Convert.ToDateTime(promotionInfo.NextReviewDate).ToString("dd-MMM-yyyy");
                }

                var dataList = new List<View_EmployeeSalaryConfiguration>();
          
                dataList = GenerateDataList(employeeCode);
                

                var employeeStatusId = employeeInfo.EmployeeStatusId;
                //var employeeStatusId = employeeInfo.promo;

                var designationId = employeeInfo.DesignationId;
                var bankAccountNo = employeeInfo.BankAccountNo;
                var bankName = employeeInfo.BankName;
                var bankBranchName = employeeInfo.BankBranchName;
                var officeLocationId = officeInfo.OfficeLocationId;
                var officeId = officeInfo.OfficeId;
                var pfTypeId = employeeInfo.PFTypeId;
                var gradeId = employeeInfo.GradeId;

                return Json(new
                {
                    Result = "OK",
                    dataList,
                    Message = "OK",
                    JoiningDate = joiningDate,
                    ConfirmationDate = confirmationDate,
                    DepartmentName = departmentName,
                    DesignationName = designationName,
                    OfficeId = officeId,
                    OfficeLocationId = officeLocationId,
                    PromotionDate = promotionDate,
                    NextReviewDate = nextReviewDate,
                    EmployeeStausId = employeeStatusId,
                    DesignationId = designationId,
                    BankAccountNo = bankAccountNo,
                    BankName = bankName,
                    BankBranchName = bankBranchName,
                    PFTypeId = pfTypeId,
                    GradeId = gradeId,
                    IsOvertimeException = employeeInfo.IsOvertimeException,
                    PayrollConfigurationType = SessionHelper.PayrollConfigurationType

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = "ERROR" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetEmployeeData(string EmployeeCode)
        {
            List<FinalSattlementViewModel> List_Employee = new List<FinalSattlementViewModel>();
            var param = new { EmployeeCode = EmployeeCode };
            var empList = employeeSpService.GetDataWithParameter(param, "SP_Get_FinalSattlement");


            if (empList.Tables[0].Rows.Count > 0)
            {
                List_Employee = empList.Tables[0].AsEnumerable()
               .Select(row => new FinalSattlementViewModel
               {
                   EmployeeId = row.Field<long>("EmployeeId"),
                   EmployeeName = row.Field<string>("EmployeeName"),
                   FirstJoiningDateMsg = row.Field<string>("FirstJoiningDate"),
                   BatchNo = "ইউনিট প্রধান, হিসাব, প্রধান কার্যালয়, ঢাকা ।",
                   kMessage = "ইউনিট প্রধান, প্রশাসন , প্রধান কার্যালয়, ঢাকা ।",
                   BirthPlace = "চুড়ান্ত পাওনা পরিশোধ প্রসঙ্গে। ",
                   PermanentAddress = row.Field<string>("PermanentAddress"),
                   ConfirmationDateMsg = row.Field<string>("ConfirmationDate"),
                   OfficeName = row.Field<string>("OfficeName"),
                   DefaultRetiredCause = row.Field<string>("LASTSALARY"),
                   DefaultTerminationCause = row.Field<string>("COMMENTS"),

               }).ToList();
            }
            else
            {
                Response.StatusCode = 403;
            }

            return Json(List_Employee.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeDataFinalSattlment(string EmployeeCode)
        {
            List<FinalSattlementViewModel> List_Employee = new List<FinalSattlementViewModel>();
            var param = new { EmployeeCode = EmployeeCode };
            var empList = employeeSpService.GetDataWithParameter(param, "SP_Get_FinalSattlement");


            if (empList.Tables[0].Rows.Count > 0)
            {
                List_Employee = empList.Tables[0].AsEnumerable()
               .Select(row => new FinalSattlementViewModel
               {
                   EmployeeId = row.Field<int>("EmployeeId"),
                   EmployeeName = row.Field<string>("name"),
                   FirstJoiningDateMsg = row.Field<string>("jogdanertarikh"),
                   BatchNo = row.Field<string>("prapok"),
                   kMessage = row.Field<string>("prerok"),
                   BirthPlace = row.Field<string>("bishoy"),
                   PermanentAddress = row.Field<string>("paunadirhisahb"),
                   ConfirmationDateMsg = row.Field<string>("nishchitkorondate"),
                   OfficeName = row.Field<string>("kormoshthol"),
                   DefaultRetiredCause = row.Field<string>("sorbosheshbeton"),
                   DefaultTerminationCause = row.Field<string>("comments"),

                   Nationality = row.Field<string>("lastworkingday"),
                   EmployeeStatusValue = row.Field<string>("podotagpotrodakhilertarikh"),
                   OfficialEmail = row.Field<string>("podotagpotrokarjokortarikh"),
                   Marks = row.Field<string>("vhata"),
                   CompanyName = row.Field<string>("podotagpotrerbiboron"),
                   CompanyAddress = row.Field<string>("rilizordererbiboron"),
                   Section = row.Field<string>("providendfundejomakritonijertaka"),
                   IsSameAsPresentAddress = row.Field<string>("providenfundnijerjomakritotakarsud"),
                   AttendanceDateFrom = row.Field<string>("providendfundeprotishtanerobodan"),
                   PABXExtension = row.Field<string>("gratuitytohobilejomakritotaka"),
                   ETinNo = row.Field<string>("groupjibonbima"),
                   AddressDetail = row.Field<string>("chutirporibortebeton"),
                   EmployeeStatusName = row.Field<string>("bokyabeton"),
                   PresentAddressDetailForGuarantor = row.Field<string>("bokayeavata"),
                   PermanentAddressDetailForGuarantor = row.Field<string>("onnanobokyea"),
                   EmployeeImageBase64 = row.Field<string>("noticpay"),
                   ImageFilePath = row.Field<string>("mobileloan"),
                   EmployeeImageLink = row.Field<string>("providendfundloan"),
                   ReferenceORGuarantorDetail = row.Field<string>("mobilebill"),
                   BankAccountNo = row.Field<string>("onnannopauna"),
                   NationalId = row.Field<string>("providendfundeprotisthaneronudanersud"),
                   StatusDateForCertificate = row.Field<string>("tarikh"),

               }).ToList();
            }
            else
            {
                Response.StatusCode = 403;
            }

            return Json(List_Employee.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult test()
        {
            try
            {

                var mainReport = employeeSpService.GetDataWithoutParameter("[emp].[SP_EmployeeRegister]");
                var reportParam = new Dictionary<string, object>();

                var dtCompanyInfo = WebHelper.GetCompanyInfo();


                var reportPartialPath = "Employee/CrystalReport1.rpt";

                ReportHelper.PrintReport(reportPartialPath, mainReport.Tables[0], reportParam);

                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult EmployeeRegisterPrint(int ManualOfficeType)
        {
            try
            {
                var param = new { ManualOfficeTypeId = ManualOfficeType };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_EmployeeRegister");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", SessionHelper.CompanyName);
                reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                ReportHelper.PrintReport("Employee/EmployeeRegister.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EmployeeRegisterOfficeWisePrint()
        {
            try
            {
                var param = new { };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_EmployeeRegister");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", SessionHelper.CompanyName);
                reportParam.Add("CompanyAddress", SessionHelper.CompanyAddress);
                ReportHelper.PrintReport("Employee/EmployeeRegisterOfficeWise.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EmployeeEducationInformation(int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status, int DegreeLevelId, string DegreeTitle, string Concentration)
        {
            try
            {
                string format = "pdf";
                var param = new
                {
                    OfficeTypeId = OfficeTypeId,
                    OfficeId = OfficeId,
                    DesignationId = DesignationId,
                    EmployeeStatusArr = Status,
                    DepartmentId = DeptId,
                    SectionId = SectionId,
                    EmployeeRank = ResponsibilityId,
                    DegreeLevelId = DegreeLevelId,
                    DegreeTitle = DegreeTitle,
                    Concentration = Concentration
                };
                DataSet mainDataSource = employeeService.GetDataWithParameter(param, "emp.SP_EmployeeEducationInformation");
                var parameters = new Dictionary<string, object>();
                parameters.Add("CompanyName", SessionHelper.CompanyName);
                parameters.Add("CompanyAddress", SessionHelper.CompanyAddress);
                parameters.Add("CompanyCode", SessionHelper.CompanyCode);
                var reportDataSourceName = "EmployeeEducationInformation";
                string reportTitle = "Employee Education Information Report";
                string reportPath = "~/Reports/RDLC/Employee/EmployeeEducationInformation.rdlc";
                string reportViewMode = ReportViewModeConstants.Landscape;
                return Report(mainDataSource.Tables[0], reportDataSourceName, parameters, reportTitle, reportPath, format, "view", reportViewMode);
            }
            catch (Exception ex)
            {
                return Redirect("/CommonReportGenerator/CommonReportGenerationError");
            }
        }

        public ActionResult EmployeeEducationInformationSummary(int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status, int DegreeLevelId, string DegreeTitle, string Concentration)
        {
            try
            {
                string format = "pdf";
                var param = new
                {
                    OfficeTypeId = OfficeTypeId,
                    OfficeId = OfficeId,
                    DesignationId = DesignationId,
                    EmployeeStatusArr = Status,
                    DepartmentId = DeptId,
                    SectionId = SectionId,
                    EmployeeRank = ResponsibilityId,
                    DegreeLevelId = DegreeLevelId,
                    DegreeTitle = DegreeTitle,
                    Concentration = Concentration
                };
                DataSet mainDataSource = employeeService.GetDataWithParameter(param, "emp.SP_EmployeeEducationInformationSummary");
                var parameters = new Dictionary<string, object>();
                parameters.Add("CompanyName", SessionHelper.CompanyName);
                parameters.Add("CompanyAddress", SessionHelper.CompanyAddress);
                var reportDataSourceName = "EmployeeEducationInformationSummary";
                string reportTitle = "Employee Education Information Summary";
                string reportPath = "~/Reports/RDLC/Employee/EmployeeEducationInformationSummary.rdlc";
                string reportViewMode = ReportViewModeConstants.Potrait;
                return Report(mainDataSource.Tables[0], reportDataSourceName, parameters, reportTitle, reportPath, format, "view", reportViewMode);
            }
            catch (Exception ex)
            {
                return Redirect("/CommonReportGenerator/CommonReportGenerationError");
            }
        }




        public ActionResult EmployeeDistrictInformationReport(
            int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = ResponsibilityId.ToString() });
                PrintSSRSReport("/gHRMPlus_Reports/EmployeeDistrictInformation", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        public ActionResult FinalPaymentToBankReportPrintExcel(string IssueDate)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "IssueDate", Value = IssueDate });

                PrintSSRSMultiformat("excel", "/gHRMPlus_Reports/FinalPaymentDailyLedger", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult FinalPaymentToBankReportPrint(string IssueDate)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "IssueDate", Value = IssueDate });

                PrintSSRSReport("/gHRMPlus_Reports/FinalPaymentDailyLedger", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult CVCount()
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                PrintSSRSReport("/gHRMPlus_Reports/CVCount", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }



        public ActionResult EmployeeDistrictInformationSummaryReport(
            int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = ResponsibilityId.ToString() });
                PrintSSRSReport("/gHRMPlus_Reports/EmployeeDistrictInformationSummary", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult DepartmentWiseEmployeeInformationReport(
            int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyCode", Value = SessionHelper.CompanyCode });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = ResponsibilityId.ToString() });
                PrintSSRSReport("/gHRMPlus_Reports/DepartmentWiseEmployeeInformation", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult DelayConfirmationEmployeeListReport(
      int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = ResponsibilityId.ToString() });
                PrintSSRSReport("/gHRMPlus_Reports/DelayConfirmationEmployeeList", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult ConfirmationEligibleEmployeeList(
       int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = ResponsibilityId.ToString() });
                PrintSSRSReport("/gHRMPlus_Reports/ConfirmationEligibleEmployeeList", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult ConfirmationEligibleEmployeeSummary(
       int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = ResponsibilityId.ToString() });
                PrintSSRSReport("/gHRMPlus_Reports/ConfirmationEligibleEmployeeSummary", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult IncrementEligibleEmployeeList(int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = ResponsibilityId.ToString() });
                PrintSSRSReport("/gHRMPlus_Reports/IncrementEligibleEmployeeList", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult IncrementEligibleEmployeeSummary(int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = ResponsibilityId.ToString() });
                PrintSSRSReport("/gHRMPlus_Reports/IncrementEligibleEmployeeSummary", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult TransferableEmployeeList(int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = ResponsibilityId.ToString() });
                PrintSSRSReport("/gHRMPlus_Reports/TransferableEmployeeList", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult TransferableEmployeeSummary(int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = ResponsibilityId.ToString() });
                PrintSSRSReport("/gHRMPlus_Reports/TransferableEmployeeSummary", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult PromotionEligibleEmployeeList(int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = ResponsibilityId.ToString() });
                PrintSSRSReport("/gHRMPlus_Reports/PromotionEligibleEmployeeList", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult PromotionEligibleEmployeeSummary(int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = ResponsibilityId.ToString() });
                PrintSSRSReport("/gHRMPlus_Reports/PromotionEligibleEmployeeSummary", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult EmployeeGuarantorInformation(
            string OfficeTypeId, string OfficeId, string DesignationId, string ResponsibilityId, string DeptId, string SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (string.IsNullOrEmpty(OfficeTypeId) ? "0" : OfficeTypeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (string.IsNullOrEmpty(OfficeId) ? "0" : OfficeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = (string.IsNullOrEmpty(DesignationId) ? "0" : DesignationId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = (string.IsNullOrEmpty(DeptId) ? "0" : DeptId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = (string.IsNullOrEmpty(SectionId) ? "0" : SectionId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = (string.IsNullOrEmpty(ResponsibilityId) ? "0" : ResponsibilityId) });
                PrintSSRSReport("/gHRMPlus_Reports/EmployeeGuarantorInformation", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult EmployeePreviousWorkExperience(
            string OfficeTypeId, string OfficeId, string DesignationId, string ResponsibilityId, string DeptId, string SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (string.IsNullOrEmpty(OfficeTypeId) ? "0" : OfficeTypeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (string.IsNullOrEmpty(OfficeId) ? "0" : OfficeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = (string.IsNullOrEmpty(DesignationId) ? "0" : DesignationId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = (string.IsNullOrEmpty(DeptId) ? "0" : DeptId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = (string.IsNullOrEmpty(SectionId) ? "0" : SectionId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = (string.IsNullOrEmpty(ResponsibilityId) ? "0" : ResponsibilityId) });
                PrintSSRSReport("/gHRMPlus_Reports/EmployeePreviousWorkExperience", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        public ActionResult EmployeeNomineeReport(
          string OfficeTypeId, string OfficeId, string DesignationId, string ResponsibilityId, string DeptId, string SectionId, string Status, string EmployeeCode)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (string.IsNullOrEmpty(OfficeTypeId) ? "0" : OfficeTypeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (string.IsNullOrEmpty(OfficeId) ? "0" : OfficeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = (string.IsNullOrEmpty(DesignationId) ? "0" : DesignationId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = (string.IsNullOrEmpty(DeptId) ? "0" : DeptId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = (string.IsNullOrEmpty(SectionId) ? "0" : SectionId) });
                // paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = (string.IsNullOrEmpty(ResponsibilityId) ? "0" : ResponsibilityId) });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = "2022-05-05" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "ToDate", Value = "2022-05-05" });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = (string.IsNullOrEmpty(EmployeeCode) ? "0" : EmployeeCode) });

                PrintSSRSReport("/gHRMPlus_Reports/EmployeeNomineeReport", paramValues.ToArray());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // Gc Eligible for Confirmation 

        public ActionResult GcConfirmationEligibleEmployeeList(
  int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status, string DateFrom, string DateTo)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateFrom", Value = DateFrom.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateTo", Value = DateTo.ToString() });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = ResponsibilityId.ToString() });


                if (OfficeTypeId == 1)
                {
                    PrintSSRSReport("/gHRMPlus_Reports/GcConfirmationEligibleEmployeeList", paramValues.ToArray());
                }

                if (OfficeTypeId != 1)
                {
                    PrintSSRSReport("/gHRMPlus_Reports/GcFOConfirmationEligibleEmployeeList", paramValues.ToArray());
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GC Employee Separation List Report
        public ActionResult GcHoEmployeeSeparationList(
  int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status, string DateFrom, string DateTo)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateFrom", Value = DateFrom.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateTo", Value = DateTo.ToString() });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = ResponsibilityId.ToString() });


                if (OfficeTypeId == 1)
                {
                    PrintSSRSReport("/gHRMPlus_Reports/GC-HO Separation Employee List", paramValues.ToArray());
                }

                if (OfficeTypeId != 1)
                {
                    PrintSSRSReport("/gHRMPlus_Reports/GC-FO Separation Employee List", paramValues.ToArray());
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        // Employee Highest Education Report
        public ActionResult EmployeeHighestEducationReport(
  int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = ResponsibilityId.ToString() });

                PrintSSRSReport("/gHRMPlus_Reports/Employee Highest Education List", paramValues.ToArray());


                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        // Gender Wise Employee Details Report
        public ActionResult GenderWiseDetailsReport(
  int OfficeTypeId, int OfficeId, int DesignationId, int ResponsibilityId, int DeptId, int SectionId, string Status)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = OfficeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = Status.ToString() });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = DeptId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = SectionId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = ResponsibilityId.ToString() });

                PrintSSRSReport("/gHRMPlus_Reports/Gender Wise Employee List", paramValues.ToArray());


                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }




        [HttpPost]
        public ActionResult FinalSattlementSave(FinalSattlementViewModel model)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();

                var empid = db.Employees.Where(x => x.EmployeeCode == model.EmployeeCode).Select(z => z.EmployeeId).FirstOrDefault();

                var param = new
                {
                    CompanyName = SessionHelper.CompanyName,
                    CompanyAddress = SessionHelper.CompanyAddress,
                    EmployeeId = empid,
                    EmployeeCode = model.EmployeeCode,
                    tarikh = model.Date == null ? "" : model.Date,
                    prapok = model.BatchNo == null ? "" : model.BatchNo,
                    prerok = model.kMessage == null ? "" : model.kMessage,
                    bishoy = model.BirthPlace == null ? "" : model.BirthPlace,
                    paunadirhisahb = model.OfficeTypeName == null ? "" : model.OfficeTypeName,
                    name = model.EmployeeName == null ? "" : model.EmployeeName,
                    kormoshthol = model.PassportNo == null ? "" : model.PassportNo,
                    jogdanertarikh = model.FirstJoiningDateMsg == null ? "" : model.FirstJoiningDateMsg,
                    nishchitkorondate = model.EmployeeStatusValue == null ? "" : model.EmployeeStatusValue,
                    podotagpotrodakhilertarikh = model.PermanentAddress == null ? "" : model.PermanentAddress,
                    podotagpotrokarjokortarikh = model.OfficialEmail == null ? "" : model.OfficialEmail,
                    sorbosheshbeton = model.BloodGroup == null ? "" : model.BloodGroup,
                    vhata = model.Marks == null ? "" : model.Marks,
                    podotagpotrerbiboron = model.CompanyName == null ? "" : model.CompanyName,
                    rilizordererbiboron = model.CompanyAddress == null ? "" : model.CompanyAddress,
                    providendfundejomakritonijertaka = model.Section == null ? "" : model.Section,
                    providenfundnijerjomakritotakarsud = model.IsSameAsPresentAddress == null ? "" : model.IsSameAsPresentAddress,
                    providendfundeprotishtanerobodan = model.AttendanceDateFrom == null ? "" : model.AttendanceDateFrom,
                    gratuitytohobilejomakritotaka = model.PABXExtension == null ? "" : model.PABXExtension,
                    groupjibonbima = model.ETinNo == null ? "" : model.ETinNo,
                    chutirporibortebeton = model.AddressDetail == null ? "" : model.AddressDetail,
                    bokyabeton = model.EmployeeStatusName == null ? "" : model.EmployeeStatusName,
                    bokayeavata = model.PresentAddressDetailForGuarantor == null ? "" : model.PresentAddressDetailForGuarantor,
                    onnanobokyea = model.PermanentAddressDetailForGuarantor == null ? "" : model.PermanentAddressDetailForGuarantor,
                    noticpay = model.EmployeeImageBase64 == null ? "" : model.EmployeeImageBase64,
                    mobileloan = model.ImageFilePath == null ? "" : model.ImageFilePath,
                    providendfundloan = model.EmployeeImageLink == null ? "" : model.EmployeeImageLink,
                    mobilebill = model.ReferenceORGuarantorDetail == null ? "" : model.ReferenceORGuarantorDetail,
                    onnannopauna = model.BankAccountNo == null ? "" : model.BankAccountNo,
                    comments = model.BirthCertificateNo == null ? "" : model.BirthCertificateNo,
                    firstsignaturename = "(নারগিস আখতার)",
                    firstsignaturedesignation = "সিনিয়র ম্যানেজার",
                    secondsignaturename = "(মোঃ মাহবুব আলম )",
                    secondsignaturedesg = "ডেপুটি ম্যানেজার, প্রশাসন",
                    unitprodan = "পরামর্শক, প্রশাসন ",
                    lastworkingday = model.Nationality == null ? "" : model.Nationality,
                    providendfundeprotisthaneronudanersud = model.NationalId == null ? "" : model.NationalId,
                };
                var empList = employeeSpService.GetDataWithParameter(param, "FinalSattlementSave");

                var jsonResult = Json("sucess", JsonRequestBehavior.AllowGet);
                return jsonResult;


            }
            catch (Exception ex)
            {
                return Content("<b>error</b><br />" + ex.Message);
                // return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult FinalSattlementReport(string EmployeeCode)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = EmployeeCode });

                PrintSSRSReport("/gHRMPlus_Reports/FinalSattlementReport_GC", paramValues.ToArray());
                return Content(string.Empty);


            }
            catch (Exception ex)
            {
                return Content("<b>error</b><br />" + ex.Message);
                // return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        // Get Join List 

        public JsonResult GetFinalSattlementReportList(int jtStartIndex, int jtPageSize, string jtSorting, string EmployeeCode )
        {

            try
            {
                List<FinalSattlementViewModel> List_Employee = new List<FinalSattlementViewModel>();
                if(EmployeeCode == null || EmployeeCode == "")
                {
                    EmployeeCode = "0";
                }
                var param = new { EmployeeCode = EmployeeCode };
                var empList = employeeSpService.GetDataWithParameter(param, "FinalSattlementReportGrid");


                if (empList.Tables[0].Rows.Count > 0)
                {
                    List_Employee = empList.Tables[0].AsEnumerable()
                   .Select(row => new FinalSattlementViewModel
                   {
                       Id = row.Field<int>("id"),
                       HeadOfficeId = row.Field<Int64>("SL"),
                       EmployeeId = row.Field<int>("EmployeeId"),
                       EmployeeCode = row.Field<string>("EmployeeCode"),
                       EmployeeName = row.Field<string>("name"),
                       FirstJoiningDateMsg = row.Field<string>("jogdanertarikh"),
                       BatchNo = row.Field<string>("tarikh"),
                       kMessage = row.Field<string>("nishchitkorondate"),
                       BirthPlace = row.Field<string>("lastworkingday"),
                       //PermanentAddress = row.Field<string>("podotagpotrodakhilertarikh"),
                       //ConfirmationDateMsg = row.Field<string>("ConfirmationDate"),
                       //OfficeName = row.Field<string>("OfficeName"),
                       //DefaultRetiredCause = row.Field<string>("LASTSALARY"),
                       //DefaultTerminationCause = row.Field<string>("COMMENTS"),

                   }).ToList();
                }
                else
                {
                    Response.StatusCode = 403;
                }
                var currentPageRecords = List_Employee.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_Employee.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }






        // YPSA Report 
        public ActionResult EmployeeETinInfoReport(
  string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status, int type, string DateFrom, string DateTo)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                string ReportName = "";
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (String.IsNullOrEmpty(officeTypeId) ? "0" : officeTypeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (String.IsNullOrEmpty(OfficeId) ? "0" : OfficeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = (String.IsNullOrEmpty(payRollDesignation) ? "0" : payRollDesignation) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = (String.IsNullOrEmpty(status) ? "" : status) });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = (String.IsNullOrEmpty(DeptId) ? "0" : DeptId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = (String.IsNullOrEmpty(Section) ? "0" : Section) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = "0" });

                ReportName = db.EmployeeReportOption.Where(z => z.EmpReportTypeId == type).Select(x => x.EmpReportTypeName).FirstOrDefault();

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "ReportName", Value = ReportName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "ReportType", Value = type });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateFrom", Value = DateFrom });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateTo", Value = DateTo });

                if (type == 41)
                {
                    PrintSSRSReport("/gHRMPlus_Reports/OfficeWiseEmployeeRectuitmentVsDropput", paramValues.ToArray());
                }
                else if (type == 40)
                {
                    PrintSSRSReport("/gHRMPlus_Reports/OfficeWiseEmployeeRectuitment", paramValues.ToArray());
                }
                else if (type == 39)
                {
                    PrintSSRSReport("/gHRMPlus_Reports/DesignationWiseEmployeeRectuitment", paramValues.ToArray());
                }
                else if (type == 38)
                {
                    PrintSSRSReport("/gHRMPlus_Reports/EmployeeDesignationWiseReportProposedNewReportFormat", paramValues.ToArray());
                }
                else if (type == 37)
                {
                    PrintSSRSReport("/gHRMPlus_Reports/MonthWiseEmployeeConfirmation", paramValues.ToArray());
                }
                else if (type == 36)
                {
                    PrintSSRSReport("/gHRMPlus_Reports/Employee_Dropout_ReportByReason_Resign", paramValues.ToArray());
                }

                else if (type == 30)
                {
                    PrintSSRSReport("/gHRMPlus_Reports/Employee_E_TIN_Info", paramValues.ToArray());
                }
                else if (type == 31)
                {
                    PrintSSRSReport("/gHRMPlus_Reports/PromotionDetailsYPSA", paramValues.ToArray());
                }
                else if (type == 32)
                {
                    PrintSSRSReport("/gHRMPlus_Reports/TransferDetailsYPSA", paramValues.ToArray());
                }
                else if (type == 33)
                {
                    PrintSSRSReport("/gHRMPlus_Reports/ActiveEmployee_E_TIN_Info", paramValues.ToArray());
                }
                else if (type == 34)
                {
                    PrintSSRSReport("/gHRMPlus_Reports/ActiveEmployeeInfoByDesignation", paramValues.ToArray());
                }
                else if (type == 35)
                {
                    PrintSSRSReport("/gHRMPlus_Reports/DepartmentWiseEmployeeReportWithCount", paramValues.ToArray());
                }



                else
                {
                    PrintSSRSReport("/gHRMPlus_Reports/Employee_E_TIN_Info", paramValues.ToArray());  /// 31
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }



        // Employee E-TIN Report
        public ActionResult MonthWiseEmplyeeConfimationYPSA(
            string DateFrom, string DateTo, string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status, int type)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                string ReportName = "";

                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = (String.IsNullOrEmpty(officeTypeId) ? "0" : officeTypeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = (String.IsNullOrEmpty(OfficeId) ? "0" : OfficeId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = (String.IsNullOrEmpty(payRollDesignation) ? "0" : payRollDesignation) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = (String.IsNullOrEmpty(status) ? "" : status) });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = (String.IsNullOrEmpty(DeptId) ? "0" : DeptId) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = (String.IsNullOrEmpty(Section) ? "0" : Section) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeRank", Value = "0" });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateFrom", Value = (String.IsNullOrEmpty(DateFrom)) });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateTo", Value = (String.IsNullOrEmpty(DateTo)) });


                ReportName = db.EmployeeReportOption.Where(z => z.EmpReportTypeId == type).Select(x => x.EmpReportTypeName).FirstOrDefault();

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "ReportName", Value = ReportName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "ReportType", Value = type });

                PrintSSRSReport("/gHRMPlus_Reports/MonthWiseEmployeeConfirmation", paramValues.ToArray());


                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }



        //EmpReportTypeId 42
        public ActionResult EmployeeDemographicReportForDetails(string bloodGroup, string officeTypeId, string OfficeId, string DeptId, string payRollDesignation, string responsibility, string Section, string status)
        {
            try
            {
                var param = new
                {
                    OfficeTypeId = string.IsNullOrEmpty(officeTypeId) ? 0 : Convert.ToInt32(officeTypeId),
                    OfficeId = string.IsNullOrEmpty(OfficeId) ? 0 : Convert.ToInt32(OfficeId),
                    DesignationId = string.IsNullOrEmpty(payRollDesignation) ? 0 : Convert.ToInt32(payRollDesignation),
                    EmployeeStatusArr = status,
                    DepartmentId = string.IsNullOrEmpty(DeptId) ? 0 : Convert.ToInt32(DeptId),
                    SectionId = string.IsNullOrEmpty(Section) ? 0 : Convert.ToInt32(Section),
                    BloodGroup = "0" == bloodGroup ? "" : bloodGroup,
                    EmployeeRank = responsibility
                };
                var mainReport = employeeSpService.GetDataWithParameter(param, "emp.SP_RPT_Employee_Demographic_Info_Details");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Employee/EmployeeDemographicReport_For_Details.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }




    }
}