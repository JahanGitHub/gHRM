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
    public interface IAssetGroupInfoService : IServiceBase<AssetGroupInfo>
    {

    }
    public class AssetGroupInfoService : IAssetGroupInfoService
    {
        private readonly IAssetGroupInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AssetGroupInfoService(IAssetGroupInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AssetGroupInfo> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.AssetGroupID);
            return entities;
        }

        public AssetGroupInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AssetGroupInfo Create(AssetGroupInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AssetGroupInfo objectToUpdate)
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
            //throw new NotImplementedException();
            unitOfWork.Commit();
        }
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public AssetGroupInfo GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<AssetGroupInfo> GetMany(Expression<Func<AssetGroupInfo, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public AssetGroupInfo Get(Expression<Func<AssetGroupInfo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AssetGroupInfo>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AssetGroupInfo>> GetManyAsync(Expression<Func<AssetGroupInfo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<AssetGroupInfo> GetAsync(Expression<Func<AssetGroupInfo, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
