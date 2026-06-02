using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository;
using gHRM.Data.Repository.payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.payroll
{
    public interface IEmployeeMonthlySalaryExceptionService : IServiceBase<EmployeeMonthlySalaryException>
    {
        List<EmployeeMonthlySalaryException> AddEmplyoeeSalaryExceptionList(List<EmployeeMonthlySalaryException> objs);

    }
    public class EmployeeMonthlySalaryExceptionService : IEmployeeMonthlySalaryExceptionService
    {
        private readonly IEmployeeMonthlySalaryExceptionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeMonthlySalaryExceptionService(IEmployeeMonthlySalaryExceptionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeMonthlySalaryException> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public EmployeeMonthlySalaryException GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public EmployeeMonthlySalaryException Create(EmployeeMonthlySalaryException objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        //public void CreateRange(List<EmployeeMonthlySalaryException> objectsToCreate)
        //{
        //    repository.AddRange(objectsToCreate);
        //    Save();
        //    return objectToCreate;
        //}

        public void Update(EmployeeMonthlySalaryException objectToUpdate)
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

        public List<EmployeeMonthlySalaryException> AddEmplyoeeSalaryExceptionList(List<EmployeeMonthlySalaryException> objs)
        {
            repository.AddEmplyoeeSalaryExceptionList(objs);
            return objs;
        }

        public EmployeeMonthlySalaryException Get(Expression<Func<EmployeeMonthlySalaryException, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeMonthlySalaryException> GetMany(Expression<Func<EmployeeMonthlySalaryException, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeMonthlySalaryException>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeMonthlySalaryException>> GetManyAsync(Expression<Func<EmployeeMonthlySalaryException, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeMonthlySalaryException> GetAsync(Expression<Func<EmployeeMonthlySalaryException, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}

