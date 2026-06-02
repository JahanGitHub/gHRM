using System;
using System.Data;
using BasicDataAccess;
using System.Data.SqlClient;
using System.Globalization;
using gHRM.Data.DBDetailModels.Payroll;
using System.Collections.Generic;
using gHRM.Core.Filters.Payroll;
using gHRM.Data.CodeFirstMigration;
using System.Linq;
using gHRM.Data.DBDetailModels.OverTimes;
using gHRM.Core.Filters.TimeKeepings;
using System.Data.Entity.Validation;
using gHRM.Core.Utilities.Constants;
using gHRM.Core.Filters.Employee;
using gHRM.Data.DBDetailModels.Employee;
using gHRM.Core.Utilities;
using BasicDataAccess.Data;
using gHRM.Data.CodeFirstMigration.Payroll;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;

namespace gHRM.Service.StoreProcedure
{
    public interface IEmployeeSPService
    {
        DataSet GetDataWithParameterIncrement<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataWithoutParameter(string storeProcedureName);
        List<EmployeeProfileModel> GetEmployeeProfileHeaderByFilter(EmployeeSearchFilter filter);
        List<DBPRComponentListViewModel> GetListingByFilter(PRComponentSearchFilter filter);
        List<DBPRComponentListViewModel_designation> GetListingByFilter_designation(PRComponentSearchFilter_designation filter);
        IEnumerable<TimeKeepingReportModel> GetTimeKeepingReportDataByFilter(TimeKeepingReportSearchFilter filter);
        List<EmployeeBasicServiceBookModel> GetEmployeeServiceBookInfoListingByFilter(string employeeCode, string bloodGroup, int officeTypeId, int OfficeId, int DeptId, int payRollDesignation, string responsibility, int Section, string status, bool SERVICE_BOOK_REPORT_HEIGHT_CONVERT_INCHES_TO_CM);
        List<MonthlySalaryForServiceBookModel> GetMonthlySalaryForServiceBookListingByFilter(string employeeCode, string bloodGroup, int officeTypeId, int OfficeId, int DeptId, int payRollDesignation, string responsibility, int Section, string status);
        List<LeaveRecordServiceBookModel> GetLeaveRecordServiceBookListingByFilter(string employeeCode, int? leaveTypeId, string bloodGroup, int officeTypeId, int OfficeId, int DeptId, int payRollDesignation, string responsibility, int Section, string status);
        List<CaseNoSlNoModel> GetEmployeeWiseCaseHistoryServiceBookListingByFilter(string employeeCode, string bloodGroup, int officeTypeId, int OfficeId, int DeptId, int payRollDesignation, string responsibility, int Section, string status);
    }

    public class EmployeeSPService : IEmployeeSPService
    {
        public DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gHRMDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataWithoutParameter(string storeProcedureName)
        {
            using (var gbData = new gHRMDataAccess())
            {
                return gbData.GetDataOnDatesetWithoutParam(storeProcedureName);
            }
        }

        public List<DBPRComponentListViewModel> GetListingByFilter(PRComponentSearchFilter filter)
        {
            var filterList = new List<DBPRComponentListViewModel>();

            try
            {
                using (var db = new gHRMDBContext())
                {
                    var employeeTypeId = filter.EmployeeTypeId > 0 ? filter.EmployeeTypeId.ToString() : "NULL";
                    var employeeStatusId = filter.EmployeeStatusId > 0 ? filter.EmployeeStatusId.ToString() : "NULL";

                    var sqlCommand = $@"[prl].[PRComponent_GetComponentListByFilter]
                                '{employeeTypeId}',
                                '{employeeStatusId}'
                                ";

                    filterList = db.Database.SqlQuery<DBPRComponentListViewModel>(sqlCommand)
                        .AsParallel().ToList();
                }
            }
            catch (Exception ex)
            {
                filterList = new List<DBPRComponentListViewModel>();
            }

            return filterList;
        }

