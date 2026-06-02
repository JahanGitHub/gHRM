using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Loan
{
    public interface IApproveDetailRepository : IRepository<ApproveDetail> { }
    public class ApproveDetailRepository : RepositoryBaseCodeFirst<ApproveDetail>, IApproveDetailRepository
    {
        public ApproveDetailRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory) { }
        
    }
}
