using gHRM.Data.CodeFirstMigration;
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
    public interface IEmployeeMonthlySalaryApprovedService : IServiceBase<EmployeeMonthlySalaryApproved>
    {
        List<EmployeeMonthlySalaryApproved> AddEmployeeMonthlyApprovedList(List<EmployeeMonthlySalaryApproved> objs);
        List<EmployeeMonthlySalaryApproved> CheckAlreadyApprovedSalary(int salaryMonth, int salaryYear);
        List<EmployeeMonthlySalaryApproved> GetEmployeeMonthlySalaryApprovedByYearAndMonth(int year, int month);
        bool CheckApprovedEmployeeSalaryExist(int salaryYear, int salaryMonth);
    }
    public class EmployeeMonthlySalaryApprovedService : IEmployeeMonthlySalaryApprovedService
    {
        private readonly IEmployeeMonthlySalaryApprovedRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeMonthlySalaryApprovedService(IEmployeeMonthlySalaryApprovedRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeMonthlySalaryApproved> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.Id);
            return entities;
        }

        public EmployeeMonthlySalaryApproved GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public bool CheckApprovedEmployeeSalaryExist(int salaryYear, int salaryMonth)
        {
            var isExist = repository.CheckApprovedEmployeeSalaryExist(salaryYear, salaryMonth);
            return isExist;
        }

        public EmployeeMonthlySalaryApproved Create(EmployeeMonthlySalaryApproved objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeMonthlySalaryApproved objectToUpdate)
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
        public List<EmployeeMonthlySalaryApproved> AddEmployeeMonthlyApprovedList(List<EmployeeMonthlySalaryApproved> objs)
        {
            repository.AddEmployeeMonthlyApprovedList(objs);
            return objs;
        }

        public EmployeeMonthlySalaryApproved Get(Expression<Func<EmployeeMonthlySalaryApproved, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeMonthlySalaryApproved> GetMany(Expression<Func<EmployeeMonthlySalaryApproved, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public List<EmployeeMonthlySalaryApproved> CheckAlreadyApprovedSalary(int salaryMonth, int salaryYear)
        {
            return repository.GetMany(p => p.SalaryMonth == salaryMonth && p.SalaryYear == salaryYear && p.IsActive == true).ToList();
        }

        public List<EmployeeMonthlySalaryApproved> GetEmployeeMonthlySalaryApprovedByYearAndMonth(int year, int month)
        {
            var listing = new List<EmployeeMonthlySalaryApproved>();

            using (var db = new gHRMDBContext())
            {
                listing = db.EmployeeMonthlySalaryApproved
                    .Where(p => p.SalaryYear == year && p.SalaryMonth == month).ToList();
            }
            return listing;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeMonthlySalaryApproved>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeMonthlySalaryApproved>> GetManyAsync(Expression<Func<EmployeeMonthlySalaryApproved, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeMonthlySalaryApproved> GetAsync(Expression<Func<EmployeeMonthlySalaryApproved, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion
    }
}
