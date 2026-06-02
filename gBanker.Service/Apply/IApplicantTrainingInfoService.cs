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
 
    public interface IApplicantTrainingInfoService : IServiceBase<ApplicantTrainingInfo>
    {
        
    }
    public class ApplicantTrainingInfoService : IApplicantTrainingInfoService
    {
        private readonly IApplicantTrainingInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApplicantTrainingInfoService(IApplicantTrainingInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public ApplicantTrainingInfo Create(ApplicantTrainingInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ApplicantTrainingInfo Get(Expression<Func<ApplicantTrainingInfo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicantTrainingInfo> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicantTrainingInfo>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicantTrainingInfo> GetAsync(Expression<Func<ApplicantTrainingInfo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public ApplicantTrainingInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<ApplicantTrainingInfo> GetMany(Expression<Func<ApplicantTrainingInfo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicantTrainingInfo>> GetManyAsync(Expression<Func<ApplicantTrainingInfo, bool>> where)
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

        public void Update(ApplicantTrainingInfo objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();

        }

       
    }
}
