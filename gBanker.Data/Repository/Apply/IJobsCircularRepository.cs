using System.Text;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Apply;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;
using gHRM.Data.Utility;
using System;
using System.Threading.Tasks;
using gHRM.Core.Filters.Offices;
using System.Data.Entity;

namespace gHRM.Data.Repository
{
    public interface IJobsCircularRepository : IRepository<JobsCircular>
    {
    }
    public class JobsCircularRepository : RepositoryBaseCodeFirst<JobsCircular>, IJobsCircularRepository
    {
        public JobsCircularRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}


