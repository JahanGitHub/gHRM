using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IPanelOfficerHistoryRepository : IRepository<PanelOfficerHistory>
    {

    }
    public class PanelOfficerHistoryRepository : RepositoryBaseCodeFirst<PanelOfficerHistory>, IPanelOfficerHistoryRepository
    {
        public PanelOfficerHistoryRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
