
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IELEncashmentConfigurationRepository : IRepository<ELEncashmentConfiguration>
    {

    }
    public class ELEncashmentConfigurationRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.ELEncashmentConfiguration>, IELEncashmentConfigurationRepository
    {
        public ELEncashmentConfigurationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
