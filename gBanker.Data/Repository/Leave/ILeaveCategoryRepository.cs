
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface ILeaveCategoryRepository : IRepository<LeaveCategory>
    {

    }

    public class LeaveCategoryRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.LeaveCategory>, ILeaveCategoryRepository
    {
        public LeaveCategoryRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

