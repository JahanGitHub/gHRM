using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IRoasterEmployeeScheduleRepository : IRepository<RoasterEmployeeSchedule>
    {

    }
    // public class PFRefundRepository : RepositoryBaseCodeFirst<PFRefund>, IPFRefundRepository
    public class RoasterEmployeeScheduleRepository : RepositoryBaseCodeFirst<RoasterEmployeeSchedule>, IRoasterEmployeeScheduleRepository
    {
        public RoasterEmployeeScheduleRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {


        }

    }// ENd of Class
}// End of Namespace
