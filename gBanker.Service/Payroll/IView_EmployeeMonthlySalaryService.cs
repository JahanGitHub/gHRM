using gHRM.Core.Common;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.Repository;
using gHRM.Data.Repository.payroll;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IView_EmployeeMonthlySalaryService : IServiceBase<View_EmployeeMonthlySalary>
    {


    }
    public class View_EmployeeMonthlySalaryService : IView_EmployeeMonthlySalaryService
    {
        private readonly IView_EmployeeMonthlySalaryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public View_EmployeeMonthlySalaryService(IView_EmployeeMonthlySalaryRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<View_EmployeeMonthlySalary> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.EmployeeId);
            return entities;
        }

        public View_EmployeeMonthlySalary GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public View_EmployeeMonthlySalary Create(View_EmployeeMonthlySalary objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(View_EmployeeMonthlySalary objectToUpdate)
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

        public View_EmployeeMonthlySalary Get(Expression<Func<View_EmployeeMonthlySalary, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<View_EmployeeMonthlySalary> GetMany(Expression<Func<View_EmployeeMonthlySalary, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<View_EmployeeMonthlySalary>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<View_EmployeeMonthlySalary>> GetManyAsync(Expression<Func<View_EmployeeMonthlySalary, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<View_EmployeeMonthlySalary> GetAsync(Expression<Func<View_EmployeeMonthlySalary, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
