using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Linq;


namespace gHRM.Data.Repository
{
    public interface ILeaveHistoryRepository : IRepository<LeaveHistory>
    {
        IEnumerable<DBLeaveHistoryModel> GetLeaveHistoryByEmployee(long employeeId, int startRowIndex, string jtSorting, int pageSize, out double TotCount);
        IEnumerable<DBLeaveModel> GetLeaveByEmployee(long employeeId, int startRowIndex, string jtSorting, int pageSize, out long TotCount);

        IEnumerable<DBLeaveModel> GetLeaveByEmployee2(long employeeId, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
        List<LeaveHistory> AddCLOpeningList(List<LeaveHistory> objs);
        List<LeaveHistory> AddLeaveHistory(List<LeaveHistory> objs);
    }

    public class LeaveHistoryRepository : RepositoryBaseCodeFirst<LeaveHistory>, ILeaveHistoryRepository
    {
        public LeaveHistoryRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }

        public IEnumerable<DBLeaveHistoryModel> GetLeaveHistoryByEmployee(long employeeId, int startRowIndex, string jtSorting, int pageSize, out double TotCount)
        {
            IQueryable<LeaveHistory> results = null;
            if (employeeId > 0)
            {
                if (employeeId > 0)
                    results = DataContext.LeaveHistories.Where(w => w.IsActive == true && w.EmployeeId == employeeId && w.IsApproved == true && (w.IsAdjustment == true || w.LeaveReason.Trim() == "OPENING") && w.LeaveStartDate.Year == DateTime.Now.Year);
                else
                    results = DataContext.LeaveHistories.Where(w => w.IsActive == true);
            }

            TotCount = results.LongCount();

            var obj = results.OrderBy(o => o.LeaveId).Skip(startRowIndex).Take(pageSize).Select(s => new DBLeaveHistoryModel()
            {
                EmployeeId = s.EmployeeId,
                LeaveId = s.LeaveId,
                LeaveStartDate = s.LeaveStartDate,
                LeaveEndDate = s.LeaveEndDate,
                LeaveTypeName = s.LeaveType.LeaveTypeName,
                TotalDays = s.TotalDays,
                TotalAvailableDays =s.LeaveType.MaxLeaveDays - s.TotalDays,
                LeaveReason = s.LeaveReason,
                AddressDuringLeave = s.AddressDuringLeave,
                LeaveDayDuration = s.LeaveDayDuration
            });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                return obj.OrderByDescending(o => o.LeaveId);
                //if (jtSorting == "LeaveStartDate ASC")
                //    return obj.OrderBy(o => o.LeaveStartDate);
                //else if (jtSorting == "LeaveStartDate DESC")
                //    return obj.OrderByDescending(o => o.LeaveStartDate);
                //else if (jtSorting == "LeaveEndDate ASC")
                //    return obj.OrderBy(o => o.LeaveEndDate);
                //else if (jtSorting == "LeaveEndDate DESC")
                //    return obj.OrderByDescending(o => o.LeaveEndDate);
                //else if (jtSorting == "LeaveTypeName ASC")
                //    return obj.OrderBy(o => o.LeaveTypeName);
                //else if (jtSorting == "LeaveTypeName DESC")
                //    return obj.OrderByDescending(o => o.LeaveTypeName);
                //else if (jtSorting == "TotalDays ASC")
                //    return obj.OrderBy(o => o.TotalDays);
                //else if (jtSorting == "TotalDays DESC")
                //    return obj.OrderByDescending(o => o.TotalDays);
                //if (jtSorting == "TotalAvailableDays ASC")
                //    return obj.OrderBy(o => o.TotalAvailableDays);
                //else if (jtSorting == "TotalAvailableDays DESC")
                //    return obj.OrderByDescending(o => o.TotalAvailableDays);
                //else if (jtSorting == "LeaveReason ASC")
                //    return obj.OrderBy(o => o.LeaveReason);
                //else if (jtSorting == "LeaveReason DESC")
                //    return obj.OrderByDescending(o => o.LeaveReason);               
                //else
                //    return obj.OrderBy(o => o.LeaveId);
            }
            else
                return obj.OrderBy(o => o.LeaveId);

        }

