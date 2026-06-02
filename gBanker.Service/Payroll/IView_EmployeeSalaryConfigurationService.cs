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
    public interface IView_EmployeeSalaryConfigurationService : IServiceBase<View_EmployeeSalaryConfiguration>
    {
        //IEnumerable<ValidationResult> IsValidView_EmployeeSalaryConfiguration(string View_EmployeeSalaryConfigurationCode);
        // IEnumerable<View_EmployeeSalaryConfiguration> SearchView_EmployeeSalaryConfiguration();

        List<View_EmployeeSalaryConfiguration> GetEmployeeSalaryConfigurationList(long employeeId);

        List<View_EmployeeSalaryConfiguration> GetEmployeeSalaryConfigurationListbyCode(string employeeCode);
    }
    public class View_EmployeeSalaryConfigurationService : IView_EmployeeSalaryConfigurationService
    {
        private readonly IView_EmployeeSalaryConfigurationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public List<View_EmployeeSalaryConfiguration> GetEmployeeSalaryConfigurationList(long employeeId)
        {
            return repository.GetAll().Where(p => p.IsActive == true && p.EmployeeID == employeeId).ToList();
        }

        public List<View_EmployeeSalaryConfiguration> GetEmployeeSalaryConfigurationListbyCode(string employeeCode)
        {
            return repository.GetEmployeeSalaryConfigurationListbyCode(employeeCode);
        }

        public View_EmployeeSalaryConfigurationService(IView_EmployeeSalaryConfigurationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<View_EmployeeSalaryConfiguration> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.EmployeeID);
            return entities;
        }


        public View_EmployeeSalaryConfiguration GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        
        public View_EmployeeSalaryConfiguration Create(View_EmployeeSalaryConfiguration objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(View_EmployeeSalaryConfiguration objectToUpdate)
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
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<View_EmployeeSalaryConfiguration> SearchView_EmployeeSalaryConfiguration()
        {
            //return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.InvestorID);
            return repository.GetMany(g => g.IsActive == true).OrderBy(o => o.EmployeeID);
        }

        //IEnumerable<ValidationResult> IView_EmployeeSalaryConfigurationService.IsValidView_EmployeeSalaryConfiguration(string View_EmployeeSalaryConfigurationCode)
        //{
        //    var entity = repository.Get(p => p.View_EmployeeSalaryConfigurationShortCode == View_EmployeeSalaryConfigurationCode);
        //    if (entity != null)
        //    {
        //        yield return new ValidationResult("View_EmployeeSalaryConfigurationCode", "Duplicate View_EmployeeSalaryConfiguration Code.");

        //    }
        //}

        public View_EmployeeSalaryConfiguration Get(Expression<Func<View_EmployeeSalaryConfiguration, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<View_EmployeeSalaryConfiguration> GetMany(Expression<Func<View_EmployeeSalaryConfiguration, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<View_EmployeeSalaryConfiguration>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<View_EmployeeSalaryConfiguration>> GetManyAsync(Expression<Func<View_EmployeeSalaryConfiguration, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<View_EmployeeSalaryConfiguration> GetAsync(Expression<Func<View_EmployeeSalaryConfiguration, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

    }
}
