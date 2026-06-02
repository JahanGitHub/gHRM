
#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity.Validation;
using gHRM.Web.ViewModels.Leave;
using System.Globalization;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using OfficeOpenXml;
using gHRM.Core.Utilities.Constants; /**/
using gHRM.Web.Helpers;
using gHRM.Core.Utilities;
#endregion

namespace gHRM.Web.Controllers
{
    public class ExcelUploadController : BaseController
    {
        #region Private Members
        private readonly ICompanyService companyService;
        private readonly ILeaveTypeService leaveTypeService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly ILeaveHistoryService leaveHistoryService;
        private readonly ILeaveELOpeningService leaveELOpeningService;
        private readonly IOfficeService officeService;
        private string CurrentSheet;
        #endregion

        #region Ctor
        public ExcelUploadController(
              ICompanyService companyService
            , ILeaveTypeService leaveTypeService
            , IEmployeeService employeeService
            , IEmployeeSPService employeeSPService
            , ILeaveHistoryService leaveHistoryService
            , ILeaveELOpeningService leaveELOpeningService
            , IOfficeService officeService
            )
        {
            this.companyService = companyService;
            this.leaveTypeService = leaveTypeService;
            this.employeeService = employeeService;
            this.employeeSPService = employeeSPService;
            this.leaveHistoryService = leaveHistoryService;
            this.leaveELOpeningService = leaveELOpeningService;
            this.officeService = officeService;
        }
        #endregion

        #region Index

        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Upload Leave Data


        public ActionResult AddExcelFile()
        {
            return Redirect("/ExcelImport/LeaveOB");
        }

