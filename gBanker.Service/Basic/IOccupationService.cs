using gHRM.Core.Common;
using gHRM.Data;
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
    public interface IOccupationService : IServiceBase<Occupation>
    {


    }
    public class OccupationService : IOccupationService
    {
        private readonly IOccupationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public OccupationService(IOccupationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Occupation> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.OccupationId);
            return entities;
        }

        public Occupation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Occupation Create(Occupation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Occupation objectToUpdate)
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

        public Occupation Get(Expression<Func<Occupation, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<Occupation> GetMany(Expression<Func<Occupation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<Occupation>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<Occupation>> GetManyAsync(Expression<Func<Occupation, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<Occupation> GetAsync(Expression<Func<Occupation, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
