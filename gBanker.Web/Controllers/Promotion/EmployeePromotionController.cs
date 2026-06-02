
#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Text;
using gHRM.Web.CommonDropdown;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels;
using gHRM.Data.CodeFirstMigration.EmployeePromotion;
using gHRM.Web.Helpers;
using System.Web;
using System.IO;
using System.Data.OleDb;
using gHRM.Data.DBDetailModels.Promotions;
using gHRM.Core.Utilities;
using System.Globalization;
using gHRM.Core.Utilities.Constants;
using gHRM.Web.ViewModels.Payroll;
using gHRM.Data.CodeFirstMigration.Promotion;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Service.Payroll;
using gHRM.Web.Reports.Promotion;

#endregion

namespace gHRM.Web.Controllers
{
    public class EmployeePromotionController : BaseController
    {  

        #region Private Variables
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IOfficeTypeService officeTypeService;
        private readonly IOfficeService officeService;
        private readonly IEmployeeDesignationService employeeDesignationService;
        private readonly IEmployeeGradeListService employeeGradeListService;
        private readonly IPromotionTypeService promotionTypeService;
        private readonly IEmployeePromotionService employeePromotionService;
        private readonly IEmployeeDepartmentService employeeDepartmentService;
        private readonly IView_EmployeeSalaryConfigurationService viewSalaryConfigurationService;
        private CommonStaticDropDown commonStaticDropDown;
        private CommonDynamicDropDown CommonDynamicDropDown;
        #endregion

        #region Ctor
        public EmployeePromotionController(
              IEmployeeService employeeService
            , IEmployeeSPService employeeSPService
            , IOfficeTypeService officeTypeService
            , IOfficeService officeService
            , IEmployeeDesignationService employeeDesignationService
            , IEmployeeGradeListService employeeGradeListService
            , IPromotionTypeService promotionTypeService
            , IEmployeePromotionService employeePromotionService
            , IEmployeeDepartmentService employeeDepartmentService
            , IView_EmployeeSalaryConfigurationService viewSalaryConfigurationService
            )
        {
            this.employeeService = employeeService;
            this.employeeSPService = employeeSPService;
            this.officeTypeService = officeTypeService;
            this.officeService = officeService;
            this.employeeDesignationService = employeeDesignationService;
            this.employeeGradeListService = employeeGradeListService;
            this.promotionTypeService = promotionTypeService;
            this.employeePromotionService = employeePromotionService;
            this.employeeDepartmentService = employeeDepartmentService;
            this.viewSalaryConfigurationService = viewSalaryConfigurationService;

            commonStaticDropDown = new CommonStaticDropDown();
            CommonDynamicDropDown = new CommonDynamicDropDown();
        }

        #endregion

        #region Actions

        // GET: EmployeePromotion
        public ActionResult Index()
        {
            List<SelectListItem> items2 = new List<SelectListItem>();
            ViewData["DesignationList"] = items2;

            var model = new EmployeePromotionViewModel();
            MapDropDown(model);

            ViewData["Months"] = commonStaticDropDown.MonthList();
            ViewData["Years"] = commonStaticDropDown.YearList(10,20);

            return View(model);
        }


        public ActionResult Index2()
        {
            List<SelectListItem> items2 = new List<SelectListItem>();
            ViewData["DesignationList"] = items2;

            var model = new EmployeePromotionViewModel();
            MapDropDown(model);

            ViewData["Months"] = commonStaticDropDown.MonthList();
            ViewData["Years"] = commonStaticDropDown.YearList(10, 20);

            return View(model);
        }

        public ActionResult PromotionReport()
        {
            EmployeePromotionReportViewModel model = new EmployeePromotionReportViewModel();
            MapReportDropDown(model);
            return View(model);
        }

        #endregion

        #region Http Request

