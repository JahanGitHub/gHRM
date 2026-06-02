using gHRM.Data.CodeFirstMigration;

using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface IAssetRevaluationRepository : IRepository<AssetRevaluation>
    {
    }
    public class AssetRevaluationRepository : RepositoryBaseCodeFirst<AssetRevaluation>, IAssetRevaluationRepository
    {
        public AssetRevaluationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
