using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface IIncomeTaxRepository : IRepository<IncomeTax>
    {
        // Add custom methods here if needed in future
    }

    public class IncomeTaxRepository : RepositoryBaseCodeFirst<IncomeTax>, IIncomeTaxRepository
    {
        public IncomeTaxRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {
            // Optional constructor logic
        }
    }
}
