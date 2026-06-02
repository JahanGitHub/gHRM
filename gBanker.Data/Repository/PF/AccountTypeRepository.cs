using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{
    public interface IAccountTypeRepository : IRepository<AccountType>
    {
        IEnumerable<AccountType> GetAccountTypeByName(string accountType);
    }
   //public class AccountTypeRepository: PFRepositoryBaseCodeFirst<AccountType>, IAccountTypeRepository
    public class AccountTypeRepository : RepositoryBaseCodeFirst<AccountType>, IAccountTypeRepository
    {
       public AccountTypeRepository(IDatabaseFactoryCodeFirst databaseFactory): base(databaseFactory)
        {
        }

       public IEnumerable<AccountType> GetAccountTypeByName(string accountType)
       {
           IQueryable<AccountType> results = null;
           results = DataContext.AccountType.Where(x => x.AccountTypeName == accountType);
           return results;
       }
    }
}
