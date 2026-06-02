using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IAssetDepreciationInfoRepository : IRepository<AssetDepreciationInfo>
    {
        bool UpdateAssetDepreciationInfo(AssetDepreciationInfo assetDepreciationInfo);
    }
    public class AssetDepreciationInfoRepository : RepositoryBaseCodeFirst<AssetDepreciationInfo>, IAssetDepreciationInfoRepository
    {
        public AssetDepreciationInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public bool UpdateAssetDepreciationInfo(AssetDepreciationInfo assetDepreciationInfo)
        {
            try
            {
                var updateAssetDepreciationInfo = DataContext.AssetDepreciationInfo.FirstOrDefault(f => f.DailyTransactionId == assetDepreciationInfo.DailyTransactionId);

                if (updateAssetDepreciationInfo == null)
                    return false;
                else
                {
                    updateAssetDepreciationInfo.AssetID = assetDepreciationInfo.AssetID;
                    updateAssetDepreciationInfo.AssetSerial = assetDepreciationInfo.AssetSerial;                   

                    DataContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
