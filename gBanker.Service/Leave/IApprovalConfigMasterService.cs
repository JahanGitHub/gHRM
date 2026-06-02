using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IApprovalConfigMasterService : IServiceBase<ApprovalConfigMaster>
    {

    }
    public class ApprovalConfigMasterService : IApprovalConfigMasterService
    {
        private readonly IApprovalConfigMasterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApprovalConfigMasterService(IApprovalConfigMasterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ApprovalConfigMaster> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.ConfigMasterId);
            return entities;
        }

        public ApprovalConfigMaster GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ApprovalConfigMaster Create(ApprovalConfigMaster objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ApprovalConfigMaster objectToUpdate)
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
        public ApprovalConfigMaster Get(Expression<Func<ApprovalConfigMaster, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ApprovalConfigMaster> GetMany(Expression<Func<ApprovalConfigMaster, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == 1);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<ApprovalConfigMaster>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<ApprovalConfigMaster>> GetManyAsync(Expression<Func<ApprovalConfigMaster, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<ApprovalConfigMaster> GetAsync(Expression<Func<ApprovalConfigMaster, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }
        

    }
}
