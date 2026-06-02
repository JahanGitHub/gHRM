
using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IELEncashmentConfigurationService : IServiceBase<ELEncashmentConfiguration>
    {

    }
    public class ELEncashmentConfigurationService : IELEncashmentConfigurationService
    {
        private readonly IELEncashmentConfigurationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ELEncashmentConfigurationService(IELEncashmentConfigurationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ELEncashmentConfiguration> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.ConfigurationId);
            return entities;
        }

        public ELEncashmentConfiguration GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ELEncashmentConfiguration Create(ELEncashmentConfiguration objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ELEncashmentConfiguration objectToUpdate)
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

        public void Save()
        {
            unitOfWork.Commit();
        }


        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }


        public ELEncashmentConfiguration Get(Expression<Func<ELEncashmentConfiguration, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ELEncashmentConfiguration> GetMany(Expression<Func<ELEncashmentConfiguration, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<ELEncashmentConfiguration>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<ELEncashmentConfiguration>> GetManyAsync(Expression<Func<ELEncashmentConfiguration, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<ELEncashmentConfiguration> GetAsync(Expression<Func<ELEncashmentConfiguration, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
