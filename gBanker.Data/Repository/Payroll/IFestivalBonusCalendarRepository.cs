
using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using gHRM.Data.CodeFirstMigration.Payroll;

namespace gHRM.Data.Repository.Payroll
{
    public interface IFestivalBonusCalendarRepository : IRepository<FestivalBonusCalendar>
    {

    }
    public class FestivalBonusCalendarRepository : RepositoryBaseCodeFirst<FestivalBonusCalendar>, IFestivalBonusCalendarRepository
    {
        public FestivalBonusCalendarRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
