using gHRM.Core.Utilities;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Web.Controllers;
using gHRM.Web.Helpers;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.Infrastucture.ExcelImport
{
    public class LeaveOBExcelImport : IExcelImport
    {
        private List<string> ErrorMsgList;
        private IEmployeeService employeeService;
        private ILeaveTypeService leaveTypeService;
        private ILeaveHistoryService leaveHistoryService;
        private ILeaveELOpeningService leaveELOpeningService;

        public LeaveOBExcelImport()
        {
            ErrorMsgList = new List<string>();
        }

        public void ProcessData(ExcelWorksheets Worksheets, ExcelImportController _Controller)
        {
            string Message = "";
            employeeService = _Controller._EmployeeService;
            leaveTypeService = _Controller._LeaveTypeService;
            leaveHistoryService = _Controller._LeaveHistoryService;
            leaveELOpeningService = _Controller._LeaveELOpeningService;

            foreach (var sheet in Worksheets)
            {
                var sheetName = sheet.Name;
                string CurrentSheet = sheetName;

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
                        var codeValue = sheet.Cells[rowIterator, 1].Value.ToString();

                        var employeeCode = GenerateEmployeeCode(codeValue);
                        var employee = employeeService.GetByCode(employeeCode.Trim());

                        if (employee == null)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, "No Employee found with Code: " + codeValue));
                            continue;
                        }

                        var elLeaveType = leaveTypeService.GetMany(l =>
                                                l.LeaveCategory.Trim() == LeaveCategoryConstants.Annual_EL &&
                                                l.EmployeeStatusId == employee.EmployeeStatusId &&
                                                l.IsActive == true);
                        if (!elLeaveType.Any())
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, "No Earn Leave exists for Employee with Code: " + codeValue));
                            continue;
                        }

                        //let's update previous el as inactive [leave.LeaveELOpening]
                        MakeEarnLeaveIsActiveFalse(employee.EmployeeId);

                        var elOpeningModel = new LeaveELOpening();
                        elOpeningModel.EmployeeId = employee.EmployeeId;

                        DateTime? LeaveStartDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 2, "LeaveStartDate", out Message);
                        if (null == LeaveStartDate)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.LeaveStartDate = LeaveStartDate.Value;

                        DateTime? LeaveEndDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 3, "LeaveEndDate", out Message);
                        if (null == LeaveEndDate)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.LeaveEndDate = LeaveEndDate.Value;

                        int? ELFull = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 4, "ELFull", out Message);
                        if (null == ELFull)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.ELFull = ELFull.Value;

                        int? EnjoyFull = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 5, "EnjoyFull", out Message);
                        if (null == EnjoyFull)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.EnjoyFull = EnjoyFull.Value;

                        int? BalanceFull = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 6, "BalanceFull", out Message);
                        if (null == BalanceFull)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.BalanceFull = BalanceFull.Value;

                        int? ELHalf = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 8, "ELHalf", out Message);
                        if (null == ELHalf)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.ELHalf = ELHalf.Value;

                        int? EnjoyHalf = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 9, "EnjoyHalf", out Message);
                        if (null == EnjoyHalf)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.EnjoyHalf = EnjoyHalf.Value;

                        int? BalanceHalf = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 10, "BalanceHalf", out Message);
                        if (null == BalanceHalf)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.BalanceHalf = BalanceHalf.Value;

                        DateTime? LastSaleDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 11, "LastSaleDate", out Message);
                        if (null == LastSaleDate)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
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
                        var codeValue = sheet.Cells[rowIterator, 1].Value.ToString();

                        if (string.IsNullOrWhiteSpace(codeValue))
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, "Employee Code is required"));
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
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, "No Employee found with Code: " + codeValue));
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
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, "No Casual Leave exists for Employee with Code: " + codeValue));
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

                        DateTime? LeaveEndDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 2, "LeaveEndDate", out Message);
                        if (null == LeaveEndDate)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        endDate = LeaveEndDate.Value;

                        if (firstJoiningDate.Day > 14)
                            firstJoiningDate = firstJoiningDate.AddMonths(1);

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
                        batchData.TotalDays = (decimal?)leaveRemainingBalance; // newly added sabet
                        batchData.JoinDate = endDate.AddDays(1);
                        batchData.IsApproved = true;
                        batchData.ApprovedBy = 0;
                        batchData.IsAdjustment = true;
                        batchData.ApprovedDate = endDate.AddDays(1);
                        batchData.AdjustmentDate = endDate.AddDays(1);
                        batchData.IsActive = true;
                        batchData.LeaveReason = "OPENING";
                        batchData.CreateUser = _Controller.CreateUserId;
                        batchData.CreateDate = DateTime.UtcNow;
                        batchData.LeaveDayDuration = "Full";
                        clOpeningList.Add(batchData);
                    }

                    //let's add into leave history [leave.LeaveHistory]
                    leaveHistoryService.AddCLOpeningList(clOpeningList);
                }
            }
        }

        public void SalaryProcessData(ExcelWorksheets Worksheets, SalaryExcelImportController _Controller)
        {
            string Message = "";
            employeeService = _Controller._EmployeeService;
            leaveTypeService = _Controller._LeaveTypeService;
            leaveHistoryService = _Controller._LeaveHistoryService;
            leaveELOpeningService = _Controller._LeaveELOpeningService;

            foreach (var sheet in Worksheets)
            {
                var sheetName = sheet.Name;
                string CurrentSheet = sheetName;

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
                        var codeValue = sheet.Cells[rowIterator, 1].Value.ToString();

                        var employeeCode = GenerateEmployeeCode(codeValue);
                        var employee = employeeService.GetByCode(employeeCode.Trim());

                        if (employee == null)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, "No Employee found with Code: " + codeValue));
                            continue;
                        }

                        var elLeaveType = leaveTypeService.GetMany(l =>
                                                l.LeaveCategory.Trim() == LeaveCategoryConstants.Annual_EL &&
                                                l.EmployeeStatusId == employee.EmployeeStatusId &&
                                                l.IsActive == true);
                        if (!elLeaveType.Any())
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, "No Earn Leave exists for Employee with Code: " + codeValue));
                            continue;
                        }

                        //let's update previous el as inactive [leave.LeaveELOpening]
                        MakeEarnLeaveIsActiveFalse(employee.EmployeeId);

                        var elOpeningModel = new LeaveELOpening();
                        elOpeningModel.EmployeeId = employee.EmployeeId;

                        DateTime? LeaveStartDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 2, "LeaveStartDate", out Message);
                        if (null == LeaveStartDate)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.LeaveStartDate = LeaveStartDate.Value;

                        DateTime? LeaveEndDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 3, "LeaveEndDate", out Message);
                        if (null == LeaveEndDate)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.LeaveEndDate = LeaveEndDate.Value;

                        int? ELFull = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 4, "ELFull", out Message);
                        if (null == ELFull)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.ELFull = ELFull.Value;

                        int? EnjoyFull = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 5, "EnjoyFull", out Message);
                        if (null == EnjoyFull)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.EnjoyFull = EnjoyFull.Value;

                        int? BalanceFull = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 6, "BalanceFull", out Message);
                        if (null == BalanceFull)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.BalanceFull = BalanceFull.Value;

                        int? ELHalf = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 8, "ELHalf", out Message);
                        if (null == ELHalf)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.ELHalf = ELHalf.Value;

                        int? EnjoyHalf = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 9, "EnjoyHalf", out Message);
                        if (null == EnjoyHalf)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.EnjoyHalf = EnjoyHalf.Value;

                        int? BalanceHalf = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 10, "BalanceHalf", out Message);
                        if (null == BalanceHalf)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.BalanceHalf = BalanceHalf.Value;

                        DateTime? LastSaleDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 11, "LastSaleDate", out Message);
                        if (null == LastSaleDate)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
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
                        var codeValue = sheet.Cells[rowIterator, 1].Value.ToString();

                        if (string.IsNullOrWhiteSpace(codeValue))
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, "Employee Code is required"));
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
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, "No Employee found with Code: " + codeValue));
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
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, "No Casual Leave exists for Employee with Code: " + codeValue));
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

                        DateTime? LeaveEndDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 2, "LeaveEndDate", out Message);
                        if (null == LeaveEndDate)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        endDate = LeaveEndDate.Value;

                        if (firstJoiningDate.Day > 14)
                            firstJoiningDate = firstJoiningDate.AddMonths(1);

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
                        batchData.TotalDays = (decimal?)leaveRemainingBalance; // newly added sabet
                        batchData.JoinDate = endDate.AddDays(1);
                        batchData.IsApproved = true;
                        batchData.ApprovedBy = 0;
                        batchData.IsAdjustment = true;
                        batchData.ApprovedDate = endDate.AddDays(1);
                        batchData.AdjustmentDate = endDate.AddDays(1);
                        batchData.IsActive = true;
                        batchData.LeaveReason = "OPENING";
                        batchData.CreateUser = _Controller.CreateUserId;
                        batchData.CreateDate = DateTime.UtcNow;
                        batchData.LeaveDayDuration = "Full";
                        clOpeningList.Add(batchData);
                    }

                    //let's add into leave history [leave.LeaveHistory]
                    leaveHistoryService.AddCLOpeningList(clOpeningList);
                }
            }
        }

        public void ChallanProcessData(ExcelWorksheets Worksheets, SalaryExcelImportController _Controller)
        {
            string Message = "";
            employeeService = _Controller._EmployeeService;
            leaveTypeService = _Controller._LeaveTypeService;
            leaveHistoryService = _Controller._LeaveHistoryService;
            leaveELOpeningService = _Controller._LeaveELOpeningService;

            foreach (var sheet in Worksheets)
            {
                var sheetName = sheet.Name;
                string CurrentSheet = sheetName;

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
                        var codeValue = sheet.Cells[rowIterator, 1].Value.ToString();

                        var employeeCode = GenerateEmployeeCode(codeValue);
                        var employee = employeeService.GetByCode(employeeCode.Trim());

                        if (employee == null)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, "No Employee found with Code: " + codeValue));
                            continue;
                        }

                        var elLeaveType = leaveTypeService.GetMany(l =>
                                                l.LeaveCategory.Trim() == LeaveCategoryConstants.Annual_EL &&
                                                l.EmployeeStatusId == employee.EmployeeStatusId &&
                                                l.IsActive == true);
                        if (!elLeaveType.Any())
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, "No Earn Leave exists for Employee with Code: " + codeValue));
                            continue;
                        }

                        //let's update previous el as inactive [leave.LeaveELOpening]
                        MakeEarnLeaveIsActiveFalse(employee.EmployeeId);

                        var elOpeningModel = new LeaveELOpening();
                        elOpeningModel.EmployeeId = employee.EmployeeId;

                        DateTime? LeaveStartDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 2, "LeaveStartDate", out Message);
                        if (null == LeaveStartDate)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.LeaveStartDate = LeaveStartDate.Value;

                        DateTime? LeaveEndDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 3, "LeaveEndDate", out Message);
                        if (null == LeaveEndDate)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.LeaveEndDate = LeaveEndDate.Value;

                        int? ELFull = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 4, "ELFull", out Message);
                        if (null == ELFull)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.ELFull = ELFull.Value;

                        int? EnjoyFull = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 5, "EnjoyFull", out Message);
                        if (null == EnjoyFull)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.EnjoyFull = EnjoyFull.Value;

                        int? BalanceFull = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 6, "BalanceFull", out Message);
                        if (null == BalanceFull)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.BalanceFull = BalanceFull.Value;

                        int? ELHalf = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 8, "ELHalf", out Message);
                        if (null == ELHalf)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.ELHalf = ELHalf.Value;

                        int? EnjoyHalf = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 9, "EnjoyHalf", out Message);
                        if (null == EnjoyHalf)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.EnjoyHalf = EnjoyHalf.Value;

                        int? BalanceHalf = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 10, "BalanceHalf", out Message);
                        if (null == BalanceHalf)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        elOpeningModel.BalanceHalf = BalanceHalf.Value;

                        DateTime? LastSaleDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 11, "LastSaleDate", out Message);
                        if (null == LastSaleDate)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
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
                        var codeValue = sheet.Cells[rowIterator, 1].Value.ToString();

                        if (string.IsNullOrWhiteSpace(codeValue))
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, "Employee Code is required"));
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
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, "No Employee found with Code: " + codeValue));
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
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, "No Casual Leave exists for Employee with Code: " + codeValue));
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

                        DateTime? LeaveEndDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 2, "LeaveEndDate", out Message);
                        if (null == LeaveEndDate)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        endDate = LeaveEndDate.Value;

                        if (firstJoiningDate.Day > 14)
                            firstJoiningDate = firstJoiningDate.AddMonths(1);

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
                        batchData.TotalDays = (decimal?)leaveRemainingBalance; // newly added sabet
                        batchData.JoinDate = endDate.AddDays(1);
                        batchData.IsApproved = true;
                        batchData.ApprovedBy = 0;
                        batchData.IsAdjustment = true;
                        batchData.ApprovedDate = endDate.AddDays(1);
                        batchData.AdjustmentDate = endDate.AddDays(1);
                        batchData.IsActive = true;
                        batchData.LeaveReason = "OPENING";
                        batchData.CreateUser = _Controller.CreateUserId;
                        batchData.CreateDate = DateTime.UtcNow;
                        batchData.LeaveDayDuration = "Full";
                        clOpeningList.Add(batchData);
                    }

                    //let's add into leave history [leave.LeaveHistory]
                    leaveHistoryService.AddCLOpeningList(clOpeningList);
                }
            }
        }
        public string GetAllErrorMsg()
        {
            return ExcelImportHelper.GetAllErrorMsg(ErrorMsgList);
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
            using (var db = new gHRMDBContext())
            {
                var sqlCommand = $@"SP_LeaveOpeningPreviousDataDelete  {employeeId}";
                db.Database.SqlQuery<int>(sqlCommand).FirstOrDefault();
            }
        }
    }
}