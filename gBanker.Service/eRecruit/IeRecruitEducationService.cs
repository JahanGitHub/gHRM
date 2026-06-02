using gHRM.Data.CodeFirstMigration.eRecruit;
using gHRM.Core;
using gHRM.Data.Repository.eRecruitApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using gHRM.Core.Filters.eRecruit;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using gHRM.Core.Filters;

namespace gHRM.Service.eRecruit
{
    public interface IeRecruitEducationService : IServiceBase<eRecruitEmployeeEducation>
    {
        IEnumerable<eRecruitEmployeeEducation> GetEmployeeEducationsByFilterByFilter(BaseSearchFilter filter);
        eRecruitEmployeeEducation GetEmployeeEducationInfoByFilter(BaseSearchFilter filter);
    }
    public class eRecruitEducationService : IeRecruitEducationService
    {
        private readonly IeRecruitEducationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public eRecruitEducationService(IeRecruitEducationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<eRecruitEmployeeEducation> GetEmployeeEducationsByFilterByFilter(BaseSearchFilter filter)
        {
            var listings = repository.GetEmployeeEducationsByFilterByFilter(filter);
            return listings;
        }
        public eRecruitEmployeeEducation GetEmployeeEducationInfoByFilter(BaseSearchFilter filter)
        {
            var single = repository.GetEmployeeEducationInfoByFilter(filter);
            return single;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public eRecruitEmployeeEducation Create(eRecruitEmployeeEducation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }
        public void Update(eRecruitEmployeeEducation objectToUpdate)
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
        public IEnumerable<eRecruitEmployeeEducation> GetAll()
        {
            var entities = repository.GetAll().Where(x => x.IsActive == true).OrderBy(o => o.EducationId);
            return entities;
        }

        public eRecruitEmployeeEducation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public eRecruitEmployeeEducation GetById(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public eRecruitEmployeeEducation Get(Expression<Func<eRecruitEmployeeEducation, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<eRecruitEmployeeEducation> GetMany(Expression<Func<eRecruitEmployeeEducation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<eRecruitEmployeeEducation>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<eRecruitEmployeeEducation>> GetManyAsync(Expression<Func<eRecruitEmployeeEducation, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<eRecruitEmployeeEducation> GetAsync(Expression<Func<eRecruitEmployeeEducation, bool>> where)
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
