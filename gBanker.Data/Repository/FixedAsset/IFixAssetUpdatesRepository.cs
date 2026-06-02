using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IFixAssetUpdatesRepository : IRepository<FixAssetUpdates>
    {

    }

    public class FixAssetUpdatesRepository : RepositoryBaseCodeFirst<FixAssetUpdates>, IFixAssetUpdatesRepository
    {
        public FixAssetUpdatesRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
