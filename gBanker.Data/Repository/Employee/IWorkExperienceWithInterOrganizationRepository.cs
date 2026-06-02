using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface IWorkExperienceWithInterOrganizationRepository : IRepository<WorkExperienceWithInterOrganization>
    {

    }
    public class WorkExperienceWithInterOrganizationRepository : RepositoryBaseCodeFirst<WorkExperienceWithInterOrganization>, IWorkExperienceWithInterOrganizationRepository
    {
        public WorkExperienceWithInterOrganizationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
