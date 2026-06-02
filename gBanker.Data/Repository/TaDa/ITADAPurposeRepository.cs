using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.TaDa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.TaDa
{
    public interface ITADAPurposeRepository : IRepository<TADAPurpose>
    {
       
    }

    public class TADAPurposeRepository : RepositoryBaseCodeFirst<TADAPurpose>, ITADAPurposeRepository
    {
        public TADAPurposeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        
    }
}
