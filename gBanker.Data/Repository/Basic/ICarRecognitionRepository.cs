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
    public interface ICarRecognitionRepository : IRepository<CarRecognition>
    {

    }

    public class CarRecognitionRepository : RepositoryBaseCodeFirst<CarRecognition>, ICarRecognitionRepository
    {
        public CarRecognitionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}


