using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using gHRM.Data.CodeFirstMigration.Payroll;

namespace gHRM.Data.Repository.Payroll
{
    public interface IView_EmployeeMonthlySalaryRepository : IRepository<View_EmployeeMonthlySalary>
    {

    }
    public class View_EmployeeMonthlySalaryRepository : RepositoryBaseCodeFirst<View_EmployeeMonthlySalary>, IView_EmployeeMonthlySalaryRepository
    {
        public View_EmployeeMonthlySalaryRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
