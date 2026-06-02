using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Basic;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface ICarRecognitionApprovalRepository : IRepository<CarRecognitionApproval>
    {

    }

    public class CarRecognitionApprovalRepository : RepositoryBaseCodeFirst<CarRecognitionApproval>, ICarRecognitionApprovalRepository
    {
        public CarRecognitionApprovalRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

