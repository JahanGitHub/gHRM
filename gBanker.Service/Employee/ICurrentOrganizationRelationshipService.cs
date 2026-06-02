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
    public interface ICurrentOrganizationRelationshipService : IServiceBase<CurrentOrganizationRelationship>
    {


    }
    public class CurrentOrganizationRelationshipService : ICurrentOrganizationRelationshipService
    {
        private readonly ICurrentOrganizationRelationshipRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public CurrentOrganizationRelationshipService(ICurrentOrganizationRelationshipRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<CurrentOrganizationRelationship> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.SelfOrgRelationId);
            return entities;
        }

        public CurrentOrganizationRelationship GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public CurrentOrganizationRelationship Create(CurrentOrganizationRelationship objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(CurrentOrganizationRelationship objectToUpdate)
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

        public CurrentOrganizationRelationship Get(Expression<Func<CurrentOrganizationRelationship, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<CurrentOrganizationRelationship> GetMany(Expression<Func<CurrentOrganizationRelationship, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<CurrentOrganizationRelationship>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<CurrentOrganizationRelationship>> GetManyAsync(Expression<Func<CurrentOrganizationRelationship, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<CurrentOrganizationRelationship> GetAsync(Expression<Func<CurrentOrganizationRelationship, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
