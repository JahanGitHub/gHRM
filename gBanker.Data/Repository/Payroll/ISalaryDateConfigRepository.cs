using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System.Collections.Generic;

namespace gHRM.Data.Repository.Payroll
{
    public interface ISalaryDateConfigRepository : IRepository<SalaryDateConfig>
    {

    }
    public class SalaryDateConfigRepository : RepositoryBaseCodeFirst<SalaryDateConfig>, ISalaryDateConfigRepository
    {
        public SalaryDateConfigRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
