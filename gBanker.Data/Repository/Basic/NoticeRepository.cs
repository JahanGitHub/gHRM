using gHRM.Data.CodeFirstMigration.Basic;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository.Basic
{
    public interface INoticeRepository : IRepository<Notice>
    {

    }
    public class NoticeRepository : RepositoryBaseCodeFirst<Notice>, INoticeRepository
    {
        public NoticeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
