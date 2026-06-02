using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface ILastAssetCodeInfoRepository : IRepository<LastAssetCodeInfo>
    {

    }
    public class LastAssetCodeInfoRepository : RepositoryBaseCodeFirst<LastAssetCodeInfo>, ILastAssetCodeInfoRepository
    {
        public LastAssetCodeInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }


    }
}
