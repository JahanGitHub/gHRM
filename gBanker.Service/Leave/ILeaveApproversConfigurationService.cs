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
    public interface ILeaveApproversConfigurationService : IServiceBase<LeaveApproversConfiguration>
    {
        List<LeaveApproversConfiguration> AddApprovalConfigList(List<LeaveApproversConfiguration> objs);
    }
    public class LeaveApproversConfigurationService : ILeaveApproversConfigurationService
    {
        private readonly ILeaveApproversConfigurationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public LeaveApproversConfigurationService(ILeaveApproversConfigurationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<LeaveApproversConfiguration> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.ID);
            return entities;
        }

        public LeaveApproversConfiguration GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public LeaveApproversConfiguration Create(LeaveApproversConfiguration objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LeaveApproversConfiguration objectToUpdate)
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

        public LeaveApproversConfiguration Get(Expression<Func<LeaveApproversConfiguration, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LeaveApproversConfiguration> GetMany(Expression<Func<LeaveApproversConfiguration, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<LeaveApproversConfiguration>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<LeaveApproversConfiguration>> GetManyAsync(Expression<Func<LeaveApproversConfiguration, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }

        public virtual async Task<LeaveApproversConfiguration> GetAsync(Expression<Func<LeaveApproversConfiguration, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        public List<LeaveApproversConfiguration> AddApprovalConfigList(List<LeaveApproversConfiguration> objs)
        {
            repository.AddApprovalConfigList(objs);
            return objs;
        }
        #endregion

    }
}

