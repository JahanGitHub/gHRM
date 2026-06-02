using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IInternalOrganizationRepository : IRepository<InternalOrganization>
    {

    }

    public class InternalOrganizationRepository : RepositoryBaseCodeFirst<InternalOrganization>, IInternalOrganizationRepository
    {
        public InternalOrganizationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }

}


