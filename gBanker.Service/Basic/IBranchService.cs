using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace gHRM.Service
{
    public interface IBranchService :IServiceBase<Branch>
    {
        IEnumerable<ValidationResult> IsValideBranch(string BranchName);
        IEnumerable<DBBranchDetails> GetBranchDetail(int companyId, int branchId, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
    }
    public class BranchService :IBranchService
    {
        private readonly IBranchRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public BranchService(IBranchRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Branch> GetAll()
        {
            var entities = repository.GetAll().Where(c=>c.IsActive==true).OrderBy(c => c.BranchId);
            return entities;
        }
        public Branch GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }


        public Branch Get(Expression<Func<Branch, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<Branch> GetMany(Expression<Func<Branch, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<Branch>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<Branch>> GetManyAsync(Expression<Func<Branch, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<Branch> GetAsync(Expression<Func<Branch, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        public Branch Create(Branch objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }
        public void Update(Branch objectToUpdate)
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
                obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
                obj.IsActive = false;
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
                var isActive = obj.IsActive;
                if (isActive == false)
                {
                    return false;
                }
            }

            return true;
        }
        IEnumerable<ValidationResult> IBranchService.IsValideBranch(string BranchName)
        {
            var entity = repository.Get(p => p.BranchName == BranchName);
            if (entity != null)
            {
                yield return new ValidationResult("BranchId", "Duplicate Branch Id.");

            }
        }
        public IEnumerable<DBBranchDetails> GetBranchDetail(int companyId, int branchId, int startRowIndex, string jtSorting, int pageSize, out long TotCount)
        {
            return repository.GetBranchDetail(companyId, branchId, startRowIndex, jtSorting, pageSize, out TotCount);
        }
    }
}
