using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Db;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public class Inv_TempStoreRepository : RepositoryBaseCodeFirst<Inv_TempStore>, IInv_TempStoreRepository
    {
        public Inv_TempStoreRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInv_TempStoreRepository : IRepository<Inv_TempStore>
    {
    }
}



