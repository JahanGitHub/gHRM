using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Loan;
using gHRM.Data.Repository.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace gHRM.Service.Loan
{
    public interface ILoanRegisterService : IServiceBase<LoanRegister>
    { }
    public class LoanRegisterService : ILoanRegisterService
    {
        private readonly ILoanRegisterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public LoanRegisterService(ILoanRegisterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<LoanRegister> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public LoanRegister GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public LoanRegister Create(LoanRegister objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LoanRegister objectToUpdate)
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

            }
            return true;
        }

        public LoanRegister Get(Expression<Func<LoanRegister, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LoanRegister> GetMany(Expression<Func<LoanRegister, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<LoanRegister>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<LoanRegister>> GetManyAsync(Expression<Func<LoanRegister, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<LoanRegister> GetAsync(Expression<Func<LoanRegister, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
