using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IAssetPartialOutRepository : IRepository<AssetPartialOut>
    {

    }
    public class AssetPartialOutRepository : RepositoryBaseCodeFirst<AssetPartialOut>, IAssetPartialOutRepository
    {
        public AssetPartialOutRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