        public JsonResult GetPromotionEligibleEmployees([DataSourceRequest]DataSourceRequest request, string Year, string MonthName, string PromotionTypeId, string DesignationId)
        {
            try
            {
                var empList = new DataSet();

                if (PromotionTypeId == PromotionTypeValueConstants.FirstJoining)
                {
                    var startDate = DateTime.Parse($"01-{MonthName}-{Year}");
                    var endDate = startDate.AddMonths(1).AddDays(-1);
                    var param = new { @DesignationId = DesignationId, @StartDate = startDate.ToString("dd-MMM-yyyy",CultureInfo.InvariantCulture), @EndDate = endDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) };
                    empList = employeeSPService.GetDataWithParameter(param, "promo.EmployeePromotion_GetPromotionFirstJoiningEligibleEmployees");
                }
                else
                {
                    //get parameters
                    StringBuilder sb = GetParameters(Year, MonthName, PromotionTypeId, DesignationId);
                    var param = new { @AndCondition = sb.ToString() };
                    empList = employeeSPService.GetDataWithParameter(param, "promo.SP_GetPromotionEligibleEmployees");
                }

                var EligibleList = empList.Tables[0].AsEnumerable()
                .Select(row => new EmployeePromotionViewModel
                {
                    rowSl = row.Field<int>("rowSl"),
                    PromotionId = row.Field<long>("PromotionId"),
                    EmployeeId = row.Field<long>("EmployeeID"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    OfficeName = row.Field<string>("OfficeName"),
                    DesignationName = row.Field<string>("DesignationName"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    PromotionTypeName = row.Field<string>("PromotionTypeName"),
                    NextReviewDateMsg = row.Field<string>("NextReviewDateMsg")
                    //FirstJoiningDate = row.Field<string>("FirstJoiningDate"),
                    // EmployeeTypeName = row.Field<string>("EmployeeTypeName"),
                    //LastPromotionDate = row.Field<string>("LastPromotionDate"),
                    //OfficeLocationName = row.Field<string>("OfficeLocationName")
                }).ToList();

                DataSourceResult result = EligibleList.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        } // End Function       

        private StringBuilder GetParameters(string Year, string MonthName, string PromotionTypeId, string DesignationId)
        {
            //Get next promotion date
            string selectedDate = GetNextPromotionDate(Year, MonthName);

            StringBuilder sb = new StringBuilder();

            if (DesignationId != null && DesignationId != "" && DesignationId != "0")
            {
                sb.Append(" AND E.DesignationId =  " + DesignationId);
            }

            if (PromotionTypeId != null && PromotionTypeId != "" && PromotionTypeId != "0")
            {
                sb.Append(" AND E.PromotionTypeId =  " + PromotionTypeId);
            }

            sb.Append("AND E.NextReviewDate <=  '" + selectedDate + "' ");

            return sb;
        }

        public JsonResult RejectPromotion(int PromotionId, DateTime NextReviewDate, string Remarks)
        {
            int result = 0;
            string message = string.Empty;
            try
            {
                //var param = new { @PromotionId= PromotionId, @EmployeeId = EmployeeId, @NextReviewDate = NextReviewDate, @Remarks = Remarks, @CreateUser = LoggedInEmployeeId };
                //var empList = employeeSPService.GetDataWithParameter(param, "promo.SP_NextPromotionReview");
                //return Json(new { Message = "Promotion Rejection executed successfully" }, JsonRequestBehavior.AllowGet);
                var entity = new EmployeePromotion();
                entity = employeePromotionService.GetById(PromotionId);
                entity.NextReviewDate = NextReviewDate;
                entity.Remarks = Remarks;
                entity.IsActive = true;
                entity.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                entity.UpdateDate = DateTime.UtcNow;
                employeePromotionService.Update(entity);
                result = 1;
                message = "Update Successfully";
            }
            catch (Exception ex)
            {
                message = "Update Denied";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        } // End Function

        public JsonResult GetDesignationList()
        {
            var designationLists = employeeDesignationService.GetMany(p => p.IsActive == true);
            var viewDesignationLists = designationLists.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.DesignationName, m.DesignationCode), Value = m.DesignationId.ToString() });
            var desig_item = new List<SelectListItem>();
            desig_item.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            desig_item.AddRange(viewDesignationLists);
            // model.DesignationList = desig_item;

            return Json(desig_item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeePromotion(int promotionId, string employeeCode)
        {
            var employeePromotion = "";

            return Json(new { employeePromotion = employeePromotion }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeeAssessmentByJoinDate([DataSourceRequest] DataSourceRequest request
           , int? year, string from, string to, int? officetype)
        {
            try
            {
                List<EmployeePromotionViewModelNew> lst = new List<EmployeePromotionViewModelNew>();
                var empList = new gHRMDBContext().Database.SqlQuery<EmployeePromotionViewModelNew>("[promo].[sp_EmployeeServiceInfo] '" + from + "-" + year + "','" + to + "-" + year +  "'," + year  +"," + officetype + "")
                    //.Select(s=>new {s.EmployeeId,s.EmployeeCode,s.EmployeeName} )
                    .ToList();
                DataSourceResult result = empList.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        #endregion

        #region Import Promotion Backlog

        [HttpGet]
        public ActionResult Import()
        {
            return Redirect("/ExcelImport/PromotionBacklog");
        }

        [HttpPost]
        public ActionResult ImportBacklog()
        {
            try
            {
                string validationMessage;
                //var importEmployeeErrorList = "";

                var isAjax = Request.IsAjaxRequest();

                if (!ModelState.IsValid)
                    return Json(new { type = "warning", errorLisings = false, message = "Error on file, Please try again" },
                               JsonRequestBehavior.AllowGet);

                if (Request.Files.Count <= 0)
                    return Json(new { type = "warning", errorLisings = false, message = "File not found. Please try again." },
                             JsonRequestBehavior.AllowGet);

                var file = Request.Files[0];

                // Generate dataset
                var ds = GetMemberDatasetFromFile(file, out validationMessage);

                if (ds == null)
                {
                    return Json(new { type = "warning", errorLisings = false, message = validationMessage },
                              JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrWhiteSpace(validationMessage))
                {
                    return Json(new { type = "warning", errorLisings = false, message = validationMessage },
                              JsonRequestBehavior.AllowGet);
                }

                var promotionBackLogImportList = new List<PromotionBackLogImportModel>();
                long createdBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);

                // Generate member list
                validationMessage = GeneratePromotionBackLogList(promotionBackLogImportList, createdBy, ds);

                if (promotionBackLogImportList.Count == 0 &&
                    !string.IsNullOrWhiteSpace(validationMessage))
                {
                    return Json(new
                    {
                        type = "warning",
                        errorLisings = true,
                        message = "Error occurred. Please seee details in validation message section."
                    }, JsonRequestBehavior.AllowGet);
                }

                if (promotionBackLogImportList.Count == 0)
                    return Json(new { type = "warning", errorLisings = false, message = "No promotion records were found to import." },
                              JsonRequestBehavior.AllowGet);

                var isAdded = employeePromotionService.BulkPromotionBackLogAdd(promotionBackLogImportList);
                if (!isAdded)
                    return Json(new { type = "warning", message = "There was an error while adding import existing promotion. Please try with valid excel data!" },
                             JsonRequestBehavior.AllowGet);

                return Json(new { type = "success", message = "Import existing promotion successfull!." },
                              JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { type = "warning", message = "There was an error while adding import existing promotion. Please try with valid excel data!" },
                            JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ImportConfirmation()
        {
            return View();
        }

        #endregion

        #region Ajax Calls


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
                //get existing salary configuration data [prl.View_EmployeeSalaryConfiguration]
                dataList = viewSalaryConfigurationService.GetEmployeeSalaryConfigurationListbyCode(employeeCode);

                //if not exist salary configuration data then generate fly data listing
                if (dataList.Count <= 0)
                {
                    //generate fly data listing for salary configuration
                    dataList = GenerateDataList(employeeCode);
                }

                var employeeStatusId = employeeInfo.EmployeeStatusId;
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

        [HttpPost]
        public JsonResult AssessmentApprovalorReject(
            long? PromotionId, long? eid, int? proTypeID, DateTime? promotiondate, int? digID, string remark, string status)
        {
            //var proObj = employeePromotionService.GetMany(x => x.IsActive && !x.IsReviewed && (string.IsNullOrEmpty(x.PromotionStatus)));
            //if (proObj.Any())
            //    return Json("Previous bot not completed data found", JsonRequestBehavior.AllowGet);
            //else
            //{
            if (status == "approved")
            {
                    string msg = (PromotionId ?? 0) == 0 ? "Score data is required" : (proTypeID ?? 0) == 0 ? "Assessment data is required":"";
                    if (string.IsNullOrEmpty(msg))
                    {
                        var typeObj = new PromotionType();
                        var typeLst = new gHRMDBContext().Database.SqlQuery<PromotionType>("SELECT * FROM [promo].[PromotionType] WHERE PromotionTypeId=" + proTypeID ?? 0 + ""); //promotionTypeService.GetById(proTypeID ?? 0);
                        if (typeLst.Any()) typeObj = typeLst.First();
                        if (typeObj != null)
                        {
                            var Lst = employeePromotionService.GetMany(x => x.PromotionId == PromotionId && x.EmployeeId == eid && x.PromotionStatus == "Pending").ToList();
                            if (Lst.Any())
                            {
                                var emp = employeeService.GetByEmpId(eid ?? 0);
                                if (typeObj.PromotionTypeValue != "PM") /*no Promotion*/
                                    digID = emp.DesignationId;
                                else
                                {
                                    emp.DesignationId = digID;
                                    emp.UpdateDate = DateTime.Now;
                                    emp.UpdateUser = SessionHelper.LoggedInEmployeeID;
                                    employeeService.Update(emp);
                                }

                                var obj_pro = Lst.First();
                                obj_pro.PromotionDate = promotiondate;
                                obj_pro.PromotionStatus = "approved";
                                obj_pro.PromotionTypeId = proTypeID ?? 0;
                                obj_pro.Remarks = remark;
                                obj_pro.UpdateDate = DateTime.Now;
                                obj_pro.UpdateUser = SessionHelper.LoggedInEmployeeID;
                                employeePromotionService.Update(obj_pro);
                            }
                            return Json(new { result = 1, message = status + " is completed" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else return Json(new { result = 0, message = msg }, JsonRequestBehavior.AllowGet);


                }
            else if (status == "rejected")
            {
                var Lst = employeePromotionService.GetMany(x => x.PromotionId == PromotionId && x.EmployeeId == eid && x.PromotionStatus == "Pending").ToList();
                if (Lst.Any())
                {
                    var obj_pro = Lst.First();
                    obj_pro.PromotionStatus = "rejected";
                    obj_pro.Remarks = remark;
                    obj_pro.UpdateDate = DateTime.Now;
                    obj_pro.UpdateUser = SessionHelper.LoggedInEmployeeID;
                    obj_pro.IsActive = false;
                    employeePromotionService.Update(obj_pro);
                }
            }
            return Json(new { result = 1, message = status + " is completed" }, JsonRequestBehavior.AllowGet);
            //}
        }
        #endregion

        #region Private Methods

        private List<View_EmployeeSalaryConfiguration> GenerateDataList(string employeeCode)
        {
            var empList = new List<View_EmployeeSalaryConfiguration>();

            var param = new { EmployeeCode = Convert.ToString(employeeCode) };
            var employeeData = employeeSPService.GetDataWithParameter(param, "prl.SP_GetPayroll_EmployeebyEmployeeCode");

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
                PFTypeId = row.Field<int>("PFTypeId")
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
                data.GrossSalary = 0;
                data.BasicSalary = 0;
                data.BankAccountNo = "";
                data.Step = 0;
                data.GradeId = 0;
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

                empList.Add(data);
            }
            return empList;
        }

        private DataSet GetMemberDatasetFromFile(HttpPostedFileBase file, out string validationMessage)
        {
            var ds = new DataSet();

            validationMessage = "";

            if (file != null && file.ContentLength > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    var ticks = DateTime.Now.Ticks;

                    var serverMappedPath = Server.MapPath("~/WebShared/Uploads/PromotionBacklog/");
                    var fileLocation = $"{serverMappedPath}{ticks}/{file.FileName}";
                    var directory = $"{serverMappedPath}{ticks}";

                    try
                    {
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }

                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        file.SaveAs(fileLocation);
                    }
                    catch
                    {
                        validationMessage = "Error on processing file, Please try again";
                        return null;
                    }

                    var excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="
                        + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

                    //Create Connection to Excel work book and add oledb namespace
                    var excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();

                    var dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    if (dt == null)
                    {
                        validationMessage = "Error on processing file, Please try again";
                        return null;
                    }

                    var excelSheets = new string[dt.Rows.Count];
                    var t = 0;

                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    var excelConnection1 = new OleDbConnection(excelConnectionString);


                    var query = string.Format("Select * from [{0}]", "Promotion$");

                    using (var dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }

                    excelConnection.Close();
                }
                else
                {
                    validationMessage = "Error! Please import an correct file. You can download the sample file & try again.";
                    return null;
                }
            }
            else
            {
                validationMessage = "Error on file. Please try again.";
                return null;
            }

            return ds;
        }

        private string GeneratePromotionBackLogList(ICollection<PromotionBackLogImportModel> promotionList,
                                                    long createdBy,
                                                    DataSet ds)
        {
            var validationMessage = "";

            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows == null)
            {
                return "There is an issue reading data from this file. Please try again.";
            }

            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var j = 0;
                var errorMessage = "";
                var newPromotionBackLog = new PromotionBackLogImportModel();

                //employee code
                var employeeCode = ds.Tables[0].Rows[i][j++].ToString();

                if (string.IsNullOrWhiteSpace(employeeCode))
                    continue;               

                employeeCode = employeeCode.Replace("\"", "");
                newPromotionBackLog.EmployeeCode = CommonHelper.GetFormattedEmployeeCodeWithFourDigit(employeeCode);

                //for test
                //if (employeeCode == "0053")
                //    employeeCode = employeeCode;

                //get employee info
                var employee = employeeService.GetByCode(newPromotionBackLog.EmployeeCode);

                if (employee != null)
                    newPromotionBackLog.EmployeeId = employee.EmployeeId;
                else
                {
                    errorMessage += " Error: Employee not found in the file. " +
                                     "Row is " + (1 + i) + " and column is " + j;
                }

                //payroll designation
                var payrollDesignation = ds.Tables[0].Rows[i][j++].ToString();

                if (!string.IsNullOrWhiteSpace(payrollDesignation))
                {
                    newPromotionBackLog.PayrollDesignation = payrollDesignation;
                }
                else
                {
                    errorMessage += " Error: Payroll Designation not found in the file. " +
                                         "Row is " + (1 + i) + " and column is " + j;
                }

                //promotion type
                var promotionType = ds.Tables[0].Rows[i][j++].ToString();
                if (!string.IsNullOrWhiteSpace(promotionType))
                {
                    newPromotionBackLog.PromotionType = promotionType;
                }
                else
                {
                    newPromotionBackLog.PromotionType = PromotionTypeConstants.FirstJoining;
                    /*
                    errorMessage += " Error: promotion type not found in the file. " +
                                         "Row is " + (1 + i) + " and column is " + j;
                    */
                }

                // promotion date 
                var promotionDate = ds.Tables[0].Rows[i][j++].ToString();
                if (!string.IsNullOrWhiteSpace(promotionDate))
                {
                    promotionDate = promotionDate.Split(' ')[0];
                    try
                    {
                        try
                        {
                            newPromotionBackLog.PromotionDate = DateTime.ParseExact(promotionDate, "M/d/yyyy", CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            newPromotionBackLog.PromotionDate = DateTime.ParseExact(promotionDate, "d/M/yyyy", CultureInfo.InvariantCulture);
                        }
                    }
                    catch
                    { 
                    
                    }

                }
                else
                {
                    newPromotionBackLog.PromotionDate = employee.FirstJoiningDate;

                    /*
                    errorMessage += " Error: promotion date not found in the file. " +
                                         "Row is " + (1 + i) + " and column is " + j;
                    */
                }

                // duration
                var duration = ds.Tables[0].Rows[i][j++].ToString();
                if (!string.IsNullOrWhiteSpace(duration))
                {
                    newPromotionBackLog.DurationInMonth = duration;
                    newPromotionBackLog.NextReviewDate = ((DateTime)newPromotionBackLog.PromotionDate).AddMonths(Convert.ToInt32(duration));
                }
                else
                {

                    errorMessage += " Error: promotion duration not found in the file. " +
                                         "Row is " + (1 + i) + " and column is " + j;
                }

                // grosssalary
                var grossSalary = ds.Tables[0].Rows[i][j++].ToString();
                if (!string.IsNullOrWhiteSpace(grossSalary))
                {
                    newPromotionBackLog.GrossSalary = Convert.ToDecimal(grossSalary);

                    //populate promotion backlog salary amount
                    PopulatePromotionBackLogSalaryAmount(newPromotionBackLog);
                }

                newPromotionBackLog.IsActive = true;
                newPromotionBackLog.CreateUser = createdBy;
                newPromotionBackLog.CreateDate = DateTime.Now;

                if (string.IsNullOrEmpty(errorMessage))
                {
                    promotionList.Add(newPromotionBackLog);
                }
                else
                {
                    var newEmployeePromotionFail = new EmployeePromotionFail
                    {
                        FailReason = errorMessage,
                        IsActive = true,
                        CreateUser = createdBy,
                        CreateDate = DateTime.Now
                    };

                    using (var db = new gHRMDBContext())
                    {
                        db.EmployeePromotionFails.Add(newEmployeePromotionFail);
                        db.SaveChanges();
                    }

                    validationMessage += errorMessage;
                }
            }

            return validationMessage;
        }

        private void PopulatePromotionBackLogSalaryAmount(PromotionBackLogImportModel newPromotionBackLog)
        {
            var basicAmount = (newPromotionBackLog.GrossSalary * 55) / 100;
            var houseRent = (newPromotionBackLog.GrossSalary * 30) / 100;
            var medicalAmount = (newPromotionBackLog.GrossSalary * 10) / 100;
            var conveyanceAmount = (newPromotionBackLog.GrossSalary * 5) / 100;

            newPromotionBackLog.BasicSalary = basicAmount;
            newPromotionBackLog.HouseRent = houseRent;
            newPromotionBackLog.Medical = medicalAmount;
            newPromotionBackLog.Conveyance = conveyanceAmount;
        }
        public List<PRSalaryScaleViewModel> GenerateEmployeeSalary(int empSalaryTypeId, int EmployeeStatusId, double grossSalary,
           string salaryGenerationType, int OfficeLocationId, string pfTypeId)
        {

            if (salaryGenerationType == SalaryGenerationTypeConstants.PayScale)
                empSalaryTypeId = 1;
            if (salaryGenerationType == SalaryGenerationTypeConstants.NonPayScale)
                empSalaryTypeId = 2;

            List<PRSalaryScaleViewModel> prSalaryList = new List<PRSalaryScaleViewModel>();
            double basicSalary = 0;
            try
            {
                var param2 = new
                {
                    EmployeeTypeId = Convert.ToInt32(empSalaryTypeId),
                    EmployeeStatusId = EmployeeStatusId,
                    OfficeLocationId = OfficeLocationId,
                    PFTypeId = Convert.ToInt32(pfTypeId)
                };

                //get payroll components from [prl].[PRComponent]
                var empTypeWiseCompConfig = employeeSPService.GetDataWithParameter(param2, "prl.SP_Get_EmployeeTypeWiseComponentConfiguration");

                for (int i = 0; i <= empTypeWiseCompConfig.Tables[0].Rows.Count - 1; i++)
                {
                    if (empTypeWiseCompConfig.Tables[0].Rows[i][3].ToString().Trim() == "Basic Salary")
                    {
                        basicSalary = CalculateRatioforComponent(Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentAmount"].ToString()), grossSalary);
                        break;
                    }
                    //break;
                }

                if (basicSalary > 0)
                {
                    prSalaryList = DistributeEmployeeSalaryInComponents(empSalaryTypeId, basicSalary, grossSalary,
                        EmployeeStatusId, OfficeLocationId, Convert.ToInt32(pfTypeId));
                }

            }
            catch (Exception ex)
            {
                var result = 0;
            }

            return prSalaryList;
        }

        private List<PRSalaryScaleViewModel> DistributeEmployeeSalaryInComponents(int empSalaryTypeId,
           double basicSalary, double gross, int EmployeeStatusId, int OfficeLocationId, int pfType)
        {
            var param2 = new
            {
                EmployeeTypeId = empSalaryTypeId,
                EmployeeStatusId = EmployeeStatusId,
                OfficeLocationId = OfficeLocationId,
                PFTypeId = pfType
            };

            var empTypeWiseCompConfig = employeeSPService.GetDataWithParameter(param2, "prl.SP_Get_EmployeeTypeWiseComponentConfiguration");
            empTypeWiseCompConfig.Tables[0].Columns.Add(new DataColumn("CalculatedAmount", typeof(System.Double)));

            for (int i = 0; i <= empTypeWiseCompConfig.Tables[0].Rows.Count - 1; i++)
            {
                var componentName = empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentName"].ToString();
                var componentType = empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentType"].ToString();
                var ratioPercent = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["ComponentAmount"].ToString());
                var ratioBasedOn = empTypeWiseCompConfig.Tables[0].Rows[i]["RatioBasedOn"].ToString();

                var salaryRoundType = empTypeWiseCompConfig.Tables[0].Rows[i]["SalaryRoundType"].ToString();

                if (componentType == SalaryCalculationTypeConstants.Ratio
                    && ratioBasedOn == RatioBasedOnConstants.Gross)
                {
                    var ratio = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), gross);
                    var maxLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MaximumLimit"].ToString());
                    var minLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MinimumLimit"].ToString());
                    if (ratio < minLimit && minLimit != 0)
                    {
                        ratio = minLimit;
                    }
                    if (ratio > maxLimit && maxLimit != 0)
                    {
                        ratio = maxLimit;
                    }

                    if (salaryRoundType == "RoundUp")
                    {
                        ratio = Math.Round(ratio);
                    }
                    if (salaryRoundType == "RoundDown")
                    {
                        ratio = Math.Ceiling(ratio);
                    }

                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratio;
                }
                else if (componentType == SalaryCalculationTypeConstants.Ratio
                        && ratioBasedOn == RatioBasedOnConstants.Basic)
                {
                    var ratio = CalculateRatioforComponent(Convert.ToDouble(ratioPercent), basicSalary);
                    var maxLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MaximumLimit"].ToString());
                    var minLimit = Convert.ToDouble(empTypeWiseCompConfig.Tables[0].Rows[i]["MinimumLimit"].ToString());
                    if (ratio < minLimit && minLimit != 0)
                    {
                        ratio = minLimit;
                    }
                    if (ratio > maxLimit && maxLimit != 0)
                    {
                        ratio = maxLimit;
                    }

                    if (salaryRoundType == "RoundUp")
                    {
                        ratio = Math.Round(ratio);
                    }
                    if (salaryRoundType == "RoundDown")
                    {
                        ratio = Math.Ceiling(ratio);
                    }
                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratio;
                }
                else if (componentType == SalaryCalculationTypeConstants.Fixed)
                {
                    empTypeWiseCompConfig.Tables[0].Rows[i]["CalculatedAmount"] = ratioPercent;//for fixed ratioPercentage is the fixed amount
                }
            }

            List<PRSalaryScaleViewModel> dataList = new List<PRSalaryScaleViewModel>();

            dataList = empTypeWiseCompConfig.Tables[0].AsEnumerable()
            .Select(row => new PRSalaryScaleViewModel
            {
                PRComponentId = row.Field<int>("PRComponentId"),
                EmployeeTypeName = row.Field<string>("EmployeeTypeName"),
                ComponentGroupName = row.Field<string>("ComponentGroupName"),
                ComponentName = row.Field<string>("ComponentName"),
                ComponentType = row.Field<string>("ComponentType"),
                ComponentAmount = row.Field<decimal>("ComponentAmount"),
                RatioBasedOn = row.Field<string>("RatioBasedOn"),
                EmployeeTypeId = row.Field<int>("EmployeeTypeId"),
                CalculatedAmount = row.Field<double>("CalculatedAmount"),
                ComponentCategory = row.Field<string>("ComponentCategory"),
                TransactionType = row.Field<string>("TransactionType"),
                EmployeeStatusId = row.Field<int?>("EmployeeStatusId"),
                TransactionTypeView = row.Field<string>("TransactionTypeView")
            }).ToList();

            return dataList;
        }

        private double CalculateRatioforComponent(double ratio, double amount)
        {
            return amount != 0 ? (ratio * amount) / 100 : 0;
        }

        private string GetNextPromotionDate(string Year, string MonthName)
        {
            var currentDateOfMonth = DateTime.Now.Day;
            var nextPromotionMaxDate = DateTime.Parse($"01-{MonthName}-{Year}").AddMonths(1).AddDays(-1);
            
            return nextPromotionMaxDate.ToString("dd-MMM-yyyy",CultureInfo.InvariantCulture);
        }
        private void MapDropDown(EmployeePromotionViewModel model)
        {
            var pleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };

            model.DesignationList = CommonDynamicDropDown.GetAllPayrollDesignationList();
            model.EmployeeSalaryType = commonStaticDropDown.SalaryStructuredTypeList();
            model.SalaryGenerationTypeList = commonStaticDropDown.SalaryGenerationTypeList();
            model.GradeList = CommonDynamicDropDown.GetEmployeeGradeList();
            model.SalaryScaleList = commonStaticDropDown.NumberSerialDropDown(0, 15);
            model.OverTimeList = commonStaticDropDown.YesNoDropDown_bool();
            model.PFTypeList = CommonDynamicDropDown.ProvidentFundType();
            model.MonthList = commonStaticDropDown.MonthList();
            model.BankList = CommonDynamicDropDown.PayrollBankNameWithCode();

            var yearList = new List<SelectListItem>();
            yearList.Add(pleaseSelect);

            for (int i = 0; i < 2; i++)
            {
                yearList.Add(new SelectListItem() { Text = (Convert.ToInt32(DateTime.Now.Year) + i).ToString(), Value = (Convert.ToInt32(DateTime.Now.Year) + i).ToString() });
            }
            model.IncrementYearFromList = yearList;

            var promotionType = promotionTypeService.GetMany(p => p.IsActive == true).ToList();
            var viewpromotionType = promotionType.Select(p => new SelectListItem()
            {
                Text = p.PromotionTypeName,
                Value = p.PromotionTypeId.ToString()
            });
            var promotionTypelist = new List<SelectListItem>();
            promotionTypelist.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            promotionTypelist.AddRange(viewpromotionType);
            model.PromotionTypeList = promotionTypelist;

            var employeeStatusList = CommonDynamicDropDown.ddlEmployeeStatusList();
            employeeStatusList.RemoveAll(x => x.Value == "");
            model.EmployeeStatusList = employeeStatusList;
        }
        public JsonResult Map_type_Dig_DropDown()
        {
            var promotionType = promotionTypeService.GetMany(p => p.IsActive == true).ToList();
            var viewpromotionType = promotionType.Select(p => new SelectListItem()
            {
                Text = p.PromotionTypeName,
                Value = p.PromotionTypeId.ToString()
            });
            var promotionTypelist = new List<SelectListItem>();
            promotionTypelist.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            promotionTypelist.AddRange(viewpromotionType);
            //
            var digLst = employeeDesignationService.GetMany(x => x.IsActive).ToList().
                Select(p => new SelectListItem()
                {
                    Text = p.DesignationName,
                    Value = p.DesignationId.ToString()
                });
            var diglist = new List<SelectListItem>();
            diglist.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            diglist.AddRange(digLst);
            return Json(new { promoTypeLst = promotionTypelist, digLst = diglist }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        private void MapReportDropDown(EmployeePromotionReportViewModel model)
        {
            var ReportList = new List<SelectListItem>();
            ReportList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            ReportList.Add(new SelectListItem() { Text = "Eligible for Promotion on Date", Value = "EFPOD" });

            ReportList.Add(new SelectListItem() { Text = "Employee Promotional Report By Employee Code ", Value = "EPRCode" });

            ReportList.Add(new SelectListItem() { Text = "Employee Promotional Report By Date to Date ", Value = "EPRDate" });

            ReportList.Add(new SelectListItem() { Text = "Employee Promotional/ Increment Report By Date to Date ", Value = "EPRINDate" });

            model.ReportTypeList = ReportList;
            var BasicOfficeTypeList = new List<SelectListItem>();
            BasicOfficeTypeList.Add(new SelectListItem() { Text = "Head Office", Value = "1" });
            BasicOfficeTypeList.Add(new SelectListItem() { Text = "Field Office", Value = "2" });
            model.BasicOfficeTypeList = BasicOfficeTypeList;
            model.DesignationList = CommonDynamicDropDown.GetAllPayrollDesignationList();
        }

        #region Report
        public ActionResult EmployeeEligibleForPromotionOnDateReport(int OfficeTypeId, DateTime Date, int DesignationId, string TotalServiceYear, string ServiceYearFromLastPromotion, bool DownloadExcel)
        {
            try
            {
                DataSet OverdueMls;
                int? TotalServiceYearData = null, ServiceYearFromLastPromotionData = null;
                if (!string.IsNullOrEmpty(TotalServiceYear)) TotalServiceYearData = Convert.ToInt32(TotalServiceYear);
                if (!string.IsNullOrEmpty(ServiceYearFromLastPromotion)) ServiceYearFromLastPromotionData = Convert.ToInt32(ServiceYearFromLastPromotion);
                var param = new {
                    Date = Date,
                    OfficeTypeId = OfficeTypeId,
                    DesignationId = DesignationId,
                    TotalServiceYear = TotalServiceYearData,
                    ServiceYearFromLastPromotion = ServiceYearFromLastPromotionData
                };
                OverdueMls = employeeSPService.GetDataWithParameter(param, "promo.SP_EmployeeEligibleForPromotionOnDate");
                var reportParam = new Dictionary<string, object>();
                string ReportPath = "Promotion/EmployeeEligibleForPromotionOnDate.rpt";
                if (DownloadExcel) ReportHelper.ExportExcelReport(ReportPath, OverdueMls.Tables[0], reportParam);
                else ReportHelper.PrintReport(ReportPath, OverdueMls.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult EmployeeForPromotionByEmployeeCodeReport(int OfficeTypeId, DateTime Date, int DesignationId, string TotalServiceYear, string ServiceYearFromLastPromotion, bool DownloadExcel, string EmployeeCode)
        {
            try
            {               
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = "1,2,3,4,5,6,7,8,9,10,11" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = "0"});
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = EmployeeCode });
              //  paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = EmployeeCode.ToString() });

                PrintSSRSReport("/gHRMPlus_Reports/PromotionalReportByCode", paramValues.ToArray());
                return Content(string.Empty);


            }
            catch (Exception ex)
            {
                return Content("<b>error</b><br />"+ ex.Message);
                // return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult EmployeeForPromotionByEmployeeDateToDateReport(int OfficeTypeId, string Date, string DateTo, int DesignationId, string TotalServiceYear, string ServiceYearFromLastPromotion, bool DownloadExcel, string EmployeeCode)
        {
            try
            {
                gHRMDBContext db = new gHRMDBContext();
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeTypeId", Value = OfficeTypeId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DesignationId", Value = DesignationId.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeStatusArr", Value = "1,2,3,4,5,6,7,8,9,10,11" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DepartmentId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "SectionId", Value = "0" });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = Date.ToString() });

                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "ToDate", Value = DateTo.ToString() });

                //  paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "EmployeeCode", Value = EmployeeCode.ToString() });

                PrintSSRSReport("/gHRMPlus_Reports/PromotionReportForDateToDate", paramValues.ToArray());
                return Content(string.Empty);


            }
            catch (Exception ex)
            {
                return Content("<b>error</b><br />" + ex.Message);
                // return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult EmployeeForPromotionIncrementByEmployeeDateToDateReport(int OfficeTypeId, string Date, string DateTo,  bool DownloadExcel )
        {
            try
            {

                var param = new
                {
                    from =  Date,
                    to = DateTo,
                    OfficeTypeId = OfficeTypeId, 
                };

                var mainReport = employeeSPService.GetDataWithParameter(param, "promo.sp_EmployeeServiceInfoPRIN");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("Promotion/rptAssessmentData.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);



            }
            catch (Exception ex)
            {
                return Content("<b>error</b><br />" + ex.Message);
                // return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult ReportEmployeeAssessment(string from, string to, int? year, string format, int?officetype)
        {
            try
            {
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyName", Value = SessionHelper.CompanyName });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "CompanyAddress", Value = SessionHelper.CompanyAddress });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "from", Value = from + "-" + year.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "to", Value = to + "-" + year.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "year", Value = (year ?? 0).ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "officetype", Value = (officetype ?? 0).ToString() });
                if(officetype==1)
                {
                    PrintSSRSMultiformat(format, "/gHRMPlus_Reports/InitialAssessmentReport_HO", paramValues.ToArray());
                }
                else
                {
                    PrintSSRSMultiformat(format, "/gHRMPlus_Reports/InitialAssessmentReport", paramValues.ToArray());
                }
       
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        #endregion
        public ActionResult EmployeeAssessmentScore()
        {
            var model = new EmployeePromotionViewModel();
            var yearList = commonStaticDropDown.YearList(2, 1);
            // Set the selected item
            string selectedValue = (System.DateTime.Now.Year - 1).ToString();
            foreach (var item in yearList)
            {
                if (item.Value == selectedValue)
                {
                    item.Selected = true;
                }
            }

            ViewData["Years"] = yearList;
            return View(model);
        }
        public ActionResult EmployeeAssessmentApproval()
        {
            var model = new EmployeePromotionViewModel();
            //model.MonthList = commonStaticDropDown.GetMonthListList();
            var BasicOfficeTypeList = new List<SelectListItem>();
            BasicOfficeTypeList.Add(new SelectListItem() { Text = "Head Office", Value = "1" });
            BasicOfficeTypeList.Add(new SelectListItem() { Text = "Field Office", Value = "2" });
            model.BasicOfficeTypeList = BasicOfficeTypeList;

            ViewData["Years"] = commonStaticDropDown.YearList(2, 1);
            return View(model);
        }





        // Employee Assesment Save 
        public JsonResult EmployeeAssessmentScoreSave(EmployeePromotion obj)
        {
            var result = 0;
            var message = "";

            try
            {
                var model = new EmployeePromotion();

                if (employeePromotionService.GetMany(x => x.IsActive && x.EmployeeId == obj.EmployeeId
                && x.AssessmentYear == obj.AssessmentYear).Any())
                {
                    message = "Data Already Exists";
                    return Json(new { result = 0, message = message }, JsonRequestBehavior.DenyGet);
                }

                else
                {
                    model.PromotionId = obj.PromotionId;
                    model.PromotionStatus = obj.PromotionStatus = "Pending";
                    model.EmployeeId = obj.EmployeeId;
                    model.Score = obj.Score;
                    model.IsActive = obj.IsActive = true;
                    model.AssessmentYear = obj.AssessmentYear;

                    employeePromotionService.Create(model);

                    message = "Data Save Successfully";
                    return Json(new { result = 1, message = message }, JsonRequestBehavior.DenyGet);

                }



                //  return Json(new { result = 1, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        // Load Promotion Score List 
        public JsonResult GetAllPromotionScore(int jtStartIndex, int jtPageSize, string jtSorting)
        {

            try
            {
                // var scoreList = employeePromotionService.GetMany(x => x.IsActive && x.AssessmentYear != null).ToList();

                //var view_scoreList = scoreList.AsEnumerable().Select(p => new EmployeePromotionViewModel()
                //{
                //    PromotionId = p.PromotionId,
                //    EmployeeId = p.EmployeeId,
                //    AssessmentYear = p.AssessmentYear,
                //    Score = p.Score                    

                //}).OrderBy(x => x.EmployeeId ).ToList();


                var scoreList = employeePromotionService.GetAllPromotionScoreCollection().ToList();

                var view_scoreList = scoreList.AsEnumerable().Select(p => new EmployeePromotionViewModel()
                {
                    PromotionId = p.PromotionId,
                    EmployeeId = p.EmployeeId,
                    EmployeeCode = p.EmpCode,
                    AssessmentYear = p.AssessmentYear,
                    Score = p.Score,
                    EmployeeName = p.EmpName,
                    IsActive = p.IsActive

                }).OrderBy(x => x.EmployeeCode).ToList();




                var currentPageRecords = view_scoreList.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = view_scoreList.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetAllPromotionScoreNotFound(int jtStartIndex, int jtPageSize, string jtSorting)
        {

            try
            {
                var employeeData = employeeSPService.GetDataWithoutParameter( "promo.SP_Get_Not_Found_Score");

                var view_scoreList = employeeData.Tables[0].AsEnumerable()
                .Select(row => new EmployeePromotionViewModel
                {
                    //PromotionId = row.Field<long>("PromotionId"),
                    EmployeeId = row.Field<long>("EmployeeId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    //AssessmentYear = row.Field<int>("AssessmentYear"),
                    //Score = row.Field<int>("Score"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    IsActive = row.Field<bool>("IsActive"),

                }).Distinct().OrderBy(x => x.EmployeeCode).ToList();




                var currentPageRecords = view_scoreList.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = view_scoreList.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        // Edit/ Update 
        public JsonResult EmployeeAssessmentScoreEdit(EmployeePromotion obj)
        {
            var result = 0;
            var message = "";

            try
            {

                //var checkData = employeePromotionService.GetMany(x => x.IsActive
                //                //   && x.PromotionId == obj.PromotionId
                //                && x.EmployeeId == obj.EmployeeId
                //                && x.AssessmentYear == obj.AssessmentYear).Any();

                var model = employeePromotionService.GetByEmpIdLong(obj.PromotionId);

                if (obj.AssessmentYear != model.AssessmentYear)
                {
                    message = "Data Already Exists";
                    return Json(new { result = 0, message = message }, JsonRequestBehavior.DenyGet);
                }
                else if ((model.AssessmentYear == obj.AssessmentYear) && obj.Score > 0)
                {
                    model.PromotionId = obj.PromotionId;
                    model.EmployeeId = obj.EmployeeId;
                    model.AssessmentYear = obj.AssessmentYear;
                    model.Score = obj.Score;
                    employeePromotionService.Update(model);
                    result = 1;
                    message = "Updated successfully";
                }
                else
                {
                    message = "Data Already Exists";
                    return Json(new { result = 0, message = message }, JsonRequestBehavior.DenyGet);
                }

            }
            catch (Exception)
            {

                result = 0;
                message = "Update denied";
            }


            return Json(new
            {
                result = result,
                message = message
            }, JsonRequestBehavior.AllowGet);

        }
    

    }
}
