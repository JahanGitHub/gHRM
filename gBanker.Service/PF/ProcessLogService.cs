using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Data.Repository.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.PF
{
    public interface IProcessLogService : IServiceBase<ProcessLog>
    {
        ProcessLog GetLastProcessLog();
        bool IsDayOpen();
        ProcessLog GetDayStatus();
        ProcessLog GetCustomDayStatus();
    }
    public class ProcessLogService : IProcessLogService
    {
        private readonly IProcessLogRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ProcessLogService(IProcessLogRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ProcessLog> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public ProcessLog GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public ProcessLog Create(ProcessLog objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        //Asad Added
        //public List<AccountChart> AddRange(List<AccountChart> objectsToCreate)
        //{
        //    repository.AddRange(objectsToCreate);
        //    Save();
        //    return objectsToCreate;
        //}

        public void Update(ProcessLog objectToUpdate)
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
                //obj.InActiveDate = DateTime.Now;
                //obj.IsActive = false;
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
                //var isActive = obj.IsActive;
                //if (isActive == false)
                //{
                //    return false;
                //}
            }
            return true;
        }

        public ProcessLog Get(Expression<Func<ProcessLog, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ProcessLog> GetMany(Expression<Func<ProcessLog, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsDeleted == false);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<ProcessLog>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<ProcessLog>> GetManyAsync(Expression<Func<ProcessLog, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<ProcessLog> GetAsync(Expression<Func<ProcessLog, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        public ProcessLog GetLastProcessLog()
        {
            return repository.GetLastProcessLog();
        }

        public bool IsDayOpen()
        {
            return repository.IsDayOpen();
        }

        public ProcessLog GetDayStatus()
        {
           return repository.GetDayStatus();
        }

        public ProcessLog GetCustomDayStatus() 
        {
            return repository.GetCustomDayStatus();
        }
    }
}
