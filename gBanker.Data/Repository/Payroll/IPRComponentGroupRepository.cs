using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IPRComponentGroupRepository : IRepository<PRComponentGroup>
    {

    }

    public class PRComponentGroupRepository : RepositoryBaseCodeFirst<PRComponentGroup>, IPRComponentGroupRepository
    {
        public PRComponentGroupRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
