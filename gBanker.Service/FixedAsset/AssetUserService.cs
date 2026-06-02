using gHRM.Data.Repository;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IAssetUserService : IServiceBase<AssetUser>
    {

    }
    public class AssetUserService : IAssetUserService
    {
        private readonly IAssetUserRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AssetUserService(IAssetUserRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AssetUser> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.UserID);
            return entities;
        }
        public AssetUser GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AssetUser Create(AssetUser objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AssetUser objectToUpdate)
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


        public AssetUser Get(Expression<Func<AssetUser, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AssetUser> GetMany(Expression<Func<AssetUser, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
        public AssetUser GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AssetUser>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AssetUser>> GetManyAsync(Expression<Func<AssetUser, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<AssetUser> GetAsync(Expression<Func<AssetUser, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
