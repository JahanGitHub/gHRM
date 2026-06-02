using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Loan
{
    public interface IApprovalMasterRepository : IRepository<ApprovalMaster> { }
    public class ApprovalMasterRepository : RepositoryBaseCodeFirst<ApprovalMaster>, IApprovalMasterRepository
    {
        public ApprovalMasterRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory) { }
        
    }
}
