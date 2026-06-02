using BasicDataAccess;
using gHRM.Core.Common;
using gHRM.Core.Filters.Offices;
using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration.Apply;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using gHRM.Data.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IJobsCircularService : IServiceBase<JobsCircular>
    {
        JobsCircular GetByCreatedBy(long? userId);
    }
    public class JobsCircularService : IJobsCircularService
    {
        private readonly IJobsCircularRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public JobsCircularService(IJobsCircularRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        #region Implementation for IServiceBase
        public IEnumerable<JobsCircular> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.JobId);
            return entities;
        }

        public JobsCircular GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public JobsCircular Create(JobsCircular objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(JobsCircular objectToUpdate)
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
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }

        public JobsCircular GetByCreatedBy(long? UserId)
        {
            var entity = repository.Get(p => p.CreatedBy == UserId);
            return entity;
        }
        public bool IsContinued(long id)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == true)
                {
                    return false;
                }
            }
            return true;
        }

        public JobsCircular Get(Expression<Func<JobsCircular, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }

        public IEnumerable<JobsCircular> GetMany(Expression<Func<JobsCircular, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public virtual async Task<IEnumerable<JobsCircular>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public virtual async Task<IEnumerable<JobsCircular>> GetManyAsync(Expression<Func<JobsCircular, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }

        public virtual async Task<JobsCircular> GetAsync(Expression<Func<JobsCircular, bool>> where)
        {
            return await repository.GetAsync(where);
        }


        #endregion
    }
}

