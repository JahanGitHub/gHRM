using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IAssetGroupInfoRepository : IRepository<AssetGroupInfo>
    {

    }

    public class AssetGroupInfoRepository : RepositoryBaseCodeFirst<AssetGroupInfo>, IAssetGroupInfoRepository
    {
        public AssetGroupInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