        [HttpPost]
        public ActionResult AddExcelFile(FormCollection formCollection)
        {
            string Message = "";
            List<string> ErrorMsgList = new List<string>();
            try
            {
                if (Request == null)
                {
                    ErrorMsgList.Add("Invalid Data");
                    SetAllErrorMsg(ErrorMsgList);
                    return View();
                }

                HttpPostedFileBase file = Request.Files["BatchFile"];
                ExcelFileUploadViewModel returnInformation = new ExcelFileUploadViewModel();
                var looprecord = 0;

                if (file == null || file.ContentLength == 0 || string.IsNullOrEmpty(file.FileName))
                {
                    ErrorMsgList.Add("Invalid Data");
                    SetAllErrorMsg(ErrorMsgList);
                    return View();
                }

                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheets = package.Workbook.Worksheets;
                    foreach (var sheet in currentSheets)
                    {
                        var sheetName = sheet.Name;
                        CurrentSheet = sheetName;

                        if (sheetName != LeaveImportSheetConstants.EarnLeaveOpening &&
                            sheetName != LeaveImportSheetConstants.CasualLeaveOpening)
                        {
                            ErrorMsgList.Add("No Sheet Found called " + LeaveImportSheetConstants.EarnLeaveOpening + " OR " + LeaveImportSheetConstants.CasualLeaveOpening);
                            break;
                        }

                        if (sheetName == LeaveImportSheetConstants.EarnLeaveOpening)
                        {
                            var noOfCol = sheet.Dimension.End.Column;
                            var noOfRow = sheet.Dimension.End.Row;
                            var elOpeningList = new List<LeaveELOpening>();

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                int CurrentRowNo = rowIterator;
                                looprecord = rowIterator;
                                var codeValue = sheet.Cells[rowIterator, 1].Value.ToString();

                                var employeeCode = GenerateEmployeeCode(codeValue);
                                var employee = employeeService.GetByCode(employeeCode.Trim());

                                if (employee == null)
                                {
                                    ErrorMsgList.Add(GetErrorWithRowNo(CurrentRowNo, "No Employee found with Code: " + codeValue));
                                    continue;
                                }

                                var elLeaveType = leaveTypeService.GetMany(l =>
                                                        l.LeaveCategory.Trim() == LeaveCategoryConstants.Annual_EL &&
                                                        l.EmployeeStatusId == employee.EmployeeStatusId &&
                                                        l.IsActive == true);
                                if (!elLeaveType.Any())
                                {
                                    ErrorMsgList.Add(GetErrorWithRowNo(CurrentRowNo, "No Earn Leave exists for Employee with Code: " + codeValue));
                                    continue;
                                }

                                //let's update previous el as inactive [leave.LeaveELOpening]
                                MakeEarnLeaveIsActiveFalse(employee.EmployeeId);

                                var elOpeningModel = new LeaveELOpening();
                                elOpeningModel.EmployeeId = employee.EmployeeId;

                                DateTime? LeaveStartDate = GetDateFromExcelCell(sheet, rowIterator, 2, "LeaveStartDate", out Message);
                                if (null == LeaveStartDate)
                                {
                                    ErrorMsgList.Add(GetErrorWithRowNo(CurrentRowNo, Message));
                                    continue;
                                }
                                elOpeningModel.LeaveStartDate = LeaveStartDate.Value;

                                DateTime? LeaveEndDate = GetDateFromExcelCell(sheet, rowIterator, 3, "LeaveEndDate", out Message);
                                if (null == LeaveEndDate)
                                {
                                    ErrorMsgList.Add(GetErrorWithRowNo(CurrentRowNo, Message));
                                    continue;
                                }
                                elOpeningModel.LeaveEndDate = LeaveEndDate.Value;

                                int? ELFull = GetIntFromExcelCell(sheet, rowIterator, 4, "ELFull", out Message);
                                if (null == ELFull)
                                {
                                    ErrorMsgList.Add(GetErrorWithRowNo(CurrentRowNo, Message));
                                    continue;
                                }
                                elOpeningModel.ELFull = ELFull.Value;

                                int? EnjoyFull = GetIntFromExcelCell(sheet, rowIterator, 5, "EnjoyFull", out Message);
                                if (null == EnjoyFull)
                                {
                                    ErrorMsgList.Add(GetErrorWithRowNo(CurrentRowNo, Message));
                                    continue;
                                }
                                elOpeningModel.EnjoyFull = EnjoyFull.Value;

                                int? BalanceFull = GetIntFromExcelCell(sheet, rowIterator, 6, "BalanceFull", out Message);
                                if (null == BalanceFull)
                                {
                                    ErrorMsgList.Add(GetErrorWithRowNo(CurrentRowNo, Message));
                                    continue;
                                }
                                elOpeningModel.BalanceFull = BalanceFull.Value;

                                int? ELHalf = GetIntFromExcelCell(sheet, rowIterator, 8, "ELHalf", out Message);
                                if (null == ELHalf)
                                {
                                    ErrorMsgList.Add(GetErrorWithRowNo(CurrentRowNo, Message));
                                    continue;
                                }
                                elOpeningModel.ELHalf = ELHalf.Value;

                                int? EnjoyHalf = GetIntFromExcelCell(sheet, rowIterator, 9, "EnjoyHalf", out Message);
                                if (null == EnjoyHalf)
                                {
                                    ErrorMsgList.Add(GetErrorWithRowNo(CurrentRowNo, Message));
                                    continue;
                                }
                                elOpeningModel.EnjoyHalf = EnjoyHalf.Value;

                                int? BalanceHalf = GetIntFromExcelCell(sheet, rowIterator, 10, "BalanceHalf", out Message);
                                if (null == BalanceHalf)
                                {
                                    ErrorMsgList.Add(GetErrorWithRowNo(CurrentRowNo, Message));
                                    continue;
                                }
                                elOpeningModel.BalanceHalf = BalanceHalf.Value;

                                DateTime? LastSaleDate = GetDateFromExcelCell(sheet, rowIterator, 11, "LastSaleDate", out Message);
                                if (null == LastSaleDate)
                                {
                                    ErrorMsgList.Add(GetErrorWithRowNo(CurrentRowNo, Message));
                                    continue;
                                }
                                elOpeningModel.LastSaleDate = LastSaleDate.Value;

                                elOpeningModel.IsActive = true;
                                elOpeningModel.CreateDate = DateTime.UtcNow;

                                elOpeningList.Add(elOpeningModel);
                            }

                            //let's add into leave EL opening  [leave.LeaveELOpening]
                            leaveELOpeningService.AddELOpeningList(elOpeningList);
                        }

                        if (sheetName == LeaveImportSheetConstants.CasualLeaveOpening)
                        {
                            var noOfCol = sheet.Dimension.End.Column;
                            var noOfRow = sheet.Dimension.End.Row;
                            var clOpeningList = new List<LeaveHistory>();

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                int CurrentRowNo = rowIterator;
                                looprecord = rowIterator;
                                var codeValue = sheet.Cells[rowIterator, 1].Value.ToString();

                                if (string.IsNullOrWhiteSpace(codeValue))
                                {
                                    ErrorMsgList.Add(GetErrorWithRowNo(CurrentRowNo, "Employee Code is required"));
                                    continue;
                                }

                                var employeeCode = "";
                                if (SessionHelper.CompanyCode == GHRMPlusCompanyConstants.GrameenKalyan)
                                {
                                    employeeCode = CommonHelper.GetFormattedEmployeeCodeWithFiveDigit(codeValue);
                                }
                                else
                                {
                                    employeeCode = GenerateEmployeeCode(codeValue);
                                }
                                var employee = employeeService.GetByCode(employeeCode.Trim());

                                if (employee == null)
                                {
                                    ErrorMsgList.Add(GetErrorWithRowNo(CurrentRowNo, "No Employee found with Code: " + codeValue));
                                    continue;
                                }

                                var firstJoiningDate = Convert.ToDateTime(employee.FirstJoiningDate);

                                var clLeaveType = leaveTypeService.GetMany(l =>
                                                        l.LeaveCategory.Trim() == LeaveCategoryConstants.Casual &&
                                                        l.EmployeeStatusId == employee.EmployeeStatusId &&
                                                        l.IsActive == true)
                                                    .FirstOrDefault();

                                if (clLeaveType == null)
                                {
                                    ErrorMsgList.Add(GetErrorWithRowNo(CurrentRowNo, "No Casual Leave exists for Employee with Code: " + codeValue));
                                    continue;
                                }

                                var value = Convert.ToDouble(sheet.Cells[rowIterator, 3].Value.ToString());

                                int leaveEnjoyed = 0; 
                                leaveEnjoyed = Convert.ToInt32(Math.Floor(value));

                                //if (leaveEnjoyed <= 0) //TODO:nee to open when live
                                //    continue;

                                DateTime requestDate = new DateTime(DateTime.Now.Year, 1, 1);
                                DateTime endDate = DateTime.Now.AddDays(-1);

                                DateTime leaveStartDate = requestDate;

                                //calcuate leave used balance
                                #region Need when customize opening leave file
                                leaveStartDate = firstJoiningDate;

                                double leaveRemainingBalance = Convert.ToDouble(sheet.Cells[rowIterator, 4].Value.ToString());

                                DateTime? LeaveEndDate = GetDateFromExcelCell(sheet, rowIterator, 2, "LeaveEndDate", out Message);
                                if (null == LeaveEndDate)
                                {
                                    ErrorMsgList.Add(GetErrorWithRowNo(CurrentRowNo, Message));
                                    continue;
                                }
                                endDate = LeaveEndDate.Value;

                                if (firstJoiningDate.Day > 14)
                                    firstJoiningDate=firstJoiningDate.AddMonths(1);

                                var diffMonths = ((endDate.Month + endDate.Year * 12) - (firstJoiningDate.Month + firstJoiningDate.Year * 12)) + 1;
                                
                                double totalLeaveBalance = (0.75 * diffMonths);
                                leaveEnjoyed = (int)Math.Round(totalLeaveBalance - leaveRemainingBalance);
                                #endregion

                                //let's update previous opening casual leave as inactive in [leave.LeaveHistory]
                                MakeCLIsActiveFalse(employee.EmployeeId);

                                var batchData = new LeaveHistory();                                
                                
                                batchData.EmployeeId = employee.EmployeeId;
                                batchData.LeaveTypeId = clLeaveType.LeaveTypeId;
                                batchData.LeaveRequestDate = requestDate;
                                batchData.LeaveStartDate = leaveStartDate;
                                batchData.LeaveEndDate = endDate;
                                batchData.ReplacementEmployee = 0;
                                //batchData.TotalDays = leaveEnjoyed;
                                batchData.TotalDays = (int?)leaveRemainingBalance; // newly added sabet
                                batchData.JoinDate = endDate.AddDays(1);
                                batchData.IsApproved = true;
                                batchData.ApprovedBy = 0;
                                batchData.IsAdjustment = true;
                                batchData.ApprovedDate = endDate.AddDays(1);
                                batchData.AdjustmentDate = endDate.AddDays(1);
                                batchData.IsActive = true;
                                batchData.LeaveReason = "OPENING";
                                batchData.CreateUser = Convert.ToInt64(LoggedInEmployeeId);
                                batchData.CreateDate = DateTime.UtcNow;
                                clOpeningList.Add(batchData);
                            }

                            //let's add into leave history [leave.LeaveHistory]
                            leaveHistoryService.AddCLOpeningList(clOpeningList);
                        }
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);

                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            SetAllErrorMsg(ErrorMsgList);
            return View();
        }
        #endregion

