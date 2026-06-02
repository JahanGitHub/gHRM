using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IOccupationRepository : IRepository<Occupation>
    {

    }

    public class OccupationRepository : RepositoryBaseCodeFirst<Occupation>, IOccupationRepository
    {
        public OccupationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}