        public List<DBPRComponentListViewModel_designation> GetListingByFilter_designation(PRComponentSearchFilter_designation filter)
        {
            var filterList = new List<DBPRComponentListViewModel_designation>();

            try
            {
                using (var db = new gHRMDBContext())
                {
                    var employeeTypeId = filter.EmployeeTypeId > 0 ? filter.EmployeeTypeId.ToString() : "NULL";
                    var employeeStatusId = filter.EmployeeStatusId > 0 ? filter.EmployeeStatusId.ToString() : "NULL";
                    var DesignationId = filter.DesignationId > 0 ? filter.DesignationId.ToString() : "NULL";

                    var sqlCommand = $@"[prl].[PRComponent_GetComponentListByFilter_designation]
                                '{employeeTypeId}',
                                '{employeeStatusId}',
                                '{DesignationId}'
                                ";

                    filterList = db.Database.SqlQuery<DBPRComponentListViewModel_designation>(sqlCommand)
                        .AsParallel().ToList();
                }
            }
            catch (Exception ex)
            {
                filterList = new List<DBPRComponentListViewModel_designation>();
            }

            return filterList;
        }


        public IEnumerable<TimeKeepingReportModel> GetTimeKeepingReportDataByFilter(TimeKeepingReportSearchFilter filter)
        {
            var filterList = new List<TimeKeepingReportModel>();

            try
            {
                using (var db = new gHRMDBContext())
                {
                    var sqlCommand = $@"[att].[SP_RPT_Timekeeping_ByEmployeeCode]
                                '{filter.EmployeeCode}',
                                '{Convert.ToDateTime(filter.StartDate).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}',
                                '{Convert.ToDateTime(filter.EndDate).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}',
                                {filter.PreparedBy}
                                ";

                    filterList = db.Database.SqlQuery<TimeKeepingReportModel>(sqlCommand)
                        .AsParallel().ToList();

                    //populate total overtime
                    if (filter.GHRMPlusCompany == GHRMPlusCompanyConstants.GrameenCommunications)
                        PopulateTotalOvertimeForGC(filterList, filter);
                    else
                        PopulateTotalOvertime(filterList, filter);
                }
            }
            catch (Exception ex)
            {
                filterList = new List<TimeKeepingReportModel>();
            }
            return filterList;
        }

        public List<EmployeeProfileModel> GetEmployeeProfileHeaderByFilter(EmployeeSearchFilter filter)
        {
            var filterList = new List<EmployeeProfileModel>();

            try
            {
                using (var db = new gHRMDBContext())
                {
                    var sqlCommand = $@"[emp].[SP_RPT_PROFILE_EmployeeBasicInfo_Header] '{filter.EmployeeId}'";

                    filterList = db.Database.SqlQuery<EmployeeProfileModel>(sqlCommand)
                        .AsParallel().ToList();
                }
            }
            catch (Exception ex)
            {
                filterList = new List<EmployeeProfileModel>();
            }
            return filterList;
        }

        public List<EmployeeBasicServiceBookModel> GetEmployeeServiceBookInfoListingByFilter(string employeeCode, string bloodGroup, int officeTypeId, int OfficeId, int DeptId, int payRollDesignation, string responsibility, int Section, string status, bool SERVICE_BOOK_REPORT_HEIGHT_CONVERT_INCHES_TO_CM)
        {
            var filterList = new List<EmployeeBasicServiceBookModel>();

            try
            {
                using (var db = new gHRMDBContext())
                {
                    var SQL = "EXEC dbo.Employee_GetEmployeeServiceBookInfo {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}";
                    object[] parameters = new object[] { employeeCode, officeTypeId, OfficeId, payRollDesignation, status, DeptId, Section, bloodGroup, responsibility, SERVICE_BOOK_REPORT_HEIGHT_CONVERT_INCHES_TO_CM };

                    filterList = db.Database.SqlQuery<EmployeeBasicServiceBookModel>(SQL, parameters)
                        .AsParallel().ToList();
                }
            }
            catch (Exception ex)
            {
                filterList = new List<EmployeeBasicServiceBookModel>();
            }

            return filterList;
        }

