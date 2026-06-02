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
    public interface IProfitDistProcessLogService : IServiceBase<ProfitDistProcessLog>
    {
        bool IsValidYearForprofitDist(DateTime yearStartDate, DateTime yearEndDate, out string message);
       
    }
    public class ProfitDistProcessLogService : IProfitDistProcessLogService
    {
        private readonly IProfitDistProcessLogRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ProfitDistProcessLogService(IProfitDistProcessLogRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ProfitDistProcessLog> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public ProfitDistProcessLog GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public ProfitDistProcessLog Create(ProfitDistProcessLog objectToCreate)
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

        public void Update(ProfitDistProcessLog objectToUpdate)
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

        public ProfitDistProcessLog Get(Expression<Func<ProfitDistProcessLog, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ProfitDistProcessLog> GetMany(Expression<Func<ProfitDistProcessLog, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsDeleted == false);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<ProfitDistProcessLog>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<ProfitDistProcessLog>> GetManyAsync(Expression<Func<ProfitDistProcessLog, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<ProfitDistProcessLog> GetAsync(Expression<Func<ProfitDistProcessLog, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        public bool IsValidYearForprofitDist(DateTime yearStartDate, DateTime yearEndDate, out string message)
        {
            return repository.IsValidYearForprofitDist(yearStartDate, yearEndDate, out message);
        }

        
    }
}
