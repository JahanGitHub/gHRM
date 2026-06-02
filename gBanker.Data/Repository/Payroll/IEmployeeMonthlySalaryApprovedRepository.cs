using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IEmployeeMonthlySalaryApprovedRepository : IRepository<EmployeeMonthlySalaryApproved>
    {
        bool CheckApprovedEmployeeSalaryExist(int salaryYear, int salaryMonth);
        List<EmployeeMonthlySalaryApproved> AddEmployeeMonthlyApprovedList(List<EmployeeMonthlySalaryApproved> objs);
    }
    public class EmployeeMonthlySalaryApprovedRepository : RepositoryBaseCodeFirst<EmployeeMonthlySalaryApproved>, IEmployeeMonthlySalaryApprovedRepository
    {
        public EmployeeMonthlySalaryApprovedRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }

        public List<EmployeeMonthlySalaryApproved> AddEmployeeMonthlyApprovedList(List<EmployeeMonthlySalaryApproved> objs)
        {
            DataContext.EmployeeMonthlySalaryApproved.AddRange(objs);
            DataContext.SaveChanges();
            return objs;
        }

        public bool CheckApprovedEmployeeSalaryExist(int salaryYear, int salaryMonth)
        {
            var isExist= DataContext.EmployeeMonthlySalaryApproved
                                        .Any(f=>
                                                f.SalaryYear== salaryYear 
                                            &&  f.SalaryMonth== salaryMonth
                                            &&  f.IsActive 
                                            &&  f.IsApproved);
            return isExist;
        }
    }
}