        public List<MonthlySalaryForServiceBookModel> GetMonthlySalaryForServiceBookListingByFilter(string employeeCode, string bloodGroup, int officeTypeId, int OfficeId, int DeptId, int payRollDesignation, string responsibility, int Section, string status)
        {
            var filterList = new List<MonthlySalaryForServiceBookModel>();

            try
            {
                using (var db = new gHRMDBContext())
                {
                    var SQL = "EXEC prl.EmployeeMonthlySalaryApproved_MonthlySalaryForServiceBook {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}";
                    object[] parameters = new object[] { employeeCode, officeTypeId, OfficeId, payRollDesignation, status, DeptId, Section, bloodGroup, responsibility };

                    filterList = db.Database.SqlQuery<MonthlySalaryForServiceBookModel>(SQL, parameters)
                        .AsParallel().ToList();
                }
            }
            catch (Exception ex)
            {
                filterList = new List<MonthlySalaryForServiceBookModel>();
            }

            return filterList;
        }

        public List<LeaveRecordServiceBookModel> GetLeaveRecordServiceBookListingByFilter(string employeeCode, int? leaveTypeId, string bloodGroup, int officeTypeId, int OfficeId, int DeptId, int payRollDesignation, string responsibility, int Section, string status)
        {
            var filterList = new List<LeaveRecordServiceBookModel>();

            try
            {
                using (var db = new gHRMDBContext())
                {
                    var SQL = "EXEC leave.LeaveHistory_GetLeaveRecordServiceBook {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}";
                    object[] parameters = new object[] { employeeCode, leaveTypeId, officeTypeId, OfficeId, payRollDesignation, status, DeptId, Section, bloodGroup, responsibility };

                    filterList = db.Database.SqlQuery<LeaveRecordServiceBookModel>(SQL, parameters)
                        .AsParallel().ToList();
                }
            }
            catch (Exception ex)
            {
                filterList = new List<LeaveRecordServiceBookModel>();
            }

            return filterList;
        }

        public List<CaseNoSlNoModel> GetEmployeeWiseCaseHistoryServiceBookListingByFilter(string employeeCode, string bloodGroup, int officeTypeId, int OfficeId, int DeptId, int payRollDesignation, string responsibility, int Section, string status)
        {
            var filterList = new List<CaseNoSlNoModel>();

            try
            {
                using (var db = new gHRMDBContext())
                {
                    var SQL = "EXEC disc.DiscCaseMaster_GetEmployeeWiseCaseHistoryServiceBook {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}";
                    object[] parameters = new object[] { employeeCode, officeTypeId, OfficeId, payRollDesignation, status, DeptId, Section, bloodGroup, responsibility };

                    filterList = db.Database.SqlQuery<CaseNoSlNoModel>(SQL, parameters)
                        .AsParallel().ToList();
                }
            }
            catch (Exception ex)
            {
                filterList = new List<CaseNoSlNoModel>();
            }

            return filterList;
        }

        #region Private Methods

