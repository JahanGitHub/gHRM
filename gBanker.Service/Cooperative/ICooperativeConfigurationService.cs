using gHRM.Core.Utilities;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Cooperative;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using gHRM.Data.DBDetailModels.Cooperative;
using gHRM.Data.Repository.Cooperative;
using gHRM.Data.Repository.WelfareFund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace gHRM.Service.Cooperative
{
    public interface ICooperativeConfigurationService : IServiceBase<CooperativeConfiguration>
    {

    }
    public class CooperativeConfigurationService : ICooperativeConfigurationService
    {
        private readonly ICooperativeConfigurationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public CooperativeConfigurationService(ICooperativeConfigurationRepository repository,
            IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<CooperativeConfiguration> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Id);
            return entities;
        }

        public CooperativeConfiguration GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public CooperativeConfiguration Create(CooperativeConfiguration objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(CooperativeConfiguration objectToUpdate)
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

        public CooperativeConfiguration Get(Expression<Func<CooperativeConfiguration, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<CooperativeConfiguration> GetMany(Expression<Func<CooperativeConfiguration, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b =>b.ActivityStatus != CoOperativeConstants.ActivityStatus_Delete);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<CooperativeConfiguration>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<CooperativeConfiguration>> GetManyAsync(Expression<Func<CooperativeConfiguration, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<CooperativeConfiguration> GetAsync(Expression<Func<CooperativeConfiguration, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion


    }
}
