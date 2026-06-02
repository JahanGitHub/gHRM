using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface ICountryRepository : IRepository<Country>
    {        

    }
    public class CountryRepository : RepositoryBaseCodeFirst<Country>, ICountryRepository
    {
        public CountryRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
