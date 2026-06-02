using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface ITimeKeepingRosterRepository : IRepository<TimeKeepingRoster>
    {

    }

    public class TimeKeepingRosterRepository : RepositoryBaseCodeFirst<TimeKeepingRoster>, ITimeKeepingRosterRepository
    {
        public TimeKeepingRosterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

