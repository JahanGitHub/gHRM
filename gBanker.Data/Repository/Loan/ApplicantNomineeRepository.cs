using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Loan
{
    public interface IApplicantNomineeRepository : IRepository<ApplicantNominee> { }
    public class ApplicantNomineeRepository : RepositoryBaseCodeFirst<ApplicantNominee>, IApplicantNomineeRepository
    {
        public ApplicantNomineeRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory) { }
        
    }
}
