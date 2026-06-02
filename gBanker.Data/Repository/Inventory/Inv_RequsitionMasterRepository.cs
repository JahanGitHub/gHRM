using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Db;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public class Inv_RequsitionMasterRepository : RepositoryBaseCodeFirst<Inv_RequsitionMaster>, IInv_RequsitionMasterRepository
    {
        public Inv_RequsitionMasterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInv_RequsitionMasterRepository : IRepository<Inv_RequsitionMaster>
    {
    }
}



