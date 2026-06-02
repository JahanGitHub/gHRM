using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Loan
{
    public interface IApplicantInfoRepository : IRepository<ApplicantInfo> { }
    public class ApplicantInfoRepository : RepositoryBaseCodeFirst<ApplicantInfo>, IApplicantInfoRepository
    {
        public ApplicantInfoRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory) { }
        
    }
}
