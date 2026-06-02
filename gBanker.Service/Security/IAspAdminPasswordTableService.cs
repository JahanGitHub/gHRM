using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IAspAdminPasswordTableService : IServiceBase<AspAdminPasswordTable>
    {
        AspAdminPasswordTable GetByUserName(string uName);
    }
    public class AspAdminPasswordTableService : IAspAdminPasswordTableService
    {
        private readonly IAspAdminPasswordTableRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AspAdminPasswordTableService(IAspAdminPasswordTableRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AspAdminPasswordTable> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.PasswordId);
            return entities;
        }
        public AspAdminPasswordTable GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public AspAdminPasswordTable GetByUserName(string uName)
        {
            var entity = repository.Get(w => w.UserName == uName);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }

        public AspAdminPasswordTable Get(Expression<Func<AspAdminPasswordTable, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AspAdminPasswordTable> GetMany(Expression<Func<AspAdminPasswordTable, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<AspAdminPasswordTable>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<AspAdminPasswordTable>> GetManyAsync(Expression<Func<AspAdminPasswordTable, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<AspAdminPasswordTable> GetAsync(Expression<Func<AspAdminPasswordTable, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        public AspAdminPasswordTable Create(AspAdminPasswordTable objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AspAdminPasswordTable objectToUpdate)
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
           throw new NotImplementedException();
           
        }
        public bool IsContinued(long id)
        {
           throw new NotImplementedException();           
        }
    }
}
