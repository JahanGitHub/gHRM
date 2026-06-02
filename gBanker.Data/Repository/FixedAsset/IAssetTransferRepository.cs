using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IAssetTransferRepository : IRepository<AssetTransfer>
    {

    }
    public class AssetTransferRepository : RepositoryBaseCodeFirst<AssetTransfer>, IAssetTransferRepository
    {
        public AssetTransferRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
