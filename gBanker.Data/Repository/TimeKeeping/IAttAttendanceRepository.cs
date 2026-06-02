using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface IAttAttendanceRepository : IRepository<AttAttendance>
    {

    }

    public class AttAttendanceRepository : RepositoryBaseCodeFirst<AttAttendance>, IAttAttendanceRepository
    {
        public AttAttendanceRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }

    }//End Class
}// End Namespace
