using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscCasePunishmentMasterRepository : IRepository<DiscCasePunishmentMaster>
    {

    }
    public class DiscCasePunishmentMasterRepository : RepositoryBaseCodeFirst<DiscCasePunishmentMaster>, IDiscCasePunishmentMasterRepository
    {
        public DiscCasePunishmentMasterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

    }
}
