using System.Text;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;
using gHRM.Data.Utility;
using System;
using System.Threading.Tasks;
using gHRM.Core.Filters.Offices;
using System.Data.Entity;
using gHRM.Data.CodeFirstMigration.Apply;

namespace gHRM.Data.Repository
{
    public interface IAppliedPostRepository : IRepository<AppliedPost>
    {
    }
    public class AppliedPostRepository : RepositoryBaseCodeFirst<AppliedPost>, IAppliedPostRepository
    {
        public AppliedPostRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

