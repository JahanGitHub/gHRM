using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public class InvStoreItemRepository : RepositoryBaseCodeFirst<InvStoreItem>, IInvStoreItemRepository
    {
        public InvStoreItemRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IInvStoreItemRepository : IRepository<InvStoreItem>
    {
    }
}



