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
    public interface ILoanPurposeService : IServiceBase<LoanPurpose>
    { }
    public class LoanPurposeService : ILoanPurposeService
    {
        private readonly ILoanPurposeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public LoanPurposeService(ILoanPurposeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<LoanPurpose> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public LoanPurpose GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public LoanPurpose Create(LoanPurpose objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LoanPurpose objectToUpdate)
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

        public LoanPurpose Get(Expression<Func<LoanPurpose, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LoanPurpose> GetMany(Expression<Func<LoanPurpose, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<LoanPurpose>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<LoanPurpose>> GetManyAsync(Expression<Func<LoanPurpose, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<LoanPurpose> GetAsync(Expression<Func<LoanPurpose, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
