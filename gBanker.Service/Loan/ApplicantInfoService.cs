using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Loan;
using gHRM.Data.Repository.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace gHRM.Service.Loan
{
    public interface IApplicantInfoService : IServiceBase<ApplicantInfo>
    { }
    public class ApplicantInfoService : IApplicantInfoService
    {
        private readonly IApplicantInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApplicantInfoService(IApplicantInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ApplicantInfo> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public ApplicantInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public ApplicantInfo Create(ApplicantInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ApplicantInfo objectToUpdate)
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
            //throw new NotImplementedException();
            var obj = repository.GetById(id);
            if (obj != null)
            {
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }
        public bool IsContinued(long id)
        {
            // throw new NotImplementedException();
            var obj = repository.GetById(id);
            if (obj != null)
            {

            }
            return true;
        }

        public ApplicantInfo Get(Expression<Func<ApplicantInfo, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ApplicantInfo> GetMany(Expression<Func<ApplicantInfo, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<ApplicantInfo>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<ApplicantInfo>> GetManyAsync(Expression<Func<ApplicantInfo, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<ApplicantInfo> GetAsync(Expression<Func<ApplicantInfo, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
