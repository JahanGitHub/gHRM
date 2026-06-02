
using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface ICurrentOrganizationRelationshipRepository : IRepository<CurrentOrganizationRelationship>
    {

    }
    public class CurrentOrganizationRelationshipRepository : RepositoryBaseCodeFirst<CurrentOrganizationRelationship>, ICurrentOrganizationRelationshipRepository
    {
        public CurrentOrganizationRelationshipRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
