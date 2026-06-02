using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.WelfareFund;
using System.Collections.Generic;

namespace gHRM.Data.Repository.WelfareFund
{
    public interface IStaffWelfareFundSettingRepository : IRepository<StaffWelfareFundSetting>
    {

    }
    public class StaffWelfareFundSettingRepository : RepositoryBaseCodeFirst<StaffWelfareFundSetting>, IStaffWelfareFundSettingRepository
    {
        public StaffWelfareFundSettingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }      
    }
}
