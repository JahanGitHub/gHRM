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
    public interface ILinkWithEmployeeService : IServiceBase<LinkWithEmployee>
    {
    }

    public class LinkWithEmployeeService : ILinkWithEmployeeService
    {
        private readonly ILinkWithEmployeeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public LinkWithEmployeeService(ILinkWithEmployeeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<LinkWithEmployee> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.LinkId);
            return entities;
        }

        public LinkWithEmployee GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public LinkWithEmployee Create(LinkWithEmployee objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LinkWithEmployee objectToUpdate)
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

        public LinkWithEmployee Get(Expression<Func<LinkWithEmployee, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<LinkWithEmployee> GetMany(Expression<Func<LinkWithEmployee, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<LinkWithEmployee>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<LinkWithEmployee>> GetManyAsync(Expression<Func<LinkWithEmployee, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<LinkWithEmployee> GetAsync(Expression<Func<LinkWithEmployee, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
