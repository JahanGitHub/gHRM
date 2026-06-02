
using System.ComponentModel;
using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gHRM.Service
{
    public interface IFamilyRelationService : IServiceBase<FamilyRelation> 
    {
        
    }
    public class FamilyRelationService : IFamilyRelationService
    {
        private readonly IFamilyRelationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public FamilyRelationService(IFamilyRelationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<FamilyRelation> GetAll()
        {
            var entities = repository.GetAll().Where(c=>c.IsActive=true).OrderBy(c => c.RelationName);
            return entities;
        }

        public FamilyRelation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public FamilyRelation Create(FamilyRelation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(FamilyRelation objectToUpdate)
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


        public FamilyRelation Get(Expression<Func<FamilyRelation, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<FamilyRelation> GetMany(Expression<Func<FamilyRelation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<FamilyRelation>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<FamilyRelation>> GetManyAsync(Expression<Func<FamilyRelation, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<FamilyRelation> GetAsync(Expression<Func<FamilyRelation, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
