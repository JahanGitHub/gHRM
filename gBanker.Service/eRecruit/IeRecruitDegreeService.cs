using gHRM.Data.CodeFirstMigration.eRecruit;
using gHRM.Core.Filters.eRecruit;
//using eRecruitment.Infrastructure.Repository.CacheManagerRepositories;
//using eRecruitment.Infrastructure.Service.CacheManagerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository.eRecruitApplication;
using gHRM.Core.Filters;
using eRecruitment.Infrastructure.Service.CacheManagerServices;

namespace gHRM.Service.eRecruit
{
    public interface IeRecruitDegreeService : IServiceBase<EducationDegree>
    {
        IEnumerable<EducationDegree> GetEducationDegreeListByFilter(BaseSearchFilter filter);

     
    }
    public class eRecruitDegreeService : IeRecruitDegreeService
    {
        private readonly IeRecruitDegreeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly ICacheManagerService cacheManagerService;

        public eRecruitDegreeService(IeRecruitDegreeRepository repository,
            ICacheManagerService cacheManagerService,
            IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.cacheManagerService = cacheManagerService;
        }

         

        public void Save()
        {
            unitOfWork.Commit();
        }
        public EducationDegree Create(EducationDegree objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }
        public void Update(EducationDegree objectToUpdate)
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
        public IEnumerable<EducationDegree> GetAll()
        {
            var entities = repository.GetAll().Where(x => x.IsActive == true).OrderBy(o => o.DegreeId);
            return entities;
        }

        public EducationDegree GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public EducationDegree GetById(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public EducationDegree Get(Expression<Func<EducationDegree, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EducationDegree> GetMany(Expression<Func<EducationDegree, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EducationDegree>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EducationDegree>> GetManyAsync(Expression<Func<EducationDegree, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EducationDegree> GetAsync(Expression<Func<EducationDegree, bool>> where)
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

        public IEnumerable<EducationDegree> GetEducationDegreeListByFilter(BaseSearchFilter filter)
        {
            throw new NotImplementedException();
        }



        #endregion
    }

}
