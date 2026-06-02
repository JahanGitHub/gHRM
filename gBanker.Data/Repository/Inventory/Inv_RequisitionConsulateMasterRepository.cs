using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Db;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public class Inv_RequisitionConsulateMasterRepository : RepositoryBaseCodeFirst<Inv_RequisitionConsulateMaster>, IInv_RequisitionConsulateMasterRepository
    {
        public Inv_RequisitionConsulateMasterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInv_RequisitionConsulateMasterRepository : IRepository<Inv_RequisitionConsulateMaster>
    {
    }
}



