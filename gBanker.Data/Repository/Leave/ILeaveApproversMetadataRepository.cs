
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface ILeaveApproversMetadataRepository : IRepository<LeaveApproversMetadata>
    {
        List<LeaveApproversMetadata> AddLeaveApproversMetadataList(List<LeaveApproversMetadata> objs);
    }

    public class LeaveApproversMetadataRepository : RepositoryBaseCodeFirst<LeaveApproversMetadata>, ILeaveApproversMetadataRepository
    {
        public LeaveApproversMetadataRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }

        public List<LeaveApproversMetadata> AddLeaveApproversMetadataList(List<LeaveApproversMetadata> objs)
        {
            DataContext.LeaveApproversMetadata.AddRange(objs);
            DataContext.SaveChanges();
            return objs;
        }
    }
}
