using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository
{
    public interface IDocumentTypeRepository : IRepository<DocumentType>
    {

    }

    public class DocumentTypeRepository : RepositoryBaseCodeFirst<DocumentType>, IDocumentTypeRepository
    {
        public DocumentTypeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

