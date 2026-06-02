using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using gHRM.Data.CodeFirstMigration.Discipline;

namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscMemorendumDetailsRepository : IRepository<DiscMemorendumDetail>
    {

    }
    public class DiscMemorendumDetailsRepository : RepositoryBaseCodeFirst<DiscMemorendumDetail>, IDiscMemorendumDetailsRepository
    {
        public DiscMemorendumDetailsRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
