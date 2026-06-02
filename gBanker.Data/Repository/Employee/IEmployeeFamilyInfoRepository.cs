using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{

    public interface IEmployeeFamilyInfoRepository : IRepository<EmployeeFamilyInfo>
    {

    }

    public class EmployeeFamilyInfoRepository : RepositoryBaseCodeFirst<EmployeeFamilyInfo>, IEmployeeFamilyInfoRepository
    {
        public EmployeeFamilyInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }

}
