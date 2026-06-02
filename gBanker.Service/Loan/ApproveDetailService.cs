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
    public interface IApproveDetailService : IServiceBase<ApproveDetail>
    { }
    public class ApproveDetailService : IApproveDetailService
    {
        private readonly IApproveDetailRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApproveDetailService(IApproveDetailRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ApproveDetail> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public ApproveDetail GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public ApproveDetail Create(ApproveDetail objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ApproveDetail objectToUpdate)
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

        public ApproveDetail Get(Expression<Func<ApproveDetail, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ApproveDetail> GetMany(Expression<Func<ApproveDetail, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<ApproveDetail>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<ApproveDetail>> GetManyAsync(Expression<Func<ApproveDetail, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<ApproveDetail> GetAsync(Expression<Func<ApproveDetail, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
