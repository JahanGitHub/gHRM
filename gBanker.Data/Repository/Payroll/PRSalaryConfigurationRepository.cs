using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IPRSalaryConfigurationRepository : IRepository<PRSalaryConfiguration>
    {

    }
    public class PRSalaryConfigurationRepository : RepositoryBaseCodeFirst<PRSalaryConfiguration>, IPRSalaryConfigurationRepository
    {
        public PRSalaryConfigurationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
