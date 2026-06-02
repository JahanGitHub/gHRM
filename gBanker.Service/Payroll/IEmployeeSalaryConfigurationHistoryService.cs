using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IEmployeeSalaryConfigurationHistoryService : IServiceBase<EmployeeSalaryConfigurationHistory>
    {

        // na
    }
    public class EmployeeSalaryConfigurationHistoryService : IEmployeeSalaryConfigurationHistoryService
    {
        private readonly IEmployeeSalaryConfigurationHistoryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeSalaryConfigurationHistoryService(IEmployeeSalaryConfigurationHistoryRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeSalaryConfigurationHistory> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public EmployeeSalaryConfigurationHistory GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeSalaryConfigurationHistory Create(EmployeeSalaryConfigurationHistory objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeSalaryConfigurationHistory objectToUpdate)
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

        public EmployeeSalaryConfigurationHistory Get(Expression<Func<EmployeeSalaryConfigurationHistory, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeSalaryConfigurationHistory> GetMany(Expression<Func<EmployeeSalaryConfigurationHistory, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeSalaryConfigurationHistory>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeSalaryConfigurationHistory>> GetManyAsync(Expression<Func<EmployeeSalaryConfigurationHistory, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeSalaryConfigurationHistory> GetAsync(Expression<Func<EmployeeSalaryConfigurationHistory, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
