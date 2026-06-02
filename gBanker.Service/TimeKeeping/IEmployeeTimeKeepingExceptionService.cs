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
    public interface IEmployeeTimeKeepingExceptionService : IServiceBase<EmployeeTimeKeepingException>
    {


    }
    public class EmployeeTimeKeepingExceptionService : IEmployeeTimeKeepingExceptionService
    {
        private readonly IEmployeeTimeKeepingExceptionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeTimeKeepingExceptionService(IEmployeeTimeKeepingExceptionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeTimeKeepingException> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public EmployeeTimeKeepingException GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }       

        public EmployeeTimeKeepingException Create(EmployeeTimeKeepingException objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeTimeKeepingException objectToUpdate)
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

        public EmployeeTimeKeepingException Get(Expression<Func<EmployeeTimeKeepingException, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeTimeKeepingException> GetMany(Expression<Func<EmployeeTimeKeepingException, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeTimeKeepingException>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeTimeKeepingException>> GetManyAsync(Expression<Func<EmployeeTimeKeepingException, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeTimeKeepingException> GetAsync(Expression<Func<EmployeeTimeKeepingException, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
