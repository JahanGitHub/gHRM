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
    public interface ILoanTypeService : IServiceBase<LoanType>
    {
        IEnumerable<LoanType> GetLoanTypeByName(string loanType);
        LoanType GetLoanTypeLoanTypeId(int loanTypeId);
        
    }
   public class LoanTypeService: ILoanTypeService
    {
        private readonly ILoanTypeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public LoanTypeService(ILoanTypeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<LoanType> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public LoanType GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public LoanType Create(LoanType objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }
             
        public void Update(LoanType objectToUpdate)
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

        public LoanType Get(Expression<Func<LoanType, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LoanType> GetMany(Expression<Func<LoanType, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsDeleted == false);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<LoanType>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<LoanType>> GetManyAsync(Expression<Func<LoanType, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<LoanType> GetAsync(Expression<Func<LoanType, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        //public IEnumerable<PFType> GetPFTypes(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        //{
        //    return repository.GetPFTypes(filterColumnName, filterValue, startRowIndex, jtSorting, pageSize, out TotCount);
        //}
        public IEnumerable<LoanType> GetLoanTypeByName(string loanType)
        {
            return repository.GetLoanTypeByName(loanType);
        }
       public LoanType GetLoanTypeLoanTypeId(int loanTypeId)
        {
            return repository.GetLoanTypeLoanTypeId(loanTypeId);
        }
    }
}
