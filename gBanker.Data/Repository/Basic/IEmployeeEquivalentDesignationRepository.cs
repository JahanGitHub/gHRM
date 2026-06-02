using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface IEmployeeEquivalentDesignationRepository : IRepository<EmployeeEquivalentDesignation>
    {

    }

    public class EmployeeEquivalentDesignationRepository : RepositoryBaseCodeFirst<EmployeeEquivalentDesignation>, IEmployeeEquivalentDesignationRepository
    {
        public EmployeeEquivalentDesignationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

