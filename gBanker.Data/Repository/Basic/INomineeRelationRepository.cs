
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface INomineeRelationRepository : IRepository<NomineeRelation>
    {

    }
    public class NomineeRelationRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.NomineeRelation>, INomineeRelationRepository
    {
        public NomineeRelationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
