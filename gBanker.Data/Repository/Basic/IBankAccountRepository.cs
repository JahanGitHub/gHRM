
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Basic;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository.Basic
{
    public interface IBankAccountRepository : IRepository<BankAccount>
    {

    }
    public class BankAccountRepository : RepositoryBaseCodeFirst<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
