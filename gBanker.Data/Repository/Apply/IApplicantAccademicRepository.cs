using gHRM.Data.CodeFirstMigration.Apply;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;

namespace gHRM.Data.Repository.Apply
{
    public interface IApplicantAccademicRepository : IRepository<ApplicantAccademic>
    {
    }
    public class ApplicantAccademicRepository : RepositoryBaseCodeFirst<ApplicantAccademic>, IApplicantAccademicRepository
    {
        public ApplicantAccademicRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

    }
}
