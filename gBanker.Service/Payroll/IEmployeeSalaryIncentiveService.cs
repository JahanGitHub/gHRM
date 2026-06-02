using gHRM.Core.Filters;
using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IEmployeeSalaryIncentiveService : IServiceBase<EmployeeSalaryIncentive>
    {
        List<EmployeeSalaryIncentive> AddTADA(List<EmployeeSalaryIncentive> objs);
        Task<BaseResponse> IsValidIncentiveByEffectiveDates(BaseSearchFilter filter);

        bool CheckEmployeeSalaryIncentive(long employeeId, DateTime startDate, DateTime endDate
           , int prComponentId, int? productId, int? serialId);

        bool CheckEmployeeSalaryIncentiveByComponentId(int prComponentId);

        EmployeeSalaryIncentive GetIncentiveByComponentAndEmployeeId(int prComponentId, int employeeId);

        bool CheckAllowanceExist(long employeeId, int prComponentId, string componentType,DateTime startDate, DateTime endDate);
        bool CheckAllowanceExist(string employeecode, string ComponentName, string componentType, DateTime startDate, DateTime endDate);
    }
    public class EmployeeSalaryIncentiveService : IEmployeeSalaryIncentiveService
    {
        private readonly IEmployeeSalaryIncentiveRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;        

        public EmployeeSalaryIncentiveService(IEmployeeSalaryIncentiveRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public List<EmployeeSalaryIncentive> AddTADA(List<EmployeeSalaryIncentive> objs)
        {
            repository.AddTADA(objs);
            return objs;
        }
        public async Task<BaseResponse> IsValidIncentiveByEffectiveDates(BaseSearchFilter filter)
        {
            return await repository.IsValidIncentiveByEffectiveDates(filter);
        }

        public IEnumerable<EmployeeSalaryIncentive> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.SalaryIncentiveId);
            return entities;
        }

        public EmployeeSalaryIncentive GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeSalaryIncentive GetIncentiveByComponentAndEmployeeId(int prComponentId, int employeeId)
        {
            var single = new EmployeeSalaryIncentive();

            using (var db = new gHRMDBContext())
            {
                single = db.EmployeeSalaryIncentive.FirstOrDefault(f=>f.IsActive 
                                                                    && f.PRComponentId== prComponentId 
                                                                            && f.EmployeeId== employeeId);
            }

            return single;
        }

        public bool CheckEmployeeSalaryIncentiveByComponentId(int prComponentId)
        {
            bool isFound = true;

            using (var db = new gHRMDBContext())
            {
                isFound = db.EmployeeSalaryIncentive
                                                .Any(f => f.IsActive
                                                        && f.PRComponentId == prComponentId);
            }

            return isFound;
        }

        public bool CheckEmployeeSalaryIncentive(long employeeId, DateTime startDate, DateTime endDate
           , int prComponentId, int? productId, int? serialId)
        {
            var checkEmployeeSalaryIncentive = true;
            using (var db = new gHRMDBContext())
            {
                checkEmployeeSalaryIncentive = db.EmployeeSalaryIncentive
                                .Any(p => DbFunctions.TruncateTime(p.StartDate) >= DbFunctions.TruncateTime(startDate)
                                    && DbFunctions.TruncateTime(p.EndDate) <= DbFunctions.TruncateTime(endDate)
                                    && p.IsActive
                                    && p.EmployeeId == employeeId
                                    && p.PRComponentId == prComponentId
                                    && p.ProductId == productId
                                    && p.SerialId == serialId);
            }

            return checkEmployeeSalaryIncentive;
        }

        

        public bool CheckAllowanceExist(long employeeId, int prComponentId, string componentType ,DateTime startDate, DateTime endDate)
        {
            var checkEmployeeAllowance = true;
            using (var db = new gHRMDBContext())
            {
                var sqlCommand = $@"[prl].[EmployeeSalaryAllowance_CheckExisting] 
                                 {employeeId}, {prComponentId},'{componentType.Trim()}','{startDate.ToString("dd-MMM-yyyy",CultureInfo.InvariantCulture)}','{endDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}' ";

                checkEmployeeAllowance = db.Database.SqlQuery<bool>(sqlCommand).FirstOrDefault();
            }

            return checkEmployeeAllowance;
        }

        public bool CheckAllowanceExist(string employeecode, string ComponentName, string componentType, DateTime startDate, DateTime endDate)
        {
            var checkEmployeeAllowance = true;
            using (var db = new gHRMDBContext())
            {
                var sqlCommand = $@"[prl].[EmployeeSalaryAllowance_CheckExistingXComponentName] 
                                 '{employeecode}', '{ComponentName}','{componentType.Trim()}','{startDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}','{endDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture)}' ";

                checkEmployeeAllowance = db.Database.SqlQuery<bool>(sqlCommand).FirstOrDefault();
            }

            return checkEmployeeAllowance;
        }

        public EmployeeSalaryIncentive Create(EmployeeSalaryIncentive objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeSalaryIncentive objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public EmployeeSalaryIncentive Get(Expression<Func<EmployeeSalaryIncentive, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeSalaryIncentive> GetMany(Expression<Func<EmployeeSalaryIncentive, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeSalaryIncentive>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeSalaryIncentive>> GetManyAsync(Expression<Func<EmployeeSalaryIncentive, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeSalaryIncentive> GetAsync(Expression<Func<EmployeeSalaryIncentive, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
