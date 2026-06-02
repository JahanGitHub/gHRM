using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Cooperative;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using System.Collections.Generic;

namespace gHRM.Data.Repository.Cooperative
{
    public interface ICooperativeLedgerRepository : IRepository<CooperativeLedger>
    {

    }
    public class CooperativeLedgerRepository : RepositoryBaseCodeFirst<CooperativeLedger>, ICooperativeLedgerRepository
    {
        public CooperativeLedgerRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }      
    }
}
