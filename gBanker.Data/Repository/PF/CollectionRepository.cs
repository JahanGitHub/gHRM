using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{
    public interface ICollectionRepository : IRepository<Collection>
    {
        IEnumerable<Collection> GetCollectionByEmpId(string employeeId);
        Collection GetCollectionByCollId(long collectionId);
        IEnumerable<Collection> GetLoanCollectionByLoanId(long loanId);
     //   AccountChart GetAccountChartByAccountCode(string accountCode);
        IEnumerable<Collection> GetAllCollection();
    }
    //public  class CollectionRepository: PFRepositoryBaseCodeFirst<Collection>, ICollectionRepository
    public class CollectionRepository : RepositoryBaseCodeFirst<Collection>, ICollectionRepository
    {
        public CollectionRepository(IDatabaseFactoryCodeFirst databaseFactory): base(databaseFactory)
        {
        }

        public IEnumerable<Collection> GetCollectionByEmpId(string employeeId)
        {
            IQueryable<Collection> results = null;


            if (string.IsNullOrEmpty(employeeId))
                results = (from s in DataContext.Collection.Include("EmployeeConfiguration")
                           select s).AsQueryable();
                //results = (from s in DataContext.Collection.Include("EmployeeConfiguration").Include("CollectionType")
                //           select s).AsQueryable();
            else
            {
                long empId = Convert.ToInt64(employeeId);
                results = (from s in DataContext.Collection.Include("EmployeeConfiguration")
                           where s.EmployeeId == empId
                           select s).AsQueryable();
                //results = (from s in DataContext.Collection.Include("EmployeeConfiguration").Include("CollectionType")
                //           where s.EmployeeId == empId
                //           select s).AsQueryable();
            }
            return results;
        }

        public IEnumerable<Collection> GetLoanCollectionByLoanId(long loanId)
        {
            IQueryable<Collection> results = null;

            //results = (from s in DataContext.Collection
            //           select s).AsQueryable();

            results = DataContext.Collection.Where(x => x.LoanId == loanId).AsQueryable();
            return results;
        }

        public Collection GetCollectionByCollId(long collectionId)
        {
            Collection results = new Collection();
            results = (from s in DataContext.Collection.Include("EmployeeConfiguration")
                           where s.CollectionId == collectionId
                           select s).SingleOrDefault();
            //results = (from s in DataContext.Collection.Include("EmployeeConfiguration").Include("CollectionType")
            //           where s.CollectionId == collectionId
            //           select s).SingleOrDefault();
            return results;
        }
        
        //public AccountChart GetAccountChartByAccountCode(string accountCode)
        //{
        //    AccountChart results = null;
        //    results = DataContext.AccountChart.Where(x => x.AccountCode == accountCode).SingleOrDefault(); 
        //    return results;
        //}

        public IEnumerable<Collection> GetAllCollection()
        {
            var results =
                        (from c in DataContext.Collection.Include("EmployeeConfiguration").Include("TransactionCategory") 
                          select c
                         ).ToList();     
      
           
            //DataContext.Configuration.ProxyCreationEnabled = false;
            //var results =
            //             (
            //             (from c in DataContext.Collection.Include("EmployeeConfiguration").Include("TransactionCategory")
            //             join ec in DataContext.EmployeeConfiguration on c.EmployeeId equals ec.EmployeeId
            //             join tc in DataContext.TransactionCategory on c.CollectionTypeId equals tc.TransCategoryId
            //             //where sa.LocationId == 1
            //              select c)
            //             ).ToList();      
            return results;
        }
    }
}
