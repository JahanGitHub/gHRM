using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{
    public interface IPFWithdrawanRepository : IRepository<PFWithdrawan>
    {
        //AccountChart GetAccountChartByAccountCode(string accountCode);
    }
    public class PFWithdrawanRepository : RepositoryBaseCodeFirst<PFWithdrawan>, IPFWithdrawanRepository
    {
        public PFWithdrawanRepository(IDatabaseFactoryCodeFirst databaseFactory): base(databaseFactory)
        {
        }

        //public AccountChart GetAccountChartByAccountCode(string accountCode)
        //{
        //    AccountChart results = null;
        //    results = DataContext.AccountChart.Where(x => x.AccountCode == accountCode).SingleOrDefault(); 
        //    return results;
        //}
    }
}
