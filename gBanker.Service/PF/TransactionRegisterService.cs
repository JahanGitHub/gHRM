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
    public interface ITransactionRegisterService : IServiceBase<TransactionRegister>
    {
        List<TransactionRegister> SaveVoucher(List<TransactionRegister> objTransactionRegisters);
    }
    public class TransactionRegisterService : ITransactionRegisterService
    {
        private readonly ITransactionRegisterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public TransactionRegisterService(ITransactionRegisterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<TransactionRegister> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public TransactionRegister GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public TransactionRegister Create(TransactionRegister objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(TransactionRegister objectToUpdate)
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
        public TransactionRegister Get(Expression<Func<TransactionRegister, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<TransactionRegister> GetMany(Expression<Func<TransactionRegister, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsDeleted == false);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<TransactionRegister>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<TransactionRegister>> GetManyAsync(Expression<Func<TransactionRegister, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<TransactionRegister> GetAsync(Expression<Func<TransactionRegister, bool>> where)
        {
            return await repository.GetAsync(where);
        }

        public List<TransactionRegister> SaveVoucher(List<TransactionRegister> objTransactionRegisters) 
        {
            return repository.SaveVoucher(objTransactionRegisters);
        }

        #endregion
    }
}