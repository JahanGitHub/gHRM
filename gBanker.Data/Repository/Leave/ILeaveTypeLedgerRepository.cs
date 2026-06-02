using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface ILeaveTypeLedgerRepository : IRepository<LeaveTypeLedger>
    {
    }

    public class LeaveTypeLedgerRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.LeaveTypeLedger>, ILeaveTypeLedgerRepository
    {
        public LeaveTypeLedgerRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }
    }
}
