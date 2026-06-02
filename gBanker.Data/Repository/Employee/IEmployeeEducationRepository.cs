using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;


namespace gHRM.Data.Repository
{
    public interface IEmployeeEducationRepository : IRepository<EmployeeEducation>
    {
        

    }
    public class EmployeeEducationRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.EmployeeEducation>, IEmployeeEducationRepository
    {
        public EmployeeEducationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
