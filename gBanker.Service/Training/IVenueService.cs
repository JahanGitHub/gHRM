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
    public interface IVenueService : IServiceBase<Venue>
    {
        bool Save(Venue Data, long LoggedInEmployeeId, out string Message);
        void DeleteVenue(int Id);
    }
    public class VenueService : IVenueService
    {
        private readonly IVenueRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public VenueService(IVenueRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public bool Save(Venue Data, long LoggedInEmployeeId, out string Message)
        {
            return repository.Save(Data, LoggedInEmployeeId, out Message);
        }

        public void DeleteVenue(int Id)
        {
            repository.DeleteVenue(Id);
        }

        #region Implementation for IServiceBase
        public IEnumerable<Venue> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public Venue GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Venue Create(Venue objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Venue objectToUpdate)
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

        public Venue Get(Expression<Func<Venue, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }

        public IEnumerable<Venue> GetMany(Expression<Func<Venue, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public virtual async Task<IEnumerable<Venue>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public virtual async Task<IEnumerable<Venue>> GetManyAsync(Expression<Func<Venue, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }

        public virtual async Task<Venue> GetAsync(Expression<Func<Venue, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
