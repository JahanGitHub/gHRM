using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IView_TimeKeepingRosterRepository : IRepository<View_TimeKeepingRoster>
    {

    }
    public class View_TimeKeepingRosterRepository : RepositoryBaseCodeFirst<View_TimeKeepingRoster>, IView_TimeKeepingRosterRepository
    {
        public View_TimeKeepingRosterRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }


    }
}

