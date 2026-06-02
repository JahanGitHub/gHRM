using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using gHRM.Data.CodeFirstMigration.Discipline;

namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscDealingOfficerRepository : IRepository<DiscDealingOfficer>
    {

    }
    public class DiscDealingOfficerRepository : RepositoryBaseCodeFirst<DiscDealingOfficer>, IDiscDealingOfficerRepository
    {
        public DiscDealingOfficerRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
