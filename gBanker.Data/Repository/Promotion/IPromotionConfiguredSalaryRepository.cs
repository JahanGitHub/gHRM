using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface IPromotionConfiguredSalaryRepository : IRepository<PromotionConfiguredSalary>
    {

    }

    public class PromotionConfiguredSalaryRepository : RepositoryBaseCodeFirst<PromotionConfiguredSalary>, IPromotionConfiguredSalaryRepository
    {
        public PromotionConfiguredSalaryRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }
    }
}
