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
    public interface IPRInstallmentProcessLogService : IServiceBase<PRInstallmentProcessLog>
    {
        //AccountChart GetAccountChartByAccountCode(string accountCode);
    }
   public class PRInstallmentProcessLogService: IPRInstallmentProcessLogService
    {
        private readonly IPRInstallmentProcessLogRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public PRInstallmentProcessLogService(IPRInstallmentProcessLogRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<PRInstallmentProcessLog> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public PRInstallmentProcessLog GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public PRInstallmentProcessLog Create(PRInstallmentProcessLog objectToCreate)
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

        public void Update(PRInstallmentProcessLog objectToUpdate)
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

        public PRInstallmentProcessLog Get(Expression<Func<PRInstallmentProcessLog, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<PRInstallmentProcessLog> GetMany(Expression<Func<PRInstallmentProcessLog, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsDeleted == false);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<PRInstallmentProcessLog>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<PRInstallmentProcessLog>> GetManyAsync(Expression<Func<PRInstallmentProcessLog, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<PRInstallmentProcessLog> GetAsync(Expression<Func<PRInstallmentProcessLog, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        //public AccountChart GetAccountChartByAccountCode(string accountCode)
        //{
        //    return repository.GetAccountChartByAccountCode(accountCode);
        //}
        
    }
}
