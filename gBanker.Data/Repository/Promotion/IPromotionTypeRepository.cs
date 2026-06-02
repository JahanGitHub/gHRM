using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Promotion;

namespace gHRM.Data.Repository.Promotion
{
    public interface IPromotionTypeRepository : IRepository<PromotionType>
    {

    }

    public class PromotionTypeRepository : RepositoryBaseCodeFirst<PromotionType>, IPromotionTypeRepository
    {
        public PromotionTypeRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }
    }
}
