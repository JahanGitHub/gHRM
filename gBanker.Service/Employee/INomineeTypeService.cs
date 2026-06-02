using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface INomineeTypeService : IServiceBase<NomineeType>
    {


    }
    public class NomineeTypeService : INomineeTypeService
    {
        private readonly INomineeTypeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public NomineeTypeService(INomineeTypeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<NomineeType> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.NomineeTypeId);
            return entities;
        }

        public NomineeType GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public NomineeType Create(NomineeType objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(NomineeType objectToUpdate)
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

        public NomineeType Get(Expression<Func<NomineeType, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<NomineeType> GetMany(Expression<Func<NomineeType, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<NomineeType>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<NomineeType>> GetManyAsync(Expression<Func<NomineeType, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<NomineeType> GetAsync(Expression<Func<NomineeType, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
