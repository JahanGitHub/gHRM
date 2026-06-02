//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;

namespace gHRM.Data.Repository
{
    public interface IFamilyRelationRepository : IRepository<FamilyRelation>
    {

    }
    public class FamilyRelationRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.FamilyRelation>, IFamilyRelationRepository
    {
        public FamilyRelationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
