using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface ILeaveMaternityOpeningRepository : IRepository<LeaveMaternityOpening>
    {

    }

    public class LeaveMaternityOpeningRepository : RepositoryBaseCodeFirst<LeaveMaternityOpening>, ILeaveMaternityOpeningRepository
    {
        public LeaveMaternityOpeningRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {
        }
    }
}
