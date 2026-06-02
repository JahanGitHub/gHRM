using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IProjectInfoRepository : IRepository<ProjectInfo>
    {

    }
    public class ProjectInfoRepository : RepositoryBaseCodeFirst<ProjectInfo>, IProjectInfoRepository
    {
        public ProjectInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
