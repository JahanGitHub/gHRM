using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IView_TimeKeepingDetailRepository : IRepository<View_TimeKeepingDetail>
    {

    }
    public class View_TimeKeepingDetailRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.View_TimeKeepingDetail>, IView_TimeKeepingDetailRepository
    {
        public View_TimeKeepingDetailRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
