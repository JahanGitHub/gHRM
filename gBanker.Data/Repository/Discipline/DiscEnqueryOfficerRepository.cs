using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using gHRM.Data.CodeFirstMigration.Discipline;

namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscEnqueryOfficerRepository : IRepository<DiscEnqueryOfficer>
    {

    }
    public class DiscEnqueryOfficerRepository : RepositoryBaseCodeFirst<DiscEnqueryOfficer>, IDiscEnqueryOfficerRepository
    {
        public DiscEnqueryOfficerRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
