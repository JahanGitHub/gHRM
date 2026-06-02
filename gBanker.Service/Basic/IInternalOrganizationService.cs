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
    public interface IInternalOrganizationService : IServiceBase<InternalOrganization>
    {


    }
    public class InternalOrganizationService : IInternalOrganizationService
    {
        private readonly IInternalOrganizationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public InternalOrganizationService(IInternalOrganizationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<InternalOrganization> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.OrgId);
            return entities;
        }

        public InternalOrganization GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public InternalOrganization Create(InternalOrganization objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(InternalOrganization objectToUpdate)
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

        public InternalOrganization Get(Expression<Func<InternalOrganization, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<InternalOrganization> GetMany(Expression<Func<InternalOrganization, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<InternalOrganization>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<InternalOrganization>> GetManyAsync(Expression<Func<InternalOrganization, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<InternalOrganization> GetAsync(Expression<Func<InternalOrganization, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
