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

    public interface IAccountChartService : IServiceBase<AccountChart>
    {
        AccountChart GetAccountChartByAccountCode(string accountCode);
        AccountChart GetAccountChartExceptThisAccountCode(string accountCode, string accountName);
        IEnumerable<AccountChart> GetAccountChartByName(string accountName);
        List<AccountChart> AddAccountChartAndParent(List<AccountChart> objAccountCharts);
        AccountChart AddAccountChart(AccountChart objAccountChart);

        IEnumerable<AccountChart> GetVoucherableAccountChart(string voucherType);
    }
    public class AccountChartService : IAccountChartService
    {
        private readonly IAccountChartRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AccountChartService(IAccountChartRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AccountChart> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public AccountChart GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public AccountChart Create(AccountChart objectToCreate)
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

        public void Update(AccountChart objectToUpdate)
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

        #region Asyc
        public virtual async Task<IEnumerable<AccountChart>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<AccountChart>> GetManyAsync(Expression<Func<AccountChart, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<AccountChart> GetAsync(Expression<Func<AccountChart, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        public AccountChart Get(Expression<Func<AccountChart, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AccountChart> GetMany(Expression<Func<AccountChart, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsDeleted == false);
            return entities;
        }
        
        public AccountChart GetAccountChartByAccountCode(string accountCode)
        {
            return repository.GetAccountChartByAccountCode(accountCode);
        }
        public AccountChart GetAccountChartExceptThisAccountCode(string accountCode, string accountName)
        {
            return repository.GetAccountChartExceptThisAccountCode(accountCode, accountName);
        }
        public IEnumerable<AccountChart> GetAccountChartByName(string accountName)
        {
            return repository.GetAccountChartByName(accountName);
        }

        public List<AccountChart> AddAccountChartAndParent(List<AccountChart> objAccountCharts)
        {
           return repository.AddAccountChartAndParent(objAccountCharts);
        }

        public AccountChart AddAccountChart(AccountChart objAccountChart)
        {
            return repository.AddAccountChart(objAccountChart);
        }

        public IEnumerable<AccountChart> GetVoucherableAccountChart(string voucherType)
        {
            return repository.GetVoucherableAccountChart(voucherType);
        }
    }
}
