using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
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
    public interface IDiscCaseDealingOfficerService : IServiceBase<DiscCaseDealingOfficer>
    {

    }
    public class DiscCaseDealingOfficerService : IDiscCaseDealingOfficerService
    {
        private readonly IDiscCaseDealingOfficerRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;


        public DiscCaseDealingOfficerService(IDiscCaseDealingOfficerRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DiscCaseDealingOfficer> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.EmployeeId);
            return entities;
        }


        public DiscCaseDealingOfficer GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DiscCaseDealingOfficer Create(DiscCaseDealingOfficer objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscCaseDealingOfficer objectToUpdate)
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

        public IEnumerable<DiscCaseDealingOfficer> GetMany(Expression<Func<DiscCaseDealingOfficer, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public DiscCaseDealingOfficer Get(Expression<Func<DiscCaseDealingOfficer, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCaseDealingOfficer>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCaseDealingOfficer>> GetManyAsync(Expression<Func<DiscCaseDealingOfficer, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscCaseDealingOfficer> GetAsync(Expression<Func<DiscCaseDealingOfficer, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
