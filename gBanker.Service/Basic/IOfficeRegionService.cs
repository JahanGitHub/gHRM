using BasicDataAccess;
using gHRM.Core.Common;
using gHRM.Core.Filters.Offices;
using gHRM.Core.Utilities;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using gHRM.Data.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IOfficeRegionService : IServiceBase<OfficeRegion>
    {
        bool Save(OfficeRegion Data, long LoggedInEmployeeId, out string Message);
        void DeleteRegion(int Id);
        string GetNameById(int Id);
        bool SaveMapOffice(OfficeRegionMapping _RegionMap, long LoggedInEmployeeId, out string Message);
        void DeleteMapOffice(int Id);
    }
    public class OfficeRegionService : IOfficeRegionService
    {
        private readonly IOfficeRegionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public OfficeRegionService(IOfficeRegionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public bool Save(OfficeRegion Data, long LoggedInEmployeeId, out string Message)
        {
            return repository.Save(Data, LoggedInEmployeeId, out Message);
        }

        public void DeleteRegion(int Id)
        {
            repository.DeleteRegion(Id);
        }

        public string GetNameById(int Id)
        {
            return repository.GetNameById(Id);
        }

        public bool SaveMapOffice(OfficeRegionMapping _RegionMap, long LoggedInEmployeeId, out string Message)
        {
            return repository.SaveMapOffice(_RegionMap, LoggedInEmployeeId, out Message);
        }

        public void DeleteMapOffice(int Id)
        {
            repository.DeleteMapOffice(Id);
        }

        #region Implementation for IServiceBase
        public IEnumerable<OfficeRegion> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public OfficeRegion GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public OfficeRegion Create(OfficeRegion objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(OfficeRegion objectToUpdate)
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

        public OfficeRegion Get(Expression<Func<OfficeRegion, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }

        public IEnumerable<OfficeRegion> GetMany(Expression<Func<OfficeRegion, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public virtual async Task<IEnumerable<OfficeRegion>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public virtual async Task<IEnumerable<OfficeRegion>> GetManyAsync(Expression<Func<OfficeRegion, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }

        public virtual async Task<OfficeRegion> GetAsync(Expression<Func<OfficeRegion, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}

