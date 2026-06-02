using gHRM.Data.CodeFirstMigration;

using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface IDepriciationRateChangeRepository : IRepository<DepriciationRateChange>
    {
    }
    public class DepriciationRateChangeRepository : RepositoryBaseCodeFirst<DepriciationRateChange>, IDepriciationRateChangeRepository
    {
        public DepriciationRateChangeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
