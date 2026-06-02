using gHRM.Core.Common;
using gHRM.Core.Filters.Leaves;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface ILeaveHistoryService : IServiceBase<LeaveHistory>
    {
        LeaveHistory GetLeaveHistoryById(Int64 leaveId);
        LeaveHistory GetByReplacementEmployeeId(Int64 EmployeeId);
        int GetTotLeaveEmpId(Int64 EmployeeId, int typId, string LeaveStatus);
        int? GetEarnLeaveTakenByEmpId(Int64 EmpId);
        DateTime? GetMaxEndDateByEmpId(Int64 EmpId);
        LeaveHistory GetNotAdjustLeave(string EmpCode, long employeeId);
        List<LeaveHistory> AddCLOpeningList(List<LeaveHistory> objs);
        List<LeaveHistory> AddLeaveHistory(List<LeaveHistory> objs);
        bool IsEmployeeInLeave(LeaveHistory leaveHistory);
        List<LeaveHistory> GetLeaveHistoriesByFilter(LeaveHistorySearchFilter filter);

        IEnumerable<DBLeaveHistoryModel> GetLeaveHistoryByEmployee(long employeeId, int startRowIndex, string jtSorting, int pageSize, out double TotCount);
        IEnumerable<DBLeaveModel> GetLeaveByEmployee(long employeeId, int startRowIndex, string jtSorting, int pageSize, out long TotCount);

        IEnumerable<DBLeaveModel> GetLeaveByEmployee2(long employeeId, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
    }

    public class LeaveHistoryService : ILeaveHistoryService
    {
        private readonly ILeaveHistoryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly IOfficeRepository OffcRepository;
        private readonly IEmployeeRepository EmployeeRepository;
        private readonly ILeaveTypeService leaveTypeService;
        private readonly IEmployeeService employeeService;
        public LeaveHistoryService(
                IEmployeeService employeeService,
                ILeaveTypeService leaveTypeService,
                ILeaveHistoryRepository repository,
                IUnitOfWorkCodeFirst unitOfWork,
                IOfficeRepository OffcRepository,
                IEmployeeRepository EmployeeRepository)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.OffcRepository = OffcRepository;

            this.EmployeeRepository = EmployeeRepository;
            this.employeeService = employeeService;
            this.leaveTypeService = leaveTypeService;
        }

        public IEnumerable<LeaveHistory> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.LeaveId);
            return entities;
        }
        public LeaveHistory GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public LeaveHistory GetLeaveHistoryById(Int64 leaveId)
        {
            var entity = repository.Get(e => e.LeaveId == leaveId);
            return entity;
        }
        public LeaveHistory GetNotAdjustLeave(string EmpCode, long employeeId)
        {
            //var emp = EmployeeRepository.GetAll().Where(w => w.EmployeeCode == EmpCode).FirstOrDefault();
            var entity = repository.GetMany(w => w.EmployeeId == employeeId && w.IsActive == true && w.IsApproved == true && w.IsAdjustment == false).FirstOrDefault();
            return entity;
        }

        public int? GetEarnLeaveTakenByEmpId(Int64 EmpId)
        {
            double totEarnLeave = 0;
            var leaveTypeId = 0;
            var empStatusId = employeeService.GetByEmpId(EmpId).EmployeeStatusId;
            var leaveType = leaveTypeService.GetMany(x => x.IsActive == true && x.LeaveCategory == "AL" && x.EmployeeStatusId == empStatusId).FirstOrDefault();
            if (leaveType != null)
            {
                leaveTypeId = leaveType.LeaveTypeId;
                var entity = repository.GetMany(w => w.IsActive == true && w.IsApproved == true && w.LeaveTypeId == leaveTypeId && w.EmployeeId == EmpId).Sum(s => s.TotalDays);
                totEarnLeave = (double)entity.Value;
            }

            return (int)totEarnLeave;
        }


        public DateTime? GetMaxEndDateByEmpId(Int64 EmpId)
        {
            DateTime? dt = null;
            var cnt = repository.GetMany(w => w.EmployeeId == EmpId && w.IsActive == true);
            if (cnt.Count() > 0)
            {
                var entity = repository.GetMany(w => w.IsActive == true
                                                 && w.EmployeeId == EmpId)
                    .Max(m => m.LeaveEndDate);
                if (entity != null)
                    dt = entity;
            }
            return dt;
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public List<LeaveHistory> GetLeaveHistoriesByFilter(LeaveHistorySearchFilter filter)
        {
            var listing = new List<LeaveHistory>();
            using (var db = new gHRMDBContext())
            {
                var sqlCommand = $@"
                        DECLARE 
	                        @StartDate date='{((DateTime)filter.StartDate).ToString("dd-MMM-yyyy",CultureInfo.InvariantCulture)}',
                            @EndDate date='{((DateTime)filter.EndDate).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}';
                        SELECT *FROM leave.LeaveHistory
                        WHERE 
	                            IsActive=1
	                        AND EmployeeId={filter.EmployeeId}
	                        AND (@StartDate between LeaveStartDate and LeaveEndDate OR @EndDate between LeaveStartDate and LeaveEndDate)
                        ";

                listing = db.Database.SqlQuery<LeaveHistory>(sqlCommand).AsParallel().ToList();
            }

            return listing;
        }

        public LeaveHistory Create(LeaveHistory objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LeaveHistory objectToUpdate)
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
                obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
                obj.IsActive = false;
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
                var isActive = obj.IsActive;
                if (isActive == false)
                {
                    return false;
                }
            }

            return true;
        }

        public int GetTotLeaveEmpId(Int64 EmployeeId, int typId, string LeaveStatus)
        {
            double Totdays = 0;
            if (LeaveStatus == "L")
            {
                var entity = repository.GetMany(w => w.IsActive == true && w.EmployeeId == EmployeeId && w.IsApproved == true && w.IsAdjustment == true && w.LeaveTypeId == typId && w.LeaveStartDate.Year == DateTime.Now.Year).Sum(m => m.TotalDays);
                Totdays = (double)entity.Value;
            }
            else
            {
                var entity = repository.GetMany(w => w.IsActive == true && w.EmployeeId == EmployeeId && w.IsApproved == true && w.IsAdjustment == true && w.LeaveTypeId == typId).Sum(m => m.TotalDays);
                Totdays = (double)entity.Value;
            }
            return (int)Totdays;
        }

        public IEnumerable<DBLeaveHistoryModel> GetLeaveHistoryByEmployee(long employeeId, int startRowIndex, string jtSorting, int pageSize, out double TotCount)
        {
            return repository.GetLeaveHistoryByEmployee(employeeId, startRowIndex, jtSorting, pageSize, out TotCount);
        }

        public IEnumerable<DBLeaveModel> GetLeaveByEmployee(long employeeId, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            return repository.GetLeaveByEmployee(employeeId, startRowIndex, jtSorting, pageSize, out TotCount);
        }

        public IEnumerable<DBLeaveModel> GetLeaveByEmployee2(long employeeId, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            return repository.GetLeaveByEmployee2(employeeId, startRowIndex, jtSorting, pageSize, out TotCount);
        }

        public LeaveHistory GetByReplacementEmployeeId(long EmployeeId)
        {
            var Result = repository.GetMany(b => b.ReplacementEmployee == EmployeeId && b.IsActive == true && b.IsAdjustment == false).FirstOrDefault();
            if (Result != null && (Result.LeaveEndDate < DateTime.Now.Date))
            {
                Result = null;
            }

            return Result;
        }

        public List<LeaveHistory> AddCLOpeningList(List<LeaveHistory> objs)
        {
            repository.AddCLOpeningList(objs);
            return objs;
        }

        public List<LeaveHistory> AddLeaveHistory(List<LeaveHistory> objs)
        {
            repository.AddLeaveHistory(objs);
            return objs;
        }

        public LeaveHistory Get(Expression<Func<LeaveHistory, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LeaveHistory> GetMany(Expression<Func<LeaveHistory, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public bool IsEmployeeInLeave(LeaveHistory leaveHistory)
        {
            bool isEmployeeInLeave =true;

            using (var db = new gHRMDBContext())
            {
                isEmployeeInLeave = db.LeaveHistories.Any(p => 
                                        p.EmployeeId == leaveHistory.ReplacementEmployee &&                                        
                                        (
                                            p.LeaveStartDate <= DbFunctions.TruncateTime(leaveHistory.LeaveStartDate) && 
                                            p.LeaveEndDate >= DbFunctions.TruncateTime(leaveHistory.LeaveStartDate)
                                        )
                                        &&
                                        (
                                            p.LeaveStartDate <= DbFunctions.TruncateTime(leaveHistory.LeaveStartDate) &&
                                            p.LeaveEndDate >= DbFunctions.TruncateTime(leaveHistory.LeaveEndDate)
                                        )
                                        &&
                                        p.IsActive);
            }

            return isEmployeeInLeave;
        }


        #region Asyc
        public virtual async Task<IEnumerable<LeaveHistory>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<LeaveHistory>> GetManyAsync(Expression<Func<LeaveHistory, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<LeaveHistory> GetAsync(Expression<Func<LeaveHistory, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
