using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IEmployeeReferencesRepository : IRepository<EmployeeReference>
    {

    }

    public class EmployeeReferencesRepository : RepositoryBaseCodeFirst<EmployeeReference>, IEmployeeReferencesRepository
    {
        public EmployeeReferencesRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
