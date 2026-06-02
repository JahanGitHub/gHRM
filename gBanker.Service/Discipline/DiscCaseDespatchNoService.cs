using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository.Discipline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gHRM.Service.Discipline
{
    public interface IDiscCaseDespatchNoService : IServiceBase<DiscCaseDespatchNo>
    {

    }
    public class DiscCaseDespatchNoService : IDiscCaseDespatchNoService
    {
        private readonly IDiscCaseDespatchNoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DiscCaseDespatchNoService(IDiscCaseDespatchNoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DiscCaseDespatchNo> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.DespatchId);
            return entities;
        }

        public DiscCaseDespatchNo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }



        public DiscCaseDespatchNo Create(DiscCaseDespatchNo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DiscCaseDespatchNo objectToUpdate)
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

        public IEnumerable<DiscCaseDespatchNo> GetMany(Expression<Func<DiscCaseDespatchNo, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public DiscCaseDespatchNo Get(Expression<Func<DiscCaseDespatchNo, bool>> where)
        {
            throw new NotImplementedException();
        }



        public Task<IEnumerable<DiscCaseDespatchNo>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DiscCaseDespatchNo>> GetManyAsync(Expression<Func<DiscCaseDespatchNo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<DiscCaseDespatchNo> GetAsync(Expression<Func<DiscCaseDespatchNo, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
