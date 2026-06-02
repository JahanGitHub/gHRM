using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.HealthWelfareFund;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IEmployeeTranningDropDownRepository : IRepository<EmployeeTranningDropDown>
    {

    }
    public class EmployeeTranningDropDownRepository : RepositoryBaseCodeFirst<EmployeeTranningDropDown>, IEmployeeTranningDropDownRepository
    {
        public EmployeeTranningDropDownRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
