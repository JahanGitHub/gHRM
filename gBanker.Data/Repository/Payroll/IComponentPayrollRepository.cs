using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IComponentPayrollRepository : IRepository<ComponentPayroll>
    {

    }
    public class ComponentPayrollRepository : RepositoryBaseCodeFirst<ComponentPayroll>, IComponentPayrollRepository
    {
        public ComponentPayrollRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
