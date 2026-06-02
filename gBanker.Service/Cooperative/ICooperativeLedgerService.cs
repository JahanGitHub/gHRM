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
    public interface ICooperativeLedgerService : IServiceBase<CooperativeLedger>
    {

    }
    public class CooperativeLedgerService : ICooperativeLedgerService
    {
        private readonly ICooperativeLedgerRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public CooperativeLedgerService(ICooperativeLedgerRepository repository,
            IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<CooperativeLedger> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }

        public CooperativeLedger GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public CooperativeLedger Create(CooperativeLedger objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(CooperativeLedger objectToUpdate)
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

        public CooperativeLedger Get(Expression<Func<CooperativeLedger, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<CooperativeLedger> GetMany(Expression<Func<CooperativeLedger, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<CooperativeLedger>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<CooperativeLedger>> GetManyAsync(Expression<Func<CooperativeLedger, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<CooperativeLedger> GetAsync(Expression<Func<CooperativeLedger, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion


    }
}
