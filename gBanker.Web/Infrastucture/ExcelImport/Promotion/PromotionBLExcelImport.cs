using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.EmployeePromotion;
using gHRM.Service;
using gHRM.Web.Controllers;
using gHRM.Web.Helpers.Transfer;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.Infrastucture.ExcelImport
{
    public class PromotionBLExcelImport : IExcelImport
    {
        private List<string> ErrorMsgList;
        private IEmployeePromotionService _EmployeePromotionService;
        private IPromotionConfiguredSalaryService _PromotionConfiguredSalaryService;

        private const string PROMOTION_SHEET = "PromotionBacklog";

        public PromotionBLExcelImport()
        {
            ErrorMsgList = new List<string>();
        }

        public void ProcessData(ExcelWorksheets Worksheets, ExcelImportController _Controller)
        {
            string Message = "";
            _EmployeePromotionService = _Controller._EmployeePromotionService;
            _PromotionConfiguredSalaryService = _Controller._PromotionConfiguredSalaryService;
            string[] YesNoList = { "yes", "no" };

            foreach (var sheet in Worksheets)
            {
                var sheetName = sheet.Name;
                string CurrentSheet = sheetName;

                if (PROMOTION_SHEET != sheetName)
                {
                    ErrorMsgList.Add("No Sheet Found called " + PROMOTION_SHEET);
                    break;
                }
                var noOfCol = sheet.Dimension.End.Column;
                var noOfRow = sheet.Dimension.End.Row;
                var ETransferList = new List<EmployeeTransfer>();

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    long EmployeeId = 0;
                    int PayrollDesignationId = 0, PromotionTypeId = 0;
                    int CurrentRowNo = rowIterator;
                    try
                    {
                        string EmployeeCode = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 1, "Employee Code", out Message);
                        string PayrollDesignation = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 2, "Payroll Designation", out Message);
                        string PromotionType = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 3, "Promotion Type", out Message);
                        string IsReviewed = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 4, "Is Reviewed", out Message);
                        if (!YesNoList.Contains(IsReviewed.Trim().ToLower()))
                        {
                            Message = "Is Reviewed must be either Yes or No";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        DateTime? PromotionDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 5, "Promotion Date", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        int? DurationMonth = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 6, "Duration Month", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        double? GrossSalary = ExcelImportHelper.GetDoubleFromExcelCell(sheet, rowIterator, 7, "Gross Salary", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        _EmployeePromotionService.GetDataFromExcelData(EmployeeCode, PayrollDesignation, PromotionType, out EmployeeId, out PayrollDesignationId, out PromotionTypeId);

                        if (null != EmployeeCode && "" != EmployeeCode.Trim() && 0 == EmployeeId)
                        {
                            Message = "Employee does not exist with code \"" + EmployeeCode + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != PayrollDesignation && "" != PayrollDesignation.Trim() && 0 == PayrollDesignationId)
                        {
                            Message = "Payroll Designation does not exist with name \"" + PayrollDesignation + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != PromotionType && "" != PromotionType.Trim() && 0 == PromotionTypeId)
                        {
                            Message = "Promotion Type does not exist with name \"" + PromotionType + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (_EmployeePromotionService.IsDuplicate(EmployeeId, PromotionTypeId, PromotionDate.Value))
                        {
                            Message = "Duplicate promotion exist";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }

                        EmployeePromotion EPromotion = new EmployeePromotion();
                        EPromotion.EmployeeId = EmployeeId;
                        EPromotion.DesignationId = PayrollDesignationId;
                        EPromotion.PromotionTypeId = PromotionTypeId;
                        EPromotion.PromotionDate = PromotionDate;
                        EPromotion.NextReviewDate = PromotionDate.Value.AddMonths(DurationMonth.Value);
                        EPromotion.IsReviewed = "yes" == IsReviewed;
                        EPromotion.IsActive = true;
                        EPromotion.CreateUser = _Controller.CreateUserId;
                        EPromotion.CreateDate = DateTime.Now;
                        _EmployeePromotionService.Create(EPromotion);

                        PromotionConfiguredSalary PCSalary = new PromotionConfiguredSalary();
                        PCSalary.PromotionId = EPromotion.PromotionId;
                        PCSalary.EmployeeId = EmployeeId;
                        PCSalary.GrossSalary = Convert.ToDecimal(GrossSalary ?? 0);
                        PCSalary.BasicSalary = (PCSalary.GrossSalary.Value * 55) / 100;
                        PCSalary.HouseRent = (PCSalary.GrossSalary.Value * 30) / 100;
                        PCSalary.Medical = (PCSalary.GrossSalary.Value * 10) / 100;
                        PCSalary.Conveyance = (PCSalary.GrossSalary.Value * 5) / 100;
                        PCSalary.Others = 0;
                        PCSalary.IsActive = true;
                        PCSalary.CreateUser = _Controller.CreateUserId;
                        PCSalary.CreateDate = DateTime.Now;
                        _PromotionConfiguredSalaryService.Create(PCSalary);
                    }
                    catch (Exception ex)
                    {
                        Message = ex.Message;
                        ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                        continue;
                    }
                }
            }
        }

        public void SalaryProcessData(ExcelWorksheets Worksheets, SalaryExcelImportController _Controller)
        {
            string Message = "";
            _EmployeePromotionService = _Controller._EmployeePromotionService;
            _PromotionConfiguredSalaryService = _Controller._PromotionConfiguredSalaryService;
            string[] YesNoList = { "yes", "no" };

            foreach (var sheet in Worksheets)
            {
                var sheetName = sheet.Name;
                string CurrentSheet = sheetName;

                if (PROMOTION_SHEET != sheetName)
                {
                    ErrorMsgList.Add("No Sheet Found called " + PROMOTION_SHEET);
                    break;
                }
                var noOfCol = sheet.Dimension.End.Column;
                var noOfRow = sheet.Dimension.End.Row;
                var ETransferList = new List<EmployeeTransfer>();

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    long EmployeeId = 0;
                    int PayrollDesignationId = 0, PromotionTypeId = 0;
                    int CurrentRowNo = rowIterator;
                    try
                    {
                        string EmployeeCode = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 1, "Employee Code", out Message);
                        string PayrollDesignation = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 2, "Payroll Designation", out Message);
                        string PromotionType = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 3, "Promotion Type", out Message);
                        string IsReviewed = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 4, "Is Reviewed", out Message);
                        if (!YesNoList.Contains(IsReviewed.Trim().ToLower()))
                        {
                            Message = "Is Reviewed must be either Yes or No";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        DateTime? PromotionDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 5, "Promotion Date", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        int? DurationMonth = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 6, "Duration Month", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        double? GrossSalary = ExcelImportHelper.GetDoubleFromExcelCell(sheet, rowIterator, 7, "Gross Salary", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        _EmployeePromotionService.GetDataFromExcelData(EmployeeCode, PayrollDesignation, PromotionType, out EmployeeId, out PayrollDesignationId, out PromotionTypeId);

                        if (null != EmployeeCode && "" != EmployeeCode.Trim() && 0 == EmployeeId)
                        {
                            Message = "Employee does not exist with code \"" + EmployeeCode + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != PayrollDesignation && "" != PayrollDesignation.Trim() && 0 == PayrollDesignationId)
                        {
                            Message = "Payroll Designation does not exist with name \"" + PayrollDesignation + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != PromotionType && "" != PromotionType.Trim() && 0 == PromotionTypeId)
                        {
                            Message = "Promotion Type does not exist with name \"" + PromotionType + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (_EmployeePromotionService.IsDuplicate(EmployeeId, PromotionTypeId, PromotionDate.Value))
                        {
                            Message = "Duplicate promotion exist";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }

                        EmployeePromotion EPromotion = new EmployeePromotion();
                        EPromotion.EmployeeId = EmployeeId;
                        EPromotion.DesignationId = PayrollDesignationId;
                        EPromotion.PromotionTypeId = PromotionTypeId;
                        EPromotion.PromotionDate = PromotionDate;
                        EPromotion.NextReviewDate = PromotionDate.Value.AddMonths(DurationMonth.Value);
                        EPromotion.IsReviewed = "yes" == IsReviewed;
                        EPromotion.IsActive = true;
                        EPromotion.CreateUser = _Controller.CreateUserId;
                        EPromotion.CreateDate = DateTime.Now;
                        _EmployeePromotionService.Create(EPromotion);

                        PromotionConfiguredSalary PCSalary = new PromotionConfiguredSalary();
                        PCSalary.PromotionId = EPromotion.PromotionId;
                        PCSalary.EmployeeId = EmployeeId;
                        PCSalary.GrossSalary = Convert.ToDecimal(GrossSalary ?? 0);
                        PCSalary.BasicSalary = (PCSalary.GrossSalary.Value * 55) / 100;
                        PCSalary.HouseRent = (PCSalary.GrossSalary.Value * 30) / 100;
                        PCSalary.Medical = (PCSalary.GrossSalary.Value * 10) / 100;
                        PCSalary.Conveyance = (PCSalary.GrossSalary.Value * 5) / 100;
                        PCSalary.Others = 0;
                        PCSalary.IsActive = true;
                        PCSalary.CreateUser = _Controller.CreateUserId;
                        PCSalary.CreateDate = DateTime.Now;
                        _PromotionConfiguredSalaryService.Create(PCSalary);
                    }
                    catch (Exception ex)
                    {
                        Message = ex.Message;
                        ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                        continue;
                    }
                }
            }
        }

        public void ChallanProcessData(ExcelWorksheets Worksheets, SalaryExcelImportController _Controller)
        {
            string Message = "";
            _EmployeePromotionService = _Controller._EmployeePromotionService;
            _PromotionConfiguredSalaryService = _Controller._PromotionConfiguredSalaryService;
            string[] YesNoList = { "yes", "no" };

            foreach (var sheet in Worksheets)
            {
                var sheetName = sheet.Name;
                string CurrentSheet = sheetName;

                if (PROMOTION_SHEET != sheetName)
                {
                    ErrorMsgList.Add("No Sheet Found called " + PROMOTION_SHEET);
                    break;
                }
                var noOfCol = sheet.Dimension.End.Column;
                var noOfRow = sheet.Dimension.End.Row;
                var ETransferList = new List<EmployeeTransfer>();

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    long EmployeeId = 0;
                    int PayrollDesignationId = 0, PromotionTypeId = 0;
                    int CurrentRowNo = rowIterator;
                    try
                    {
                        string EmployeeCode = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 1, "Employee Code", out Message);
                        string PayrollDesignation = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 2, "Payroll Designation", out Message);
                        string PromotionType = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 3, "Promotion Type", out Message);
                        string IsReviewed = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 4, "Is Reviewed", out Message);
                        if (!YesNoList.Contains(IsReviewed.Trim().ToLower()))
                        {
                            Message = "Is Reviewed must be either Yes or No";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        DateTime? PromotionDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 5, "Promotion Date", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        int? DurationMonth = ExcelImportHelper.GetIntFromExcelCell(sheet, rowIterator, 6, "Duration Month", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        double? GrossSalary = ExcelImportHelper.GetDoubleFromExcelCell(sheet, rowIterator, 7, "Gross Salary", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        _EmployeePromotionService.GetDataFromExcelData(EmployeeCode, PayrollDesignation, PromotionType, out EmployeeId, out PayrollDesignationId, out PromotionTypeId);

                        if (null != EmployeeCode && "" != EmployeeCode.Trim() && 0 == EmployeeId)
                        {
                            Message = "Employee does not exist with code \"" + EmployeeCode + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != PayrollDesignation && "" != PayrollDesignation.Trim() && 0 == PayrollDesignationId)
                        {
                            Message = "Payroll Designation does not exist with name \"" + PayrollDesignation + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != PromotionType && "" != PromotionType.Trim() && 0 == PromotionTypeId)
                        {
                            Message = "Promotion Type does not exist with name \"" + PromotionType + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (_EmployeePromotionService.IsDuplicate(EmployeeId, PromotionTypeId, PromotionDate.Value))
                        {
                            Message = "Duplicate promotion exist";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }

                        EmployeePromotion EPromotion = new EmployeePromotion();
                        EPromotion.EmployeeId = EmployeeId;
                        EPromotion.DesignationId = PayrollDesignationId;
                        EPromotion.PromotionTypeId = PromotionTypeId;
                        EPromotion.PromotionDate = PromotionDate;
                        EPromotion.NextReviewDate = PromotionDate.Value.AddMonths(DurationMonth.Value);
                        EPromotion.IsReviewed = "yes" == IsReviewed;
                        EPromotion.IsActive = true;
                        EPromotion.CreateUser = _Controller.CreateUserId;
                        EPromotion.CreateDate = DateTime.Now;
                        _EmployeePromotionService.Create(EPromotion);

                        PromotionConfiguredSalary PCSalary = new PromotionConfiguredSalary();
                        PCSalary.PromotionId = EPromotion.PromotionId;
                        PCSalary.EmployeeId = EmployeeId;
                        PCSalary.GrossSalary = Convert.ToDecimal(GrossSalary ?? 0);
                        PCSalary.BasicSalary = (PCSalary.GrossSalary.Value * 55) / 100;
                        PCSalary.HouseRent = (PCSalary.GrossSalary.Value * 30) / 100;
                        PCSalary.Medical = (PCSalary.GrossSalary.Value * 10) / 100;
                        PCSalary.Conveyance = (PCSalary.GrossSalary.Value * 5) / 100;
                        PCSalary.Others = 0;
                        PCSalary.IsActive = true;
                        PCSalary.CreateUser = _Controller.CreateUserId;
                        PCSalary.CreateDate = DateTime.Now;
                        _PromotionConfiguredSalaryService.Create(PCSalary);
                    }
                    catch (Exception ex)
                    {
                        Message = ex.Message;
                        ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                        continue;
                    }
                }
            }
        }


        public string GetAllErrorMsg()
        {
            return ExcelImportHelper.GetAllErrorMsg(ErrorMsgList);
        }
    }
}