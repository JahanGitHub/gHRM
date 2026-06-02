
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface ILeaveApproversRepository : IRepository<LeaveApprovers>
    {
        List<LeaveApprovers> AddApproversList(List<LeaveApprovers> objs);
    }

    public class LeaveApproversRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.LeaveApprovers>, ILeaveApproversRepository
    {
        public LeaveApproversRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }

        public List<LeaveApprovers> AddApproversList(List<LeaveApprovers> objs)
        {
            DataContext.LeaveApprovers.AddRange(objs);
            DataContext.SaveChanges();
            return objs;
        }
    }
}

