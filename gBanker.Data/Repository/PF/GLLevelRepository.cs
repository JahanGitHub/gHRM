using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{
    public interface IGLLevelRepository : IRepository<GLLevel>
    {
        IEnumerable<GLLevel> GetGLLevelByName(string glLevelName);
    }
   //public class GLLevelRepository: PFRepositoryBaseCodeFirst<GLLevel>, IGLLevelRepository
    public class GLLevelRepository : RepositoryBaseCodeFirst<GLLevel>, IGLLevelRepository
   {
        public GLLevelRepository(IDatabaseFactoryCodeFirst databaseFactory): base(databaseFactory)
        {
        }

        public IEnumerable<GLLevel> GetGLLevelByName(string glLevelName)
        {
            IQueryable<GLLevel> results = null;
            results = DataContext.GLLevel.Where(x => x.GLLevelName == glLevelName);
            return results;
        }
    
    }
}
