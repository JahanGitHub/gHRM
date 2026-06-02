using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;

namespace gHRM.Data.Repository
{
    public interface IUnionRepository : IRepository<LgUnion>
    {
        
    }
    public class UnionRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.LgUnion>, IUnionRepository
    {
        public UnionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }        
    }
}
