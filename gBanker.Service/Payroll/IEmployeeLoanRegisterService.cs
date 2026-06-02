using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IEmployeeLoanRegisterService : IServiceBase<EmployeeLoanRegister>
    {

    }
    public class EmployeeLoanRegisterService : IEmployeeLoanRegisterService
    {
        private readonly IEmployeeLoanRegisterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeLoanRegisterService(IEmployeeLoanRegisterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<EmployeeLoanRegister> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }

        public EmployeeLoanRegister GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeLoanRegister Create(EmployeeLoanRegister objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeLoanRegister objectToUpdate)
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
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }


        public EmployeeLoanRegister Get(Expression<Func<EmployeeLoanRegister, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeLoanRegister> GetMany(Expression<Func<EmployeeLoanRegister, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeLoanRegister>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeLoanRegister>> GetManyAsync(Expression<Func<EmployeeLoanRegister, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeLoanRegister> GetAsync(Expression<Func<EmployeeLoanRegister, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
