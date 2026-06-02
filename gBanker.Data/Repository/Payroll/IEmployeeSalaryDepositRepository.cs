using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IEmployeeSalaryDepositRepository : IRepository<EmployeeSalaryDeposit>
    {

    }
    public class EmployeeSalaryDepositRepository : RepositoryBaseCodeFirst<EmployeeSalaryDeposit>, IEmployeeSalaryDepositRepository
    {
        public EmployeeSalaryDepositRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }


    }
}
