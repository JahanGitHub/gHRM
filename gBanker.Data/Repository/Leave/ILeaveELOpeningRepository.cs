using System.Collections.Generic;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface ILeaveELOpeningRepository : IRepository<LeaveELOpening>
    {
        List<LeaveELOpening> AddELOpeningList(List<LeaveELOpening> objs);
    }

    public class LeaveELOpeningRepository : RepositoryBaseCodeFirst<LeaveELOpening>, ILeaveELOpeningRepository
    {
        public LeaveELOpeningRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }

        public List<LeaveELOpening> AddELOpeningList(List<LeaveELOpening> objs)
        {
            DataContext.LeaveELOpenings.AddRange(objs);
            DataContext.SaveChanges();
            return objs;
        }
    }
}
