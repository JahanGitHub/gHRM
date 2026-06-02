using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using System.Collections.Generic;

namespace gHRM.Data.Repository.WelfareFund
{
    public interface IHealthFundingRepository : IRepository<HealthFunding>
    {

    }
    public class HealthFundingRepository : RepositoryBaseCodeFirst<HealthFunding>, IHealthFundingRepository
    {
        public HealthFundingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
