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
    public interface IAttendancePenaltyConfigurationService : IServiceBase<AttendancePenaltyConfiguration>
    {
        List<AttendancePenaltyConfiguration> AddAttendancePenaltyConfigurationList(List<AttendancePenaltyConfiguration> objs);
    }


    public class AttendancePenaltyConfigurationService : IAttendancePenaltyConfigurationService
    {
        private readonly IAttendancePenaltyConfigurationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AttendancePenaltyConfigurationService(IAttendancePenaltyConfigurationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<AttendancePenaltyConfiguration> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public AttendancePenaltyConfiguration GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AttendancePenaltyConfiguration Create(AttendancePenaltyConfiguration objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AttendancePenaltyConfiguration objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public AttendancePenaltyConfiguration Get(Expression<Func<AttendancePenaltyConfiguration, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AttendancePenaltyConfiguration> GetMany(Expression<Func<AttendancePenaltyConfiguration, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public List<AttendancePenaltyConfiguration> AddAttendancePenaltyConfigurationList(List<AttendancePenaltyConfiguration> objs)
        {
            repository.AddAttendancePenaltyConfigurationList(objs);
            return objs;
        }

        #region Asyc

        public virtual async Task<IEnumerable<AttendancePenaltyConfiguration>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public virtual async Task<IEnumerable<AttendancePenaltyConfiguration>> GetManyAsync(Expression<Func<AttendancePenaltyConfiguration, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }

        public virtual async Task<AttendancePenaltyConfiguration> GetAsync(Expression<Func<AttendancePenaltyConfiguration, bool>> where)
        {
            return await repository.GetAsync(where);
        }

        #endregion
    }
}
