using BasicDataAccess;
using gHRM.Core.Utilities;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels.OverTimes;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IAttAttendanceService : IServiceBase<AttAttendance>
    {
        string BulkInsertCSVForGTTTerminal01(DataTable dt);
        string BulkInsertCSVForGTTTerminal02(DataTable dt);
        string BulkInsertCSVForACTAtekGC(DataTable dt);
        string BulkInsertCSVForZKTecoGC(DataTable dt);
        string BulkInsertCSVForPidimZKTechoTerminal(DataTable dt);
        string BulkInsertCSVForGKFingerTecTerminal(DataTable dt);
        string BulkInsertCSVForJCFZKTecoTerminal(DataTable dt);
        string BulkInsertCSVForProyasZKTecoTerminal(DataTable dt);
        string BulkInsertCSVForZKTecoSangramTerminal(DataTable dt, string company);
        string BulkInsertCSVForGUKFingerTecTerminal(DataTable dt);

        string BulkInsertCSVForGrameenTrustTerminal01(DataTable dt);

        string BulkInsertCSVForGUKFingerTecOnonya(DataTable dt);

        string BulKInsertCSVForVERC(DataTable dt);
    }

    public class AttAttendanceService : IAttAttendanceService
    {
        private readonly IAttAttendanceRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AttAttendanceService(IAttAttendanceRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AttAttendance> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.AttenDate != null).OrderBy(c => c.AttAttendanceId);
            return entities;
        }
        public AttAttendance GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        #region Bulk TimeKeeping 


        public string BulkInsertCSVForGUKFingerTecOnonya(DataTable dt)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("SNo", typeof(int));
                table.Columns.Add("EmployeeCode", typeof(string));
                table.Columns.Add("EmployeeName", typeof(string));
                table.Columns.Add("AttendanceDate", typeof(DateTime));
                table.Columns.Add("AttendanceTime", typeof(string));
                table.Columns.Add("TimeStamp", typeof(DateTime));
                table.Columns.Add("EventType", typeof(string));

                DataRow row1 = table.NewRow();

                foreach (DataRow row in dt.Rows)
                {
                    var inTime = row[5].ToString().Replace('"', ' ').Trim();
                    var outTime = row[6].ToString().Replace('"', ' ').Trim();

                    if (string.IsNullOrWhiteSpace(inTime))
                        continue;

                    if ((inTime == "0:00" || inTime == "00:00") &&
                        (outTime == "0:00" || outTime == "00:00"))
                        continue;

                    if (outTime.IndexOf('(') > 0)
                    {
                        var outTimeFull = "";
                        outTimeFull = outTime;

                        var outTimeOpenBracIndex = outTimeFull.IndexOf('(');
                        outTime = outTimeFull.Substring(0, outTimeOpenBracIndex);
                    }

                    var employeeCode = row[1].ToString().Replace('"', ' ').Trim();


                    if (employeeCode.Length != 4)
                        employeeCode = CommonHelper.GetFormattedEmployeeCodeWithFiveDigit(employeeCode);


                    //TODO: for test purpose
                    //if (employeeCode == "1380")
                    //    employeeCode = employeeCode;

                    var employeeName = row[2].ToString().Replace('"', ' ').Trim();

                    var attendanceDate = row[0].ToString().Replace('"', ' ').Trim();

                    var attendanceInTime = row[5].ToString().Trim().Replace("\"", "");

                    var attendanceInTimeShortDate = DateTime.ParseExact(attendanceDate, "M/d/yyyy", CultureInfo.InvariantCulture);

                    //var attendanceInTimeShortDate = Convert.ToDateTime(attendanceDate);

                    var attendanceInTimeDate = Convert.ToDateTime($"{attendanceInTimeShortDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture)} {attendanceInTime}", CultureInfo.InvariantCulture);
                    string attendanceInTimeOnly = attendanceInTimeDate.ToString("hh:mm:ss");
                    var inTimeTimeStamp = attendanceInTimeDate;

                    //for login time
                    row1 = table.NewRow();

                    row1["SNo"] = 0;
                    row1["EmployeeCode"] = employeeCode;
                    row1["EmployeeName"] = employeeName;
                    row1["AttendanceDate"] = attendanceInTimeShortDate.ToString("yyyy-MM-dd");
                    row1["AttendanceTime"] = attendanceInTimeOnly;
                    row1["TimeStamp"] = inTimeTimeStamp;
                    row1["EventType"] = AttendanceEventTypeConstants.InTime;

                    table.Rows.Add(row1);

                    if (string.IsNullOrWhiteSpace(outTime) || outTime == "0:00" || outTime == "00:00")
                        continue;

                    var attendanceOutTime = row[6].ToString();

                    if (attendanceOutTime.IndexOf('(') > 0)
                    {
                        var attendanceOutTimeFull = "";
                        attendanceOutTimeFull = attendanceOutTime;

                        var outTimeOpenBracIndex = attendanceOutTimeFull.IndexOf('(');
                        attendanceOutTime = attendanceOutTimeFull.Substring(0, outTimeOpenBracIndex);
                    }

                    //if logout time
                    if (string.IsNullOrWhiteSpace(attendanceOutTime))
                        continue;
                    attendanceOutTime = attendanceOutTime.Trim().Replace("\"", "");

                    var attendanceOutTimeShortDate = DateTime.ParseExact(attendanceDate, "M/d/yyyy", CultureInfo.InvariantCulture);

                    var attendanceOutTimeDate = Convert.ToDateTime($"{attendanceOutTimeShortDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture)} {attendanceOutTime}", CultureInfo.InvariantCulture);
                    string attendanceOutTimeOnly = attendanceOutTimeDate.ToString("hh:mm:ss");
                    var outTimeTimeStamp = attendanceOutTimeDate;

                    row1 = table.NewRow();

                    row1["SNo"] = 0;
                    row1["EmployeeCode"] = employeeCode;
                    row1["EmployeeName"] = employeeName;
                    row1["AttendanceDate"] = attendanceOutTimeShortDate.ToString("yyyy-MM-dd");
                    row1["AttendanceTime"] = attendanceOutTimeOnly;
                    row1["TimeStamp"] = outTimeTimeStamp;
                    row1["EventType"] = AttendanceEventTypeConstants.OutTime;

                    table.Rows.Add(row1);
                }

                var gbData = new gHRMDataAccess();

                var ConnString = gbData.GetConnectionString();
                using (SqlConnection con = new SqlConnection(ConnString))
                {
                    SqlDataAdapter adp = new SqlDataAdapter();

                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        // Set the database table name
                        sqlBulkCopy.DestinationTableName = "att.AttCSVData";
                        con.Open();
                        sqlBulkCopy.WriteToServer(table);
                        con.Close();
                    }
                }
                return "ok";
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        public string BulkInsertCSVForGrameenTrustTerminal01(DataTable dt)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("SNo", typeof(int));
                table.Columns.Add("EmployeeCode", typeof(string));
                table.Columns.Add("EmployeeName", typeof(string));
                table.Columns.Add("AttendanceDate", typeof(DateTime));
                table.Columns.Add("AttendanceTime", typeof(string));
                table.Columns.Add("TimeStamp", typeof(DateTime));
                table.Columns.Add("EventType", typeof(string));

                DataRow row1 = table.NewRow();

                foreach (DataRow row in dt.Rows)
                {
                    var inTime = row[12].ToString().Replace('"', ' ').Trim();
                    var outTime = row[15].ToString().Replace('"', ' ').Trim();

                    if (string.IsNullOrWhiteSpace(inTime))
                        continue;

                    if ((inTime == "0:00" || inTime == "00:00") &&
                        (outTime == "0:00" || outTime == "00:00"))
                        continue;

                    if (outTime.IndexOf('(') > 0)
                    {
                        var outTimeFull = "";
                        outTimeFull = outTime;

                        var outTimeOpenBracIndex = outTimeFull.IndexOf('(');
                        outTime = outTimeFull.Substring(0, outTimeOpenBracIndex);
                    }

                    var attendanceResultType = row[25].ToString();

                    if (attendanceResultType == AttendanceResultTypeConstants.AbsenceTrue)
                        continue;

                    var employeeCode = row[1].ToString().Replace('"', ' ').Trim();
                    if (employeeCode.Length != 4)
                    {
                        if (employeeCode.Length == 2) /// GTHO-02
                        {
                            employeeCode = "GTHO-" + employeeCode;
                        }
                        if (employeeCode.Length == 1)
                        {
                            employeeCode = "GTHO-0" + employeeCode;
                        }
                        if (employeeCode.Length == 3)
                        {
                            employeeCode = "GTHO-" + employeeCode;
                        }
                    }

                    //TODO: for test purpose
                    //if (employeeCode == "1380")
                    //    employeeCode = employeeCode;

                    var employeeName = row[2].ToString().Replace('"', ' ').Trim();

                    var attendanceDate = row[8].ToString().Replace('"', ' ').Trim();

                    var attendanceInTime = row[12].ToString();

                    //var attendanceInTimeShortDate = DateTime.ParseExact(attendanceDate, "M/d/yyyy", CultureInfo.InvariantCulture);

                    var attendanceInTimeShortDate = Convert.ToDateTime(attendanceDate);

                    var attendanceInTimeDate = Convert.ToDateTime($"{attendanceInTimeShortDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture)} {attendanceInTime}", CultureInfo.InvariantCulture);
                    string attendanceInTimeOnly = attendanceInTimeDate.ToString("hh:mm:ss");
                    var inTimeTimeStamp = attendanceInTimeDate;

                    //for login time
                    row1 = table.NewRow();

                    row1["SNo"] = 0;
                    row1["EmployeeCode"] = employeeCode;
                    row1["EmployeeName"] = employeeName;
                    row1["AttendanceDate"] = attendanceInTimeShortDate.ToString("yyyy-MM-dd");
                    row1["AttendanceTime"] = attendanceInTimeOnly;
                    row1["TimeStamp"] = inTimeTimeStamp;
                    row1["EventType"] = AttendanceEventTypeConstants.InTime;

                    table.Rows.Add(row1);

                    if ((outTime == "0:00" || outTime == "00:00"))
                        continue;

                    var attendanceOutTime = row[15].ToString();

                    if (attendanceOutTime.IndexOf('(') > 0)
                    {
                        var attendanceOutTimeFull = "";
                        attendanceOutTimeFull = attendanceOutTime;

                        var outTimeOpenBracIndex = attendanceOutTimeFull.IndexOf('(');
                        attendanceOutTime = attendanceOutTimeFull.Substring(0, outTimeOpenBracIndex);
                    }

                    //if logout time
                    if (attendanceOutTime != null)
                    {
                        //  var attendanceOutTimeShortDate = DateTime.ParseExact(attendanceDate, "M/d/yyyy", CultureInfo.InvariantCulture);

                        var attendanceOutTimeShortDate = Convert.ToDateTime(attendanceDate);

                        var attendanceOutTimeDate = Convert.ToDateTime($"{attendanceOutTimeShortDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture)} {attendanceOutTime}", CultureInfo.InvariantCulture);
                        string attendanceOutTimeOnly = attendanceOutTimeDate.ToString("hh:mm:ss");
                        var outTimeTimeStamp = attendanceOutTimeDate;

                        row1 = table.NewRow();

                        row1["SNo"] = 0;
                        row1["EmployeeCode"] = employeeCode;
                        row1["EmployeeName"] = employeeName;
                        row1["AttendanceDate"] = attendanceOutTimeShortDate.ToString("yyyy-MM-dd");
                        row1["AttendanceTime"] = attendanceOutTimeOnly;
                        row1["TimeStamp"] = outTimeTimeStamp;
                        row1["EventType"] = AttendanceEventTypeConstants.OutTime;

                        table.Rows.Add(row1);
                    }
                }

                var gbData = new gHRMDataAccess();

                var ConnString = gbData.GetConnectionString();
                using (SqlConnection con = new SqlConnection(ConnString))
                {
                    SqlDataAdapter adp = new SqlDataAdapter();

                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        // Set the database table name
                        sqlBulkCopy.DestinationTableName = "att.AttCSVData";
                        con.Open();
                        sqlBulkCopy.WriteToServer(table);
                        con.Close();
                    }
                }
                return "ok";
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public string BulkInsertCSVForGTTTerminal01(DataTable dt)
        {
            DataTable table = new DataTable();
            table.Columns.Add("SNo", typeof(int));
            table.Columns.Add("EmployeeCode", typeof(string));
            table.Columns.Add("EmployeeName", typeof(string));
            table.Columns.Add("AttendanceDate", typeof(DateTime));
            table.Columns.Add("AttendanceTime", typeof(string));
            table.Columns.Add("TimeStamp", typeof(DateTime));
            table.Columns.Add("EventType", typeof(string));

            DataRow row1 = table.NewRow();

            foreach (DataRow row in dt.Rows)
            {
                var employeeCode = row[0].ToString().Replace('"', ' ').Trim();
                if (employeeCode.Length != 4)
                {
                    if (employeeCode.Length == 2)
                    {
                        employeeCode = "00" + employeeCode;
                    }
                    if (employeeCode.Length == 1)
                    {
                        employeeCode = "000" + employeeCode;
                    }
                    if (employeeCode.Length == 3)
                    {
                        employeeCode = "0" + employeeCode;
                    }
                }

                var employeeName = row[1].ToString().Replace('"', ' ').Trim();
                var attendanceDate = row[2].ToString().Replace('"', ' ').Trim();
                var attendanceDateTime = Convert.ToDateTime(attendanceDate);
                var attendanceDateOnly = attendanceDateTime.ToShortDateString();
                string attendanceTimeOnly = attendanceDateTime.ToString("hh:mm:ss");

                var eventType = row[3].ToString().Replace('"', ' ').Trim();

                row1 = table.NewRow();
                row1["SNo"] = 0;
                row1["EmployeeCode"] = employeeCode;
                row1["EmployeeName"] = employeeName;
                row1["AttendanceDate"] = attendanceDateOnly;
                row1["AttendanceTime"] = attendanceTimeOnly;
                var timeStamp = attendanceDateTime;
                row1["TimeStamp"] = timeStamp;
                row1["EventType"] = eventType == "IN" ? AttendanceEventTypeConstants.InTime : AttendanceEventTypeConstants.OutTime;

                table.Rows.Add(row1);
            }

            var gbData = new gHRMDataAccess();

            var ConnString = gbData.GetConnectionString();
            using (SqlConnection con = new SqlConnection(ConnString))
            {
                SqlDataAdapter adp = new SqlDataAdapter();

                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    // Set the database table name
                    sqlBulkCopy.DestinationTableName = "att.AttCSVData";

                    con.Open();
                    sqlBulkCopy.WriteToServer(table);
                    con.Close();
                }
            }
            return "ok";
        }

        public string BulkInsertCSVForGTTTerminal02(DataTable dt)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("SNo", typeof(int));
                table.Columns.Add("EmployeeCode", typeof(string));
                table.Columns.Add("EmployeeName", typeof(string));
                table.Columns.Add("AttendanceDate", typeof(DateTime));
                table.Columns.Add("AttendanceTime", typeof(string));
                table.Columns.Add("TimeStamp", typeof(DateTime));
                table.Columns.Add("EventType", typeof(string));

                DataRow row1 = table.NewRow();

                foreach (DataRow row in dt.Rows)
                {
                    var inTime = row[5].ToString().Replace('"', ' ').Trim();
                    var outTime = row[6].ToString().Replace('"', ' ').Trim();

                    if (string.IsNullOrWhiteSpace(inTime))
                        continue;

                    if ((inTime == "0:00" || inTime == "00:00") &&
                        (outTime == "0:00" || outTime == "00:00"))
                        continue;

                    if (outTime.IndexOf('(') > 0)
                    {
                        var outTimeFull = "";
                        outTimeFull = outTime;

                        var outTimeOpenBracIndex = outTimeFull.IndexOf('(');
                        outTime = outTimeFull.Substring(0, outTimeOpenBracIndex);
                    }

                    var attendanceResultType = row[7].ToString();

                    if (attendanceResultType == AttendanceResultTypeConstants.Absence)
                        continue;

                    var employeeCode = row[1].ToString().Replace('"', ' ').Trim();
                    if (employeeCode.Length != 4)
                    {
                        if (employeeCode.Length == 2)
                        {
                            employeeCode = "00" + employeeCode;
                        }
                        if (employeeCode.Length == 1)
                        {
                            employeeCode = "000" + employeeCode;
                        }
                        if (employeeCode.Length == 3)
                        {
                            employeeCode = "0" + employeeCode;
                        }
                    }

                    //if (employeeCode == "0005")
                    //    employeeCode = employeeCode;

                    var employeeName = row[2].ToString().Replace('"', ' ').Trim();

                    var attendanceDate = row[0].ToString().Replace('"', ' ').Trim();

                    var attendanceInTime = row[5].ToString();

                    var attendanceInTimeShortDate = DateTime.ParseExact(attendanceDate, "M/d/yyyy", CultureInfo.InvariantCulture);

                    var attendanceInTimeDate = Convert.ToDateTime($"{attendanceInTimeShortDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture)} {attendanceInTime}", CultureInfo.InvariantCulture);
                    string attendanceInTimeOnly = attendanceInTimeDate.ToString("hh:mm:ss");
                    var inTimeTimeStamp = attendanceInTimeDate;

                    //for login time
                    row1 = table.NewRow();

                    row1["SNo"] = 0;
                    row1["EmployeeCode"] = employeeCode;
                    row1["EmployeeName"] = employeeName;
                    row1["AttendanceDate"] = attendanceInTimeShortDate.ToString("yyyy-MM-dd");
                    row1["AttendanceTime"] = attendanceInTimeOnly;
                    row1["TimeStamp"] = inTimeTimeStamp;
                    row1["EventType"] = AttendanceEventTypeConstants.InTime;

                    table.Rows.Add(row1);

                    if ((outTime == "0:00" || outTime == "00:00"))
                        continue;

                    var attendanceOutTime = row[6].ToString();

                    if (attendanceOutTime.IndexOf('(') > 0)
                    {
                        var attendanceOutTimeFull = "";
                        attendanceOutTimeFull = attendanceOutTime;

                        var outTimeOpenBracIndex = attendanceOutTimeFull.IndexOf('(');
                        attendanceOutTime = attendanceOutTimeFull.Substring(0, outTimeOpenBracIndex);
                    }

                    //if logout time
                    if (attendanceOutTime != null)
                    {
                        var attendanceOutTimeShortDate = DateTime.ParseExact(attendanceDate, "M/d/yyyy", CultureInfo.InvariantCulture);

                        var attendanceOutTimeDate = Convert.ToDateTime($"{attendanceOutTimeShortDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture)} {attendanceOutTime}", CultureInfo.InvariantCulture);
                        string attendanceOutTimeOnly = attendanceOutTimeDate.ToString("hh:mm:ss");
                        var outTimeTimeStamp = attendanceOutTimeDate;

                        row1 = table.NewRow();

                        row1["SNo"] = 0;
                        row1["EmployeeCode"] = employeeCode;
                        row1["EmployeeName"] = employeeName;
                        row1["AttendanceDate"] = attendanceOutTimeShortDate.ToString("yyyy-MM-dd");
                        row1["AttendanceTime"] = attendanceOutTimeOnly;
                        row1["TimeStamp"] = outTimeTimeStamp;
                        row1["EventType"] = AttendanceEventTypeConstants.OutTime;

                        table.Rows.Add(row1);
                    }
                }

                var gbData = new gHRMDataAccess();

                var ConnString = gbData.GetConnectionString();
                using (SqlConnection con = new SqlConnection(ConnString))
                {
                    SqlDataAdapter adp = new SqlDataAdapter();

                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        // Set the database table name
                        sqlBulkCopy.DestinationTableName = "att.AttCSVData";
                        con.Open();
                        sqlBulkCopy.WriteToServer(table);
                        con.Close();
                    }
                }
                return "ok";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public string BulkInsertCSVForPidimZKTechoTerminal(DataTable dt)
        //{
        //    try
        //    {
        //        DataTable table = new DataTable();
        //        table.Columns.Add("SNo", typeof(int));
        //        table.Columns.Add("EmployeeCode", typeof(string));
        //        table.Columns.Add("EmployeeName", typeof(string));
        //        table.Columns.Add("AttendanceDate", typeof(DateTime));
        //        table.Columns.Add("AttendanceTime", typeof(string));
        //        table.Columns.Add("TimeStamp", typeof(DateTime));
        //        table.Columns.Add("EventType", typeof(string));

        //        DataRow row1 = table.NewRow();

        //        foreach (DataRow row in dt.Rows)
        //        {
        //            var inTime = row[7].ToString().Replace('"', ' ').Trim();
        //            var outTime = row[8].ToString().Replace('"', ' ').Trim();

        //            if (string.IsNullOrWhiteSpace(inTime))
        //                continue;

        //            if ((inTime == "0:00" || inTime == "00:00") &&
        //                (outTime == "0:00" || outTime == "00:00"))
        //                continue;

        //            if (outTime.IndexOf('(') > 0)
        //            {
        //                var outTimeFull = "";
        //                outTimeFull = outTime;

        //                var outTimeOpenBracIndex = outTimeFull.IndexOf('(');
        //                outTime = outTimeFull.Substring(0, outTimeOpenBracIndex);
        //            }

        //            var attendanceResultType = row[11].ToString();

        //            if (attendanceResultType == AttendanceResultTypeConstants.AbsenceTrue)
        //                continue;

        //            var employeeCode = row[2].ToString().Replace('"', ' ').Trim();
        //            if (employeeCode.Length != 4)
        //            {
        //                if (employeeCode.Length == 2)
        //                {
        //                    employeeCode = "00" + employeeCode;
        //                }
        //                if (employeeCode.Length == 1)
        //                {
        //                    employeeCode = "000" + employeeCode;
        //                }
        //                if (employeeCode.Length == 3)
        //                {
        //                    employeeCode = "0" + employeeCode;
        //                }
        //            }

        //            //TODO: for test purpose
        //            //if (employeeCode == "1380")
        //            //    employeeCode = employeeCode;

        //            var employeeName = row[3].ToString().Replace('"', ' ').Trim();

        //            var attendanceDate = row[4].ToString().Replace('"', ' ').Trim();

        //            var attendanceInTime = row[7].ToString();

        //            var attendanceInTimeShortDate = DateTime.ParseExact(attendanceDate, "M/d/yyyy", CultureInfo.InvariantCulture);

        //            var attendanceInTimeDate = Convert.ToDateTime($"{attendanceInTimeShortDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture)} {attendanceInTime}", CultureInfo.InvariantCulture);
        //            string attendanceInTimeOnly = attendanceInTimeDate.ToString("hh:mm:ss");
        //            var inTimeTimeStamp = attendanceInTimeDate;

        //            //for login time
        //            row1 = table.NewRow();

        //            row1["SNo"] = 0;
        //            row1["EmployeeCode"] = employeeCode;
        //            row1["EmployeeName"] = employeeName;
        //            row1["AttendanceDate"] = attendanceInTimeShortDate.ToString("yyyy-MM-dd");
        //            row1["AttendanceTime"] = attendanceInTimeOnly;
        //            row1["TimeStamp"] = inTimeTimeStamp;
        //            row1["EventType"] = AttendanceEventTypeConstants.InTime;

        //            table.Rows.Add(row1);

        //            if ((outTime == "0:00" || outTime == "00:00"))
        //                continue;

        //            var attendanceOutTime = row[8].ToString();

        //            if (attendanceOutTime.IndexOf('(') > 0)
        //            {
        //                var attendanceOutTimeFull = "";
        //                attendanceOutTimeFull = attendanceOutTime;

        //                var outTimeOpenBracIndex = attendanceOutTimeFull.IndexOf('(');
        //                attendanceOutTime = attendanceOutTimeFull.Substring(0, outTimeOpenBracIndex);
        //            }

        //            //if logout time
        //            if (attendanceOutTime != null)
        //            {
        //                var attendanceOutTimeShortDate = DateTime.ParseExact(attendanceDate, "M/d/yyyy", CultureInfo.InvariantCulture);

        //                var attendanceOutTimeDate = Convert.ToDateTime($"{attendanceOutTimeShortDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture)} {attendanceOutTime}", CultureInfo.InvariantCulture);
        //                string attendanceOutTimeOnly = attendanceOutTimeDate.ToString("hh:mm:ss");
        //                var outTimeTimeStamp = attendanceOutTimeDate;

        //                row1 = table.NewRow();

        //                row1["SNo"] = 0;
        //                row1["EmployeeCode"] = employeeCode;
        //                row1["EmployeeName"] = employeeName;
        //                row1["AttendanceDate"] = attendanceOutTimeShortDate.ToString("yyyy-MM-dd");
        //                row1["AttendanceTime"] = attendanceOutTimeOnly;
        //                row1["TimeStamp"] = outTimeTimeStamp;
        //                row1["EventType"] = AttendanceEventTypeConstants.OutTime;

        //                table.Rows.Add(row1);
        //            }
        //        }

        //        var gbData = new gHRMDataAccess();

        //        var ConnString = gbData.GetConnectionString();
        //        using (SqlConnection con = new SqlConnection(ConnString))
        //        {
        //            SqlDataAdapter adp = new SqlDataAdapter();

        //            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
        //            {
        //                // Set the database table name
        //                sqlBulkCopy.DestinationTableName = "att.AttCSVData";
        //                con.Open();
        //                sqlBulkCopy.WriteToServer(table);
        //                con.Close();
        //            }
        //        }
        //        return "ok";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public string BulkInsertCSVForPidimZKTechoTerminal(DataTable dt)
        {
            try

            {

                DataTable table = new DataTable();

                table.Columns.Add("SNo", typeof(int));

                table.Columns.Add("EmployeeCode", typeof(string));

                table.Columns.Add("EmployeeName", typeof(string));

                table.Columns.Add("AttendanceDate", typeof(DateTime));

                table.Columns.Add("AttendanceTime", typeof(string));

                table.Columns.Add("TimeStamp", typeof(DateTime));

                table.Columns.Add("EventType", typeof(string));

                DataRow row1 = table.NewRow();

                foreach (DataRow row in dt.Rows)

                {

                    var inTime = row[4].ToString().Replace('"', ' ').Trim();

                    var outTime = row[5].ToString().Replace('"', ' ').Trim();

                    if (string.IsNullOrWhiteSpace(inTime))

                        continue;

                    if ((inTime == "0:00" || inTime == "00:00") &&

                        (outTime == "0:00" || outTime == "00:00"))

                        continue;

                    if (outTime.IndexOf('(') > 0)

                    {

                        var outTimeFull = "";

                        outTimeFull = outTime;

                        var outTimeOpenBracIndex = outTimeFull.IndexOf('(');

                        outTime = outTimeFull.Substring(0, outTimeOpenBracIndex);

                    }

                    var attendanceResultType = row[11].ToString();

                    if (attendanceResultType == AttendanceResultTypeConstants.AbsenceTrue)

                        continue;

                    var employeeCode = row[1].ToString().Replace('"', ' ').Trim();

                    if (employeeCode.Length != 4)

                    {

                        if (employeeCode.Length == 2)

                        {

                            employeeCode = "00" + employeeCode;

                        }

                        if (employeeCode.Length == 1)

                        {

                            employeeCode = "000" + employeeCode;

                        }

                        if (employeeCode.Length == 3)

                        {

                            employeeCode = "0" + employeeCode;

                        }

                    }

                    //TODO: for test purpose

                    //if (employeeCode == "1380")

                    //    employeeCode = employeeCode;

                    var employeeName = row[2].ToString().Replace('"', ' ').Trim();

                    var attendanceDate = row[3].ToString().Replace('"', ' ').Trim();

                    var attendanceInTime = row[4].ToString();

                    var attendanceInTimeShortDate = DateTime.ParseExact(attendanceDate, "M/d/yyyy", CultureInfo.InvariantCulture);

                    var attendanceInTimeDate = Convert.ToDateTime($"{attendanceInTimeShortDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture)} {attendanceInTime}", CultureInfo.InvariantCulture);

                    string attendanceInTimeOnly = attendanceInTimeDate.ToString("hh:mm:ss");

                    var inTimeTimeStamp = attendanceInTimeDate;

                    //for login time

                    row1 = table.NewRow();

                    row1["SNo"] = 0;

                    row1["EmployeeCode"] = employeeCode;

                    row1["EmployeeName"] = employeeName;

                    row1["AttendanceDate"] = attendanceInTimeShortDate.ToString("yyyy-MM-dd");

                    row1["AttendanceTime"] = attendanceInTimeOnly;

                    row1["TimeStamp"] = inTimeTimeStamp;

                    row1["EventType"] = AttendanceEventTypeConstants.InTime;

                    table.Rows.Add(row1);

                    if ((outTime == "0:00" || outTime == "00:00"))

                        continue;

                    var attendanceOutTime = row[5].ToString();

                    if (attendanceOutTime.IndexOf('(') > 0)

                    {

                        var attendanceOutTimeFull = "";

                        attendanceOutTimeFull = attendanceOutTime;

                        var outTimeOpenBracIndex = attendanceOutTimeFull.IndexOf('(');

                        attendanceOutTime = attendanceOutTimeFull.Substring(0, outTimeOpenBracIndex);

                    }

                    //if logout time

                    if (attendanceOutTime != null)
                    {

                        var attendanceOutTimeShortDate = DateTime.ParseExact(attendanceDate, "M/d/yyyy", CultureInfo.InvariantCulture);

                        var attendanceOutTimeDate = Convert.ToDateTime($"{attendanceOutTimeShortDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture)} {attendanceOutTime}", CultureInfo.InvariantCulture);

                        string attendanceOutTimeOnly = attendanceOutTimeDate.ToString("hh:mm:ss");

                        var outTimeTimeStamp = attendanceOutTimeDate;

                        row1 = table.NewRow();

                        row1["SNo"] = 0;

                        row1["EmployeeCode"] = employeeCode;

                        row1["EmployeeName"] = employeeName;

                        row1["AttendanceDate"] = attendanceOutTimeShortDate.ToString("yyyy-MM-dd");

                        row1["AttendanceTime"] = attendanceOutTimeOnly;

                        row1["TimeStamp"] = outTimeTimeStamp;

                        row1["EventType"] = AttendanceEventTypeConstants.OutTime;

                        table.Rows.Add(row1);
                    }
                }

                var gbData = new gHRMDataAccess();

                var ConnString = gbData.GetConnectionString();
                using (SqlConnection con = new SqlConnection(ConnString))
                {
                    SqlDataAdapter adp = new SqlDataAdapter();

                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        // Set the database table name
                        sqlBulkCopy.DestinationTableName = "att.AttCSVData";
                        con.Open();
                        sqlBulkCopy.WriteToServer(table);
                        con.Close();
                    }
                }
                return "ok";
            }
        catch (Exception ex)
            {
                throw;
            }
        }

        public string BulkInsertCSVForGKFingerTecTerminal(DataTable dt)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("SNo", typeof(int));
                table.Columns.Add("EmployeeCode", typeof(string));
                table.Columns.Add("EmployeeName", typeof(string));
                table.Columns.Add("AttendanceDate", typeof(DateTime));
                table.Columns.Add("AttendanceTime", typeof(string));
                table.Columns.Add("TimeStamp", typeof(DateTime));
                table.Columns.Add("EventType", typeof(string));

                DataRow row1 = table.NewRow();

                foreach (DataRow row in dt.Rows)
                {
                    var inTime = row[12].ToString().Replace('"', ' ').Trim();
                    var outTime = row[15].ToString().Replace('"', ' ').Trim();

                    if (string.IsNullOrWhiteSpace(inTime))
                        continue;

                    if ((inTime == "0:00" || inTime == "00:00") &&
                        (outTime == "0:00" || outTime == "00:00"))
                        continue;

                    if (outTime.IndexOf('(') > 0)
                    {
                        var outTimeFull = "";
                        outTimeFull = outTime;

                        var outTimeOpenBracIndex = outTimeFull.IndexOf('(');
                        outTime = outTimeFull.Substring(0, outTimeOpenBracIndex);
                    }

                    var employeeCode = row[1].ToString().Replace('"', ' ').Trim();

                    if (employeeCode.Length != 5)
                        employeeCode = CommonHelper.GetFormattedEmployeeCodeWithFiveDigit(employeeCode);


                    //TODO: for test purpose
                    //if (employeeCode == "1380")
                    //    employeeCode = employeeCode;

                    var employeeName = row[2].ToString().Replace('"', ' ').Trim();

                    var attendanceDate = row[8].ToString().Replace('"', ' ').Trim();

                    var attendanceInTime = row[12].ToString().Trim().Replace("\"", "");

                    var attendanceInTimeShortDate = DateTime.ParseExact(attendanceDate, "d/M/yyyy", CultureInfo.InvariantCulture);

                    var attendanceInTimeDate = Convert.ToDateTime($"{attendanceInTimeShortDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture)} {attendanceInTime}", CultureInfo.InvariantCulture);
                    string attendanceInTimeOnly = attendanceInTimeDate.ToString("hh:mm:ss");
                    var inTimeTimeStamp = attendanceInTimeDate;

                    //for login time
                    row1 = table.NewRow();

                    row1["SNo"] = 0;
                    row1["EmployeeCode"] = employeeCode;
                    row1["EmployeeName"] = employeeName;
                    row1["AttendanceDate"] = attendanceInTimeShortDate.ToString("yyyy-MM-dd");
                    row1["AttendanceTime"] = attendanceInTimeOnly;
                    row1["TimeStamp"] = inTimeTimeStamp;
                    row1["EventType"] = AttendanceEventTypeConstants.InTime;

                    table.Rows.Add(row1);

                    if (string.IsNullOrWhiteSpace(outTime) || outTime == "0:00" || outTime == "00:00")
                        continue;

                    var attendanceOutTime = row[15].ToString();

                    if (attendanceOutTime.IndexOf('(') > 0)
                    {
                        var attendanceOutTimeFull = "";
                        attendanceOutTimeFull = attendanceOutTime;

                        var outTimeOpenBracIndex = attendanceOutTimeFull.IndexOf('(');
                        attendanceOutTime = attendanceOutTimeFull.Substring(0, outTimeOpenBracIndex);
                    }

                    //if logout time
                    if (string.IsNullOrWhiteSpace(attendanceOutTime))
                        continue;
                    attendanceOutTime = attendanceOutTime.Trim().Replace("\"", "");

                    var attendanceOutTimeShortDate = DateTime.ParseExact(attendanceDate, "d/M/yyyy", CultureInfo.InvariantCulture);

                    var attendanceOutTimeDate = Convert.ToDateTime($"{attendanceOutTimeShortDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture)} {attendanceOutTime}", CultureInfo.InvariantCulture);
                    string attendanceOutTimeOnly = attendanceOutTimeDate.ToString("hh:mm:ss");
                    var outTimeTimeStamp = attendanceOutTimeDate;

                    row1 = table.NewRow();

                    row1["SNo"] = 0;
                    row1["EmployeeCode"] = employeeCode;
                    row1["EmployeeName"] = employeeName;
                    row1["AttendanceDate"] = attendanceOutTimeShortDate.ToString("yyyy-MM-dd");
                    row1["AttendanceTime"] = attendanceOutTimeOnly;
                    row1["TimeStamp"] = outTimeTimeStamp;
                    row1["EventType"] = AttendanceEventTypeConstants.OutTime;

                    table.Rows.Add(row1);
                }

                var gbData = new gHRMDataAccess();

                var ConnString = gbData.GetConnectionString();
                using (SqlConnection con = new SqlConnection(ConnString))
                {
                    SqlDataAdapter adp = new SqlDataAdapter();

                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        // Set the database table name
                        sqlBulkCopy.DestinationTableName = "att.AttCSVData";
                        con.Open();
                        sqlBulkCopy.WriteToServer(table);
                        con.Close();
                    }
                }
                return "ok";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string BulkInsertCSVForJCFZKTecoTerminal(DataTable dt)
        {
            int count = 1;
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("SNo", typeof(int));
                table.Columns.Add("EmployeeCode", typeof(string));
                table.Columns.Add("EmployeeName", typeof(string));
                table.Columns.Add("AttendanceDate", typeof(DateTime));
                table.Columns.Add("AttendanceTime", typeof(string));
                table.Columns.Add("TimeStamp", typeof(DateTime));
                table.Columns.Add("EventType", typeof(string));

                DataRow row1 = table.NewRow();

                foreach (DataRow row in dt.Rows)
                {
                    var inTime = row[9].ToString().Replace('"', ' ').Trim();
                    var outTime = row[10].ToString().Replace('"', ' ').Trim();

                    if (string.IsNullOrWhiteSpace(inTime))
                        continue;

                    if ((inTime == "0:00" || inTime == "00:00") &&
                        (outTime == "0:00" || outTime == "00:00"))
                        continue;

                    if (outTime.IndexOf('(') > 0)
                    {
                        var outTimeFull = "";
                        outTimeFull = outTime;

                        var outTimeOpenBracIndex = outTimeFull.IndexOf('(');
                        outTime = outTimeFull.Substring(0, outTimeOpenBracIndex);
                    }

                    var attendanceResultType = row[15].ToString();

                    if (attendanceResultType == AttendanceResultTypeConstants.AbsenceTrue)
                        continue;

                    //get employee code
                    var employeeCode = row[1].ToString().Replace('"', ' ').Trim();
                    if (employeeCode.Length != 6)
                        employeeCode = CommonHelper.GetFormattedEmployeeCodeWithSixDigit(employeeCode);

                    if (employeeCode == "001225")
                    {
                        int ininddd = 12;
                    }

                    var employeeName = row[3].ToString().Replace('"', ' ').Trim();

                    var attDate = row[5].ToString().Replace('"', ' ').Trim();
                    var splittedAttDate = attDate.Split('-');
                    var attDay = splittedAttDate[0];
                    string attendanceDate = attDate;
                    if (attDay.Length == 1)
                    {
                        var attMonth = splittedAttDate[1];
                        var attYear = splittedAttDate[2];

                        attendanceDate = $"0{attDay}-{attMonth}-{attYear}";
                    }                        
                    var attendanceInTime = row[9].ToString();

                    //var attendanceInTimeShortDate = DateTime.ParseExact(attendanceDate, "M-D-yy", CultureInfo.InvariantCulture);

                    var attendanceInTimeShortDate = DateTime.ParseExact(attendanceDate, "dd-MMM-yy", CultureInfo.InvariantCulture);
                    //DateTime.ParseExact(attendanceDate, "d/M/yyyy", CultureInfo.InvariantCulture);

                    var attendanceInTimeDate = Convert.ToDateTime($"{attendanceInTimeShortDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture)} {attendanceInTime}", CultureInfo.InvariantCulture);
                    string attendanceInTimeOnly = attendanceInTimeDate.ToString("hh:mm:ss");
                    var inTimeTimeStamp = attendanceInTimeDate;

                    //for login time
                    row1 = table.NewRow();

                    row1["SNo"] = 0;
                    row1["EmployeeCode"] = employeeCode;
                    row1["EmployeeName"] = employeeName;
                    row1["AttendanceDate"] = attendanceInTimeShortDate.ToString("yyyy-MM-dd");
                    row1["AttendanceTime"] = attendanceInTimeOnly;
                    row1["TimeStamp"] = inTimeTimeStamp;
                    row1["EventType"] = AttendanceEventTypeConstants.InTime;

                    table.Rows.Add(row1);

                    if ((outTime == "0:00" || outTime == "00:00"))
                        continue;

                    var attendanceOutTime = row[10].ToString();

                    if (attendanceOutTime.IndexOf('(') > 0)
                    {
                        var attendanceOutTimeFull = "";
                        attendanceOutTimeFull = attendanceOutTime;

                        var outTimeOpenBracIndex = attendanceOutTimeFull.IndexOf('(');
                        attendanceOutTime = attendanceOutTimeFull.Substring(0, outTimeOpenBracIndex);
                    }

                    //if logout time
                    if (attendanceOutTime != null)
                    {
                        //var attendanceOutTimeShortDate = DateTime.ParseExact(attendanceDate, "M-D-yy", CultureInfo.InvariantCulture);
                        var attendanceOutTimeShortDate = DateTime.ParseExact(attendanceDate, "dd-MMM-yy", CultureInfo.InvariantCulture);
                        //DateTime.ParseExact(attendanceDate, "d/M/yyyy", CultureInfo.InvariantCulture);

                        var attendanceOutTimeDate = Convert.ToDateTime($"{attendanceOutTimeShortDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture)} {attendanceOutTime}", CultureInfo.InvariantCulture);
                        string attendanceOutTimeOnly = attendanceOutTimeDate.ToString("hh:mm:ss");
                        var outTimeTimeStamp = attendanceOutTimeDate;

                        row1 = table.NewRow();

                        row1["SNo"] = 0;
                        row1["EmployeeCode"] = employeeCode;
                        row1["EmployeeName"] = employeeName;
                        row1["AttendanceDate"] = attendanceOutTimeShortDate.ToString("yyyy-MM-dd");
                        row1["AttendanceTime"] = attendanceOutTimeOnly;
                        row1["TimeStamp"] = outTimeTimeStamp;
                        row1["EventType"] = AttendanceEventTypeConstants.OutTime;

                        table.Rows.Add(row1);
                    }

                    count++;

                    if (count >= 2000)
                    {
                        count = 0;

                        // if error don't continue, fall back
                        AddBulkAttendances(table);
                        table.Rows.Clear();
                    }
                }

                if (count > 0)
                {
                    count = 0;

                    // if error don't continue, fall back
                    AddBulkAttendances(table);
                    table.Rows.Clear();
                }

                return "ok";
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string BulkInsertCSVForProyasZKTecoTerminal(DataTable dt)
        {
            DataTable table = new DataTable();
            table.Columns.Add("SNo", typeof(int));
            table.Columns.Add("EmployeeCode", typeof(string));
            table.Columns.Add("EmployeeName", typeof(string));
            table.Columns.Add("AttendanceDate", typeof(DateTime));
            table.Columns.Add("AttendanceTime", typeof(string));
            table.Columns.Add("TimeStamp", typeof(DateTime));
            table.Columns.Add("EventType", typeof(string));
            DataRow row1 = table.NewRow();

            foreach (DataRow row in dt.Rows)
            {
                var employeeCode = row[2].ToString().Replace('"', ' ').Trim();
                if (employeeCode.Length != 4)
                    employeeCode = CommonHelper.GetFormattedEmployeeCodeWithFourDigit(employeeCode);

                var employeeName = row[1].ToString().Replace('"', ' ').Trim() + " " + row[2].ToString().Replace('"', ' ').Trim();
                var dateAtten = row[3].ToString().Replace('"', ' ').Trim();

                var attendanceDate = dateAtten.Split(' ')[0];
                var timeAttendance = dateAtten.Split(' ')[1];

                var dateTime = DateTime.ParseExact(attendanceDate, "M-d-yy", CultureInfo.InvariantCulture);
                var dateOnly = dateTime.ToShortDateString();
                var attendanceTime = timeAttendance.Trim();
                var timeStamp = Convert.ToDateTime(dateOnly + " " + attendanceTime);
                //var eventType = row[7].ToString().Replace('"', ' ').Trim();

                row1 = table.NewRow();
                row1["SNo"] = 0;
                row1["EmployeeCode"] = employeeCode;
                row1["EmployeeName"] = employeeName;
                row1["AttendanceDate"] = dateTime;
                row1["AttendanceTime"] = attendanceTime;
                row1["TimeStamp"] = timeStamp;
                row1["EventType"] = AttendanceEventTypeConstants.InTime;
                table.Rows.Add(row1);
            }

            var gbData = new gHRMDataAccess();

            var ConnString = gbData.GetConnectionString();
            using (SqlConnection con = new SqlConnection(ConnString))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    //Set the database table name
                    sqlBulkCopy.DestinationTableName = "att.AttCSVData";

                    con.Open();
                    sqlBulkCopy.WriteToServer(table);
                    con.Close();
                }
            }
            return "ok";
        }
        public string BulkInsertCSVForACTAtekGC(DataTable dt)
        {
            DataTable table = new DataTable();
            table.Columns.Add("SNo", typeof(int));
            table.Columns.Add("EmployeeCode", typeof(string));
            table.Columns.Add("EmployeeName", typeof(string));
            table.Columns.Add("AttendanceDate", typeof(DateTime));
            table.Columns.Add("AttendanceTime", typeof(string));
            table.Columns.Add("TimeStamp", typeof(DateTime));
            table.Columns.Add("EventType", typeof(string));
            DataRow row1 = table.NewRow();

            foreach (DataRow row in dt.Rows)
            {
                var employeeCode = row[1].ToString().Replace('"', ' ').Trim();
                if (employeeCode.Length != 4)
                {
                    if (employeeCode.Length == 2)
                    {
                        employeeCode = "00" + employeeCode;
                    }
                    if (employeeCode.Length == 1)
                    {
                        employeeCode = "000" + employeeCode;
                    }
                    if (employeeCode.Length == 3)
                    {
                        employeeCode = "0" + employeeCode;
                    }
                }
                var employeeName = row[2].ToString().Replace('"', ' ').Trim() + " " + row[3].ToString().Replace('"', ' ').Trim();
                var dateAtten = row[5].ToString().Replace('"', ' ').Trim();

                var dateTime = DateTime.ParseExact(dateAtten, "yyyy/M/d", CultureInfo.InvariantCulture);
                var dateOnly = dateTime.ToShortDateString();
                var attendanceTime = row[6].ToString().Replace('"', ' ').Trim();
                var timeStamp = Convert.ToDateTime(dateOnly + " " + attendanceTime);
                var eventType = row[7].ToString().Replace('"', ' ').Trim();

                row1 = table.NewRow();
                row1["SNo"] = 0;
                row1["EmployeeCode"] = employeeCode;
                row1["EmployeeName"] = employeeName;
                row1["AttendanceDate"] = dateTime;
                row1["AttendanceTime"] = attendanceTime;
                row1["TimeStamp"] = timeStamp;
                row1["EventType"] = eventType == "IN" ? AttendanceEventTypeConstants.InTime : AttendanceEventTypeConstants.OutTime;
                table.Rows.Add(row1);
            }

            var gbData = new gHRMDataAccess();

            var ConnString = gbData.GetConnectionString();
            using (SqlConnection con = new SqlConnection(ConnString))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    //Set the database table name
                    sqlBulkCopy.DestinationTableName = "att.AttCSVData";

                    con.Open();
                    sqlBulkCopy.WriteToServer(table);
                    con.Close();
                }
            }
            return "ok";
        }

        public string BulkInsertCSVForZKTecoGC(DataTable dt)
        {
            DataTable table = new DataTable();
            table.Columns.Add("SNo", typeof(int));
            table.Columns.Add("EmployeeCode", typeof(string));
            table.Columns.Add("EmployeeName", typeof(string));
            table.Columns.Add("AttendanceDate", typeof(DateTime));
            table.Columns.Add("AttendanceTime", typeof(string));
            table.Columns.Add("TimeStamp", typeof(DateTime));
            table.Columns.Add("EventType", typeof(string));
            DataRow row1 = table.NewRow();

            foreach (DataRow row in dt.Rows)
            {
                var employeeCode = row[2].ToString().Replace('"', ' ').Trim();
                if (employeeCode.Length != 4)
                {
                    if (employeeCode.Length == 2)
                        employeeCode = "00" + employeeCode;
                    if (employeeCode.Length == 1)
                        employeeCode = "000" + employeeCode;
                    if (employeeCode.Length == 3)
                        employeeCode = "0" + employeeCode;
                }
                var employeeName = row[1].ToString().Replace('"', ' ').Trim();
                var dateAtten = row[3].ToString().Replace('"', ' ').Trim();

                var dateTime = DateTime.ParseExact(dateAtten, "M/d/yyyy h:mm tt", CultureInfo.InvariantCulture);
                var dateOnly = dateTime.ToShortDateString();
                var attendanceTime = dateTime.ToString("HH:mm:ss");
                var timeStamp = Convert.ToDateTime(dateOnly + " " + attendanceTime);
                var eventType = "IN";

                row1 = table.NewRow();
                row1["SNo"] = 0;
                row1["EmployeeCode"] = employeeCode;
                row1["EmployeeName"] = employeeName;
                row1["AttendanceDate"] = dateTime;
                row1["AttendanceTime"] = attendanceTime;
                row1["TimeStamp"] = timeStamp;
                row1["EventType"] = eventType == "IN" ? AttendanceEventTypeConstants.InTime : AttendanceEventTypeConstants.OutTime;
                table.Rows.Add(row1);
            }

            var gbData = new gHRMDataAccess();

            var ConnString = gbData.GetConnectionString();
            using (SqlConnection con = new SqlConnection(ConnString))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    //Set the database table name
                    sqlBulkCopy.DestinationTableName = "att.AttCSVData";

                    con.Open();
                    sqlBulkCopy.WriteToServer(table);
                    con.Close();
                }
            }
            return "ok";
        }

        public string BulkInsertCSVForZKTecoSangramTerminal(DataTable dt, string company)
        {
            int count = 1;
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("SNo", typeof(int));
                table.Columns.Add("EmployeeCode", typeof(string));
                table.Columns.Add("EmployeeName", typeof(string));
                table.Columns.Add("AttendanceDate", typeof(DateTime));
                table.Columns.Add("AttendanceTime", typeof(string));
                table.Columns.Add("TimeStamp", typeof(DateTime));
                table.Columns.Add("EventType", typeof(string));

                DataRow row1 = table.NewRow();

                //Populate Eployee Attendances
                var employeeAttendances = PopulateEmployeeAttendances(dt, company);
                if (!employeeAttendances.Any())
                    return "Employee attendance not found.";

                var startDate = employeeAttendances.Min(f => f.InOutDateTime).Date;
                var endDate = employeeAttendances.Max(f => f.InOutDateTime).Date;
                var employeeCodes = employeeAttendances.Select(f => f.EmployeeCode).Distinct();

                //for (var date = startDate; date <= endDate; date.AddDays(1))
                while (startDate <= endDate)
                {
                    foreach (var code in employeeCodes)
                    {
                        //TODO: for test purpose
                        if (code == "000029")
                        {
                            var coded = code;
                        }

                        var filterEmpAttendance = employeeAttendances
                            .Where(f =>
                                f.EmployeeCode == code &&
                                f.InOutDateTime.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) == startDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)
                            ).OrderBy(o => o.InOutDateTime).AsParallel().ToList();

                        if (!filterEmpAttendance.Any())
                            continue;

                        //intime
                        var attendanceIntime = filterEmpAttendance.OrderBy(f => f.InOutDateTime).ToList().FirstOrDefault();
                        row1 = table.NewRow();
                        row1["SNo"] = count;
                        row1["EmployeeCode"] = attendanceIntime.EmployeeCode;
                        row1["EmployeeName"] = attendanceIntime.EmployeeName;
                        row1["AttendanceDate"] = attendanceIntime.InOutDateTime.Date;
                        row1["AttendanceTime"] = attendanceIntime.InOutDateTime.ToString("hh:mm tt"); ;
                        row1["TimeStamp"] = attendanceIntime.InOutDateTime;
                        row1["EventType"] = AttendanceEventTypeConstants.InTime;
                        table.Rows.Add(row1);

                        count++;

                        if (filterEmpAttendance.Count() <= 1)
                            continue;

                        //out time
                        var attendanceOutime = filterEmpAttendance.OrderByDescending(f => f.InOutDateTime).ToList().FirstOrDefault();
                        row1 = table.NewRow();
                        row1["SNo"] = count;
                        row1["EmployeeCode"] = attendanceOutime.EmployeeCode;
                        row1["EmployeeName"] = attendanceOutime.EmployeeName;
                        row1["AttendanceDate"] = attendanceOutime.InOutDateTime.Date;
                        row1["AttendanceTime"] = attendanceOutime.InOutDateTime.ToString("hh:mm tt"); ;
                        row1["TimeStamp"] = attendanceOutime.InOutDateTime;
                        row1["EventType"] = AttendanceEventTypeConstants.OutTime;
                        table.Rows.Add(row1);

                        count++;
                    }

                    startDate = startDate.AddDays(1);
                }

                var gbData = new gHRMDataAccess();

                var ConnString = gbData.GetConnectionString();
                using (SqlConnection con = new SqlConnection(ConnString))
                {
                    SqlDataAdapter adp = new SqlDataAdapter();

                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        // Set the database table name
                        sqlBulkCopy.DestinationTableName = "att.AttCSVData";
                        con.Open();
                        sqlBulkCopy.WriteToServer(table);
                        con.Close();
                    }
                }
                return "ok";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string BulkInsertCSVForGUKFingerTecTerminal(DataTable dt)
        {
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("SNo", typeof(int));
                table.Columns.Add("EmployeeCode", typeof(string));
                table.Columns.Add("EmployeeName", typeof(string));
                table.Columns.Add("AttendanceDate", typeof(DateTime));
                table.Columns.Add("AttendanceTime", typeof(string));
                table.Columns.Add("TimeStamp", typeof(DateTime));
                table.Columns.Add("EventType", typeof(string));

                DataRow row1 = table.NewRow();
                DateTime AttendanceDate;
                string AttDateStr = dt.Rows[0][0].ToString().Replace('"', ' ').Trim();
                try
                {
                    if (string.IsNullOrEmpty(AttDateStr)) return "Attendance Date is required on second row first cell";
                    AttendanceDate = DateTime.ParseExact(AttDateStr, "d/M/yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    return "Attendance Date must be with format dd/mm/yyyy";
                }
                foreach (DataRow row in dt.Rows)
                {
                    var inTime = row[17].ToString().Replace('"', ' ').Trim();
                    var outTime = row[20].ToString().Replace('"', ' ').Trim();

                    if (string.IsNullOrWhiteSpace(inTime) || inTime == "In")
                        continue;

                    if ((inTime == "0:00" || inTime == "00:00") &&
                        (outTime == "0:00" || outTime == "00:00"))
                        continue;

                    if (outTime.IndexOf('(') > 0)
                    {
                        var outTimeFull = "";
                        outTimeFull = outTime;

                        var outTimeOpenBracIndex = outTimeFull.IndexOf('(');
                        outTime = outTimeFull.Substring(0, outTimeOpenBracIndex);
                    }

                    var employeeCode = row[5].ToString().Replace('"', ' ').Trim();


                    //TODO: for test purpose
                    //if (employeeCode == "1380")
                    //    employeeCode = employeeCode;

                    var employeeName = row[7].ToString().Replace('"', ' ').Trim();

                    var attendanceDate = AttendanceDate.ToString("dd/MM/yyyy");

                    var attendanceInTime = row[17].ToString().Trim().Replace("\"", "");

                    var attendanceInTimeShortDate = AttendanceDate;

                    var attendanceInTimeDate = Convert.ToDateTime($"{attendanceInTimeShortDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture)} {attendanceInTime}", CultureInfo.InvariantCulture);
                    string attendanceInTimeOnly = attendanceInTimeDate.ToString("hh:mm:ss");
                    var inTimeTimeStamp = attendanceInTimeDate;

                    //for login time
                    row1 = table.NewRow();

                    row1["SNo"] = 0;
                    row1["EmployeeCode"] = employeeCode;
                    row1["EmployeeName"] = employeeName;
                    row1["AttendanceDate"] = attendanceInTimeShortDate.ToString("yyyy-MM-dd");
                    row1["AttendanceTime"] = attendanceInTimeOnly;
                    row1["TimeStamp"] = inTimeTimeStamp;
                    row1["EventType"] = AttendanceEventTypeConstants.InTime;

                    table.Rows.Add(row1);

                    if (string.IsNullOrWhiteSpace(outTime) || outTime == "0:00" || outTime == "00:00")
                        continue;

                    var attendanceOutTime = row[20].ToString();

                    if (attendanceOutTime.IndexOf('(') > 0)
                    {
                        var attendanceOutTimeFull = "";
                        attendanceOutTimeFull = attendanceOutTime;

                        var outTimeOpenBracIndex = attendanceOutTimeFull.IndexOf('(');
                        attendanceOutTime = attendanceOutTimeFull.Substring(0, outTimeOpenBracIndex);
                    }

                    //if logout time
                    if (string.IsNullOrWhiteSpace(attendanceOutTime))
                        continue;
                    attendanceOutTime = attendanceOutTime.Trim().Replace("\"", "");

                    var attendanceOutTimeShortDate = DateTime.ParseExact(attendanceDate, "d/M/yyyy", CultureInfo.InvariantCulture);

                    var attendanceOutTimeDate = Convert.ToDateTime($"{attendanceOutTimeShortDate.ToString("M/d/yyyy", CultureInfo.InvariantCulture)} {attendanceOutTime}", CultureInfo.InvariantCulture);
                    string attendanceOutTimeOnly = attendanceOutTimeDate.ToString("hh:mm:ss");
                    var outTimeTimeStamp = attendanceOutTimeDate;

                    row1 = table.NewRow();

                    row1["SNo"] = 0;
                    row1["EmployeeCode"] = employeeCode;
                    row1["EmployeeName"] = employeeName;
                    row1["AttendanceDate"] = attendanceOutTimeShortDate.ToString("yyyy-MM-dd");
                    row1["AttendanceTime"] = attendanceOutTimeOnly;
                    row1["TimeStamp"] = outTimeTimeStamp;
                    row1["EventType"] = AttendanceEventTypeConstants.OutTime;

                    table.Rows.Add(row1);
                }

                var gbData = new gHRMDataAccess();

                var ConnString = gbData.GetConnectionString();
                using (SqlConnection con = new SqlConnection(ConnString))
                {
                    SqlDataAdapter adp = new SqlDataAdapter();

                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        // Set the database table name
                        sqlBulkCopy.DestinationTableName = "att.AttCSVData";
                        con.Open();
                        sqlBulkCopy.WriteToServer(table);
                        con.Close();
                    }
                }
                return "ok";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private List<EmployeeAttendanceModel> PopulateEmployeeAttendances(DataTable dt, string company = "")
        {
            var employeeAttendances = new List<EmployeeAttendanceModel>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    //get employee code
                    var employeeCode = row[2].ToString().Replace('"', ' ').Trim();
                    if (company == GHRMPlusCompanyConstants.Sangram)
                    {
                        if (employeeCode.Length != 5)
                            employeeCode = CommonHelper.GetFormattedEmployeeCodeWithFiveDigit(employeeCode);
                    }
                    else
                    {
                        if (employeeCode.Length != 6)
                            employeeCode = CommonHelper.GetFormattedEmployeeCodeWithSixDigit(employeeCode);
                    }
                    //TODO: for test purpose
                    if (employeeCode == "000029")
                    {
                        var coded = employeeCode;
                    }

                    var employeeName = row[1].ToString().Replace('"', ' ').Trim();
                    var dateAtten = row[3].ToString().Replace('"', ' ').Trim();
                    var fragmenetedAttendDate = dateAtten.Split(' ');
                    var attendanceDate = fragmenetedAttendDate[0];
                    var attendanceTime = $"{fragmenetedAttendDate[1]}";
                    var amPM = $"{fragmenetedAttendDate[2]}";

                    var dateTime = DateTime.ParseExact(attendanceDate, "M/d/yyyy", CultureInfo.InvariantCulture);
                    var dateOnly = dateTime.ToShortDateString();
                    var timeStamp = Convert.ToDateTime($"{ dateOnly}  { attendanceTime}{amPM}");

                    employeeAttendances.Add(new EmployeeAttendanceModel
                    {
                        EmployeeCode = employeeCode,
                        EmployeeName = employeeName,
                        InOutDateTime = timeStamp,
                        AttendanceTime = attendanceTime,
                    });
                }
                catch
                {
                    continue;
                }
            }

            return employeeAttendances;
        }

        #endregion
        public void Save()
        {
            unitOfWork.Commit();
        }

        public AttAttendance Get(Expression<Func<AttAttendance, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AttAttendance> GetMany(Expression<Func<AttAttendance, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<AttAttendance>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<AttAttendance>> GetManyAsync(Expression<Func<AttAttendance, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<AttAttendance> GetAsync(Expression<Func<AttAttendance, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        public AttAttendance Create(AttAttendance objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AttAttendance objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            //throw new NotImplementedException();
            var obj = repository.GetById(id);
            if (obj != null)
            {
                // obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
                // obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }

        public bool IsContinued(long id)
        {
            // throw new NotImplementedException();
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = true;  //obj.IsActive;
                if (isActive == false)
                {
                    return false;
                }
            }

            return true;
        }
        #region    verc
        public string BulKInsertCSVForVERC(DataTable dt)
        {
            int count = 1;
            try
            {
                DataTable table = new DataTable();
                table.Columns.Add("SNo", typeof(int));
                table.Columns.Add("EmployeeCode", typeof(string));
                table.Columns.Add("EmployeeName", typeof(string));
                table.Columns.Add("AttendanceDate", typeof(DateTime));
                table.Columns.Add("AttendanceTime", typeof(string));
                table.Columns.Add("TimeStamp", typeof(DateTime));
                table.Columns.Add("EventType", typeof(string));

                DataRow row1 = table.NewRow();

                //Populate Eployee Attendances
                var employeeAttendances = PopulateEmployeeAttendancesVERC(dt);
                if (!employeeAttendances.Any())
                    return "Employee attendance not found.";

                var startDate = employeeAttendances.Min(f => f.InOutDateTime).Date;
                var endDate = employeeAttendances.Max(f => f.InOutDateTime).Date;
                var employeeCodes = employeeAttendances.Select(f => f.EmployeeCode).Distinct();

                //for (var date = startDate; date <= endDate; date.AddDays(1))
                while (startDate <= endDate)
                {
                    foreach (var code in employeeCodes)
                    {
                        //TODO: for test purpose
                        if (code == "000029")
                        {
                            var coded = code;
                        }

                        var filterEmpAttendance = employeeAttendances
                            .Where(f =>
                                f.EmployeeCode == code &&
                                f.InOutDateTime.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) == startDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)
                            ).OrderBy(o => o.InOutDateTime).AsParallel().ToList();

                        if (!filterEmpAttendance.Any())
                            continue;

                        //intime
                        var attendanceIntime = filterEmpAttendance.OrderBy(f => f.InOutDateTime).ToList().FirstOrDefault();
                        row1 = table.NewRow();
                        row1["SNo"] = count;
                        row1["EmployeeCode"] = attendanceIntime.EmployeeCode;
                        row1["EmployeeName"] = attendanceIntime.EmployeeName;
                        row1["AttendanceDate"] = attendanceIntime.InOutDateTime.Date;
                        row1["AttendanceTime"] = attendanceIntime.InOutDateTime.ToString("hh:mm tt"); ;
                        row1["TimeStamp"] = attendanceIntime.InOutDateTime;
                        row1["EventType"] = AttendanceEventTypeConstants.InTime;
                        table.Rows.Add(row1);

                        count++;

                        if (filterEmpAttendance.Count() <= 1)
                            continue;

                        //out time
                        var attendanceOutime = filterEmpAttendance.OrderByDescending(f => f.InOutDateTime).ToList().FirstOrDefault();
                        row1 = table.NewRow();
                        row1["SNo"] = count;
                        row1["EmployeeCode"] = attendanceOutime.EmployeeCode;
                        row1["EmployeeName"] = attendanceOutime.EmployeeName;
                        row1["AttendanceDate"] = attendanceOutime.InOutDateTime.Date;
                        row1["AttendanceTime"] = attendanceOutime.InOutDateTime.ToString("hh:mm tt"); ;
                        row1["TimeStamp"] = attendanceOutime.InOutDateTime;
                        row1["EventType"] = AttendanceEventTypeConstants.OutTime;
                        table.Rows.Add(row1);

                        count++;
                    }

                    startDate = startDate.AddDays(1);
                }

                var gbData = new gHRMDataAccess();

                var ConnString = gbData.GetConnectionString();
                using (SqlConnection con = new SqlConnection(ConnString))
                {
                    SqlDataAdapter adp = new SqlDataAdapter();

                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        // Set the database table name
                        sqlBulkCopy.DestinationTableName = "att.AttCSVData";
                        con.Open();
                        sqlBulkCopy.WriteToServer(table);
                        con.Close();
                    }
                }
                return "ok";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private List<EmployeeAttendanceModel> PopulateEmployeeAttendancesVERC(DataTable dt)
        {
            var employeeAttendances = new List<EmployeeAttendanceModel>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    //get employee code
                    var employeeCode = row[0].ToString().Replace('"', ' ').Trim();

                    if (employeeCode.Length != 4)
                        employeeCode = CommonHelper.GetFormattedEmployeeCodeWithFourDigit(employeeCode);


                    // var employeeName = row[1].ToString().Replace('"', ' ').Trim();
                    var dateAtten = row[1].ToString().Replace('"', ' ').Trim();
                    var fragmenetedAttendDate = dateAtten.Split(' ');
                    var attendanceDate = fragmenetedAttendDate[0];
                    var attendanceTime = $"{fragmenetedAttendDate[1]}";
                    //var amPM = $"{fragmenetedAttendDate[2]}";

                    // var dateTime = DateTime.ParseExact(attendanceDate, "M/d/yyyy", CultureInfo.InvariantCulture);
                    var dateTime = DateTime.ParseExact(attendanceDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //  dd / MM / yy
                    var dateOnly = dateTime.ToShortDateString();
                    var timeStamp = Convert.ToDateTime($"{ dateOnly}  { attendanceTime}");

                    employeeAttendances.Add(new EmployeeAttendanceModel
                    {
                        EmployeeCode = employeeCode,
                        // EmployeeName = employeeName,
                        InOutDateTime = timeStamp,
                        AttendanceTime = attendanceTime,
                    });
                }
                catch(Exception ex)
                {
                    var tt = ex.Message;
                    continue;
                }
            }

            return employeeAttendances;
        }
        #endregion verc
        #region Private Methods

        private bool AddBulkAttendances(DataTable dt)
        {
            try
            {
                var gbData = new gHRMDataAccess();

                var ConnString = gbData.GetConnectionString();
                using (SqlConnection con = new SqlConnection(ConnString))
                {
                    SqlDataAdapter adp = new SqlDataAdapter();

                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        // Set the database table name
                        sqlBulkCopy.DestinationTableName = "att.AttCSVData";
                        con.Open();

                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
