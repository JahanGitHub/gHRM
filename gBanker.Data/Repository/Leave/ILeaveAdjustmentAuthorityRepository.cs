
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface ILeaveAdjustmentAuthorityRepository : IRepository<LeaveAdjustmentAuthority>
    {

    }

    public class LeaveAdjustmentAuthorityRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.LeaveAdjustmentAuthority>, ILeaveAdjustmentAuthorityRepository
    {
        public LeaveAdjustmentAuthorityRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }
    }
}

