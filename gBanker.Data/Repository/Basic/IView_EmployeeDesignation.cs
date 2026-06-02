using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IView_EmployeeDesignationRepository : IRepository<View_EmployeeDesignation>
    {

    }
    public class View_EmployeeDesignationRepository : RepositoryBaseCodeFirst<View_EmployeeDesignation>, IView_EmployeeDesignationRepository
    {
        public View_EmployeeDesignationRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }


    }
}
