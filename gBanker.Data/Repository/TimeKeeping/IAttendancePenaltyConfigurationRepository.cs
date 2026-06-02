using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IAttendancePenaltyConfigurationRepository : IRepository<AttendancePenaltyConfiguration>
    {

        List<AttendancePenaltyConfiguration> AddAttendancePenaltyConfigurationList(List<AttendancePenaltyConfiguration> objs);
    }

    public class AttendancePenaltyConfigurationRepository : RepositoryBaseCodeFirst<AttendancePenaltyConfiguration>, IAttendancePenaltyConfigurationRepository
    {
        public AttendancePenaltyConfigurationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public List<AttendancePenaltyConfiguration> AddAttendancePenaltyConfigurationList(List<AttendancePenaltyConfiguration> objs)
        {
            DataContext.AttendancePenaltyConfigurations.AddRange(objs);
            DataContext.SaveChanges();
            return objs;
        }

    }
}

