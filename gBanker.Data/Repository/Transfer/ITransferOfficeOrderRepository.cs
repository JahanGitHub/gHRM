
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository
{
    public interface ITransferOfficeOrderRepository : IRepository<TransferOfficeOrder>
    {

    }

    public class TransferOfficeOrderRepository : RepositoryBaseCodeFirst<TransferOfficeOrder>, ITransferOfficeOrderRepository
    {
        public TransferOfficeOrderRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

