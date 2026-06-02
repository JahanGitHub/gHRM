
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IEmployeeOfficeTimeExceptionRepository : IRepository<EmployeeOfficeTimeException>
    {

    }
    public class EmployeeOfficeTimeExceptionRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.EmployeeOfficeTimeException>, IEmployeeOfficeTimeExceptionRepository
    {
        public EmployeeOfficeTimeExceptionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
