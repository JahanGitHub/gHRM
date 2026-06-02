using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IEmployeeTimeKeepingExceptionRepository : IRepository<EmployeeTimeKeepingException>
    {

    }

    public class EmployeeTimeKeepingExceptionRepository : RepositoryBaseCodeFirst<EmployeeTimeKeepingException>, IEmployeeTimeKeepingExceptionRepository
    {
        public EmployeeTimeKeepingExceptionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