        private void PopulateTotalOvertimeForGC(List<TimeKeepingReportModel> filterList, TimeKeepingReportSearchFilter filter)
        {
            if (filterList.Any())
            {
                var holidayDeclarations = new List<AttHolidayDeclaration>();
                var overtimeExceptions = new List<OvertimeException>();
                var exceptionalTimeKeepingList = new List<EmployeeTimeKeepingException>();

                double totalOvertime = 0, totalHours = 0, totalMinutes = 0, totalSeconds = 0;

                var employee = GetEmployeeByCode(filter.EmployeeCode);
                if (employee == null && employee.EmployeeId == 0)
                    return;
                ManualOvertimeConfiguration ManualOTConfig = GetManualOvertimeConfiguration(employee.DesignationId ?? 0, employee.EmployeeId, (null != filterList && filterList.Count() > 0 ? filterList.First().AttendanceDate : ""));

                filter.EmployeeId = (int)employee.EmployeeId;

                //Get Holiday Declaration form [AttHolidayDeclaration]
                if (employee.IsOverTime.Value && employee.IsOvertimeException)
                {
                    overtimeExceptions = GetOvertimeExceptions(filter);
                    holidayDeclarations = GetHolidayDeclaration(filter);
                }

                if (employee.DesignationId.ToString() == GCDesignationConstants.Driver)
                {
                    //Get employeetime keeping exceptions
                    exceptionalTimeKeepingList = GetEmployeeTimeKeepingExceptions(filter);
                }

                foreach (var item in filterList)
                {
                    if (employee.IsOverTime.Value && employee.IsOvertimeException)
                    {
                        var overtime = item.OverTime;

                        //Get Overtime Exception
                        overtime = GetOvertimeException(holidayDeclarations, overtimeExceptions, item, overtime);

                        try
                        {
                            if (string.IsNullOrWhiteSpace(overtime))
                                overtime = "00:00:00";

                            item.OverTime = overtime;

                            //let's fragmented item
                            var fragmentedWorkingHour = item.WorkingHour.Split(':');

                            if (overtime.Split(':').Length < 3)
                            {
                                overtime = "00:00:00";
                                item.OverTime = overtime;
                            }

                            if (fragmentedWorkingHour.Length < 3)
                                item.WorkingHour = "00:00:00";

                            var fragmentedOvertime = overtime.Split(':');

                            //Populate Overtimes
                            PopulateOvertimes(filter, ref totalHours, ref totalMinutes, ref totalSeconds, item, fragmentedOvertime);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        try
                        {
                            var overtime = item.OverTime;
                            if (null != ManualOTConfig && ManualOTConfig.ManualOvertimeOnly
                                && !exceptionalTimeKeepingList.Any(f => f.EventDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) == Convert.ToDateTime(item.AttendanceDate).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)))
                            {
                                overtime = "00:00:00";
                                item.OverTime = overtime;
                            }
                            if (string.IsNullOrWhiteSpace(item.OverTime))
                                continue;

                            //let's fragmented item
                            var fragmentedWorkingHour = item.WorkingHour.Split(':');

                            if (overtime.Split(':').Length < 3)
                            {
                                overtime = "00:00:00";
                                item.OverTime = overtime;
                            }

                            if (fragmentedWorkingHour.Length < 3)
                                item.WorkingHour = "00:00:00";

                            var fragmentedOvertime = overtime.Split(':');
                            double OTHours = 0, OTMinutes = 0, OTSeconds = 0;
                            GetOTTime(fragmentedOvertime, item, ManualOTConfig, out OTHours, out OTMinutes, out OTSeconds);

                            totalHours += Convert.ToDouble(OTHours);
                            totalMinutes += Convert.ToDouble(OTMinutes);
                            totalSeconds += Convert.ToDouble(OTSeconds);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                    /*else if (employee.DesignationId.ToString() == GCDesignationConstants.Driver)
                    {
                        try
                        {
                            var overtime = item.OverTime;
                            if (!exceptionalTimeKeepingList.Any(f => f.EventDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture) == Convert.ToDateTime(item.AttendanceDate).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)))
                            {
                                overtime = "00:00:00";
                                item.OverTime = overtime;
                            }

                            if (string.IsNullOrWhiteSpace(item.OverTime)) 
                                continue;                            

                            //let's fragmented item
                            var fragmentedWorkingHour = item.WorkingHour.Split(':');

                            if (overtime.Split(':').Length < 3)
                            {
                                overtime = "00:00:00";
                                item.OverTime = overtime;
                            }

                            if (fragmentedWorkingHour.Length < 3)
                                item.WorkingHour = "00:00:00";

                            var fragmentedOvertime = overtime.Split(':');

                            totalHours += Convert.ToDouble(fragmentedOvertime[0]);
                            totalMinutes += Convert.ToDouble(fragmentedOvertime[1]);
                            totalSeconds += Convert.ToDouble(fragmentedOvertime[2]);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        try
                        {
                            if (string.IsNullOrWhiteSpace(item.OverTime))
                                continue;

                            var overtime = item.OverTime;

                            //let's fragmented item
                            var fragmentedWorkingHour = item.WorkingHour.Split(':');

                            if (overtime.Split(':').Length < 3)
                            {
                                overtime = "00:00:00";
                                item.OverTime = overtime;
                            }

                            if (fragmentedWorkingHour.Length < 3)
                                item.WorkingHour = "00:00:00";

                            var fragmentedOvertime = overtime.Split(':');
                            double OTHours = 0, OTMinutes = 0, OTSeconds = 0;
                            GetOTTime(fragmentedOvertime, item, out OTHours, out OTMinutes, out OTSeconds);

                            totalHours += Convert.ToDouble(OTHours);
                            totalMinutes += Convert.ToDouble(OTMinutes);
                            totalSeconds += Convert.ToDouble(OTSeconds);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }*/
                }

                totalOvertime = totalHours + totalMinutes / 60 + (totalSeconds / (60 * 60));
                double TotalOvertimeActualAmount = totalOvertime;
                if (null != ManualOTConfig && totalOvertime > ManualOTConfig.MonthlyMax) totalOvertime = ManualOTConfig.MonthlyMax;

                var overTimeHourSUMInText = $"{totalHours} Hours {totalMinutes} Minutes {totalSeconds} Seconds";

                filterList.ForEach(f => f.OverTimeHourSUM = String.Format("{0:0.##}", totalOvertime));
                filterList.ForEach(f => f.TotalOvertimeActualAmount = String.Format("{0:0.##}", TotalOvertimeActualAmount));
            }
        }

        private List<EmployeeTimeKeepingException> GetEmployeeTimeKeepingExceptions(TimeKeepingReportSearchFilter filter)
        {
            var exceptionalTimeKeepingList = new List<EmployeeTimeKeepingException>();
            try
            {
                var sqlCommand = $@"SELECT *
                                FROM att.EmployeeTimeKeepingException tke
                                WHERE (tke.EventDate BETWEEN '{Convert.ToDateTime(filter.StartDate).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}' AND '{Convert.ToDateTime(filter.EndDate).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}') 
                                AND tke.EmployeeId = {filter.EmployeeId}
                                AND tke.IsActive = 1";
           
                using (var db = new gHRMDBContext())
                {
                    exceptionalTimeKeepingList = db.Database.SqlQuery<EmployeeTimeKeepingException>(sqlCommand).AsParallel().ToList();
                }
            }
            catch (Exception ex)
            {

            }

            return exceptionalTimeKeepingList;
        }

        private void PopulateTotalOvertime(List<TimeKeepingReportModel> filterList, TimeKeepingReportSearchFilter filter)
        {
            if (filterList.Any())
            {
                double totalOvertime = 0, totalHours = 0, totalMinutes = 0, totalSeconds = 0;

                var employee = GetEmployeeByCode(filter.EmployeeCode);
                if (employee == null && employee.EmployeeId == 0)
                    return;

                filter.EmployeeId = (int)employee.EmployeeId;

                foreach (var item in filterList)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(item.OverTime))
                            continue;

                        var overtime = item.OverTime;

                        //let's fragmented item
                        var fragmentedWorkingHour = item.WorkingHour.Split(':');

                        if (overtime.Split(':').Length < 3)
                        {
                            overtime = "00:00:00";
                            item.OverTime = overtime;
                        }

                        if (fragmentedWorkingHour.Length < 3)
                            item.WorkingHour = "00:00:00";

                        var fragmentedOvertime = overtime.Split(':');

                        totalHours += Convert.ToDouble(fragmentedOvertime[0]);
                        totalMinutes += Convert.ToDouble(fragmentedOvertime[1]);
                        totalSeconds += Convert.ToDouble(fragmentedOvertime[2]);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }

                totalOvertime = totalHours + totalMinutes / 60 + (totalSeconds / (60 * 60));

                var overTimeHourSUMInText = $"{totalHours} Hours {totalMinutes} Minutes {totalSeconds} Seconds";

                filterList.ForEach(f => f.OverTimeHourSUM = String.Format("{0:0.##}", totalOvertime));
            }
        }

