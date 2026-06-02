using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Loan;
using gHRM.Data.Repository.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace gHRM.Service.Loan
{
    public interface ICollectionMethodService : IServiceBase<CollectionMethod>
    { }
    public class CollectionMethodService : ICollectionMethodService
    {
        private readonly ICollectionMethodRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public CollectionMethodService(ICollectionMethodRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<CollectionMethod> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public CollectionMethod GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public CollectionMethod Create(CollectionMethod objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(CollectionMethod objectToUpdate)
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

            }
            return true;
        }

        public CollectionMethod Get(Expression<Func<CollectionMethod, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<CollectionMethod> GetMany(Expression<Func<CollectionMethod, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<CollectionMethod>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<CollectionMethod>> GetManyAsync(Expression<Func<CollectionMethod, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<CollectionMethod> GetAsync(Expression<Func<CollectionMethod, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
