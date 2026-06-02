using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Data.Repository.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.PF
{
    public interface IAccountTypeService : IServiceBase<AccountType>
    {
        IEnumerable<AccountType> GetAccountTypeByName(string accountType);
    }
    public class AccountTypeService: IAccountTypeService
    {
        private readonly IAccountTypeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AccountTypeService(IAccountTypeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AccountType> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public AccountType GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public AccountType Create(AccountType objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AccountType objectToUpdate)
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
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            //throw new NotImplementedException();
            var obj = repository.GetById(id);
            if (obj != null)
            {
                //obj.InActiveDate = DateTime.Now;
                //obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }
        public bool IsContinued(long id)
        {
            // throw new NotImplementedException();
            var obj = repository.GetById(id);
            if (obj != null)
            {
                //var isActive = obj.IsActive;
                //if (isActive == false)
                //{
                //    return false;
                //}
            }
            return true;
        }

        public AccountType Get(Expression<Func<AccountType, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AccountType> GetMany(Expression<Func<AccountType, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsDeleted == false);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<AccountType>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<AccountType>> GetManyAsync(Expression<Func<AccountType, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<AccountType> GetAsync(Expression<Func<AccountType, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        public IEnumerable<AccountType> GetAccountTypeByName(string accountType)
        {
            return repository.GetAccountTypeByName(accountType);
        }
    }
}
