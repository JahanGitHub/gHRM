using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Loan;
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
    public interface ILoanDisbursementService : IServiceBase<LoanDisbursement>
    {
        IEnumerable<LoanDisbursement> GetLoanDisburseInfoByEmployeeId(long employeeId, int loanTypeId);
        LoanDisbursement GetEmployeeWiseByLoanTypeId(long employeeId, int loanTypeId);
    }
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
        public LoanDisbursement GetEmployeeWiseByLoanTypeId(long employeeId,int loanTypeId)
        {
            var single = new LoanDisbursement();
            using (var db = new gHRMDBContext())
            {
                single = db.LoanDisbursements
                                .FirstOrDefault(x => x.EmployeeId == employeeId 
                                   // && x.LoanTypeId == loanTypeId
                                    && x.IsInstallmentOver == false && x.IsDeleted == false);                
            }

            return single;
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

        public LoanDisbursement Get(Expression<Func<LoanDisbursement, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LoanDisbursement> GetMany(Expression<Func<LoanDisbursement, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsDeleted == false);
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
        public IEnumerable<LoanDisbursement> GetLoanDisburseInfoByEmployeeId(long employeeId, int loanTypeId)
        {
            return repository.GetLoanDisburseInfoByEmployeeId(employeeId, loanTypeId);
        }
    }
}
