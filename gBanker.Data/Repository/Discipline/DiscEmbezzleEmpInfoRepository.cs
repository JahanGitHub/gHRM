using gHRM.Data.CodeFirstMigration.Discipline;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;


namespace gHRM.Data.Repository.Discipline
{
    public interface IDiscEmbezzleEmpInfoRepository : IRepository<DiscEmbezzleEmpInfo>
    {

    }
    public class DiscEmbezzleEmpInfoRepository : RepositoryBaseCodeFirst<DiscEmbezzleEmpInfo>, IDiscEmbezzleEmpInfoRepository
    {
        public DiscEmbezzleEmpInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
