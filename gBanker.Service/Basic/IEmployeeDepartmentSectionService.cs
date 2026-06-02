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
    public interface IEmployeeDepartmentSectionService : IServiceBase<EmployeeDepartmentSection>
    {


    }
    public class EmployeeDepartmentSectionService : IEmployeeDepartmentSectionService
    {
        private readonly IEmployeeDepartmentSectionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeDepartmentSectionService(IEmployeeDepartmentSectionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeDepartmentSection> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.SectionId);
            return entities;
        }

        public EmployeeDepartmentSection GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeDepartmentSection Create(EmployeeDepartmentSection objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeDepartmentSection objectToUpdate)
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

        public EmployeeDepartmentSection Get(Expression<Func<EmployeeDepartmentSection, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeDepartmentSection> GetMany(Expression<Func<EmployeeDepartmentSection, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeDepartmentSection>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeDepartmentSection>> GetManyAsync(Expression<Func<EmployeeDepartmentSection, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeDepartmentSection> GetAsync(Expression<Func<EmployeeDepartmentSection, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}

