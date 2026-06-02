using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.HealthWelfareFund;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using System.Collections.Generic;

namespace gHRM.Data.Repository.WelfareFund
{
    public interface IHealthWelfareFundConfigurationRepository : IRepository<HealthWelfareFundConfiguration>
    {

    }
    public class HealthWelfareFundConfigurationRepository : RepositoryBaseCodeFirst<HealthWelfareFundConfiguration>, IHealthWelfareFundConfigurationRepository
    {
        public HealthWelfareFundConfigurationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
