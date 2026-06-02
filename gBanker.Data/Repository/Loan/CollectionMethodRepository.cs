using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Loan
{
    public interface ICollectionMethodRepository : IRepository<CollectionMethod> { }
    public class CollectionMethodRepository : RepositoryBaseCodeFirst<CollectionMethod>, ICollectionMethodRepository
    {
        public CollectionMethodRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory) { }
        
    }
}
