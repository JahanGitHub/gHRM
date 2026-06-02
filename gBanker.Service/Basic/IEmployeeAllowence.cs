using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Data.Repository.Basic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Basic
{
   public interface IEmployeeAllowenceService : IServiceBase<EmployeeAllowence>
    {
        IEnumerable<EmployeeAllowanceCommonClass> GetAllowanceList();
    }

    public class EmployeeAllowenceService : IEmployeeAllowenceService
    {
        private readonly IEmployeeAllowenceRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;


        public EmployeeAllowenceService(IEmployeeAllowenceRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }


        public EmployeeAllowence Create(EmployeeAllowence objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public EmployeeAllowence Get(Expression<Func<EmployeeAllowence, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EmployeeAllowence> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public virtual async Task<IEnumerable<EmployeeAllowence>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public Task<EmployeeAllowence> GetAsync(Expression<Func<EmployeeAllowence, bool>> where)
        {
            throw new NotImplementedException();
        }

        public EmployeeAllowence GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<EmployeeAllowence> GetMany(Expression<Func<EmployeeAllowence, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;

        }

        public Task<IEnumerable<EmployeeAllowence>> GetManyAsync(Expression<Func<EmployeeAllowence, bool>> where)
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

        public void Update(EmployeeAllowence objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }     

        public IEnumerable<EmployeeAllowanceCommonClass> GetAllowanceList()
        {
            return repository.GetAllAllowanceCollection();
        }
    }
}
