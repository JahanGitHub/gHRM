using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.Basic;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository.Basic
{
    public interface IBankBranchRepository : IRepository<BankBranch>
    {

    }
    public class BankBranchRepository : RepositoryBaseCodeFirst<BankBranch>, IBankBranchRepository
    {
        public BankBranchRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}

