using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Basic;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using gHRM.Data.Repository.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Basic
{
    public interface IBankAccountService : IServiceBase<BankAccount>
    {

    }
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public BankAccountService(IBankAccountRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<BankAccount> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.AccountId);
            return entities;
        }

        public BankAccount GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public BankAccount Create(BankAccount objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(BankAccount objectToUpdate)
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

        public BankAccount Get(Expression<Func<BankAccount, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<BankAccount> GetMany(Expression<Func<BankAccount, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<BankAccount>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<BankAccount>> GetManyAsync(Expression<Func<BankAccount, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<BankAccount> GetAsync(Expression<Func<BankAccount, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}


