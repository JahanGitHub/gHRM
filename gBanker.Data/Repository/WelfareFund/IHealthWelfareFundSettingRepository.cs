using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.HealthWelfareFund;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using System.Collections.Generic;

namespace gHRM.Data.Repository.WelfareFund
{
    public interface IHealthWelfareFundSettingRepository : IRepository<HealthWelfareFundSetting>
    {

    }
    public class HealthWelfareFundSettingRepository : RepositoryBaseCodeFirst<HealthWelfareFundSetting>, IHealthWelfareFundSettingRepository
    {
        public HealthWelfareFundSettingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
