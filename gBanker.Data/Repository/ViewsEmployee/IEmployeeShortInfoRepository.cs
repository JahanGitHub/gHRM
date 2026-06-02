using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using gHRM.Data.CodeFirstMigration.ViewsEmployee;

namespace gHRM.Data.Repository
{
    public interface IEmployeeShortInfoRepository : IRepository<EmployeeShortInfo>
    {


    }
    public class EmployeeShortInfoRepository : RepositoryBaseCodeFirst<EmployeeShortInfo>, IEmployeeShortInfoRepository
    {
        public EmployeeShortInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
