using gHRM.Core.Filters.Payroll;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using gHRM.Data.DBDetailModels.Payroll;
using gHRM.Data.Repository.Payroll;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service.Payroll
{
    public interface IEmployeeMonthlySalaryService : IServiceBase<EmployeeMonthlySalary>
    {
        List<EmployeeMonthlySalary> GetListingByFilter(EmployeeMonthlySalarySearchFilter filter);
        bool CheckMonthlySalaryByComponent(int salaryMonth, int salaryYear, int employeeId, int prComponentId);
        List<EmployeeMonthlySalary> AddEmployeeMonthlySalaryList(List<EmployeeMonthlySalary> objs);
        List<EmployeeMonthlySalary> GetForHoldSalary(long employeeId, int month, int year);
        List<EmployeeMonthlySalary> GetApprovedSalary(int salaryYear, int salaryMonth);
        List<EmployeeMonthlySalary> GetSendForApprovalSalary(int salaryYear, int salaryMonth);
        List<EmployeeMonthlySalary> GetEmployeeMonthlySalaryActiveAndIsSendForApprovalByYearAndMonth(
            int year, int month);

        bool CheckEmployeeMonthlySalary(long employeeId, DateTime startDate, DateTime endDate);
        bool CheckEmployeeMonthlySalaryByComponent(int componentId);
        List<EmployeeMonthlySalary> GetActiveEmployeeMonthlySalary(int salaryYear, int salaryMonth, int employeeId);

        bool CheckMonthlySalaryByEmployeeAndComponents(EmployeeMonthlySalarySearchFilter filter);
        bool CheckMonthlySalaryByFilter(EmployeeMonthlySalarySearchFilter filter);
    }
    public class EmployeeMonthlySalaryService : IEmployeeMonthlySalaryService
    {
        private readonly IEmployeeMonthlySalaryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeMonthlySalaryService(IEmployeeMonthlySalaryRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public List<EmployeeMonthlySalary> GetListingByFilter(EmployeeMonthlySalarySearchFilter filter)
        {
            var listings = repository.GetListingByFilter(filter);
            return listings;
        }

        public List<EmployeeMonthlySalary> GetApprovedSalary(int salaryYear, int salaryMonth)
        {
            return repository.GetMany(p => p.SalaryYear == salaryYear && p.SalaryMonth == salaryMonth && p.IsActive == true && p.IsApproved == true).ToList();
        }

        public List<EmployeeMonthlySalary> GetSendForApprovalSalary(int salaryYear, int salaryMonth)
        {
            return repository.GetMany(p => p.SalaryYear == salaryYear && p.SalaryMonth == salaryMonth && p.IsActive == true && p.IsSendForApproval == true).ToList();
        }

        public List<EmployeeMonthlySalary> GetActiveEmployeeMonthlySalary(int salaryYear, int salaryMonth, int employeeId)
        {
            return repository.GetMany(p => p.SalaryYear == salaryYear && p.SalaryMonth == salaryMonth && p.EmployeeId == employeeId && p.IsActive == true).ToList();
        }
        public IEnumerable<EmployeeMonthlySalary> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.SalaryId);
            return entities;
        }

        public EmployeeMonthlySalary GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public bool CheckMonthlySalaryByComponent(int salaryMonth, int salaryYear, int employeeId, int prComponentId)
        {
            return repository.CheckMonthlySalaryByComponent(
                salaryMonth,
                salaryYear,
                employeeId,
                prComponentId
            );
        }
        public bool CheckMonthlySalaryByFilter(EmployeeMonthlySalarySearchFilter filter)
        {
            return repository.CheckMonthlySalaryByFilter(filter);
        }


        public bool CheckEmployeeMonthlySalary(long employeeId, DateTime startDate, DateTime endDate)
        {
            bool employeeSalaryFound = true;

            using (var db = new gHRMDBContext())
            {
                employeeSalaryFound = db.EmployeeMonthlySalary.Any(z => z.IsActive
                            && !z.IsSendForApproval
                            && !z.IsApproved
                            && (
                                   DbFunctions.TruncateTime(z.SalaryDate) >= DbFunctions.TruncateTime(startDate)
                                && DbFunctions.TruncateTime(z.SalaryDate) <= DbFunctions.TruncateTime(endDate)
                            )
                            && z.EmployeeId == employeeId);
            }

            return employeeSalaryFound;
        }

        public bool CheckEmployeeMonthlySalaryByComponent(int componentId)
        {
            bool employeeSalaryFound = true;

            using (var db = new gHRMDBContext())
            {
                employeeSalaryFound = db.EmployeeMonthlySalary.Any(z => z.IsActive
                            && z.PRComponentId == componentId);
            }

            return employeeSalaryFound;
        }

        public bool CheckMonthlySalaryByEmployeeAndComponents(EmployeeMonthlySalarySearchFilter filter)
        {
            bool employeeSalaryFound = true;

            using (var db = new gHRMDBContext())
            {
                List<int> componentIds = db.PRComponents
                                            .Where(c => c.IsActive && c.EmployeeTypeId == filter.EmployeeTypeId
                                                && c.EmployeeStatusId == filter.EmployeeStatusId && c.PFTypeId == filter.PFTypeId
                                                    && c.OfficeLocationId == filter.OfficeLocationId
                                                        && filter.Components.Any(a => a == c.ComponentName))
                                            .Select(s => s.PRComponentID).ToList();

                employeeSalaryFound = db.EmployeeMonthlySalary.Any(z => z.IsActive && z.EmployeeId == filter.EmployeeId
                            && componentIds.Any(a => a == z.PRComponentId));
            }

            return employeeSalaryFound;
        }

        public EmployeeMonthlySalary Create(EmployeeMonthlySalary objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(EmployeeMonthlySalary objectToUpdate)
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

        public List<EmployeeMonthlySalary> AddEmployeeMonthlySalaryList(List<EmployeeMonthlySalary> objs)
        {
            repository.AddEmplyoeeSalaryList(objs);
            return objs;
        }

        public EmployeeMonthlySalary Get(Expression<Func<EmployeeMonthlySalary, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<EmployeeMonthlySalary> GetMany(Expression<Func<EmployeeMonthlySalary, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public List<EmployeeMonthlySalary>
            GetEmployeeMonthlySalaryActiveAndIsSendForApprovalByYearAndMonth(int year, int month)
        {
            var listing = new List<EmployeeMonthlySalary>();

            using (var db = new gHRMDBContext())
            {
                listing = db.EmployeeMonthlySalary
                    .Where(p => p.SalaryYear == year && p.SalaryMonth == month
                    && p.IsActive && p.IsSendForApproval).ToList();
            }
            return listing;
        }

        #region Asyc
        public virtual async Task<IEnumerable<EmployeeMonthlySalary>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<EmployeeMonthlySalary>> GetManyAsync(Expression<Func<EmployeeMonthlySalary, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<EmployeeMonthlySalary> GetAsync(Expression<Func<EmployeeMonthlySalary, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        public List<EmployeeMonthlySalary> GetForHoldSalary(long employeeId, int month, int year)
        {
            return repository.GetMany(m => m.IsActive == true && m.EmployeeId == employeeId && m.SalaryMonth == month && m.SalaryYear == year).ToList();
        }
    }

}
