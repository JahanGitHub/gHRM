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
    public interface IApplicantMasterService : IServiceBase<ApplicantMaster>
    {
        ApplicantMaster GetByUserId(long? userId);
    }
    public class ApplicantMasterService : IApplicantMasterService
    {
        private readonly IApplicantMasterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApplicantMasterService(IApplicantMasterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }


        public ApplicantMaster Create(ApplicantMaster objectToCreate)
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

        public ApplicantMaster Get(Expression<Func<ApplicantMaster, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicantMaster> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicantMaster>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicantMaster> GetAsync(Expression<Func<ApplicantMaster, bool>> where)
        {
            throw new NotImplementedException();
        }

        public ApplicantMaster GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
            throw new NotImplementedException();
        }

        public ApplicantMaster GetByUserId(long? UserId)
        {
            var entity = repository.Get(p => p.UserId == UserId);
            return entity;
        }

        public IEnumerable<ApplicantMaster> GetMany(Expression<Func<ApplicantMaster, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicantMaster>> GetManyAsync(Expression<Func<ApplicantMaster, bool>> where)
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

        public void Update(ApplicantMaster objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }
    }
}
