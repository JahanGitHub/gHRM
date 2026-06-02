using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IView_EmployeeOfficeTimeExceptionRepository : IRepository<View_EmployeeOfficeTimeException>
    {

    }

    public class View_EmployeeOfficeTimeExceptionRepository : RepositoryBaseCodeFirst<View_EmployeeOfficeTimeException>, IView_EmployeeOfficeTimeExceptionRepository
    {
        public View_EmployeeOfficeTimeExceptionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

