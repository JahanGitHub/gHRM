using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IGuarantorRelationshipRepository : IRepository<GuarantorRelationship>
    {

    }

    public class GuarantorRelationshipRepository : RepositoryBaseCodeFirst<GuarantorRelationship>, IGuarantorRelationshipRepository
    {
        public GuarantorRelationshipRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

