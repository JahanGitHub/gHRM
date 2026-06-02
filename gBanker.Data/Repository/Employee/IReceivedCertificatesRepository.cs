//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace gHRM.Data.Repository
//{
//    interface IReceivedCertificatesRepository
//    {

//    }
//}

using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IReceivedCertificatesRepository : IRepository<ReceivedCertificates>
    {

    }
    public class ReceivedCertificatesRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.ReceivedCertificates>, IReceivedCertificatesRepository
    {
        public ReceivedCertificatesRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}

