using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IVMCarTypeRepository : IRepository<VMCarType>
    {

    }

    public class VMCarTypeRepository : RepositoryBaseCodeFirst<VMCarType>, IVMCarTypeRepository
    {
        public VMCarTypeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
