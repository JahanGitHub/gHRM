using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IVMServiceProviderConfigurationRepository : IRepository<VMServiceProviderConfiguration>
    {

    }
    public class VMServiceProviderConfigurationRepository : RepositoryBaseCodeFirst<VMServiceProviderConfiguration>, IVMServiceProviderConfigurationRepository
    {
        public VMServiceProviderConfigurationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
