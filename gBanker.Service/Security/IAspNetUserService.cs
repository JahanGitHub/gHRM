using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
      public interface IAspNetUserService : IServiceBase<AspNetUser>
    {
          AspNetUser GetByName(string name);
          AspNetUser GetByUserId(string id);
          void DeleteLogin(string id);
      }

    public class AspNetUserService : IAspNetUserService
    {
        private readonly IUserRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AspNetUserService(IUserRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AspNetUser> GetAll()
        {
            var entities = repository.GetAll().OrderByDescending(c => c.Id);
            return entities;
        }

        public AspNetUser GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AspNetUser Create(AspNetUser objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AspNetUser objectToUpdate)
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

        public AspNetUser Get(Expression<Func<AspNetUser, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AspNetUser> GetMany(Expression<Func<AspNetUser, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.Activated == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<AspNetUser>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<AspNetUser>> GetManyAsync(Expression<Func<AspNetUser, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<AspNetUser> GetAsync(Expression<Func<AspNetUser, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public AspNetUser GetByName(string name)
        {
            var entity = repository.Get(g => g.UserName == name);
            return entity;
        }


        public AspNetUser GetByUserId(string id)
        {
            var entity = repository.Get(g => g.Id == id );
            return entity;
        }


        public void DeleteLogin(string id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }
    }
}
