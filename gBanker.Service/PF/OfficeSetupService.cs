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
    public interface IOfficeSetupService : IServiceBase<OfficeSetup>
    {
        IEnumerable<OfficeSetup> GetOfficeSetupByName(string officeName);
    }
    public class OfficeSetupService: IOfficeSetupService
    {
         private readonly IOfficeSetupRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public OfficeSetupService(IOfficeSetupRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<OfficeSetup> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public OfficeSetup GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public OfficeSetup Create(OfficeSetup objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(OfficeSetup objectToUpdate)
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
        public OfficeSetup Get(Expression<Func<OfficeSetup, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<OfficeSetup> GetMany(Expression<Func<OfficeSetup, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsDeleted == false);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<OfficeSetup>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<OfficeSetup>> GetManyAsync(Expression<Func<OfficeSetup, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<OfficeSetup> GetAsync(Expression<Func<OfficeSetup, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        public IEnumerable<OfficeSetup> GetOfficeSetupByName(string officeName)
        {
            return repository.GetOfficeSetupByName(officeName);
        }
    }
}
