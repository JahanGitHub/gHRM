using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.Repository;

namespace gHRM.Service
{
    public interface IEmployeeNomineeService : IServiceBase<EmployeeNominee>
    {
        EmployeeNominee GetByGurId(int NomineeId);
        EmployeeNominee GetByDetailId(Int64 DetailId);
    }

    public class EmployeeNomineeService : IEmployeeNomineeService
    {
        private readonly IEmployeeNomineeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;


        public EmployeeNominee GetByGurId(int NomineeId)
        {
            var entity = repository.Get(e => e.NomineeId == NomineeId && e.IsActive == true);
            return entity;
        }

        public EmployeeNomineeService(IEmployeeNomineeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeNominee> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.NomineeId);
            return entities;
        }

        public EmployeeNominee GetByDetailId(Int64 DetailId)
        {
            var entity = repository.Get(e => e.NomineeId == DetailId && e.IsActive == true);
            return entity;
        }

        public EmployeeNominee GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public EmployeeNominee Create(EmployeeNominee objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeNominee objectToUpdate)
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

        public void Save()
        {
            unitOfWork.Commit();
        }
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public EmployeeNominee Get(Expression<Func<EmployeeNominee, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeNominee> GetMany(Expression<Func<EmployeeNominee, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeNominee>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeNominee>> GetManyAsync(Expression<Func<EmployeeNominee, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }


        public virtual async Task<EmployeeNominee> GetAsync(Expression<Func<EmployeeNominee, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
