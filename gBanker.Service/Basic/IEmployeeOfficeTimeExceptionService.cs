
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using gHRM.Data.Repository;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Service
{
    public interface IEmployeeOfficeTimeExceptionService : IServiceBase<EmployeeOfficeTimeException>
    {

    }

    public class EmployeeOfficeTimeExceptionService : IEmployeeOfficeTimeExceptionService
    {
        private readonly IEmployeeOfficeTimeExceptionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeOfficeTimeExceptionService(IEmployeeOfficeTimeExceptionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeOfficeTimeException> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Id);
            return entities;
        }

        public EmployeeOfficeTimeException GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeOfficeTimeException Create(EmployeeOfficeTimeException objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeOfficeTimeException objectToUpdate)
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
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public EmployeeOfficeTimeException Get(Expression<Func<EmployeeOfficeTimeException, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeOfficeTimeException> GetMany(Expression<Func<EmployeeOfficeTimeException, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeOfficeTimeException>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeOfficeTimeException>> GetManyAsync(Expression<Func<EmployeeOfficeTimeException, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeOfficeTimeException> GetAsync(Expression<Func<EmployeeOfficeTimeException, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}

