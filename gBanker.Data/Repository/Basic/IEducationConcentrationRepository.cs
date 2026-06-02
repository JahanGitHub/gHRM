//using System;
//using System.Collections.Generic;

using System;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IEducationConcentrationRepository: IRepository<EducationConcentration>
    {        

    }
    public class EducationConcentrationRepository : RepositoryBaseCodeFirst<EducationConcentration>, IEducationConcentrationRepository
    {
        public EducationConcentrationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
