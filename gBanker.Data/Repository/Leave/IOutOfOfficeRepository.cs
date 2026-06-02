using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;


namespace gHRM.Data.Repository
{
    public interface IOutOfOfficeRepository :  IRepository<OutOfOffice>
    {
       
    }

    public class OutOfOfficeRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.OutOfOffice>, IOutOfOfficeRepository
    {
        public OutOfOfficeRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }
    }

}
