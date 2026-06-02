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
    public interface IApplicantNomineeService : IServiceBase<ApplicantNominee>
    { }
    public class ApplicantNomineeService : IApplicantNomineeService
    {
        private readonly IApplicantNomineeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApplicantNomineeService(IApplicantNomineeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ApplicantNominee> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }
        public ApplicantNominee GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public void Save()
        {
            unitOfWork.Commit();
        }
        public ApplicantNominee Create(ApplicantNominee objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ApplicantNominee objectToUpdate)
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

        public ApplicantNominee Get(Expression<Func<ApplicantNominee, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ApplicantNominee> GetMany(Expression<Func<ApplicantNominee, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }
        #region Asyc
        public virtual async Task<IEnumerable<ApplicantNominee>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }
        public virtual async Task<IEnumerable<ApplicantNominee>> GetManyAsync(Expression<Func<ApplicantNominee, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }
        public virtual async Task<ApplicantNominee> GetAsync(Expression<Func<ApplicantNominee, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
