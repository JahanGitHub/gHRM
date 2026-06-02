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
    public interface IInstituteService : IServiceBase<Institute>
    {
        bool Save(Institute Data, long LoggedInEmployeeId, out string Message);
        void DeleteInstitute(int Id);
    }
    public class InstituteService : IInstituteService
    {
        private readonly IInstituteRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public InstituteService(IInstituteRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public bool Save(Institute Data, long LoggedInEmployeeId, out string Message)
        {
            return repository.Save(Data, LoggedInEmployeeId, out Message);
        }

        public void DeleteInstitute(int Id)
        {
            repository.DeleteInstitute(Id);
        }

        #region Implementation for IServiceBase
        public IEnumerable<Institute> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public Institute GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Institute Create(Institute objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Institute objectToUpdate)
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

        public Institute Get(Expression<Func<Institute, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }

        public IEnumerable<Institute> GetMany(Expression<Func<Institute, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public virtual async Task<IEnumerable<Institute>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public virtual async Task<IEnumerable<Institute>> GetManyAsync(Expression<Func<Institute, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }

        public virtual async Task<Institute> GetAsync(Expression<Func<Institute, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
