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
    public interface IVMServiceProviderConfigurationService : IServiceBase<VMServiceProviderConfiguration>
    {


    }
    public class VMServiceProviderConfigurationService : IVMServiceProviderConfigurationService
    {
        private readonly IVMServiceProviderConfigurationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public VMServiceProviderConfigurationService(IVMServiceProviderConfigurationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<VMServiceProviderConfiguration> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == 1).OrderBy(c => c.VMServiceProviderConfigurationId);
            return entities;
        }

        public VMServiceProviderConfiguration GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public VMServiceProviderConfiguration Create(VMServiceProviderConfiguration objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(VMServiceProviderConfiguration objectToUpdate)
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

        public VMServiceProviderConfiguration Get(Expression<Func<VMServiceProviderConfiguration, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<VMServiceProviderConfiguration> GetMany(Expression<Func<VMServiceProviderConfiguration, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == 1);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<VMServiceProviderConfiguration>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<VMServiceProviderConfiguration>> GetManyAsync(Expression<Func<VMServiceProviderConfiguration, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<VMServiceProviderConfiguration> GetAsync(Expression<Func<VMServiceProviderConfiguration, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
