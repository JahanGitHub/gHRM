using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.HealthWelfareFund;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using System.Collections.Generic;

namespace gHRM.Data.Repository.TimeKeeping
{
    public interface ITimekeepingAttendanceDeviceRepository : IRepository<TimekeepingAttendanceDevice>
    {

    }
    public class TimekeepingAttendanceDeviceRepository : RepositoryBaseCodeFirst<TimekeepingAttendanceDevice>, ITimekeepingAttendanceDeviceRepository
    {
        public TimekeepingAttendanceDeviceRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
