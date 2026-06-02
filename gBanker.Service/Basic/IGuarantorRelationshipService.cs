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
    public interface IGuarantorRelationshipService : IServiceBase<GuarantorRelationship>
    {


    }
    public class GuarantorRelationshipService : IGuarantorRelationshipService
    {
        private readonly IGuarantorRelationshipRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public GuarantorRelationshipService(IGuarantorRelationshipRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<GuarantorRelationship> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.GuarantorRelationshipId);
            return entities;
        }

        public GuarantorRelationship GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public GuarantorRelationship Create(GuarantorRelationship objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(GuarantorRelationship objectToUpdate)
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

        public GuarantorRelationship Get(Expression<Func<GuarantorRelationship, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<GuarantorRelationship> GetMany(Expression<Func<GuarantorRelationship, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<GuarantorRelationship>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<GuarantorRelationship>> GetManyAsync(Expression<Func<GuarantorRelationship, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<GuarantorRelationship> GetAsync(Expression<Func<GuarantorRelationship, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