        public IEnumerable<DBLeaveModel> GetLeaveByEmployee(long employeeId, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            IQueryable<LeaveHistory> results = null;
            if (employeeId > 0)
            {
                results = DataContext.LeaveHistories.Where(w => w.IsActive == true && w.EmployeeId == employeeId && w.IsApproved == false);
            }
            else
                results = DataContext.LeaveHistories.Where(w => w.IsActive == true);

            TotCount = results.LongCount();

            var obj = results.OrderBy(o => o.LeaveId).Skip(startRowIndex).Take(pageSize).Select(s => new DBLeaveModel()
            {
                EmployeeId = s.EmployeeId,
                LeaveId = s.LeaveId,
                LeaveStartDate = s.LeaveStartDate,
                LeaveEndDate = s.LeaveEndDate,
                LeaveTypeName = s.LeaveType.LeaveTypeName,
                TotalDays = s.TotalDays,
                LeaveReason = s.LeaveReason,
                AddressDuringLeave = s.AddressDuringLeave,
                CreateDate = s.CreateDate,
                LeaveDayDuration = s.LeaveDayDuration
            });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                //if (jtSorting == "LeaveStartDate ASC")
                return obj.OrderBy(o => o.CreateDate);
                //else if (jtSorting == "LeaveStartDate DESC")
                //    return obj.OrderByDescending(o => o.LeaveStartDate);
                //else if (jtSorting == "LeaveEndDate ASC")
                //    return obj.OrderBy(o => o.LeaveEndDate);
                //else if (jtSorting == "LeaveEndDate DESC")
                //    return obj.OrderByDescending(o => o.LeaveEndDate);
                //else if (jtSorting == "LeaveTypeName ASC")
                //    return obj.OrderBy(o => o.LeaveTypeName);
                //else if (jtSorting == "LeaveTypeName DESC")
                //    return obj.OrderByDescending(o => o.LeaveTypeName);
                //else if (jtSorting == "TotalDays ASC")
                //    return obj.OrderBy(o => o.TotalDays);
                //else if (jtSorting == "TotalDays DESC")
                //    return obj.OrderByDescending(o => o.TotalDays);
                //if (jtSorting == "AddressDuringLeave ASC")
                //    return obj.OrderBy(o => o.AddressDuringLeave);
                //else if (jtSorting == "AddressDuringLeave DESC")
                //    return obj.OrderByDescending(o => o.AddressDuringLeave);
                //else if (jtSorting == "LeaveReason ASC")
                //    return obj.OrderBy(o => o.LeaveReason);
                //else if (jtSorting == "LeaveReason DESC")
                //    return obj.OrderByDescending(o => o.LeaveReason);
                //else
                //    return obj.OrderBy(o => o.LeaveId);

                //return obj.OrderBy(o => o.LeaveStartDate);
            }
            else
                return obj.OrderBy(o => o.LeaveId);

        }

        public IEnumerable<DBLeaveModel> GetLeaveByEmployee2(long employeeId, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            IQueryable<LeaveHistory> results = null;
            if (employeeId > 0)
            {
                results = DataContext.LeaveHistories.Where(w => w.IsActive == true && w.EmployeeId == employeeId).OrderByDescending(w => w.LeaveId)
                        .Take(20);


            }
            else
                results = DataContext.LeaveHistories.Where(w => w.IsActive == true);

            TotCount = results.LongCount();

            var obj = results.OrderByDescending(o => o.LeaveId).Skip(startRowIndex).Take(pageSize).Select(s => new DBLeaveModel()
            {
                EmployeeId = s.EmployeeId,
                LeaveId = s.LeaveId,
                LeaveStartDate = s.LeaveStartDate,
                LeaveEndDate = s.LeaveEndDate,
                LeaveTypeName = s.LeaveType.LeaveTypeName,
                TotalDays = s.TotalDays,
                LeaveReason = s.LeaveReason,
                AddressDuringLeave = s.AddressDuringLeave,
                CreateDate = s.CreateDate,
                LeaveDayDuration = s.LeaveDayDuration
            });

            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                //if (jtSorting == "LeaveStartDate ASC")
                return obj.OrderByDescending(o => o.CreateDate);
                //else if (jtSorting == "LeaveStartDate DESC")
                //    return obj.OrderByDescending(o => o.LeaveStartDate);
                //else if (jtSorting == "LeaveEndDate ASC")
                //    return obj.OrderBy(o => o.LeaveEndDate);
                //else if (jtSorting == "LeaveEndDate DESC")
                //    return obj.OrderByDescending(o => o.LeaveEndDate);
                //else if (jtSorting == "LeaveTypeName ASC")
                //    return obj.OrderBy(o => o.LeaveTypeName);
                //else if (jtSorting == "LeaveTypeName DESC")
                //    return obj.OrderByDescending(o => o.LeaveTypeName);
                //else if (jtSorting == "TotalDays ASC")
                //    return obj.OrderBy(o => o.TotalDays);
                //else if (jtSorting == "TotalDays DESC")
                //    return obj.OrderByDescending(o => o.TotalDays);
                //if (jtSorting == "AddressDuringLeave ASC")
                //    return obj.OrderBy(o => o.AddressDuringLeave);
                //else if (jtSorting == "AddressDuringLeave DESC")
                //    return obj.OrderByDescending(o => o.AddressDuringLeave);
                //else if (jtSorting == "LeaveReason ASC")
                //    return obj.OrderBy(o => o.LeaveReason);
                //else if (jtSorting == "LeaveReason DESC")
                //    return obj.OrderByDescending(o => o.LeaveReason);
                //else
                //    return obj.OrderBy(o => o.LeaveId);

                //return obj.OrderBy(o => o.LeaveStartDate);
            }
            else
                return obj.OrderBy(o => o.LeaveId);

        }

        public List<LeaveHistory> AddCLOpeningList(List<LeaveHistory> objs)
        {
            DataContext.LeaveHistories.AddRange(objs);
            DataContext.SaveChanges();
            return objs;
        }

        public List<LeaveHistory> AddLeaveHistory(List<LeaveHistory> objs)
        {
            DataContext.LeaveHistories.AddRange(objs);
            DataContext.SaveChanges();
            return objs;
        }
    }
}