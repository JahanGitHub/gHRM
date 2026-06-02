using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IView_EmployeeSalaryConfigurationRepository : IRepository<View_EmployeeSalaryConfiguration>
    {
        List<View_EmployeeSalaryConfiguration> GetEmployeeSalaryConfigurationListbyCode(string employeeCode);
    }
    public class View_EmployeeSalaryConfigurationRepository : RepositoryBaseCodeFirst<View_EmployeeSalaryConfiguration>, IView_EmployeeSalaryConfigurationRepository
    {
        public View_EmployeeSalaryConfigurationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public List<View_EmployeeSalaryConfiguration> GetEmployeeSalaryConfigurationListbyCode(string employeeCode)
        {
            IQueryable<View_EmployeeSalaryConfiguration> employeeSalaryConfiguration = DataContext.View_EmployeeSalaryConfiguration;

            var listing= employeeSalaryConfiguration
                                .Where(p => 
                                    p.IsActive == true && 
                                    p.EmployeeCode == employeeCode
                                ).AsParallel().ToList();

            return listing;
        }
    }
}
