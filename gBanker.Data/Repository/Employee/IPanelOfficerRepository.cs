
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IPanelOfficerRepository : IRepository<PanelOfficer>
    {

    }
    public class PanelOfficerRepository : RepositoryBaseCodeFirst<PanelOfficer>, IPanelOfficerRepository
    {
        public PanelOfficerRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
