using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface IAssetProcessInfoRepository : IRepository<AssetProcessInfo>
    {

    }
    public class AssetProcessInfoRepository : RepositoryBaseCodeFirst<AssetProcessInfo>, IAssetProcessInfoRepository
    {
        public AssetProcessInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
