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
    public interface IEmployeeEquivalentDesignationService : IServiceBase<EmployeeEquivalentDesignation>
    {


    }
    public class EmployeeEquivalentDesignationService : IEmployeeEquivalentDesignationService
    {
        private readonly IEmployeeEquivalentDesignationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeEquivalentDesignationService(IEmployeeEquivalentDesignationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeEquivalentDesignation> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.EquivalentDesigId);
            return entities;
        }

        public EmployeeEquivalentDesignation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeEquivalentDesignation Create(EmployeeEquivalentDesignation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeEquivalentDesignation objectToUpdate)
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

        public EmployeeEquivalentDesignation Get(Expression<Func<EmployeeEquivalentDesignation, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeEquivalentDesignation> GetMany(Expression<Func<EmployeeEquivalentDesignation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeEquivalentDesignation>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeEquivalentDesignation>> GetManyAsync(Expression<Func<EmployeeEquivalentDesignation, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeEquivalentDesignation> GetAsync(Expression<Func<EmployeeEquivalentDesignation, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
