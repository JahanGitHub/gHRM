using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Db;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public class Inv_RequsitionDetailsRepository : RepositoryBaseCodeFirst<Inv_RequsitionDetails>, IInv_RequsitionDetailsRepository
    {
        public Inv_RequsitionDetailsRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInv_RequsitionDetailsRepository : IRepository<Inv_RequsitionDetails>
    {
    }
}



