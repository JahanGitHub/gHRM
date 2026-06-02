using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IView_EmployeeTypeWiseComponentConfigurationRepository : IRepository<View_EmployeeTypeWiseComponentConfiguration>
    {

    }
    public class View_EmployeeTypeWiseComponentConfigurationRepository : RepositoryBaseCodeFirst<View_EmployeeTypeWiseComponentConfiguration>, IView_EmployeeTypeWiseComponentConfigurationRepository
    {
        public View_EmployeeTypeWiseComponentConfigurationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
