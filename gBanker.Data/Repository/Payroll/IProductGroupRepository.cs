using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IProductGroupRepository : IRepository<ProductGroup>
    {

    }
    public class ProductGroupRepository : RepositoryBaseCodeFirst<ProductGroup>, IProductGroupRepository
    {
        public ProductGroupRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
