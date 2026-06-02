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
    public interface IVMServiceProviderService : IServiceBase<VMServiceProvider>
    {


    }
    public class VMServiceProviderService : IVMServiceProviderService
    {
        private readonly IVMServiceProviderRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public VMServiceProviderService(IVMServiceProviderRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<VMServiceProvider> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == 1).OrderBy(c => c.VMServiceProviderId);
            return entities;
        }

        public VMServiceProvider GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public VMServiceProvider Create(VMServiceProvider objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(VMServiceProvider objectToUpdate)
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

        public VMServiceProvider Get(Expression<Func<VMServiceProvider, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<VMServiceProvider> GetMany(Expression<Func<VMServiceProvider, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == 1);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<VMServiceProvider>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<VMServiceProvider>> GetManyAsync(Expression<Func<VMServiceProvider, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<VMServiceProvider> GetAsync(Expression<Func<VMServiceProvider, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
