using gHRM.Data.CodeFirstMigration.eRecruit;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.eRecruitApplication
{
  
    public interface IeRecruitDegreeRepository : IRepository<EducationDegree>
    {
    }
    public class eRecruitDegreeRepository : RepositoryBaseCodeFirst<EducationDegree>, IeRecruitDegreeRepository
    {
        public eRecruitDegreeRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }


    }

}
