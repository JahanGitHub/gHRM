using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface IEmployeeGradeListRepository : IRepository<EmployeeGradeList>
    {

    }
    public class EmployeeGradeListRepository : RepositoryBaseCodeFirst<EmployeeGradeList>, IEmployeeGradeListRepository
    {
        public EmployeeGradeListRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
