using gHRM.Service;
using gHRM.Web.Infrastucture.ExcelImport;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers
{
    public class SalaryExcelImportController : BaseController
    {
        public IEmployeeService _EmployeeService;
        public ILeaveTypeService _LeaveTypeService;
        public ILeaveHistoryService _LeaveHistoryService;
        public ILeaveELOpeningService _LeaveELOpeningService;
        public IEmployeeTransferService _EmployeeTransferService;
        public IEmployeePromotionService _EmployeePromotionService;
        public IPromotionConfiguredSalaryService _PromotionConfiguredSalaryService;
        public long CreateUserId;

        private IExcelImport ExcelImport;
        private string Name;
        private const string LEAVE_OB = "LeaveOB";
        private const string TRANSFER_BACKLOG = "TransferBacklog";
        private const string PROMOTION_BACKLOG = "PromotionBacklog";

        public SalaryExcelImportController(
            IEmployeeService _EmployeeService,
            IEmployeeTransferService _EmployeeTransferService,
            ILeaveTypeService _LeaveTypeService,
            ILeaveHistoryService _LeaveHistoryService,
            ILeaveELOpeningService _LeaveELOpeningService,
            IEmployeePromotionService _EmployeePromotionService,
            IPromotionConfiguredSalaryService _PromotionConfiguredSalaryService)
        {
            CreateUserId = LoggedInEmployeeId ?? 0;
            this._EmployeeService = _EmployeeService;
            this._EmployeeTransferService = _EmployeeTransferService;
            this._LeaveTypeService = _LeaveTypeService;
            this._LeaveHistoryService = _LeaveHistoryService;
            this._LeaveELOpeningService = _LeaveELOpeningService;
            this._EmployeePromotionService = _EmployeePromotionService;
            this._PromotionConfiguredSalaryService = _PromotionConfiguredSalaryService;
        }

        public ActionResult LeaveOB() { Name = LEAVE_OB; return ImportPage(); }
        public ActionResult TransferBacklog() { Name = TRANSFER_BACKLOG; return ImportPage(); }
        public ActionResult PromotionBacklog() { Name = PROMOTION_BACKLOG; return ImportPage(); }

        [HttpPost]
        public ActionResult Import(string Name, FormCollection formCollection)
        {
            this.Name = Name;
            if (LEAVE_OB == Name) ExcelImport = new LeaveOBExcelImport();
            else if(TRANSFER_BACKLOG == Name) ExcelImport = new TransferExcelImport();
            else if (PROMOTION_BACKLOG == Name) ExcelImport = new PromotionBLExcelImport();
            else
            {
                ViewBag.ErrMessage = "Excel import service is not Available for " + Name;
                return View("~/Views/Shared/ShowError.cshtml");
            }
            List<string> ErrorMsgList = new List<string>();
            try
            {
                if (Request == null)
                {
                    ErrorMsgList.Add("Invalid Data");
                    ViewBag.AllErrorMsg = ExcelImport.GetAllErrorMsg();
                    return ImportPage();
                }
                HttpPostedFileBase file = Request.Files["BatchFile"];

                if (file == null || file.ContentLength == 0 || string.IsNullOrEmpty(file.FileName))
                {
                    ErrorMsgList.Add("Invalid Data");
                    ViewBag.AllErrorMsg = ExcelImport.GetAllErrorMsg();
                    return ImportPage();
                }
                using (var package = new ExcelPackage(file.InputStream))
                {
                    ExcelImport.SalaryProcessData(package.Workbook.Worksheets, this);
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
            ViewBag.AllErrorMsg = ExcelImport.GetAllErrorMsg();
            return ImportPage();
        }

        private ActionResult ImportPage()
        {
            ViewData["Name"] = Name;
            SetPageTitle();
            SetSaveBtnCaption();
            SetSampleFilePath();
            return View("Index");
        }

        private void SetPageTitle()
        {
            string PageTitle = "";
            switch (Name)
            {
                case LEAVE_OB:
                    PageTitle = "Import Earn Leave(EL) And Casual Leave";
                    break;
                case TRANSFER_BACKLOG:
                    PageTitle = "Import Exsting Transfer Backlogs";
                    break;
                case PROMOTION_BACKLOG:
                    PageTitle = "Import Exsting Promotion Backlogs";
                    break;
                default:
                    break;
            }
            ViewData["PageTitle"] = PageTitle;
        }

        private void SetSaveBtnCaption()
        {
            string SaveBtnCaption = "";
            switch (Name)
            {
                case LEAVE_OB:
                    SaveBtnCaption = "Click Here to Import EL/Cl";
                    break;
                case TRANSFER_BACKLOG:
                    SaveBtnCaption = "Click Here to Import your Transfer Backlogs";
                    break;
                case PROMOTION_BACKLOG:
                    SaveBtnCaption = "Click Here to Import your Promotion Backlogs";
                    break;
                default:
                    break;
            }
            ViewData["SaveBtnCaption"] = SaveBtnCaption;
        }

        private void SetSampleFilePath()
        {
            string SampleFilePath = "";
            switch (Name)
            {
                case LEAVE_OB:
                    SampleFilePath = "leave/Leave_Opening_Data_Format.xlsx";
                    break;
                case TRANSFER_BACKLOG:
                    SampleFilePath = "transfer/transfer-backlog-import-sample-file.xlsx";
                    break;
                case PROMOTION_BACKLOG:
                    SampleFilePath = "promotion/promotion-backlog-import-sample-file.xlsx";
                    break;
                default:
                    break;
            }
            ViewData["SampleFilePath"] = "/Assets/docs/" + SampleFilePath;
        }
    }
}