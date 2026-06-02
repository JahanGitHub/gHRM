using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IInvStoreService : IServiceBase<Inv_Store>
    {
    }
    public class InvStoreService : IInvStoreService
    {
        private readonly IInvStoreRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public InvStoreService(IInvStoreRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Inv_Store> GetAll()
        {
            return repository.GetMany(g => g.IsActive == true);
        }
       

        public IEnumerable<Inv_Store> GetCheck(int warehouseID, int itemID)
        {
            return repository.GetMany(gm => gm.WarehouseID == warehouseID && gm.ItemID==itemID && gm.IsActive == true);
        }
        
        public Inv_Store GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public Inv_Store Create(Inv_Store objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Inv_Store objectToUpdate)
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
        public bool IsValidInv_Items(Inv_Store Inv_Items)
        {
            var entity = repository.Get(p => p.WarehouseID == Inv_Items.WarehouseID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<Inv_Store> SearchInv_Items()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.WarehouseID);
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }

        public bool IsContinued(long id)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == true)
                {
                    return false;
                }
            }

            return true;
        }
        public Inv_Store GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<Inv_Store> GetMany(Expression<Func<Inv_Store, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public Inv_Store Get(Expression<Func<Inv_Store, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Inv_Store>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Inv_Store>> GetManyAsync(Expression<Func<Inv_Store, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<Inv_Store> GetAsync(Expression<Func<Inv_Store, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
