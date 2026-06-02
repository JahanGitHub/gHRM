//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace gHRM.Data.Repository
//{
//    interface IEmployeeSupervisorRepository
//    {
//    }
//}
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IEmployeeSupervisorRepository : IRepository<EmployeeSupervisor>
    {

    }
    public class EmployeeSupervisorRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.EmployeeSupervisor>, IEmployeeSupervisorRepository
    {
        public EmployeeSupervisorRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
