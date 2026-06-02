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
    public interface IYearEndVoucherService : IServiceBase<YearEndVoucher>
    {
        
    }

    public class YearEndVoucherService : IYearEndVoucherService
    {
        private readonly IYearEndVoucherRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public YearEndVoucherService(IYearEndVoucherRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<YearEndVoucher> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public YearEndVoucher GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public YearEndVoucher Create(YearEndVoucher objectToCreate)
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

        public void Update(YearEndVoucher objectToUpdate)
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

        public YearEndVoucher Get(Expression<Func<YearEndVoucher, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<YearEndVoucher> GetMany(Expression<Func<YearEndVoucher, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsDeleted == false);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<YearEndVoucher>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<YearEndVoucher>> GetManyAsync(Expression<Func<YearEndVoucher, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<YearEndVoucher> GetAsync(Expression<Func<YearEndVoucher, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
