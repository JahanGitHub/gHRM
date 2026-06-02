using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IEmployeePreviousWorkExperienceRepository : IRepository<EmployeePreviousWorkExperience>
    {

    }
    public class EmployeePreviousWorkExperienceRepository : RepositoryBaseCodeFirst<EmployeePreviousWorkExperience>, IEmployeePreviousWorkExperienceRepository
    {
        public EmployeePreviousWorkExperienceRepository(IDatabaseFactoryCodeFirst databaseFactory)  //check
            : base(databaseFactory)
        {

        }


    }
}
