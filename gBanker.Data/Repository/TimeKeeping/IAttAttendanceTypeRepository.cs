using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IAttAttendanceTypeRepository : IRepository<AttAttendanceType>
    {

    }

    public class AttAttendanceTypeRepository : RepositoryBaseCodeFirst<AttAttendanceType>, IAttAttendanceTypeRepository
    {
        public AttAttendanceTypeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}


