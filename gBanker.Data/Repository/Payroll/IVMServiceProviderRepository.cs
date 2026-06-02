using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IVMServiceProviderRepository : IRepository<VMServiceProvider>
    {

    }
    public class VMServiceProviderRepository : RepositoryBaseCodeFirst<VMServiceProvider>, IVMServiceProviderRepository
    {
        public VMServiceProviderRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
