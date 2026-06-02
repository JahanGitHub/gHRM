using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Db;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public class Inv_RequisitionConsulateDetailsRepository : RepositoryBaseCodeFirst<Inv_RequisitionConsulateDetails>, IInv_RequisitionConsulateDetailsRepository
    {
        public Inv_RequisitionConsulateDetailsRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInv_RequisitionConsulateDetailsRepository : IRepository<Inv_RequisitionConsulateDetails>
    {
    }
}



