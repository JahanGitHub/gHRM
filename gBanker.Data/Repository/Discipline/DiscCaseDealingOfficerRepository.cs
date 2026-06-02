using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscCaseDealingOfficerRepository : IRepository<DiscCaseDealingOfficer>
    {

    }
    public class DiscCaseDealingOfficerRepository : RepositoryBaseCodeFirst<DiscCaseDealingOfficer>, IDiscCaseDealingOfficerRepository
    {
        public DiscCaseDealingOfficerRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

    }
}
