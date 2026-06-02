using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using gHRM.Data.CodeFirstMigration.Discipline;

namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscCasePunishmentDetailRepository : IRepository<DiscCasePunishmentDetail>
    {

    }
    public class DiscCasePunishmentDetailRepository : RepositoryBaseCodeFirst<DiscCasePunishmentDetail>, IDiscCasePunishmentDetailRepository
    {
        public DiscCasePunishmentDetailRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

    }
}
