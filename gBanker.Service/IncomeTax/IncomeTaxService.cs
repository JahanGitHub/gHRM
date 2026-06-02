using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;

namespace gHRM.Service
{
    public interface IIncomeTaxService : IServiceBase<IncomeTax>
    {
        IncomeTax GetByEmployeeID(long employeeID);
        bool Inactivate(long employeeID, DateTime? inactiveDate);
        bool IsContinued(long employeeID);
        IEnumerable<IncomeTax> GetMany(Expression<Func<IncomeTax, bool>> where);
    }
    public class IncomeTaxService : IIncomeTaxService
    {
        private readonly IIncomeTaxRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public IncomeTaxService(IIncomeTaxRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<IncomeTax> GetAll()
        {
            return repository.GetAll().OrderBy(x => x.EmployeeID);
        }

        public IncomeTax GetById(int id)
        {
            return repository.GetById(id);
        }

        public IncomeTax GetByEmployeeID(long employeeID)
        {
            return repository.Get(e => e.EmployeeID == employeeID);
        }

        public IncomeTax Create(IncomeTax objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(IncomeTax objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            if (entity != null)
            {
                repository.Delete(entity);
                Save();
            }
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.CreateDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
                // Add your own `IsActive` column if applicable
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }

        public bool IsContinued(long id)
        {
            var obj = repository.GetById(id);
            // Modify as per your logic (e.g., check if inactive)
            return obj != null;
        }

        public IEnumerable<IncomeTax> GetMany(Expression<Func<IncomeTax, bool>> where)
        {
            return repository.GetMany(where);
        }

        public IncomeTax Get(Expression<Func<IncomeTax, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IncomeTax>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IncomeTax>> GetManyAsync(Expression<Func<IncomeTax, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IncomeTax> GetAsync(Expression<Func<IncomeTax, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
