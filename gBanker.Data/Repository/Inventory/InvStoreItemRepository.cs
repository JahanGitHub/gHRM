using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public class InvWarehouseRepository : RepositoryBaseCodeFirst<InvWarehouse>, IInvWarehouseRepository
    {
        public InvWarehouseRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IInvWarehouseRepository : IRepository<InvWarehouse>
    {
    }
}



