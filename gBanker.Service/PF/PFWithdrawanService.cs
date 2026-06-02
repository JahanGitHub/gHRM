using gHRM.Data.CodeFirstMigration;
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
    public interface IPFWithdrawanService : IServiceBase<PFWithdrawan>
    {
        bool IsExistPFWithdrawan(PFWithdrawan model);
    }
    public class PFWithdrawanService : IPFWithdrawanService
    {
        private readonly IPFWithdrawanRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public PFWithdrawanService(IPFWithdrawanRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<PFWithdrawan> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public PFWithdrawan GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public bool IsExistPFWithdrawan(PFWithdrawan model)
        {
            var isExistPFWithdrawan = false;

            using (var db = new gHRMDBContext())
            {
                isExistPFWithdrawan = db.PFWithdrawan
                    .Any(f => !f.IsDeleted && f.EmployeeId == model.EmployeeId);
            }

            return isExistPFWithdrawan;
        }

        public void Save()
        {
            unitOfWork.Commit();
        }
        public PFWithdrawan Create(PFWithdrawan objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(PFWithdrawan objectToUpdate)
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

        public PFWithdrawan Get(Expression<Func<PFWithdrawan, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<PFWithdrawan> GetMany(Expression<Func<PFWithdrawan, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsDeleted == false);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<PFWithdrawan>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<PFWithdrawan>> GetManyAsync(Expression<Func<PFWithdrawan, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<PFWithdrawan> GetAsync(Expression<Func<PFWithdrawan, bool>> where)
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
