using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IProductItemRepository : IRepository<ProductItem>
    {

    }
    public class ProductItemRepository : RepositoryBaseCodeFirst<ProductItem>, IProductItemRepository
    {
        public ProductItemRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
