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
    public interface IManualOvertimeConfigurationService : IServiceBase<ManualOvertimeConfiguration>
    {
        bool IsManualConfigSaveValid(ManualOvertimeConfiguration Config, out string Message);
        void DeleteConfiguration(long Id);
        void DisablePreviousConfig(ManualOvertimeConfiguration Config);
    }
    public class ManualOvertimeConfigurationService : IManualOvertimeConfigurationService
    {
        private readonly IManualOvertimeConfigurationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ManualOvertimeConfigurationService(IManualOvertimeConfigurationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public bool IsManualConfigSaveValid(ManualOvertimeConfiguration Config, out string Message)
        {
            return repository.IsManualConfigSaveValid(Config, out Message);
        }

        public void DeleteConfiguration(long Id)
        {
            repository.DeleteConfiguration(Id);
        }

        public void DisablePreviousConfig(ManualOvertimeConfiguration Config)
        {
            repository.DisablePreviousConfig(Config);
        }

        #region Implementation for IServiceBase
        public IEnumerable<ManualOvertimeConfiguration> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public ManualOvertimeConfiguration GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ManualOvertimeConfiguration Create(ManualOvertimeConfiguration objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ManualOvertimeConfiguration objectToUpdate)
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

        public ManualOvertimeConfiguration Get(Expression<Func<ManualOvertimeConfiguration, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }

        public IEnumerable<ManualOvertimeConfiguration> GetMany(Expression<Func<ManualOvertimeConfiguration, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public virtual async Task<IEnumerable<ManualOvertimeConfiguration>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public virtual async Task<IEnumerable<ManualOvertimeConfiguration>> GetManyAsync(Expression<Func<ManualOvertimeConfiguration, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }

        public virtual async Task<ManualOvertimeConfiguration> GetAsync(Expression<Func<ManualOvertimeConfiguration, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}

