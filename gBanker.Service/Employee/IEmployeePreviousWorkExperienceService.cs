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
    public interface IEmployeePreviousWorkExperienceService : IServiceBase<EmployeePreviousWorkExperience>
    {

        //
    }
    public class EmployeePreviousWorkExperienceService : IEmployeePreviousWorkExperienceService
    {
        private readonly IEmployeePreviousWorkExperienceRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeePreviousWorkExperienceService(IEmployeePreviousWorkExperienceRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeePreviousWorkExperience> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.OrgId);
            return entities;
        }

        public EmployeePreviousWorkExperience GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeePreviousWorkExperience Create(EmployeePreviousWorkExperience objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeePreviousWorkExperience objectToUpdate)
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

        public EmployeePreviousWorkExperience Get(Expression<Func<EmployeePreviousWorkExperience, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeePreviousWorkExperience> GetMany(Expression<Func<EmployeePreviousWorkExperience, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeePreviousWorkExperience>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeePreviousWorkExperience>> GetManyAsync(Expression<Func<EmployeePreviousWorkExperience, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeePreviousWorkExperience> GetAsync(Expression<Func<EmployeePreviousWorkExperience, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}

