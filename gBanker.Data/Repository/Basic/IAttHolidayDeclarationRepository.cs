using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IAttHolidayDeclarationRepository : IRepository<AttHolidayDeclaration>
    {

    }
    // public class PFRefundRepository : RepositoryBaseCodeFirst<PFRefund>, IPFRefundRepository
    public class AttHolidayDeclarationRepository : RepositoryBaseCodeFirst<AttHolidayDeclaration>, IAttHolidayDeclarationRepository
    {
        public AttHolidayDeclarationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {


        }

    }//End of Class
}//End of Namespace
