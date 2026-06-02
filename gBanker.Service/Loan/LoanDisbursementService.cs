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
    public interface ILoanDisbursementService : IServiceBase<LoanDisbursement>
    { }
    public class LoanDisbursementService : ILoanDisbursementService
    {
        private readonly ILoanDisbursementRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public LoanDisbursementService(ILoanDisbursementRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<LoanDisbursement> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public LoanDisbursement GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public LoanDisbursement Create(LoanDisbursement objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LoanDisbursement objectToUpdate)
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

        public LoanDisbursement Get(Expression<Func<LoanDisbursement, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LoanDisbursement> GetMany(Expression<Func<LoanDisbursement, bool>> where)
        {
            var entities = repository.GetMany(where).Where(x=>!(x.IsDeleted??false));
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<LoanDisbursement>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<LoanDisbursement>> GetManyAsync(Expression<Func<LoanDisbursement, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<LoanDisbursement> GetAsync(Expression<Func<LoanDisbursement, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
