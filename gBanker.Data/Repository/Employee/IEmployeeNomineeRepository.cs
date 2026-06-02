using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface IEmployeeNomineeRepository : IRepository<EmployeeNominee>
    {


    }
    public class EmployeeNomineeRepository : RepositoryBaseCodeFirst<EmployeeNominee>, IEmployeeNomineeRepository
    {
        public EmployeeNomineeRepository(IDatabaseFactoryCodeFirst databaseFactory): base(databaseFactory)
        {

        }
    }
}