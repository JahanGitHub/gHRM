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
    public interface IApplicationLogService : IServiceBase<ApplicationLog>
    {
        IEnumerable<ApplicationLog> GetApplicationLogPaged(string organizationId, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount);
    }
    public class ApplicationLogService : IApplicationLogService
    {
        private readonly IApplicationLogRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApplicationLogService(IApplicationLogRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<ApplicationLog> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }

        public ApplicationLog GetById(int id)
        {
            //throw new NotImplementedException();
            var entity = repository.GetById(id);
            return entity;
        }

        public ApplicationLog Create(ApplicationLog objectToCreate)
        {
            //throw new NotImplementedException();
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ApplicationLog objectToUpdate)
        {
            //throw new NotImplementedException();
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            //throw new NotImplementedException();
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public void Save()
        {
            //throw new NotImplementedException();
            unitOfWork.Commit();
        }
        public ApplicationLog Get(Expression<Func<ApplicationLog, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ApplicationLog> GetMany(Expression<Func<ApplicationLog, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<ApplicationLog>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<ApplicationLog>> GetManyAsync(Expression<Func<ApplicationLog, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<ApplicationLog> GetAsync(Expression<Func<ApplicationLog, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicationLog> GetApplicationLogPaged(string organizationId, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount)
        {
            return repository.GetApplicationLogPaged(organizationId, filterColumnName, filterValue, startRowIndex, pageSize, out totalCount);
        }
    }
}
