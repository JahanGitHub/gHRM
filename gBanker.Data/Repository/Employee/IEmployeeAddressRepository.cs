using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IEmployeeAddressRepository : IRepository<EmployeeAddress>
    {

    }
    public class EmployeeAddressRepository : RepositoryBaseCodeFirst<EmployeeAddress>, IEmployeeAddressRepository
    {
        public EmployeeAddressRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }      
    }
}
