using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IEmployeeReportOptionRepository : IRepository<EmployeeReportOption>
    {

    }

    public class EmployeeReportOptionRepository : RepositoryBaseCodeFirst<EmployeeReportOption>, IEmployeeReportOptionRepository
    {
        public EmployeeReportOptionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

