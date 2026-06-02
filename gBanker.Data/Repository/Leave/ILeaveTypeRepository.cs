using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface ILeaveTypeRepository : IRepository<LeaveType>
    {
    }

    public class LeaveTypeRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.LeaveType>, ILeaveTypeRepository
    {
        public LeaveTypeRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }
    }
}
