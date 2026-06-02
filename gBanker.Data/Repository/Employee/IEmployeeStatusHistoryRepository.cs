using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;

namespace gHRM.Data.Repository
{
    public interface IEmployeeStatusHistoryRepository : IRepository<EmployeeStatusHistory>
    {
               
    }
    public class EmployeeStatusHistoryRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.EmployeeStatusHistory>, IEmployeeStatusHistoryRepository
    {
        public EmployeeStatusHistoryRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }       
    }
}
