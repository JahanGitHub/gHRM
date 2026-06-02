using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{
    public interface ITransactionCategoryRepository : IRepository<TransactionCategory>
    {
        //AccountChart GetAccountChartByAccountCode(string accountCode);
        //IEnumerable<AccountChart> GetAccountChartByName(string accountName);
        //List<AccountChart> AddAccountChartAndParent(List<AccountChart> objAccountCharts);
        //AccountChart AddAccountChart(AccountChart objAccountChart);
    }
    public class TransactionCategoryRepository : RepositoryBaseCodeFirst<TransactionCategory>, ITransactionCategoryRepository
    {
        public TransactionCategoryRepository(IDatabaseFactoryCodeFirst databaseFactory): base(databaseFactory)
        {
            
        }

        //public AccountChart GetAccountChartByAccountCode(string accountCode)
        //{
        //    AccountChart results = null;
        //    results = DataContext.AccountChart.Where(x => x.AccountCode == accountCode).SingleOrDefault(); 
        //    return results;
        //}
        //public IEnumerable<AccountChart> GetAccountChartByName(string accountName)
        //{
        //    IQueryable<AccountChart> results = null;
        //    results = DataContext.AccountChart.Where(x => x.AccountName == accountName);
        //    return results;
        //}

        //public List<AccountChart> AddAccountChartAndParent(List<AccountChart> objAccountCharts)
        //{
        //    DataContext.AccountChart.Attach(objAccountCharts[0]);
        //    DataContext.Entry(objAccountCharts[0]).State = EntityState.Added;
        //    DataContext.AccountChart.Attach(objAccountCharts[1]);
        //    DataContext.Entry(objAccountCharts[1]).State = EntityState.Modified;
        //    DataContext.SaveChanges();

        //    //DataContext.Entry(objAccountCharts[0]).State = EntityState.Added;
        //    //DataContext.Entry(objAccountCharts[0]).State = EntityState.Modified;
        //    //DataContext.AccountChart.Add(objAccountCharts[0]);
        //    //DataContext.SaveChanges();
        //    return objAccountCharts;
        //}
        //public AccountChart AddAccountChart(AccountChart objAccountChart)
        //{
        //    DataContext.Entry(objAccountChart).State = EntityState.Added;
        //    DataContext.AccountChart.Add(objAccountChart);
        //    DataContext.SaveChanges();
        //    return objAccountChart;
        //}
    }
}
