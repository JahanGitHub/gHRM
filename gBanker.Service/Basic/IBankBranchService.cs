
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
    public interface IBankBranchService : IServiceBase<BankBranch>
    {

    }
    public class BankBranchService : IBankBranchService
    {
        private readonly IBankBranchRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public BankBranchService(IBankBranchRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<BankBranch> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.BranchId);
            return entities;
        }

        public BankBranch GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public BankBranch Create(BankBranch objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(BankBranch objectToUpdate)
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

        public BankBranch Get(Expression<Func<BankBranch, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<BankBranch> GetMany(Expression<Func<BankBranch, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<BankBranch>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<BankBranch>> GetManyAsync(Expression<Func<BankBranch, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<BankBranch> GetAsync(Expression<Func<BankBranch, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}

