using gHRM.Data.CodeFirstMigration.Basic;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Basic
{
    public interface IBankNameRepository : IRepository<BankName>
    {

    }

    public class BankNameRepository : RepositoryBaseCodeFirst<BankName>, IBankNameRepository
    {
        public BankNameRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
