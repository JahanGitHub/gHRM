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


    public interface IApplicantReferenceInfoService : IServiceBase<ApplicantReferenceInfo>
    {
      
    }
    public class ApplicantReferenceInfoService : IApplicantReferenceInfoService
    {
        private readonly IApplicantReferenceInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApplicantReferenceInfoService(IApplicantReferenceInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public ApplicantReferenceInfo Create(ApplicantReferenceInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ApplicantReferenceInfo Get(Expression<Func<ApplicantReferenceInfo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicantReferenceInfo> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicantReferenceInfo>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicantReferenceInfo> GetAsync(Expression<Func<ApplicantReferenceInfo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public ApplicantReferenceInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<ApplicantReferenceInfo> GetMany(Expression<Func<ApplicantReferenceInfo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicantReferenceInfo>> GetManyAsync(Expression<Func<ApplicantReferenceInfo, bool>> where)
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

        public void Update(ApplicantReferenceInfo objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }
     
    }
}
