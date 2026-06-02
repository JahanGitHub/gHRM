using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IPRDepositRepository : IRepository<PRDeposit>
    {

    }
    public class PRDepositRepository : RepositoryBaseCodeFirst<PRDeposit>, IPRDepositRepository
    {
        public PRDepositRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
