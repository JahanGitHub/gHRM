using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IEmployeeLoanRegisterRepository : IRepository<EmployeeLoanRegister>
    {

    }
    public class EmployeeLoanRegisterRepository : RepositoryBaseCodeFirst<EmployeeLoanRegister>, IEmployeeLoanRegisterRepository
    {
        public EmployeeLoanRegisterRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }


    }
}
