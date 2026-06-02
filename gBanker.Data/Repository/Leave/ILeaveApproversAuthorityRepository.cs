using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface ILeaveApproversAuthorityRepository : IRepository<LeaveApproversAuthority>
    {

    }
    public class LeaveApproversAuthorityRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.LeaveApproversAuthority>, ILeaveApproversAuthorityRepository
    {
        public LeaveApproversAuthorityRepository(IDatabaseFactoryCodeFirst databaseFactory)  : base(databaseFactory)
        {

        }

    }
}

