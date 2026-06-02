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
    public interface ILeaveApproversAuthorityService : IServiceBase<LeaveApproversAuthority>
    {
    }

    public class LeaveApproversAuthorityService : ILeaveApproversAuthorityService
    {
        private readonly ILeaveApproversAuthorityRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public LeaveApproversAuthorityService(ILeaveApproversAuthorityRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<LeaveApproversAuthority> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Id);
            return entities;
        }

        public LeaveApproversAuthority GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public LeaveApproversAuthority Create(LeaveApproversAuthority objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LeaveApproversAuthority objectToUpdate)
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

        public LeaveApproversAuthority Get(Expression<Func<LeaveApproversAuthority, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LeaveApproversAuthority> GetMany(Expression<Func<LeaveApproversAuthority, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<LeaveApproversAuthority>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<LeaveApproversAuthority>> GetManyAsync(Expression<Func<LeaveApproversAuthority, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<LeaveApproversAuthority> GetAsync(Expression<Func<LeaveApproversAuthority, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}

