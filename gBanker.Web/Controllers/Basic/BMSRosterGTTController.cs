using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Service.TimeKeeping;
using gHRM.Web.Helpers;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using OfficeOpenXml;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers
{
    public class BMSRosterGTTController : BaseController
    {
        #region Variables
        private readonly ITimeKeepingRosterService timeKeepingRosterService;
        private readonly IView_TimeKeepingRosterService view_TimeKeepingRosterService;
        private readonly IEmployeeRosterScheduleService employeeRosterScheduleService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IRoasterEmployeeScheduleService roasterEmployeeScheduleService;

        public BMSRosterGTTController(
            ITimeKeepingRosterService timeKeepingRosterService,
            IView_TimeKeepingRosterService view_TimeKeepingRosterService,
            IEmployeeRosterScheduleService employeeRosterScheduleService,
            IEmployeeSPService employeeSPService,
            IRoasterEmployeeScheduleService roasterEmployeeScheduleService
        )
        {
            this.timeKeepingRosterService = timeKeepingRosterService;
            this.view_TimeKeepingRosterService = view_TimeKeepingRosterService;
            this.employeeRosterScheduleService = employeeRosterScheduleService;
            this.employeeSPService = employeeSPService;
            this.roasterEmployeeScheduleService = roasterEmployeeScheduleService;
        }

        #endregion

        #region Actions

        public ActionResult Index()
        {
            return View();
        }

        #endregion


        #region HttpRequests

        public JsonResult SaveTimeKeepingRoster(TimeKeepingRoster timeKeepingRoster)
        {
            var result = string.Empty;
            try
            {
                var entity = new TimeKeepingRoster();
                entity.TimeKeepingRosterId = timeKeepingRoster.TimeKeepingRosterId;
                entity.RosterName = timeKeepingRoster.RosterName;
                entity.LoginTime = timeKeepingRoster.LoginTime;
                entity.LastLoginTime = timeKeepingRoster.LastLoginTime;
                entity.LogoutTime = timeKeepingRoster.LogoutTime;
                entity.EffectiveStartDate = timeKeepingRoster.EffectiveStartDate;
                entity.EffectiveEndDate = timeKeepingRoster.EffectiveEndDate;
                entity.IsActive = true;
                entity.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                //entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                entity.CreateDate = DateTime.UtcNow;
                //entity.UpdateDate = DateTime.UtcNow;
                timeKeepingRosterService.Create(entity);
                result = "Save Successfull";
            }
            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public JsonResult UpdateTimeKeepingRoster(TimeKeepingRoster timeKeepingRoster)
        {
            var result = string.Empty;          

            try
            {
                var employeeRoasterSchedule = roasterEmployeeScheduleService.GetByTimeKeepingRoasterId(timeKeepingRoster.TimeKeepingRosterId);

                if (employeeRoasterSchedule != null && employeeRoasterSchedule.Id > 0)
                    return Json(new { result = "This roaster is currently in used. Please try another!" }, JsonRequestBehavior.AllowGet);

                var isDuplicate =
                       timeKeepingRosterService.GetAll()
                           .Where(
                               p =>
                                   p.IsActive == true && p.TimeKeepingRosterId != timeKeepingRoster.TimeKeepingRosterId &&
                                   p.RosterName.ToUpper().Trim() == timeKeepingRoster.RosterName.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Employee Roster Name, Update denied";
                    return Json(new { result = result }, JsonRequestBehavior.AllowGet);
                }

                var entity = timeKeepingRosterService.GetById(timeKeepingRoster.TimeKeepingRosterId);

                entity.TimeKeepingRosterId = timeKeepingRoster.TimeKeepingRosterId;
                entity.RosterName = timeKeepingRoster.RosterName;
                entity.LoginTime = timeKeepingRoster.LoginTime;
                entity.LastLoginTime = timeKeepingRoster.LastLoginTime;
                entity.LogoutTime = timeKeepingRoster.LogoutTime;
                entity.EffectiveStartDate = timeKeepingRoster.EffectiveStartDate;
                entity.EffectiveEndDate = timeKeepingRoster.EffectiveEndDate;
                entity.IsActive = true;                
                entity.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                entity.UpdateDate = DateTime.UtcNow;

                //let's add into [TimeKeepingRoster]
                timeKeepingRosterService.Update(entity);
                result = "Update Successfull";
            }
            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ListTimeKeepingRoster([DataSourceRequest]DataSourceRequest request)
        {
            var VMcar = view_TimeKeepingRosterService.GetAll().Where(t => t.IsActive == true).ToList();

            DataSourceResult result = VMcar.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRoasterDetailsById(int id)
        {
            var loginTime = "";
            var lastLoginTime = "";
            var logoutTime = "";

            //get from [timeKeepingRoster] by id
            var timeKeepingRoster = timeKeepingRosterService.GetById(id);

            if (timeKeepingRoster != null)
            {
                loginTime = timeKeepingRoster.LoginTime.ToString("HH:mm",CultureInfo.InvariantCulture);
                lastLoginTime = timeKeepingRoster.LastLoginTime.ToString("HH:mm", CultureInfo.InvariantCulture);
                logoutTime = timeKeepingRoster.LogoutTime.ToString("HH:mm", CultureInfo.InvariantCulture);
            } 

            var newtimeKeepingRoster =new {
                LoginTime= loginTime,
                LastLoginTime= lastLoginTime,
                LogoutTime= logoutTime
            };

            return Json(newtimeKeepingRoster, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InformationDeleteTimeKeepingRoster(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var employeeRoasterSchedule = roasterEmployeeScheduleService.GetByTimeKeepingRoasterId(Id);

                if (employeeRoasterSchedule != null && employeeRoasterSchedule.Id > 0)
                    return Json(new { result=0, message = "This roaster is currently in used. Please try another!" }, JsonRequestBehavior.AllowGet);
                
                var model = timeKeepingRosterService.GetById(Id);
                model.IsActive = false;
                model.CreateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateBy = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                timeKeepingRosterService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion


    

            [HttpPost]
            public ActionResult UploadRoster(HttpPostedFileBase file)
            {
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        // Save the uploaded file
                        string fileName = Path.GetFileName(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileName);
                        file.SaveAs(path);

                        // Process the Excel file
                        ProcessExcelFile(path);

                        ViewBag.Message = "File uploaded successfully!";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR: " + ex.Message;
                    }
                }
                else
                {
                    ViewBag.Message = "Please select a file to upload.";
                }

                return View("Index");
            }

            private void ProcessExcelFile(string filePath)
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    foreach (var worksheet in package.Workbook.Worksheets)
                    {
                        if (worksheet.Name != "Sheet1") // Skip empty sheets
                        {
                            ProcessWorksheet(worksheet);
                        }
                    }
                }
            }

            private void ProcessWorksheet(ExcelWorksheet worksheet)
            {
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;
                string sheetName = worksheet.Name;

                // Find the start of the data table (look for "ID No" header)
                int startRow = FindDataStartRow(worksheet, rowCount, colCount);
                if (startRow == -1) return;

                // Process the first part of the month (days 1-15)
                ProcessMonthPart(worksheet, sheetName, startRow, 5, 19); // Columns E to S (1-15)

                // Find the second part of the month (days 16-31)
                int secondPartStartRow = FindSecondPartStartRow(worksheet, startRow + 1, rowCount);
                if (secondPartStartRow != -1)
                {
                    ProcessMonthPart(worksheet, sheetName, secondPartStartRow, 5, 20); // Columns E to T (16-31)
                }
            }

            private int FindDataStartRow(ExcelWorksheet worksheet, int rowCount, int colCount)
            {
                for (int row = 1; row <= rowCount; row++)
                {
                    if (worksheet.Cells[row, 1].Text == "ID  No")
                    {
                        return row + 2; // Skip two header rows
                    }
                }
                return -1;
            }

            private int FindSecondPartStartRow(ExcelWorksheet worksheet, int startFromRow, int rowCount)
            {
                for (int row = startFromRow; row <= rowCount; row++)
                {
                    if (worksheet.Cells[row, 1].Text == "ID  No")
                    {
                        return row + 2; // Skip two header rows
                    }
                }
                return -1;
            }

            private void ProcessMonthPart(ExcelWorksheet worksheet, string sheetName, int startRow, int startCol, int endCol)
            {
                int row = startRow;

                while (!string.IsNullOrEmpty(worksheet.Cells[row, 1].Text))
                {
                    string employeeId = worksheet.Cells[row, 1].Text;
                    string employeeName = worksheet.Cells[row, 2].Text;
                    string designation = worksheet.Cells[row, 3].Text;
                    string mobileNumber = worksheet.Cells[row, 4].Text;

                    // Extract month and year from sheet name
                    DateTime baseDate = ExtractDateFromSheetName(sheetName);

                    for (int col = startCol; col <= endCol; col++)
                    {
                        string shift = worksheet.Cells[row, col].Text;
                        if (!string.IsNullOrEmpty(shift) && shift != "OFF")
                        {
                            int day = col - startCol + 1;
                            if (col > 19) day = col - 5; // Adjustment for second part

                            DateTime date = new DateTime(baseDate.Year, baseDate.Month, day);

                            // Save to database
                            SaveRosterEntry(sheetName, employeeId, employeeName, designation, mobileNumber, date, shift);
                        }
                    }

                    row++;
                }
            }

            private DateTime ExtractDateFromSheetName(string sheetName)
            {
                // Implement logic to extract date from sheet name like "FEB-2022" or "May  2022"
                // This is a simplified version - you might need more robust parsing
                string[] parts = sheetName.Split('-', ' ');
                string monthStr = parts[0].Trim();
                string yearStr = parts[1].Trim();

                int month = DateTime.ParseExact(monthStr, "MMM", CultureInfo.InvariantCulture).Month;
                int year = int.Parse(yearStr);

                return new DateTime(year, month, 1);
            }

            private void SaveRosterEntry(string sheetName, string employeeId, string employeeName,
                                        string designation, string mobileNumber, DateTime date, string shift)
            {

            var param = new
            {
                SheetName = sheetName,
                EmployeeId = employeeId,
                EmployeeName = employeeName,
                Designation = designation,
                MobileNumber = mobileNumber,
                Date = date,
                Shift = shift,
                CreatedDate = DateTime.Now
            };

            var resultData = employeeSPService.GetDataWithParameter(param, "sp_InsertRosterEntry");
            int result = 1;
            string message = "Roster entry inserted successfully";
        }
    }


    // Model (RosterEntry.cs)
    public class RosterEntry
    {
        public int Id { get; set; }
        public string SheetName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string MobileNumber { get; set; }
        public DateTime Date { get; set; }
        public string Shift { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}