using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using gHRM.Data.CodeFirstMigration.Discipline;

namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscMemorendumMasterRepository : IRepository<DiscMemorendumMaster>
    {

    }
    public class DiscMemorendumMasterRepository : RepositoryBaseCodeFirst<DiscMemorendumMaster>, IDiscMemorendumMasterRepository
    {
        public DiscMemorendumMasterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
