using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IPRComponentGroupService : IServiceBase<PRComponentGroup>
    {

    }

    public class PRComponentGroupService : IPRComponentGroupService
    {
        private readonly IPRComponentGroupRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public PRComponentGroupService(IPRComponentGroupRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<PRComponentGroup> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.PRComponentGroupID);
            return entities;
        }

        public PRComponentGroup GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public PRComponentGroup Create(PRComponentGroup objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(PRComponentGroup objectToUpdate)
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

        public PRComponentGroup Get(Expression<Func<PRComponentGroup, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<PRComponentGroup> GetMany(Expression<Func<PRComponentGroup, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<PRComponentGroup>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<PRComponentGroup>> GetManyAsync(Expression<Func<PRComponentGroup, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<PRComponentGroup> GetAsync(Expression<Func<PRComponentGroup, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