        private void PopulateOvertimes(TimeKeepingReportSearchFilter filter, ref double totalHours, ref double totalMinutes, ref double totalSeconds, TimeKeepingReportModel item, string[] fragmentedOvertime)
        {
            if (filter.GHRMPlusCompany == GHRMPlusCompanyConstants.GrameenCommunications)
            {
                var totalHour = Convert.ToDouble(fragmentedOvertime[0]);
                if (totalHour < 8)
                {
                    totalHours += Convert.ToDouble(fragmentedOvertime[0]);
                    totalMinutes += Convert.ToDouble(fragmentedOvertime[1]);
                    totalSeconds += Convert.ToDouble(fragmentedOvertime[2]);
                }
                else
                {
                    totalHours += 8;
                    totalMinutes += 0;
                    totalSeconds += 0;

                    item.OverTime = $"08:00:00";
                }
            }
            else
            {
                totalHours += Convert.ToDouble(fragmentedOvertime[0]);
                totalMinutes += Convert.ToDouble(fragmentedOvertime[1]);
                totalSeconds += Convert.ToDouble(fragmentedOvertime[2]);
            }
        }


        private string GetOvertimeException(List<AttHolidayDeclaration> holidayDeclarations, List<OvertimeException> overtimeExceptions, TimeKeepingReportModel item, string overTime)
        {
            if (!holidayDeclarations.Any())
            {
                overTime = "00:00:00";
                return overTime;
            }

            //get OvertimeException
            var overtimeException = GetOvertimeException(overtimeExceptions, item);
            if (overtimeException == null)
            {
                overTime = "00:00:00";
                return overTime;
            }

            var overtimeHolidayDeclaration = GetOvertimeHolidayDeclaration(holidayDeclarations, overtimeException);

            //TODO: for testing
            //if (item.AttendanceDate == "Jul  5 2020 12:00AM")
            //{
            //    var attendanceDate = Convert.ToDateTime(item.AttendanceDate);
            //}

            if (!overtimeHolidayDeclaration.Any(f => f.HolidayDate == Convert.ToDateTime(item.AttendanceDate)))
            {
                overTime = "00:00:00";
                return overTime;
            }

            return overTime;
        }


