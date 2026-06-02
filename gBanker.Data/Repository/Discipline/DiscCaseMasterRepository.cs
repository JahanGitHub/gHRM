using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscCaseMasterRepository : IRepository<DiscCaseMaster>
    {

    }
    public class DiscCaseMasterRepository : RepositoryBaseCodeFirst<DiscCaseMaster>, IDiscCaseMasterRepository
    {
        public DiscCaseMasterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

    }
}
