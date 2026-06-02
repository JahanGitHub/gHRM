using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IAssetClientInfoRepository : IRepository<AssetClientInfo>
    {

    }

    public class AssetClientInfoRepository : RepositoryBaseCodeFirst<AssetClientInfo>, IAssetClientInfoRepository
    {
        public AssetClientInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
