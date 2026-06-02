using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IApplicationSettingService : IServiceBase<ApplicationSetting>
    {
        IEnumerable<ValidationResult> IsValidSettings(ApplicationSetting applicationsetting);
        IEnumerable<ValidationResult> IsValidEdit(ApplicationSetting applicationsetting);
        IEnumerable<DBApplicationSettingsDetail> GetApplicationDetailDetail(int? officeID);
    }
    public class ApplicationSettingService: IApplicationSettingService
    {
        private readonly IApplicationSettingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApplicationSettingService(IApplicationSettingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
           
        }
      
        public IEnumerable<ApplicationSetting> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.OfficeID);
            return entities;
        }

        public ApplicationSetting GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ApplicationSetting Create(ApplicationSetting objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ApplicationSetting objectToUpdate)
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
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == false)
                {
                    return false;
                }
            }

            return true;
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public ApplicationSetting Get(Expression<Func<ApplicationSetting, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ApplicationSetting> GetMany(Expression<Func<ApplicationSetting, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<ApplicationSetting>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<ApplicationSetting>> GetManyAsync(Expression<Func<ApplicationSetting, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<ApplicationSetting> GetAsync(Expression<Func<ApplicationSetting, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        public IEnumerable<ValidationResult> IsValidSettings(ApplicationSetting applicationsetting)
        {
            var entity = repository.Get(a => a.OfficeID == applicationsetting.OfficeID);

            if (entity != null)
            {
                yield return new ValidationResult("OfficeID", "Duplicate Record");
            }
        }


        public IEnumerable<ValidationResult> IsValidEdit(ApplicationSetting applicationsetting)
        {
            var entity = repository.Get(a => a.OfficeID == applicationsetting.OfficeID);

            if (entity == null)
            {
                yield return new ValidationResult("OfficeID", "Duplicate Record");
            }
        }


        public IEnumerable<DBApplicationSettingsDetail> GetApplicationDetailDetail(int? officeID)
        {
            return repository.GetApplicationDetailDetail(officeID);
        }
    }
}
