using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface IAssetUserRepository : IRepository<AssetUser>
    {

    }
    public class AssetUserRepository : RepositoryBaseCodeFirst<AssetUser>, IAssetUserRepository
    {
        public AssetUserRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
