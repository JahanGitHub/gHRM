using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public class UserRepository : RepositoryBaseCodeFirst<AspNetUser>, IUserRepository
    {
        public UserRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IUserRepository : IRepository<AspNetUser>
    {

    }
}
