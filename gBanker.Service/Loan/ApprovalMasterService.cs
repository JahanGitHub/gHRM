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
    public interface IApprovalMasterService : IServiceBase<ApprovalMaster>
    { }
    public class ApprovalMasterService : IApprovalMasterService
    {
        private readonly IApprovalMasterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApprovalMasterService(IApprovalMasterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ApprovalMaster> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public ApprovalMaster GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public ApprovalMaster Create(ApprovalMaster objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ApprovalMaster objectToUpdate)
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

        public ApprovalMaster Get(Expression<Func<ApprovalMaster, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ApprovalMaster> GetMany(Expression<Func<ApprovalMaster, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<ApprovalMaster>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<ApprovalMaster>> GetManyAsync(Expression<Func<ApprovalMaster, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<ApprovalMaster> GetAsync(Expression<Func<ApprovalMaster, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
