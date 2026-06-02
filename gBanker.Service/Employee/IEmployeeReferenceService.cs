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
    public interface IEmployeeReferenceService : IServiceBase<EmployeeReference>
    {

        //IEnumerable<ValidationResult> IsValidEmployee(Employee employee);
        //IEnumerable<Employee> SearchEmployee();

        IEnumerable<EmployeeReference> GetByEmployeeId(Int64 EmployeeId);
        EmployeeReference GetByReferenceId(Int64 referenceId);
    }
    public class EmployeeReferenceService : IEmployeeReferenceService
    {
        private readonly IEmployeeReferencesRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeReferenceService(IEmployeeReferencesRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeReference> GetAll()
        {
            var entities = repository.GetAll().Where(w => w.IsActive == true).OrderBy(c => c.ReferenceId);
            return entities;
        }

        public EmployeeReference GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeReference GetByReferenceId(Int64 referenceId)
        {
            var entity = repository.Get(e => e.ReferenceId == referenceId && e.IsActive == true);
            return entity;
        }
        public IEnumerable<EmployeeReference> GetByEmployeeId(Int64 EmployeeId)
        {
            var entity = repository.GetAll().Where(w => w.EmployeeId == EmployeeId && w.IsActive == true);
            return entity;
        }
        public EmployeeReference Create(EmployeeReference objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeReference objectToUpdate)
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

        public EmployeeReference Get(Expression<Func<EmployeeReference, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeReference> GetMany(Expression<Func<EmployeeReference, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeReference>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeReference>> GetManyAsync(Expression<Func<EmployeeReference, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeReference> GetAsync(Expression<Func<EmployeeReference, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
        //public IEnumerable<Employee> SearchEmployee()
        //{
        //    return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.EmployeeId);
        //}


        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }


        public bool IsContinued(long id)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == false)
                {
                    return false;
                }
            }

            return true;
        }



        //IEnumerable<ValidationResult> IEmployeeService.IsValidEmployee(Employee employee)
        //{
        //    var entity = repository.Get(p => p.EmployeeCode == employee.EmployeeCode);
        //    if (entity != null)
        //    {

        //        yield return new ValidationResult("EmployeeCode", "Duplicate Employee.");

        //    }
        //}
    }
}
