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
    public interface ITransactionCategoryService : IServiceBase<TransactionCategory>
    {
        //AccountChart GetAccountChartByAccountCode(string accountCode);
        //IEnumerable<AccountChart> GetAccountChartByName(string accountName);
        //List<AccountChart> AddAccountChartAndParent(List<AccountChart> objAccountCharts);
        //AccountChart AddAccountChart(AccountChart objAccountChart);
    }
    public class TransactionCategoryService: ITransactionCategoryService
    {
         private readonly ITransactionCategoryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public TransactionCategoryService(ITransactionCategoryRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<TransactionCategory> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public TransactionCategory GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public TransactionCategory Create(TransactionCategory objectToCreate)
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

        public void Update(TransactionCategory objectToUpdate)
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

        public TransactionCategory Get(Expression<Func<TransactionCategory, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<TransactionCategory> GetMany(Expression<Func<TransactionCategory, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsDeleted == false);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<TransactionCategory>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<TransactionCategory>> GetManyAsync(Expression<Func<TransactionCategory, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<TransactionCategory> GetAsync(Expression<Func<TransactionCategory, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        //public AccountChart GetAccountChartByAccountCode(string accountCode)
        //{
        //    return repository.GetAccountChartByAccountCode(accountCode);
        //}
        //public IEnumerable<AccountChart> GetAccountChartByName(string accountName)
        //{
        //    return repository.GetAccountChartByName(accountName);
        //}

        //public List<AccountChart> AddAccountChartAndParent(List<AccountChart> objAccountCharts)
        //{
        //   return repository.AddAccountChartAndParent(objAccountCharts);
        //}

        //public AccountChart AddAccountChart(AccountChart objAccountChart)
        //{
        //    return repository.AddAccountChart(objAccountChart);
        //}
    }
}
