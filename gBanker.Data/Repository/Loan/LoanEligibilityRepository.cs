using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.Loan
{
    public interface ILoanEligibilityRepository : IRepository<LoanEligibility> { }
    public class LoanEligibilityRepository : RepositoryBaseCodeFirst<LoanEligibility>, ILoanEligibilityRepository
    {
        public LoanEligibilityRepository(IDatabaseFactoryCodeFirst databaseFactory) : base(databaseFactory) { }
        
    }
}
