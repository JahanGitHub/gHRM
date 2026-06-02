using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IEASSCompanyRepository : IRepository<EASSCompany>
    {

    }
    public class EASSCompanyRepository : RepositoryBaseCodeFirst<EASSCompany>, IEASSCompanyRepository
    {
        public EASSCompanyRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
