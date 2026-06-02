using gHRM.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using gHRM.Data.CodeFirstMigration.eRecruit;
using gHRM.Data.Repository.eRecruitApplication;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Service.eRecruit
{

    public interface IApplicationInfoService : IServiceBase<ApplicationInfo>
    {
        IEnumerable<ApplicationInfo> GetListingByFilter(BaseSearchFilter filter);
        ApplicationInfo GetByNID(string nId);
        ApplicationInfo GetByEmpId(Int64 ApplicationId);
        ApplicationInfo GetByBirthRegistrationNo(string birthRegistrationNo);
        bool IsExistApplicationInfo(BaseSearchFilter filter);
    }
    public class ApplicationInfoService : IApplicationInfoService
    {
        private readonly IApplicationInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public ApplicationInfoService(IApplicationInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<ApplicationInfo> GetListingByFilter(BaseSearchFilter filter)
        {
            var listings = repository.GetListingByFilter(filter);
            return listings;
        }
        public ApplicationInfo GetByBirthRegistrationNo(string birthRegistrationNo)
        {
            var single = repository.GetByBirthRegistrationNo(birthRegistrationNo);
            return single;
        }

        public ApplicationInfo GetByNID(string nId)
        {
            var single = repository.GetByNID(nId);
            return single;

        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public ApplicationInfo Create(ApplicationInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }
        public void Update(ApplicationInfo objectToUpdate)
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
        public IEnumerable<ApplicationInfo> GetAll()
        {
            var entities = repository.GetAll().Where(x => x.IsActive == true).OrderBy(o => o.ApplicationId);
            return entities;
        }

        public ApplicationInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public ApplicationInfo GetById(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public ApplicationInfo Get(Expression<Func<ApplicationInfo, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ApplicationInfo> GetMany(Expression<Func<ApplicationInfo, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
        public ApplicationInfo GetByEmpId(Int64 applicationId)
        {
            var entity = repository.GetActiveApplicationInfo(applicationId);
            return entity;
        }

        public bool IsExistApplicationInfo(BaseSearchFilter filter)
        {
            var isExist = repository.IsExistApplicationInfo(filter);
            return isExist;
        }

        #region Asyc
        public virtual async Task<IEnumerable<ApplicationInfo>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        
        public virtual async Task<IEnumerable<ApplicationInfo>> GetManyAsync(Expression<Func<ApplicationInfo, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }

        public virtual async Task<ApplicationInfo> GetAsync(Expression<Func<ApplicationInfo, bool>> where)
        {
            return await repository.GetAsync(where);
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }


}
