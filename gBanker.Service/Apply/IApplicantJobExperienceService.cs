using gHRM.Core.Common;
using gHRM.Core.Filters.Offices;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Apply;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using gHRM.Data.Repository.Apply;
using gHRM.Data.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Apply
{
    public interface IApplicantJobExperienceService : IServiceBase<ApplicantJobExperience>
    {
      
    }

    public class ApplicantJobExperienceService : IApplicantJobExperienceService
    {
        private readonly IApplicantJobExperienceRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApplicantJobExperienceService(IApplicantJobExperienceRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }


        public ApplicantJobExperience Create(ApplicantJobExperience objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }



        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public ApplicantJobExperience Get(Expression<Func<ApplicantJobExperience, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicantJobExperience> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicantJobExperience>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicantJobExperience> GetAsync(Expression<Func<ApplicantJobExperience, bool>> where)
        {
            throw new NotImplementedException();
        }

        public ApplicantJobExperience GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
            throw new NotImplementedException();
        }


        public IEnumerable<ApplicantJobExperience> GetMany(Expression<Func<ApplicantJobExperience, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicantJobExperience>> GetManyAsync(Expression<Func<ApplicantJobExperience, bool>> where)
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
            //throw new NotImplementedException();
            unitOfWork.Commit();
        }

        public void Update(ApplicantJobExperience objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

      
    }
}
