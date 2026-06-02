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
    public interface IAssetOutService : IServiceBase<AssetOut>
    {
        AssetOut GetByIdLong(long id);
    }
    public class AssetOutService : IAssetOutService
    {
        private readonly IAssetOutRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AssetOutService(IAssetOutRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AssetOut> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.AssetOutID);
            return entities;
        }       
        public AssetOut GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AssetOut Create(AssetOut objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AssetOut objectToUpdate)
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


        public AssetOut Get(Expression<Func<AssetOut, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AssetOut> GetMany(Expression<Func<AssetOut, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }        
        public AssetOut GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Task<IEnumerable<AssetOut>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AssetOut>> GetManyAsync(Expression<Func<AssetOut, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<AssetOut> GetAsync(Expression<Func<AssetOut, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
