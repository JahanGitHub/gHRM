using gHRM.Core.Filters;
using gHRM.Core.Filters.Payroll;
using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IEmployeeSalaryDeductionService : IServiceBase<EmployeeSalaryDeduction>
    {
        List<EmployeeSalaryDeduction> AddEmplyoeeSalaryDeductionList(List<EmployeeSalaryDeduction> objs);
        bool CheckEmployeeSalaryDeduction(long employeeId, DateTime startDate, DateTime endDate
           , int prComponentId, int? productId, int? serialId);

        bool CheckEmployeeSalaryDeductionByComponentId(int prComponentId);
        Task<BaseResponse> IsValidDeductionByEffectiveDates(BaseSearchFilter filter);
        IEnumerable<EmployeeSalaryDeduction> GetEmployeeSalaryDeductionsByFilter(EmployeeSalaryDeductionSearchFilter filter);
    }
    public class EmployeeSalaryDeductionService : IEmployeeSalaryDeductionService
    {
        private readonly IEmployeeSalaryDeductionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeSalaryDeductionService(IEmployeeSalaryDeductionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeSalaryDeduction> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public async Task<BaseResponse> IsValidDeductionByEffectiveDates(BaseSearchFilter filter)
        {
            var response = await repository.IsValidDeductionByEffectiveDates(filter);
            return response;
        }

        public IEnumerable<EmployeeSalaryDeduction> GetEmployeeSalaryDeductionsByFilter(EmployeeSalaryDeductionSearchFilter filter)
        {
            var listings = new List<EmployeeSalaryDeduction>();

            using (var db = new gHRMDBContext())
            {
                listings = db.EmployeeSalaryDeduction.Where(p => p.ComponentId == filter.PrComponentId
                                                                && p.StartDate == filter.StartDate
                                                                && p.EndDate == filter.EndDate
                                                                && p.IsActive
                                                                && p.EmployeeId == filter.EmployeeId
                                                            ).ToList();
            }

            return listings;
        }

        public EmployeeSalaryDeduction GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public bool CheckEmployeeSalaryDeductionByComponentId(int prComponentId)
        {
            bool isFound = true;

            using (var db = new gHRMDBContext())
            {
                isFound = db.EmployeeSalaryDeduction
                                                .Any(f => f.IsActive
                                                        && f.ComponentId == prComponentId);
            }

            return isFound;
        }

        public bool CheckEmployeeSalaryDeduction(long employeeId, DateTime startDate, DateTime endDate
            , int prComponentId, int? productId, int? serialId)
        {
            var checkEmployeeSalaryDeduction = true;
            using (var db = new gHRMDBContext())
            {
                checkEmployeeSalaryDeduction = db.EmployeeSalaryDeduction
                                .Any(p => DbFunctions.TruncateTime(p.StartDate) >= DbFunctions.TruncateTime(startDate)
                                    && DbFunctions.TruncateTime(p.EndDate) <= DbFunctions.TruncateTime(endDate)
                                    && p.IsActive == true
                                    && p.EmployeeId == employeeId
                                    && p.ComponentId == prComponentId && p.ProductId == productId
                                    && p.SerialId == serialId);
            }

            return checkEmployeeSalaryDeduction;
        }

        public EmployeeSalaryDeduction Create(EmployeeSalaryDeduction objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeSalaryDeduction objectToUpdate)
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

        public List<EmployeeSalaryDeduction> AddEmplyoeeSalaryDeductionList(List<EmployeeSalaryDeduction> objs)
        {
            repository.AddEmplyoeeSalaryDeductionList(objs);
            return objs;
        }

        public EmployeeSalaryDeduction Get(Expression<Func<EmployeeSalaryDeduction, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeSalaryDeduction> GetMany(Expression<Func<EmployeeSalaryDeduction, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeSalaryDeduction>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeSalaryDeduction>> GetManyAsync(Expression<Func<EmployeeSalaryDeduction, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeSalaryDeduction> GetAsync(Expression<Func<EmployeeSalaryDeduction, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
