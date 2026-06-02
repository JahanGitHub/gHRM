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
    public interface IDepriciationRateChangeService : IServiceBase<DepriciationRateChange>
    {


    }
    public class DepriciationRateChangeService : IDepriciationRateChangeService
    {
        private readonly IDepriciationRateChangeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DepriciationRateChangeService(IDepriciationRateChangeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<DepriciationRateChange> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.DepRateChangeID);
            return entities;
        }

        public DepriciationRateChange GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DepriciationRateChange Create(DepriciationRateChange objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DepriciationRateChange objectToUpdate)
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

        public DepriciationRateChange Get(Expression<Func<DepriciationRateChange, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<DepriciationRateChange> GetMany(Expression<Func<DepriciationRateChange, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
        public DepriciationRateChange GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DepriciationRateChange>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DepriciationRateChange>> GetManyAsync(Expression<Func<DepriciationRateChange, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DepriciationRateChange> GetAsync(Expression<Func<DepriciationRateChange, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
