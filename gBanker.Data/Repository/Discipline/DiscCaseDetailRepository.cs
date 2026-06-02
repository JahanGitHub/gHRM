using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscCaseDetailRepository : IRepository<DiscCaseDetail>
    {

    }
    public class DiscCaseDetailRepository : RepositoryBaseCodeFirst<DiscCaseDetail>, IDiscCaseDetailRepository
    {
        public DiscCaseDetailRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }

}
