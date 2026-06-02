using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IFeedbackRegisterRepository : IRepository<FeedbackRegister>
    {

    }
    public class FeedbackRegisterRepository : RepositoryBaseCodeFirst<FeedbackRegister>, IFeedbackRegisterRepository
    {
        public FeedbackRegisterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
