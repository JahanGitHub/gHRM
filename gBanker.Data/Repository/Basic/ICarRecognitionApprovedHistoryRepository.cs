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
    public interface ICarRecognitionApprovedHistoryRepository : IRepository<CarRecognitionApprovedHistory>
    {

    }

    public class CarRecognitionApprovedHistoryRepository : RepositoryBaseCodeFirst<CarRecognitionApprovedHistory>, ICarRecognitionApprovedHistoryRepository
    {
        public CarRecognitionApprovedHistoryRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

