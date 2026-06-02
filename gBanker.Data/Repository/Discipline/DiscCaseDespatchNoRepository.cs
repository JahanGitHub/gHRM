using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscCaseDespatchNoRepository : IRepository<DiscCaseDespatchNo>
    {

    }
    public class DiscCaseDespatchNoRepository : RepositoryBaseCodeFirst<DiscCaseDespatchNo>, IDiscCaseDespatchNoRepository
    {
        public DiscCaseDespatchNoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
