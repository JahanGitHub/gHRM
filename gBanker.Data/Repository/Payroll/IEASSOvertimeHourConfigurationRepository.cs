using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IEASSOvertimeHourConfigurationRepository : IRepository<EASSOvertimeHourConfiguration>
    {

    }
    public class EASSOvertimeHourConfigurationRepository : RepositoryBaseCodeFirst<EASSOvertimeHourConfiguration>, IEASSOvertimeHourConfigurationRepository
    {
        public EASSOvertimeHourConfigurationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
