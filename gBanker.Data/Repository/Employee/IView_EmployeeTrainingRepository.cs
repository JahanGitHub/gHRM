using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IView_EmployeeTrainingRepository : IRepository<View_EmployeeTraining>
    {

    }
    public class View_EmployeeTrainingRepository : RepositoryBaseCodeFirst<View_EmployeeTraining>, IView_EmployeeTrainingRepository
    {
        public View_EmployeeTrainingRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }


    }
}

