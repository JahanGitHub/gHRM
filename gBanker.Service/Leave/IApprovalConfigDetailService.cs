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
    public interface IApprovalConfigDetailService : IServiceBase<ApprovalConfigDetail>
    {
        ApprovalConfigDetail getApprovalDetailByMasterIdAndDetailsId(int MasterId, int DetailsId);
        List<ApprovalConfigDetail> AddApprovalConfigDetailList(List<ApprovalConfigDetail> objs);
    }
    public class ApprovalConfigDetailService : IApprovalConfigDetailService
    {
        private readonly IApprovalConfigDetailRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApprovalConfigDetailService(IApprovalConfigDetailRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ApprovalConfigDetail> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.ConfigDetailsId);
            return entities;
        }

        public ApprovalConfigDetail GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ApprovalConfigDetail Create(ApprovalConfigDetail objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ApprovalConfigDetail objectToUpdate)
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
        public ApprovalConfigDetail Get(Expression<Func<ApprovalConfigDetail, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ApprovalConfigDetail> GetMany(Expression<Func<ApprovalConfigDetail, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == 1);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<ApprovalConfigDetail>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<ApprovalConfigDetail>> GetManyAsync(Expression<Func<ApprovalConfigDetail, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<ApprovalConfigDetail> GetAsync(Expression<Func<ApprovalConfigDetail, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException(); ;
        }
        public ApprovalConfigDetail getApprovalDetailByMasterIdAndDetailsId(int MasterId, int DetailsId)
        {
            var list = repository.Get(b => b.ConfigMasterId == MasterId && b.ConfigDetailsId == DetailsId);
            return list;

        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public List<ApprovalConfigDetail> AddApprovalConfigDetailList(List<ApprovalConfigDetail> objs)
        {
            repository.AddApprovalConfigDetailList(objs);
            return objs;
        }
    }
}
