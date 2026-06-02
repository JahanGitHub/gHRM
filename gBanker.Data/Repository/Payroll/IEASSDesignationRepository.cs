using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Payroll
{
    public interface IEASSDesignationRepository : IRepository<EASSDesignation>
    {

    }
    public class EASSDesignationRepository : RepositoryBaseCodeFirst<EASSDesignation>, IEASSDesignationRepository
    {
        public EASSDesignationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
