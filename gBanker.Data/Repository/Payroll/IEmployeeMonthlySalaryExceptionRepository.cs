
using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using System.Data.Entity;
using gHRM.Data.CodeFirstMigration.Payroll;

namespace gHRM.Data.Repository.payroll
{
    public interface IEmployeeMonthlySalaryExceptionRepository : IRepository<EmployeeMonthlySalaryException>
    {
        List<EmployeeMonthlySalaryException> AddEmplyoeeSalaryExceptionList(List<EmployeeMonthlySalaryException> objs);
    }
    public class EmployeeMonthlySalaryExceptionRepository : RepositoryBaseCodeFirst<EmployeeMonthlySalaryException>, IEmployeeMonthlySalaryExceptionRepository
    {
        public EmployeeMonthlySalaryExceptionRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }

        public List<EmployeeMonthlySalaryException> AddEmplyoeeSalaryExceptionList(List<EmployeeMonthlySalaryException> objs)
        {
            DataContext.EmployeeMonthlySalaryException.AddRange(objs);            
            DataContext.SaveChanges();
            return objs;
        }
    }


}
