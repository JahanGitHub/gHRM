using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IEmployeeTrainingRepository : IRepository<EmployeeTraining>
    {

    }

    public class EmployeeTrainingRepository : RepositoryBaseCodeFirst<EmployeeTraining>, IEmployeeTrainingRepository
    {
        public EmployeeTrainingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