        #region FileUpload Test

        [HttpPost]
        public ActionResult AddExcelFile_FileUpload_Test(FormCollection formCollection)
        {
            try
            {
                if (Request == null)
                    return RedirectToAction("AddExcelFile");

                HttpPostedFileBase file = Request.Files["BatchFile"];
                ExcelFileUploadViewModel returnInformation = new ExcelFileUploadViewModel();
                var looprecord = 0;

                if (file == null || file.ContentLength == 0 || string.IsNullOrEmpty(file.FileName))
                    return RedirectToAction("AddExcelFile");

                var officeList = new List<Office>();

                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheets = package.Workbook.Worksheets;
                    foreach (var sheet in currentSheets)
                    {
                        var sheetName = sheet.Name;
                        if (sheetName != "Branch_Office")
                            continue;

                        var noOfRow = sheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            looprecord = rowIterator;

                            var officeCodeHO = sheet.Cells[rowIterator, 1].Value.ToString();
                            var officeCodeZone = sheet.Cells[rowIterator, 3].Value.ToString();
                            var officeCodeArea = sheet.Cells[rowIterator, 5].Value.ToString();

                            var officeCodeBranch = sheet.Cells[rowIterator, 7].Value.ToString();
                            var officeNameBranch = sheet.Cells[rowIterator, 8].Value.ToString();

                            if (officeList.Any(f => f.OfficeCode == officeCodeBranch))
                                continue;

                            officeList.Add(new Office
                            {
                                CompanyId = 1,
                                OfficeTypeId = 6,
                                OfficeCode = officeCodeBranch,
                                OfficeName = officeNameBranch,
                                OfficeNameBn = officeNameBranch,
                                OfficeLevel = 4,

                                FirstLevel = officeCodeHO,
                                SecondLevel = officeCodeZone,
                                ThirdLevel = officeCodeArea,
                                FourthLevel = officeCodeBranch,

                                OperationStartDate = DateTime.Now,
                                OfficeAddress = officeNameBranch,
                                PostCode = "N/A",
                                Email = "jcjsr@ymail.com",
                                Phone = "042168823",
                                ImagePath = null,
                                PRWorkAreaID = null,
                                IsActive = true,
                                InActiveDate = null,
                                CreateUser = 1,
                                CreateDate = DateTime.Now,
                                UpdateUser = null,
                                UpdateDate = null,
                                OfficeLocationId = 2
                            });
                        }
                    }

                    officeService.AddOfficeList(officeList);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);

                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            return RedirectToAction("AddExcelFile");
        }


        #endregion

        #region Pidim TotalDays Mapping

        [HttpPost]
        public ActionResult AddExcelFile_Not_Used(FormCollection formCollection)
        {
            try
            {
                if (Request == null)
                    return RedirectToAction("AddExcelFile");

                HttpPostedFileBase file = Request.Files["BatchFile"];
                ExcelFileUploadViewModel returnInformation = new ExcelFileUploadViewModel();
                var looprecord = 0;

                if (file == null || file.ContentLength == 0 || string.IsNullOrEmpty(file.FileName))
                    return RedirectToAction("AddExcelFile");
                
                using (var package = new ExcelPackage(file.InputStream))
                {
                    var currentSheets = package.Workbook.Worksheets;
                    foreach (var sheet in currentSheets)
                    {
                        var sheetName = sheet.Name;
                        if (sheetName != "CL_Opening")
                            continue;

                        var noOfRow = sheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            looprecord = rowIterator;
                            
                            var codeValue = sheet.Cells[rowIterator, 1].Value.ToString();
                            if (codeValue == "101")
                                continue;

                            var employeeCode = GenerateEmployeeCode(codeValue);
                            var totalLeaveBalance = Convert.ToInt32(Convert.ToDouble(sheet.Cells[rowIterator, 5].Value.ToString()));

                            var sqlCommand = $@"
                                    UPDATE lh
                                    SET TotalDays = {totalLeaveBalance}-ISNULL(TotalDays,0)
                                    FROM leave.LeaveHistory lh
                                    WHERE
		                                    LeaveReason='OPENING'
	                                    and IsActive=1
	                                    and EmployeeId = (select EmployeeId from Employee WHERE EmployeeCode='{employeeCode}')

                                        select 'success';
                                    ";


                            using (var db = new gHRMDBContext())
                            {
                                db.Database.SqlQuery<string>(sqlCommand).FirstOrDefault();
                            }
                        }
                    }                    
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);

                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            return RedirectToAction("AddExcelFile");
        }


        #endregion

        #region Private Methods

        private int GetELRemainingBalance(ExcelWorksheet sheet, int rowIterator, LeaveELOpening elOpeningModel)
        {
            int balanceFull = Convert.ToInt32(Math.Floor(Convert.ToDouble(sheet.Cells[rowIterator, 6].Value.ToString())));

            double totalServiceDays = (elOpeningModel.LeaveEndDate - elOpeningModel.LeaveStartDate).Days + 1;
            double totalLeaveBalance = (18 * totalServiceDays) / 365;
            int remainingBalance = (int)Math.Round(totalLeaveBalance - balanceFull);
            return remainingBalance;
        }

        private string GenerateEmployeeCode(string codeValue)
        {
            string employeeCode = string.Empty;
            if (SessionHelper.CompanyCode == GHRMPlusCompanyConstants.JagoraniChakraFoundation)
            {
                employeeCode = CommonHelper.GetFormattedEmployeeCodeWithSixDigit(codeValue);
                return employeeCode;
            }
            if (SessionHelper.CompanyCode == GHRMPlusCompanyConstants.GT)
            {
                employeeCode = codeValue;
                return employeeCode;
            }
            if (codeValue.Trim().Length == 1)
            {
                employeeCode = "000" + codeValue.Trim();
            }
            else if (codeValue.Trim().Length == 2)
            {
                employeeCode = "00" + codeValue.Trim();
            }
            else if (codeValue.Trim().Length == 3)
            {
                employeeCode = "0" + codeValue.Trim();
            }
            else
            {
                employeeCode = codeValue.Trim();
            }
            return employeeCode;
        }

        private void MakeELIsActiveFalse(long employeeId)
        {
            var elOpening = leaveELOpeningService.GetByEmployeeId(employeeId);

            if (elOpening != null)
            {
                var param = new { EmployeeId = employeeId };
                var employeeData2 = employeeSPService.GetDataWithParameter(param, "SP_LeaveOpeningELPreviousDataDelete");
            }
        }

        private void MakeEarnLeaveIsActiveFalse(long employeeId)
        {
            var elOpening = leaveELOpeningService.GetByEmployeeId(employeeId);

            using (var db = new gHRMDBContext())
            {
                var sqlCommand = $@"SP_LeaveOpeningELPreviousDataDelete  {employeeId}";
                db.Database.SqlQuery<int>(sqlCommand).FirstOrDefault();
            }
        }

        private void MakeCLIsActiveFalse(long employeeId)
        {
            //var leaveHistory = leaveHistoryService.GetMany(p => p.EmployeeId == employeeId && p.IsActive == true && p.LeaveReason.Trim() == "OPENING").FirstOrDefault();
            //if (leaveHistory != null)
            //{
            //var param = new { EmployeeId = employeeId };
            //var employeeData2 = employeeSPService.GetDataWithParameter(param, "SP_LeaveOpeningPreviousDataDelete");
            //}

            using (var db = new gHRMDBContext())
            {
                var sqlCommand = $@"SP_LeaveOpeningPreviousDataDelete  {employeeId}";
                db.Database.SqlQuery<int>(sqlCommand).FirstOrDefault();
            }
        }

        private string GetErrorWithRowNo(int CurrentRowNo, string Message)
        {
            return "Sheet: <b>" + CurrentSheet + "</b>, Row No: <b>" + CurrentRowNo + "</b>, " + Message;
        }

        private void SetAllErrorMsg(List<string> ErrorMsgList)
        {
            ViewBag.AllErrorMsg = ErrorMsgList.Count() > 0 ? "<ul><li>" + string.Join("</li><li>", ErrorMsgList) + "</li></ul>" : "Success";
        }

        private DateTime? GetDateFromExcelCell(ExcelWorksheet Sheet, int RowIndex, int ColIndex, string Label, out string Message)
        {
            DateTime? DateData = null;
            Message = "";
            object DateObj = null;
            try
            {
                DateObj = Sheet.Cells[RowIndex, ColIndex].Value;
                string DateStr = DateObj.ToString().Trim();
                DateData = DateTime.ParseExact(DateStr, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            catch
            {
                try
                {
                    if (DateObj is DateTime) DateData = Convert.ToDateTime(DateObj);
                    else DateData = DateTime.FromOADate(double.Parse(DateObj.ToString()));
                }
                catch { }
            }
            if (null == DateData)
            {
                Message = Label + " must be a valid data with date format d/M/yyyy for excel cell formated as Text";
            }
            return DateData;
        }

        private int? GetIntFromExcelCell(ExcelWorksheet Sheet, int RowIndex, int ColIndex, string Label, out string Message)
        {
            int? NumData = null;
            Message = "";
            object DateObj = null;
            try
            {
                DateObj = Sheet.Cells[RowIndex, ColIndex].Value;
                if (null == DateObj || string.IsNullOrWhiteSpace(DateObj.ToString())) NumData = 0;
                else NumData = Convert.ToInt32(DateObj.ToString());
            }
            catch { }
            if (null == NumData)
            {
                Message = Label + " must be a valid integer number";
            }
            return NumData;
        }
        #endregion

    }
}