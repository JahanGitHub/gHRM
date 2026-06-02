using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscCaseStatusRepository : IRepository<DiscCaseStatu>
    {

    }
    public class DiscCaseStatusRepository : RepositoryBaseCodeFirst<DiscCaseStatu>, IDiscCaseStatusRepository
    {
        public DiscCaseStatusRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

    }
}
