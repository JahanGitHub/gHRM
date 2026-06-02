using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface ISalaryGenerationLogRepository : IRepository<SalaryGenerationLog>
    {

    }
    public class SalaryGenerationLogRepository : RepositoryBaseCodeFirst<SalaryGenerationLog>, ISalaryGenerationLogRepository
    {
        public SalaryGenerationLogRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }


    }
}
