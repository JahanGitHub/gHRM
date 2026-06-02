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
    public interface IEmployeePublicationService : IServiceBase<EmployeePublication>
    {


    }
    public class EmployeePublicationService : IEmployeePublicationService
    {
        private readonly IEmployeePublicationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeePublicationService(IEmployeePublicationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeePublication> GetAll() 
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.PublicationId);
            return entities;
        }

        public EmployeePublication GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeePublication Create(EmployeePublication objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeePublication objectToUpdate)
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

        public EmployeePublication Get(Expression<Func<EmployeePublication, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeePublication> GetMany(Expression<Func<EmployeePublication, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeePublication>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeePublication>> GetManyAsync(Expression<Func<EmployeePublication, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeePublication> GetAsync(Expression<Func<EmployeePublication, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}

