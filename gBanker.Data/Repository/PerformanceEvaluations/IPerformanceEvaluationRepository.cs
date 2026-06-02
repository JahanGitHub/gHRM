using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PerformanceEvaluations;

namespace gHRM.Data.Repository.PerformanceEvaluations
{
    public interface IPerformanceEvaluationRepository : IRepository<PerformanceEvaluation>
    {

    }

    public class PerformanceEvaluationRepository : RepositoryBaseCodeFirst<PerformanceEvaluation>, IPerformanceEvaluationRepository
    {
        public PerformanceEvaluationRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory)
        {

        }
    }
}
