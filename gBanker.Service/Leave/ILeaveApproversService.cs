
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface ILeaveApproversService : IServiceBase<LeaveApprovers>
    {
        List<LeaveApprovers> AddApproversList(List<LeaveApprovers> objs);
    }

    public class LeaveApproversService : ILeaveApproversService
    {
        private readonly ILeaveApproversRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public LeaveApproversService(ILeaveApproversRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<LeaveApprovers> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.ID);
            return entities;
        }

        public LeaveApprovers GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public LeaveApprovers Create(LeaveApprovers objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LeaveApprovers objectToUpdate)
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

        public LeaveApprovers Get(Expression<Func<LeaveApprovers, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LeaveApprovers> GetMany(Expression<Func<LeaveApprovers, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<LeaveApprovers>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<LeaveApprovers>> GetManyAsync(Expression<Func<LeaveApprovers, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }

        public virtual async Task<LeaveApprovers> GetAsync(Expression<Func<LeaveApprovers, bool>> where)
        {
            return await repository.GetAsync(where);
        }

        public List<LeaveApprovers> AddApproversList(List<LeaveApprovers> objs)
        {
            repository.AddApproversList(objs);
            return objs;
        }

        #endregion

    }
}

