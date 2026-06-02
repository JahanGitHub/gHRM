using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Payroll;

namespace gHRM.Data.Repository.payroll
{
    public interface IOvertimeRepository : IRepository<OvertimeConfiguration>
    {

    }

    public class OvertimeRepository : RepositoryBaseCodeFirst<OvertimeConfiguration>, IOvertimeRepository
    {
        public OvertimeRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }
    }
}

