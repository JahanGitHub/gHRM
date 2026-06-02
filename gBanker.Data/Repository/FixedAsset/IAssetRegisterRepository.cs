using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IAssetRegisterRepository : IRepository<AssetRegister>
    {

    }
    public class AssetRegisterRepository : RepositoryBaseCodeFirst<AssetRegister>, IAssetRegisterRepository
    {
        public AssetRegisterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
