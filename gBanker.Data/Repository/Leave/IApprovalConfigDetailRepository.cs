using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels;
using System.Collections.Generic;
using System.Linq;
namespace gHRM.Data.Repository
{
    public interface IApprovalConfigDetailRepository : IRepository<ApprovalConfigDetail>
    {
        List<ApprovalConfigDetail> AddApprovalConfigDetailList(List<ApprovalConfigDetail> objs);
    }
    public class ApprovalConfigDetailRepository : RepositoryBaseCodeFirst<gHRM.Data.CodeFirstMigration.ApprovalConfigDetail>, IApprovalConfigDetailRepository
    {
        public ApprovalConfigDetailRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public List<ApprovalConfigDetail> AddApprovalConfigDetailList(List<ApprovalConfigDetail> objs)
        {
            DataContext.ApprovalConfigDetail.AddRange(objs);
            DataContext.SaveChanges();
            return objs;
        }
      
    }
}
