using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IView_EmployeeTimeKeepingExceptionRepository : IRepository<View_EmployeeTimeKeepingException>
    {

    }
    public class View_EmployeeTimeKeepingExceptionRepository : RepositoryBaseCodeFirst<View_EmployeeTimeKeepingException>, IView_EmployeeTimeKeepingExceptionRepository
    {
        public View_EmployeeTimeKeepingExceptionRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }


    }
}
