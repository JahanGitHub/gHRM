using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{

    public interface IAccountChartRepository : IRepository<AccountChart>
    {

        AccountChart GetAccountChartByAccountCode(string accountCode);
        AccountChart GetAccountChartExceptThisAccountCode(string accountCode, string accountName);
        IEnumerable<AccountChart> GetAccountChartByName(string accountName);
        List<AccountChart> AddAccountChartAndParent(List<AccountChart> objAccountCharts);
        AccountChart AddAccountChart(AccountChart objAccountChart);
        IEnumerable<AccountChart> GetVoucherableAccountChart(string voucherType);
    }
    
    //public class AccountChartRepository : PFRepositoryBaseCodeFirst<AccountChart>, IAccountChartRepository
    public class AccountChartRepository : RepositoryBaseCodeFirst<AccountChart>, IAccountChartRepository
    {
        public AccountChartRepository(IDatabaseFactoryCodeFirst databaseFactory): base(databaseFactory)
        {
            
        }

        public AccountChart GetAccountChartByAccountCode(string accountCode)
        {
            AccountChart results = null;
            results = DataContext.AccountChart.Where(x => x.AccountCode == accountCode).SingleOrDefault(); 
            return results;
        }

        public AccountChart GetAccountChartExceptThisAccountCode(string accountCode, string accountName)
        {
            AccountChart results = null;
            results = DataContext.AccountChart.Where(x => x.AccountCode != accountCode).Where(x=>x.AccountName == accountName).SingleOrDefault();
            return results;
        }

        public IEnumerable<AccountChart> GetAccountChartByName(string accountName)
        {
            IQueryable<AccountChart> results = null;
            results = DataContext.AccountChart.Where(x => x.AccountName == accountName);
            return results;
        }

        public List<AccountChart> AddAccountChartAndParent(List<AccountChart> objAccountCharts)
        {
            DataContext.AccountChart.Attach(objAccountCharts[0]);
            DataContext.Entry(objAccountCharts[0]).State = EntityState.Added;
            DataContext.AccountChart.Attach(objAccountCharts[1]);
            DataContext.Entry(objAccountCharts[1]).State = EntityState.Modified;
            DataContext.SaveChanges();
            return objAccountCharts;
        }
        public AccountChart AddAccountChart(AccountChart objAccountChart)
        {
            DataContext.Entry(objAccountChart).State = EntityState.Added;
            DataContext.AccountChart.Add(objAccountChart);
            DataContext.SaveChanges();
            return objAccountChart;
        }
        public  IEnumerable<AccountChart> GetVoucherableAccountChart(string voucherType)
        {
            IEnumerable<AccountChart> objAccountCharts = null;
            objAccountCharts = DataContext.AccountChart.Where(x => x.IsDeleted == false && x.IsVoucher == true);
            return objAccountCharts;
        }
    }
}
