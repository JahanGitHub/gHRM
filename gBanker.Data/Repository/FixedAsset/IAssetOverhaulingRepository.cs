using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IAssetOverhaulingRepository : IRepository<AssetOverhauling>
    {

    }

    public class AssetOverhaulingRepository : RepositoryBaseCodeFirst<AssetOverhauling>, IAssetOverhaulingRepository
    {
        public AssetOverhaulingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
