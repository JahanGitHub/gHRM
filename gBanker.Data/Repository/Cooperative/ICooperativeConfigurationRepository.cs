using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Cooperative;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using System.Collections.Generic;

namespace gHRM.Data.Repository.Cooperative
{
    public interface ICooperativeConfigurationRepository : IRepository<CooperativeConfiguration>
    {

    }
    public class CooperativeConfigurationRepository : RepositoryBaseCodeFirst<CooperativeConfiguration>, ICooperativeConfigurationRepository
    {
        public CooperativeConfigurationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }      
    }
}
