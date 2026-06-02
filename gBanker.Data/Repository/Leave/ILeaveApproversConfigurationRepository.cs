using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface ILeaveApproversConfigurationRepository : IRepository<LeaveApproversConfiguration>
    {
        List<LeaveApproversConfiguration> AddApprovalConfigList(List<LeaveApproversConfiguration> objs);
    }

    public class LeaveApproversConfigurationRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.LeaveApproversConfiguration>, ILeaveApproversConfigurationRepository
    {
        public LeaveApproversConfigurationRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }

        public List<LeaveApproversConfiguration> AddApprovalConfigList(List<LeaveApproversConfiguration> objs)
        {
            DataContext.LeaveApproversConfiguration.AddRange(objs);
            DataContext.SaveChanges();
            return objs;
        }
    }
}
