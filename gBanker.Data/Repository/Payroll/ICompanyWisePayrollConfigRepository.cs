using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface ICompanyWisePayrollConfigRepository : IRepository<CompanyWisePayrollConfig>
    {

    }
    public class CompanyWisePayrollConfigRepository : RepositoryBaseCodeFirst<CompanyWisePayrollConfig>, ICompanyWisePayrollConfigRepository
    {
        public CompanyWisePayrollConfigRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
