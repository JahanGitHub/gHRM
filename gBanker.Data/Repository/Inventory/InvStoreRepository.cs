using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public class InvStoreRepository : RepositoryBaseCodeFirst<Inv_Store>, IInvStoreRepository
    {
        public InvStoreRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IInvStoreRepository : IRepository<Inv_Store>
    {
    }
}



