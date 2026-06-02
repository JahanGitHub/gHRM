using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using System.Collections.Generic;

namespace gHRM.Data.Repository.WelfareFund
{
    public interface IFundSetupRepository : IRepository<FundSetup>
    {

    }
    public class FundSetupRepository : RepositoryBaseCodeFirst<FundSetup>, IFundSetupRepository
    {
        public FundSetupRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }      
    }
}
