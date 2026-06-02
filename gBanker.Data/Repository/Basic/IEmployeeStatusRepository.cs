//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace gHRM.Data.Repository
//{
//    interface IEmployeeStatusRepository
//    {
//    }
//}
using System.Linq;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IEmployeeStatusRepository : IRepository<EmployeeStatus>
    {
    }
    public class EmployeeStatusRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.EmployeeStatus>, IEmployeeStatusRepository
    {
        public EmployeeStatusRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
