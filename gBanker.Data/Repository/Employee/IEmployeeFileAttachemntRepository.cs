
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IEmployeeFileAttachemntRepository : IRepository<EmployeeFileAttachemnt>
    {

    }
    public class EmployeeFileAttachemntRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.EmployeeFileAttachemnt>, IEmployeeFileAttachemntRepository
    {
        public EmployeeFileAttachemntRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        
    }
}
