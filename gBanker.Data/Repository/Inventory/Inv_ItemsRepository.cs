using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Db;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public class Inv_ItemsRepository : RepositoryBaseCodeFirst<Inv_Items>, IInv_ItemsRepository
    {
        public Inv_ItemsRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

       
    }
    public interface IInv_ItemsRepository : IRepository<Inv_Items>
    {
       
    }
}



