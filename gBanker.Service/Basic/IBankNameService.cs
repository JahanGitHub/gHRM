using gHRM.Data.CodeFirstMigration.Basic;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Basic
{
    public interface IBankNameService : IServiceBase<BankName>
    {

    }
    public class BankNameService : IBankNameService
    {
        private readonly IBankNameRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public BankNameService(IBankNameRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<BankName> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Id);
            return entities;
        }

        public BankName GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public BankName Create(BankName objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(BankName objectToUpdate)
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
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }


        public BankName Get(Expression<Func<BankName, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<BankName> GetMany(Expression<Func<BankName, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<BankName>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<BankName>> GetManyAsync(Expression<Func<BankName, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<BankName> GetAsync(Expression<Func<BankName, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
