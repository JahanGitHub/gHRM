using gHRM.Data.CodeFirstMigration;
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
    public class TransferExcelImport : IExcelImport
    {
        private List<string> ErrorMsgList;
        private IEmployeeTransferService _EmployeeTransferService;

        private const string TRANSFER_SHEET = "TransferBacklog";

        public TransferExcelImport()
        {
            ErrorMsgList = new List<string>();
        }

        public void ProcessData(ExcelWorksheets Worksheets, ExcelImportController _Controller)
        {
            string Message = "";
            _EmployeeTransferService = _Controller._EmployeeTransferService;
            string[] YesNoList = { "yes", "no" };

            foreach (var sheet in Worksheets)
            {
                var sheetName = sheet.Name;
                string CurrentSheet = sheetName;

                if (TRANSFER_SHEET != sheetName)
                {
                    ErrorMsgList.Add("No Sheet Found called " + TRANSFER_SHEET);
                    break;
                }
                var noOfCol = sheet.Dimension.End.Column;
                var noOfRow = sheet.Dimension.End.Row;
                var ETransferList = new List<EmployeeTransfer>();

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    long EmployeeId = 0;
                    int OfficeId = 0, DepartmentId = 0, SectionId = 0, ResponsibilityId = 0;
                    int CurrentRowNo = rowIterator;
                    try
                    {
                        string EmployeeCode = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 1, "Employee Code", out Message);
                        string OfficeName = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 2, "Office", out Message);
                        string DepartmentName = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 3, "Department", out Message);
                        string SectionName = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 4, "Section", out Message);
                        string ResponsibilityName = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 5, "Responsibility", out Message);
                        long? OrderNo = ExcelImportHelper.GetLongFromExcelCell(sheet, rowIterator, 6, "Order No", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        //DateTime? OrderDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 7, "Order Date", out Message);
                        DateTime? OrderDate = ExcelImportHelper.GetDateFromExcelCell2(sheet, rowIterator, 7, "Order Date", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        //string ReleaseDateText = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 8, "Release Date", out Message);
                        //DateTime? ReleaseDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 8, "Release Date", out Message);

                        string ReleaseDateText = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 8, "Release Date", out Message);
                        DateTime? ReleaseDate = ExcelImportHelper.GetDateFromExcelCell2(sheet, rowIterator, 8, "Release Date", out Message);

                        if ("" != Message && "" != ReleaseDateText.ToString().Trim())
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        DateTime? JoiningDate = ExcelImportHelper.GetDateFromExcelCell2(sheet, rowIterator, 9, "Joining Date", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        string Mutual = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 10, "Mutual", out Message);
                        if (!YesNoList.Contains(Mutual.Trim().ToLower()))
                        {
                            Message = "Mutual must be either Yes or No";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        string TADA_Applicable = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 11, "TADA Applicable", out Message);
                        if (!YesNoList.Contains(TADA_Applicable.Trim().ToLower()))
                        {
                            Message = "TADA Applicable must be either Yes or No";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        _EmployeeTransferService.GetDataFromExcelData(EmployeeCode, OfficeName, DepartmentName, SectionName, ResponsibilityName, out EmployeeId, out OfficeId, out DepartmentId, out SectionId, out ResponsibilityId);

                        if (null != EmployeeCode && "" != EmployeeCode.Trim() && 0 == EmployeeId)
                        {
                            Message = "Employee does not exist with code \"" + EmployeeCode + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != OfficeName && "" != OfficeName.Trim() && 0 == OfficeId)
                        {
                            Message = "Office does not exist with name \"" + OfficeName + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != DepartmentName && "" != DepartmentName.Trim() && 0 == DepartmentId)
                        {
                            Message = "Department does not exist with name \"" + DepartmentName + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != SectionName && "" != SectionName.Trim() && 0 == SectionId)
                        {
                            Message = "Section does not exist with name \"" + SectionName + "\" on Department \"" + DepartmentName + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != ResponsibilityName && "" != ResponsibilityName.Trim() && 0 == ResponsibilityId)
                        {
                            Message = "Employee Responsibility does not exist with name \"" + ResponsibilityName + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }

                        TransferBacklogHelper Helper = new TransferBacklogHelper();
                        Helper.LoggedInEmployeeId = _Controller.CreateUserId;
                        Helper._EmployeeTransferService = _EmployeeTransferService;
                        Helper.IsCurrentOfficeReleaseDate = true;
                        var ETransfer = new EmployeeTransfer();
                        ETransfer.EmployeeId = EmployeeId;
                        ETransfer.OfficeDesignationId = ResponsibilityId;
                        ETransfer.OfficeId = OfficeId;
                        ETransfer.DepartmentId = DepartmentId;
                        if (SectionId > 0) ETransfer.SectionId = SectionId;
                        ETransfer.OrderNo = OrderNo ?? 0;
                        ETransfer.OrderDate = OrderDate.Value;
                        ETransfer.IsTADAApplicable = "yes" == TADA_Applicable;
                        ETransfer.IsMutual = "yes" == Mutual;
                        ETransfer.JoiningDate = JoiningDate;
                        ETransfer.ReleaseDate = ReleaseDate;

                        if (!Helper.Save(ETransfer, out Message))
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
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
            _EmployeeTransferService = _Controller._EmployeeTransferService;
            string[] YesNoList = { "yes", "no" };

            foreach (var sheet in Worksheets)
            {
                var sheetName = sheet.Name;
                string CurrentSheet = sheetName;

                if (TRANSFER_SHEET != sheetName)
                {
                    ErrorMsgList.Add("No Sheet Found called " + TRANSFER_SHEET);
                    break;
                }
                var noOfCol = sheet.Dimension.End.Column;
                var noOfRow = sheet.Dimension.End.Row;
                var ETransferList = new List<EmployeeTransfer>();

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    long EmployeeId = 0;
                    int OfficeId = 0, DepartmentId = 0, SectionId = 0, ResponsibilityId = 0;
                    int CurrentRowNo = rowIterator;
                    try
                    {
                        string EmployeeCode = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 1, "Employee Code", out Message);
                        string OfficeName = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 2, "Office", out Message);
                        string DepartmentName = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 3, "Department", out Message);
                        string SectionName = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 4, "Section", out Message);
                        string ResponsibilityName = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 5, "Responsibility", out Message);
                        long? OrderNo = ExcelImportHelper.GetLongFromExcelCell(sheet, rowIterator, 6, "Order No", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        DateTime? OrderDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 7, "Order Date", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        string ReleaseDateText = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 8, "Release Date", out Message);
                        DateTime? ReleaseDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 8, "Release Date", out Message);
                        if ("" != Message && "" != ReleaseDateText.ToString().Trim())
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        DateTime? JoiningDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 9, "Joining Date", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        string Mutual = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 10, "Mutual", out Message);
                        if (!YesNoList.Contains(Mutual.Trim().ToLower()))
                        {
                            Message = "Mutual must be either Yes or No";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        string TADA_Applicable = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 11, "TADA Applicable", out Message);
                        if (!YesNoList.Contains(TADA_Applicable.Trim().ToLower()))
                        {
                            Message = "TADA Applicable must be either Yes or No";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        _EmployeeTransferService.GetDataFromExcelData(EmployeeCode, OfficeName, DepartmentName, SectionName, ResponsibilityName, out EmployeeId, out OfficeId, out DepartmentId, out SectionId, out ResponsibilityId);

                        if (null != EmployeeCode && "" != EmployeeCode.Trim() && 0 == EmployeeId)
                        {
                            Message = "Employee does not exist with code \"" + EmployeeCode + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != OfficeName && "" != OfficeName.Trim() && 0 == OfficeId)
                        {
                            Message = "Office does not exist with name \"" + OfficeName + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != DepartmentName && "" != DepartmentName.Trim() && 0 == DepartmentId)
                        {
                            Message = "Department does not exist with name \"" + DepartmentName + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != SectionName && "" != SectionName.Trim() && 0 == SectionId)
                        {
                            Message = "Section does not exist with name \"" + SectionName + "\" on Department \"" + DepartmentName + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != ResponsibilityName && "" != ResponsibilityName.Trim() && 0 == ResponsibilityId)
                        {
                            Message = "Employee Responsibility does not exist with name \"" + ResponsibilityName + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }

                        TransferBacklogHelper Helper = new TransferBacklogHelper();
                        Helper.LoggedInEmployeeId = _Controller.CreateUserId;
                        Helper._EmployeeTransferService = _EmployeeTransferService;
                        Helper.IsCurrentOfficeReleaseDate = true;
                        var ETransfer = new EmployeeTransfer();
                        ETransfer.EmployeeId = EmployeeId;
                        ETransfer.OfficeDesignationId = ResponsibilityId;
                        ETransfer.OfficeId = OfficeId;
                        ETransfer.DepartmentId = DepartmentId;
                        if (SectionId > 0) ETransfer.SectionId = SectionId;
                        ETransfer.OrderNo = OrderNo ?? 0;
                        ETransfer.OrderDate = OrderDate.Value;
                        ETransfer.IsTADAApplicable = "yes" == TADA_Applicable;
                        ETransfer.IsMutual = "yes" == Mutual;
                        ETransfer.JoiningDate = JoiningDate;
                        ETransfer.ReleaseDate = ReleaseDate;

                        if (!Helper.Save(ETransfer, out Message))
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
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
            _EmployeeTransferService = _Controller._EmployeeTransferService;
            string[] YesNoList = { "yes", "no" };

            foreach (var sheet in Worksheets)
            {
                var sheetName = sheet.Name;
                string CurrentSheet = sheetName;

                if (TRANSFER_SHEET != sheetName)
                {
                    ErrorMsgList.Add("No Sheet Found called " + TRANSFER_SHEET);
                    break;
                }
                var noOfCol = sheet.Dimension.End.Column;
                var noOfRow = sheet.Dimension.End.Row;
                var ETransferList = new List<EmployeeTransfer>();

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    long EmployeeId = 0;
                    int OfficeId = 0, DepartmentId = 0, SectionId = 0, ResponsibilityId = 0;
                    int CurrentRowNo = rowIterator;
                    try
                    {
                        string EmployeeCode = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 1, "Employee Code", out Message);
                        string OfficeName = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 2, "Office", out Message);
                        string DepartmentName = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 3, "Department", out Message);
                        string SectionName = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 4, "Section", out Message);
                        string ResponsibilityName = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 5, "Responsibility", out Message);
                        long? OrderNo = ExcelImportHelper.GetLongFromExcelCell(sheet, rowIterator, 6, "Order No", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        DateTime? OrderDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 7, "Order Date", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        string ReleaseDateText = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 8, "Release Date", out Message);
                        DateTime? ReleaseDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 8, "Release Date", out Message);
                        if ("" != Message && "" != ReleaseDateText.ToString().Trim())
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        DateTime? JoiningDate = ExcelImportHelper.GetDateFromExcelCell(sheet, rowIterator, 9, "Joining Date", out Message);
                        if ("" != Message)
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        string Mutual = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 10, "Mutual", out Message);
                        if (!YesNoList.Contains(Mutual.Trim().ToLower()))
                        {
                            Message = "Mutual must be either Yes or No";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        string TADA_Applicable = ExcelImportHelper.GetStringFromExcelCell(sheet, rowIterator, 11, "TADA Applicable", out Message);
                        if (!YesNoList.Contains(TADA_Applicable.Trim().ToLower()))
                        {
                            Message = "TADA Applicable must be either Yes or No";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        _EmployeeTransferService.GetDataFromExcelData(EmployeeCode, OfficeName, DepartmentName, SectionName, ResponsibilityName, out EmployeeId, out OfficeId, out DepartmentId, out SectionId, out ResponsibilityId);

                        if (null != EmployeeCode && "" != EmployeeCode.Trim() && 0 == EmployeeId)
                        {
                            Message = "Employee does not exist with code \"" + EmployeeCode + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != OfficeName && "" != OfficeName.Trim() && 0 == OfficeId)
                        {
                            Message = "Office does not exist with name \"" + OfficeName + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != DepartmentName && "" != DepartmentName.Trim() && 0 == DepartmentId)
                        {
                            Message = "Department does not exist with name \"" + DepartmentName + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != SectionName && "" != SectionName.Trim() && 0 == SectionId)
                        {
                            Message = "Section does not exist with name \"" + SectionName + "\" on Department \"" + DepartmentName + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
                        if (null != ResponsibilityName && "" != ResponsibilityName.Trim() && 0 == ResponsibilityId)
                        {
                            Message = "Employee Responsibility does not exist with name \"" + ResponsibilityName + "\"";
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }

                        TransferBacklogHelper Helper = new TransferBacklogHelper();
                        Helper.LoggedInEmployeeId = _Controller.CreateUserId;
                        Helper._EmployeeTransferService = _EmployeeTransferService;
                        Helper.IsCurrentOfficeReleaseDate = true;
                        var ETransfer = new EmployeeTransfer();
                        ETransfer.EmployeeId = EmployeeId;
                        ETransfer.OfficeDesignationId = ResponsibilityId;
                        ETransfer.OfficeId = OfficeId;
                        ETransfer.DepartmentId = DepartmentId;
                        if (SectionId > 0) ETransfer.SectionId = SectionId;
                        ETransfer.OrderNo = OrderNo ?? 0;
                        ETransfer.OrderDate = OrderDate.Value;
                        ETransfer.IsTADAApplicable = "yes" == TADA_Applicable;
                        ETransfer.IsMutual = "yes" == Mutual;
                        ETransfer.JoiningDate = JoiningDate;
                        ETransfer.ReleaseDate = ReleaseDate;

                        if (!Helper.Save(ETransfer, out Message))
                        {
                            ErrorMsgList.Add(ExcelImportHelper.GetErrorWithRowNo(CurrentSheet, CurrentRowNo, Message));
                            continue;
                        }
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