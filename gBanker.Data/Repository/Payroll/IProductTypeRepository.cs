using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IProductTypeRepository : IRepository<ProductType>
    {

    }
    public class ProductTypeRepository : RepositoryBaseCodeFirst<ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
