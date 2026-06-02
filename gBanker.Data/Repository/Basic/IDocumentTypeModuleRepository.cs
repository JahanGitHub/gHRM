using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IDocumentTypeModuleRepository : IRepository<DocumentTypeModule>
    {

    }

    public class DocumentTypeModuleRepository : RepositoryBaseCodeFirst<DocumentTypeModule>, IDocumentTypeModuleRepository
    {
        public DocumentTypeModuleRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}


