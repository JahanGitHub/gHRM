using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IEmployeeOfficeVisitInformationRepository : IRepository<EmployeeOfficeVisitInformation>
    {

    }

    public class EmployeeOfficeVisitInformationRepository : RepositoryBaseCodeFirst<EmployeeOfficeVisitInformation>, IEmployeeOfficeVisitInformationRepository
    {
        public EmployeeOfficeVisitInformationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}


