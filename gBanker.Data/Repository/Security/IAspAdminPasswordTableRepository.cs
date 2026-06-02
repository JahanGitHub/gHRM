using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IAspAdminPasswordTableRepository : IRepository<AspAdminPasswordTable>
    {

    }
    public class AspAdminPasswordTableRepository : RepositoryBaseCodeFirst<AspAdminPasswordTable>, IAspAdminPasswordTableRepository
    {
        public AspAdminPasswordTableRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
