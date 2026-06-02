using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{
    public interface IProfitDeclarationRepository : IRepository<ProfitDeclaration>
    {
        //IEnumerable<ProfitDeclaration> GetProfitDeclarationByYear(int declarationYear);
    }
    public class ProfitDeclarationRepository : RepositoryBaseCodeFirst<ProfitDeclaration>, IProfitDeclarationRepository
    {
        public ProfitDeclarationRepository(IDatabaseFactoryCodeFirst databaseFactory): base(databaseFactory)
        {
        }
        //public IEnumerable<ProfitDeclaration> GetProfitDeclarationByYear(int declarationYear)
        //{
        //    IQueryable<ProfitDeclaration> results = null;
        //    results = DataContext.ProfitDeclaration.Where(x => x.DeclarationYear == declarationYear);
        //    return results;
        //}
    }
}
