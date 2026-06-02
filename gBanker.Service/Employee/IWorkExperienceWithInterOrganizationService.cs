using gHRM.Core.Common;
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
    public interface IWorkExperienceWithInterOrganizationService : IServiceBase<WorkExperienceWithInterOrganization>
    {


    }
    public class WorkExperienceWithInterOrganizationService : IWorkExperienceWithInterOrganizationService
    {
        private readonly IWorkExperienceWithInterOrganizationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public WorkExperienceWithInterOrganizationService(IWorkExperienceWithInterOrganizationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<WorkExperienceWithInterOrganization> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.WorkExpId);
            return entities;
        }

        public WorkExperienceWithInterOrganization GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public WorkExperienceWithInterOrganization Create(WorkExperienceWithInterOrganization objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(WorkExperienceWithInterOrganization objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public WorkExperienceWithInterOrganization Get(Expression<Func<WorkExperienceWithInterOrganization, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<WorkExperienceWithInterOrganization> GetMany(Expression<Func<WorkExperienceWithInterOrganization, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<WorkExperienceWithInterOrganization>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<WorkExperienceWithInterOrganization>> GetManyAsync(Expression<Func<WorkExperienceWithInterOrganization, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<WorkExperienceWithInterOrganization> GetAsync(Expression<Func<WorkExperienceWithInterOrganization, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
