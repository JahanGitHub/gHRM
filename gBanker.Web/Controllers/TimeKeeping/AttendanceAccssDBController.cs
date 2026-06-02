
using gHRM.Core.Utilities.Constants;
using gHRM.Service;
using gHRM.Service.StoreProcedure;
using gHRM.Service.TimeKeeping;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace gHRM.Web.Controllers
{
    public class AttendanceAccssDBController : BaseController
    {
        #region DbConnection and  DbCommand

        OleDbConnection con;
        OleDbCommand cmd;

        #endregion

        #region Variables

        private readonly ICompanyService companyService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IAttAttendanceService attAttendanceService;
        private readonly ITimekeepingAttendanceDeviceService timekeepingAttendanceDeviceService;

        public AttendanceAccssDBController(
              ICompanyService companyService
             , IEmployeeSPService employeeSPService
            , IAttAttendanceService attAttendanceService
            , ITimekeepingAttendanceDeviceService timekeepingAttendanceDeviceService)
        {
            this.companyService = companyService;
            this.employeeSPService = employeeSPService;
            this.attAttendanceService = attAttendanceService;
            this.timekeepingAttendanceDeviceService = timekeepingAttendanceDeviceService;
        }

        #endregion

        #region Events

        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["OfficeDayTypeList"] = items;

            var model = new AttendanceAccssDBViewModel { };

            model.AttendanceDevicesDropdown = GetAttendanceDevicesDropdown();

            return View(model);
        }

        #endregion

        #region TextDataUpload

        [HttpPost]
        public ActionResult TxtDataUpload(AttendanceAccssDBViewModel model)
        {
            try
            {
                var param = new
                {
                    EmployeeId = LoggedInEmployeeId
                };

                var companyId = SessionHelper.CompanyID;
                var company = companyService.GetAll().Where(p => p.CompanyId == companyId).FirstOrDefault();
                var fileNameT = Path.GetFileName(model.TxtFile_AttachmentU.FileName);
                var fileTypeT = Path.GetFileName(model.TxtFile_AttachmentU.ContentType);
                string uploadDay = "CSV-" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + DateTime.Now.Second;
                var path = Path.Combine(Server.MapPath("~/TimeKeepingFile"), uploadDay + fileNameT);
                model.TxtFile_AttachmentU.SaveAs(path);
                model.IsActive = true;
                string statustext = "";

                if (company.CompanyCode.Trim() == GHRMPlusCompanyConstants.GrameenCommunications)
                    BulKInsertCSVForGC(model, path);
                if (company.CompanyCode.Trim() == GHRMPlusCompanyConstants.GrameenTelecomTrust)
                    BulKInsertCSVForGTT(model, path);
                if (company.CompanyCode.Trim() == GHRMPlusCompanyConstants.PidimFoundation || company.CompanyCode.Trim() == GHRMPlusCompanyConstants.GrameenMotshoOPashusampadFoundation )
                    BulKInsertCSVForPidim(model, path);
                if (company.CompanyCode.Trim() == GHRMPlusCompanyConstants.GrameenKalyan)
                    BulKInsertCSVForGK(model, path);
                if (company.CompanyCode.Trim() == GHRMPlusCompanyConstants.JagoraniChakraFoundation)
                    BulKInsertCSVForJCF(model, path);
                if (company.CompanyCode.Trim() == GHRMPlusCompanyConstants.Proyas)
                    BulKInsertCSVForProyas(model, path);
                if (company.CompanyCode.Trim() == GHRMPlusCompanyConstants.Sangram)
                {
                    model.Company = company.CompanyCode.Trim();
                    BulKInsertCSVForSangram(model, path);
                }
                if (company.CompanyCode.Trim() == GHRMPlusCompanyConstants.GUK)
                {
                    statustext = BulKInsertCSVForGUK(model, path);
                    if (statustext != "ok") TempData["Error"] = statustext;
                }

                // code added by mahbub for grameeen trust @ 7-11-2022
                if (company.CompanyCode.Trim() == GHRMPlusCompanyConstants.GT)
                    BulKInsertCSVForGrameenTrust(model, path);
                if (company.CompanyCode.Trim() == GHRMPlusCompanyConstants.Ononyo)
                    BulKInsertCSVForOnonya(model, path);
                if (company.CompanyCode.Trim() == GHRMPlusCompanyConstants.VillageEducationResourceCenter)
                    BulKInsertCSVForVERC(model, path);

               

                if (null == TempData["Error"])
                {
                    employeeSPService.GetDataWithParameter(new
                    {
                        LEAVE_AUTO_ADJUSTMENT_DISABLED = AppSetting.GetBool(AppSetting.LEAVE_AUTO_ADJUSTMENT_DISABLED, HttpContext)
                    }, "att.SP_Att_Attendance_DataCollectonBulkInsert");
                    //}

                    TempData["Success"] = "Success message text.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region AccessDataUpload

        [HttpPost]
        public ActionResult AccessDataUpload(AttendanceAccssDBViewModel model)
        {
            model.IsActive = true;
            try
            {
                if (model.AttOfficeDayTypeIdForAccess == 0)
                {
                    Response.StatusCode = 403;
                    TempData["Error"] = "Error message text.";
                    return RedirectToAction("Index");
                }

                // File attachment
                DateTime dt = DateTime.Now;
                string uploadDay = dt.Day + "-" + dt.Month + "-" + dt.Year;
                string sec = Convert.ToString(dt.Second);
                uploadDay = "AccessDB" + uploadDay + sec;
                var fileName = Path.GetFileName(model.AccessFile_AttachmentU.FileName);
                var fileType = Path.GetFileName(model.AccessFile_AttachmentU.ContentType);

                //var path = Path.Combine(@"E:\gHRM_UploadedFile", uploadDay + fileName);
                //var path = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                var path = Path.Combine(Server.MapPath("~/TimeKeepingFile"), uploadDay + fileName);

                //file.SaveAs(path);
                model.AccessFile_AttachmentU.SaveAs(path);
                //location :: path

                InsertAccessData(path, model.AttOfficeDayTypeIdForAccess);

                TempData["Success"] = "Success message text.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException;
                return RedirectToAction("Index");
                // return GetErrorMessageResult(ex);
            }
        }

        private void InsertAccessData(string Path = "", int AttOfficeDayTypeId = 0)
        {
            try
            {
                DataSet ds = BindDetails(Path);
                var List_ViewModel = ds.Tables[0].AsEnumerable().Select(dataRow => new AttendanceAccssDBViewModel
                {
                    ID = dataRow.Field<int>("EmployeeID"),
                    Date = dataRow.Field<DateTime>("AttendanceDate"),
                    time = dataRow.Field<DateTime>("AttendanceTime")
                }).ToList();

                foreach (var Data in List_ViewModel)
                {
                    string EmployeeId = "";
                    string AttenDate = "";
                    string AttendanceTimeRaw = "";

                    EmployeeId = Data.ID.ToString();
                    AttenDate = Data.Date.ToString();
                    AttendanceTimeRaw = Data.time.ToString();


                    DateTime dt = Convert.ToDateTime(AttendanceTimeRaw);
                    string AttTime = dt.Hour.ToString("00") + ":" + dt.Minute.ToString("00") + ":" + dt.Second.ToString("00");
                    DateTime time = Convert.ToDateTime(AttTime);
                    string AttendanceTime = time.ToString("hh:mm tt");

                    DateTime fc = Convert.ToDateTime(AttenDate);
                    string AttendanceDate = fc.Day.ToString("00") + "/" + fc.Month.ToString("00") + "/" + fc.Year.ToString();

                    int value;
                    if (int.TryParse(EmployeeId, out value))
                    {
                        CommonInsertMethod(EmployeeId, AttendanceDate, AttendanceTime, AttOfficeDayTypeId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected DataSet BindDetails(string Path = "")
        {
            //@"PROVIDER=Microsoft.Jet.OLEDB.4.0;" + @"DATA SOURCE=E:\AccessKTest\AttendanceAcc.mdb"
            string Connection = @"PROVIDER=Microsoft.Jet.OLEDB.4.0;" + @"DATA SOURCE=" + Path;
            DataSet ds = new DataSet();
            string strquery = "SELECT * FROM DailyAttendance";
            using (con = new OleDbConnection(Connection)) //E:\AccessKTest
            {
                using (cmd = new OleDbCommand(strquery, con))
                {
                    OleDbDataAdapter Da = new OleDbDataAdapter(cmd);
                    Da.Fill(ds);
                }
            }
            return ds;
        }

        private void CommonInsertMethod(string EmployeeId = "", string AttendanceDate = "", string AttendanceTime = "", int AttOfficeDayTypeId = 0)
        {
            DateTime d = Convert.ToDateTime(AttendanceDate + " " + AttendanceTime);
            var param = new
            {
                EmployeeCode = EmployeeId,
                AttenDate = AttendanceDate,
                LogInType = "A",
                InOutType = "",
                InOutTime = d,
                AttOfficeMachineId = 0,
                AttOfficeDayTypeId = AttOfficeDayTypeId
            };

            var val = employeeSPService.GetDataWithParameter(param, "att.SP_Att_Attendance_DataCollecton");
        }

        public JsonResult GetLastTime()
        {
            int result = 0;
            string message = "";
            var List_EmployeeViewModel = new List<AttendanceAccssDBViewModel>();
            try
            {
                var empList = employeeSPService.GetDataWithoutParameter("att.SP_Get_LastTimeAttCSVDataHistory");

                List_EmployeeViewModel = empList.Tables[0].AsEnumerable()
                    .Select(row => new AttendanceAccssDBViewModel
                    {
                        EmployeeCode = row.Field<string>("EmployeeCode"),
                        AttendanceDate = row.Field<DateTime>("AttendanceDate"),
                        AttendanceDateMsg = row.Field<string>("AttendanceDateMsg"),
                    }).ToList();

                result = 1;
                message = "Transfer Information deleted succesfully";
            }
            catch (Exception e)
            {
                result = 0;
                message = "Failed to delete transfer Information";
            }
            return Json(new { result = result, message = message, data = List_EmployeeViewModel.ToList() }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Methods


        private void BulKInsertCSVForOnonya(AttendanceAccssDBViewModel model, string path)
        {
            string CSVFilePathName = path;
            //generate datatable collections
            DataTable dt = GetDataTableFromCsv(CSVFilePathName, true);

            if (model.AttendanceTerminal == "Finger TEC")
                attAttendanceService.BulkInsertCSVForGUKFingerTecOnonya(dt);
        }



        private void BulKInsertCSVForGrameenTrust(AttendanceAccssDBViewModel model, string path)
        {
            string CSVFilePathName = path;
            //generate datatable collections
            DataTable dt = GetDataTableFromCsv(CSVFilePathName, true);

            if (model.AttendanceTerminal == "Terminal_01")
                attAttendanceService.BulkInsertCSVForGrameenTrustTerminal01(dt);
            //else if (model.AttendanceTerminal == "Terminal_02")
            // attAttendanceService.BulkInsertCSVForGTTTerminal02(dt);
        }


        private void BulKInsertCSVForGC(AttendanceAccssDBViewModel model, string path)
        {
            string CSVFilePathName = path;
            DataTable dt = GetDataTableFromCsv(CSVFilePathName, true);

            if (model.AttendanceTerminal == "ACTAtek")
                attAttendanceService.BulkInsertCSVForACTAtekGC(dt);
            else if (model.AttendanceTerminal == "ZK Teco")
                attAttendanceService.BulkInsertCSVForZKTecoGC(dt);
        }

        private void BulKInsertCSVForGTT(AttendanceAccssDBViewModel model, string path)
        {
            string CSVFilePathName = path;
            //generate datatable collections
            DataTable dt = GetDataTableFromCsv(CSVFilePathName, true);

            if (model.AttendanceTerminal == "Terminal_01")
                attAttendanceService.BulkInsertCSVForGTTTerminal01(dt);
            else if (model.AttendanceTerminal == "Terminal_02")
                attAttendanceService.BulkInsertCSVForGTTTerminal02(dt);
        }

        private void BulKInsertCSVForPidim(AttendanceAccssDBViewModel model, string path)
        {
            string CSVFilePathName = path;
            //generate datatable collections
            DataTable dt = GetDataTableFromCsv(CSVFilePathName, true);

            if (model.AttendanceTerminal == "ZKTecho" || model.AttendanceTerminal == "Terminal_01" )
                attAttendanceService.BulkInsertCSVForPidimZKTechoTerminal(dt);
        }

        private void BulKInsertCSVForGK(AttendanceAccssDBViewModel model, string path)
        {
            string CSVFilePathName = path;
            //generate datatable collections
            DataTable dt = GetDataTableFromCsv(CSVFilePathName, true);

            if (model.AttendanceTerminal == "Finger TEC")
                attAttendanceService.BulkInsertCSVForGKFingerTecTerminal(dt);
        }

        private void BulKInsertCSVForJCF(AttendanceAccssDBViewModel model, string path)
        {
            string CSVFilePathName = path;
            //generate datatable collections
            DataTable dt = GetDataTableFromCsv(CSVFilePathName, true);

            if (model.AttendanceTerminal == "ZKTeco")
                attAttendanceService.BulkInsertCSVForJCFZKTecoTerminal(dt);
        }

        private void BulKInsertCSVForProyas(AttendanceAccssDBViewModel model, string path)
        {
            string CSVFilePathName = path;
            //generate datatable collections
            DataTable dt = GetDataTableFromCsv(CSVFilePathName, true);

            if (model.AttendanceTerminal == "ZKTeco")
                attAttendanceService.BulkInsertCSVForProyasZKTecoTerminal(dt);
        }
        private void BulKInsertCSVForSangram(AttendanceAccssDBViewModel model, string path)
        {
            string CSVFilePathName = path;
            //generate datatable collections
            DataTable dt = GetDataTableFromCsv(CSVFilePathName, true);

            if (model.AttendanceTerminal == "ZKTeco")
                attAttendanceService.BulkInsertCSVForZKTecoSangramTerminal(dt, model.Company);
        }

        private string BulKInsertCSVForGUK(AttendanceAccssDBViewModel model, string path)
        {
            string CSVFilePathName = path;
            //generate datatable collections
            DataTable dt = GetDataTableFromCsv(CSVFilePathName, true);

            if (model.AttendanceTerminal == "Finger TEC")
                return attAttendanceService.BulkInsertCSVForGUKFingerTecTerminal(dt);
            return "ok";
        }

        static DataTable GetDataTableFromCsv(string path, bool isFirstRowHeader)
        {
            StreamReader sr = new StreamReader(path);
            string[] headers = sr.ReadLine().Split(',');
            DataTable dt = new DataTable();
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }

            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }


        private IEnumerable<SelectListItem> GetAttendanceDevicesDropdown(string selected = "")
        {
            var allAttendanceDevices = timekeepingAttendanceDeviceService.GetAll();

            var attendanceDevices = allAttendanceDevices.Select(f => new ConstantDropdownItem
            {
                Text = f.DeviceName,
                Value = f.DeviceName.ToString(CultureInfo.InvariantCulture)
            });

            return attendanceDevices.Select(
               i => new SelectListItem
               {
                   Value = i.Value,
                   Text = i.Text,
                   Selected = !String.IsNullOrWhiteSpace(selected) ? selected == i.Value : i.Selected
               }).ToList();
        }
        private void BulKInsertCSVForVERC(AttendanceAccssDBViewModel model, string path)
        {
            string CSVFilePathName = path;
            //generate datatable collections
            DataTable dt = GetDataTableFromCsv(CSVFilePathName, true);

            if (model.AttendanceTerminal == "Terminal_01")
                attAttendanceService.BulKInsertCSVForVERC(dt);
        }
        #endregion
    }
}