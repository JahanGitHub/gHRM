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
    public interface ITempPFCollectionService : IServiceBase<TempPFCollection>
    {
        bool AddBulk(List<TempPFCollection> objs);
    }
    public class TempPFCollectionService : ITempPFCollectionService
    {
        private readonly ITempPFCollectionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public TempPFCollectionService(ITempPFCollectionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<TempPFCollection> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public TempPFCollection GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            // unitOfWork.Commit();
            unitOfWork.Commit();
        }
        public TempPFCollection Create(TempPFCollection objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

       
        public void Update(TempPFCollection objectToUpdate)
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
             throw new NotImplementedException();
        }

        public TempPFCollection Get(Expression<Func<TempPFCollection, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<TempPFCollection> GetMany(Expression<Func<TempPFCollection, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<TempPFCollection>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<TempPFCollection>> GetManyAsync(Expression<Func<TempPFCollection, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<TempPFCollection> GetAsync(Expression<Func<TempPFCollection, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        // Extra function
        public bool AddBulk(List<TempPFCollection> objs)
        {
            repository.AddBulk(objs);
            return true;
        }
    }
}