        #endregion

        public DataSet GetDataWithParameterIncrement<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new IncrementDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        #region Private Methods

        private List<OvertimeException> GetOvertimeExceptions(TimeKeepingReportSearchFilter filter)
        {
            var listins = new List<OvertimeException>();
            var currentdate = DateTime.Now;
            using (var db = new gHRMDBContext())
            {
                listins = db.OvertimeExceptions
                    .Where(f => f.IsActive && f.EmployeeId == filter.EmployeeId)
                    .AsParallel().ToList();
            }

            return listins;
        }

        private OvertimeException GetOvertimeException(List<OvertimeException> overtimeExceptions, TimeKeepingReportModel timeKeepingReport)
        {
            var overtimeException = new OvertimeException();
            overtimeException = overtimeExceptions
                .FirstOrDefault(f =>
                                f.IsActive
                            && f.EmployeeId == timeKeepingReport.EmployeeId
                            && (
                                    f.EffectiveStartDate <= Convert.ToDateTime(timeKeepingReport.AttendanceDate)
                                    || f.EffectiveEndDate >= Convert.ToDateTime(timeKeepingReport.AttendanceDate)
                                )
                            );

            return overtimeException;
        }

        private List<AttHolidayDeclaration> GetOvertimeHolidayDeclaration(List<AttHolidayDeclaration> attHolidayDeclarations, OvertimeException overtimeException)
        {
            var listing = new List<AttHolidayDeclaration>();

            if (overtimeException.ExceptionType == OvertimeExceptionTypeConstants.Weekend)
                listing = attHolidayDeclarations
                            .Where(f => f.AttHolidayTypeId == Convert.ToInt32(OvertimeExceptionTypeConstants.Weekend)
                       ).AsParallel().ToList();

            if (overtimeException.ExceptionType == OvertimeExceptionTypeConstants.PublicHoliday)
                listing = attHolidayDeclarations
                            .Where(f => f.AttHolidayTypeId != Convert.ToInt32(OvertimeExceptionTypeConstants.Weekend)
                       ).AsParallel().ToList();

            else if (overtimeException.ExceptionType == OvertimeExceptionTypeConstants.WeekendAndPublicHoliday)
                listing = attHolidayDeclarations;

            return listing;
        }

