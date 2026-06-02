using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{
    public interface IOfficeSetupRepository : IRepository<OfficeSetup>
    {
        IEnumerable<OfficeSetup> GetOfficeSetupByName(string officeName);
    }
    public class OfficeSetupRepository : RepositoryBaseCodeFirst<OfficeSetup>, IOfficeSetupRepository
    {
        public OfficeSetupRepository(IDatabaseFactoryCodeFirst databaseFactory): base(databaseFactory)
        {
        }
        public IEnumerable<OfficeSetup> GetOfficeSetupByName(string officeName)
        {
            IQueryable<OfficeSetup> results = null;
            results = DataContext.OfficeSetup.Where(x => x.OfficeName == officeName);
            return results;
        }
    }
}
