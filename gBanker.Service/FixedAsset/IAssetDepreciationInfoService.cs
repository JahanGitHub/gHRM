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
    public interface IAssetDepreciationInfoService : IServiceBase<AssetDepreciationInfo>
    {
        bool UpdateAssetDepreciationInfo(AssetDepreciationInfo assetDepreciationInfo);
    }
    public class AssetDepreciationInfoService : IAssetDepreciationInfoService
    {
        private readonly IAssetDepreciationInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AssetDepreciationInfoService(IAssetDepreciationInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AssetDepreciationInfo> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.AssetID);
            return entities;
        }

        public AssetDepreciationInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AssetDepreciationInfo Create(AssetDepreciationInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AssetDepreciationInfo objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }
        public bool UpdateAssetDepreciationInfo(AssetDepreciationInfo assetDepreciationInfo)
        {
            return repository.UpdateAssetDepreciationInfo(assetDepreciationInfo);
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


        public AssetDepreciationInfo Get(Expression<Func<AssetDepreciationInfo, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AssetDepreciationInfo> GetMany(Expression<Func<AssetDepreciationInfo, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
        public AssetDepreciationInfo GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AssetDepreciationInfo>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AssetDepreciationInfo>> GetManyAsync(Expression<Func<AssetDepreciationInfo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<AssetDepreciationInfo> GetAsync(Expression<Func<AssetDepreciationInfo, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
