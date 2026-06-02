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
    public interface IGLLevelService : IServiceBase<GLLevel>
    {
        IEnumerable<GLLevel> GetGLLevelByName(string glLevelName);
    }
    public class GLLevelService: IGLLevelService
    {
         private readonly IGLLevelRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public GLLevelService(IGLLevelRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<GLLevel> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public GLLevel GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public GLLevel Create(GLLevel objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(GLLevel objectToUpdate)
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

        public GLLevel Get(Expression<Func<GLLevel, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<GLLevel> GetMany(Expression<Func<GLLevel, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsDeleted == false);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<GLLevel>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<GLLevel>> GetManyAsync(Expression<Func<GLLevel, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<GLLevel> GetAsync(Expression<Func<GLLevel, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        public IEnumerable<GLLevel> GetGLLevelByName(string glLevelName)
        {
            return repository.GetGLLevelByName(glLevelName);
        }
    }
}
