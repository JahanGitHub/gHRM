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
    public interface IApplicantInfoService3 : IServiceBase<ApplicantInfo2>
    { }
    public class ApplicantInfoService3 : IApplicantInfoService3
    {
        private readonly IApplicantInfoRepository2 repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApplicantInfoService3(IApplicantInfoRepository2 repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ApplicantInfo2> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public ApplicantInfo2 GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public ApplicantInfo2 Create(ApplicantInfo2 objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ApplicantInfo2 objectToUpdate)
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

        public ApplicantInfo2 Get(Expression<Func<ApplicantInfo2, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ApplicantInfo2> GetMany(Expression<Func<ApplicantInfo2, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<ApplicantInfo2>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<ApplicantInfo2>> GetManyAsync(Expression<Func<ApplicantInfo2, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<ApplicantInfo2> GetAsync(Expression<Func<ApplicantInfo2, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
