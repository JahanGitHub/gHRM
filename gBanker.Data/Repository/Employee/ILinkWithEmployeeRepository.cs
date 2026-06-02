
using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface ILinkWithEmployeeRepository : IRepository<LinkWithEmployee>
    {

    }
    public class LinkWithEmployeeRepository : RepositoryBaseCodeFirst<LinkWithEmployee>, ILinkWithEmployeeRepository
    {
        public LinkWithEmployeeRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }


    }
}
