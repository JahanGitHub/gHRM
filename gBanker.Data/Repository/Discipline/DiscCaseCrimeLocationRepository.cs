using System;
using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using gHRM.Data.CodeFirstMigration.Discipline;

namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscCaseCrimeLocationRepository : IRepository<DiscCaseCrimeLocation>
    {

    }
    public class DiscCaseCrimeLocationRepository : RepositoryBaseCodeFirst<DiscCaseCrimeLocation>, IDiscCaseCrimeLocationRepository
    {
        public DiscCaseCrimeLocationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

    }
}
