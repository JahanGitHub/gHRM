using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Loan
{
    public interface IApplicantInfoRepository2 : IRepository<ApplicantInfo2> { }
    public class ApplicantInfoRepository2 : RepositoryBaseCodeFirst<ApplicantInfo2>, IApplicantInfoRepository2
    {
        public ApplicantInfoRepository2(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory) { }
        
    }
}
