using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IAspNetSecurityModuleRepository : IRepository<AspNetSecurityModule>
    {

    }
    public class AspNetSecurityModuleRepository : RepositoryBaseCodeFirst<AspNetSecurityModule>, IAspNetSecurityModuleRepository
    {
        public AspNetSecurityModuleRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