        private List<AttHolidayDeclaration> GetHolidayDeclaration(TimeKeepingReportSearchFilter filter)
        {
            var listing = new List<AttHolidayDeclaration>();
            var holidayYearStart = filter.StartDate.Value.Year;
            var holidayYearEnd = filter.EndDate.Value.Year;
            using (var db = new gHRMDBContext())
            {
                listing = db.AttHolidayDeclarations
                    .Where(f => f.IsActive == true
                       && (f.HolidayYear >= holidayYearStart && f.HolidayYear <= holidayYearEnd)
                ).AsParallel().ToList();
            }

            return listing;
        }

        private Employee GetEmployeeByCode(string employeeCode)
        {
            var employee = new Employee();
            var currentdate = DateTime.Now;
            using (var db = new gHRMDBContext())
            {
                employee = db.Employees
                    .FirstOrDefault(f => f.IsActive && f.EmployeeCode == employeeCode);
            }

            return employee;
        }

        private void GetOTTime(string[] FragmentedOvertime, TimeKeepingReportModel Item, ManualOvertimeConfiguration ManualOTConfig, out double OTHours, out double OTMinutes, out double OTSeconds)
        {
            OTHours = Convert.ToDouble(FragmentedOvertime[0]);
            OTMinutes = Convert.ToDouble(FragmentedOvertime[1]);
            OTSeconds = Convert.ToDouble(FragmentedOvertime[2]);
            if (null == ManualOTConfig) return;
            DateTime AttendanceDate = Convert.ToDateTime(Item.AttendanceDate).Date;
            bool IsHoliday = false;

            using (var db = new gHRMDBContext())
            {
                IsHoliday = db.AttHolidayDeclarations.Where(x => null != x.IsActive && x.IsActive.Value && x.HolidayDate == AttendanceDate).Count() > 0;
            }
            if (IsHoliday)
            {
                if (OTHours >= ManualOTConfig.HolidayMax)
                {
                    OTHours = ManualOTConfig.HolidayMax;
                    OTMinutes = 0;
                    OTSeconds = 0;
                    Item.OverTime = ManualOTConfig.HolidayMax.ToString().PadLeft(2, '0') + ":00:00";
                }
            }
            else
            {
                if (OTHours >= ManualOTConfig.WorkingDayMax)
                {
                    OTHours = ManualOTConfig.WorkingDayMax;
                    OTMinutes = 0;
                    OTSeconds = 0;
                    Item.OverTime = ManualOTConfig.WorkingDayMax.ToString().PadLeft(2, '0') + ":00:00";
                }
            }
        }

        private ManualOvertimeConfiguration GetManualOvertimeConfiguration(int DesignationId, long EmployeeId, string AttendanceDateStr)
        {
            using (var db = new gHRMDBContext())
            {
                DateTime AttendanceDate = Convert.ToDateTime(AttendanceDateStr).Date;
                return db.ManualOvertimeConfigurations
                    .Where(x => x.IsActive
                        && (x.EmployeeId == EmployeeId || x.EmployeeDesignationId == DesignationId)
                        && x.EffectiveStartDate <= AttendanceDate && (x.EffectiveEndDate >= AttendanceDate || x.EffectiveEndDate == null)
                    ).FirstOrDefault();
            }
        }
        #endregion

    }
}
