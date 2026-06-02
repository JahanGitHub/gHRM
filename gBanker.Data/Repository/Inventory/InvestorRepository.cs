using gHRM.Data.CodeFirstMigration.Db;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public class InvestorRepository : RepositoryBaseCodeFirst<Investor>, IInvestorRepository
    {
        public InvestorRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInvestorRepository : IRepository<Investor>
    { 
    }
}
