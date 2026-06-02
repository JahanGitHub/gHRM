using System;
using System.Linq;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IELEncashmentAuthorityRepository : IRepository<ELEncashmentAuthority>
    {
        bool IsEmployeeAuthorizedForEncashment(long EmployeeId);
    }
    public class ELEncashmentAuthorityRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.ELEncashmentAuthority>, IELEncashmentAuthorityRepository
    {
        public ELEncashmentAuthorityRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public bool IsEmployeeAuthorizedForEncashment(long EmployeeId)
        {
            return DataContext.ELEncashmentAuthority.Where(x => x.IsActive && x.EmployeeId == EmployeeId).Count() > 0;
        }
    }
}

