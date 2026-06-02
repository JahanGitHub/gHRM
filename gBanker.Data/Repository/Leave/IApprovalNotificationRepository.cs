using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
 
    public interface IApprovalNotificationRepository : IRepository<ApprovalNotification>
    {

    }
    public class ApprovalNotificationRepository : RepositoryBaseCodeFirst<ApprovalNotification>, IApprovalNotificationRepository
    {
        public ApprovalNotificationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

    }
}
