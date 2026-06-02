using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using System.Collections.Generic;

namespace gHRM.Data.Repository.WelfareFund
{
    public interface IStaffWelfareFundConfigurationRepository : IRepository<StaffWelfareFundConfiguration>
    {

    }
    public class StaffWelfareFundConfigurationRepository : RepositoryBaseCodeFirst<StaffWelfareFundConfiguration>, IStaffWelfareFundConfigurationRepository
    {
        public StaffWelfareFundConfigurationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
