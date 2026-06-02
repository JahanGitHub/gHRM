using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IAttHolidayTypeRepository : IRepository<AttHolidayType>
    {

    }
    // public class PFRefundRepository : RepositoryBaseCodeFirst<PFRefund>, IPFRefundRepository
    public class AttHolidayTypeRepository : RepositoryBaseCodeFirst<AttHolidayType>, IAttHolidayTypeRepository
    {
        public AttHolidayTypeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {


        }

    }//End of Class
}//End of Namespace
