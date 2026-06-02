using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IAttCardIssueRepository : IRepository<AttCardIssue>
    {

    }
    // public class PFRefundRepository : RepositoryBaseCodeFirst<PFRefund>, IPFRefundRepository
    public class AttCardIssueRepository : RepositoryBaseCodeFirst<AttCardIssue>, IAttCardIssueRepository
    {
        public AttCardIssueRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {


        }

    }// ENd of Class
}// End of Namespace
