using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using gHRM.Data.CodeFirstMigration.Discipline;

namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscEmbezzleInfoRepository : IRepository<DiscEmbezzleInfo>
    {

    }
    public class DiscEmbezzleInfoRepository : RepositoryBaseCodeFirst<DiscEmbezzleInfo>, IDiscEmbezzleInfoRepository
    {
        public DiscEmbezzleInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
