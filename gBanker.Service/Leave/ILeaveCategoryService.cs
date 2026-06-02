
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface ILeaveCategoryService : IServiceBase<LeaveCategory>
    {

    }

    public class LeaveCategoryService : ILeaveCategoryService
    {
        private readonly ILeaveCategoryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public LeaveCategoryService(ILeaveCategoryRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<LeaveCategory> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Id);
            return entities;
        }

        public LeaveCategory GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public LeaveCategory Create(LeaveCategory objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LeaveCategory objectToUpdate)
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

        public LeaveCategory Get(Expression<Func<LeaveCategory, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LeaveCategory> GetMany(Expression<Func<LeaveCategory, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<LeaveCategory>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<LeaveCategory>> GetManyAsync(Expression<Func<LeaveCategory, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<LeaveCategory> GetAsync(Expression<Func<LeaveCategory, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}
