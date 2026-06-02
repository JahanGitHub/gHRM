using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IView_PRComponentConfigurationRepository : IRepository<View_PRComponentConfiguration>
    {

    }
    public class View_PRComponentConfigurationRepository : RepositoryBaseCodeFirst<View_PRComponentConfiguration>, IView_PRComponentConfigurationRepository
    {
        public View_PRComponentConfigurationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
