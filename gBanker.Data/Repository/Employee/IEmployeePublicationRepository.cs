
using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IEmployeePublicationRepository : IRepository<EmployeePublication>
    {

    }
    public class EmployeePublicationRepository : RepositoryBaseCodeFirst<EmployeePublication>, IEmployeePublicationRepository
    {
        public EmployeePublicationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
