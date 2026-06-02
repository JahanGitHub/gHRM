using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{
  
    public interface IVw_AccChartRepository : IRepository<Vw_AccChart>
        {
        }


    public class Vw_AccChartRepository : RepositoryBaseCodeFirst<Vw_AccChart>, IVw_AccChartRepository
    {
        public Vw_AccChartRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {
            
        }
    }
}


