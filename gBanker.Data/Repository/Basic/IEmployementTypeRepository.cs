using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IEmployementTypeRepository : IRepository<EmployementType>
    {

    }

    public class EmployementTypeRepository : RepositoryBaseCodeFirst<EmployementType>, IEmployementTypeRepository
    {
        public EmployementTypeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

