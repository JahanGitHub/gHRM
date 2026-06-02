using gHRM.Core.Filters.Payroll;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IEmployeeMonthlySalaryRepository : IRepository<EmployeeMonthlySalary>
    {
        List<EmployeeMonthlySalary> GetListingByFilter(EmployeeMonthlySalarySearchFilter filter);
        bool CheckMonthlySalaryByComponent(int salaryMonth, int salaryYear, int employeeId, int prComponentId);
        List<EmployeeMonthlySalary> AddEmplyoeeSalaryList(List<EmployeeMonthlySalary> objs);
        bool CheckMonthlySalaryByFilter(EmployeeMonthlySalarySearchFilter filter);
    }
    public class EmployeeMonthlySalaryRepository : RepositoryBaseCodeFirst<EmployeeMonthlySalary>, IEmployeeMonthlySalaryRepository
    {
        public EmployeeMonthlySalaryRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }

        public List<EmployeeMonthlySalary> AddEmplyoeeSalaryList(List<EmployeeMonthlySalary> objs)
        {
            DataContext.EmployeeMonthlySalary.AddRange(objs);           
            DataContext.SaveChanges();
            return objs;
        }

        public bool CheckMonthlySalaryByComponent(int salaryMonth,int salaryYear,int employeeId,int prComponentId)
        {
            var isExistMontlySalary = false;
            isExistMontlySalary = DataContext.EmployeeMonthlySalary.
                                        Any(f=>f.SalaryMonth == salaryMonth
                                            && f.SalaryYear == salaryYear
                                            && f.EmployeeId == employeeId
                                            && f.PRComponentId == prComponentId
                                            && f.IsActive);
            return isExistMontlySalary;
        }

        public bool CheckMonthlySalaryByFilter(EmployeeMonthlySalarySearchFilter filter)
        {
            IQueryable<EmployeeMonthlySalary> employeeMonthlySalary
                = DataContext.EmployeeMonthlySalary.
                        Where(f => (filter.IsActive ==null || f.IsActive== filter.IsActive)                               
                                && (filter.SalaryMonth == null && filter.SalaryMonth == 0 || f.SalaryMonth == filter.SalaryMonth)
                                && (filter.SalaryYear == null && filter.SalaryYear == 0 || f.SalaryYear == filter.SalaryYear)
                                && (filter.EmployeeId == null && filter.EmployeeId == 0 || f.EmployeeId == filter.EmployeeId)
                                && (filter.PRComponentId == null && filter.PRComponentId == 0 || f.PRComponentId == filter.PRComponentId)
                                && (filter.IsSendForApproval == null || (f.IsSendForApproval == filter.IsSendForApproval))
                                && (filter.IsApproved == null || (f.IsApproved == filter.IsApproved))
                                && (filter.IsRejected == null || (f.IsRejected == filter.IsRejected))
                            );
            return employeeMonthlySalary.Any();
        }

        public List<EmployeeMonthlySalary> GetListingByFilter(EmployeeMonthlySalarySearchFilter filter)
        {
            IQueryable<EmployeeMonthlySalary> listings
                = DataContext.EmployeeMonthlySalary.
                        Where(f=>f.IsActive
                                && (filter.SalaryMonth == null && filter.SalaryMonth==0 || f.SalaryMonth == filter.SalaryMonth)
                                && (filter.SalaryYear == null && filter.SalaryYear == 0 || f.SalaryYear == filter.SalaryYear)
                                && (filter.EmployeeId == null && filter.EmployeeId == 0 || f.EmployeeId == filter.EmployeeId)
                                && (filter.PRComponentId == null && filter.PRComponentId == 0 || f.PRComponentId == filter.PRComponentId)                            
                                && (filter.IsSendForApproval ==null || (f.IsSendForApproval== filter.IsSendForApproval))
                                && (filter.IsApproved == null || (f.IsApproved == filter.IsApproved))
                                && (filter.IsRejected == null || (f.IsRejected == filter.IsRejected))
                            );
            return listings.AsParallel().ToList();
        }

       
    }
}
