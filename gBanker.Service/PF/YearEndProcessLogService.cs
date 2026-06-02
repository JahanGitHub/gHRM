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
    public interface IYearEndProcessLogService : IServiceBase<YearEndProcessLog>
    {
        YearEndProcessLog GetLastYearEndProcessLog(out bool opStatus);
        bool IsProcessed(DateTime yearStartDate, DateTime yearEndDate);
        bool IsValidYearForEnding(DateTime yearStartDate, DateTime yearEndDate, out string message);
        bool IsProfitDistributed(DateTime yearStartDate, DateTime yearEndDate, out string message);
    }
   public class YearEndProcessLogService : IYearEndProcessLogService
    {
        private readonly IYearEndProcessLogRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public YearEndProcessLogService(IYearEndProcessLogRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<YearEndProcessLog> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public YearEndProcessLog GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public YearEndProcessLog Create(YearEndProcessLog objectToCreate)
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

        public void Update(YearEndProcessLog objectToUpdate)
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

        public YearEndProcessLog Get(Expression<Func<YearEndProcessLog, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<YearEndProcessLog> GetMany(Expression<Func<YearEndProcessLog, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsDeleted == false);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<YearEndProcessLog>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<YearEndProcessLog>> GetManyAsync(Expression<Func<YearEndProcessLog, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<YearEndProcessLog> GetAsync(Expression<Func<YearEndProcessLog, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        public YearEndProcessLog GetLastYearEndProcessLog(out bool opStatus)
        {
            return repository.GetLastYearEndProcessLog(out opStatus);
        }
       public bool IsProcessed(DateTime yearStartDate, DateTime yearEndDate)
       {
           return repository.IsProcessed(yearStartDate, yearEndDate);
       }
       public bool IsValidYearForEnding(DateTime yearStartDate, DateTime yearEndDate, out string message)
       {
           return repository.IsValidYearForEnding(yearStartDate, yearEndDate, out message);
       }

       public bool IsProfitDistributed(DateTime yearStartDate, DateTime yearEndDate, out string message)
       {
           return repository.IsProfitDistributed(yearStartDate, yearEndDate, out message);
       }
       
    }
}
