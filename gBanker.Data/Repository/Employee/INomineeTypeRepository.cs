using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;


namespace gHRM.Data.Repository
{
    public interface INomineeTypeRepository : IRepository<NomineeType>
    {

    }

    public class NomineeTypeRepository : RepositoryBaseCodeFirst<NomineeType>, INomineeTypeRepository
    {
        public NomineeTypeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}


