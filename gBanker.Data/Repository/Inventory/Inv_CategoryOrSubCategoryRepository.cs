using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Db;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public class Inv_CategoryOrSubCategoryRepository : RepositoryBaseCodeFirst<Inv_CategoryOrSubCategory>, IInv_CategoryOrSubCategoryRepository
    {
        public Inv_CategoryOrSubCategoryRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInv_CategoryOrSubCategoryRepository : IRepository<Inv_CategoryOrSubCategory>
    {

    }
}



