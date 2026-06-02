using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;
namespace gHRM.Data.Repository
{
   public interface IApprovalConfigMasterRepository :IRepository<ApprovalConfigMaster>
    {
   
    }
   public class ApprovalConfigMasterRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.ApprovalConfigMaster>, IApprovalConfigMasterRepository
    {
        public ApprovalConfigMasterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
       
    }
}
