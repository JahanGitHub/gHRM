using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Db;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public class Inv_VendorRepository : RepositoryBaseCodeFirst<Inv_Vendor>, IInv_VendorRepository
    {
        public Inv_VendorRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInv_VendorRepository : IRepository<Inv_Vendor>
    {
    }
}



