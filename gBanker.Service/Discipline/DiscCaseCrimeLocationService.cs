using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using gHRM.Data.Repository;
using gHRM.Data.Repository.Discipline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Discipline
{

    public interface IDiscCaseCrimeLocationService : IServiceBase<DiscCaseCrimeLocation>
    {

    }
    public class DiscCaseCrimeLocationService : IDiscCaseCrimeLocationService
    {
        private readonly IDiscCaseCrimeLocationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DiscCaseCrimeLocationService(IDiscCaseCrimeLocationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DiscCaseCrimeLocation> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.DiscCaseCrimeLocationId);
            return entities;
        }

        public DiscCaseCrimeLocation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DiscCaseCrimeLocation Create(DiscCaseCrimeLocation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscCaseCrimeLocation objectToUpdate)
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
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DiscCaseCrimeLocation> GetMany(Expression<Func<DiscCaseCrimeLocation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public DiscCaseCrimeLocation Get(Expression<Func<DiscCaseCrimeLocation, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCaseCrimeLocation>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCaseCrimeLocation>> GetManyAsync(Expression<Func<DiscCaseCrimeLocation, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscCaseCrimeLocation> GetAsync(Expression<Func<DiscCaseCrimeLocation, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
