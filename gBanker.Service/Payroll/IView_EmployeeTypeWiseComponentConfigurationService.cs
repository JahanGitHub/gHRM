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
    public interface IView_EmployeeTypeWiseComponentConfigurationService : IServiceBase<View_EmployeeTypeWiseComponentConfiguration>
    {

    }

    public class View_EmployeeTypeWiseComponentConfigurationService : IView_EmployeeTypeWiseComponentConfigurationService
    {
        private readonly IView_EmployeeTypeWiseComponentConfigurationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public View_EmployeeTypeWiseComponentConfigurationService(IView_EmployeeTypeWiseComponentConfigurationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<View_EmployeeTypeWiseComponentConfiguration> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.PRComponentId);
            return entities;
        }

        public View_EmployeeTypeWiseComponentConfiguration GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public View_EmployeeTypeWiseComponentConfiguration Create(View_EmployeeTypeWiseComponentConfiguration objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(View_EmployeeTypeWiseComponentConfiguration objectToUpdate)
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

        public View_EmployeeTypeWiseComponentConfiguration Get(Expression<Func<View_EmployeeTypeWiseComponentConfiguration, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<View_EmployeeTypeWiseComponentConfiguration> GetMany(Expression<Func<View_EmployeeTypeWiseComponentConfiguration, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<View_EmployeeTypeWiseComponentConfiguration>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<View_EmployeeTypeWiseComponentConfiguration>> GetManyAsync(Expression<Func<View_EmployeeTypeWiseComponentConfiguration, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<View_EmployeeTypeWiseComponentConfiguration> GetAsync(Expression<Func<View_EmployeeTypeWiseComponentConfiguration, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
