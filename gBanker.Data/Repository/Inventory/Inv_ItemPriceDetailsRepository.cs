using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Db;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public class Inv_ItemPriceDetailsRepository : RepositoryBaseCodeFirst<Inv_ItemPriceDetails>, IInv_ItemPriceDetailsRepository
    {
        public Inv_ItemPriceDetailsRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInv_ItemPriceDetailsRepository : IRepository<Inv_ItemPriceDetails>
    {
    }
}



