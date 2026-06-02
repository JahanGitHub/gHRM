using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gHRM.Service
{
    public interface INomineeRelationService : IServiceBase<NomineeRelation>
    {

    }
    public class NomineeRelationService : INomineeRelationService
    {
        private readonly INomineeRelationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public NomineeRelationService(INomineeRelationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<NomineeRelation> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive = true).OrderBy(c => c.RelationName);
            return entities;
        }

        public NomineeRelation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public NomineeRelation Create(NomineeRelation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(NomineeRelation objectToUpdate)
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


        public NomineeRelation Get(Expression<Func<NomineeRelation, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<NomineeRelation> GetMany(Expression<Func<NomineeRelation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<NomineeRelation>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<NomineeRelation>> GetManyAsync(Expression<Func<NomineeRelation, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<NomineeRelation> GetAsync(Expression<Func<NomineeRelation, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
